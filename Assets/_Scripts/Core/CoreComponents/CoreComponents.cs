using UnityEngine;

namespace Gameplay.CoreSystem
{
    public interface ILogicUpdate
    {
        void LogicUpdate();
    }

    public abstract class CoreComponents : MonoBehaviour, ILogicUpdate
    {
        protected Core core;

        protected virtual void Awake()
        {
            core = transform.parent.GetComponent<Core>();
            
            if (core == null)
            {
                Debug.LogError("There no core in parent");
            }
            core.AddComponent(this);
        }

        public virtual void LogicUpdate()
        {
        }
    }
}