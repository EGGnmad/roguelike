using VContainer;
using VContainer.Unity;

namespace MapGeneration
{
    public class MapDependencyInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GlobalMap>();
            builder.RegisterComponentInHierarchy<MapGenerator>();
        }
    }
}