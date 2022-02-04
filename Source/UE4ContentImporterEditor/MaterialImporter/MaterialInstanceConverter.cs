using System.Linq;
using FlaxEditor;
using FlaxEngine;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Parser;
using MaterialInstance = JollySamurai.UnrealEngine4.T3D.MaterialInstance.MaterialInstance;
using Vector4 = FlaxEngine.Vector4;

namespace UE4ContentImporterEditor.MaterialImporter
{
    public class MaterialInstanceConverter
    {
        private readonly MaterialInstance _unrealMaterialInstance;
        private readonly Material _parentMaterialAsset;
        private readonly string _outputPath;

        private MaterialInstanceConverter(MaterialInstance unrealMaterialInstance, Material parentMaterialAsset, string outputPath)
        {
            _unrealMaterialInstance = unrealMaterialInstance;
            _parentMaterialAsset = parentMaterialAsset;
            _outputPath = outputPath;
        }

        private void Run()
        {
            var originalAsset = Helper.CreateWorkingCopyOfAsset<FlaxEngine.MaterialInstance>(_outputPath, Editor.NewAssetType.MaterialInstance, out var workingCopyPath, out var workingCopy);

            if (workingCopy == null) {
                return;
            }

            workingCopy.BaseMaterial = _parentMaterialAsset;

            SetParameters(workingCopy, _unrealMaterialInstance.ScalarParameters, propertyBag => ValueUtil.ParseFloat(propertyBag.FindPropertyValue("ParameterValue")));
            SetParameters(workingCopy, _unrealMaterialInstance.VectorParameters, propertyBag => {
                var unrealVector = ValueUtil.ParseVector4(propertyBag.FindPropertyValue("ParameterValue"));

                return new Vector4(unrealVector.X, unrealVector.Y, unrealVector.Z, unrealVector.A);
            });
            SetParameters(workingCopy, _unrealMaterialInstance.TextureParameters, propertyBag => {
                var reference = ValueUtil.ParseResourceReference(propertyBag.FindPropertyValue("ParameterValue")).FileName;

                return Helper.LoadAsset<Asset>(reference);
            });

            workingCopy.Save();

            Helper.SaveToOriginalAsset(workingCopy, originalAsset.ID, workingCopyPath, _outputPath);
        }

        private delegate object ValueProcessorDelegate(ParsedPropertyBag propertyBag);

        private void SetParameters(FlaxEngine.MaterialInstance asset, ParsedPropertyBag[] parameters, ValueProcessorDelegate valueProcessor)
        {
            foreach (var parameter in parameters) {
                var parameterName = parameter.FindPropertyValue("ParameterName") ?? FindNameFromParameterInfo(parameter);

                if (! parameter.HasProperty("ParameterValue") || ! asset.Parameters.Any(p => p.Name == parameterName)) {
                    continue;
                }

                var parameterValue = valueProcessor(parameter);

                if (parameterValue != null) {
                    asset.SetParameterValue(parameterName, parameterValue);
                }
            }
        }

        private string FindNameFromParameterInfo(ParsedPropertyBag parameter)
        {
            // FIXME: this should be handled by the parser
            var tmpBag = ValueUtil.ParseAttributeList(parameter.FindPropertyValue("ParameterInfo"));

            return tmpBag.FindPropertyValue("Name");
        }

        public static void FromUnrealMaterialInstance(MaterialInstance materialInstance, Material parentMaterialAsset, string outputPath)
        {
            var converter = new MaterialInstanceConverter(materialInstance, parentMaterialAsset, outputPath);
            converter.Run();
        }
    }
}
