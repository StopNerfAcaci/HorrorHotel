using Cysharp.Threading.Tasks;

public interface IItem : IInteractable
{
    ItemSO Item { get; }
    UniTask Confirm();
}