using Gameplay.CoreSystem;
using UnityEngine;

namespace Gameplay.CoreSystem
{
    public class GroundChecker : CoreComponents
    {
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;
        
        
        [SerializeField] private bool drawGizmo = true;
        private bool grounded;
        public bool Grounded => grounded;
        
        void Update()
        {
            grounded = CheckGrounded();
        }
        private bool CheckGrounded()
        {
            return Physics.CheckSphere(transform.position, GroundedRadius, GroundLayers);
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo) return;

            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, GroundedRadius);
        }
    }
}
