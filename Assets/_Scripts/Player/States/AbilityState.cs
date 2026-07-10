using Gameplay.Combat;
using Gameplay.CoreSystem;
using Utils.Commons;
using Utils.Helpers;

namespace HSM
{
    public class AbilityState : State
    {
        private readonly PlayerStateDriver player;
        private readonly InteractState InteractState;
        protected bool isAbilityDone = true;

                
        private Movement movement;
        
        public Movement Movement
        {
            get => movement ??= core.GetCoreComponent<Movement>();
        }
        public AbilityState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            core = player.Core;
            InteractState = new InteractState(machine, this, player);
        }

        protected override void OnEnter()
        {
            isAbilityDone = false;
            Movement.SetVelocityZero();
        }

        protected override State GetInitialState() => InteractState;
        protected override State GetTransition() => isAbilityDone ? ((PlayerRoot)Parent).Locomotion : null;
    }
}