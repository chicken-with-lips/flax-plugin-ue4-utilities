using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionVectorParameterConverter : GenericParameterConverter<MaterialExpressionVectorParameter>
    {
        public override uint NodeTypeId => 1;

        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionVectorParameter;
        }

        protected override SurfaceParameter CreateSurfaceParameter(MaterialExpressionVectorParameter parameterNode, MaterialConverter converter)
        {
            return converter.FindOrCreateParameter<Vector4>(parameterNode.ParameterName, (p) => {
                p.Value = new Vector4(
                    parameterNode.DefaultValue.X,
                    parameterNode.DefaultValue.Y,
                    parameterNode.DefaultValue.Z,
                    parameterNode.DefaultValue.A
                );
            });
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            var hasR = propertyBag.HasProperty("MaskR");
            var hasG = propertyBag.HasProperty("MaskG");
            var hasB = propertyBag.HasProperty("MaskB");
            var hasA = propertyBag.HasProperty("MaskA");

            if(hasR && hasG && hasB && hasA) {
                return 0;
            } else if(hasR && hasG && hasB) {
                // should this be only RGB instead?
                return 0;
            } else if(hasR) {
                return 2;
            } else if(hasG) {
                return 3;
            } else if(hasB) {
                return 4;
            } else if(hasA) {
                return 5;
            }

            return 0;
        }
    }
}
