using UnityEngine;
using VitalRouter;

public readonly struct ItemInteractionStartedCommand : ICommand
{
    public readonly ItemSO ItemData;
    public ItemInteractionStartedCommand(ItemSO itemData) => ItemData = itemData;
}

public readonly struct ItemInteractionEndedCommand : ICommand { }
