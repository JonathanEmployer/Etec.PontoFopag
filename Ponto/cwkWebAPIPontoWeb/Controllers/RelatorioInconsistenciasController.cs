using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class RelatorioInconsistenciasController : ExtendedApiController
    {

        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        public HttpResponseMessage RelatorioInconsistencias(RelatorioInconsistenciasParam parms)
        {
            RetornoErro retErro = new RetornoErro();

            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                if (ModelState.IsValid)
                {
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    BLL.Inconsistencia bllInconsistencias = new BLL.Inconsistencia(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    BLL.ContratoFuncionario bllContratoFuncionario = new BLL.ContratoFuncionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    funcionarios = bllFuncionario.GetAllFuncsListPorCPF(parms.CPFsMatriculas.Select(s => s.CPF).ToList());
                    ConcurrentBag<int> idsFuncs = new ConcurrentBag<int>();
                    List<bool> listaParamInconsistencia = new List<bool>();

                    if (funcionarios.Count() <= 0)
                    {
                        retErro.erroGeral = "Não existem funcionários para os números de CPF informados";
                        return TrataErroModelState(retErro);
                    }

                    Parallel.ForEach(parms.CPFsMatriculas, (CpfMatricula) =>
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

                    #region popula lista de parâmetros ocorrência
                    listaParamInconsistencia.Add(parms.Intrajornada);
                    listaParamInconsistencia.Add(parms.Interjornada);
                    listaParamInconsistencia.Add(parms.SetimoDiaTrabalhado);
                    listaParamInconsistencia.Add(parms.LimiteHorasTrabalhadas);
                    listaParamInconsistencia.Add(parms.TerceiroDomTrabalhado);
                    listaParamInconsistencia.Add(parms.SeisHorasSemIntervalo);
                    #endregion

                    List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias> rel = new List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias>();
                    rel = bllInconsistencias.RelatorioInconsistencias(idsFuncs.ToList(), parms.InicioPeriodo, parms.FimPeriodo, listaParamInconsistencia);
                    if (rel.Count() <= 0)
                    {
                        retErro.erroGeral = "Não há inconsistências para os dados informados.";
                        return TrataErroModelState(retErro);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, rel);
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