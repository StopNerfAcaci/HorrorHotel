using UnityEngine;

public abstract class BaseUIMenu : MonoBehaviour, IMenu
{
    public abstract void Setup(UIManager uiManager);

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}