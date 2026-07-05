using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.CoreSystem
{
    public class Core : MonoBehaviour
    // , IBind<EntityData>
    {
        // public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        // public EntityData data;
        [field: SerializeField] public GameObject Root { get; private set; }
        public readonly List<CoreComponents> components = new List<CoreComponents>();

        private void Awake()
        {
            Root = Root ? Root : transform.parent.gameObject;
        }


        public void LogicUpdate()
        {
            foreach (CoreComponents component in components)
            {
                component.LogicUpdate();
            }
        }

        public void AddComponent(CoreComponents component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
            }
        }

        public T GetCoreComponent<T>() where T : CoreComponents
        {
            var comp = components.OfType<T>().FirstOrDefault();

            if (comp) return comp;
            comp = GetComponentInChildren<T>();
            if (comp) return comp;
            Debug.LogWarning($"Component of type {typeof(T)} not found in {transform.parent.name}");
            return null;
        }

        // public void Bind(EntityData data)
        // {
        //     this.data = data;
        //     this.data.Id = Id;
        //     Root.transform.position = data.position;
        // }
    }
}