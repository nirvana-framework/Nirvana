using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications;
using TechFu.Nirvana.SignalRNotifications;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications
{
    public class InfrastructureController : ApiControllerWithHub<TestNotificationHub>
    {

        public InfrastructureController():base()
        {
        }
        [HttpPost]
        public HttpResponseMessage TestUiEvent([FromBody]TestUiEvent testUiEvent)
        {
            
           return  Request.CreateResponse(HttpStatusCode.OK,new {});
        }

    }


    public class TestNotificationHub : UiNotificationHub
    {
       
    }
}