using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
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
    /// Controller Responsável pelos dados do Relatório Homem Hora
    /// </summary>
    public class RelatorioHomemHoraController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar os dados do Relatório Homem Hora
        /// </summary>
        /// <param name="parametros">Parâmetros do Relatório</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        public HttpResponseMessage RelatorioHomemHora(ParametrosRelatorioBase parametros)
        {
            RetornoErro retErro = new RetornoErro();
            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                if (ModelState.IsValid)
                {
                    string connectionStr = MetodosAuxiliares.Conexao();
                    BLL.Relatorios.RelatorioHomemHora bllRelatorioHomemHora = new BLL.Relatorios.RelatorioHomemHora(userPW.ConnectionString);
                    ConcurrentBag<int> idsFuncs = new ConcurrentBag<int>();
                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);

                    funcionarios = bllFuncionario.GetAllFuncsListPorCPF(parametros.CPFsMatriculas.Select(s => s.CPF).ToList());

                    if (funcionarios.Count() <= 0)
                    {
                        retErro.erroGeral = "Não existem funcionários para os números de CPF informados";
                        return TrataErroModelState(retErro);
                    }

                    Parallel.ForEach(parametros.CPFsMatriculas, (CpfMatricula) =>
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

                    if (idsFuncs.Count() <= 0)
                    {
                        retErro.erroGeral = "Ocorreu um erro ao gerar o relatório. Verifique se o CPF e a Matrícula pertencem ao mesmo funcionário.";
                        return TrataErroModelState(retErro);
                    }

                    List<Modelo.Proxy.Relatorios.RelHomemHora> listRel = bllRelatorioHomemHora.GetRelatorioHomemHora(idsFuncs.ToList(), DateTime.Parse(parametros.InicioPeriodo), DateTime.Parse(parametros.FimPeriodo), "").ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, listRel);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                retErro.erroGeral = ex.Message;
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
