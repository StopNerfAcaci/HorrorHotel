using MVP;
using R3;

public class InventoryViewState: ViewState
{
    public ReactiveCommand ClosePopupCommand { get; } = new();
}