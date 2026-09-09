using UnityEngine;

namespace Gameplay.CoreSystem
{
    //Each character will hold a dialogue for themself, base on current state will trigger suitable dialogue
    public class DialogueReader : CoreComponents
    {
        [SerializeField] private DialogueConfig config;

        public LineData BeginDialogue(int id)
        {
            var conversation = config.GetConversation(id);
            return conversation.GetLine();
        }
    }
}