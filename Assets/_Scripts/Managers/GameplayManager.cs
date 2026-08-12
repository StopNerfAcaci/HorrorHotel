using System;
using System.Collections.Generic;
using GlobalSettings;
using HSM;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;

[DefaultExecutionOrder(-1)]
public class GameplayManager : MonoBehaviour
{
    public event Action<string> OnDayPhaseChanged;

    [SerializeField] private CameraController camController;
    [SerializeField] private GlobalSettings.Gameplay gameplay;
    [SerializeField] private PlayerStateDriver player;
    public PlayerStateDriver Player => player;

    private DayPhase currentDayPhase;
    private int currentDayIndex = -1;

    public GameStateMachine FSM { get; set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (player == null)  player = FindAnyObjectByType<PlayerStateDriver>(FindObjectsInactive.Include);
    }
#endif
    private void Awake()
    {
        Inventory.Get().LoadInventory();

        ServiceLocator.Global.Register<GameplayManager>(this);

        if (gameplay.TryLoadDay(out currentDayPhase))
        {
            currentDayIndex = FindIndex(currentDayPhase);
        }
    }

    private void Start()
    {
        if (currentDayIndex < 0)
        {
            currentDayIndex = 0;
            currentDayPhase = gameplay.Days[0];
            gameplay.SaveDay(currentDayPhase);
        }

        FSM = new GameStateMachine(this);
        OnDayPhaseChanged?.Invoke(currentDayPhase.GetDayString());
    }

    private int FindIndex(DayPhase phase)
    {
        var days = gameplay.Days;
        for (int i = 0; i < days.Length; i++)
        {
            if (days[i].day == phase.day && days[i].isDaytime == phase.isDaytime)
                return i;
        }

        return -1;
    }

    public bool CanMoveNextPhase()
    {
        foreach (var item in currentDayPhase.requireItems)
        {
            if (!Inventory.Get().CheckHasKey(item)) return false;
        }

        return true;
    }

    public void HandleNextPhase()
    {
        currentDayIndex++;
        if (currentDayIndex >= gameplay.Days.Length)
        {
            Debug.Log("Reached end of days.");
            return;
        }

        currentDayPhase = gameplay.Days[currentDayIndex];
        gameplay.SaveDay(currentDayPhase);
        OnDayPhaseChanged?.Invoke(currentDayPhase.GetDayString());
    }

    [Button]
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void SwitchToPlayerCam()
    {
        camController.SwitchCam(true);
    }
}

public struct ChangeStateCommand : ICommand
{
    public GameStateType StateType { get; }
    public ChangeStateCommand(GameStateType stateType) => StateType = stateType;
}