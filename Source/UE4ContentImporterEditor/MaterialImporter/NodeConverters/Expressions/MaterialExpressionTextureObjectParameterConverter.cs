using System;
using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionTextureObjectParameterConverter : GenericParameterConverter<MaterialExpressionTextureObjectParameter>
    {
        public override uint NodeTypeId => 1;

        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionTextureObjectParameter;
        }

        protected override SurfaceParameter CreateSurfaceParameter(MaterialExpressionTextureObjectParameter parameterNode, MaterialConverter converter)
        {
            return converter.FindOrCreateParameter<Texture>(parameterNode.ParameterName, (p) => {
                // FIXME: don't use unresolved reference directly and don't guess the extension
                var assetItem = Helper.FindAsset(parameterNode.DefaultValue.FileName);

                if (null != assetItem) {
                    p.Value = assetItem.ID;
                }
            });
        }

        protected override SurfaceNode CreateNodeForSurfaceParameter(SurfaceParameter surfaceParameter, MaterialConverter converter, MaterialExpressionTextureObjectParameter unrealNode)
        {
            var parameterNode = base.CreateNodeForSurfaceParameter(surfaceParameter, converter, unrealNode);
            var samplerNode = converter.SpawnNode(Helper.FindTextureNodeArchetype(9), Helper.EditorPosition(unrealNode));
            samplerNode.SetValue(0, Helper.ConvertUnrealSamplerType(unrealNode.SamplerType));

            converter.Connect(parameterNode.GetBox(6), samplerNode.GetBox(0));

            return samplerNode;
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 4;
        }
    }
}
