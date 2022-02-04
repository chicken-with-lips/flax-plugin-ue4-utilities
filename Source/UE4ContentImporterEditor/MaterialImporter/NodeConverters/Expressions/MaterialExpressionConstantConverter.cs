using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionConstantConverter : GenericConverter<MaterialExpressionConstant>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionConstant;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionConstant unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindConstantNodeArchetype(3), Helper.EditorPosition(unrealNode));
            node.SetValue(0, unrealNode.R);

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
