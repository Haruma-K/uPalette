using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace uPalette.Runtime.Foundation.LocalPersistence.IO
{
    internal class UnityWebRequestAsyncOperationAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation _asyncOperation;

        public UnityWebRequestAsyncOperationAwaiter(UnityWebRequestAsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public bool IsCompleted => _asyncOperation.isDone;

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => continuation();
        }

        public void GetResult()
        {
        }

        public UnityWebRequestAsyncOperationAwaiter GetAwaiter()
        {
            return this;
        }
    }
}