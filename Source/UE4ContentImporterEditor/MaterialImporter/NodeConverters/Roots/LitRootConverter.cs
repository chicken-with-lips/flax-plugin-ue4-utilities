using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Roots
{
    public class LitRootConverter : GenericConverter<Material>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is Material rootNode && (rootNode.ShadingModel == ShadingModel.DefaultLit || rootNode.ShadingModel == ShadingModel.Subsurface || rootNode.ShadingModel == ShadingModel.TwoSidedFoliage);
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, Material unrealNode)
        {
            return converter.SpawnNode(Helper.FindMaterialNodeArchetype(1), Helper.EditorPosition(unrealNode));
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return -1;
        }

        public override void CreateConnections(Material unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            converter.Connect(unrealNode.BaseColor, unrealNode.Name, 1);
            converter.Connect(unrealNode.OpacityMask, unrealNode.Name, 2);
            converter.Connect(unrealNode.EmissiveColor, unrealNode.Name, 3);
            converter.Connect(unrealNode.Metallic, unrealNode.Name, 4);
            converter.Connect(unrealNode.Specular, unrealNode.Name, 5);
            converter.Connect(unrealNode.Roughness, unrealNode.Name, 6);
            converter.Connect(unrealNode.AmbientOcclusion, unrealNode.Name, 7);
            converter.Connect(unrealNode.Normal, unrealNode.Name, 8);
            converter.Connect(unrealNode.Opacity, unrealNode.Name, 9);
            converter.Connect(unrealNode.Refraction, unrealNode.Name, 10);
        }
    }
}
