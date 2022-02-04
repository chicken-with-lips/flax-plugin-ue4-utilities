using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionNormalizeConverter : GenericConverter<MaterialExpressionNormalize>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionNormalize;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionNormalize unrealNode)
        {
            return converter.SpawnNode(Helper.FindMathNodeArchetype(12), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionNormalize unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.VectorInput, unrealNode.Name, 0);
        }
    }
}
