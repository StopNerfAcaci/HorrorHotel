using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IItem : IInteractable
{
    Transform Transform { get; }
    UniTask Use();
}