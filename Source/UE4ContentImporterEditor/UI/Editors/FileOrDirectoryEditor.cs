using FlaxEditor;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace UE4ContentImporterEditor.UI.CustomEditors
{
    public abstract class FileOrDirectoryEditor : CustomEditor
    {
        public abstract string FileFilter { get; }

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        public virtual bool AllowSingleFileSelection { get; } = true;
        public virtual bool AllowDirectorySelection { get; } = true;

        private TextBoxElement _textBoxElement;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var container = layout.HorizontalPanel();

            _textBoxElement = container.TextBox();
            _textBoxElement.Control.AnchorPreset = AnchorPresets.HorizontalStretchMiddle;

            _textBoxElement.TextBox.TextChanged += () => {
                OnValueChanged(_textBoxElement.Text);
            };

            if (AllowSingleFileSelection) {
                var fileButton = CreateButton(Editor.Instance.Icons.Document128, container, "Select a file");

                fileButton.Button.Clicked += () => {
                    if (FileSystem.ShowOpenFileDialog(Editor.Instance.Windows.MainWindow, null, FileFilter, false, "Select file", out var files)) {
                        return;
                    }

                    OnValueChanged(files[0]);
                };
            }

            if (AllowDirectorySelection) {
                var directoryButton = CreateButton(Editor.Instance.Icons.Folder32, container, "Select a directory");

                directoryButton.Button.Clicked += () => {
                    if (FileSystem.ShowBrowseFolderDialog(Editor.Instance.Windows.MainWindow, null, "Select directory", out var path)) {
                        return;
                    }

                    OnValueChanged(path);
                };
            }

            container.Panel.Height = _textBoxElement.Control.Height;
        }

        private ButtonElement CreateButton(SpriteHandle sprite, LayoutElementsContainer container, string tooltip)
        {
            var button = container.Button("F");
            button.Control.AnchorPreset = AnchorPresets.MiddleRight;
            button.Control.TooltipText = tooltip;
            button.Control.Width = 15;
            button.Button.Text = "";
            button.Button.BorderColor = Color.Transparent;
            button.Button.BorderColorHighlighted = Color.Transparent;
            button.Button.BackgroundBrush = new SpriteBrush(sprite);

            return button;
        }

        protected override void SynchronizeValue(object value)
        {
            base.SynchronizeValue(value);

            _textBoxElement.Text = (string) value;
        }

        private void OnValueChanged(string newValue)
        {
            SetValue(newValue);
        }

        public override void Refresh()
        {
            base.Refresh();

            if (!HasDifferentValues) {
                SynchronizeValue(Values[0]);
            }
        }
    }
}
