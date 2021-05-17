using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class FuncionarioExcluidoController : Controller
    {
        Modelo.UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "FuncionarioExcluido")]
        public ActionResult Grid()
        {
            return View(new PxyFuncionarioExcluidoGrid());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                List<PxyFuncionarioExcluidoGrid> dados = bllFuncionario.GetExcluidosList();
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "FuncionarioExcluidoAlterar")]
        public ActionResult Restaurar(int id)
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            int limitefunc = 0;
            if (bllEmpresa.ValidaLicenca(out limitefunc, false))
            {
                try
                {
                    string mensagem = "";
                    Modelo.Funcionario objfunc = bllFuncionario.LoadObject(id);

                    try
                    {
                        objfunc.Excluido = 0;
                        objfunc.Funcionarioativo = 1;
                        Dictionary<string, string> erros = bllFuncionario.Salvar1(Modelo.Acao.Alterar, objfunc, 1);
                        if (erros.Count > 0)
                        {
                            string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                            erro = erro.Replace("Matricula=", string.Empty).Replace("Horario=", string.Empty);

                            return new JsonResult { Data = new { success = false, Erro = erro } };
                        }
                        else
                        {
                            UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();

                            HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW);
                            Modelo.Proxy.PxyJobReturn ret = hfm.RestauraFuncionarioJob(objfunc);
                            return new JsonResult { Data = new { success = true, job = ret } };
                        }
                    }
                    catch (Exception e)
                    {
                        return new JsonResult { Data = new { success = false, Erro = mensagem + Environment.NewLine + e.Message } };
                    }

                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    return new JsonResult { Data = new { success = false, Erro = ex.Message } };
                }
            }
            else
            {
                string s = "A quantidade de funcionários chegou no limite de " + limitefunc + " funcionários ativos. Entre em contato com a revenda.";
                return new JsonResult { Data = new { success = false, Erro = s } };
            }
        }
    }
}
