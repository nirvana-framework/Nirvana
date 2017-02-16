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
        public Exception Exception { get; set; }

        public bool IsValid { get; set; }

        public bool Success()
        {
            return IsValid && Exception == null;
        }
    }
}