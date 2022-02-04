using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionConstant4VectorConverter : GenericConverter<MaterialExpressionConstant4Vector>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionConstant4Vector;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionConstant4Vector unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindConstantNodeArchetype(6), Helper.EditorPosition(unrealNode));
            node.SetValue(0, new Vector4(unrealNode.Constant.X, unrealNode.Constant.Y, unrealNode.Constant.Z, unrealNode.Constant.A));

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
