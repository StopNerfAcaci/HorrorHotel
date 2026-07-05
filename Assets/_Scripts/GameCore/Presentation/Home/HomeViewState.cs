using MVP;
using R3;

namespace Gamecore.Presentation.Home
{
    public class HomeViewState : ViewState
    {
        public ReactiveCommand PlayCommand { get; } = new();
        public ReactiveCommand<Unit> SettingCommand { get; } = new();
        public ReactiveCommand CreditsCommand { get; } = new();
        public ReactiveCommand<Unit> LeaveCommand { get; } = new();
    }
}