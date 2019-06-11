using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Utils
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> GetAllExceptions(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx;
            }
        }

        public static IEnumerable<string> GetAllExceptionAsString(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx.ToString();
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx.ToString();
            }
        }

        public static IEnumerable<string> GetAllExceptionMessages(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx.Message;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx.Message;
            }
        }
    }
}