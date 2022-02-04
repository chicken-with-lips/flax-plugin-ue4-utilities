using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionTextureSampleConverter : GenericConverter<MaterialExpressionTextureSample>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionTextureSample;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionTextureSample unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindTextureNodeArchetype(9), Helper.EditorPosition(unrealNode));

            if (unrealNode.Texture != null) {
                var textureNode = converter.SpawnNode(Helper.FindTextureNodeArchetype(1), Helper.EditorPosition(unrealNode));
                converter.Connect(
                    textureNode.GetBox(6),
                    node.GetBox(0)
                );

                var assetItem = Helper.FindAsset(unrealNode.Texture.FileName);

                if (null != assetItem) {
                    textureNode.SetValue(0, assetItem.ID);
                }
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 4;
        }

        public override void CreateConnections(MaterialExpressionTextureSample unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if (unrealNode.TextureObject != null) {
                builder.Connect(unrealNode.TextureObject, unrealNode.Name, 0);
            }

            builder.Connect(unrealNode.Coordinates, unrealNode.Name, 1);
        }
    }
}
