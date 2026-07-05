using System;

namespace MVP
{
    public abstract class Presenter<TView> : IDisposable
    {
        protected Presenter(TView view)
        {
            View = view;
        }

        public bool IsDisposed { get; private set; }

        public bool IsInitialized { get; private set; }

        private TView View { get; }

        public virtual void Dispose()
        {
            if (!IsInitialized)
                return;

            if (IsDisposed)
                return;

            Dispose(View);
            IsDisposed = true;
        }

        public void Initialize()
        {
            if (IsInitialized)
                throw new InvalidOperationException($"{GetType().Name} is already initialized.");

            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Presenter<TView>));

            Initialize(View);
            IsInitialized = true;
        }

        /// <summary>
        ///     Initializes the presenter.
        /// </summary>
        /// <param name="view"></param>
        protected abstract void Initialize(TView view);

        /// <summary>
        ///     Disposes the presenter.
        /// </summary>
        /// <param name="view"></param>
        protected abstract void Dispose(TView view);
    }

}