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
    }

    public override void OnExit()
    {
    }

    // private async UniTask ExitAsync()
    // {
    // }
    //
    // private async UniTask ShowHomeAsync()
    // {
    //     //TODO: Show home menu UI
    //     //TODO: Start home music
    // }
}