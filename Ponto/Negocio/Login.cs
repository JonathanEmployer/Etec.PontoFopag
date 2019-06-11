using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Negocio
{
    public class Login : BLLBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Método que realiza login na Webapi do ponto, caso ocorra erro retornará valor na propriedade Erro
        /// </summary>
        /// <param name="usuario">usuário</param>
        /// <param name="senha">Senha</param>
        /// <returns>Retorna o obj PxyConfigComunicadorServico com os dados do login e token</returns>
        public static async Task<Modelo.Proxy.PxyConfigComunicadorServico> EfetuarLogin(Modelo.Proxy.PxyConfigComunicadorServico login)
        {
            string usuario = login.Usuario;
            string senha = login.Senha;
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(ModeloAux.VariaveisGlobais.UrlWebAPi) })
                {
                    var token = client.PostAsync("Token",
                        new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string,string>("grant_type","password"),
                                new KeyValuePair<string,string>("username",login.Usuario),
                                new KeyValuePair<string,string>("password",login.Senha)
                            })).Result.Content.ReadAsAsync<Modelo.Proxy.PxyConfigComunicadorServico>().Result;
                    login = token;
                }
            }
            catch (Exception ec)
            {
                string msg = string.Empty;
                if (ec.Message.Contains("400 (Bad Request)"))
                {
                    msg = "Usuário/Senha incorretos.";
                }
                else
                {
                    msg = "Não foi possível contactar o servidor. ";
                }

                login = new Modelo.Proxy.PxyConfigComunicadorServico() { ErrorDescription = msg };
            }
            finally
            {
                login.Usuario = usuario;
                login.Senha = senha;
            }
            return login;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="senha"></param>
        /// <returns></returns>
        public static async Task<Modelo.Proxy.PxyConfigComunicadorServico> RealizarLogin(string usuario, string senha)
        {
            try
            {
                Modelo.Proxy.PxyConfigComunicadorServico login = new Modelo.Proxy.PxyConfigComunicadorServico();
                if (!String.IsNullOrEmpty(usuario) && !String.IsNullOrEmpty(senha))
                {
                    login.Usuario = usuario;
                    login.Senha = senha;
                    login = await Login.EfetuarLogin(login);

                    if (!String.IsNullOrEmpty(login.TokenAccess) && String.IsNullOrEmpty(login.Erro))
                    {
                        try
                        {
                            Modelo.Proxy.PxyConfigComunicadorServico config = Configuracao.CarregarXMLConfig();
                            config.Usuario = login.Usuario;
                            config.Senha = login.Senha;
                            config.TokenAccess = login.TokenAccess;
                            config.TokenExpires = login.TokenExpires;
                            config.TokenExpiresIn = login.TokenExpiresIn;
                            config.TokenIssuedAt = login.TokenIssuedAt;
                            config.TokenType = login.TokenType;
                            config.IdentificacaoServico = String.IsNullOrEmpty(config.IdentificacaoServico) ? Environment.MachineName : config.IdentificacaoServico;
                            config.Mac = String.IsNullOrEmpty(config.Mac) ? Util.GetMacAddress() : config.Mac;
                            Configuracao.SaveConfiguracao(config);
                        }
                        catch (Exception eg)
                        {
                            log.Error("Erro ao gravar configuração, erro: " + eg.Message, eg);
                            login.ErrorDescription = "Erro ao gravar configuração, erro: " + eg.Message;
                        }
                    }
                    else
                    {
                        login.Erro = "Ocorreu um erro ao realizar o login:\r\n" + login.ErrorDescription;
                    }
                    return login;
                }
                else
                {
                    return login;
                }
            }
            catch (Exception e)
            {
                log.Error("Erro ao realizar login", e);
                throw e;
            }
        }
    }
}
