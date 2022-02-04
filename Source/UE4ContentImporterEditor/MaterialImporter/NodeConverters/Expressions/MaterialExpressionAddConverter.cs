using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionAddConverter : GenericConverter<MaterialExpressionAdd>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionAdd;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionAdd unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMathNodeArchetype(1), Helper.EditorPosition(unrealNode));

            if (unrealNode.A == null) {
                node.SetValue(0, unrealNode.ConstA);
            }

            if (unrealNode.B == null) {
                node.SetValue(1, unrealNode.ConstB);
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionAdd unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.A, unrealNode.Name, 0);
            builder.Connect(unrealNode.B, unrealNode.Name, 1);
        }
    }
}
