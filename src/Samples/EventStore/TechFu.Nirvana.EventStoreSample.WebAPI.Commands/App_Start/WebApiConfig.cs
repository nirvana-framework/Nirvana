using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Commands
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
                };

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


            config.Formatters.Add(new BrowserJsonFormatter());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MapHttpAttributeRoutes();


            // Map this rule first
            config.Routes.MapHttpRoute(
                "WithActionApi",
                "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {action = "DefaultAction", id = RouteParameter.Optional}
            );

            //            config.Routes.MapHttpRoute(
            //                name: "DefaultApi",
            //                routeTemplate: "api/{controller}/{id}",
            //                defaults: new { id = RouteParameter.Optional }
            //            );
        }

        public class BrowserJsonFormatter : JsonMediaTypeFormatter
        {
            public BrowserJsonFormatter()
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
                SerializerSettings.Formatting = Formatting.Indented;
            }

            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers,
                MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }
    }
}