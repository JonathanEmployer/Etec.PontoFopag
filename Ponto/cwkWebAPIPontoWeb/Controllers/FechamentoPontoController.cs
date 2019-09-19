using cwkWebAPIPontoWeb.Utils;
using Modelo.Proxy;
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
    /// Método para buscar o último fechamento de Ponto e de Banco de Horas do Funcionário
    /// </summary>
    public class FechamentoPontoController : ApiController
    {
        /// <summary>
        ///     Método para buscar o último fechamento de Ponto e de Banco de Horas do Funcionário
        /// </summary>
        /// <param name="parametros">Combinação de CPF e Matricula dos Funcionários a consultar</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        public HttpResponseMessage Get(List<Modelo.Proxy.PxyCPFMatricula> parametros)
        {
            if (ModelState.IsValid)
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPW.ConnectionString);

                List<Modelo.Funcionario> funcionarios = bllFuncionario.GetAllFuncsListPorCPF(parametros.Select(s => s.CPF).ToList());
                if (funcionarios.Count() <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Nenhum funcionário encontrado com os parâmetros informados");
                }
                ConcurrentBag<int> idsFuncs = new ConcurrentBag<int>();
                Parallel.ForEach(parametros, (CpfMatricula) =>
                {
                    if (!String.IsNullOrEmpty(CpfMatricula.CPF) && !String.IsNullOrEmpty(CpfMatricula.Matricula))
                    {
                        Modelo.Funcionario func = funcionarios.Where(w => Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(w.CPF)) == Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(CpfMatricula.CPF)) && w.Matricula == CpfMatricula.Matricula).FirstOrDefault();
                        if (func != null && func.Id > 0)
                        {
                            idsFuncs.Add(func.Id);
                        }
                    }
                });
                if (idsFuncs.Count() == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Nenhum funcionário encontrado com a combinação de CPF e Matrícula informado");
                }

                List<PxyUltimoFechamentoPonto> ret = bllFuncionario.GetUltimoFechamentoPontoFuncionarios(idsFuncs.ToList());
                return Request.CreateResponse(HttpStatusCode.OK, ret);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList());
        }
    }
}
