using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionStaticSwitchParameterConverter : GenericParameterConverter<MaterialExpressionStaticSwitchParameter>
    {
        public override uint NodeTypeId => 1;

        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionStaticSwitchParameter;
        }

        protected override SurfaceParameter CreateSurfaceParameter(MaterialExpressionStaticSwitchParameter parameterNode, MaterialConverter converter)
        {
            return converter.FindOrCreateParameter<bool>(parameterNode.ParameterName, (p) => {
                p.Value = parameterNode.DefaultValue;
            });
        }

        protected override SurfaceNode CreateNodeForSurfaceParameter(SurfaceParameter surfaceParameter, MaterialConverter builder, MaterialExpressionStaticSwitchParameter unrealNode)
        {
            var propertyNode = base.CreateNodeForSurfaceParameter(surfaceParameter, builder, unrealNode);
            var branchNode = builder.SpawnNode(Helper.FindComparisonNodeArchetype(7), Helper.EditorPosition(unrealNode));

            builder.Connect(propertyNode.GetBox(0), branchNode.GetBox(0));

            return branchNode;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 3;
        }

        public override void CreateConnections(MaterialExpressionStaticSwitchParameter unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            builder.Connect(unrealNode.A, unrealNode.Name, 1);
            builder.Connect(unrealNode.B, unrealNode.Name, 2);
        }
    }
}
