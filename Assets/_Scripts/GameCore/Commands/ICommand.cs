using System.Text;
using UnityEngine;
using VitalRouter;

public readonly struct ItemInteractionStartedCommand : ICommand
{
    public readonly ItemSO ItemData;
    public ItemInteractionStartedCommand(ItemSO itemData) => ItemData = itemData;
}

public readonly struct ItemInteractionEndedCommand : ICommand { }

public readonly struct DialogueDisplayCommand : ICommand
{
    public readonly int id;
    public DialogueDisplayCommand(int id) => this.id = id;
}

public readonly struct DialogueConfigCommand : ICommand
{
    public readonly DialogueConfig Config;
    public DialogueConfigCommand(DialogueConfig config) => Config = config;
}