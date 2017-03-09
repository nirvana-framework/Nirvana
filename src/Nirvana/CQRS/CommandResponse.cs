using System;
using System.Collections.Generic;
using System.Linq;

namespace Nirvana.CQRS
{
    public class UIEventResponse : Response
    {


        private static UIEventResponse BuildResponse(bool isValid,List<ValidationMessage> validationMessages = null, Exception exception = null)
        {
            return new UIEventResponse
            {
                IsValid = isValid,
                ValidationMessages = validationMessages ?? new List<ValidationMessage>(),
                Exception = exception
            };
        }

        public static UIEventResponse Succeeded()
        {
            return BuildResponse(true);
        }
        public static UIEventResponse Succeeded(string message)
        {
            return BuildResponse(true,new List<ValidationMessage> {new ValidationMessage (MessageType.Info,"",message) });
        }
        public static UIEventResponse Failed()
        {
            return BuildResponse(false);
        }


    }
    public class InternalEventResponse : Response
    {
        private static InternalEventResponse BuildResponse(bool isValid, List<ValidationMessage> validationMessages = null, Exception exception = null)
        {
            return new InternalEventResponse
            {
                IsValid = isValid,
                ValidationMessages = validationMessages ?? new List<ValidationMessage>(),
                Exception = exception
            };
        }
        public static InternalEventResponse Succeeded()
        {
            return BuildResponse(true);
        }
        public static InternalEventResponse Succeeded(string message)
        {
            return BuildResponse(true, new List<ValidationMessage> { new ValidationMessage(MessageType.Info, "", message) });
        }
        public static InternalEventResponse Failed()
        {
            return BuildResponse(false);
        }
    }

    public class CommandResponse<TResult> : Response
    {
        public CommandResponse()
        {
            ValidationMessages = new List<ValidationMessage>();
        }


        public TResult Result { get; set; }

        public CommandResponse<TResult> Throw()
        {
            if (Exception != null)
            {
                throw Exception;
            }

            return this;
        }
    }

    public static class CommandResponse
    {
        private static CommandResponse<T> BuildResponse<T>(T result, bool isValid,
            List<ValidationMessage> validationMessages = null, Exception exception = null)
        {
            return new CommandResponse<T>
            {
                Result = result,
                IsValid = isValid,
                ValidationMessages = validationMessages ?? new List<ValidationMessage>(),
                Exception = exception
            };
        }

        public static CommandResponse<T> Succeeded<T>(T result)
        {
            return BuildResponse(result, true);
        }
        public static CommandResponse<T> Succeeded<T>(T result,string message)
        {
            return BuildResponse(result, true,new List<ValidationMessage> {new ValidationMessage(MessageType.Info, "",message)});
        }
        public static CommandResponse<T> Succeeded<T>(T result,List<ValidationMessage> messages)
        {
            return BuildResponse(result, true, messages);
        }

        public static CommandResponse<Nop> Succeeded()
        {
            return BuildResponse(Nop.NoValue, true, new List<ValidationMessage>());
        }


        public static CommandResponse<T> Failed<T>(params ValidationMessage[] messageses)
        {
            return BuildResponse(default(T), false, messageses?.ToList());
        }
        public static CommandResponse<T> Failed<T>(string message)
        {
            return BuildResponse(default(T), false, new List<ValidationMessage> {new ValidationMessage(MessageType.Error, "",message)});
        }

        public static CommandResponse<T> Failed<T>(T result, params ValidationMessage[] messageses)
        {
            return BuildResponse(result, false, messageses?.ToList());
        }
        public static CommandResponse<T> Failed<T>(T result, string message)
        {
            return BuildResponse(result, false, new List<ValidationMessage> { new ValidationMessage(MessageType.Error, "", message)}.ToList());
        }

        public static CommandResponse<T> Failed<T>(Exception exception)
        {
            return BuildResponse(default(T), false, null, exception);
        }
    }
}