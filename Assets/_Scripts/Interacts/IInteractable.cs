using UnityEngine;

public interface IInteractable
{
    bool CanPerform();
    void Interact(InteractContext ctx);
}

public interface IEnvironment : IInteractable
{
    float Delay { get; }
}

public struct InteractContext
{
    public Transform NewTransform;
    public Vector3 Offset;
}