using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GlobalSettings
{
    internal static class GlobalSettingsLoader
    {
        public static CancellationTokenSource StartLoad<T>(
            string fileName,
            Action<AsyncOperationHandle<T>?> onLoadStarted,
            Action<T> onComplete)
        {
            var cts = new CancellationTokenSource();
            LoadAsync(fileName, onLoadStarted, onComplete, cts.Token).Forget();
            return cts;
        }

        private static async UniTaskVoid LoadAsync<T>(
            string fileName,
            Action<AsyncOperationHandle<T>?> onLoadStarted,
            Action<T> onComplete,
            CancellationToken token)
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, token);

            if (token.IsCancellationRequested) return;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>("GlobalSettings/" + fileName);
            AsyncLoadOrderingManager.OnStartedLoad(handle, out int orderHandle);
            onLoadStarted(handle);

            T result;
            try
            {
                result = await handle.ToUniTask(cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            AsyncLoadOrderingManager.OnCompletedLoad(handle, orderHandle);
            onComplete(result);
        }
    }
}