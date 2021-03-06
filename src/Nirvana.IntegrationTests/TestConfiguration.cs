﻿using Nirvana.Configuration;

namespace Nirvana.AzureQueues.IntegrationTests
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
                .ForCommands(MediationStrategy.InProcess, MediationStrategy.ForwardToQueue, MediationStrategy.ForwardToQueue)
                .BuildConfiguration();
        }
    }
}