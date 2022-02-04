using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionDivideConverter : GenericConverter<MaterialExpressionDivide>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionDivide;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionDivide unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMathNodeArchetype(5), Helper.EditorPosition(unrealNode));

            if(unrealNode.A == null) {
                node.SetValue(0, unrealNode.ConstA);
            }

            if(unrealNode.B == null) {
                node.SetValue(1, unrealNode.ConstB);
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionDivide unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if(unrealNode.A != null) {
                builder.Connect(unrealNode.A, unrealNode.Name, 0);
            }

            if(unrealNode.B != null) {
                builder.Connect(unrealNode.B, unrealNode.Name, 1);
            }
        }
    }
}
