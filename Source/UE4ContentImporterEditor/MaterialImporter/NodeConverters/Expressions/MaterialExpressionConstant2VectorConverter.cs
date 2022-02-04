using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionConstant2VectorConverter : GenericConverter<MaterialExpressionConstant2Vector>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionConstant2Vector;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionConstant2Vector unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindConstantNodeArchetype(4), Helper.EditorPosition(unrealNode));
            node.SetValue(0, new Vector2(unrealNode.R, unrealNode.G));

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
