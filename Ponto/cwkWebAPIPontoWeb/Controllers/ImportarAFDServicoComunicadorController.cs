using BLL.IntegracaoRelogio;
using cwkPontoMT.Integracao;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Método responsável por receber os AFDs do serviço comunicador e importar
    /// </summary>
    public class ImportarAFDServicoComunicadorController : ExtendedApiController
    {
        /// <summary>
        ///  Método que recebe e importa o AFD
        /// </summary>
        /// <param name="RegsAFD">Lista de registros do AFD</param>
        /// <returns></returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Importar(List<RegistroAFD> RegsAFD)
        {
            RetornoErro retErro = new RetornoErro(); 
            if (ModelState.IsValid)
            {
                try
                {
                    BLL.REP bllRep = new BLL.REP(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    string numSerie = RegsAFD.Where(w => w.Campo02 == "1").Select(s => s.Campo07).Distinct().FirstOrDefault().ToString();
                    Modelo.REP repCliente = bllRep.LoadObjectByNumSerie(numSerie);
                    ProcessarRegistroAFD processarRegistros = new ProcessarRegistroAFD(repCliente, usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

                    ResultadoImportacao res = processarRegistros.ProcessarImportacao(new List<int>(), RegsAFD);
                    return Request.CreateResponse(HttpStatusCode.OK, res);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                }
            }
            return TrataErroModelState(retErro);
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
    }
}
