using UnityEngine;

namespace Gameplay.Combat
{
    public interface IWeapon
    {
        void Enter();
        void Exit();
        bool CanPerform(Collider other);
        void Perform(Collider other);
    }
}