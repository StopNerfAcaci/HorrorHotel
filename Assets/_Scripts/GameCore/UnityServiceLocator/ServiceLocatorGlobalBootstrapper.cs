using UnityEngine;

namespace UnityServiceLocator
{
    [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
    public class ServiceLocatorGlobalBootstrapper: Bootstrapper
    {
        [SerializeField] bool dontDestroyOnLoad = true;
        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}