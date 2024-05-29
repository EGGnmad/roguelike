using VContainer;
using VContainer.Unity;

public class GameDependencyInstaller : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<MapGenerator>();
    }
}