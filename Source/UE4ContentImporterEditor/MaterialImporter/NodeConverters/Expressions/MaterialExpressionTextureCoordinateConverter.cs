using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionTextureCoordinateConverter : GenericConverter<MaterialExpressionTextureCoordinate>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionTextureCoordinate;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionTextureCoordinate unrealNode)
        {
            return converter.SpawnNode(Helper.FindTextureNodeArchetype(2), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
