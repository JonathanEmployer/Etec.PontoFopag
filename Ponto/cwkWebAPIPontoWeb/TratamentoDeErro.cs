using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace cwkWebAPIPontoWeb
{
    public class TratamentoDeErro : ActionFilterAttribute
    {
        public static HttpResponseException NaoEncontrado(string msg)
        {
            return TrataErro(msg, HttpStatusCode.NotFound);
        }

        private static HttpResponseException TrataErro(string msg, HttpStatusCode httpStatusCode)
        {
            HttpResponseMessage response = new HttpResponseMessage(httpStatusCode);
            response.ReasonPhrase = msg;
            response.Content = new StringContent(msg);

            throw new HttpResponseException(response);
        }
    }
}