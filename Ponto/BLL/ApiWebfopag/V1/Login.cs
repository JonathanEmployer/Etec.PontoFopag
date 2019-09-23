using Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.ApiWebfopag.V1
{
    public class Login
    {
        public TokenResponse LoginAsync(string usuario, string senha, string CS, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                string str = "grant_type=password&username=" +usuario + "&password=" + senha;

                HttpContent content = new StringContent(str);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.Add("cs", CS);

                string txtUri = ConfigurationManager.AppSettings["ApiWebfopagV1"];
                httpClient.BaseAddress = new Uri(txtUri);
                HttpResponseMessage response = httpClient.PostAsync("Token", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();

                    TokenResponse token = response.Content.ReadAsAsync<TokenResponse>().Result;

                    if (token.ErrorDescription != null)
                    {
                        var erro = token.ErrorDescription;
                    }

                    var tokenkey = token.AccessToken;
                    var validade = token.ExpirationDate;
                    return token;
                }
                else
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }

            }
        }
    }
}
