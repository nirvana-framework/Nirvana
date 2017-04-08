using System;
using System.IO;
using Newtonsoft.Json;
using Nirvana.Util.Io;

namespace Nirvana.JsonSerializer
{
    public class Serializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private readonly JsonSerializerSettings _settings;

        public Serializer()
        {
            _settings = JsonSerializerSettingsFactory.GetJsonSerializerSettings();
            _serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(_settings);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public void SerializeToStream(object obj, TextWriter writer)
        {
            _serializer.Serialize(writer, obj);
        }

        public object Deserialize(Type type, string value)
        {
            return JsonConvert.DeserializeObject(value, type, _settings);
        }

    

        public T Deserialize<T>(string value)
        {
            return (T) Deserialize(typeof(T), value);
        }

     

        public T Deserialize<T>(T prototype, string value)
        {
            return Deserialize<T>(value);
        }

        public object DeserializeFromStream(Type type, TextReader reader)
        {
            return _serializer.Deserialize(reader, type);
        }

        public T DeserializeFromStream<T>(TextReader reader)
        {
            return (T) DeserializeFromStream(typeof(T), reader);
        }
    }
}