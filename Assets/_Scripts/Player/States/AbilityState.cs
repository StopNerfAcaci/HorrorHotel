using Gameplay.Combat;
using Utils.Commons;
using Utils.Helpers;

namespace HSM
{
    public class AbilityState : State
    {
        private readonly AttackState AttackState;
        private readonly PlayerStateDriver player;
        private readonly WeaponView _weaponView;
        
        protected bool isAbilityDone = true;

        public AbilityState(StateMachine machine, State parent, PlayerStateDriver player) : base(
            machine, parent)
        {
            this.player = player;
            // AttackState = new AttackState(machine, this, player, player.WeaponView);
            
        }

        protected override void OnEnter()
        {
            isAbilityDone = false;
        }
        
        protected override State GetInitialState() => AttackState;
        protected override State GetTransition() => isAbilityDone ? ((PlayerRoot)Parent).Locomotion : null;
        
        internal void SetDone(bool isDone) => isAbilityDone = isDone;
    }

    public class AttackState : State
    {
        private readonly PlayerStateDriver player;
        private readonly WeaponView _weaponView;
        
        private bool hasEnteredAttackAnimation;
        public AttackState(StateMachine machine, State parent, PlayerStateDriver player, WeaponView weaponView) : base(machine, parent)
        {
            this.player = player;
            _weaponView = weaponView;
            weaponView.Initialize();
        }

        protected override void OnEnter()
        {
            hasEnteredAttackAnimation = false;
            // player.Animator.CrossFadeInFixedTime(Constants.AttackHash, 0.1f);
            _weaponView.Enter();
        }

        protected override void OnExit()
        {
            _weaponView.Exit();
        }

        // protected override void OnUpdate(float deltaTime)
        // {
        //     if (player.Animator == null || !player.Animator.runtimeAnimatorController)
        //     {
        //         ((AbilityState)Parent).SetDone(true);
        //         return;
        //     }
        //
        //     if (player.Animator.IsInTransition(0))
        //         return;
        //
        //     var stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);
        //     if (stateInfo.shortNameHash != Constants.AttackHash)
        //         return;
        //
        //     hasEnteredAttackAnimation = true;
        //
        //     if (hasEnteredAttackAnimation && stateInfo.normalizedTime >= 1f)
        //         ((AbilityState)Parent).SetDone(true);
        // }
    }

    public class RollState : State
    {
        private readonly PlayerStateDriver player;
        public RollState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
        }
    }
}
