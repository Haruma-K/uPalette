using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace uPalette.Editor.Foundation.LocalPersistence.IO
{
    /// <summary>
    ///     Utilities for handling files in Unity.
    /// </summary>
    public static class UnityFile
    {
        /// <summary>
        ///     Read all bytes from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static byte[] ReadAllBytes(string path)
        {
            if (path.Contains(Application.streamingAssetsPath))
            {
                return StreamingAssetsFile.ReadAllBytes(path);
            }

            return File.ReadAllBytes(path);
        }

        /// <summary>
        ///     Read all text from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal static string ReadAllText(string path, Encoding encoding = null)
        {
            encoding = encoding ?? new UTF8Encoding(false);

            if (path.Contains(Application.streamingAssetsPath))
            {
                return StreamingAssetsFile.ReadAllText(path, encoding);
            }

            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        ///     Read all bytes from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            if (path.Contains(Application.streamingAssetsPath))
            {
                return await StreamingAssetsFile.ReadAllBytesAsync(path);
            }

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var bytes = new byte[fileStream.Length];
                await fileStream.ReadAsync(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        /// <summary>
        ///     Read all text from a file asynchronously.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal static async Task<string> ReadAllTextAsync(string path, Encoding encoding = null)
        {
            encoding = encoding ?? new UTF8Encoding(false);

            if (path.Contains(Application.streamingAssetsPath))
            {
                return await StreamingAssetsFile.ReadAllTextAsync(path, encoding);
            }

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var streamReader = new StreamReader(fileStream, encoding))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
