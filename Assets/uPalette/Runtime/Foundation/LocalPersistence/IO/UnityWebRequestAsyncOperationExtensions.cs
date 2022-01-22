using UnityEngine.Networking;

namespace uPalette.Runtime.Foundation.LocalPersistence.IO
{
    internal static class UnityWebRequestAsyncOperationExtensions
    {
        public static UnityWebRequestAsyncOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOperation)
        {
            return new UnityWebRequestAsyncOperationAwaiter(asyncOperation);
        }
    }
}
