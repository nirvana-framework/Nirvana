using System.Collections.Generic;
using Newtonsoft.Json;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Io
{
    public class JsonSerializerSettingsFactory
    {
        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,

                Converters = new List<JsonConverter>
                {
                    new EnumerationJsonConverter(),
                }
            };
        }
    }
}