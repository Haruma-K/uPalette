namespace uPalette.Editor.Foundation.LocalPersistence.Serialization
{
    /// <summary>
    ///     Interface of the serializer.
    /// </summary>
    public interface ISerializer<TSerialized>
    {
        /// <summary>
        ///     Serialize.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        TSerialized Serialize(object obj);

        /// <summary>
        ///     Deserialize.
        /// </summary>
        /// <param name="serialized"></param>
        /// <typeparam name="TDeserialized"></typeparam>
        /// <returns></returns>
        TDeserialized Deserialize<TDeserialized>(TSerialized serialized);
    }
    
    /// <summary>
    ///     Interface of the serializer.
    /// </summary>
    public interface ISerializer<TDeserialized, TSerialized>
    {
        /// <summary>
        ///     Serialize.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        TSerialized Serialize(TDeserialized obj);

        /// <summary>
        ///     Deserialize.
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        TDeserialized Deserialize(TSerialized serialized);
    }
}