using GameCore.MVP;
using GlobalSettings;
using UnityEngine;
using UnityEngine.Events;

public class HomeScreen : UIView
{
    [SerializeField] private GameObject mainMenuContainer;
    [SerializeField] SettingMenu settingMenu;
    [SerializeField] HomeButton startButton;
    [SerializeField] HomeButton settingButton;
    [SerializeField] HomeButton creditButton;
    [SerializeField] HomeButton quitButton;
    public UnityAction OnStartGame;

    public void Setup(UIManager owner)
    {
        startButton.Setup(StartGame);
        settingButton.Setup(OpenSetting);
        creditButton.Setup(OpenCredit);
        quitButton.Setup(GameManager.Get().QuitLevel);
        mainMenuContainer.SetActive(true);
        settingMenu.Setup(owner);
        settingMenu.Hide();
    }

    private void StartGame()
    {
        OnStartGame?.Invoke();
    }

    void OpenSetting()
    {
        settingMenu.Show();
        mainMenuContainer.SetActive(false);
    }

    void OpenCredit()
    {
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
}