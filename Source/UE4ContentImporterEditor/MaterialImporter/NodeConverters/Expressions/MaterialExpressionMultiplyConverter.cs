using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionMultiplyConverter : GenericConverter<MaterialExpressionMultiply>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionMultiply;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionMultiply unrealNode)
        {
            return converter.SpawnNode(Helper.FindMathNodeArchetype(3), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionMultiply unrealNode, Material unrealMaterial, MaterialConverter builder)
        {

            if (unrealNode.B != null) {
                builder.Connect(unrealNode.B, unrealNode.Name, 1);
            }
            
            
            if (unrealNode.A != null) {
                builder.Connect(unrealNode.A, unrealNode.Name, 0);
            }
        }
    }
}
