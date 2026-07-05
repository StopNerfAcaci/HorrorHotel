using UnityEngine;
using VContainer;
using VContainer.Unity;

public static class DIExtension
{
    public static RegistrationBuilder RegisterSelfAsEntryPoint<T>(
        this IContainerBuilder builder,
        Lifetime lifetime = Lifetime.Singleton)
    {
        return builder.RegisterEntryPoint<T>(lifetime).AsSelf();
    }

    public static void RegisterModuleInstaller<T>(this IContainerBuilder builder)
        where T : class, IModuleInstaller, new()
    {
        var installer = new T();
        installer.Register(builder);
    }
        
    public static void RegisterModuleInstallerPrefab<T>(this IContainerBuilder builder, T prefabInstaller)
        where T : ScriptableObject, IModuleInstaller
    {
        prefabInstaller.Register(builder);
    }
        
    public static void RegisterModuleInstallerComponent<T>(this IContainerBuilder builder, T componentInstaller)
        where T : Component, IModuleInstaller
    {
        componentInstaller.Register(builder);
    }
}