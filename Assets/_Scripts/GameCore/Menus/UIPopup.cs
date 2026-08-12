using GameCore.MVP;
using UnityEngine;
using Utils.Extensions;


public abstract class UIPopup : UIView, IMenu<UIContainer>
{
    [SerializeField] protected CanvasGroup panelGroup;
    [SerializeField] private HomeButton backButton;

    private void OnValidate()
    {
        if (panelGroup == null) panelGroup = this.GetOrAddComponent<CanvasGroup>();
    }
    public virtual void Setup(UIContainer owner)
    {
        panelGroup.alpha = 0f;
        panelGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
        backButton.Setup(null);
    }

    public abstract void Show();
    public abstract void Hide();
}