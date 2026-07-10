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
        private float targetSpeed;
        private Movement movement;


        public Movement Movement
        {
            get => movement ??= core.GetCoreComponent<Movement>();
        }

        protected Vector2 MoveInput => player.Reader.Direction;

        public Locomotion(StateMachine machine, State parent, PlayerStateDriver player, PlayerData data) : base(machine,
            parent)
        {
            this.player = player;
            core = player.Core;
            this.data = data;
        }

        protected override void PhysicsUpdate(float deltaTime)
        {
            
            targetSpeed = player.GetSpeed();
            if (MoveInput == Vector2.zero) targetSpeed = 0.0f;
            Vector3 horizontalVelocity = new Vector3(Movement.CurrentVelocity.x, 0.0f, Movement.CurrentVelocity.z);
            float currentHorizontalSpeed = horizontalVelocity.magnitude;

            float speedOffset = 0.1f;
            
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {

                currentSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * MoveInput.magnitude,
                    Time.deltaTime * data.SpeedChangeRate);

                currentSpeed = Mathf.Round(currentSpeed * 1000f) / 1000f;
            }
            else
            {
                currentSpeed = targetSpeed;
            }
            
            Vector3 inputDirection = new Vector3(MoveInput.x, 0.0f, MoveInput.y).normalized;
            
            if (MoveInput != Vector2.zero)
            {
                inputDirection = player.transform.right * MoveInput.x + player.transform.forward * MoveInput.y;
            }
            Movement.SetVelocityXZ(inputDirection, currentSpeed);
        }

        protected override State GetTransition() => player.HasInteractable ? ((PlayerRoot)Parent).AbilityState : null;
    }
}
