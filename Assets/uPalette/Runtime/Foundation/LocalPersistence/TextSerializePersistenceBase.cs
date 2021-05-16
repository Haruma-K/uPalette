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
        public Encoding Encoding { get; set; } = new UTF8Encoding(false);

        public TextSerializePersistenceBase(string filePath) : base(filePath)
        {
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

        protected override async Task<string> InternalLoadAsync(string path)
        {
            var result = await UnityFile.ReadAllTextAsync(path, Encoding).ConfigureAwait(false);
            return result;
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