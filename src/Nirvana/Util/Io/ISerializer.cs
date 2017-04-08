using System;
using System.IO;

namespace Nirvana.Util.Io
{
    public interface ISerializer
    {
        string Serialize(object obj);
        void SerializeToStream(object obj, TextWriter writer);
        object Deserialize(Type type, string value);

        T Deserialize<T>(string value);
        T Deserialize<T>(T type, string value);
        object DeserializeFromStream(Type type, TextReader reader);
        T DeserializeFromStream<T>(TextReader reader);
    }
}