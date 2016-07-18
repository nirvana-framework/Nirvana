using System;
using TechFu.Nirvana.CQRS.UiNotifications;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.UiNotifications
{
    public interface ITestNotificationHub:INirvanaNotificationHub<InfrastructureRoot>
    {
        void TestUiEvent(TestUiEvent testUiEvent);
    }
}