using GameCore.MVP;

public interface IMenu<TScreen> where TScreen : UIView
{
    void Setup(TScreen owner);
    
    void Show();
    void Hide();
}