#if UNITY_ANDROID && !UNITY_EDITOR
#define USE_WEB_REQUEST
#endif

using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
#if USE_WEB_REQUEST
using System;
using System.Threading;
using UnityEngine.Networking;
#endif

namespace uPalette.Editor.Foundation.LocalPersistence.IO
{
    /// <summary>
    ///     Utilities for handling files in the StreamingAssets folder.
    /// </summary>
    internal static class StreamingAssetsFile
    {
        /// <summary>
        ///     Read all bytes from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string path)
        {
            Assert.IsTrue(path.StartsWith(Application.streamingAssetsPath));
#if USE_WEB_REQUEST
            using (var request = UnityWebRequest.Get(path))
            {
                request.SendWebRequest();
                while (!request.isDone)
                {
                    Thread.Sleep(100);
                }

                var isError = request.result == UnityWebRequest.Result.ProtocolError
                              || request.result == UnityWebRequest.Result.ConnectionError
                              || request.result == UnityWebRequest.Result.DataProcessingError;
                if (isError)
                {
                    throw new Exception(request.error);
                }

                return request.downloadHandler.data;
            }
#else
            return File.ReadAllBytes(path);
#endif
        }

        /// <summary>
        ///     Read all bytes from a file.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytesWithRelativePath(string relativePath)
        {
            var path = Path.Combine(Application.streamingAssetsPath, relativePath);
            return ReadAllBytes(path);
        }

        /// <summary>
        ///     Read all text from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadAllText(string path, Encoding encoding = null)
        {
            Assert.IsTrue(path.StartsWith(Application.streamingAssetsPath));
            encoding = encoding ?? new UTF8Encoding(false);

#if USE_WEB_REQUEST
            using (var request = UnityWebRequest.Get(path))
            {
                request.SendWebRequest();
                while (!request.isDone)
                {
                    Thread.Sleep(100);
                }

                var isError = request.result == UnityWebRequest.Result.ProtocolError
                              || request.result == UnityWebRequest.Result.ConnectionError
                              || request.result == UnityWebRequest.Result.DataProcessingError;
                if (isError)
                {
                    throw new Exception(request.error);
                }

                return encoding.GetString(request.downloadHandler.data);
            }
#else
            return File.ReadAllText(path, encoding);
#endif
        }

        /// <summary>
        ///     Read all text from a file.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadAllTextWithRelativePath(string relativePath, Encoding encoding = null)
        {
            var path = Path.Combine(Application.streamingAssetsPath, relativePath);
            return ReadAllText(path, encoding);
        }

        /// <summary>
        ///     Read all bytes from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            Assert.IsTrue(path.StartsWith(Application.streamingAssetsPath));

#if USE_WEB_REQUEST
            using (var request = UnityWebRequest.Get(path))
            {
                await request.SendWebRequest();
                var isError = request.result == UnityWebRequest.Result.ProtocolError
                              || request.result == UnityWebRequest.Result.ConnectionError
                              || request.result == UnityWebRequest.Result.DataProcessingError;
                if (isError)
                {
                    throw new Exception(request.error);
                }

                return request.downloadHandler.data;
            }
#else
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);
                return bytes;
            }
#endif
        }

        /// <summary>
        ///     Read all bytes from a file asynchronously.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static async Task<byte[]> ReadAllBytesWithRelativePathAsync(string relativePath)
        {
            var path = Path.Combine(Application.streamingAssetsPath, relativePath);
            return await ReadAllBytesAsync(path);
        }

        /// <summary>
        ///     Read all text from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> ReadAllTextAsync(string path, Encoding encoding = null)
        {
            Assert.IsTrue(path.StartsWith(Application.streamingAssetsPath));
            encoding = encoding ?? new UTF8Encoding(false);

#if USE_WEB_REQUEST
            using (var request = UnityWebRequest.Get(path))
            {
                await request.SendWebRequest();
                var isError = request.result == UnityWebRequest.Result.ProtocolError
                              || request.result == UnityWebRequest.Result.ConnectionError
                              || request.result == UnityWebRequest.Result.DataProcessingError;
                if (isError)
                {
                    throw new Exception(request.error);
                }

                return encoding.GetString(request.downloadHandler.data);
            }
#else
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream, encoding))
            {
                return await streamReader.ReadToEndAsync();
            }
#endif
        }

        /// <summary>
        ///     Read all text from a file asynchronously.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> ReadAllTextWithRelativePathAsync(string relativePath, Encoding encoding = null)
        {
            var path = Path.Combine(Application.streamingAssetsPath, relativePath);
            return await ReadAllTextAsync(path, encoding);
        }
    }
}
