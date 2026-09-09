using System;
using Gameplay.CoreSystem;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour, IInteractable
{
    [Serializable]
    public class NPCEvents
    {
        public UnityEvent onSelect = new UnityEvent();

        public UnityEvent onDeselect = new UnityEvent();

        public UnityEvent onUse = new UnityEvent();
    }
    
    public NPCEvents events;
    public Core Core { get; private set; }
    private DialogueReader dialogueReader;
    public DialogueReader DialogueReader
    {
        get => dialogueReader ??= Core.GetCoreComponent<DialogueReader>();
    }

    public Transform Transform { get; }
    public string PlayerAnimName { get; }

    private int _savedConversationId;
    public int SavedConversationId => _savedConversationId;

    private bool _isBusy;
    
    public delegate void NpcDelegate(NPC npc); 
    public event NpcDelegate disabled = delegate { };
    private void Awake()
    {
        SetupCore();
    }
    private void SetupCore()
    {
        Core = GetComponentInChildren<Core>();
    }
    protected virtual void OnDisable()
    {
        this.disabled(this);
    }

    public bool CanInteract() => !_isBusy;

    public void Interact(InteractContext ctx)
    {
    }

    public void OnSelectUsable()
    {
        if (events != null && events.onSelect != null)
        {
            events.onSelect.Invoke();
        }
    }

    public void OnDeselectUsable()
    {
        if (events != null && events.onDeselect != null)
        {
            events.onDeselect.Invoke();
        }
    }

    public LineData GetDialogueDataThroughId()
    {
        return DialogueReader.BeginDialogue(_savedConversationId);
    }
    
}