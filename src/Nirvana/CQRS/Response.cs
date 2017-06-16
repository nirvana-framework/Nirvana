using System;
using System.Collections.Generic;

namespace Nirvana.CQRS
{
    public class Response
    {
        public Response()
        {
            ValidationMessages = new List<ValidationMessage>();
        }

        public IList<ValidationMessage> ValidationMessages { get; internal set; }
        public ExceptionSummay Exception { get; set; }
        
        //This is so we dont' pass this data around outside hte system
        public Exception GetException()
        {
            return internalException;
        }
        private Exception internalException;

        public Response SetException(Exception x)
        {
            if (x != null)
            {
                internalException = x;
                Exception = new ExceptionSummay
                {
                    Message = x.Message,
                    TypeName = x.GetType().FullName,
                    StackTrace = x.StackTrace
                };
            }
            return this;
        }

        public bool IsValid { get; set; }

        public bool Success()
        {
            return IsValid && Exception == null;
        }
    }

    public class ExceptionSummay
    {
        public string Message { get; set; }
        public string TypeName{ get; set; }
        public string StackTrace{ get; set; }
    }
}