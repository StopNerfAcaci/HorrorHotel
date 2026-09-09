using System;
using UnityEngine;
using UnityEngine.Events;
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
    public event UnityAction OnStartGame;
    [SerializeField] private MaingameScreen maingameScreen;
    [SerializeField] private HomeScreen homeScreen;
    
    private void Start()
    {
        maingameScreen?.Setup(this);
        homeScreen?.Setup(this);
    }

    private void OnEnable()
    {
        if(homeScreen)
        {
            homeScreen.OnStartGame += ShowIngameScreen;
        }
    }

    private void OnDisable()
    {
        if (homeScreen)
        {
            homeScreen.OnStartGame -= ShowIngameScreen;
        }
    }

    public void ShowIngameScreen()
    {
        homeScreen.Hide();
        maingameScreen.Show();
        OnStartGame?.Invoke();
    }
}