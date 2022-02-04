using FlaxEngine;
using JollySamurai.UnrealEngine4.Import.Map;
using JollySamurai.UnrealEngine4.T3D;
using JollySamurai.UnrealEngine4.T3D.Map;

namespace UE4ContentImporterEditor.MapImporter.Converters
{
    public abstract class BaseComponentConverter<T, U> : BaseNodeConverter<T>
        where T : Node where U : Actor
    {
        public override void Convert(Node unrealNode, MapConverter converter, Node parentUnrealNode, Actor parentActor, Scene scene)
        {
            var nodeAsT = unrealNode as T;

            if (nodeAsT == null) {
                // FIXME:
                throw new System.Exception("Unexpected node type");
            }

            CreateComponent(converter, nodeAsT, parentUnrealNode, parentActor);
        }

        protected virtual void CreateComponent(MapConverter converter, T unrealNode, Node parentUnrealNode, Actor actor)
        {
            var actorAsU = actor as U;

            if (! actorAsU) {
                actorAsU = actor.AddChild<U>();
            }

            ApplyCommonParameters(actorAsU, unrealNode, parentUnrealNode);
            Apply(converter, unrealNode, parentUnrealNode, actorAsU);
        }

        protected virtual void Apply(MapConverter converter, T unrealNode, Node parentUnrealNode, U parentActor)
        {
        }

        protected void ApplyCommonParameters(Actor actor, T unrealNode, Node parentUnrealNode)
        {
            if (parentUnrealNode is BaseActorNode parentUnrealActor) {
                actor.Name = parentUnrealActor.ActorLabel;
            } else {
                actor.Name = parentUnrealNode.Name;
            }

            if (unrealNode is ILocation translatable) {
                actor.LocalPosition = Helper.ConvertUnrealVector3(translatable.Location);
            }

            if (unrealNode is IRotation rotatable) {
                actor.LocalOrientation = Helper.ConvertUnrealRotator(rotatable.Rotation);
            }

            if (unrealNode is IScale3D scalable) {
                actor.Scale = Helper.ConvertUnrealVector3(scalable.Scale3D);
            }

            if (unrealNode is IMobility movable) {
                switch (movable.Mobility) {
                    case Mobility.Movable:
                        actor.StaticFlags = StaticFlags.None;

                        break;
                    case Mobility.Static:
                        actor.StaticFlags = StaticFlags.FullyStatic;

                        break;
                    case Mobility.Stationary:
                        actor.StaticFlags = StaticFlags.Transform | StaticFlags.Navigation | StaticFlags.ReflectionProbe;

                        break;
                }
            }
        }
    }
}
