using System;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;

public class DialogueTrigger : MonoBehaviour, IDialogue
{
    [SerializeField] private bool isBusy;

    private Router router;

    private void Start()
    {
        ServiceLocator.For(this).Get<Router>(out router);
    }

    public bool CanPerform() => !isBusy;

    public void Interact(InteractContext ctx)
    {
        if (!CanPerform()) return;
    }

    public void SetHighlighted(bool on)
    {
    }

    
}