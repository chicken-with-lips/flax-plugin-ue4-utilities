using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionDDYConverter : GenericConverter<MaterialExpressionDDY>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionDDY;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionDDY unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(30), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionDDY unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Value, unrealNode.Name, 0);
        }
    }
}
