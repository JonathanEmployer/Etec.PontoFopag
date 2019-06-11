using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Models.Helpers
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<Error> AllErrors(this ModelStateDictionary modelState)
        {
            var result = new List<Error>();
            var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any())
                                            .Select(x => new { x.Key, x.Value.Errors });

            foreach (var erroneousField in erroneousFields)
            {
                var fieldKey = erroneousField.Key;
                var fieldErrors = erroneousField.Errors
                                   .Select(error => new Error(fieldKey, error.ErrorMessage));
                result.AddRange(fieldErrors);
            }

            return result;
        }

        public static JsonResult JsonValidation(this ModelStateDictionary state)
        {
            return new JsonResult
            {
                Data = new
                {
                    Tag = "ValidationError",
                    State = from e in state
                            where e.Value.Errors.Count > 0
                            select new
                            {
                                Name = e.Key,
                                Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                                   .Concat(e.Value.Errors.Where(x => x.Exception != null).Select(x => x.Exception.Message))
                            }
                }
            };
        }

        public static JsonResult JsonErrorResult(this ModelStateDictionary state)
        {
            return new JsonResult
            {
                Data = new
                {
                    success = false,
                    errors = state.AllErrors()
                }
            };
        }
    }

    public class Error
    {
        public Error(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }
    }
}