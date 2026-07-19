using System;
using System.Collections.Generic;
using Gameplay.Inventory;
using UnityEngine;

[Serializable]
public struct DayPhase
{
    public int day;
    public bool isDaytime;
    public List<ItemSO> requireItems;
    
    public string GetDayString()
    {
        var dayString = isDaytime ? "Day" : "Night";
        return $"Day {day} - at {dayString}";
    }

}