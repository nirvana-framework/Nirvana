using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nirvana.Configuration;
using Nirvana.Mediation;
using Nirvana.Util;
using Nirvana.Util.Io;

namespace Nirvana.Web.Controllers
{
    [Route("api/ApiUpdates")]
    public class ApiUpdatesController : CommandQueryApiControllerBase
    {
        private readonly NirvanaSetup _setup;

        public ApiUpdatesController(NirvanaSetup setup, IMediatorFactory mediator,ISerializer serializer) : base(mediator,serializer)
        {
            _setup = setup;
        }

        [HttpGet]
        public HttpResponseMessage GetA2Updates()
        {
            byte[] myByteArray = Encoding.UTF8.GetBytes(new Angular2CqrsGenerator(_setup).GetV2Items());
            MemoryStream stream = new MemoryStream(myByteArray);

            var contentRresult = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            contentRresult.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            var value = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "CommandQuery.ts"
            };
            contentRresult.Content.Headers.ContentDisposition = value;
            return contentRresult;

        }
    }
}