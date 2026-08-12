using UnityEngine;

public interface IInteractable
{
    bool CanPerform();
    void Interact(InteractContext ctx);
    void SetHighlighted(bool on);
}

public interface IEnvironment : IInteractable
{
    float Delay { get; }
}

public struct InteractContext
{
    public IVisitor Source;
    public Transform NewTransform;
    public Vector3 Offset;
}