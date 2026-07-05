using GameCore.DI.ModuleInstaller;
using GameCore.Navigator;
using GameCore.Presentation.Shared;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class AppLifetimeScope : LifetimeScope
{
    [SerializeField] private GameNavigatorLauncher launcher;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterModuleInstaller<RouterModuleInstaller>();
        builder.Register<TransitionService>(Lifetime.Singleton);

        if (launcher != null)
        {
            builder.RegisterComponent(launcher);
        }
        else
        {
            builder.RegisterComponentInHierarchy<GameNavigatorLauncher>();
        }
    }
}
