using System;
using Newtonsoft.Json;
using TechFu.Nirvana.Domain;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Io
{
    public class EnumerationJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((IEnumeration)value).DisplayName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var displayName = (string)reader.Value;

            return string.IsNullOrWhiteSpace(displayName) ? null : Enumeration.FromValueOrDisplayName(objectType, displayName);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Closes(typeof(Enumeration<>));
        }
    }
}