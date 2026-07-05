using UnityEngine;
using UnityEngine.Serialization;
using Utils.Extensions;

namespace Gameplay.Combat
{
    public class WeaponView : MonoBehaviour
    {
        // [SerializeField] private WeaponDataSO data;

        private IWeapon weapon;

        public void Initialize()
        {
            AttachTriggerRelays();
        }

        public void Enter() => weapon.Enter();
        public void Exit() => weapon.Exit();

        public void HandleTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hitable") && weapon.CanPerform(other))
            {
                Debug.Log("Hit "+ other.name);
                weapon.Perform(other);
            }
        }

        private void AttachTriggerRelays()
        {
            Collider childCollider = GetComponentInChildren<Collider>(true);
            if (!childCollider.isTrigger)
                return;

            WeaponTriggerRelay relay = childCollider.GetOrAddComponent<WeaponTriggerRelay>();
            relay.Initialize(this);
            weapon = new Sword(relay.Col);
            
        }

    }
}