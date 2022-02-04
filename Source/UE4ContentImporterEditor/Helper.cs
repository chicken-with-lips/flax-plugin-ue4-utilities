using System;
using System.Collections.Generic;
using System.IO;
using FlaxEngine;
using FlaxEditor;
using FlaxEditor.Content;
using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D;
using T3D = JollySamurai.UnrealEngine4.T3D;
using Vector4 = FlaxEngine.Vector4;

namespace UE4ContentImporterEditor
{
    public class Helper
    {
        public static FlaxEngine.Vector3 ConvertUnrealVector3(T3D.Vector3 v)
        {
            var x = v.X;
            var y = v.Y;
            var z = v.Z;

            return new FlaxEngine.Vector3(y, z, x);
        }

        public static Quaternion ConvertUnrealRotator(Rotator rotation)
        {
            var q = Quaternion.RotationYawPitchRoll(Mathf.DegreesToRadians * rotation.Yaw, Mathf.DegreesToRadians * -rotation.Pitch, Mathf.DegreesToRadians * -rotation.Roll);

            return q;
        }

        public static Color ConvertUnrealColor(T3D.Vector4 value)
        {
            return new Color(value.X / 255f, value.Y / 255f, value.Z / 255f, value.A / 255f);
        }

        public static Vector4 ConvertUnrealColorF(T3D.Vector4 value)
        {
            return new Vector4(value.X, value.Y, value.Z, value.A);
        }

        public static FlaxEngine.Vector2 EditorPosition(IEditorPositionable node)
        {
            return new FlaxEngine.Vector2(node.EditorX, node.EditorY);
        }

        public static NodeArchetype FindNodeArchetype(IList<NodeArchetype> archetypes, uint typeId)
        {
            foreach (var nodeArchetype in archetypes) {
                if (nodeArchetype.TypeID == typeId) {
                    return nodeArchetype;
                }
            }

            return null;
        }

        public static NodeArchetype FindMaterialNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Material.Nodes, typeId);
        }

        public static NodeArchetype FindMathNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Math.Nodes, typeId);
        }

        public static NodeArchetype FindParameterNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Parameters.Nodes, typeId);
        }

        public static NodeArchetype FindToolNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Tools.Nodes, typeId);
        }

        public static NodeArchetype FindConstantNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Constants.Nodes, typeId);
        }

        public static NodeArchetype FindTextureNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Textures.Nodes, typeId);
        }

        public static NodeArchetype FindPackingNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Packing.Nodes, typeId);
        }

        public static NodeArchetype FindComparisonNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Comparisons.Nodes, typeId);
        }

        public static NodeArchetype FindLayersNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Layers.Nodes, typeId);
        }

        public static NodeArchetype FindParticlesNodeArchetype(uint typeId)
        {
            return FindNodeArchetype(FlaxEditor.Surface.Archetypes.Particles.Nodes, typeId);
        }

        public static AssetItem FindAsset(string path)
        {
            path = Path.Combine(Globals.ProjectContentFolder, path.Substring(1));
            path = Path.ChangeExtension(path, "flax");

            return Editor.Instance.ContentDatabase.Find(path) as AssetItem;
        }

        public static MaterialDomain ConvertUnrealMaterialDomain(T3D.Material.MaterialDomain domain)
        {
            switch (domain) {
                case T3D.Material.MaterialDomain.Surface:
                    return MaterialDomain.Surface;
                case T3D.Material.MaterialDomain.Ui:
                    return MaterialDomain.GUI;
                case T3D.Material.MaterialDomain.Volume:
                    return MaterialDomain.VolumeParticle;
                case T3D.Material.MaterialDomain.DeferredDecal:
                    return MaterialDomain.Decal;
                case T3D.Material.MaterialDomain.PostProcess:
                    return MaterialDomain.PostProcess;

                case T3D.Material.MaterialDomain.RuntimeVirtualTexture:
                case T3D.Material.MaterialDomain.LightFunction:
                    break;
            }

            return MaterialDomain.Surface;
        }

        public static MaterialShadingModel ConvertUnrealShadingModel(ShadingModel shadingModel)
        {
            switch (shadingModel) {
                case ShadingModel.Unlit:
                    return MaterialShadingModel.Unlit;
                case ShadingModel.DefaultLit:
                    return MaterialShadingModel.Lit;
                case ShadingModel.Subsurface:
                    return MaterialShadingModel.Subsurface;
            }

            return MaterialShadingModel.Lit;
        }

        public static MaterialBlendMode ConvertUnrealBlendMode(BlendMode blendMode)
        {
            switch (blendMode) {
                case BlendMode.Additive:
                    return MaterialBlendMode.Additive;
                case BlendMode.Masked:
                    return MaterialBlendMode.Opaque;
                case BlendMode.Modulate:
                    return MaterialBlendMode.Multiply;
                case BlendMode.Opaque:
                    return MaterialBlendMode.Opaque;
                case BlendMode.Translucent:
                    return MaterialBlendMode.Transparent;
            }

            return MaterialBlendMode.Additive;
        }

        public static MaterialDecalBlendingMode ConvertUnrealDecalBlendMode(DecalBlendMode blendMode)
        {
            switch (blendMode) {
                case DecalBlendMode.Translucent:
                    return MaterialDecalBlendingMode.Translucent;
                case DecalBlendMode.Stain:
                    return MaterialDecalBlendingMode.Stain;
                case DecalBlendMode.Normal:
                    return MaterialDecalBlendingMode.Normal;
                case DecalBlendMode.Emissive:
                    return MaterialDecalBlendingMode.Emissive;
            }

            return MaterialDecalBlendingMode.Translucent;
        }

        public static Asset CreateWorkingCopyOfAsset<T>(string path, Editor.NewAssetType type, out string workingCopyPath, out T workingCopy)
            where T : Asset
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            if (null == Editor.Instance.ContentDatabase.Find(path)) {
                Scripting.RunOnUpdate(() => Editor.CreateAsset(type, path)).Wait();
            }

            var originalAsset = Content.LoadAsync(path);

            if (originalAsset.WaitForLoaded()) {
                throw new System.Exception("Failed to load original asset: " + path);
            }

            if (Editor.Instance.ContentEditing.FastTempAssetClone(path, out workingCopyPath)) {
                throw new System.Exception("Failed to clone asset: " + path);
            }

            var tmp = Content.LoadAsync(workingCopyPath);
            workingCopy = tmp as T;

            if (workingCopy == null) {
                return null;
            }

            if (workingCopy.WaitForLoaded()) {
                throw new System.Exception("Failed to load working copy: " + workingCopyPath);
            }

            return originalAsset;
        }

        public static void SaveToOriginalAsset(Asset workingCopy, Guid originalId, string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

            // Wait until temporary asset file be fully loaded
            if (workingCopy.WaitForLoaded()) {
                Editor.LogError($"Cannot save asset {sourcePath}. Wait for temporary asset loaded failed.");

                return;
            }

            // Check if original asset is loaded
            var originalAsset = Content.GetAsset(originalId);

            if (originalAsset) {
                // Wait for loaded to prevent any issues
                if (! originalAsset.IsLoaded && originalAsset.LastLoadFailed) {
                    Editor.LogWarning($"Copying asset \"{sourcePath}\" to \"{destinationPath}\" (last load failed)");
                } else if (originalAsset.WaitForLoaded()) {
                    Editor.LogError($"Cannot save asset \"{sourcePath}\". Wait for original asset loaded failed.");

                    return;
                }
            }

            // Copy temporary material to the final destination (and restore ID)
            if (Editor.Instance.ContentEditing.CloneAssetFile(sourcePath, destinationPath, originalId)) {
                Editor.LogError($"Cannot copy asset \"{sourcePath}\" to \"{destinationPath}\"");
            }

            // Reload original asset
            if (originalAsset) {
                // originalAsset.Reload();
                if (originalAsset.WaitForLoaded()) {
                    throw new System.Exception("Failed to load original asset");
                }

                Editor.Instance.ContentDatabase.FindAsset(originalId)?.RefreshThumbnail();
            }
        }

        public static string GetAssetPath(string unrealFileName)
        {
            return Globals.ProjectContentFolder + Path.ChangeExtension(unrealFileName, "flax");
        }

        public static T LoadAsset<T>(string unrealFileName)
            where T : Asset
        {
            if (string.IsNullOrEmpty(unrealFileName)) {
                return null;
            }

            var assetPath = GetAssetPath(unrealFileName);
            var asset = Content.LoadAsync(assetPath) as T;
            asset?.WaitForLoaded();

            if (asset == null) {
                Editor.LogWarning($"Couldn't find asset \"{assetPath}\" ({unrealFileName})");
            }

            return asset;
        }

        public static float ScaleUnrealLightIntensity(float intensity)
        {
            return intensity / 5000f;
        }

        public static float ScaleUnrealLightAttenuationRadius(float attenuation)
        {
            return attenuation * 2f;
        }

        public static MaterialSceneTextures ConvertUnrealMaterialSceneTextureId(SceneTextureId textureId)
        {
            switch (textureId) {
                case SceneTextureId.SceneDepth:
                    return MaterialSceneTextures.SceneDepth;
                case SceneTextureId.DiffuseColor:
                    return MaterialSceneTextures.DiffuseColor;
                case SceneTextureId.SpecularColor:
                    return MaterialSceneTextures.SpecularColor;
                case SceneTextureId.WorldNormal:
                    return MaterialSceneTextures.WorldNormal;
                case SceneTextureId.AmbientOcclusion:
                    return MaterialSceneTextures.AmbientOcclusion;
                case SceneTextureId.Metallic:
                    return MaterialSceneTextures.Metalness;
                case SceneTextureId.Roughness:
                    return MaterialSceneTextures.Roughness;
                case SceneTextureId.Specular:
                    return MaterialSceneTextures.Specular;
                case SceneTextureId.BaseColor:
                    return MaterialSceneTextures.BaseColor;
                case SceneTextureId.ShadingModelId:
                    return MaterialSceneTextures.ShadingModel;
            }

            return MaterialSceneTextures.SceneColor;
        }

        public static int ConvertUnrealSamplerType(SamplerType samplerType)
        {
            return 0; // LinearClamp
        }

        public static TransformCoordinateSystem ConvertUnrealMaterialVectorCoordTransformSource(MaterialVectorCoordTransformSource value)
        {
            switch (value) {
                case MaterialVectorCoordTransformSource.Tangent:
                    return TransformCoordinateSystem.Tangent;
                case MaterialVectorCoordTransformSource.Local:
                    return TransformCoordinateSystem.Local;
                case MaterialVectorCoordTransformSource.View:
                    return TransformCoordinateSystem.View;
            }

            return TransformCoordinateSystem.World;
        }

        public static TransformCoordinateSystem ConvertUnrealMaterialVectorCoordTransform(MaterialVectorCoordTransform value)
        {
            switch (value) {
                case MaterialVectorCoordTransform.Tangent:
                    return TransformCoordinateSystem.Tangent;
                case MaterialVectorCoordTransform.Local:
                    return TransformCoordinateSystem.Local;
                case MaterialVectorCoordTransform.View:
                    return TransformCoordinateSystem.View;
            }

            return TransformCoordinateSystem.World;
        }
    }
}
