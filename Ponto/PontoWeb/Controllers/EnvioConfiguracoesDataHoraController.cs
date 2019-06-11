using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using PontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class EnvioConfiguracoesDataHoraController : Controller
    {
        [PermissoesFiltro(Roles = "EnvioConfiguracoesDataHoraCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "EnvioConfiguracoesDataHoraCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(EnvioConfiguracoesDataHora obj)
        {
            return Salvar(obj);
        }

        protected ActionResult GetPagina(int id)
        {
            EnvioConfiguracoesDataHora env = new EnvioConfiguracoesDataHora();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EnvioConfiguracoesDataHora bllEnvioConfiguracoesDataHora = new BLL.EnvioConfiguracoesDataHora(conn, usr);

            env = bllEnvioConfiguracoesDataHora.LoadObjectByID(id);
            if (env == null)
                env = new EnvioConfiguracoesDataHora();

            return View("Cadastrar", env);
        }

        private ActionResult Salvar(EnvioConfiguracoesDataHora obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.EnvioConfiguracoesDataHora bllEnvioConfiguracoesDataHora = new BLL.EnvioConfiguracoesDataHora(conn, usr);
            String msg = String.Empty;
            Dictionary<string, string> erros = new Dictionary<string, string>();
            ValidarForm(ref obj);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao act = Acao.Incluir;
                    obj.Codigo = bllEnvioConfiguracoesDataHora.MaxCodigo();
                    erros = bllEnvioConfiguracoesDataHora.Salvar(act, obj);

                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                        msg = erro;

                        return View("Cadastrar", obj);
                    }
                    else
                    {
                        string erro = String.Empty;
                        try
                        {
                            BLL.REP bllRep = new BLL.REP(conn, usr);
                            Modelo.REP rep = bllRep.LoadObject(obj.idRelogio);
                            BLL.EquipamentoHomologado bllEquiHom = new BLL.EquipamentoHomologado(conn, usr);
                            Modelo.EquipamentoHomologado equiHom = bllEquiHom.LoadObject(rep.IdEquipamentoHomologado);

                            if (rep.Relogio == 17 || rep.EquipamentoHomologado.ServicoComunicador)
                            {
                                using (var db = new CentralCliente.CENTRALCLIENTEEntities())
                                {
                                    CentralCliente.Rep repCC = db.Rep.Where(x => x.numSerie == rep.NumSerie).FirstOrDefault();
                                    if (repCC != null && repCC.Id > 0)
                                    {
                                        if (obj.bEnviaHorarioVerao)
                                        {
                                            repCC.temHorarioVeraoExportar = true;
                                        }
                                        if (obj.bEnviaDataHoraServidor)
                                        {
                                            repCC.temDataHoraExportar = true;
                                        }
                                        db.Entry(repCC).State = EntityState.Modified;
                                        db.SaveChanges(); 
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                    msg = ex.Message;
                    return View("Cadastrar", obj);
                }
            }
            else
            {
                return View("Cadastrar", obj);
            }
        }

        protected void ValidarForm(ref EnvioConfiguracoesDataHora obj)
        {
            StringBuilder str = new StringBuilder();
            string connection = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.REP bllREP = new BLL.REP(connection, usr);

            obj.relogio = RetornaRelogio(obj.nomeRelogio);
            if (obj.relogio.Id == 0)
            {
                ModelState["nomeRelogio"].Errors.Add("Selecione o relógio que receberá os dados.");
            }
            else
            {
                obj.idRelogio = obj.relogio.Id;
                if (obj.relogio.Relogio <= 0)
                {
                    ModelState["nomeRelogio"].Errors.Add("Configure os parâmetros de comunicação antes de realizar esta operação.");
                }
            }

            if (!obj.bEnviaDataHoraServidor && !obj.bEnviaHorarioVerao)
            {
                ModelState["tipoHorario"].Errors.Add("Selecione pelo menos uma opção para salvar.");
            }


            if (obj.bEnviaHorarioVerao)
            {
                if (obj.dtInicioHorarioVerao == null)
                {
                    ModelState["dtInicioHorarioVerao"].Errors.Add("Selecione a data e a hora de início do horário de verão.");
                }

                if (obj.dtFimHorarioVerao == null)
                {
                    ModelState["dtFimHorarioVerao"].Errors.Add("Selecione a data e a hora de término do horário de verão.");
                }
            }

        }

        private static Modelo.REP RetornaRelogio(string nomeRelogio)
        {
            Modelo.REP retorno;
            try
            {
                int codigo;
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.REP bllRelogio = new BLL.REP(conn, usr);
                string codigoStr = nomeRelogio.Split('|') == null ? "0" : nomeRelogio.Split('|').FirstOrDefault().Trim();
                Int32.TryParse(codigoStr, out codigo);

                retorno = bllRelogio.LoadObjectByCodigo(codigo);
                retorno = retorno == null ? new Modelo.REP() : retorno;
            }
            catch (Exception)
            {
                retorno = new Modelo.REP();
            }

            return retorno;
        }
	}
}