using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public abstract class BaseNodeConverter<T> : BaseNodeConverter
        where T : Node
    {
        public override bool CanConvert(Node unrealNode)
        {
            return unrealNode is T;
        }
    }

    public abstract class BaseNodeConverter
    {
        public abstract bool CanConvert(Node unrealNode);
        public abstract void Convert(Node unrealNode, MapConverter converter, Node parentUnrealNode, Actor parentActor, Scene scene);
    }
}
