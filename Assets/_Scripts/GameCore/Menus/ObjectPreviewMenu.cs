using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.MVP;
using Horror.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class ObjectPreviewMenu : UIView
{
    [SerializeField] private TextMeshProUGUI objectNameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private Image backdrop;
    [SerializeField] private GameObject container;
    
    public void Setup(UIManager manager)
    {
        container.SetActive(false);
        EventBus.Register<HideMenuEventData>(HideMenu);
    }

    private void HideMenu(HideMenuEventData obj)
    {
        Hide();
    }

    public void Inspect(ItemSO itemData)
    {
        objectNameTxt.text = itemData.displayName;
        descriptionTxt.text = itemData.description;
        backdrop.SetActive(true);
        Show();
    }
    
    protected override void OnDestroy()
    {
        EventBus.InRegister<HideMenuEventData>(HideMenu);
    }


    public void Show()
    {
        gameObject.SetActive(true);
        _ = ShowAsync();
    }

    private async UniTask ShowAsync()
    {
        await backdrop.DOFade(0, .4f);
        backdrop.SetActive(false);
        container.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}