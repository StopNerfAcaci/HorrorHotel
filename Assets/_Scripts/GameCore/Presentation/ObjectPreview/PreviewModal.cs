using System;
using Cysharp.Threading.Tasks;
using MVP;
using UnityEngine;

namespace GameCore.Presentation
{
    public class PreviewModal : Modal<PreviewViewState>
    {
        public override UniTask InitializeState(PreviewViewState state, Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}