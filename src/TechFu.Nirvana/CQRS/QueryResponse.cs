using System;
using System.Collections.Generic;
using System.Linq;

namespace TechFu.Nirvana.CQRS
{
    public class QueryResponse<T> : Response
    {
        public T Result { get; set; }

        public QueryResponse<T> Throw()
        {
            if (Exception != null)
            {
                throw Exception;
            }

            return this;
        }
    }

    public static class QueryResponse
    {
        private static QueryResponse<T> BuildResult<T>(T result, bool isValid, IList<ValidationMessage> messages = null,
            Exception exception = null)
        {
            return new QueryResponse<T>
            {
                ValidationMessages = messages ?? new List<ValidationMessage>(),
                Exception = exception,
                IsValid = isValid,
                Result = result
            };
        }

        public static QueryResponse<T> Succeeded<T>(T result)
        {
            return BuildResult(result, true);
        }
        public static QueryResponse<T> Succeeded<T>(T result,string messsage)
        {
            return BuildResult(result, true,new List<ValidationMessage>() {new ValidationMessage(MessageType.Error, "",messsage)});
        }

        public static QueryResponse<T> Failed<T>()
        {
            return BuildResult(default(T), false, null, null);
        }

        public static QueryResponse<T> Failed<T>(Exception exception)
        {
            return BuildResult(default(T), false, null, exception);
        }

        public static QueryResponse<T> Failed<T>(T result, params ValidationMessage[] messages)
        {
            return BuildResult(default(T), false, messages?.ToList());
        }
        public static QueryResponse<T> Failed<T>(string message)
        {
            return BuildResult(default(T), false, new List<ValidationMessage>() { new ValidationMessage(MessageType.Error, "", message) });
        }
        
    }
}