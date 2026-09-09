using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.MVP
{
    public interface ICloseTransition
    {
        public UniTask ClosePopup();
    }

//Detect and call UI
    public class UITransition : ICloseTransition
    {
        private List<Popup> _popups = new();

        public UniTask ClosePopup()
        {
            var modalsCount = _popups.Count;
            return modalsCount > 0 ? PopAsync(true) : UniTask.CompletedTask;
        }

        public bool IsInTransition { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask PopAsync(bool animated)
        {
            await PopAsyncInternal(animated);
        }

        private async Task PopAsyncInternal(object playAnimation)
        {
            if (IsInTransition)
            {
                ErrorIfCannotTransition();
                return;
            }

            IsInTransition = true;

            var lastModalIndex = _popups.Count - 1;
            var exitModalRef = _popups[lastModalIndex];

            var enterModal = _popups.Count == 1 ? null : _popups[^2];
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorIfCannotTransition()
        {
            UnityEngine.Debug.LogError("Cannot transition because there is a modal already in transition.");
        }
    }

    public static class RectTransformExtensions
    {
        public static void FillParent(this RectTransform self, RectTransform parent)
        {
            self.SetParent(parent, false);
            self.localPosition = Vector3.zero;
            self.anchorMin = Vector2.zero;
            self.anchorMax = Vector2.one;
            self.offsetMin = Vector2.zero;
            self.offsetMax = Vector2.zero;
            self.pivot = new Vector2(0.5f, 0.5f);
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }
    }
}