using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = JollySamurai.UnrealEngine4.T3D.Material.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionFresnelConverter : GenericConverter<MaterialExpressionFresnel>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionFresnel;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionFresnel unrealNode)
        {
            return converter.SpawnNode(Helper.FindToolNodeArchetype(4), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 3;
        }

        public override void CreateConnections(MaterialExpressionFresnel unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if (unrealNode.ExponentIn != null) {
                builder.Connect(unrealNode.ExponentIn, unrealNode.Name, 0);
            }

            if (unrealNode.BaseReflectFractionIn != null) {
                builder.Connect(unrealNode.BaseReflectFractionIn, unrealNode.Name, 1);
            }

            if (unrealNode.Normal != null) {
                builder.Connect(unrealNode.Normal, unrealNode.Name, 2);
            }

            if (unrealNode.Power != null) {
                Debug.LogWarning("MaterialExpressionFresnelConverter.Power is not supported");
            }
        }
    }
}
