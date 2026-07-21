using Utils.Helpers;

namespace HSM
{
    public class CutsceneState : State
    {
        private readonly PlayerStateDriver player;
        
        private bool isSceneDone = false;
        private IEnvironment environment;
        private float timer;
        private bool isTicking;
        public CutsceneState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
        }

        protected override void OnEnter()
        {
            isSceneDone = false;
            var root = (PlayerRoot)Parent.Parent;
            environment = root.PendingInteractable as IEnvironment;
            root.PendingInteractable = null;
            timer = environment.Delay;
            isTicking = true;
        }

        protected override void OnExit()
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            if(!isTicking) return;
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