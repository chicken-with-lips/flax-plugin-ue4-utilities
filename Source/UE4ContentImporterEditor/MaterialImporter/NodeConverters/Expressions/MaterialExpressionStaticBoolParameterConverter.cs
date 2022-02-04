using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionStaticBoolParameterConverter : GenericParameterConverter<MaterialExpressionStaticBoolParameter>
    {
        public override uint NodeTypeId => 1;

        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionStaticBoolParameter;
        }

        protected override SurfaceParameter CreateSurfaceParameter(MaterialExpressionStaticBoolParameter parameterNode, MaterialConverter converter)
        {
            return converter.FindOrCreateParameter<bool>(parameterNode.ParameterName, (p) => {
                p.Value = parameterNode.DefaultValue;
            });
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 0;
        }
    }
}
