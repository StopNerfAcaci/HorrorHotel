using UnityEngine;

public class CutScene : MonoBehaviour, IInteractable
{
    public Transform Transform => transform;
    public string PlayerAnimName { get; }
    public bool CanInteract() => true;

    public void Interact(InteractContext ctx)
    {
        throw new System.NotImplementedException();
    }
}