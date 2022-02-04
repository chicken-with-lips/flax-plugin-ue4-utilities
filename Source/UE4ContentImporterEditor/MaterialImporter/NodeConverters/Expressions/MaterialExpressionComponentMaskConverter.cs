using FlaxEditor.Surface;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;
using Material = JollySamurai.UnrealEngine4.T3D.Material.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters.Expressions
{
    public class MaterialExpressionComponentMaskConverter : GenericConverter<MaterialExpressionComponentMask>
    {
        public override bool CanConvert(MaterialNode unrealNode)
        {
            return unrealNode is MaterialExpressionComponentMask;
        }

        protected override SurfaceNode CreateNode(MaterialConverter converter, MaterialExpressionComponentMask unrealNode)
        {
            bool maskR = unrealNode.R;
            bool maskG = unrealNode.G;
            bool maskB = unrealNode.B;
            bool maskA = unrealNode.A;

            if (maskR && maskG && maskB && maskA) {
                // this is unlikely, leave it unsupported for now
                throw new System.Exception("Unsupported MaterialExpressionComponentMask configuration");
            } else if (maskR && maskG && maskB) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(70), Helper.EditorPosition(unrealNode));                
            } else if (maskR && maskG) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(44), Helper.EditorPosition(unrealNode));
            } else if (maskR && maskB) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(45), Helper.EditorPosition(unrealNode));
            } else if (maskR && maskA) {
                throw new System.Exception("maskR && maskA not supported by Flax");
            } else if (maskG && maskB) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(46), Helper.EditorPosition(unrealNode));
            } else if (maskG && maskA) {
                throw new System.Exception("maskG && maskA not supported by Flax");
            } else if (maskB && maskA) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(47), Helper.EditorPosition(unrealNode));
            } else if (maskR) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(40), Helper.EditorPosition(unrealNode));
            } else if (maskG) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(41), Helper.EditorPosition(unrealNode));
            } else if (maskB) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(42), Helper.EditorPosition(unrealNode));
            } else if (maskA) {
                return converter.SpawnNode(Helper.FindPackingNodeArchetype(43), Helper.EditorPosition(unrealNode));
            }

            throw new System.Exception($"Unhandled mask configuration (R={maskR},G={maskG},B={maskB},A={maskA})");
        }

        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 1;
        }

        public override void CreateConnections(MaterialExpressionComponentMask unrealNode, Material unrealMaterial, MaterialConverter builder)
        {
            if (unrealNode.Input != null) {
                builder.Connect(unrealNode.Input, unrealNode.Name, 0);
            }
        }
    }
}
