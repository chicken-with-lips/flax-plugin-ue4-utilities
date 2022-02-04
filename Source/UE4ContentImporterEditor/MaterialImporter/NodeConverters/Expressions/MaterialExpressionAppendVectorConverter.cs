using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionAppendVectorConverter : GenericConverter<MaterialExpressionAppendVector>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionAppendVector;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionAppendVector unrealNode)
        {
            return converter.SpawnNode(Helper.FindPackingNodeArchetype(100), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionAppendVector unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.A, unrealNode.Name, 0);
            builder.Connect(unrealNode.B, unrealNode.Name, 1);
        }
    }
}
