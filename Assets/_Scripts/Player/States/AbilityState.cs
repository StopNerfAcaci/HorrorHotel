using Gameplay.CoreSystem;

namespace HSM
{
    public class AbilityState : State
    {
        private readonly PlayerStateDriver player;
        private readonly InteractState InteractState;
        public readonly CutsceneState CutsceneState;
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
            CutsceneState = new CutsceneState(machine, this, player);
        }

        protected override void OnEnter()
        {
            isAbilityDone = false;
            Movement.SetVelocityZero();
        }

        protected override State GetInitialState()
        {
            var root = (PlayerRoot)Parent;
            return root.PendingInteractable is IItem ? InteractState : CutsceneState;
        }

        protected override State GetTransition() => isAbilityDone ? ((PlayerRoot)Parent).Locomotion : null;
    }
}