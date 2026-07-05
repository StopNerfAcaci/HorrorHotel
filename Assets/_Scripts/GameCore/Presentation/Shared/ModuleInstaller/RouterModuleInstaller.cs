using VContainer;
using VitalRouter;

namespace GameCore.DI.ModuleInstaller
{
    public class RouterModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(new Router())
                .AsImplementedInterfaces();
        }
    }
}