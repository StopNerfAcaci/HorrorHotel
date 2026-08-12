using UnityEngine;
using VitalRouter;

public enum PopupType
{
    NotDoneProgress,
    Settings
}

public readonly struct PopupCommand : ICommand
{
    public readonly PopupType type;

    public PopupCommand(PopupType type)
    {
        this.type = type;
    }
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIContainer container;
    private GameplayManager gameplayManager;
    public GameplayManager GameplayManager => gameplayManager;

    private DialogueController dialogueController;
    public DialogueController DialogueController => dialogueController;

    private void Awake()
    {
        gameplayManager = FindAnyObjectByType<GameplayManager>();
        dialogueController = new DialogueController();
        container?.Setup(this);
    }
}