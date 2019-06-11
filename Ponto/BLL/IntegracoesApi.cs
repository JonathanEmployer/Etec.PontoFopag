using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace BLL
{
    public static class IntegracoesApi
    {

        private static string enderecoApi = WebConfigurationManager.AppSettings["IntegracoesApi"];
        private static string idAplicacaoNaApiIntegracoes = WebConfigurationManager.AppSettings["IdAplicacaoApiIntegracoes"];

        #region Iniciar, Cancelar, Finalizar, Cadastrar uma "Integração" e Buscar uma "Integração"

        public static string IniciarIntegracaoExecucao(IniciarIntegracaoExecucao objIniciarIntegracaoExecucao)
        {
            return GerericEnviarDadosIntegracao(
                        objIniciarIntegracaoExecucao,
                        MontarUri("api/Integracao/Iniciar")
                    );
        }

        public static string FinalizarIntegracaoExecucao(FinalizarIntegracaoExecucao objFinalizarIntegracaoExecucao)
        {
            return GerericEnviarDadosIntegracao(
                        objFinalizarIntegracaoExecucao,
                        MontarUri("api/Integracao/Finalizar")
                    );
        }

        public static string CancelarIntegracaoExecucao(CancelarIntegracaoExecucao objCancelarIntegracaoExecucao)
        {
            return GerericEnviarDadosIntegracao(
                        objCancelarIntegracaoExecucao,
                        MontarUri("api/Integracao/Cancelar")
                    );
        }

        public static int CriarIntegracao(CadastrarIntegracao objNovaIntegracao)
        {
            int inteiro = 0;
            objNovaIntegracao.IdAplicacao = !string.IsNullOrWhiteSpace(objNovaIntegracao.IdAplicacao) ? objNovaIntegracao.IdAplicacao : idAplicacaoNaApiIntegracoes;
            string retorno = CriarNovaIntegracao(
                        objNovaIntegracao,
                        MontarUri("api/Integracao")
                    );
            return int.TryParse(Regex.Match(retorno, @"[0-9]+").Value, out inteiro) ? inteiro : 0;
        }

        public static RetornoIntegracao VerificaExistenciaIntegracao(ObterIntegracao objIntegracao)
        {
            string idAplicacao = !string.IsNullOrWhiteSpace(idAplicacaoNaApiIntegracoes) ? idAplicacaoNaApiIntegracoes : "";

            RetornoIntegracao retorno = ObterIntegracao(
                        objIntegracao,
                        MontarUri("api/Integracao/ObterPorDescricao?descricao=" + objIntegracao.descricao + "&idAplicacao=" + idAplicacao)
                    );
            return retorno;
        }

        #endregion

        #region Metodos Auxiliares Internos

        /// <summary>
        /// Envia os dados para Integração na Api de integrações, podendo ser utilizado por Iniciar / Finalizar / Cancelar
        /// </summary>
        /// <param name="objEnviar"></param>
        /// <param name="url"></param>
        /// <returns>String(Json) { idIntegracaoExecucao, StatusCode }</returns>
        private static string GerericEnviarDadosIntegracao(EntidadeBase objEnviar, Uri url)
        {
            Dictionary<string, string> retorno = new Dictionary<string, string>();

            var objEnviarSerializado = new StringContent(JsonConvert.SerializeObject(objEnviar), Encoding.UTF8, "application/json");
            HttpResponseMessage response = SimpleSend(objEnviarSerializado, url);

            string retornoContent = response.Content.ReadAsStringAsync().Result;

            if (!retorno.ContainsKey("idIntegracaoExecucao"))
                retorno.Add("idIntegracaoExecucao", RetornaIdIntegracaoExecucao(retornoContent));
            if (!retorno.ContainsKey("StatusCode"))
                retorno.Add("StatusCode", response.StatusCode.ToString());

            return JsonConvert.SerializeObject(retorno);
        }

        private static string CriarNovaIntegracao(CadastrarIntegracao objEnviar, Uri url)
        {
            Dictionary<string, string> retorno = new Dictionary<string, string>();

            var objEnviarSerializado = new StringContent(JsonConvert.SerializeObject(objEnviar), Encoding.UTF8, "application/json");

            HttpResponseMessage response = SimpleSend(objEnviarSerializado, url);
            string retornoContent = response.Content.ReadAsStringAsync().Result;

            if (!retorno.ContainsKey("idIntegracao"))
                retorno.Add("idIntegracao", RetornaIdIntegracao(retornoContent));
            if (!retorno.ContainsKey("StatusCode"))
                retorno.Add("StatusCode", response.StatusCode.ToString());

            return JsonConvert.SerializeObject(retorno);
        }

        private static RetornoIntegracao ObterIntegracao(ObterIntegracao objEnviar, Uri url)
        {
            RetornoIntegracao objRetornoIntegracao = new RetornoIntegracao();

            var objEnviarSerializado = new StringContent(JsonConvert.SerializeObject(objEnviar), Encoding.UTF8, "application/json");

            HttpResponseMessage response = SimpleSend(objEnviarSerializado, url, false);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
                objRetornoIntegracao = Newtonsoft.Json.JsonConvert.DeserializeObject<RetornoIntegracao>(response.Content.ReadAsStringAsync().Result);

            return objRetornoIntegracao;
        }

        private static HttpResponseMessage SimpleSend(StringContent conteudo, Uri url, bool post = true)
        {
            using (var client = new HttpClient())
            {
                if (post)
                    return client.PostAsync(url, conteudo).Result;
                else
                    return client.GetAsync(url).Result;
            }
        }

        private static string RetornaIdIntegracao(string jsonRetorno)
        {
            return (Regex.Match(
                        Regex.Match(jsonRetorno, @"Id""[\:]( *)[0-9]+", RegexOptions.IgnoreCase).Value, @"[0-9]+").Value
                    );
        }

        private static string RetornaIdIntegracaoExecucao(string jsonRetorno)
        {
            return (Regex.Match(
                        Regex.Match(jsonRetorno, @"IdIntegracaoExecucao""[\:]( *)[0-9]+", RegexOptions.IgnoreCase).Value, @"[0-9]+").Value
                    );
        }

        private static Uri MontarUri(string relative)
        {
            Uri serverUri = new Uri(enderecoApi += "/");
            Uri relativeUri = new Uri(relative, UriKind.Relative);

            return new Uri(serverUri, relativeUri);
        }

        #endregion
    }

    #region Classes Template Piloto

    public class IniciarIntegracaoExecucao : EntidadeBase
    {
        public int IdIntegracao { get; set; }
        public string CentroServico { get; set; }
        public string ValorRecebido { get; set; }

        // Construtores
        public IniciarIntegracaoExecucao() { }
        public IniciarIntegracaoExecucao(int IdIntegracao, string Usuario, string CentroServico, string ValorRecebido)
        {
            this.IdIntegracao = IdIntegracao;
            this.Usuario = Usuario;
            this.CentroServico = CentroServico;
            this.ValorRecebido = ValorRecebido;
        }
    }

    public class FinalizarIntegracaoExecucao : EntidadeBase
    {
        public int IdIntegracaoExecucao { get; set; }
        public string ValorDevolvido { get; set; }

        //Construtores
        public FinalizarIntegracaoExecucao() { }
        public FinalizarIntegracaoExecucao(int IdIntegracaoExecucao, string Usuario, string ValorDevolvido)
        {
            this.IdIntegracaoExecucao = IdIntegracaoExecucao;
            this.Usuario = Usuario;
            this.ValorDevolvido = ValorDevolvido;
        }
    }

    public class CancelarIntegracaoExecucao : EntidadeBase
    {
        public int IdIntegracaoExecucao { get; set; }
        public string ValorDevolvido { get; set; }

        // Construtores
        public CancelarIntegracaoExecucao() { }
        public CancelarIntegracaoExecucao(int IdIntegracaoExecucao, string Usuario, string ValorDevolvido)
        {
            this.IdIntegracaoExecucao = IdIntegracaoExecucao;
            this.Usuario = Usuario;
            this.ValorDevolvido = ValorDevolvido;
        }
    }

    public class CadastrarIntegracao
    {
        public string IdAplicacao { get; set; }
        public string Nome { get; set; }

        public CadastrarIntegracao() { }
        public CadastrarIntegracao(string Nome)
        {
            this.Nome = Nome;
        }
    }

    public class ObterIntegracao
    {
        public string descricao { get; set; }

        public ObterIntegracao() { }
        public ObterIntegracao(string descricao)
        {
            this.descricao = descricao;
        }
    }

    public class EntidadeBase
    {
        public string Usuario { get; set; }
        public DateTime DtaCadastro { get; protected set; }

        public EntidadeBase()
        {
            DtaCadastro = DateTime.Now;
        }
    }

    #endregion

    #region Entidades Serialize Retorno

    public class RetornoIntegracao
    {
        public string Id { get; set; }
        public string StatusCode { get; protected set; }
    }

    public class RetornoIntegracaoExecucao
    {
        public string IdIntegracaoExecucao { get; set; }
        public string StatusCode { get; protected set; }
    }

    #endregion
}