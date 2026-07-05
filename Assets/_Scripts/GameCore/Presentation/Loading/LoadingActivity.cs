using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MVP;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Presentation.Loading
{
    public class LoadingActivity : Activity<LoadingViewState>
    {
        [Header("LOADING CANVAS")] [SerializeField]
        private CanvasGroup bg;

        [SerializeField] private RectTransform rectLoading;
        [Header("IMAGE")] [SerializeField] private RectTransform imageSpin;
        [SerializeField] private float timeRotate = .5f;
        [SerializeField] private int rounds = 5;

        [Header("Parameters")] [SerializeField]
        private float timeFadeIn;

        [SerializeField] private float timeFadeOut;
        [SerializeField] private float startFade;

        private LoadingViewState currentState;
        private Tween fillTween;

        public override UniTask InitializeState(LoadingViewState state, Memory<object> args)
        {
            currentState = state;
            AnimationLoading();
            return UniTask.CompletedTask;
        }

        private void AnimationLoading()
        {
            currentState.IntroAnimationComplete = false;
            ResetAnim();
            bg.alpha = startFade;
            imageSpin.localEulerAngles = Vector3.zero;


            var fillSequence = DOTween.Sequence()
                .SetUpdate(true)
                .Append(imageSpin.DOLocalRotate(
                    new Vector3(0, 0, 360f * rounds),
                    timeRotate,
                    RotateMode.FastBeyond360
                ).SetEase(Ease.OutCubic));
            
            fillTween = fillSequence.OnComplete(CompleteIntro);

            bg.DOFade(1, timeFadeIn).SetUpdate(true);
        }

        private void CompleteIntro()
        {
            if (currentState.IntroAnimationComplete)
            {
                return;
            }

            currentState.IntroAnimationComplete = true;
            currentState.OnAnimationDoneCommand.Execute(Unit.Default);
        }

        private void ResetAnim()
        {
            rectLoading.transform.DOKill();
            fillTween?.Kill();
            fillTween = null;
            rectLoading.localScale = Vector3.one;
        }

        public async UniTask PlayMoveOut()
        {
            DG.Tweening.Sequence seq = DOTween.Sequence();
            await seq.Join(rectLoading.DOScale(1.5f, timeFadeOut))
                .Join(bg.DOFade(0, timeFadeOut))
                .SetEase(Ease.Linear)
                .OnComplete(() => { bg.alpha = 0; }).SetUpdate(true);
        }
    }
}