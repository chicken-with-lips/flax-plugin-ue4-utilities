using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionRotateAboutAxisConverter : GenericConverter<MaterialExpressionRotateAboutAxis>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionRotateAboutAxis;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionRotateAboutAxis unrealNode)
        {
            return converter.SpawnNode(Helper.FindMathNodeArchetype(37), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 4;
        }

        public override void CreateConnections(MaterialExpressionRotateAboutAxis unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.NormalizedRotationAxis, unrealNode.Name, 0);
            builder.Connect(unrealNode.RotationAngle, unrealNode.Name, 1);
            builder.Connect(unrealNode.PivotPoint, unrealNode.Name, 2);
            builder.Connect(unrealNode.Position, unrealNode.Name, 3);
        }
    }
}
