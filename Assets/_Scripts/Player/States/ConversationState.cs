using VitalRouter;

namespace HSM
{
    public class ConversationState: State
    {
        public readonly PlayerStateDriver player;
        private int currentConversationIndex;
        public ConversationState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            currentConversationIndex = 0;
        }

        protected override void OnEnter()
        {
            player.Reader.Click += HandleDialogue;
        }

        protected override void OnExit()
        {
            player.Reader.Click -= HandleDialogue;
        }

        private void HandleDialogue()
        {
            currentConversationIndex++;
        }
    }
}