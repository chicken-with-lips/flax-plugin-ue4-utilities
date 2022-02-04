using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionDotProductConverter : GenericConverter<MaterialExpressionDotProduct>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionDotProduct;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionDotProduct unrealNode)
        {
            return converter.SpawnNode(Helper.FindMathNodeArchetype(20), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionDotProduct unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.A, unrealNode.Name, 0);
            builder.Connect(unrealNode.B, unrealNode.Name, 1);
        }
    }
}
