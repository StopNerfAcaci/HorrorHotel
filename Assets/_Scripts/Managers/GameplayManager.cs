using System;
using System.Collections.Generic;
using GlobalSettings;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityServiceLocator;

public class GameplayManager : MonoBehaviour
{
    public event Action<string> OnDayPhaseChanged;
    
    [SerializeField] private GlobalSettings.Gameplay gameplay;
    private Queue<DayPhase> dayProcesses = new();
    private DayPhase currentDayPhase;
    private int currentDayIndex = -1;
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
            Debug.Log(item.name);
            if(!Inventory.Get().CheckHasKey(item)) return false;
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
}
