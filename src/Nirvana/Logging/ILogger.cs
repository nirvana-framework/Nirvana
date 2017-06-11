using System;
using System.Diagnostics.Eventing.Reader;

namespace Nirvana.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void DetailedDebug(string message);
        void Warning(string message);
        void Error(string message);
        void Exception(Exception ex,params string[]messages);
        
        bool LogDetailedDebug { get; }
        bool LogDebug{ get; }
        bool LogInfo{ get; }
        bool LogWarning{ get; }
        bool LogError{ get; }
        bool LogException{ get; }

    }


    public class ConsoleLogger : ILogger
    {
        public bool LogDetailedDebug { get; set; }
        public bool LogDebug{ get; set; }
        public bool LogInfo{ get; set; }
        public bool LogWarning{ get; set; }
        public bool LogError{ get; set; }
        public bool LogException{ get; set; }

        public ConsoleLogger(bool logDebug,bool logInfo,bool logWarning,bool logError,bool logException,bool logDetailedDebug)
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

        public void DetailedDebug(string message)
        {
            if (LogDetailedDebug)
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