using UnityEngine;

namespace Utils.Commons
{
    public class Constants
    {
        #region Animation Strings
        
        public static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        public static readonly int JumpStartHash = Animator.StringToHash("JumpStart");
        public static readonly int InAirHash = Animator.StringToHash("InAir");
        public static readonly int DashHash = Animator.StringToHash("Dash");
        public static readonly int AttackHash = Animator.StringToHash("Attack");

        public static readonly int SpeedHash = Animator.StringToHash("Speed");
        public static readonly int MotionSpeedHash = Animator.StringToHash("MotionSpeed");
        public static readonly int JumpHash = Animator.StringToHash("Jump");
        public static readonly int GroundedHash = Animator.StringToHash("Grounded");
        public static readonly int FreeFallHash = Animator.StringToHash("FreeFall");

        #endregion

    }
}
