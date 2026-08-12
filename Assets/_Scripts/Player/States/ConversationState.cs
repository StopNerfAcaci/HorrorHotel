using UnityServiceLocator;
using VitalRouter;

namespace HSM
{
    public class ConversationState: State
    {
        public readonly PlayerStateDriver player;
        private Router router;
        private int currentConversationIndex;
        public ConversationState(StateMachine machine, State parent, PlayerStateDriver player) : base(machine, parent)
        {
            this.player = player;
            ServiceLocator.For(player).Get<Router>(out router);   
            currentConversationIndex = 0;
        }

        protected override void OnEnter()
        {
            router.PublishAsync(new DialogueDisplayCommand(currentConversationIndex));
            player.Reader.Click += HandleDialogue;
        }

        protected override void OnExit()
        {
            player.Reader.Click -= HandleDialogue;
        }

        private void HandleDialogue()
        {
            currentConversationIndex++;
            router.PublishAsync(new DialogueDisplayCommand(currentConversationIndex));
        }
    }
}