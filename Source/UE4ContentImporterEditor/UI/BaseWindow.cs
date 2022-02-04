using System.Reflection;
using FlaxEditor;
using FlaxEditor.Windows;
using FlaxEngine.GUI;

namespace UE4ContentImporterEditor.UI
{
    public abstract class BaseWindow : EditorWindow
    {
        protected virtual string SettingsPrefix { get; }

        protected BaseWindow(Editor editor, bool hideOnClose, ScrollBars scrollBars)
            : base(editor, hideOnClose, scrollBars)
        {
        }

        protected T LoadSettings<T>()
            where T : new()
        {
            var settings = new T();

            foreach (var propertyInfo in settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                string dataKey = $"{SettingsPrefix}[{propertyInfo.Name}]";

                if (Editor.ProjectCache.HasCustomData(dataKey)) {
                    object dataValue = Editor.ProjectCache.GetCustomData(dataKey);

                    if (propertyInfo.PropertyType == typeof(bool)) {
                        dataValue = bool.Parse(dataValue.ToString());
                    }

                    propertyInfo.SetValue(settings, dataValue);
                }
            }

            return settings;
        }

        protected void SaveSettings<T>(T settings)
            where T : new()
        {
            foreach (var propertyInfo in settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                string dataKey = $"{SettingsPrefix}[{propertyInfo.Name}]";
                string dataValue = propertyInfo.GetValue(settings).ToString();

                Editor.ProjectCache.SetCustomData(dataKey, dataValue);
            }
        }
    }
}
