using System;
using System.IO;
using Newtonsoft.Json;
using TechFu.Nirvana.Util.Io;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Io
{
    public class Serializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;
        private readonly JsonSerializer _serializer;

        public Serializer()
        {
            _settings = JsonSerializerSettingsFactory.GetJsonSerializerSettings();
            _serializer = JsonSerializer.CreateDefault(_settings);
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

        public object DeserializeEnclosing(Type type, string value)
        {
            return JsonConvert.DeserializeObject(value, type, new EnclosingTypeJsonConverter(type));
        }

        public T Deserialize<T>(string value)
        {
            return (T)Deserialize(typeof(T), value);
        }

        public T DeserializeEnclosing<T>(string value)
        {
            return (T)DeserializeEnclosing(typeof(T), value);
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
            return (T)DeserializeFromStream(typeof(T), reader);
        }
    }
}