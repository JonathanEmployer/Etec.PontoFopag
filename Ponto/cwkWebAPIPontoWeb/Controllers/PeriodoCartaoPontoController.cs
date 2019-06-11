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
    /// <summary>
    /// Copntroler para retornar o período configurado pela empresa para o cartão ponto
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PeriodoCartaoPontoController : ExtendedApiController
    {
        /// <summary>
        /// Método responsável por retornar o período de acordo com o configurado.
        /// </summary>
        /// <param name="bil">Recebe os dados do bilhete (utiliza apenas o CPF e a Senha)</param>
        /// <returns>Data Inicial e Final de acordo com a configuração do Pontofopag.</returns>
        [ResponseType(typeof(FiltroCartaoPonto))]
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage PeriodoCartaoPonto(LoginFuncionario login)
        {
            ValidaDadosFuncionarioSetaConexao(login);
            if (ModelState.IsValid)
            {
                try
                {
                    BLL.ConfiguracoesGerais bllConfiguracoes = new BLL.ConfiguracoesGerais(StrConexao);
                    DateTime dataIni = DateTime.Now.Date;
                    DateTime dataFin = DateTime.Now.Date.AddDays(30);
                    bool mudadataautomaticamente;
                    FiltroCartaoPonto filtroCartaoPonto = new FiltroCartaoPonto();
                    bllConfiguracoes.AtribuiDatas(String.Empty, out dataIni, out dataFin, out mudadataautomaticamente);

                    if ((dataIni == new DateTime() || dataIni == null) ||
                        (dataFin == new DateTime() || dataFin == null))
                    {
                        dataIni = DateTime.Now.Date;
                        dataFin = DateTime.Now.Date.AddDays(30);
                    }

                    filtroCartaoPonto.DataInicial = dataIni;
                    filtroCartaoPonto.DataFinal = dataFin;
                    return Request.CreateResponse(HttpStatusCode.OK, filtroCartaoPonto);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    ModelState.AddModelError("CustomError", e.Message);
                }
            }
            return TrataErroModelState();
        }
    }
}
