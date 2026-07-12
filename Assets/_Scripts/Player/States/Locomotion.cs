using Gameplay.CoreSystem;
using UnityEngine;
using Utils.Commons;

namespace HSM
{
    public class Locomotion : State
    {
        private readonly PlayerStateDriver player;
        private readonly PlayerData data;

        private float currentSpeed;
        private float vel;
        private float targetSpeed;
        private Movement movement;
        private WorldItem pendingItem;

        public Movement Movement
        {
            get => movement ??= core.GetCoreComponent<Movement>();
        }
        private Interaction _interaction;

        public Interaction Interaction
        {
            get => _interaction ?? core.GetCoreComponent<Interaction>();
        }
        private bool clickRequested;

        protected Vector2 MoveInput => player.Reader.Direction;

        public Locomotion(StateMachine machine, State parent, PlayerStateDriver player, PlayerData data) : base(machine,
            parent)
        {
            this.player = player;
            core = player.Core;
            this.data = data;
        }

        protected override void OnEnter()
        {
            clickRequested = false;
            player.Reader.Cancel += OnCancel;

        }

        protected override void OnExit()
        {
            player.Reader.Cancel -= OnCancel;
        }

        private void OnCancel()
        {
            if (Interaction.TryPressed(out var item))
            {
                clickRequested = true;
                pendingItem = item;
            }
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
        
        protected override State GetTransition()
        {
            if (!clickRequested) return null;
            ((PlayerRoot)Parent).PendingItem = pendingItem;
            return ((PlayerRoot)Parent).AbilityState;
        }
        
    }
}
