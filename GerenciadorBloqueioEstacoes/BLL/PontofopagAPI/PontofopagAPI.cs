using BLL.PontofopagAPI.ModeloAPI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BLL.PontofopagAPI
{
    public class PontofopagAPI : IDisposable
    {

        #region Construtor

        private Modelo.Acesso.AcessoAPI _acesso;
        public PontofopagAPI(Modelo.Acesso.AcessoAPI acesso)
        {
            this._acesso = acesso;
        }

        public void Dispose()
        {
        }

        #endregion

        #region Autenticação

        public event Action<Modelo.Acesso.AcessoAPI> AoAtualizarToken;
        public TokenResponse Autenticar()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_acesso.Url);
                HttpContent requisicao = new StringContent("grant_type=password&username=" + _acesso.Usuario + "&password=" + _acesso.Senha);
                requisicao.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = client.PostAsync("token", requisicao).Result;
                response.EnsureSuccessStatusCode();
                TokenResponse result = response.Content.ReadAsAsync<TokenResponse>().Result;
                _acesso.Token = result.AccessToken;
                if(AoAtualizarToken != null)
                    AoAtualizarToken(_acesso);
                return result;
            }
        }

        #endregion

        #region Client

        protected HttpClient CriarClient()
        {
            HttpClient client = new HttpClient();
            PrepararClient(client);
            return client;
        }

        protected void PrepararClient(HttpClient client)
        {
            string url = _acesso.Url;
            if (!url.EndsWith("/")) url += "/";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Clear();
            client.Timeout = TimeSpan.FromMinutes(5);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "bearer " + _acesso.Token);
        }

        #endregion

        #region Processar

        protected T Processar<T>(Func<HttpResponseMessage> acao)
        {
            HttpResponseMessage response = acao();
            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                Autenticar();
                response = acao();
            }
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                return response.Content.ReadAsAsync<T>().Result;
            }
            if (!response.IsSuccessStatusCode)
            {
                dynamic responseForInvalidStatusCode = response.Content.ReadAsAsync<dynamic>();
                Newtonsoft.Json.Linq.JContainer msg = responseForInvalidStatusCode.Result;
                Modelo.Utils.Utils.EscreveLog("Log", "Erro ao consultar a api, método = " + response.RequestMessage.RequestUri + ", Erro = " + msg.ToString());
            }
            
            return default(T);
        }

        #endregion

        #region Carregar bloqueios
        public List<EstadoBloqueioFuncionario> CarregarBloqueios(List<string> cpfs)
        {
            List<EstadoBloqueioFuncionario> retorno = Processar<List<EstadoBloqueioFuncionario>>(() =>
            {
                HttpClient client = new HttpClient();
                PrepararClient(client);
                string uri = string.Format("api/BloqueioEstacoes");
                return client.PostAsJsonAsync(uri, cpfs.ToArray()).Result;
            });
            return retorno;
        }
        #endregion

        #region CarregarFuncionarios
        public List<ModeloAPI.Funcionario> CarregarFuncionarios()
        {
            List<ModeloAPI.Funcionario> retorno = Processar<List<ModeloAPI.Funcionario>>(() =>
            {
                HttpClient client = new HttpClient();
                PrepararClient(client);
                string uri = string.Format("api/Funcionario/CarregarAtivosBloqueio");
                return client.GetAsync(uri).Result;
            });
            return retorno;
        }

        public List<ModeloAPI.Funcionario> CarregarFuncionarios(System.Web.Caching.Cache cache, bool recarregar = false)
        {
            string chaveCacheFuncionarios = "_cacheFuncionariosAPI";
            List<ModeloAPI.Funcionario> funcionarios = cache[chaveCacheFuncionarios] as List<ModeloAPI.Funcionario>;
            if (funcionarios == null || recarregar)
            {
                funcionarios = this.CarregarFuncionarios();
                cache.Insert(
                    chaveCacheFuncionarios, funcionarios, null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration, new System.TimeSpan(0, 15, 0));
            }
            return funcionarios;
        }

        #endregion

    }
}
