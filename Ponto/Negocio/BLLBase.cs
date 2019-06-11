using Modelo;
using System;
using System.Net.Http;

namespace Negocio
{
    public abstract class BLLBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void TratarErroRetornoApi(HttpResponseMessage response, string metodoChamado)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            string erro = "";
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    erro = "Requisição não autorizada, solicitando novo token.";
                    Negocio.Configuracao.RequisitarNovoToken();
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    RetornoErro objetoErro = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoErro>(content);
                    erro = "Erro na chamada do método " + metodoChamado + " " + Environment.NewLine + " Erro: " + objetoErro.erroGeral + Environment.NewLine +
                                    " Para maiores detalhes verifique o objeto de erro." + Environment.NewLine +
                                    "Json de Erro Retornado: " + content;
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    erro = metodoChamado + " não encontrada!" + Environment.NewLine + " Json de Erro Retornado: " + content;
                    break;
                default:
                    erro = "Erro no método " + metodoChamado + " " + Environment.NewLine + " Erro: " + response.StatusCode + Environment.NewLine +
                                    "Json de Erro Retornado: " + content;
                    break;
            }
            if (String.IsNullOrEmpty(erro))
            {
                erro = "Erro ao requisitar "+metodoChamado+" "+ response.StatusCode.ToString();
            }
            log.Error(erro);
            throw new Exception(erro);
        }
    }
}
