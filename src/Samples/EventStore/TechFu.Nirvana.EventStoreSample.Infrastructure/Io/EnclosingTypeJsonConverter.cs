using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Io
{
    public class EnclosingTypeJsonConverter : JsonConverter
    {
        private readonly Type _type;

        public EnclosingTypeJsonConverter(Type type)
        {
            _type = type;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                Converters = serializer.Converters.Where(c => !(c is EnclosingTypeJsonConverter)).ToList(),
                MissingMemberHandling = serializer.MissingMemberHandling,
                NullValueHandling = serializer.NullValueHandling,
                Formatting = serializer.Formatting
            };

            var writerSerializer = JsonSerializer.Create(serializerSettings);

            writer.WriteStartObject();
            writer.WritePropertyName(_type.Name);

            var jObject = JObject.FromObject(value, writerSerializer);
            jObject.WriteTo(writer);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            reader.Read();
            reader.Read();

            var jsonObject = JObject.Load(reader);
            var target = Activator.CreateInstance(objectType);

            var jObjectReader = jsonObject.CreateReader();
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;

            serializer.Populate(jObjectReader, target);

            reader.Read();

            return target;
        }

        public override bool CanConvert(Type objectType)
        {
            return _type.IsAssignableFrom(objectType);
        }
    }
}