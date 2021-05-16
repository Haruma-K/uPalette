using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using uPalette.Runtime.Foundation.LocalPersistence.Serialization;

namespace uPalette.Runtime.Foundation.LocalPersistence
{
    /// <summary>
    ///     Base class to serialize and persist the data.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSerialized"></typeparam>
    public abstract class SerializeLocalPersistenceBase<TSource, TSerialized> : ILocalPersistence<TSource>
    {
        private static readonly object _semaphoreLocker = new object();
        private static readonly Dictionary<string, SemaphoreSlim> _semaphores = new Dictionary<string, SemaphoreSlim>();
        
        private string FilePath { get; }

        protected abstract ISerializer<TSource, TSerialized> Serializer { get; }
        
        public SerializeLocalPersistenceBase(string filePath)
        {
            FilePath = CleanupDirectorySeparator(filePath);
        }
        
#if UNITY_EDITOR
        public async Task SaveAsync(TSource target)
        {
            var semaphore = GetSemaphore();
            try
            {
                await semaphore.WaitAsync().ConfigureAwait(false);
                var serialized = Serializer.Serialize(target);
                var path = GetPath();
                CreateFolders(path);
                await InternalSaveAsync(path, serialized).ConfigureAwait(false);
            }
            finally
            {
                semaphore.Release();
            }
        }
#endif

        public async Task<TSource> LoadAsync()
        {
            var semaphore = GetSemaphore();
            try
            {
                await semaphore.WaitAsync().ConfigureAwait(false);
                var path = GetPath();
                var serialized = await InternalLoadAsync(path).ConfigureAwait(false);
                return Serializer.Deserialize(serialized);
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void Delete()
        {
            var semaphore = GetSemaphore();
            try
            {
                var path = GetPath();
                DeleteInternal(path);
            }
            finally
            {
                semaphore.Release();
            }
        }

        public bool Exists()
        {
            var path = GetPath();
            return ExistsInternal(path);
        }

        public virtual string GetPath()
        {
            return FilePath;
        }

        protected abstract Task InternalSaveAsync(string path, TSerialized serialized);

        protected abstract Task<TSerialized> InternalLoadAsync(string path);

        protected abstract void DeleteInternal(string path);

        protected abstract bool ExistsInternal(string path);

        private static bool IsAssetPath(string path)
        {
            return path.StartsWith("Assets");
        }

        private static string CleanupDirectorySeparator(string path)
        {
#if UNITY_EDITOR
            if (IsAssetPath(path))
            {
                return path.Replace(Path.DirectorySeparatorChar, '/')
                    .Replace(Path.AltDirectorySeparatorChar, '/');
            }
#endif
            return path;
        }

        private static void CreateFolders(string path)
        {
            var folderPath = Path.GetDirectoryName(path);
            if (folderPath != null)
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private static void DeleteEmptyFolders(string path)
        {
            DeleteEmptyFoldersRecursive(path);
        }

        private static void DeleteEmptyFoldersRecursive(string path)
        {
            if (Directory.Exists(path))
            {
                if (Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0)
                {
                    Directory.Delete(path, false);
#if UNITY_EDITOR
                    // Delete the meta file of the directory.
                    var metaPath = $"{path}.meta";
                    if (File.Exists(metaPath))
                    {
                        File.Delete(metaPath);
                    }
#endif
                }
            }

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                DeleteEmptyFolders(directory);
            }
        }

        private SemaphoreSlim GetSemaphore()
        {
            lock (_semaphoreLocker)
            {
                if (_semaphores.TryGetValue(FilePath, out var semaphore))
                {
                    return semaphore;
                }
                semaphore = new SemaphoreSlim(1);
                _semaphores.Add(FilePath, semaphore);
                return semaphore;
            }
        }
    }
}