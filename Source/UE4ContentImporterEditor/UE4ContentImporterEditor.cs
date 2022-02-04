using FlaxEditor;
using FlaxEditor.GUI.ContextMenu;
using UE4ContentImporterEditor.UI;

namespace UE4ContentImporterEditor
{
    public class UE4ContentImporterEditor : EditorPlugin
    {
        private ContextMenuChildMenu _childMenu;
        private ContextMenuSeparator _separator;
        
        private ContextMenuButton _converterButton;
        private ConverterWindow _converterWindow;

        private ContextMenuButton _modelMapperButton;
        private ModelMaterialMapperWindow _modelMapperWindow;

        /// <inheritdoc />
        public override void InitializeEditor()
        {
            base.InitializeEditor();
            
            _separator = Editor.UI.MenuTools.ContextMenu.AddSeparator();
            _childMenu = Editor.UI.MenuTools.ContextMenu.AddChildMenu("UE4 Utilities");
            _converterButton = _childMenu.ContextMenu.AddButton("Content Importer");
            _modelMapperButton = _childMenu.ContextMenu.AddButton("Model Material Mapper");

            _converterButton.Clicked += () => {
                if (_converterWindow == null) {
                    _converterWindow = new ConverterWindow(Editor);
                }

                _converterWindow.Show();
            };

            _modelMapperButton.Clicked += () => {
                if (_modelMapperWindow == null) {
                    _modelMapperWindow = new ModelMaterialMapperWindow(Editor);
                }

                _modelMapperWindow.Show();
            };
        }

        /// <inheritdoc />
        public override void Deinitialize()
        {
            if (_converterButton != null) {
                _converterButton.Dispose();
                _converterButton = null;
            }

            if (_converterWindow != null) {
                _converterWindow.Dispose();
                _converterWindow = null;
            }

            if (_modelMapperButton != null) {
                _modelMapperButton.Dispose();
                _modelMapperButton = null;
            }

            if (_modelMapperWindow != null) {
                _modelMapperWindow.Dispose();
                _modelMapperWindow = null;
            }

            if (_modelMapperWindow != null) {
                _modelMapperWindow.Dispose();
                _modelMapperWindow = null;
            }

            if (_childMenu != null) {
                _childMenu.Dispose();
                _childMenu = null;
            }

            if (_separator != null) {
                _separator.Dispose();
                _separator = null;
            }

            base.Deinitialize();
        }
    }
}
