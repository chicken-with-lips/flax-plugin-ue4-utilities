using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionDesaturationConverter : GenericConverter<MaterialExpressionDesaturation>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionDesaturation;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionDesaturation unrealNode)
        {
            return converter.SpawnNode(Helper.FindToolNodeArchetype(2), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionDesaturation unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Input, unrealNode.Name, 0);
            builder.Connect(unrealNode.Fraction, unrealNode.Name, 1);
        }
    }
}
