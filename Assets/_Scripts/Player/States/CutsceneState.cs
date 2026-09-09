using Gameplay.CoreSystem;
using Sirenix.Utilities;
using Utils.Helpers;

namespace HSM
{
    public class CutsceneState : State
    {
        private readonly PlayerStateDriver player;
        private AnimationHandler animationHandler;
        private bool isSceneDone = false;
        private IInteractable interactable;
        private float timer;
        private bool isTicking;

        public AnimationHandler AnimationHandler => animationHandler ?? core.GetCoreComponent<AnimationHandler>();

        public CutsceneState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            core = player.Core;
        }

        protected override void OnEnter()
        {
            isSceneDone = false;
            interactable = player.GetInteractable<CutScene>();
            if (interactable != null && !interactable.PlayerAnimName.IsNullOrWhitespace())
            {
                AnimationHandler.SmoothChangeAnim(interactable.PlayerAnimName);
                timer = 2f;
                isTicking = true;
            }
        }

        protected override void OnExit()
        {
            AnimationHandler.StopAnim();
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (!isTicking) return;
            timer -= deltaTime;
            if (timer <= 0)
            {
                isSceneDone = true;
                isTicking = false;
            }
        }

        protected override State GetTransition() => isSceneDone ? ((PlayerRoot)Parent.Parent).Locomotion : null;
    }
}