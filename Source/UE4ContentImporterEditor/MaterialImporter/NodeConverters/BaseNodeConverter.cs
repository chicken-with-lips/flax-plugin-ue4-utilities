using FlaxEditor.Surface;
using JollySamurai.UnrealEngine4.T3D.Material;
using JollySamurai.UnrealEngine4.T3D.Parser;

namespace UE4ContentImporterEditor.MaterialImporter.NodeConverters
{
    public abstract class BaseNodeConverter<T> : BaseNodeConverter
        where T : MaterialNode
    {
        public virtual void CreateConnections(T unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
        }

        public override void CreateConnections(MaterialNode unrealNode, Material unrealMaterial, MaterialConverter converter)
        {
            var unrealNodeAsT = (T)unrealNode;

            if (unrealNodeAsT != null) {
                CreateConnections(unrealNodeAsT, unrealMaterial, converter);
            }
        }
    }

    public abstract class BaseNodeConverter
    {
        public abstract bool CanConvert(MaterialNode unrealNode);
        public abstract void Convert(MaterialNode unrealNode, MaterialConverter converter);

        public abstract int GetConnectionBoxId(SurfaceNode from, SurfaceNode to, int toBoxId, ParsedPropertyBag propertyBag);

        public abstract void CreateConnections(MaterialNode unrealNode, Material unrealMaterial, MaterialConverter converter);
    }
}
