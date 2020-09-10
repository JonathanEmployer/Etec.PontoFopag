using Newtonsoft.Json;
using IdentityModel.Client;
using PontoWeb.Utils.Interface;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PontoWeb.Utils
{
    public class SyncAsync : IDisposable, ISyncAsync
    {
        public object Obj { get; set; }
        public HTTPVerb HTTPVerb { get; set; }
        public string Url { get; set; }
        public string SerializeModel { get; set; }
        public static string Token { get; set; }

        private readonly HttpClient _httpclient;
        private HttpClient _httpclientToken;
        private Task<HttpResponseMessage> _taskhttpmessage;
        private HttpResponseMessage _httpmessage;

        public SyncAsync(string User, string Password)
        {
            _httpclient = new HttpClient();
        }
        public SyncAsync()
        {
            _httpclient = new HttpClient();
        }
        private HttpResponseMessage Post()
        {
            _taskhttpmessage = _httpclient.PostAsync(Url, new JsonContent(Obj, SerializeModel));
            return Wait();
        }
        private HttpResponseMessage Get()
        {
            _taskhttpmessage = _httpclient.GetAsync(Url);
            return Wait();
        }
        private HttpResponseMessage Put()
        {
            _taskhttpmessage = _httpclient.PutAsync(Url, new JsonContent(Obj, SerializeModel));
            return Wait();
        }
        private HttpResponseMessage Delete()
        {
            _taskhttpmessage = _httpclient.DeleteAsync(Url);
            return Wait();
        }
        private HttpResponseMessage Wait()
        {
            _taskhttpmessage.Wait();
            return _taskhttpmessage.Result;
        }
        public async Task<Result<string>> GoSyncAsync()
        {
            if (Token == null && !Url.ToLower().Contains("token"))
            {
                await GetToken();
            }
            else if (Token != null)
            {
                _httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Token);
            }

            Result<string> result = new Result<string>();
            try
            {
                switch (HTTPVerb)
                {
                    case HTTPVerb.GET:
                        _httpmessage = Get();
                        break;
                    case HTTPVerb.POST:
                        _httpmessage = Post();
                        break;
                    case HTTPVerb.PUT:
                        _httpmessage = Put();
                        break;
                    case HTTPVerb.DELETE:
                        _httpmessage = Delete();
                        break;
                    default:
                        break;
                }
                Task<string> retorno = _httpmessage.Content.ReadAsStringAsync();
                retorno.Wait();
                result.StatusCode = _httpmessage.StatusCode;

                if (_httpmessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<Result<string>>(retorno.Result);
                    }
                    catch (Exception)
                    {
                        result.Message = JsonConvert.DeserializeObject<Result<string>>(retorno.Result).Message;
                    }
                }
                else if (_httpmessage.StatusCode == HttpStatusCode.Forbidden)
                {
                    result.Data = retorno.Result;
                }
                else if (_httpmessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await GetToken();
                    return await GoSyncAsync();
                }
                else if (_httpmessage.StatusCode != HttpStatusCode.OK)
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<Result<string>>(retorno.Result);
                    }
                    catch (Exception)
                    {
                        result.Message = JsonConvert.DeserializeObject<Result<string>>(retorno.Result).Message;
                    }
                }
                else
                {
                    result.Data = retorno.Result;
                }
            }
            catch (InvalidOperationException ex)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<Result<string>> GetToken()
        {
            var result = new Result<string>();

            _httpclientToken = new HttpClient();

            var tokenResponse = await _httpclient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = TokenUri.Token,
                ClientId = UserToken.User,
                ClientSecret = UserToken.Password,
                Scope = "ApiMongo"
            });

            if (tokenResponse.IsError)
            {
                result.Error = true;
                result.StatusCode = tokenResponse.HttpStatusCode;
                result.Message = $"{ tokenResponse.ErrorDescription } *** { tokenResponse.Error}";
            }
            else
            {
                result.Data = tokenResponse.AccessToken;
            }

            if (!result.Error)
            {
                var token = result.Data;
                _httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
            }
            return result;
        }

        public void Dispose()
        {
            _httpclient.Dispose();
            _httpmessage.Dispose();
        }
    }
    public enum HTTPVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}