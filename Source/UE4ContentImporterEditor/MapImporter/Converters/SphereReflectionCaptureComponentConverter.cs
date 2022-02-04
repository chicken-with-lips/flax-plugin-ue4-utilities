using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class SphereReflectionCaptureComponentConverter : BaseComponentConverter<SphereReflectionCaptureComponent, EnvironmentProbe>
    {
        protected override void Apply(MapConverter converter, SphereReflectionCaptureComponent unrealNode, Node parentUnrealNode, EnvironmentProbe actor)
        {
            actor.Radius = unrealNode.InfluenceRadius;
            actor.Brightness = unrealNode.Brightness;

            if (unrealNode.ReflectionSourceType == ReflectionSourceType.SpecifiedCubemap) {
                actor.CustomProbe = Helper.LoadAsset<CubeTexture>(unrealNode.Cubemap.FileName);
            }
        }
    }
}
