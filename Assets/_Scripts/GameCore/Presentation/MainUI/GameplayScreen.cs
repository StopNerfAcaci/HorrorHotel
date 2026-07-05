using System;
using Cysharp.Threading.Tasks;
using MVP;
using R3;
using UnityEngine;

namespace GameCore.Presentation
{
    public class GameplayScreen : Screen<GameplayViewState>
    {
        public GameplayViewState ViewModel { get; }

        private DisposableBag _bag;
        public override UniTask InitializeState(GameplayViewState state, Memory<object> args)
        {
            _bag.Dispose();
            _bag = new DisposableBag();
            
            //Assign state
            return UniTask.CompletedTask;
        }
    }
    
}
