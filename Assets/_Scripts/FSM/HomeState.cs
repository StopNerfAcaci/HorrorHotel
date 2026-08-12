using Cysharp.Threading.Tasks;

public class HomeState : GameState
{
    private GameplayManager _manager;

    public HomeState(GameplayManager manager)
    {
        _manager = manager;
    }
    public override void OnEnter()
    {
        _ = ShowHomeAsync();
    }

    public override void OnExit()
    {
        _ = ExitAsync();
    }

    private async UniTask ExitAsync()
    {
    }

    private async UniTask ShowHomeAsync()
    {
        //TODO: Show home menu UI
        //TODO: Start home music
    }
}