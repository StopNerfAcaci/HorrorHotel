using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private BaseUIMenu[] menus;

    private GameplayManager gameplayManager;
    public GameplayManager GameplayManager => gameplayManager;

    private void Awake()
    {
        gameplayManager = FindAnyObjectByType<GameplayManager>();
        foreach (var menu in menus) menu.Setup(this);
    }

    private void OnEnable()
    {
        Interaction.OnInspectItem += UpdatePreview;
        gameplayManager.OnGameplayStateChanged += UpdateState;
    }

    private void OnDisable()
    {
        Interaction.OnInspectItem -= UpdatePreview;
        gameplayManager.OnGameplayStateChanged -= UpdateState;
    }

    private void UpdatePreview(ItemSO obj)
    {
        
    }

    private void UpdateState(GameplayManager.GameState state)
    {
        switch (state)
        {
            case GameplayManager.GameState.Interact:
                ShowMenu<ObjectPreviewMenu>();
                break;
            case GameplayManager.GameState.Movement:
                ShowMenu<GameplayMenu>();
                break;
        }
    }

    private void ShowMenu<T>() where T : BaseUIMenu
    {
        foreach (var menu in menus)
        {
            if (menu is T)
            {
                menu.Show();
            }
            else
            {
                menu.Hide();
            }
        }
    }
}