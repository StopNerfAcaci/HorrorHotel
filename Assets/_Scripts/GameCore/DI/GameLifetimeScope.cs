using Managers;
using Managers.FSM;
using VContainer;
using VContainer.Unity;
using VitalRouter.VContainer;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<HomeManager>();
        // --- Game States ---
        builder.Register<HomeState>(Lifetime.Singleton);
        builder.Register<LobbyState>(Lifetime.Singleton);
        builder.Register<IngameState>(Lifetime.Singleton);
        builder.Register<WinState>(Lifetime.Singleton);
        builder.Register<LoseState>(Lifetime.Singleton);

        // --- Game FSM ---
        builder.RegisterVitalRouter(routing =>
        {
            routing.MapEntryPoint<GameFSM>();
        });
    }
}
