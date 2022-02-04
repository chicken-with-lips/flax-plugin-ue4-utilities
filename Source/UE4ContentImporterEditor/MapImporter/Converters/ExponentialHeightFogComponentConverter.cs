using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;
using ExponentialHeightFog = FlaxEngine.ExponentialHeightFog;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class ExponentialHeightFogComponentConverter : BaseComponentConverter<ExponentialHeightFogComponent, ExponentialHeightFog>
    {
        protected override void Apply(MapConverter converter, ExponentialHeightFogComponent unrealNode, Node parentUnrealNode, ExponentialHeightFog actor)
        {
            actor.FogDensity = unrealNode.FogDensity;
            actor.FogHeightFalloff = unrealNode.FogHeightFalloff;
            actor.FogInscatteringColor = Helper.ConvertUnrealColorF(unrealNode.FogInscatteringColor);
            actor.FogMaxOpacity = unrealNode.FogMaxOpacity;

            actor.DirectionalInscatteringColor = Helper.ConvertUnrealColorF(unrealNode.InscatteringTextureTint);
            actor.DirectionalInscatteringStartDistance = unrealNode.FullyDirectionalInscatteringColorDistance;

            actor.StartDistance = unrealNode.StartDistance;
            actor.FogCutoffDistance = unrealNode.FogCutoffDistance;

            actor.VolumetricFogEnable = unrealNode.EnableVolumetricFog;
            actor.VolumetricFogScatteringDistribution = unrealNode.VolumetricFogScatteringDistribution;
            actor.VolumetricFogAlbedo = Helper.ConvertUnrealColorF(unrealNode.VolumetricFogAlbedo);
            actor.VolumetricFogEmissive = Helper.ConvertUnrealColorF(unrealNode.VolumetricFogEmissive);
            actor.VolumetricFogExtinctionScale = unrealNode.VolumetricFogExtinctionScale;
            actor.VolumetricFogDistance = unrealNode.VolumetricFogDistance;
        }
    }
}
