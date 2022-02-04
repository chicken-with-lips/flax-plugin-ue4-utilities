using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class PointLightComponentConverter : BaseComponentConverter<PointLightComponent, PointLight>
    {
        protected override void Apply(MapConverter converter, PointLightComponent unrealNode, Node parentUnrealNode, PointLight actor)
        {
            actor.Radius = Helper.ScaleUnrealLightAttenuationRadius(unrealNode.AttenuationRadius);
            actor.Brightness = Helper.ScaleUnrealLightIntensity(unrealNode.Intensity);
            actor.Color = Helper.ConvertUnrealColor(unrealNode.LightColor);
            actor.ShadowsMode = unrealNode.CastShadows ? ShadowsCastingMode.All : ShadowsCastingMode.None;
        }
    }
}
