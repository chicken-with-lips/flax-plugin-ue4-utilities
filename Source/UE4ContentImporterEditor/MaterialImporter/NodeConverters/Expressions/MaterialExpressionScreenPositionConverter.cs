using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionScreenPositionConverter : GenericConverter<MaterialExpressionScreenPosition>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionScreenPosition;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionScreenPosition unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(6), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return -1;
        }
    }
}
