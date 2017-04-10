using System;
using System.Diagnostics.Eventing.Reader;

namespace Nirvana.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Warning(string message);
        void Error(string message);
        void Exception(Exception ex,params string[]messages);
    }


    public class ConsoleLogger : ILogger
    {
        public bool LogDebug;
        public bool LogInfo;
        public bool LogWarning;
        public bool LogError;
        public bool LogException;

        public ConsoleLogger(bool logDebug,bool logInfo,bool logWarning,bool logError,bool logException)
        {
            LogDebug = logDebug;
            LogInfo = logInfo;
            LogWarning = logWarning;
            LogError = logError;
            LogException = logException;
        }

        public void Info(string message)
        {
            if (LogInfo)
            {
                Console.WriteLine(message);
            }
        }

        public void Debug(string message)
        {
            if (LogDebug)
            {
                Console.WriteLine(message);
            }
        }

        public void Warning(string message)
        {
            if (LogWarning)
            {
                Console.WriteLine(message);
            }
        }

        public void Error(string message)
        {
            if (LogError)
            {
                Console.WriteLine(message);
            }
        }

        public void Exception(Exception ex, params string[] messages)
        {
            throw new NotImplementedException();
        }

        public void Exception(Exception ex)
        {
            if (LogException)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}