using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechFu.Nirvana.CQRS.UiNotifications;
using TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications;
using TechFu.Nirvana.SignalRNotifications;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications.Controllers
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