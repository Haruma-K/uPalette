using uPalette.Runtime.Foundation.LocalPersistence.Serialization;

namespace uPalette.Runtime.Foundation.LocalPersistence
{
    public sealed class TextLocalPersistence : TextSerializeLocalPersistenceBase<string>
    {
        public TextLocalPersistence(string filePath) : base(filePath)
        {
        }

        protected override ISerializer<string, string> Serializer { get; } =
            new AnonymousSerializer<string, string>(x => x, x => x);
    }
}
