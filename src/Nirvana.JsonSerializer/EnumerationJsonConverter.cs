using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nirvana.Domain;
using Nirvana.Util.Extensions;

namespace Nirvana.JsonSerializer
{
    public class EnumerationJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var enumValue = (IEnumeration) value;

            serializer.Serialize(writer, new {enumValue.DisplayName,Value=enumValue.Value});
          //  writer.WriteRaw($"{{\"DisplayName\":\"{enumValue.DisplayName}\",\"Value\":\"{enumValue.Value}\"}}");
            //serializer.Serialize(writer, $"{{\"displayName\":\"{enumValue.DisplayName}\",\"Value\":\"{enumValue.Value}\"}}");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            var inner = JObject.Load(reader);
            //var inner = (dynamic) reader.Value;

            var value = ((JValue) inner.Property("Value").First).Value;

            return value==null || value.Equals("")
                ? null
                : Enumeration.FromValue(objectType, Convert.ToInt32(value));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.ClosesOrImplements(typeof(Enumeration<>));
        }
    }
}