using Flax.Build;
using Flax.Build.NativeCpp;

public class UE4ContentImporterEditor : GameEditorModule
{
    public override void Init()
    {
        base.Init();

        BuildNativeCode = false;
    }

    /// <inheritdoc />
    public override void Setup(BuildOptions options)
    {
        base.Setup(options);

        options.ScriptingAPI.FileReferences.Add(@"Dependencies\JollySamurai.UnrealEngine4.T3D.dll");
    }
}
