using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionPixelDepthConverter : GenericConverter<MaterialExpressionPixelDepth>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionPixelDepth;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionPixelDepth unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(15), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
