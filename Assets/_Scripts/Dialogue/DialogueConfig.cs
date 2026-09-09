using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "new Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueConfig : ScriptableObject
{
    [Serializable]
    public class ConversationData
    {
        public int conversationId;
        public int dialogueIdAfterDoneConversation;
        public LineConfig[] lines;
        private bool _hasConversationDone;
        private int _currentLineId;
        
        [Button]
        public void ResetConversation()
        {
            _currentLineId = 0;
            _hasConversationDone = false;
        }
        public LineData GetLine()
        {
            if (lines == null || lines.Length == 0)
            {
                _hasConversationDone = true;
                return null;
            }

            if (_hasConversationDone)
            {
                // var line = lines[dialogueIdAfterDoneConversation];
                // var data = new LineData(dialogueIdAfterDoneConversation, line.dialogue, line.sound, line.displayDuration);
                // return data;
                return null;
            }
            foreach (var line in lines)
            {
                if (!line.TryGetDialogue(_currentLineId, out var data)) continue;
                _currentLineId++;
                if (_currentLineId >= lines.Length)
                {
                    _hasConversationDone = true;
                }
                return data;
            }

            return null;
        }
    }
    [SerializeField] private ConversationData[] conversationDatas;

    public IReadOnlyList<ConversationData> ConversationDatas => conversationDatas;

    public ConversationData GetConversation(int id)
    {
        foreach (var data in conversationDatas)
        {
            if (data.conversationId == id)
            {
                return data;
            }
        }

        return null;
    }

}
