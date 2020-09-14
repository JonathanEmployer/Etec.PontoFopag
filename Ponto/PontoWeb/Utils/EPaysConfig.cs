using Newtonsoft.Json;
using PontoWeb.Models;
using System.Net;
using System.Threading.Tasks;

namespace PontoWeb.Utils
{
    public class EPaysConfig
    {
        private readonly SyncAsync _SyncAsync;
        public EPaysConfig()
        {
            _SyncAsync = new SyncAsync();
        }
        public Result<string> PostToken(ParametersPontofopagDto Parameters)
        {
            var result = new Result<string>();
            _SyncAsync.HTTPVerb = HTTPVerb.POST;
            _SyncAsync.Url = UriMongo.Parameters;
            _SyncAsync.Obj = Parameters;
            var resultAux = _SyncAsync.GoSyncAsync();

            if (resultAux.StatusCode == HttpStatusCode.OK)
            {
                result = JsonConvert.DeserializeObject<Result<string>>(resultAux.Data);
            }
            else
            {
                result.Message = resultAux.Message;
                result.StatusCode = resultAux.StatusCode;
            }
            return result;
        }
        public Result<ParametersPontofopagDto> GetToken(string DataBaseName, string Cnpj)
        {
            var result = new Result<ParametersPontofopagDto>();
            _SyncAsync.HTTPVerb = HTTPVerb.GET;
            _SyncAsync.Url = $"{UriMongo.Parameters}?Name={DataBaseName}&Cnpj={Cnpj}";
            var resultAux = _SyncAsync.GoSyncAsync();

            if (resultAux.StatusCode == HttpStatusCode.OK)
            {
                result = JsonConvert.DeserializeObject<Result<ParametersPontofopagDto>>(resultAux.Data);
            }
            else
            {
                result.Message = resultAux.Message;
                result.StatusCode = resultAux.StatusCode;
            }
            return result;
        }
    }
}