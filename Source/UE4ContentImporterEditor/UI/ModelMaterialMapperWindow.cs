using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlaxEditor;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.Windows;
using FlaxEngine;
using FlaxEngine.GUI;
using UE4ContentImporterEditor.UI.CustomEditors;

namespace UE4ContentImporterEditor.UI
{
    public class ModelMaterialMapperWindow : BaseWindow
    {
        public class Settings
        {
            [CustomEditor(typeof(FlaxFileOrDirectoryEditor)), EditorDisplay("Content")]
            public string SourceFiles { get; set; }

            [CustomEditor(typeof(DirectoryEditor)), EditorDisplay("Content")]
            public string MaterialSearchRoot { get; set; }
        }

        protected override string SettingsPrefix => "UE4ModelMaterialMapperEditor";

        private Settings _settings;

        private CustomEditorPresenter _presenter;
        private Button _processButton;

        private ProgressBar _progressBar;
        private Label _progressBarLabel;

        private bool _isProcessing;

        public ModelMaterialMapperWindow(Editor editor)
            : base(editor, true, ScrollBars.Vertical)
        {
            Title = "Model Material Mapper";
            AutoFocus = true;

            _settings = LoadSettings<Settings>();

            CreateUI(editor);
            CalculateState();
        }

        private void CreateUI(Editor editor)
        {
            _presenter = new CustomEditorPresenter(editor.Undo, null, this);
            _presenter.Panel.Parent = this;
            _presenter.Select(_settings);

            _presenter.Modified += () => {
                SaveSettings(_settings);
                CalculateState();
            };

            var container = AddChild<VerticalPanel>();
            container.AnchorPreset = AnchorPresets.StretchAll;
            container.Height = 400;
            container.Location = _presenter.Panel.BottomLeft;

            _processButton = container.AddChild<Button>();
            _processButton.Text = "Process";
            _processButton.Clicked += StartProcessing;

            _progressBar = container.AddChild<ProgressBar>();
            _progressBar.Visible = false;
            _progressBarLabel = _progressBar.AddChild<Label>();
            _progressBarLabel.AnchorPreset = AnchorPresets.StretchAll;
        }

        private void StartProcessing()
        {
            if (_isProcessing) {
                return;
            }

            CalculateState();
            
            var assetItems = CollectFiles();

            _progressBar.Visible = true;
            _progressBarLabel.Text = "Processing...";
            _progressBar.Maximum = assetItems.Count;

            var tasks = assetItems.Select(a => Task.Run(() => FixModel(a))).ToArray();
            Task.WhenAll(tasks).GetAwaiter().OnCompleted(CompleteProcessing);
        }

        private void CompleteProcessing()
        {
            _progressBar.Value = 0;
            _progressBarLabel.Text = "Done!";
            _isProcessing = false;

            CalculateState();
        }

        private List<AssetItem> CollectFiles()
        {
            var assets = new List<AssetItem>();
            var contentItem = Editor.Instance.ContentDatabase.Find(_settings.SourceFiles);

            if (null == contentItem) {
                return assets;
            }

            if (contentItem.IsAsset) {
                assets.Add((AssetItem)contentItem);
            } else if (contentItem.IsFolder) {
                FindModels(((ContentFolder) contentItem).Children, assets);
            }

            return assets;
        }

        private void FixModel(AssetItem assetItem)
        {
            var model = Content.Load<Model>(assetItem.ID);
            var searchRoot = Editor.Instance.ContentDatabase.Find(_settings.MaterialSearchRoot) as ContentFolder;

            if (model != null && searchRoot != null) {
                for (var i = 0; i < model.MaterialSlots.Length; i++) {
                    model.MaterialSlots[i].Material = FindMaterial(searchRoot.Children, model.MaterialSlots[i].Name);
                }

                model.Save();
            }

            Scripting.RunOnUpdate(TickProgressBar).Wait();
        }

        private void CalculateState()
        {
            var sourceFilesExists = Editor.Instance.ContentDatabase.Find(_settings.SourceFiles) != null;
            var searchRootExists = Editor.Instance.ContentDatabase.Find(_settings.MaterialSearchRoot) != null;

            _processButton.Enabled = sourceFilesExists && searchRootExists && ! _isProcessing;
            _presenter.Panel.Enabled = ! _isProcessing;
        }

        private void FindModels(List<ContentItem> items, List<AssetItem> matches)
        {
            Find(items, matches, a => a.TypeName == "FlaxEngine.Model");
        }

        private MaterialBase FindMaterial(List<ContentItem> items, string name)
        {
            var matches = new List<AssetItem>();

            Find(items, matches, a => (a.TypeName == "FlaxEngine.Material" || a.TypeName == "FlaxEngine.MaterialInstance") && a.ShortName == name);

            if (matches.Count == 0) {
                return null;
            }

            return Content.Load<MaterialBase>(matches[0].ID);
        }

        private void Find(List<ContentItem> items, List<AssetItem> matches, MatcherDelegate matcher)
        {
            foreach (var contentItem in items) {
                if (contentItem.IsAsset) {
                    var asset = contentItem as AssetItem;

                    if (matcher(asset)) {
                        matches.Add(asset);
                    }
                } else if (contentItem.IsFolder) {
                    Find(((ContentFolder)contentItem).Children, matches, matcher);
                }
            }
        }

        private delegate bool MatcherDelegate(AssetItem assetItem);

        private void TickProgressBar()
        {
            _progressBar.Value++;
            _progressBarLabel.Text = $"Processing...   {_progressBar.Value}/{_progressBar.Maximum}";
        }
    }
}
