using UnityEngine;
using VContainer;

public interface IModuleInstaller
{
    void Register(IContainerBuilder builders);
}