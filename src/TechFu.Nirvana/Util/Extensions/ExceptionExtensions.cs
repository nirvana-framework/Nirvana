using System;
using System.Collections.Generic;

namespace Nirvana.Util.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception InnermostException(this Exception exception)
        {
            if (exception == null)
                return null;

            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception;
        }

        public static IEnumerable<Exception> InnerExceptions(this Exception exception)
        {
            var exceptions = new List<Exception> {exception};

            var currentEx = exception;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                exceptions.Add(currentEx);
            }

            return exceptions;
        }
    }
}