using Gameplay.Combat;
using Gameplay.CoreSystem;
using Gameplay.Inventory;
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

    public class InteractState : State
    {
        private readonly PlayerStateDriver player;
        private readonly PlayerInventory inventory;
        private bool isAbilityDone;

        public InteractState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            var interact = player.GetComponent<PlayerInteraction>();
            inventory = interact.Inventory;
            inventory.OnItemAdded += HandleItemAdded;
        }

        public override void Dispose()
        {
            base.Dispose();
            inventory.OnItemAdded -= HandleItemAdded;
        }

        private void HandleItemAdded(ItemSO item, int amount)
        {
            isAbilityDone = true;
        }

        protected override void OnEnter()
        {
            UnityEngine.Debug.Log("Interact Enter");
            player.SetBusy(true);
        }

        protected override void OnExit()
        {
            player.SetBusy(false);
        }

        protected override State GetTransition()
        {
            return isAbilityDone ? ((PlayerRoot)Parent.Parent).Locomotion : null;
        }
    }
}