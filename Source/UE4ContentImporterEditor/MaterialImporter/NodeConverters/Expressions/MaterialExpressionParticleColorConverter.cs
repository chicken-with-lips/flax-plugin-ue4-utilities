using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionParticleColorConverter : GenericConverter<MaterialExpressionParticleColor>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionParticleColor;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionParticleColor unrealNode)
        {
            var colorNode = converter.SpawnNode(Helper.FindParticlesNodeArchetype(104), Helper.EditorPosition(unrealNode));
            var unpackNode = converter.SpawnNode(Helper.FindPackingNodeArchetype(32), Helper.EditorPosition(unrealNode));

            converter.Connect(colorNode.GetBox(0), unpackNode.GetBox(0));

            return unpackNode;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return -1;
        }
    }
}
