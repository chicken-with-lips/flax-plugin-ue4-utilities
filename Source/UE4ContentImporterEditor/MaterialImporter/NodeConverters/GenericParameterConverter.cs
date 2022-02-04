using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters
{
    public abstract class GenericParameterConverter<U> : BaseNodeConverter<U>
        where U : ParameterNode
    {
        public abstract uint NodeTypeId { get; }

        public sealed override void Convert(MaterialNode unrealNode, MaterialConverter converter)
        {
            var parameterNode = unrealNode as U;

            if (parameterNode == null) {
                // FIXME:
                throw new System.Exception("Unexpected node type");
            }

            var surfaceParameter = CreateSurfaceParameter(parameterNode, converter);
            var graphNode = CreateNodeForSurfaceParameter(surfaceParameter, converter, (U)unrealNode);

            converter.TrackNode(graphNode, unrealNode);
        }

        protected virtual SurfaceNode CreateNodeForSurfaceParameter(SurfaceParameter surfaceParameter, MaterialConverter converter, U unrealNode)
        {
            var node = converter.SpawnNode(Helper.FindParameterNodeArchetype(NodeTypeId), Helper.EditorPosition(unrealNode));

            node.SetValues(new[] {
                (object)surfaceParameter.ID
            }, false);

            return node;
        }

        protected abstract SurfaceParameter CreateSurfaceParameter(U parameterNode, MaterialConverter converter);
    }
}
