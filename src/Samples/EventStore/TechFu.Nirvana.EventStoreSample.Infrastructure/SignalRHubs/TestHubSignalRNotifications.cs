using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.EventStoreSample.Services.Shared;
using TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications;
using TechFu.Nirvana.SignalRNotifications;

namespace TechFu.Nirvana.EventStoreSample.Infrastructure.SignalRHubs
{
    public class TestHubSignalRNotifications : NirvanaHubConnection<InfrastructureRoot>
    {
        public override Type[] Hubs => new[] {typeof(ITestNotificationHub)};

        public TestHubSignalRNotifications(INirvanaConfiguration configuration)
            : base(configuration.NotificationEndpoint)
        {
        }

        public void TestUiEvent(TestUiEvent testUiEvent)
        {
            Invoke<ITestNotificationHub>(x => x.TestUiEvent( testUiEvent));
        }
    }
}