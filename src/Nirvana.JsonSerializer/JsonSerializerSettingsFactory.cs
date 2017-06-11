using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nirvana.JsonSerializer
{
    public class JsonSerializerSettingsFactory
    {
        public static JsonSerializerSettings GetJsonSerializerSettings(bool includeNulls,List<JsonConverter> converters=null)
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
                NullValueHandling = includeNulls?NullValueHandling.Include: NullValueHandling.Ignore,

                Converters = jsonConverters
            };
        }
    }
}