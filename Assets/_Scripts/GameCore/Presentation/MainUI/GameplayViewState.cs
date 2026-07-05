using Cysharp.Threading.Tasks;
using MVP;
using R3;
using UnityEngine;

namespace GameCore.Presentation
{
    public class GameplayPresenter : ScreenPresenter<GameplayScreen, GameplayViewState>
    {
        public ReactiveCommand<bool> PauseCommand { get; } = new();
        public ReactiveCommand<bool> RetryCommand { get; } = new();
        public ReactiveCommand<Unit> QuitLevel { get; } = new ();
        public GameplayPresenter(GameplayScreen view) : base(view)
        {
        }

        public UniTask StartLevel()
        {
            return UniTask.CompletedTask;
        }
    }
}