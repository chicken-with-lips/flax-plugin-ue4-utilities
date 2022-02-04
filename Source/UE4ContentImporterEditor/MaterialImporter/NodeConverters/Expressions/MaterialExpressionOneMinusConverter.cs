using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionOneMinusConverter : GenericConverter<MaterialExpressionOneMinus>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionOneMinus;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionOneMinus unrealNode)
        {
            return converter.SpawnNode(Helper.FindMathNodeArchetype(28), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionOneMinus unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Input, unrealNode.Name, 0);
        }
    }
}
