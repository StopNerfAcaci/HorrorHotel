using System;
using Cysharp.Threading.Tasks;
using MVP;
using R3;

namespace Gamecore.Presentation.Home
{
    public class HomePresenter : ScreenPresenter<HomeScreen, HomeViewState>
    {
        public ReactiveCommand PlayCommand { get; private set; }
        public ReactiveCommand<Unit> SettingCommand { get; private set; }
        public ReactiveCommand CreditCommand { get; private set; }

        public HomePresenter(HomeScreen view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, HomeViewState state, HomeScreen view)
        {
            PlayCommand = state.PlayCommand;
            SettingCommand = state.SettingCommand;
            CreditCommand = state.CreditsCommand;

            return UniTask.CompletedTask;
        }
    }
}
