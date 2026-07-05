using System;

namespace MVP
{
    public class ViewState : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                InternalDispose();
                _disposed = true;
            }
        }

        protected virtual void InternalDispose()
        {
        }
    }
}