using System.IO;
using System.Text;
using System.Threading.Tasks;
using uPalette.Runtime.Foundation.LocalPersistence.IO;

namespace uPalette.Runtime.Foundation.LocalPersistence
{
    /// <summary>
    ///     Serialize and persist the text data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TextSerializePersistenceBase<T> : SerializeLocalPersistenceBase<T, string>
    {
        public TextSerializePersistenceBase(string filePath) : base(filePath)
        {
        }

        public Encoding Encoding { get; set; } = new UTF8Encoding(false);

        protected override void InternalSave(string path, string serialized)
        {
            var folderPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(path, serialized);
        }

        protected override async Task InternalSaveAsync(string path, string serialized)
        {
            var folderPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream, Encoding))
            {
                await streamWriter.WriteAsync(serialized).ConfigureAwait(false);
            }
        }

        protected override string InternalLoad(string path)
        {
            return UnityFile.ReadAllText(path, Encoding);
        }

        protected override async Task<string> InternalLoadAsync(string path)
        {
            return await UnityFile.ReadAllTextAsync(path, Encoding);
        }

        protected override void DeleteInternal(string path)
        {
            File.Delete(path);
        }

        protected override bool ExistsInternal(string path)
        {
            return File.Exists(path);
        }
    }
}
