using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.MVP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityServiceLocator;
using Utils.Extensions;
using VitalRouter;

[Routes]
public partial class ObjectPreviewMenu : UIView, IMenu<UIContainer>
{
    [SerializeField] private TextMeshProUGUI objectNameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private Image backdrop;
    [SerializeField] private GameObject container;
    
    private Router router;
    private ItemSO item;

    public void Setup(UIContainer uiContainer)
    {
        container.SetActive(false);
        ServiceLocator.For(this).Get<Router>(out router);
        MapTo(router);
    }
    private void OnDestroy() => UnmapRoutes();
    [Route]
    private void On(ItemInteractionStartedCommand cmd)
    {
        item = cmd.ItemData;
        Show();
    }

    [Route]
    private void On(ItemInteractionEndedCommand cmd)
    {
        Hide();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        _ = ShowAsync();
    }

    private async UniTask ShowAsync()
    {
        await backdrop.DOFade(0, .4f);

        objectNameTxt.text = item.displayName;
        descriptionTxt.text = item.description;
        container.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        // _ = HideAsync();
    }
}