using Gameplay.CoreSystem;
using R3;
using UnityEngine;
using Utils.Helpers;

namespace HSM
{
    public class PlayerRoot : State
    {
        public readonly Locomotion Locomotion;
        public readonly AbilityState AbilityState;
        

        private bool attackRequested;
        private DisposableBag _bag;

        public PlayerRoot(StateMachine machine, PlayerStateDriver player) : base(machine, null)
        {
            core = player.Core;
            var data = player.Data;
            Locomotion = new Locomotion(machine, this, player, data);
            AbilityState = new AbilityState(machine, this, player);
            _bag = new DisposableBag();
            // player.AttackCommand.Subscribe(request => OnAttack(request)).AddTo(ref _bag);
        }

        // private void OnAttack(bool request)
        // {
        //     if (ActiveChild == AbilityState)
        //         return;
        //     attackRequested = request;
        // }


        protected override State GetInitialState() => Locomotion;
        protected override State GetTransition()
        {
            if (ActiveChild == AbilityState)
            {
                attackRequested = false;
                return null;
            }
        
            if (!attackRequested)
                return null;
        
            attackRequested = false;
            return AbilityState;
        }

        public override void Dispose()
        {
            base.Dispose();
            _bag.Dispose();
        }
    }

    public class Airborne : State
    {
        private readonly PlayerStateDriver player;
        private readonly PlayerData data;
        private float jumpVelocity;
        private bool jumpRequested;

        private Movement movement;
        private CountdownTimer jumpTimer;
        private float GravityAcceleration => data.Gravity > 0f ? -data.Gravity : data.Gravity;

        public Movement Movement
        {
            get => movement ??= core.GetCoreComponent<Movement>();
        }

        private GroundChecker groundChecker;

        public GroundChecker GroundChecker
        {
            get => groundChecker ??= core.GetCoreComponent<GroundChecker>();
        }

        public Airborne(StateMachine machine, State parent, PlayerStateDriver player, PlayerData data) : base(machine,
            parent)
        {
            this.player = player;
            core = player.Core;
            this.data = data;
            jumpTimer = new CountdownTimer(data.JumpTimeout);
            jumpTimer.OnTimerStart += () => jumpVelocity = Mathf.Sqrt(data.JumpHeight * -2f * GravityAcceleration);
        }

        public void RequestJump()
        {
            jumpRequested = true;
        }

        protected override void OnEnter()
        {
            if (jumpRequested)
            {
                jumpRequested = false;
                jumpTimer.Start();
            }
            else
            {
                jumpTimer.Stop();
                jumpVelocity = Mathf.Min(Movement.CurrentVelocity.y, 0f);
            }
            //Update anim here
            // player.Animator.CrossFade(Constants.JumpHash, .15f);
        }

        protected override void OnExit()
        {
            jumpVelocity = ZeroF;
        }

        protected override State GetTransition() =>
            GroundChecker.Grounded && Movement.CurrentVelocity.y <= 0f ? ((PlayerRoot)Parent).Locomotion : null;

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            jumpTimer.Tick(deltaTime);
        }

        protected override void PhysicsUpdate(float deltaTime)
        {
            if (!jumpTimer.IsRunning)
            {
                jumpVelocity += GravityAcceleration * deltaTime;
            }

            Movement.SetVelocityY(jumpVelocity);
        }
    }
    
}
