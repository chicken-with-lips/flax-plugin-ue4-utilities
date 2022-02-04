using System;
using System.Collections.Generic;
using System.IO;
using FlaxEditor;
using FlaxEngine;
using UE4ContentImporterEditor;
using UE4ContentImporterEditor.MapImporter.Converters;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;
using Level = FlaxEngine.Level;
using Object = FlaxEngine.Object;

namespace JollySamurai.UnrealEngine4.Import.Map
{
    public class MapConverter
    {
        private readonly T3D.Map.Map _unrealMap;
        private readonly ResultSetBuilder _resultSetBuilder;

        private readonly Dictionary<string, SceneObject> _nodeLookupByUnrealNodeName;
        private readonly List<BaseNodeConverter> _converters;

        private MapConverter(T3D.Map.Map unrealMap, ResultSetBuilder resultSetBuilder)
        {
            _unrealMap = unrealMap;
            _resultSetBuilder = resultSetBuilder;
            _nodeLookupByUnrealNodeName = new Dictionary<string, SceneObject>();
            _converters = new List<BaseNodeConverter>();

            AddBuiltinConverters();
        }

        private void AddBuiltinConverters()
        {
            AddConverter(new DecalActorConverter());
            AddConverter(new ExponentialHeightFogConverter());
            AddConverter(new DirectionalLightActorConverter());
            AddConverter(new DirectionalLightComponentConverter());
            AddConverter(new PointLightActorConverter());
            AddConverter(new PostProcessingVolumeConverter());
            AddConverter(new SkyLightConverter());
            AddConverter(new SphereReflectionCaptureActorConverter());
            AddConverter(new SpotLightActorConverter());
            AddConverter(new StaticMeshActorConverter());

            AddConverter(new DecalComponentConverter());
            AddConverter(new ExponentialHeightFogComponentConverter());
            AddConverter(new PointLightComponentConverter());
            AddConverter(new SkyLightComponentConverter());
            AddConverter(new SphereReflectionCaptureComponentConverter());
            AddConverter(new SpotLightComponentConverter());
            AddConverter(new StaticMeshComponentConverter());
        }

        private void AddConverter(BaseNodeConverter nodeConverter)
        {
            _converters.Add(nodeConverter);
        }

        private void Run(string outputPath)
        {
            if (_unrealMap.LevelCount > 1) {
                _resultSetBuilder.AddProblem(ProblemSeverity.Fatal, outputPath, "Maps with more than one level is not supported");
            }

            var level = _unrealMap.Levels[0];

            if (Editor.Instance.ContentDatabase.Find(outputPath) != null) {
                Scripting.RunOnUpdate(() => {
                    Editor.Instance.ContentDatabase.Delete(Editor.Instance.ContentDatabase.Find(outputPath));
                });
            }

            var scene = new Scene() {
                StaticFlags = StaticFlags.FullyStatic
            };

            foreach (var node in level.Children) {
                ConvertUnrealNode(node, scene);
            }

            Scripting.RunOnUpdate(() => SaveScene(scene, outputPath)).Wait();
        }

        private void SaveScene(Scene scene, string outputPath)
        {
            var bytes = Level.SaveSceneToBytes(scene);

            Object.Destroy(ref scene);

            if (bytes == null || bytes.Length == 0) {
                throw new Exception("Failed to serialize scene");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.Read)) {
                fileStream.Write(bytes, 0, bytes.Length);
            }

            var directoryItem = Editor.Instance.ContentDatabase.Find(Path.GetDirectoryName(outputPath));

            if (null != directoryItem) {
                Editor.Instance.ContentDatabase.RefreshFolder(directoryItem, true);
            }
        }

        public void ConvertUnrealNode(Node unrealNode, Scene scene)
        {
            var converter = FindConverterForUnrealNode(unrealNode);
            converter?.Convert(unrealNode, this, null, null, scene);
        }

        public BaseNodeConverter FindConverterForUnrealNode(Node unrealNode)
        {
            foreach (var converter in _converters) {
                if (converter.CanConvert(unrealNode)) {
                    return converter;
                }
            }

            return null;
        }

        public static void FromUnrealMap(T3D.Map.Map map, string outputFile, ResultSetBuilder resultSetBuilder)
        {
            var converter = new MapConverter(map, resultSetBuilder);
            converter.Run(outputFile);
        }
    }
}
