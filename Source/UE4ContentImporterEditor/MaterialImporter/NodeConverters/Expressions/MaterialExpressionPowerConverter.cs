using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionPowerConverter : GenericConverter<MaterialExpressionPower>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionPower;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionPower unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMathNodeArchetype(23), Helper.EditorPosition(unrealNode));

            if(unrealNode.Exponent == null) {
                node.SetValue(1, unrealNode.ConstExponent);
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionPower unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Value, unrealNode.Name, 0);

            if(unrealNode.Exponent != null) {
                builder.Connect(unrealNode.Exponent, unrealNode.Name, 1);
            }
        }
    }
}
