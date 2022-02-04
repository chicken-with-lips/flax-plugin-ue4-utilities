using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;
using Vector3 = FlaxEngine.Vector3;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class DecalComponentConverter : BaseComponentConverter<DecalComponent, Decal>
    {
        protected override void Apply(MapConverter converter, DecalComponent unrealNode, Node parentUnrealNode, Decal actor)
        {
            actor.LocalOrientation = Quaternion.RotationYawPitchRoll(
                Mathf.DegreesToRadians * Mathf.UnwindDegrees(unrealNode.Rotation.Yaw),
                Mathf.DegreesToRadians * Mathf.UnwindDegrees(unrealNode.Rotation.Roll),
                Mathf.DegreesToRadians * Mathf.UnwindDegrees(unrealNode.Rotation.Pitch)
                
            );

            actor.LocalScale = new Vector3(
                unrealNode.Scale3D.Z < 0 ? -1 : 1,
                unrealNode.Scale3D.X < 0 ? -1 : 1,
                unrealNode.Scale3D.Y < 0 ? -1 : 1);

            actor.Size = new Vector3(
                unrealNode.Scale3D.Z * unrealNode.DeclSize.Z,
                unrealNode.Scale3D.X * unrealNode.DeclSize.X,
                unrealNode.Scale3D.Y * unrealNode.DeclSize.Y)
                         * 2f;

            if (unrealNode.DecalMaterial != null) {
                actor.Material = Helper.LoadAsset<MaterialBase>(unrealNode.DecalMaterial.FileName);
            }
        }
    }
}
