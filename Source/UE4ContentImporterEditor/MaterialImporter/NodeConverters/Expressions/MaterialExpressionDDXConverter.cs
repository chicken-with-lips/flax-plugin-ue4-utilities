using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionDDXConverter : GenericConverter<MaterialExpressionDDX>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionDDX;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionDDX unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(30), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionDDX unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Value, unrealNode.Name, 0);
        }
    }
}
