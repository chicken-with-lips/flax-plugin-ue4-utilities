using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionConstant3VectorConverter : GenericConverter<MaterialExpressionConstant3Vector>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionConstant3Vector;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionConstant3Vector unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindConstantNodeArchetype(5), Helper.EditorPosition(unrealNode));
            node.SetValue(0, new Vector3(unrealNode.Constant.X, unrealNode.Constant.Y, unrealNode.Constant.Z));

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
