using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = JollySamurai.UnrealEngine4.T3D.Material.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionPannerConverter : GenericConverter<MaterialExpressionPanner>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionPanner;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionPanner unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindToolNodeArchetype(6), Helper.EditorPosition(unrealNode));

            node.SetValue(0, unrealNode.FractionalPart);

            if (unrealNode.Speed == null && (unrealNode.SpeedX != 0 || unrealNode.SpeedY != 0)) {
                var speedNode = converter.SpawnNode(Helper.FindConstantNodeArchetype(4), Helper.EditorPosition(unrealNode));
                speedNode.SetValue(0, new Vector2(unrealNode.SpeedX, unrealNode.SpeedY));
                
                converter.Connect(
                    speedNode.GetBox(0),
                    node.GetBox(2)
                );
            }

            if (unrealNode.Coordinate == null && unrealNode.ConstCoordinate != 0) {
                var coordNode = converter.SpawnNode(Helper.FindConstantNodeArchetype(4), Helper.EditorPosition(unrealNode));
                coordNode.SetValue(0, new Vector2(unrealNode.ConstCoordinate, unrealNode.ConstCoordinate));
                
                converter.Connect(
                    coordNode.GetBox(0),
                    node.GetBox(0)
                );
            }

            return node;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 3;
        }

        public override void CreateConnections(MaterialExpressionPanner unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if (unrealNode.Coordinate != null) {
                builder.Connect(unrealNode.Coordinate, unrealNode.Name, 0);
            }

            if (unrealNode.Time != null) {
                builder.Connect(unrealNode.Time, unrealNode.Name, 1);
            }

            if (unrealNode.Speed != null) {
                builder.Connect(unrealNode.Speed, unrealNode.Name, 2);
            }
        }
    }
}
