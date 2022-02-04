using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionStaticSwitchConverter : GenericConverter<MaterialExpressionStaticSwitch>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionStaticSwitch;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionStaticSwitch unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindComparisonNodeArchetype(7), Helper.EditorPosition(unrealNode));
            node.SetValue(0, unrealNode.DefaultValue);
            node.SetValue(1, unrealNode.DefaultValue);

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 3;
        }

        public override void CreateConnections(MaterialExpressionStaticSwitch unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if (unrealNode.A != null) {
                builder.Connect(unrealNode.A, unrealNode.Name, 2);
            }

            if (unrealNode.B != null) {
                builder.Connect(unrealNode.B, unrealNode.Name, 1);
            }

            if (unrealNode.Value != null) {
                builder.Connect(unrealNode.Value, unrealNode.Name, 0);
            }
        }
    }
}
