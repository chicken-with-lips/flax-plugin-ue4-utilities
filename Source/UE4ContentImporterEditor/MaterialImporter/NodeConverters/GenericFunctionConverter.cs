using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters
{
    public abstract class GenericFunctionConverter : GenericConverter<MaterialExpressionMaterialFunctionCall>
    {
        public override int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag)
        {
            return 2;
        }

        public override void CreateConnections(MaterialExpressionMaterialFunctionCall unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            foreach (var functionInput in unrealNode.FunctionInputs) {
                var inputPropertyBag = ValueUtil.ParseAttributeList(functionInput.FindPropertyValue("Input"));
                var expressionValue = ValueUtil.ParseExpressionReference(inputPropertyBag.FindPropertyValue("Expression"));
                var resolvedFunctionInput = unrealMaterial.ResolveExpressionReference(expressionValue);
            
                if (resolvedFunctionInput != null) {
                    var slotId = GetConnectionIdSlotForFunctionInput(inputPropertyBag.FindPropertyValue("InputName"), resolvedFunctionInput);
                    var inputSlot = converter.FindBox(resolvedFunctionInput?.Name, unrealNode.Name, slotId, inputPropertyBag);
            
                    converter.Connect(resolvedFunctionInput?.Name, unrealNode.Name, slotId, inputPropertyBag);
                }
            }
        }

        protected abstract int GetConnectionIdSlotForFunctionInput(string inputName, MaterialNode functionNode);
    }
}
