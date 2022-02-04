using System.Collections.Generic;
using System.Linq;
using FlaxEditor;
using FlaxEditor.Scripting;
using FlaxEditor.Surface;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using UE4ContentImporterEditor.MaterialImporter.NodeConverters;
using UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions;
using UE4ContentImporterEditor.MaterialImporter.NodeConverters.Functions;
using UE4ContentImporterEditor.MaterialImporter.NodeConverters.Roots;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = JollySamurai.UnrealEngine4.T3D.Material.Material;
using Vector2 = FlaxEngine.Vector2;

namespace UE4ContentImporterEditor.MaterialImporter
{
    public class MaterialConverter
    {
        private readonly Material _unrealMaterial;
        private readonly string _outputPath;
        private readonly ResultSetBuilder _resultSetBuilder;

        private SurfaceOwner _surfaceOwner;
        private MaterialSurface _surface;

        private readonly Dictionary<string, SurfaceParameter> _propertyLookup;

        private readonly Dictionary<string, SurfaceNode> _nodeLookupByUnrealNodeName;
        private readonly List<BaseNodeConverter> _nodeConverters;

        private MaterialConverter(Material unrealMaterial, string outputPath, ResultSetBuilder resultSetBuilder)
        {
            _unrealMaterial = unrealMaterial;
            _outputPath = outputPath;
            _resultSetBuilder = resultSetBuilder;

            _propertyLookup = new Dictionary<string, SurfaceParameter>();

            _nodeLookupByUnrealNodeName = new Dictionary<string, SurfaceNode>();
            _nodeConverters = new List<BaseNodeConverter>();

            AddBuiltinConverters();
        }

        private void AddBuiltinConverters()
        {
            AddNodeConverter(new MaterialExpressionAbsConverter());
            AddNodeConverter(new MaterialExpressionAddConverter());
            AddNodeConverter(new MaterialExpressionAppendVectorConverter());
            AddNodeConverter(new MaterialExpressionClampConverter());
            AddNodeConverter(new MaterialExpressionCameraPositionWSConverter());
            AddNodeConverter(new MaterialExpressionCommentConverter());
            AddNodeConverter(new MaterialExpressionComponentMaskConverter());
            AddNodeConverter(new MaterialExpressionConstantConverter());
            AddNodeConverter(new MaterialExpressionConstant2VectorConverter());
            AddNodeConverter(new MaterialExpressionConstant3VectorConverter());
            AddNodeConverter(new MaterialExpressionConstant4VectorConverter());
            AddNodeConverter(new MaterialExpressionCrossProductConverter());
            AddNodeConverter(new MaterialExpressionDDXConverter());
            AddNodeConverter(new MaterialExpressionDDYConverter());
            AddNodeConverter(new MaterialExpressionDesaturationConverter());
            AddNodeConverter(new MaterialExpressionDepthFadeConverter());
            AddNodeConverter(new MaterialExpressionDistanceConverter());
            AddNodeConverter(new MaterialExpressionDivideConverter());
            AddNodeConverter(new MaterialExpressionDotProductConverter());
            AddNodeConverter(new MaterialExpressionFloorConverter());
            AddNodeConverter(new MaterialExpressionFracConverter());
            AddNodeConverter(new MaterialExpressionFresnelConverter());
            AddNodeConverter(new MaterialExpressionLinearInterpolateConverter());
            AddNodeConverter(new MaterialExpressionMultiplyConverter());
            AddNodeConverter(new MaterialExpressionNormalizeConverter());
            AddNodeConverter(new MaterialExpressionOneMinusConverter());
            AddNodeConverter(new MaterialExpressionPannerConverter());
            AddNodeConverter(new MaterialExpressionParticleColorConverter());
            AddNodeConverter(new MaterialExpressionParticleSizeConverter());
            AddNodeConverter(new MaterialExpressionPixelDepthConverter());
            AddNodeConverter(new MaterialExpressionPowerConverter());
            AddNodeConverter(new MaterialExpressionRotateAboutAxisConverter());
            AddNodeConverter(new MaterialExpressionScalarParameterConverter());
            AddNodeConverter(new MaterialExpressionSceneDepthConverter());
            AddNodeConverter(new MaterialExpressionSceneTextureConverter());
            AddNodeConverter(new MaterialExpressionScreenPositionConverter());
            AddNodeConverter(new MaterialExpressionSineConverter());
            AddNodeConverter(new MaterialExpressionStaticBoolParameterConverter());
            AddNodeConverter(new MaterialExpressionStaticSwitchParameterConverter());
            AddNodeConverter(new MaterialExpressionStaticSwitchConverter());
            AddNodeConverter(new MaterialExpressionSubtractConverter());
            AddNodeConverter(new MaterialExpressionTextureCoordinateConverter());
            AddNodeConverter(new MaterialExpressionTextureObjectConverter());
            AddNodeConverter(new MaterialExpressionTextureObjectParameterConverter());
            AddNodeConverter(new MaterialExpressionTextureSampleConverter());
            AddNodeConverter(new MaterialExpressionTextureSampleParameter2DConverter());
            AddNodeConverter(new MaterialExpressionTimeConverter());
            AddNodeConverter(new MaterialExpressionTransformConverter());
            AddNodeConverter(new MaterialExpressionVectorParameterConverter());
            AddNodeConverter(new MaterialExpressionVertexColorConverter());
            AddNodeConverter(new MaterialExpressionVertexNormalWSConverter());
            AddNodeConverter(new MaterialExpressionWorldPositionConverter());

            AddNodeConverter(new MaterialFunctionMatLayerBlendStandardConverter());

            AddNodeConverter(new LitRootConverter());
            AddNodeConverter(new UnlitRootConverter());
        }

        private void AddNodeConverter(BaseNodeConverter converter)
        {
            _nodeConverters.Add(converter);
        }

        private void Run()
        {
            var originalAsset = SetupSurface(out var workingCopyPath, out var workingCopy);

            if (originalAsset == null) {
                _resultSetBuilder.AddProblem(ProblemSeverity.Fatal, _outputPath, "There was a problem creating the asset");

                return;
            }

            ConvertUnrealNode(_unrealMaterial);

            IterateOverExpressionReferences(_unrealMaterial.EditorComments, ConvertUnrealNode);
            IterateOverExpressionReferences(_unrealMaterial.Expressions, ConvertUnrealNode);
            IterateOverExpressionReferences(_unrealMaterial.Expressions, ConnectUnrealNode);

            ConnectUnrealNode(_unrealMaterial);

            var info = workingCopy.Info;
            info.Domain = Helper.ConvertUnrealMaterialDomain(_unrealMaterial.MaterialDomain);
            info.ShadingModel = Helper.ConvertUnrealShadingModel(_unrealMaterial.ShadingModel);
            info.BlendMode = Helper.ConvertUnrealBlendMode(_unrealMaterial.BlendMode);
            info.DecalBlendingMode = Helper.ConvertUnrealDecalBlendMode(_unrealMaterial.DecalBlendMode);
            info.CullMode = _unrealMaterial.IsTwoSided ? CullMode.TwoSided : CullMode.Normal;
            info.OpacityThreshold = _unrealMaterial.OpacityMaskClipValue;

            SaveSurface(workingCopy, info, originalAsset, workingCopyPath);
        }

        private void SaveSurface(FlaxEngine.Material workingCopy, MaterialInfo info, Asset originalAsset, string workingCopyPath)
        {
            var contentItem = Editor.Instance.ContentDatabase.Find(_outputPath);
            
            if (contentItem == null) {
                return;
            }

            _surface.Save();
            workingCopy.SaveSurface(_surfaceOwner.SurfaceData, info);

            Helper.SaveToOriginalAsset(workingCopy, originalAsset.ID, workingCopyPath, _outputPath);

            originalAsset.Reload();

            if (originalAsset.WaitForLoaded()) {
                throw new System.Exception("Failed to load material: " + _outputPath);
            }

            contentItem.RefreshThumbnail();
        }

        private Asset SetupSurface(out string workingCopyPath, out FlaxEngine.Material workingCopy)
        {
            var originalAsset = Helper.CreateWorkingCopyOfAsset(_outputPath, Editor.NewAssetType.Material, out workingCopyPath, out workingCopy);

            if (workingCopy == null) {
                return null;
            }

            var surfaceData = workingCopy.LoadSurface(true);

            if (surfaceData.Length == 0) {
                throw new System.Exception("Failed to load surface");
            }

            _surfaceOwner = new SurfaceOwner {
                SurfaceData = surfaceData
            };

            _surface = new MaterialSurface(_surfaceOwner, null, null);

            return originalAsset;
        }

        public SurfaceNode SpawnNode(NodeArchetype nodeArchetype, Vector2 position)
        {
            GroupArchetype groupArchetype = null;

            foreach (var tmp in NodeFactory.DefaultGroups) {
                if (tmp.Archetypes.Contains(nodeArchetype)) {
                    groupArchetype = tmp;
                }
            }

            if (null == groupArchetype) {
                // FIXME:
                throw new System.Exception("Unknown group archetype: " + nodeArchetype.Description);
            }

            return _surface.Context.SpawnNode(groupArchetype, nodeArchetype, position);
        }

        private delegate void ExpressionReferenceCallback(MaterialNode node);

        private void IterateOverExpressionReferences(ExpressionReference[] references, ExpressionReferenceCallback callback)
        {
            if (null == references) {
                return;
            }

            foreach (var unresolvedExpression in references) {
                var childNode = _unrealMaterial.ResolveExpressionReference(unresolvedExpression);

                if (null == childNode) {
                    continue;
                }

                callback(childNode);
            }
        }

        public void ConvertUnrealNode(MaterialNode unrealNode)
        {
            var converter = FindConverterForUnrealNode(unrealNode);

            if (null == converter) {
                if (unrealNode is MaterialExpressionMaterialFunctionCall functionCallNode) {
                    _resultSetBuilder.AddProblem(ProblemSeverity.Warning, _outputPath, string.Format("Converter not found for {0} ({1})", unrealNode.GetType().FullName, functionCallNode.MaterialFunction.NodeName));
                } else {
                    _resultSetBuilder.AddProblem(ProblemSeverity.Warning, _outputPath, "Converter not found for " + unrealNode.GetType().FullName);
                }
            } else {
                converter.Convert(unrealNode, this);
            }
        }

        public void ConnectUnrealNode(MaterialNode unrealNode)
        {
            var converter = FindConverterForUnrealNode(unrealNode);
            converter?.CreateConnections(unrealNode, _unrealMaterial, this);
        }

        public BaseNodeConverter FindConverterForUnrealNode(MaterialNode unrealNode)
        {
            foreach (var converter in _nodeConverters) {
                if (converter.CanConvert(unrealNode)) {
                    return converter;
                }
            }

            return null;
        }

        public SurfaceNode FindNodeByUnrealName(string unrealName)
        {
            if (string.IsNullOrEmpty(unrealName)) {
                return null;
            }

            if (_nodeLookupByUnrealNodeName.ContainsKey(unrealName)) {
                return _nodeLookupByUnrealNodeName[unrealName];
            }

            return null;
        }

        public void Connect(Box from, Box to)
        {
            if (from != null && to != null) {
                from.CreateConnection(to);
            }
        }

        public void Connect(ParsedPropertyBag propertyBag, string toUnrealNodeName, int toBoxId)
        {
            if (null == propertyBag) {
                return;
            }

            if (! propertyBag.HasProperty("Expression")) {
                return;
            }

            var expressionValue = propertyBag.FindPropertyValue("Expression");
            var expression = ValueUtil.ParseExpressionReference(expressionValue);

            Connect(expression.NodeName, toUnrealNodeName, toBoxId, propertyBag);
        }

        public void Connect(ExpressionReference expression, string toUnrealNodeName, int toBoxId)
        {
            if (null == expression) {
                return;
            }

            Connect(expression.NodeName, toUnrealNodeName, toBoxId, ParsedPropertyBag.Empty);
        }

        public void Connect(string fromUnrealNodeName, string toUnrealNodeName, int toBoxId, ParsedPropertyBag propertyBag)
        {
            var fromNode = FindNodeByUnrealName(fromUnrealNodeName);
            var toNode = FindNodeByUnrealName(toUnrealNodeName);
            var fromBoxId = FindBoxId(fromUnrealNodeName, toUnrealNodeName, toBoxId, propertyBag);

            if (fromNode != null && toNode != null && fromBoxId != -1) {
                var fromBox = fromNode.GetBox(fromBoxId);
                var toBox = toNode.GetBox(toBoxId);

                Connect(fromBox, toBox);
            }
        }

        public delegate void ConfigureShaderPropertyDelegate(SurfaceParameter parameter);

        public SurfaceParameter FindOrCreateParameter<T>(string name, ConfigureShaderPropertyDelegate configureShaderProperty)
        {
            SurfaceParameter parameter;

            if (! _propertyLookup.ContainsKey(name)) {
                parameter = SurfaceParameter.Create(new ScriptType(typeof(T)), name);

                _surface.Context.Parameters.Add(parameter);

                configureShaderProperty(parameter);

                _propertyLookup.Add(name, parameter);
            } else {
                parameter = _propertyLookup[name];
            }

            return parameter;
        }

        public int FindBoxId(string fromUnrealName, string toUnrealName, int toBoxId, ParsedPropertyBag propertyBag)
        {
            var fromUnrealNode = _unrealMaterial.FindChildByName(fromUnrealName) as MaterialNode;
            var fromNode = FindNodeByUnrealName(fromUnrealName);
            var toNode = FindNodeByUnrealName(toUnrealName);

            if (fromUnrealNode == null || fromNode == null || toNode == null) {
                return -1;
            }

            foreach (var converter in _nodeConverters) {
                if (converter.CanConvert(fromUnrealNode)) {
                    return converter.GetConnectionBoxId(fromNode, toNode, toBoxId, propertyBag);
                }
            }

            return -1;
        }

        public Box FindBox(string fromUnrealName, string toUnrealName, int toBoxId, ParsedPropertyBag propertyBag)
        {
            var boxId = FindBoxId(fromUnrealName, toUnrealName, toBoxId, propertyBag);
            var fromNode = FindNodeByUnrealName(fromUnrealName);

            if (boxId == -1) {
                return default;
            }

            return fromNode.GetBox(boxId);
        }

        public static void FromUnrealMaterial(Material material, string outputPath, ResultSetBuilder resultSetBuilder)
        {
            var converter = new MaterialConverter(material, outputPath, resultSetBuilder);
            converter.Run();
        }

        public void TrackNode(SurfaceNode node, Node unrealNode)
        {
            _nodeLookupByUnrealNodeName.Add(unrealNode.Name, node);
        }

        private class SurfaceOwner : IVisjectSurfaceOwner
        {
            public string SurfaceName => "Xerg";

            public byte[] SurfaceData { get; set; }
            public Undo Undo => null;

            public void OnContextCreated(VisjectSurfaceContext context)
            {
            }

            public void OnSurfaceEditedChanged()
            {
            }

            public void OnSurfaceGraphEdited()
            {
            }

            public void OnSurfaceClose()
            {
            }
        }
    }
}
