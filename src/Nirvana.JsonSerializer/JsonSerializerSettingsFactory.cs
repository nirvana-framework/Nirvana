using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nirvana.JsonSerializer
{
    public class JsonSerializerSettingsFactory
    {
        public static JsonSerializerSettings GetJsonSerializerSettings(List<JsonConverter> converters=null)
        {
            var jsonConverters = new List<JsonConverter>
            {
                new EnumerationJsonConverter(),
            };
            converters?.ForEach(x =>
            {
                jsonConverters.Add(x); 
            });

            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,

                Converters = jsonConverters
            };
        }
    }
}