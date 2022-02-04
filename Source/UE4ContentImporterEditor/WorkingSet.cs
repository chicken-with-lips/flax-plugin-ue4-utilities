using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FlaxEngine;
using UE4ContentImporterEditor.MaterialImporter;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Exception;
using JollySamurai.UnrealEngine4.T3D.Map;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.MaterialInstance;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = FlaxEngine.Material;

namespace UE4ContentImporterEditor
{
    public class WorkingSet
    {
        public event ProgressStageChangeDelegate StageChange;
        public event ProgressStepDelegate Step;

        private readonly string[] _selection;
        private readonly string _unrealContentRootDirectory;
        private readonly string _outputDirectory;
        private readonly bool _shouldConvertMaterials;
        private readonly bool _shouldConvertMaterialInstances;
        private readonly bool _shouldConvertMaps;

        private readonly ConcurrentBag<ParsedDocument> _materialList = new ConcurrentBag<ParsedDocument>();
        private readonly ConcurrentBag<ParsedDocument> _materialInstanceList = new ConcurrentBag<ParsedDocument>();
        private readonly ConcurrentBag<ParsedDocument> _mapList = new ConcurrentBag<ParsedDocument>();

        private readonly Mutex _mutex = new Mutex();
        private readonly ResultSetBuilder _resultSetBuilder = new ResultSetBuilder();

        public WorkingSet(string[] selection, string unrealContentRootDirectory, string outputDirectory, bool shouldConvertMaterials, bool shouldConvertMaterialInstances, bool shouldConvertMaps)
        {
            _selection = selection;
            _unrealContentRootDirectory = unrealContentRootDirectory;
            _outputDirectory = outputDirectory;
            _shouldConvertMaterials = shouldConvertMaterials;
            _shouldConvertMaterialInstances = shouldConvertMaterialInstances;
            _shouldConvertMaps = shouldConvertMaps;
        }

        public void Execute(Action<ResultSet> onCompleted)
        {
            RunTasks(new ConcurrentBag<string>(_selection), Task_ParseDocument, "Parsing documents").OnCompleted(
            () => RunTasks(_materialList, Task_CreateMaterial, "Creating materials").OnCompleted(
                () => RunTasks(_materialInstanceList, Task_CreateMaterialInstance, "Creating material instances").OnCompleted(
                    () => RunTasks(_mapList, Task_CreateScene, "Creating scenes").OnCompleted(
                       () => onCompleted(_resultSetBuilder.ToResultSet())
                       )
                    )
                )
            );
        }

        private TaskAwaiter RunTasks<T>(ConcurrentBag<T> documents, Action<T> task, string stageName)
        {
            RaiseProgressStageChangeEvent(stageName, documents.Count);

            return Task.Run(() => {
                foreach (var document in documents) {
                    task(document);
                }
            }).GetAwaiter();

            return Task.WhenAll(new Task[] {}).GetAwaiter();

            // FIXME: re-enable multi-threading wen stability issues are resolved.
            // var tasks = documents.Select(f => Task.Run(() => task(f))).ToArray();

            // return Task.WhenAll(tasks).GetAwaiter();
        }

        private void Task_ParseDocument(string file)
        {
            RaiseProgressStepEvent();

            try {
                using (StreamReader reader = new StreamReader(file)) {
                    var content = reader.ReadToEnd();

                    var document = ParsedDocument.From(file, content);
                    var classValue = document.RootNode.FindAttributeValue("Class");

                    if (document.RootNode.SectionType == "Object") {
                        if(classValue == "/Script/Engine.Material" && _shouldConvertMaterials) {
                            _materialList.Add(document);
                        } else if (classValue == "/Script/Engine.MaterialInstanceConstant" && _shouldConvertMaterialInstances) {
                            _materialInstanceList.Add(document);
                        }
                    } else if (document.RootNode.SectionType == "Map" && _shouldConvertMaps) {
                        _mapList.Add(document);
                    }
                }
            } catch (ParserException ex) {
                _resultSetBuilder.AddProblem(ProblemSeverity.Fatal, file, ex.Message);
            }
        }

        private void Task_CreateMaterial(ParsedDocument document)
        {
            RaiseProgressStepEvent();

            try {
                var processor = new MaterialDocumentProcessor();
                var result = processor.Convert(document);
                var material = result.RootNode;

                _resultSetBuilder.AddProcessorProblems(result.Problems, document.FileName);

                if (material == null) {
                    return;
                }

                var outputPath = MakeOutputPath(document.FileName);

                MaterialConverter.FromUnrealMaterial(material, outputPath, _resultSetBuilder);
            } catch (ParserException ex) {
                _resultSetBuilder.AddProblem(ProblemSeverity.Fatal, document.FileName, ex.ToString());
            }
        }

        private void Task_CreateMaterialInstance(ParsedDocument instanceDocument)
        {
            RaiseProgressStepEvent();

            // FIXME: it seems the engine doesn't like multiple loads for the same asset happening in multiple threads.
            // we use a mutex as it isn't super important to us that this is fast.
            _mutex.WaitOne();

            var parentMaterialName = GetUnrealMaterialNameForInstanceDocument(instanceDocument);
            var parentMaterialPath = MakeOutputPath(parentMaterialName, true);

            var parentMaterialAsset = Content.LoadAsync(parentMaterialPath) as Material;
            parentMaterialAsset?.WaitForLoaded();

            if (null == parentMaterialAsset) {
                _resultSetBuilder.AddProblem(ProblemSeverity.Fatal, instanceDocument.FileName, $"Parent material \"{GetUnrealMaterialNameForInstanceDocument(instanceDocument)}\" not found");
                _mutex.ReleaseMutex();

                return;
            }

            _mutex.ReleaseMutex();

            var processor = new MaterialInstanceDocumentProcessor();
            var result = processor.Convert(instanceDocument);
            var materialInstance = result.RootNode;

            _resultSetBuilder.AddProcessorProblems(result.Problems, instanceDocument.FileName);

            if (materialInstance == null) {
                return;
            }

            var outputPath = MakeOutputPath(instanceDocument.FileName);

            MaterialInstanceConverter.FromUnrealMaterialInstance(materialInstance, parentMaterialAsset, outputPath);
        }

        private void Task_CreateScene(ParsedDocument document)
        {
            RaiseProgressStepEvent();

            var processor = new MapDocumentProcessor();
            var result = processor.Convert(document);
            var map = result.RootNode;

            _resultSetBuilder.AddProcessorProblems(result.Problems, document.FileName);

            if (map == null) {
                return;
            }

            var outputPath = MakeOutputPath(document.FileName, extension: "scene");

            MapConverter.FromUnrealMap(map, outputPath, _resultSetBuilder);
        }

        private string GetUnrealMaterialNameForInstanceDocument(ParsedDocument document)
        {
            var parentValue = document.RootNode.FindPropertyValue("Parent");
            var parentExpr = ValueUtil.ParseExpressionReference(parentValue);

            // TODO: check on filesystem to see if the parent has already been converted

            return parentExpr.NodeName;
        }

        private string MakeRelativePath(string input, string path)
        {
            var relativePath = path.Substring(_unrealContentRootDirectory.Length);
            relativePath = relativePath.Replace('\\', '/');

            if (relativePath.StartsWith("/")) {
                relativePath = relativePath.Substring(1);
            }

            return relativePath;
        }

        private string MakeOutputPath(string file, bool isRelative = false, string extension = "flax")
        {
            var relativePath = isRelative ? file : MakeRelativePath(_unrealContentRootDirectory, file);
            relativePath = Path.ChangeExtension(relativePath, extension);

            if (relativePath.StartsWith("/")) {
                relativePath = relativePath.Substring(1);
            }
            
            return Path.Combine(_outputDirectory, relativePath);
        }

        private void RaiseProgressStageChangeEvent(string stage, int itemCount)
        {
            StageChange?.Invoke(new ProgressStageChangeEventArgs(stage, itemCount));
        }

        private void RaiseProgressStepEvent()
        {
            Step?.Invoke(new ProgressStepEventArgs());
        }
    }

    public delegate void ProgressStageChangeDelegate(ProgressStageChangeEventArgs args);
    public delegate void ProgressStepDelegate(ProgressStepEventArgs args);

    public class ProgressStageChangeEventArgs : EventArgs
    {
        public string Stage { get; }
        public int ItemCount { get; }

        public ProgressStageChangeEventArgs(string stage, int itemCount)
        {
            Stage = stage;
            ItemCount = itemCount;
        }
    }

    public class ProgressStepEventArgs : EventArgs
    {
    }
}
