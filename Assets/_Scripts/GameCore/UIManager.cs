using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Button exitButton;
    
    private GameplayManager gameplayManager;
    private void Awake()
    {
        gameplayManager = FindAnyObjectByType<GameplayManager>();
        exitButton.onClick.RemoveAllListeners();
        
        exitButton.onClick.AddListener(Application.Quit);
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
