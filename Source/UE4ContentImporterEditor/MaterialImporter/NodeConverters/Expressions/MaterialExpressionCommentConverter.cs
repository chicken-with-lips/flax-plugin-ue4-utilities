using System;
using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionCommentConverter : GenericConverter<MaterialExpressionComment>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionComment;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionComment unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindToolNodeArchetype(11), Helper.EditorPosition(unrealNode));
            node.SetValue(0, unrealNode.Text);
            node.SetValue(2, new Vector2(unrealNode.SizeX, unrealNode.SizeY));

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
