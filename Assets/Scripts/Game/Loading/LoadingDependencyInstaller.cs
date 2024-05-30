using VContainer;
using VContainer.Unity;

public class LoadingDependencyInstaller : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<LoadingView>();
    }
}
