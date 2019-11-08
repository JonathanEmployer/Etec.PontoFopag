using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    public class ExportaArquivosController : ApiController
    {
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage GetArquivo([FromUri]ExportaArquivos parms)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Empresa bllEmp = new BLL.Empresa(connectionStr);
            Modelo.Empresa emp = bllEmp.LoadObjectByDocumento(parms.DocumentoEmpresa);
            DateTime dataini;
            DateTime datafin;
            ValidarDados(parms, emp, out dataini, out datafin);

            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
                    objProgressBar.incrementaPB = this.IncrementaProgressBar;
                    objProgressBar.setaMensagem = this.SetaMensagem;
                    objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
                    objProgressBar.setaValorPB = this.SetaValorProgressBar;

                    byte[] arquivoMemoria;
                    string nomeArquivo = "ArquivoExportado";
                    BLL.ExportaArquivos bllExpArquivos = new BLL.ExportaArquivos(connectionStr);
                    bllExpArquivos.efetuaExportacaoWeb(out arquivoMemoria, parms.TipoArquivo, emp.Id, dataini, datafin, objProgressBar, out nomeArquivo, connectionStr, User.Identity.Name);

               

                    var result = Request.CreateResponse(HttpStatusCode.OK);

                    result.Headers.AcceptRanges.Add("none");

                    result.Content = new StreamContent(new MemoryStream(arquivoMemoria));
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentLength = arquivoMemoria.Length;
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                    {
                        FileName = nomeArquivo
                    };

                    return result;
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                }
            }
            return TrataErroModelState(retErro);
        }

        private void ValidarDados(ExportaArquivos parms, Modelo.Empresa emp, out DateTime dataini, out DateTime datafin)
        {
            if (emp == null || emp.Id <= 0)
            {
                ModelState["DocumentoEmpresa"].Errors.Add("Documento " + parms.DocumentoEmpresa + " não encontrado/cadastrado ");
            }

            if (parms.TipoArquivo != 0 && parms.TipoArquivo != 1)
            {
                ModelState["TipoArquivo"].Errors.Add("Tipo arquivo deve possuir apenas 0 ou 1. (0 = AFDT, 1 = ACJEF) ");
            }

            if (!DateTime.TryParse(parms.DataInicial, out dataini))
            {
                ModelState["DataInicial"].Errors.Add("Data " + parms.DataInicial + " inválida");
            }

            if (!DateTime.TryParse(parms.DataFinal, out datafin))
            {
                ModelState["DataFinal"].Errors.Add("Data " + parms.DataInicial + " inválida");
            }
        }
        private HttpResponseMessage TrataErroModelState(RetornoErro retErro)
        {
            List<ErroDetalhe> lErroDet = new List<ErroDetalhe>();
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
            foreach (var item in errorList)
            {
                ErroDetalhe ed = new ErroDetalhe();
                ed.campo = item.Key;
                ed.erro = String.Join(", ", item.Value);
                lErroDet.Add(ed);
            }
            if (retErro.erroGeral == "")
            {
                retErro.erroGeral = "Um ou mais erros encontrados, verifique os detalhes!";
            }
            retErro.ErrosDetalhados = lErroDet;
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }


        private void IncrementaProgressBar(int incremento)
        {
        }

        private void SetaValorProgressBar(int valor)
        {
        }

        private void SetaMinMaxProgressBar(int min, int max)
        {
        }

        private void SetaMensagem(string mensagem)
        {
        }
    }
}
