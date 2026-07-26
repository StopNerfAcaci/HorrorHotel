using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IItem : IInteractable
{
    ItemSO Item { get; }
    Transform Transform { get; }
    UniTask Use();
}