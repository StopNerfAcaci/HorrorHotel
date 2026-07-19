using UnityEngine;

namespace UnityServiceLocator
{
    [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
    public class ServiceLocatorSceneBootstrapper: Bootstrapper
    {
        protected override void Bootstrap()
        {
            Container.ConfigureAsScene();
        }   
    }
}