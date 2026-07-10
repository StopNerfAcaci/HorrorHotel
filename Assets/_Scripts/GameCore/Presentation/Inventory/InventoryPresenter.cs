using System;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using MVP;
using R3;

public class InventoryPresenter : ScreenPresenter<InventoryScreen, InventoryViewState>
{
    
    private InventoryViewState _viewState;
    private TransitionService _transitionService;
    private DisposableBag _bag;
    
    public InventoryPresenter(InventoryScreen view) : base(view)
    {

    }

    protected override UniTask Initialize(Memory<object> args, InventoryViewState state, InventoryScreen view)
    {
        _bag.Dispose();
        _bag = new DisposableBag();
        _viewState = state;
        return UniTask.CompletedTask;
    }
}