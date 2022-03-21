using System;

namespace uPalette.Editor.Foundation.LocalPersistence.Serialization
{
    public class AnonymousSerializer<TSerialized> : ISerializer<TSerialized>
    {
        private readonly Func<object, TSerialized> _serialize;
        private readonly Func<TSerialized, object> _deserialize;

        public AnonymousSerializer(Func<object, TSerialized> serialize, Func<TSerialized, object> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }
        
        public TSerialized Serialize(object obj)
        {
            return _serialize.Invoke(obj);
        }

        public T Deserialize<T>(TSerialized serialized)
        {
            return (T)_deserialize.Invoke(serialized);
        }
    }
    
    public class AnonymousSerializer<TDeserialized, TSerialized> : ISerializer<TDeserialized, TSerialized>
    {
        private readonly Func<TDeserialized, TSerialized> _serialize;
        private readonly Func<TSerialized, TDeserialized> _deserialize;

        public AnonymousSerializer(Func<TDeserialized, TSerialized> serialize, Func<TSerialized, TDeserialized> deserialize)
        {
            _serialize = serialize;
            _deserialize = deserialize;
        }
        
        public TSerialized Serialize(TDeserialized obj)
        {
            return _serialize.Invoke(obj);
        }

        public TDeserialized Deserialize(TSerialized serialized)
        {
            return _deserialize.Invoke(serialized);
        }
    }
}