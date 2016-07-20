using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using TechFu.Nirvana.CQRS.UiNotifications;
using TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications;
using TechFu.Nirvana.SignalRNotifications;

namespace Notifications.Console
{
    
    public class InfrastructureController : ApiControllerWithHub<EventHub>
    {
        [HttpPost]
        public HttpResponseMessage TestUiEvent([FromBody]TestUiEvent testUiEvent)
        {
            PublishEvent("TestUiEvent", testUiEvent, Constants.TaskChannel);
            return Request.CreateResponse(HttpStatusCode.OK, new { });
        }
    }
}