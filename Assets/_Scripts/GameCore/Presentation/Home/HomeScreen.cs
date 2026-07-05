using System;
using Cysharp.Threading.Tasks;
using MVP;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Gamecore.Presentation.Home
{
    public class HomeScreen : Screen<HomeViewState>
    {
        [SerializeField] Button playButton;
        [SerializeField] Button settingButton;

        private DisposableBag _bag;

        public override UniTask InitializeState(HomeViewState state, Memory<object> args)
        {
            _bag.Dispose();
            _bag = new DisposableBag();
            playButton.onClick.RemoveAllListeners();

            playButton.SubscribeToCommand(state.PlayCommand)
                .AddTo(ref _bag);
            settingButton.SubscribeToCommand(state.SettingCommand)
                .AddTo(ref _bag);
            
            return UniTask.CompletedTask;
        }
    }
}
