using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionDepthFadeConverter : GenericConverter<MaterialExpressionDepthFade>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionDepthFade;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionDepthFade unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindMaterialNodeArchetype(23), Helper.EditorPosition(unrealNode));
            node.SetValue(0, unrealNode.FadeDistanceDefault);

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionDepthFade unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if(unrealNode.FadeDistance != null) {
                builder.Connect(unrealNode.FadeDistance, unrealNode.Name, 0);
            }
        }
    }
}
