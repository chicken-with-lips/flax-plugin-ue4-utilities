using FlaxEditor;
using FlaxEditor.Content.Settings;
using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class PostProcessingVolumeConverter : BaseActorConverter<PostProcessingVolume, PostFxVolume>
    {
        protected override Actor CreateActor(MapConverter converter, Scene scene, PostProcessingVolume unrealNode)
        {
            var actor = base.CreateActor(converter, scene, unrealNode) as PostFxVolume;

            if (! unrealNode.Unbound) {
                Editor.LogWarning("Bounded PostProcessingVolume is not supported");
            }

            actor.IsBounded = ! unrealNode.Unbound;
            actor.IsActive = unrealNode.Enabled;
            actor.Priority = (int)(unrealNode.Priority * 100);
            actor.BlendWeight = unrealNode.BlendWeight;

            SetColorGrading(actor, unrealNode);
            SetLens(actor, unrealNode);
            SetRenderingFeatures(actor, unrealNode);
            SetMotionBlur(actor, unrealNode);

            return actor;
        }

        private void SetColorGrading(PostFxVolume actor, PostProcessingVolume unrealNode)
        {
            var colorGrading = actor.ColorGrading;
            var toneMapping = actor.ToneMapping;
            var ueColorGrading = unrealNode.Settings.ColorGrading;

            if (ueColorGrading.OverrideColorSaturation) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorSaturation;
                colorGrading.ColorSaturation = Helper.ConvertUnrealColorF(ueColorGrading.ColorSaturation);
            }

            if (ueColorGrading.OverrideColorContrast) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorContrast;
                colorGrading.ColorContrast = Helper.ConvertUnrealColorF(ueColorGrading.ColorContrast);
            }

            if (ueColorGrading.OverrideColorContrast) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorContrast;
                colorGrading.ColorContrast = Helper.ConvertUnrealColorF(ueColorGrading.ColorContrast);
            }

            if (ueColorGrading.OverrideColorGamma) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGamma;
                colorGrading.ColorGamma = Helper.ConvertUnrealColorF(ueColorGrading.ColorGamma);
            }

            if (ueColorGrading.OverrideColorGain) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGain;
                colorGrading.ColorGain = Helper.ConvertUnrealColorF(ueColorGrading.ColorGain);
            }

            if (ueColorGrading.OverrideColorOffset) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorOffset;
                colorGrading.ColorOffset = Helper.ConvertUnrealColorF(ueColorGrading.ColorOffset);
            }

            if (ueColorGrading.OverrideColorSaturationShadows) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorOffset;
                colorGrading.ColorSaturationShadows = Helper.ConvertUnrealColorF(ueColorGrading.ColorSaturationShadows);
            }

            if (ueColorGrading.OverrideColorContrastShadows) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorContrastShadows;
                colorGrading.ColorContrastShadows = Helper.ConvertUnrealColorF(ueColorGrading.ColorContrastShadows);
            }

            if (ueColorGrading.OverrideColorGammaShadows) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGammaShadows;
                colorGrading.ColorGammaShadows = Helper.ConvertUnrealColorF(ueColorGrading.ColorGammaShadows);
            }

            if (ueColorGrading.OverrideColorGainShadows) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGainShadows;
                colorGrading.ColorGainShadows = Helper.ConvertUnrealColorF(ueColorGrading.ColorGainShadows);
            }

            if (ueColorGrading.OverrideColorOffsetShadows) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorOffsetShadows;
                colorGrading.ColorOffsetShadows = Helper.ConvertUnrealColorF(ueColorGrading.ColorGainShadows);
            }

            if (ueColorGrading.OverrideColorSaturationMidtones) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorSaturationMidtones;
                colorGrading.ColorSaturationMidtones = Helper.ConvertUnrealColorF(ueColorGrading.ColorSaturationMidtones);
            }

            if (ueColorGrading.OverrideColorContrastMidtones) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorContrastMidtones;
                colorGrading.ColorContrastMidtones = Helper.ConvertUnrealColorF(ueColorGrading.ColorContrastMidtones);
            }

            if (ueColorGrading.OverrideColorGammaMidtones) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGammaMidtones;
                colorGrading.ColorGammaMidtones = Helper.ConvertUnrealColorF(ueColorGrading.ColorGammaMidtones);
            }

            if (ueColorGrading.OverrideColorGainMidtones) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGainMidtones;
                colorGrading.ColorGainMidtones = Helper.ConvertUnrealColorF(ueColorGrading.ColorGainMidtones);
            }

            if (ueColorGrading.OverrideColorOffsetMidtones) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorOffsetMidtones;
                colorGrading.ColorOffsetMidtones = Helper.ConvertUnrealColorF(ueColorGrading.ColorOffsetMidtones);
            }

            if (ueColorGrading.OverrideColorSaturationHighlights) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorSaturationHighlights;
                colorGrading.ColorSaturationHighlights = Helper.ConvertUnrealColorF(ueColorGrading.ColorSaturationHighlights);
            }

            if (ueColorGrading.OverrideColorContrastHighlights) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorContrastHighlights;
                colorGrading.ColorContrastHighlights = Helper.ConvertUnrealColorF(ueColorGrading.ColorContrastHighlights);
            }

            if (ueColorGrading.OverrideColorGammaHighlights) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGammaHighlights;
                colorGrading.ColorGammaHighlights = Helper.ConvertUnrealColorF(ueColorGrading.ColorGammaHighlights);
            }

            if (ueColorGrading.OverrideColorGainHighlights) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorGainHighlights;
                colorGrading.ColorGainHighlights = Helper.ConvertUnrealColorF(ueColorGrading.ColorGainHighlights);
            }

            if (ueColorGrading.OverrideColorOffsetHighlights) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ColorOffsetHighlights;
                colorGrading.ColorOffsetHighlights = Helper.ConvertUnrealColorF(ueColorGrading.ColorOffsetHighlights);
            }

            if (ueColorGrading.OverrideColorCorrectionShadowsMax) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ShadowsMax;
                colorGrading.ShadowsMax = ueColorGrading.ColorCorrectionShadowsMax;
            }

            if (ueColorGrading.OverrideColorCorrectionHighlightsMin) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.ShadowsMax;
                colorGrading.HighlightsMin = ueColorGrading.ColorCorrectionHighlightsMin;
            }

            if (ueColorGrading.OverrideColorCorrectionHighlightsMin) {
                colorGrading.OverrideFlags |= ColorGradingSettingsOverride.LutTexture;
                colorGrading.LutTexture = Helper.LoadAsset<Texture>(ueColorGrading.ColorGradingLUT.FileName);
            }

            if (ueColorGrading.OverrideWhiteTemp) {
                toneMapping.OverrideFlags |= ToneMappingSettingsOverride.WhiteTemperature;
                toneMapping.WhiteTemperature = ueColorGrading.WhiteTemp;
            }

            if (ueColorGrading.OverrideWhiteTint) {
                toneMapping.OverrideFlags |= ToneMappingSettingsOverride.WhiteTint;
                toneMapping.WhiteTint = ueColorGrading.WhiteTint;
            }

            actor.ColorGrading = colorGrading;
            actor.ToneMapping = toneMapping;
        }

        private void SetLens(PostFxVolume actor, PostProcessingVolume unrealNode)
        {
            var lens = actor.CameraArtifacts;
            var depthOfField = actor.DepthOfField;
            var bloom = actor.Bloom;
            var ueLens = unrealNode.Settings.Lens;

            if (ueLens.OverrideVignetteIntensity) {
                lens.OverrideFlags |= CameraArtifactsSettingsOverride.VignetteIntensity;
                lens.VignetteIntensity = ueLens.VignetteIntensity;
            }

            if (ueLens.OverrideGrainIntensity) {
                lens.OverrideFlags |= CameraArtifactsSettingsOverride.GrainAmount;
                lens.GrainAmount = ueLens.GrainIntensity * (0.005f / 0.1f);
            }

            if (ueLens.OverrideChromaticAberrationStartOffset) {
                lens.OverrideFlags |= CameraArtifactsSettingsOverride.ChromaticDistortion;
                lens.ChromaticDistortion = ueLens.ChromaticAberrationStartOffset;
            }

            if (ueLens.OverrideDepthOfFieldFocalDistance) {
                depthOfField.OverrideFlags |= DepthOfFieldSettingsOverride.FocalDistance;
                depthOfField.FocalDistance = ueLens.DepthOfFieldFocalDistance;
            }

            if (ueLens.OverrideDepthOfFieldDepthBlurAmount) {
                depthOfField.OverrideFlags |= DepthOfFieldSettingsOverride.BlurStrength;
                depthOfField.BlurStrength = ueLens.DepthOfFieldDepthBlurAmount;
            }

            if (ueLens.OverrideDepthOfFieldFocalRegion) {
                depthOfField.OverrideFlags |= DepthOfFieldSettingsOverride.FocalRegion;
                depthOfField.FocalRegion = ueLens.DepthOfFieldFocalRegion;
            }

            if (ueLens.OverrideDepthOfFieldNearTransitionRegion) {
                depthOfField.OverrideFlags |= DepthOfFieldSettingsOverride.NearTransitionRange;
                depthOfField.NearTransitionRange = ueLens.DepthOfFieldNearTransitionRegion;
            }

            if (ueLens.OverrideDepthOfFieldFarTransitionRegion) {
                depthOfField.OverrideFlags |= DepthOfFieldSettingsOverride.FarTransitionRange;
                depthOfField.FarTransitionRange = ueLens.DepthOfFieldFarTransitionRegion;
            }

            actor.CameraArtifacts = lens;
            actor.Bloom = bloom;
        }

        private void SetRenderingFeatures(PostFxVolume actor, PostProcessingVolume unrealNode)
        {
            var ambient = actor.AmbientOcclusion;
            var screenSpaceReflections = actor.ScreenSpaceReflections;
            var ueRendering = unrealNode.Settings.RenderingFeatures;

            if (ueRendering.OverrideAmbientCubemapIntensity) {
                ambient.OverrideFlags |= AmbientOcclusionSettingsOverride.Intensity;
                ambient.Intensity = ueRendering.AmbientCubemapIntensity * 5f;
            }

            if (ueRendering.OverrideAmbientOcclusionPower) {
                ambient.OverrideFlags |= AmbientOcclusionSettingsOverride.Power;
                ambient.Power = ueRendering.AmbientOcclusionPower * (4f / 8f);
            }

            if (ueRendering.OverrideAmbientOcclusionRadius) {
                ambient.OverrideFlags |= AmbientOcclusionSettingsOverride.Radius;
                ambient.Radius = ueRendering.AmbientOcclusionRadius * (100f / 500f);
            }

            if (ueRendering.OverrideAmbientOcclusionFadeDistance) {
                ambient.OverrideFlags |= AmbientOcclusionSettingsOverride.FadeDistance;
                ambient.FadeDistance = ueRendering.AmbientOcclusionFadeDistance;
            }

            if (ueRendering.OverrideAmbientOcclusionFadeRadius) {
                ambient.OverrideFlags |= AmbientOcclusionSettingsOverride.FadeOutDistance;
                ambient.FadeOutDistance = ueRendering.AmbientOcclusionFadeRadius;
            }

            if (ueRendering.OverrideScreenSpaceReflectionIntensity) {
                screenSpaceReflections.OverrideFlags |= ScreenSpaceReflectionsSettingsOverride.Intensity;
                screenSpaceReflections.Intensity = ueRendering.ScreenSpaceReflectionIntensity * (1f / 100f);
            }

            if (ueRendering.OverrideScreenSpaceReflectionIntensity) {
                screenSpaceReflections.OverrideFlags |= ScreenSpaceReflectionsSettingsOverride.Intensity;
                screenSpaceReflections.ResolveSamples = (int)(ueRendering.ScreenSpaceReflectionQuality * (8f / 100f));
            }

            if (ueRendering.OverrideScreenSpaceReflectionMaxRoughness) {
                screenSpaceReflections.OverrideFlags |= ScreenSpaceReflectionsSettingsOverride.RoughnessThreshold;
                screenSpaceReflections.RoughnessThreshold = ueRendering.ScreenSpaceReflectionMaxRoughness;
            }

            actor.AmbientOcclusion = ambient;
            actor.ScreenSpaceReflections = screenSpaceReflections;
        }

        private void SetMotionBlur(PostFxVolume actor, PostProcessingVolume unrealNode)
        {
            var motionBlur = actor.MotionBlur;
            var ueMotionBlur = unrealNode.Settings.MotionBlur;

            if (ueMotionBlur.OverrideMotionBlurAmount) {
                motionBlur.OverrideFlags |= MotionBlurSettingsOverride.Scale;
                motionBlur.Scale = ueMotionBlur.MotionBlurAmount;
            }

            actor.MotionBlur = motionBlur;
        }
    }
}
