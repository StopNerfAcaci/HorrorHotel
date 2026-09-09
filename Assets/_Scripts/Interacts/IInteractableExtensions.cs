using UnityEngine;

public static class IInteractableExtensions
{
    public static Vector3 GetInteractableCenter(this IInteractable interactable, Collider fallback)
    {
        var root = (interactable as Component)?.transform;
        var col = root != null ? root.GetComponent<Collider>() : null;
        return col != null ? col.bounds.center : fallback.bounds.center;
    }

}