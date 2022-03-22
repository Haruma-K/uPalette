using System.IO;
using uPalette.Editor.Foundation.LocalPersistence.Serialization;

namespace uPalette.Editor.Foundation.LocalPersistence
{
    public class UnityJsonLocalPersistence<T> : TextSerializeLocalPersistenceBase<T>
    {
        private const string Extension = ".json";

        public bool PrettyPrint
        {
            get => _serializer.PrettyPrint;
            set => _serializer.PrettyPrint = value;
        }

        private readonly UnityJsonSerializer<T> _serializer =
            new UnityJsonSerializer<T>();

        protected override ISerializer<T, string> Serializer => _serializer;

        public UnityJsonLocalPersistence(string folderPath, string fileNameWithoutExtensions)
            : base($"{Path.Combine(folderPath, fileNameWithoutExtensions)}{Extension}")
        {
        }
    }
}
