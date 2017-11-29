using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nirvana.CQRS;
using Nirvana.Mediation;
using Nirvana.Util.Io;

namespace Nirvana.Web.Controllers
{
    [EnableCors("AllowAllOrigins")]
    public abstract class CommandQueryApiControllerBase : Controller
    {
        private readonly ISerializer _serializer;
        
        public IMediatorFactory Mediator { get; set; }

        protected CommandQueryApiControllerBase(IMediatorFactory mediator,ISerializer serializer)
        {
            Mediator = mediator;
        }

        protected CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            var response = Mediator.Command(command);
            LogException(response);
            return response;
        }


        protected InternalEventResponse InternalEvent(InternalEvent command)
        {
            var response = Mediator.InternalEvent(command);
            LogException(response);
            return response;
        }


        protected QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            var response = Mediator.Query(query);
            LogException(response);
            return response;
        }

        protected HttpResponseMessage QueryForFile<TResult>(Query<TResult> query)
            where TResult : FileQueryResult
        {
            var response = Mediator.Query(query);

            var httpResponseMessage = response.BuildResponse(_serializer);


            if (response.Success())
            {
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(response.Result.FileType);
                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = response.Result.FileName
                };
            }

            if (response.Result.IsStream)
            {
                httpResponseMessage.Content = new StreamContent(response.Result.FileStream);
            }


            return httpResponseMessage;
        }


        private static void LogException(Response response)
        {
            if (response.Exception != null)
            {
                //Logging.Exception(response.Exception);
            }
        }
    }


    public static class ResponseBuilder
    {
        private static HttpStatusCode GetResponseCode<T>(CommandResponse<T> input)
        {
            return input.Exception != null
                ? HttpStatusCode.InternalServerError
                : input.IsValid
                    ? HttpStatusCode.OK
                    : HttpStatusCode.BadRequest;
        }

        private static HttpStatusCode GetResponseCode(InternalEventResponse input)
        {
            return input.Exception != null
                ? HttpStatusCode.InternalServerError
                : input.IsValid
                    ? HttpStatusCode.OK
                    : HttpStatusCode.BadRequest;
        }

        private static HttpStatusCode GetResponseCode<T>(QueryResponse<T> input)
        {
            return input.Exception != null
                ? HttpStatusCode.InternalServerError
                : input.Success()
                    ? HttpStatusCode.OK
                    : HttpStatusCode.BadRequest;
        }


        public static HttpResponseMessage BuildResponse<TResult>(this QueryResponse<TResult> response,
            ISerializer serializer)
        {
            return new HttpResponseMessage(GetResponseCode(response))
            {
                Content = new StringContent(serializer.Serialize(response)),
                Version = HttpVersion.Version11
            };
        }


        public static HttpResponseMessage BuildResponse<TResult>(this CommandResponse<TResult> response,
            ISerializer serializer)
        {
            return new HttpResponseMessage(GetResponseCode(response))
            {
                Content = new StringContent(serializer.Serialize(response)),
                Version = HttpVersion.Version11
            };
        }

        public static HttpResponseMessage BuildResponse(this InternalEventResponse response, ISerializer serializer)
        {
            return new HttpResponseMessage(GetResponseCode(response))
            {
                Content = new StringContent(serializer.Serialize(response)),
                Version = HttpVersion.Version11
            };
        }
    }

    public class WebApiSecurity
    {
        public string GetAuthCode(HttpContext context)
        {
            return context.Request.Headers["Authorization"];
        }
    }
}