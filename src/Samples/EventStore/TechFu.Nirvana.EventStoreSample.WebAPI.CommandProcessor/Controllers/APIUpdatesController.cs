using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TechFu.Nirvana.Util;
using TechFu.Nirvana.WebApi;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor.Controllers
{
    public class ApiUpdatesController : CommandQueryApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage GetA2Updates()
        {
            byte[] myByteArray = Encoding.UTF8.GetBytes(new Angular2CqrsGenerator().GetV2Items());
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