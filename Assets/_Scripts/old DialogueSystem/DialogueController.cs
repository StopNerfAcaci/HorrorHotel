using System;
using Horror.Events;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Utils.Extensions;

public delegate bool GetInputButtonDownDelegate(string buttonName);
public class DialogueController : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;
    
    private void Awake()
    {
        dialogueText.text = "";
        dialogueText.SetActive(false);
    }

    private void OnEnable()
    {
        EventBus.Register<DialogueEventData>(Play);
        Interaction.onSelectedUsable += Play;
    }

    private void OnDisable()
    {
        EventBus.InRegister<DialogueEventData>(Play);
        Interaction.onSelectedUsable -= Play;
    }

    private void Play(DialogueEventData data)
    {
        Play(data.NPC);
    }

    public void Play(NPC npc)
    {
        ResetText();
        dialogueText.SetActive(true);
        LineData data = npc.GetDialogueDataThroughId();
        dialogueText.text = data != null ? data.Text : "Yo";
    }
    private void ResetText()
    {
        dialogueText.text = "";
    }
    
    public static bool IsDialogueSystemInputDisabled()
    {
        return false;
    }
}