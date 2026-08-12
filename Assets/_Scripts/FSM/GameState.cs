public abstract class GameState
{
    protected GameState()
    {
    }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnLateUpdate() { }
}

public class WinState : GameState
{
}

public class LoseState : GameState
{
    
}