using System;
using UnityEngine;

public enum DialogueValueType
{
    Bool,
    Int,
    Float,
    String
}
[Serializable]
public class GameDialogueState
{
    public DialogueValueType type;
    [SerializeField] private string id;
    public string ID => "GameState." + id;
    public object initialValue;
    public string description;
    
    [HideInInspector]
    public DialogueValueType previousType;

    public void OnTypeChanged()
    {
        if (previousType == type)
            return;

        previousType = type;

        // Reset the newly selected type if desired.
        switch (type)
        {
            case DialogueValueType.Bool:
                initialValue = false;
                break;
            case DialogueValueType.Int:
                initialValue = 0;
                break;
            case DialogueValueType.Float:
                initialValue = 0f;
                break;
            case DialogueValueType.String:
                initialValue = "";
                break;
        }
    }
}