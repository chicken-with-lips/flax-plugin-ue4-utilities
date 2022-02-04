using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionCameraPositionWSConverter : GenericConverter<MaterialExpressionCameraPositionWS>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionCameraPositionWS;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionCameraPositionWS unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(18), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
