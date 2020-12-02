using System.Net;

namespace PontoWeb.Utils
{
    public class Result<T>
    {
        public Result()
        {
            Message = "";
            Error = false;
            StatusCode = HttpStatusCode.OK;
        }

        public T Data { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}