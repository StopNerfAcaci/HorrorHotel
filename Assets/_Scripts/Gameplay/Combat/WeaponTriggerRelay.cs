using UnityEngine;

namespace Gameplay.Combat
{
    [RequireComponent(typeof(Collider))]
    public class WeaponTriggerRelay : MonoBehaviour
    {
        private WeaponView weaponView;
        private Collider col;
        public Collider Col => col;

        public void Initialize(WeaponView owner)
        {
            weaponView = owner;
        }

        private void OnTriggerEnter(Collider other)
        {
            weaponView?.HandleTriggerEnter(other);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (col == null) col = GetComponentInChildren(typeof(Collider)) as Collider;
        }
#endif
    }
}