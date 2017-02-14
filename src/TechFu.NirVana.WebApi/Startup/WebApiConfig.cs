using System;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;

namespace Nirvana.Web.Startup
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config,Action<HttpConfiguration> additionalConfig)
        {
            config.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
                };

//            var cors = new EnableCorsAttribute("*", "*", "*");
//            config.EnableCors(cors);


            config.Formatters.Add(new BrowserJsonFormatter());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();


            // Map this rule first
            config.Routes.MapHttpRoute(
                "WithActionApi",
                "api/{controller}/{action}",
                new {id=RouteParameter.Optional}
            );

            additionalConfig(config);
        }
    }
}