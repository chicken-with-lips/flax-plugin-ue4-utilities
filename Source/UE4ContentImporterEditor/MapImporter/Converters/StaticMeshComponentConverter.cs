using System.Linq;
using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public class StaticMeshComponentConverter : BaseComponentConverter<StaticMeshComponent, StaticModel>
    {
        protected override void Apply(MapConverter converter, StaticMeshComponent unrealNode, Node parentUnrealNode, StaticModel actor)
        {
            actor.Model = Helper.LoadAsset<Model>(unrealNode.StaticMesh.FileName);

            var overrideMaterials = unrealNode.OverrideMaterials
                .Where(reference => "None" != reference.FileName)
                .Select(reference => Helper.LoadAsset<MaterialBase>(reference.FileName))
                .ToArray();
            
            for (var i = 0; i < overrideMaterials.Length; i++) {
                actor.SetMaterial(i, overrideMaterials[i]);
            }
        }
    }
}
