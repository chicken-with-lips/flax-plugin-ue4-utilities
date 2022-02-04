using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Roots
{
    public class UnlitRootConverter : GenericConverter<Material>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is Material rootNode && rootNode.ShadingModel == ShadingModel.Unlit;
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
            converter.Connect(unrealNode.OpacityMask, unrealNode.Name, 2);
            converter.Connect(unrealNode.EmissiveColor, unrealNode.Name, 3);
            converter.Connect(unrealNode.Opacity, unrealNode.Name, 9);
            converter.Connect(unrealNode.Refraction, unrealNode.Name, 10);
        }
    }
}
