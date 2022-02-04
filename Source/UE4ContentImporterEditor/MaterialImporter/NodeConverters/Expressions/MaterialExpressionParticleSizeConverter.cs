using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionParticleSizeConverter : GenericConverter<MaterialExpressionParticleSize>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionParticleSize;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionParticleSize unrealNode)
        {
            return converter.SpawnNode(Helper.FindParticlesNodeArchetype(106), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
