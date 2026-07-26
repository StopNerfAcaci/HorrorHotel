using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityServiceLocator;
using VitalRouter;

[Routes]
public partial class ObjectPreviewMenu : BaseUIMenu
{
    [SerializeField] private TextMeshProUGUI objectNameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private Image backdrop;
    [SerializeField] private GameObject container;
    
    private Router router;
    private ItemSO item;
    private UIManager uIManager;

    public override void Setup(UIManager uiManager)
    {
        container.SetActive(false);
        this.uIManager = uiManager;
        ServiceLocator.For(this).Get<Router>(out router);
        MapTo(router);
    }
    private void OnDestroy() => UnmapRoutes();
    [Route]
    private void On(ItemInteractionStartedCommand cmd)
    {
        Debug.Log("Got item:" + cmd.ItemData);
        item = cmd.ItemData;
    }
    
    public void UpdatePreview(ItemSO item)
    {
        this.item = item;
    }

    public override void Show()
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

    public override void Hide()
    {
        gameObject.SetActive(false);
        // _ = HideAsync();
    }

    // private async UniTask HideAsync()
    // {
    //     
    // }
}