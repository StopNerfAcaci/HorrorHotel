using Cysharp.Threading.Tasks;

public class IngameState : GameState
{
    private GameplayManager _manager;
    
    public IngameState(GameplayManager manager)
    {
        _manager = manager;
    }

    public override void OnEnter()
    {
        _ = OnEnterAsync();
    }

    private async UniTask OnEnterAsync()
    {
        _manager.SwitchToPlayerCam();
    }

    public override void OnUpdate()
    {
        _manager.Player.OnUpdate();
    }
}