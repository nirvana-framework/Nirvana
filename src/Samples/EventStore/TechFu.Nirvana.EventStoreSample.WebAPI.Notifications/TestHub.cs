using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechFu.Nirvana.EventStoreSample.Infrastructure.Io;
using TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications;
using TechFu.Nirvana.SignalRNotifications;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Notifications
{
    public class InfrastructureController : ApiControllerWithHub<TestNotificationHub>
    {
        private bool isHit;

        public InfrastructureController():base(new Serializer())
        {
            isHit = true;
        }
        [HttpPost]
        public HttpResponseMessage TestUiEvent([FromBody]TestUiEvent testUiEvent)
        {
            this.Hub.Clients.Group(GroupName(testUiEvent.GroupId.ToString())).testEvent(testUiEvent);
           return  Request.CreateResponse(HttpStatusCode.OK,new {});
        }

    }


    public class TestNotificationHub : UiNotificationHub
    {
       
    }
}