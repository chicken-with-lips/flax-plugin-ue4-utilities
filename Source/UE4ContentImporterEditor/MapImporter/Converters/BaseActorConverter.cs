using System;
using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public abstract class BaseActorConverter<T, U> : BaseNodeConverter<T>
        where T : BaseActorNode
        where U : Actor
    {
        public virtual Type GetActorType(T unrealNode)
        {
            return typeof(U);
        }
        
        public override void Convert(Node unrealNode, MapConverter converter, Node parentUnrealNode, Actor parentActor, Scene scene)
        {
            var nodeAsT = unrealNode as T;

            if (nodeAsT == null) {
                // FIXME:
                throw new System.Exception("Unexpected node type");
            }

            var actor = CreateActor(converter, scene, nodeAsT);

            if (null == actor) {
                return;
            }

            foreach (var childNode in unrealNode.Children) {
                var nodeConverter = converter.FindConverterForUnrealNode(childNode);
                nodeConverter?.Convert(childNode, converter, unrealNode, actor, scene);
            }
        }

        protected virtual Actor CreateActor(MapConverter converter, Scene scene, T unrealNode)
        {
            var actor = scene.AddChild(GetActorType(unrealNode));
            actor.Name = unrealNode.ActorLabel;
            actor.StaticFlags = StaticFlags.FullyStatic;

            SetFolder(actor, unrealNode.FolderPath, scene);

            return actor;
        }

        private void SetFolder(Actor actor, string folderPath, Scene scene)
        {
            if (string.IsNullOrEmpty(folderPath)) {
                return;
            }

            var pathParts = folderPath.Split('/');
            var current = scene as Actor;

            foreach (var pathPart in pathParts) {
                var child = current.GetChild(pathPart);

                if (null == child) {
                    current = current.AddChild<EmptyActor>();
                    current.Name = pathPart;
                } else {
                    current = child;
                }
            }

            actor.Parent = current;
        }
    }
}
