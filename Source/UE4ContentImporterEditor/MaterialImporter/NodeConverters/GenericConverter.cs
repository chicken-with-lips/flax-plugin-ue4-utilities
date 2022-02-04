using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters
{
    public abstract class GenericConverter<T> : BaseNodeConverter<T>
        where T :  MaterialNode
    {
        public override void Convert(MaterialNode unrealNode, MaterialConverter converter)
        {
            var nodeAsT = unrealNode as T;

            if (nodeAsT == null) {
                // FIXME:
                throw new System.Exception("Unexpected node type");
            }

            var node = CreateNode(converter, nodeAsT);
            converter.TrackNode(node, unrealNode);
        }

        protected abstract SurfaceNode CreateNode(MaterialConverter converter, T unrealNode);
    }
}
