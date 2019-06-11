using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
 
namespace BLL_N.IntegracaoTerceiro.API
{
    public class ApiException : Exception
    {
        public HttpResponseMessage Response { get; set; }
        public ApiException(HttpResponseMessage response)
        {
            this.Response = response;
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return this.Response.StatusCode;
            }
        }


        public IEnumerable<string> Errors
        {
            get
            {
                return this.Data.Values.Cast<string>().ToList();
            }
        }

        public static ApiException CreateApiException(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject =
                new { message = "", ModelState = new Dictionary<string, string[]>() };

            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            // Now wrap into an exception which best fullfills the needs of your application:
            var ex = new ApiException(response);

            // Sometimes, there may be Model Errors:
            if (deserializedErrorObject.ModelState != null)
            {
                var errors =
                    deserializedErrorObject.ModelState
                                            .Select(kvp => string.Join(". ", kvp.Value));
                for (int i = 0; i < errors.Count(); i++)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }
            // Othertimes, there may not be Model Errors:
            else
            {
                var error =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var kvp in error)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(kvp.Key, kvp.Value);
                }
            }
            return ex;
        }
    }
}
