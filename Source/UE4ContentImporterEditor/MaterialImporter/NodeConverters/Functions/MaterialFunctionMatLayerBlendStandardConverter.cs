using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Functions
{
    public class MaterialFunctionMatLayerBlendStandardConverter : GenericFunctionConverter
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionMaterialFunctionCall functionNode && functionNode.MaterialFunction.NodeName == "/Engine/Functions/MaterialLayerFunctions/MatLayerBlend_Standard.MatLayerBlend_Standard";
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionMaterialFunctionCall unrealNode)
        {
            return converter.SpawnNode(Helper.FindLayersNodeArchetype(2), Helper.EditorPosition(unrealNode));
        }
        
        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
           return 3;
        }

        protected override int GetConnectionIdSlotForFunctionInput(string inputName, MaterialNode functionNode)
        {
            switch (inputName) {
                case "Base Material":
                    return 0;

                case "Top Material":
                    return 1;

                case "Alpha":
                    return 2;
            }

            return -1;
        }
    }
}
