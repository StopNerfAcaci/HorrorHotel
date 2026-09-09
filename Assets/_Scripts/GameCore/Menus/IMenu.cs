using GameCore.MVP;

public interface IMenu
{
    void Setup(UIManager owner);
    
    void Show();
    void Hide();
}