﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Nirvana.CQRS
{
    public class QueryResponse<T> : Response
    {
        public T Result { get; set; }

        public QueryResponse<T> Throw()
        {
            if (Exception != null)
            {
                throw GetException();
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
                
                IsValid = isValid,
                Result = result
            }.SetException(exception) as QueryResponse<T>;
        }

        public static QueryResponse<T> Success<T>(T result)
        {
            return BuildResult(result, true);
        }
        public static QueryResponse<T> Success<T>(T result,string messsage)
        {
            return BuildResult(result, true,new List<ValidationMessage>() {new ValidationMessage(MessageType.Info, "",messsage)});
        }
        public static QueryResponse<T> Success<T>(T result, params ValidationMessage[] messages)
        {
            return BuildResult(default(T), false, messages?.ToList());
        }

        public static QueryResponse<T> Fail<T>()
        {
            return BuildResult(default(T), false, null, null);
        }

        public static QueryResponse<T> Fail<T>(Exception exception)
        {
            return BuildResult(default(T), false, null, exception);
        }

        public static QueryResponse<T> Fail<T>(T result, params ValidationMessage[] messages)
        {
            return BuildResult(default(T), false, messages?.ToList());
        }
        public static QueryResponse<T> Fail<T>(string message)
        {
            return BuildResult(default(T), false, new List<ValidationMessage>() { new ValidationMessage(MessageType.Error, "", message) });
        }
        
    }
}