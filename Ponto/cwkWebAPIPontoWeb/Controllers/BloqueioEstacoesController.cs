using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// API para carga de bloqueios de estações por regras temporais.
    /// </summary>
    [Authorize]
    public class BloqueioEstacoesController : ExtendedApiController
    {
        /// <summary>
        /// Retorna o estado do bloqueio das estações dos funcionários informados.
        /// </summary>
        /// <param name="cpfs">Lista de CPFs dos funcionários.</param>
        /// <returns></returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage EstadoFuncionarios(string[] cpfs)
        {
            ConcurrentBag<EstadoBloqueioFuncionario> retorno = new ConcurrentBag<EstadoBloqueioFuncionario>();

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

            try
            {
                List<Modelo.Funcionario> objListFunc = bllFuncionario.LoadAtivoPorListCPF(cpfs.ToList());
                Parallel.ForEach(objListFunc, objFunc =>
                        {
                            //string cpf = cpfLista.Replace("-", "").Replace(".", "");
                            EstadoBloqueioFuncionario estado = new EstadoBloqueioFuncionario();
                            try
                            {
                                DateTime data = DateTime.Now.Date;
                                Modelo.BloqueioFuncionario bloqueio = bllFuncionario.ValidarBloqueioFuncionario(objFunc, data);
                                
                                estado.CPF = objFunc.CPF;
                                estado.Bloqueado = bloqueio.Bloqueado;
                                estado.RegraBloqueio = bloqueio.RegraBloqueio;
                                estado.Mensagem = bloqueio.Motivo;
                                estado.PrevisaoLiberacao = bloqueio.PrevisaoLiberacao.HasValue ? bloqueio.PrevisaoLiberacao.Value.ToString("yyyy-MM-dd HH:mm") : null;
                                retorno.Add(estado);
                            }
                            catch (Exception ex)
                            {
                                estado.CPF = objFunc.CPF;
                                estado.Bloqueado = false;
                                estado.RegraBloqueio = null;
                                estado.Mensagem = "Erro ao validar a regra do funcionário, detalhe: " + ex.Message;
                                estado.PrevisaoLiberacao = null;
                                retorno.Add(estado);
                            }
                        });

                return Request.CreateResponse(HttpStatusCode.OK, retorno.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
