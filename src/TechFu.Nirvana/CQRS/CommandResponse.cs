using System;
using System.Collections.Generic;
using System.Linq;

namespace TechFu.Nirvana.CQRS
{
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