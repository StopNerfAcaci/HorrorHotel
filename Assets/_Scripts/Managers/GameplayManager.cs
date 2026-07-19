using System;
using System.Collections.Generic;
using Gameplay.Inventory;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public event Action<string> OnDayPhaseChanged;
    
    private Queue<DayPhase> dayProcesses = new();
    private DayPhase currentDayPhase;
    private List<ItemSO> items = new();

    private void Start()
    {
        var days = GlobalSettings.Gameplay.Days;
        foreach (var day in days)
            dayProcesses.Enqueue(day);
        
        NextPhase();
    }
    
    
    public void UpdateDayPhase(ItemSO item)
    {
        items.Remove(item);
        if (items.Count == 0)
        {
            NextPhase();
        }
    }
    
    private void NextPhase()
    {
        currentDayPhase = dayProcesses.Dequeue();
        items = currentDayPhase.requireItems;
        OnDayPhaseChanged?.Invoke(currentDayPhase.GetDayString());
    }

}
