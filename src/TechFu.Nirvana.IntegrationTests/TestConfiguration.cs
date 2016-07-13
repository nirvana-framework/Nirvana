using TechFu.Nirvana.Configuration;

namespace TechFu.Nirvana.IntegrationTests
{
    internal static class TestConfiguration
    {
        public static bool Configured;

        public static void Configure()
        {
            if (Configured)
            {
                return;
            }

            Configured = true;
            NirvanaSetup
                .Configure()
                .ForCommands()
                .WithQueues();
        }
    }
}