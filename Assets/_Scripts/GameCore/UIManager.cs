using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    
    private GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = FindAnyObjectByType<GameplayManager>();
    }

    private void OnEnable()
    {
        gameplayManager.OnDayPhaseChanged += UpdateText;
    }

    private void OnDisable()
    {
        gameplayManager.OnDayPhaseChanged -= UpdateText;
    }

    private void UpdateText(string res)
    {
        Debug.Log(res);
        dayText.text = res;
    }
}
