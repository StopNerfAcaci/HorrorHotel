    using GameCore.MVP;
using UnityEngine;
using Utils.Extensions;

public class HomeScreen : UIView, IMenu<UIContainer>
{
    private IMenu<HomeScreen>[] homeMenus;

    public void Setup(UIContainer owner)
    {
        homeMenus = GetComponentsInChildren<IMenu<HomeScreen>>(true);

        foreach (var menu in homeMenus)
        {
            if (menu == null) continue;
            menu.Setup(this);
        }
        ShowMenu<MainMenuScreen>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    internal void ShowMenu<T>() where T : UIView
    {
        foreach (var menu in homeMenus)
        {
            if (menu is T)
            {
                menu.Show();
            }
            else
            {
                menu.Hide();
            }
        }
    }
}