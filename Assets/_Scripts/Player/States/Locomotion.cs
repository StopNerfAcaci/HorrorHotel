using Gameplay.CoreSystem;
using UnityEngine;
using Utils.Commons;

namespace HSM
{
    public class Locomotion : State
    {
        private readonly PlayerStateDriver player;
        private readonly PlayerData data;
        private const float smoothTime = 0.2f;

        private float currentSpeed;
        private float vel;
        private bool jumpPressed;
        private float targetSpeed;
        private Movement movement;


        public Movement Movement
        {
            get => movement ??= core.GetCoreComponent<Movement>();
        }

        private GroundChecker groundChecker;

        public GroundChecker GroundChecker
        {
            get => groundChecker ??= core.GetCoreComponent<GroundChecker>();
        }

        protected Vector2 MoveInput => player.Reader.Direction;
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        public Locomotion(StateMachine machine, State parent, PlayerStateDriver player, PlayerData data) : base(machine,
            parent)
        {
            this.player = player;
            core = player.Core;
            this.data = data;
        }

        protected override void OnEnter()
        {
            jumpPressed = false;
            player.Reader.Jump += OnJump;
            targetSpeed = player.GetSpeed();
            // player.CrossFadeIfReady(Constants.LocomotionHash);
        }

        protected override void OnExit()
        {
            base.OnExit();
            player.Reader.Jump -= OnJump;
        }

        private void OnJump(bool pressed)
        {
            jumpPressed = pressed;
        }

        protected override void PhysicsUpdate(float deltaTime)
        {
            
            targetSpeed = player.GetSpeed();
            if (MoveInput == Vector2.zero) targetSpeed = 0.0f;
            // a reference to the players current horizontal velocity
            Vector3 horizontalVelocity = new Vector3(Movement.CurrentVelocity.x, 0.0f, Movement.CurrentVelocity.z);
            float currentHorizontalSpeed = horizontalVelocity.magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                currentSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * MoveInput.magnitude,
                    Time.deltaTime * data.SpeedChangeRate);

                // round speed to 3 decimal places
                currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;
            }
            else
            {
                currentSpeed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(MoveInput.x, 0.0f, MoveInput.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (MoveInput != Vector2.zero)
            {
                // move
                inputDirection = player.transform.right * MoveInput.x + player.transform.forward * MoveInput.y;
            }
            Movement.SetVelocityXZ(inputDirection, currentSpeed);
            // move the player

            // if (player.Animator != null && player.Animator.runtimeAnimatorController)
            // {
            //     player.Animator.SetFloat(SpeedHash, currentSpeed);
            // }
        }

        public void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref vel, smoothTime);
        }

        protected override State GetTransition()
        {
            if (jumpPressed)
            {
                jumpPressed = false;
                ((PlayerRoot)Parent).Airborne.RequestJump();
                return ((PlayerRoot)Parent).Airborne;
            }

            return GroundChecker.Grounded ? null : ((PlayerRoot)Parent).Airborne;
        }
    }
}
