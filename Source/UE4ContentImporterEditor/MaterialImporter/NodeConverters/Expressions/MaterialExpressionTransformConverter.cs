using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionTransformConverter : GenericConverter<MaterialExpressionTransform>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionTransform;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionTransform unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMathNodeArchetype(30), Helper.EditorPosition(unrealNode));
            node.SetValue(0, (int) Helper.ConvertUnrealMaterialVectorCoordTransformSource(unrealNode.TransformSourceType));
            node.SetValue(0, (int) Helper.ConvertUnrealMaterialVectorCoordTransform(unrealNode.TransformType));

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionTransform unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            converter.Connect(unrealNode.Input, unrealNode.Name, 0);
        }
    }
}
