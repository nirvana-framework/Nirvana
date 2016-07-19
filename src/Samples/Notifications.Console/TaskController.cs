using System.Threading;
using System.Web.Http;
using TechFu.Nirvana.EventStoreSample.Infrastructure.Io;
using TechFu.Nirvana.SignalRNotifications;

namespace Notifications.Console
{
    [RoutePrefix("tasks")]
    public class TaskController : ApiControllerWithHub<EventHub>
    {
        private readonly string _channel = Constants.TaskChannel;
        
        public TaskController() : base(new Serializer())
        {
            
        }


        [Route("long")]
        [HttpGet]
        public IHttpActionResult GetLongTask()
        {
            double steps = 10;
            var eventName = "longTask.status";
            ExecuteTask(eventName, steps);
            return Ok("Long task complete");
        }


        [Route("short")]
        [HttpGet]
        public IHttpActionResult GetShortTask()
        {
            double steps = 5;
            var eventName = "shortTask.status";
            ExecuteTask(eventName, steps);

            return Ok("Short task complete");
        }

        private void ExecuteTask(string eventName, double steps)
        {
            var status = new TaskStatus
            {
                State = "starting",
                PercentComplete = 0.0
            };

            PublishEvent(eventName, status,_channel);

            for (double i = 0; i < steps; i++)
            {
                status.State = "working";
                status.PercentComplete = i/steps*100;
                PublishEvent(eventName, status, _channel);
                Thread.Sleep(500);
            }

            status.State = "complete";
            status.PercentComplete = 100;
            PublishEvent(eventName, status, _channel);
        }

        
    }
}