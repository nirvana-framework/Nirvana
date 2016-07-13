using System;
using TechFu.Nirvana.Configuration;

namespace TechFu.Nirvana.CQRS.Queue
{
    internal static class QueueRouer
    {
        public static bool CheckLongRunningCommand(Type taskType)
        {
            return NirvanaSetup.QueueStrategy == QueueStrategy.None
                ||
                (NirvanaSetup.QueueStrategy == QueueStrategy.LongRunningCommands &&
                 !IsLongRunningTask(taskType));
        }

        private static bool IsLongRunningTask(Type taskType)
        {
            throw new NotImplementedException();
        }
    }
}