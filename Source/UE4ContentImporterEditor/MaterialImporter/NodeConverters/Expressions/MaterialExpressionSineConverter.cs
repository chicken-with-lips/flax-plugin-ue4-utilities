using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionSineConverter : GenericConverter<MaterialExpressionSine>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionSine;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionSine unrealNode)
        {
            return converter.SpawnNode(Helper.FindMathNodeArchetype(15), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionSine unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            converter.Connect(unrealNode.Input, unrealNode.Name, 0);
        }
    }
}
