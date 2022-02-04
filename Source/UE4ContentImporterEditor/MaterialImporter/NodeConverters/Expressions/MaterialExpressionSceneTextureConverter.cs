using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionSceneTextureConverter : GenericConverter<MaterialExpressionSceneTexture>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionSceneTexture;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionSceneTexture unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindTextureNodeArchetype(6), Helper.EditorPosition(unrealNode));
            node.SetValue(0, (int) Helper.ConvertUnrealMaterialSceneTextureId(unrealNode.SceneTextureId));

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionSceneTexture unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            converter.Connect(unrealNode.Coordinates, unrealNode.Name, 0);
        }
    }
}
