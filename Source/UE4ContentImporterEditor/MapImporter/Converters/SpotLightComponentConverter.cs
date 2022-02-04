using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class SpotLightComponentConverter : BaseComponentConverter<SpotLightComponent, SpotLight>
    {
        protected override void Apply(MapConverter converter, SpotLightComponent unrealNode, Node parentUnrealNode, SpotLight actor)
        {
            actor.Radius = Helper.ScaleUnrealLightAttenuationRadius(unrealNode.AttenuationRadius);
            actor.Brightness = Helper.ScaleUnrealLightIntensity(unrealNode.Intensity);
            actor.Color = Helper.ConvertUnrealColor(unrealNode.LightColor);
            actor.ShadowsMode = unrealNode.CastShadows ? ShadowsCastingMode.All : ShadowsCastingMode.None;

            actor.InnerConeAngle = unrealNode.InnerConeAngle;
            actor.OuterConeAngle = unrealNode.OuterConeAngle;
        }
    }
}
