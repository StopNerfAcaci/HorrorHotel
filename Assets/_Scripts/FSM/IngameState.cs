using Cysharp.Threading.Tasks;
using HSM;
using UnityEngine;

public class IngameState : GameState
{
    private GameplayManager _manager;
    private readonly PlayerStateDriver player; 
    public IngameState(GameplayManager manager, PlayerStateDriver player)
    {
        _manager = manager;
        this.player = player;
    }

    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
        player.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        player.OnFixedUpdate();
    }

    public override void OnLateUpdate()
    {
        if (player.IsBusy) return;
        _manager.LateUpdateInternal();
    }
}