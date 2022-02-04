using System.IO;
using FlaxEditor;
using FlaxEditor.CustomEditors;
using FlaxEngine;
using FlaxEngine.GUI;
using UE4ContentImporterEditor.UI.CustomEditors;

namespace UE4ContentImporterEditor.UI
{
    public class ConverterWindow : BaseWindow
    {
        public class Settings
        {
            [CustomEditor(typeof(T3DFileOrDirectoryEditor)), EditorDisplay("Content")]
            public string SourceFiles { get; set; }

            [CustomEditor(typeof(DirectoryEditor)), EditorDisplay("Content")]
            public string ContentRoot { get; set; }

            [EditorDisplay("File Types", "Process Materials")]
            public bool ShouldConvertMaterials { get; set; } = true;

            [EditorDisplay("File Types", "Process Material Instances")]
            public bool ShouldConvertMaterialInstances { get; set; } = true;

            [EditorDisplay("File Types", "Process Maps")]
            public bool ShouldConvertMaps { get; set; } = true;
        }

        protected override string SettingsPrefix => "UE4ContentImporterEditor";

        private WorkingSet _currentWorkingSet;
        private Settings _settings;

        private CustomEditorPresenter _presenter;
        private Button _processButton;

        private ProgressBar _progressBar;
        private Label _progressBarLabel;

        private string _stageLabel;

        public ConverterWindow(Editor editor)
            : base(editor, true, ScrollBars.Vertical)
        {
            Title = "UE4 Content Importer";
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
            if (null != _currentWorkingSet) {
                return;
            }

            string[] files = null;

            if (Directory.Exists(_settings.SourceFiles)) {
                files = Directory.GetFiles(_settings.SourceFiles, "*.t3d", SearchOption.AllDirectories);
            } else {
                files = new[] {
                    _settings.SourceFiles
                };
            }

            _currentWorkingSet = new WorkingSet(
                files,
                _settings.ContentRoot,
                Globals.ProjectContentFolder,
                _settings.ShouldConvertMaterials,
                _settings.ShouldConvertMaterialInstances,
                _settings.ShouldConvertMaps
            );

            _currentWorkingSet.StageChange += args => {
                if (args.ItemCount == 0) {
                    return;
                }

                _stageLabel = args.Stage;

                _progressBar.Visible = true;
                _progressBarLabel.Text = _stageLabel;
                _progressBar.Value = 0;
                _progressBar.Maximum = args.ItemCount;
            };

            _currentWorkingSet.Step += args => {
                _progressBar.Value++;
                _progressBarLabel.Text = $"{_stageLabel}...   {_progressBar.Value}/{_progressBar.Maximum}";
            };
            
            CalculateState();

            _currentWorkingSet.Execute(CompleteProcessing);
        }

        private void CompleteProcessing(ResultSet resultSet)
        {
            foreach (var problem in resultSet.Problems) {
                if (problem.Severity == ProblemSeverity.Fatal) {
                    Editor.LogError($"{problem.Message}\nin {problem.FileName}");
                } else if (problem.Severity == ProblemSeverity.Warning) {
                    Editor.LogWarning($"{problem.Message}\nin {problem.FileName}");
                }
            }

            _progressBar.Value = 0;
            _progressBarLabel.Text = "Done!";
            _currentWorkingSet = null;

            CalculateState();
        }

        private void CalculateState()
        {
            bool isFile = File.Exists(_settings.SourceFiles);
            bool isDirectory = Directory.Exists(_settings.SourceFiles);
            bool isAlreadyProcessing = _currentWorkingSet != null;

            _processButton.Enabled = (isFile || isDirectory) && ! isAlreadyProcessing;
            _presenter.Panel.Enabled = ! isAlreadyProcessing;
        }
    }
}
