using System;
using GameCore.MVP;
using GlobalSettings;
using UnityEngine;
using UnityServiceLocator;
using VitalRouter;

public class MainMenuScreen : UIView, IMenu<HomeScreen>
{
    [SerializeField] HomeButton startButton;
    [SerializeField] HomeButton settingButton;
    [SerializeField] HomeButton creditButton;
    [SerializeField] HomeButton quitButton;

    private HomeScreen screen;

    public void Setup(HomeScreen screen)
    {
        this.screen = screen;
        Router router = ServiceLocator.Global.Get<Router>();
        startButton.Setup(() =>
        {
            router.PublishAsync(new ChangeStateCommand(GameStateType.InGame));
        });
        settingButton.Setup(OpenSetting);
        creditButton.Setup(OpenCredit);
        quitButton.Setup(GameManager.Get().QuitLevel);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OpenSetting()
    {
        screen.ShowMenu<SettingMenu>();
    }

    void OpenCredit()
    {
    }
}