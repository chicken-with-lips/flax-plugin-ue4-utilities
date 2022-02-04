using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = JollySamurai.UnrealEngine4.T3D.Material.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionTextureSampleParameter2DConverter : GenericParameterConverter<MaterialExpressionTextureSampleParameter2D>
    {
        public override uint NodeTypeId => 1;

        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionTextureSampleParameter2D;
        }

        protected override SurfaceParameter CreateSurfaceParameter(MaterialExpressionTextureSampleParameter2D parameterNode, MaterialConverter converter)
        {
            return converter.FindOrCreateParameter<Texture>(parameterNode.ParameterName, (p) => {
                // FIXME: don't use unresolved reference directly and don't guess the extension
                var assetItem = Helper.FindAsset(parameterNode.Texture.FileName);

                if (null != assetItem) {
                    p.Value = assetItem.ID;
                }
            });
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            var hasR = propertyBag.HasProperty("MaskR");
            var hasG = propertyBag.HasProperty("MaskG");
            var hasB = propertyBag.HasProperty("MaskB");
            var hasA = propertyBag.HasProperty("MaskA");
            
            if(hasR && hasG && hasB && hasA) {
                return 1;
            } else if(hasR && hasG && hasB) {
                // should this be only RGB instead?
                return 1;
            } else if(hasR) {
                return 2;
            } else if(hasG) {
                return 3;
            } else if(hasB) {
                return 4;
            } else if(hasA) {
                return 5;
            }
            
            return 1;
        }

        public override void CreateConnections(MaterialExpressionTextureSampleParameter2D unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            converter.Connect(unrealNode.Coordinates, unrealNode.Name, 0);
        }
    }
}
