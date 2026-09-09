using UnityEngine;

public class SettingMenu : UIPopup
{
    [SerializeField] private HomeButton audioButton;
    [SerializeField] private HomeButton languageButton;

    
    public override void Setup(UIManager owner)
    {
        audioButton.Setup(null);
        languageButton.Setup(null);
    }

    public override void Show() => gameObject.SetActive(true);

    public override void Hide() => gameObject.SetActive(false);
}