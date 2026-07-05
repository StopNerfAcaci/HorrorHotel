using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Combat
{
    public class Sword : IWeapon
    {
        private readonly Collider collider;
        private HashSet<Collider> hitColliders = new HashSet<Collider>();

        public Sword(Collider collider)
        {
            this.collider = collider;
        }

        public void Enter()
        {
            collider.enabled = true;
            hitColliders.Clear();
        }

        public void Exit()
        {
            collider.enabled = false;
        }

        public bool CanPerform(Collider other) => !hitColliders.Contains(other);


        public void Perform(Collider other)
        {
            hitColliders.Add(other);
        }
    }
}