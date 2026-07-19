using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace UnityServiceLocator
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator global;
        private static Dictionary<Scene, ServiceLocator> sceneContainers = new();

        readonly ServiceManager services = new();
        const string k_globalServiceLocatorName = "ServiceLocator [Global]";
        const string k_sceneServiceLocatorName = "ServiceLocator [Scene]";
        static List<GameObject> tmpSceneObjects = new();

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (global == this)
            {
                Debug.LogWarning($"ServiceLocator.ConfigureAsGlobal: Already configured as global", this);
                return;
            }
            else if (global != null)
            {
                Debug.LogError("ServiceLocator.ConfigureAsGlobal: Another ServiceGlobal configured as global", this);
            }
            else
            {
                global = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureAsScene()
        {
            Scene scene = gameObject.scene;

            if (sceneContainers.ContainsKey(scene))
            {
                Debug.LogWarning("ServiceLocator.ConfigureAsScene: Already configured in this Scene", this);
                return;
            }

            sceneContainers.Add(scene, this);
        }

        public static ServiceLocator Global
        {
            get
            {
                if (global != null) return global;

                if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return global;
                }

                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();
                return global;
            }
        }

        public static ServiceLocator For(MonoBehaviour mb)
        {
            return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? ForScene(mb) ?? Global;
        }


        public static ServiceLocator ForScene(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            if (sceneContainers.TryGetValue(scene, out var container) && container != mb)
            {
                return container;
            }

            tmpSceneObjects.Clear();
            scene.GetRootGameObjects(tmpSceneObjects);

            foreach (var go in tmpSceneObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
            {
                if (go.TryGetComponent(out ServiceLocatorSceneBootstrapper boostrapper) && boostrapper.Container != mb)
                {
                    boostrapper.BootstrapOnDemand();
                    return boostrapper.Container;
                }
            }

            return Global;
        }

        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;
            if (TryGetNextInHierarchy(out ServiceLocator container))
            {
                container.Get(out service);
                return this;
            }

            throw new ArgumentException($"ServiceLocator.Get: Service of type {typeof(T).FullName} not registered");
        }

        public ServiceLocator Register<T>(T service)
        {
            services.Register(service);
            return this;
        }

        public ServiceLocator Register(Type type, object service)
        {
            services.Register(type, service);
            return this;
        }

        bool TryGetService<T>(out T service) where T : class => services.TryGet(out service);

        bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == global)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForScene(this);
            return true;
        }

        private void OnDestroy()
        {
            if (this == global)
            {
                global = null;
            }else if (sceneContainers.ContainsValue(this))
            {
                sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            global = null;
            sceneContainers = new();
            tmpSceneObjects = new();
        }
#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/Add global")]
        static void AddGlobal()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add scene")]
        static void AddScene()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
        }
    }
#endif
}