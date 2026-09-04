using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMenu: BaseUIMenu
{
    
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Button exitButton;
    
    
    private UIManager uiManager;
    public override void Setup(UIManager uiManager)
    {
        this.uiManager = uiManager;
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(Application.Quit);
    }

    private void OnEnable()
    {
        uiManager.GameplayManager.OnDayPhaseChanged += UpdateText;
    }

    private void OnDisable()
    {
        uiManager.GameplayManager.OnDayPhaseChanged -= UpdateText;
    }

    private void UpdateText(string res)
    {
        dayText.text = res;
    }


}