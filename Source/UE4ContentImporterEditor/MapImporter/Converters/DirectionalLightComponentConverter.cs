using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class DirectionalLightComponentConverter : BaseComponentConverter<DirectionalLightComponent, DirectionalLight>
    {
        protected override void Apply(MapConverter converter, DirectionalLightComponent unrealNode, Node parentUnrealNode, DirectionalLight actor)
        {
            actor.Brightness = unrealNode.Intensity;
            actor.Color = Helper.ConvertUnrealColor(unrealNode.LightColor);
            actor.ShadowsMode = unrealNode.CastShadows ? ShadowsCastingMode.All : ShadowsCastingMode.None;
        }
    }
}
