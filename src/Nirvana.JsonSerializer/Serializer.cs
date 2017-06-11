using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Nirvana.Util.Io;

namespace Nirvana.JsonSerializer
{
    public class Serializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private readonly Newtonsoft.Json.JsonSerializer _withNullSerializer;
        private readonly JsonSerializerSettings _settings;
        private readonly JsonSerializerSettings _includeNullSettings;

        public Serializer(List<JsonConverter> converters = null)
        {
            _settings = JsonSerializerSettingsFactory.GetJsonSerializerSettings(false,converters);
            _includeNullSettings= JsonSerializerSettingsFactory.GetJsonSerializerSettings(true,converters);
            _serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(_settings);
            _withNullSerializer= Newtonsoft.Json.JsonSerializer.CreateDefault(_settings);
        }

        public string Serialize(object obj,bool includeNulls=false)
        {
            if (includeNulls)
            {

                return JsonConvert.SerializeObject(obj, _includeNullSettings);
            }
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public void SerializeToStream(object obj, TextWriter writer,bool includeNulls=false)
        {
            if (includeNulls)
            {
                
                _withNullSerializer.Serialize(writer, obj);
            }
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