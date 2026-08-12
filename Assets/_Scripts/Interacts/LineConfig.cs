using System;
using UnityEngine;

public enum SoundType
{
    OpenDoor,
    CloseDoor
}
[Serializable]
public class LineConfig
{
    public int dialogueID;
   
    [TextArea(3,5)]public string dialogue;
    public float displayDuration = 0;
    public SoundType sound;
    public bool TryGetDialogue(int id, out DialogueData data)
    {
        if (dialogueID != id)
        {
            data = null;
            return false;
        }
        data = new DialogueData(dialogueID, dialogue, sound, displayDuration);
        return true;
    }
}

[Serializable]
public class DialogueData
{
    public int Id { get; }
    public string Text { get; }
    public SoundType Sound { get; }
    public float DisplayDuration { get; }

    public DialogueData(int idAnswer, string text, SoundType sound, float displayDuration)
    {
        Id = idAnswer;
        Text = text;
        Sound = sound;
        DisplayDuration = displayDuration;
    }
}