using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    Transform Transform { get; }
    string PlayerAnimName { get; }
    bool CanInteract();
    void Interact(InteractContext ctx);
}


public struct InteractContext
{
    public IVisitor Source;
    public Transform NewTransform;
    public Vector3 Offset;
}