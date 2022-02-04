using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionClampConverter : GenericConverter<MaterialExpressionClamp>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionClamp;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionClamp unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMathNodeArchetype(24), Helper.EditorPosition(unrealNode));
            node.SetValue(0, unrealNode.MinDefault);
            node.SetValue(1, unrealNode.MaxDefault);

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 3;
        }

        public override void CreateConnections(MaterialExpressionClamp unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Input, unrealNode.Name, 0);
            builder.Connect(unrealNode.Min, unrealNode.Name, 1);
            builder.Connect(unrealNode.Max, unrealNode.Name, 2);
        }
    }
}
