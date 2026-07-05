using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MVP
{
    public abstract class AppView<TState> : MonoBehaviour
        where TState : ViewState
    {
        private bool _isInitialized;

        public async UniTask InitializeAsync(TState state)
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            await Initialize(state);
        }

        /// <summary>
        /// Clears the once-initialized guard so the next <see cref="InitializeAsync"/> call
        /// will run <see cref="Initialize"/> again. Needed for pooled views where the
        /// owning presenter (and its state) is re-created per push.
        /// </summary>
        public void ResetInitialized()
        {
            _isInitialized = false;
        }

        protected abstract UniTask Initialize(TState state);
    }
}