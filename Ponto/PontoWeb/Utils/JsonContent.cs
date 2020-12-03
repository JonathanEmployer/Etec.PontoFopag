using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace PontoWeb.Utils
{
    public class JsonContent : StringContent
    {
        public JsonContent(object value) : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
        {

        }
        public JsonContent(object value, string Model) : base(JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        }), Encoding.UTF8, "application/json")
        {

        }
    }
}