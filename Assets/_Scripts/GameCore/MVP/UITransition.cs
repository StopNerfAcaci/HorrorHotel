using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Extensions;
using Debug = UnityEngine.Debug;

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

    public class UIView: UIBehaviour
    {
        private RectTransform _rectTransform;

        public virtual RectTransform RectTransform
        {
            get
            {
                if (IsDestroyed())
                    return null;

                if (_rectTransform == false)
                    _rectTransform = gameObject.GetOrAddComponent<RectTransform>();

                return _rectTransform;
            }
        }

        private RectTransform _parent;

        public virtual RectTransform Parent
        {
            get
            {
                if (IsDestroyed())
                {
                    return null;
                }

                return _parent;
            }

            internal set => _parent = value;
        }

        protected static async UniTask WaitForAsync(IEnumerable<UniTask> tasks)
        {
            try
            {
                foreach (var task in tasks)
                {
                    try
                    {
                        await task;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        private CanvasGroup _canvasGroup;
        public virtual CanvasGroup CanvasGroup
        {
            get
            {
                if (IsDestroyed())
                    return null;

                if (_canvasGroup == false)
                    _canvasGroup = gameObject.GetComponent<CanvasGroup>();

                if (_canvasGroup == false)
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();

                return _canvasGroup;
            }
        }
        
        public virtual float Alpha
        {
            get
            {
                if (IsDestroyed() || gameObject == false)
                    return 0;

                if (CanvasGroup)
                    return CanvasGroup.alpha;

                return 1f;
            }
            set
            {
                if (IsDestroyed() || gameObject == false)
                    return;

                if (CanvasGroup)
                    CanvasGroup.alpha = value;
            }
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