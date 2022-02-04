using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;
using SkyLight = FlaxEngine.SkyLight;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class SkyLightComponentConverter : BaseComponentConverter<SkyLightComponent, SkyLight>
    {
        protected override void Apply(MapConverter converter, SkyLightComponent unrealNode, Node parentUnrealNode, SkyLight actor)
        {
            actor.Mode = unrealNode.SourceType == SkyLightSourceType.CapturedScene ? SkyLight.Modes.CaptureScene : SkyLight.Modes.CustomTexture;
            actor.CustomTexture = Helper.LoadAsset<CubeTexture>(unrealNode.Cubemap?.FileName);
            actor.SkyDistanceThreshold = unrealNode.SkyDistanceThreshold;
            actor.Brightness = Helper.ScaleUnrealLightIntensity(unrealNode.Intensity);

            
            // intensity is a scale here not an absolute value
            actor.Brightness = Helper.ScaleUnrealLightIntensity(5000f * unrealNode.Intensity);
            actor.IndirectLightingIntensity = unrealNode.IndirectLightingIntensity * (100f / 6f);
            actor.VolumetricScatteringIntensity = unrealNode.VolumetricScatteringIntensity * (100f / 4f);
            actor.CastVolumetricShadow = unrealNode.CastVolumetricShadow;
            actor.Color = Helper.ConvertUnrealColor(unrealNode.LightColor);
        }
    }
}
