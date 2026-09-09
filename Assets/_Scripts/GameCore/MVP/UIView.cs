using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Extensions;

namespace GameCore.MVP
{
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
}