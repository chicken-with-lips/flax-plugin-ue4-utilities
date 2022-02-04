using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = JollySamurai.UnrealEngine4.T3D.Material.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionSceneDepthConverter : GenericConverter<MaterialExpressionSceneDepth>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionSceneDepth;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionSceneDepth unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindTextureNodeArchetype(8), Helper.EditorPosition(unrealNode));

            if (unrealNode.Input == null) {
                var inputNode = converter.SpawnNode(Helper.FindConstantNodeArchetype(4), Helper.EditorPosition(unrealNode));
                inputNode.SetValue(0, new Vector2(unrealNode.ConstInput.X, unrealNode.ConstInput.Y));

                converter.Connect(inputNode.GetBox(0), node.GetBox(0));
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }

        public override void CreateConnections(MaterialExpressionSceneDepth unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.Input, unrealNode.Name, 0);
        }
    }
}
