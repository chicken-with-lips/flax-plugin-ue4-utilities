using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionWorldPositionConverter : GenericConverter<MaterialExpressionWorldPosition>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionWorldPosition;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionWorldPosition unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(2), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
