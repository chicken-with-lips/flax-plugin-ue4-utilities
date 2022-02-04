using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionLinearInterpolateConverter : GenericConverter<MaterialExpressionLinearInterpolate>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionLinearInterpolate;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionLinearInterpolate unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMathNodeArchetype(25), Helper.EditorPosition(unrealNode));
            node.SetValue(0, unrealNode.ConstA);
            node.SetValue(1, unrealNode.ConstB);
            node.SetValue(2, unrealNode.ConstAlpha);
            
            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 3;
        }

        public override void CreateConnections(MaterialExpressionLinearInterpolate unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.A, unrealNode.Name, 0);
            builder.Connect(unrealNode.B, unrealNode.Name, 1);
            builder.Connect(unrealNode.Alpha, unrealNode.Name, 2);
        }
    }
}
