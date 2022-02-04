namespace UE4ContentImporterEditor.UI.CustomEditors
{
    public sealed class DirectoryEditor : FileOrDirectoryEditor
    {
        public override string FileFilter => null;
        public override bool AllowSingleFileSelection => false;
    }
}
