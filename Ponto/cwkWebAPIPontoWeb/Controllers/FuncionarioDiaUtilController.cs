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
    /// <summary>
    /// Método responsável por retornar se o dia deve ser trabalhado pelo funcionário
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FuncionarioDiaUtilController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar se o dia deve ser trabalhado pelo funcionário
        /// </summary>
        /// <returns>Objeto contendo se o funcionário possui afastamento, folga, feriado ou jornada no dia.;
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage GetFuncionarioPorCPFeMatricula(string CPF, string Matricula, DateTime Data)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Funcionario bllFunc = new BLL.Funcionario(connectionStr);
            try
            {
                CPF = CPF.Replace("-", "").Replace(".", "");
                Int64 CPFint = Convert.ToInt64(CPF);
                Modelo.Funcionario funcionario = new Modelo.Funcionario();
                funcionario = bllFunc.GetFuncionarioPorCpfeMatricula(CPFint, Matricula);
                List<int> funcs = new List<int>();
                List<Modelo.Proxy.PxyFuncionarioDiaUtil> funcionarioDiaUtil = new List<Modelo.Proxy.PxyFuncionarioDiaUtil>();
                if (funcionario != null && funcionario.Id > 0 && funcionario.Excluido != 0)
                {
                    funcs.Add(funcionario.Id);
                }
                else
                {
                    // Caso não encontre o funcionário mesmo assim retorno se o dia "possui feriado"
                    funcs.Add(-1);
                }
                funcionarioDiaUtil = bllFunc.GetDiaUtilFuncionario(funcs, Data, Data);
                return Request.CreateResponse(HttpStatusCode.OK, funcionarioDiaUtil.FirstOrDefault());  
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                retErro.erroGeral += e.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
            }
        }
    }
}
