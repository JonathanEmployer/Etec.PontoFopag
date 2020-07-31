using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class RelatorioSaldoBancoHorasController : ExtendedApiController
    {
       [HttpPost]
       [Authorize]
       [TratamentoDeErro]
       public HttpResponseMessage RelatorioSaldoBancoHoras(RelatorioSaldoBancoHoras parms)
        {
            RetornoErro retErro = new RetornoErro();
            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                if (ModelState.IsValid)
                {
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    BLL.Contrato bllContratos = new BLL.Contrato(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    BLL.ContratoFuncionario bllContratoFuncionario = new BLL.ContratoFuncionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    //Parallel.ForEach(parms.CPFsMatriculas, (CpfMatricula) =>
                    //{
                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(CpfMatricula.CPF) && !String.IsNullOrEmpty(CpfMatricula.Matricula))
                    //        {
                    //            Modelo.Funcionario func = bllFuncionario.GetFuncionarioPorCpfeMatricula(Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(CpfMatricula.CPF)), CpfMatricula.Matricula);
                    //            if (func != null && func.Id > 0)
                    //            {
                    //                funcionarios.Add(func.Id);	 
                    //            }
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        throw e;
                    //    }
                    //});
                    funcionarios = bllFuncionario.GetAllFuncsListPorCPF(parms.CPFsMatriculas.Select(s => s.CPF).ToList());
                    ConcurrentBag<int> idsFuncs = new ConcurrentBag<int>();
                    //foreach (Modelo.Proxy.PxyCPFMatricula CpfMatricula in parms.CPFsMatriculas)
                    //{
                        Parallel.ForEach(parms.CPFsMatriculas, (CpfMatricula) =>
                        {
                                if (!String.IsNullOrEmpty(CpfMatricula.CPF) && !String.IsNullOrEmpty(CpfMatricula.Matricula))
                                    {
                                        Modelo.Funcionario func = funcionarios.Where(w => Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(w.CPF)) == Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(CpfMatricula.CPF)) && w.Matricula == CpfMatricula.Matricula).OrderByDescending(x => x.Funcionarioativo).FirstOrDefault();
                                        if (func != null && func.Id > 0)
	                                    {
                                            idsFuncs.Add(func.Id);
	                                    }
                                    }
                        });
                    //}
                    List<Modelo.Proxy.Relatorios.PxyRelBancoHoras> rel = new List<Modelo.Proxy.Relatorios.PxyRelBancoHoras>();
                    rel = bllBancoHoras.RelatorioSaldoBancoHoras(parms.MesInicio, parms.AnoInicio, parms.MesFim, parms.AnoFim, idsFuncs.ToList());
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