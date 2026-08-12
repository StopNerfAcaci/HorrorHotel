using System.Collections.Generic;
using System.Linq;
using GameCore.MVP;
using UnityEngine;

public class UIContainer : UIView
{
    [SerializeField] private List<GameObject> menuGOs;
    private UIManager uiManager;
    private IMenu<UIContainer>[] menus; 
    
    public void Setup(UIManager manager)
    {
        this.uiManager = manager;
        menus = menuGOs
            .Where(go => go != null)
            .Select(go => go.GetComponent<IMenu<UIContainer>>())
            .Where(m => m != null)
            .ToArray();
        foreach (var m in menus)
        {
            if(m == null) continue;
            m.Setup(this);
        }
    }
}
