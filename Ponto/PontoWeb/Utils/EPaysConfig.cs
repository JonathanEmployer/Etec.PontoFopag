using Newtonsoft.Json;
using PontoWeb.Models;
using PontoWeb.Utils.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PontoWeb.Utils
{
    public class EPaysConfig : IEPaysConfig
    {
        private readonly ISyncAsync _iSyncAsync;
        public EPaysConfig(ISyncAsync ISyncAsync)
        {
            _iSyncAsync = ISyncAsync;
        }
        public async Task<Result<string>> PostToken(ConnectionDataBaseDto ConnectionDataBase)
        {
            var result = new Result<string>();
            _iSyncAsync.HTTPVerb = HTTPVerb.POST;
            _iSyncAsync.Url = UriMongo.Parameters;
            _iSyncAsync.Obj = ConnectionDataBase;
            var resultAux = await _iSyncAsync.GoSyncAsync();

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
    }
}