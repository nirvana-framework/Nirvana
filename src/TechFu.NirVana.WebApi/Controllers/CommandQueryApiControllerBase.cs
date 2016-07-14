using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.WebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public abstract class CommandQueryApiControllerBase : ApiController
    {
        public IMediator Mediator { get; set; }

        protected CommandQueryApiControllerBase()
        {
            
            Mediator = (IMediator)NirvanaSetup.GetService(typeof(IMediator));
        }

        protected HttpResponseMessage Command<TResult>(Command<TResult> command)
        {
            var response = Mediator.Command(AddAuthToCommand(command));
            LogException(response);
            return Request.CreateResponse(GetResponseCode(response), PrepCommand(response));
        }

        private Command<TResult> AddAuthToCommand<TResult>(Command<TResult> command)
        {
            var authCommand = command as AuthorizedCommand<TResult>;

            if (authCommand != null)
            {
                authCommand.AuthCode = GetAuthCode();
            }
            return command;
        }

        private string GetAuthCode()
        {
            var httpControllerContext = ControllerContext;
            if (httpControllerContext != null)
            {
                var authHeader = httpControllerContext.Request.Headers.Authorization;
                return authHeader.Parameter;
            }
            return string.Empty;
        }

        private Query<TResult> AddAuthToQuery<TResult>(Query<TResult> query)
        {
            var authorizedQuery = query as AuthorizedQuery<TResult>;

            if (authorizedQuery != null)
            {
                authorizedQuery.AuthCode = GetAuthCode();
            }
            return query;
        }

        protected HttpResponseMessage Query<TResult>(Query<TResult> query)
        {
            var response = Mediator.Query(AddAuthToQuery(query));
            LogException(response);
            return Request.CreateResponse(GetResponseCode(response), PrepQuery(response));
        }
        
        protected HttpResponseMessage QueryForFile<TResult>(Query<TResult> query)
            where TResult : FileQueryResult
        {
            var response = Mediator.Query(query);

            var httpResponseMessage = Request.CreateResponse(GetResponseCode(response), response.Result.FileBytes);


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


        private object PrepCommand<T>(CommandResponse<T> response)
        {
            return new
            {
                Exception = new {Message = response.Exception},
                response.ValidationMessages,
                response.Result,
                response.IsValid
            };
        }

        private object PrepQuery<T>(QueryResponse<T> response)
        {
            return new
            {
                Exception = new {Message = response.Exception},
                response.ValidationMessages,
                response.Result,
                response.IsValid
            };
        }

        private HttpStatusCode GetResponseCode<T>(CommandResponse<T> input)
        {
            return input.Exception != null
                ? HttpStatusCode.InternalServerError
                : input.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }

        private HttpStatusCode GetResponseCode<T>(QueryResponse<T> input)
        {
            return input.Exception != null
                ? HttpStatusCode.InternalServerError
                : input.Success() ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
        }


        private static void LogException(Response response)
        {
            if (response.Exception != null)
            {
                //Logging.Exception(response.Exception);
            }
        }
    }
}