using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionTextureObjectConverter : GenericConverter<MaterialExpressionTextureObject>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionTextureObject;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionTextureObject unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindTextureNodeArchetype(1), Helper.EditorPosition(unrealNode));

            if (unrealNode.Texture != null) {
                // FIXME: don't use unresolved reference directly and don't guess the extension
                var assetItem = Helper.FindAsset(unrealNode.Texture.FileName);

                if (null != assetItem) {
                    node.SetValue(0, assetItem.ID);
                }
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }
    }
}
