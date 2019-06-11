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
    [Authorize]
    public class ManutencaoBiometricaController : Controller
    {
        [PermissoesFiltro(Roles = "ManutencaoBiometria")]
        public ActionResult Index()
        {
            return GetPagina(0);
        }
        [PermissoesFiltro(Roles = "ManutencaoBiometria")]
        public ActionResult Cadastrar(ManutencaoBiometrica ManuBio)
        {
            return Salvar(ManuBio);
        }
        [Authorize]
        public JsonResult FuncsGrid()
        {
            return DadosGrid();
        }

        [Authorize]
        public JsonResult GetTipoBiometria(int Codigo)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();

                BLL.REP bllRep = new BLL.REP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                var rep = bllRep.LoadObjectByCodigo(Codigo);

                BLL.EquipamentoTipoBiometria bllEquipamentoTipoBiometria = new BLL.EquipamentoTipoBiometria(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                //var EquipamentoTipoBiometria = bllEquipamentoTipoBiometria.LoadObject(rep.IdEquipamentoTipoBiometria);
                var TipoBiometria = bllEquipamentoTipoBiometria.GetAllList(rep.IdEquipamentoHomologado).Where(x => x.Id == rep.IdEquipamentoTipoBiometria.ToString()).SingleOrDefault();

                JsonResult jsonResult = Json(new { data = TipoBiometria }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {

                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                var funcionarioBLL = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                var dados = funcionarioBLL.GetAllGrid();

                var localPath = HttpContext.Request.Url.LocalPath.Split('/');
                if (localPath.Length > 3)
                {
                    var funcionarios = localPath[3].Split(',').ToList().Select(x => int.Parse(x));
                    dados = dados.Where(x => funcionarios.Contains(x.Id)).ToList();
                }
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
        [PermissoesFiltro(Roles = "ManutencaoBiometria")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }
        protected ActionResult GetPagina(int id)
        {
            var manutencaoBio = new ManutencaoBiometrica();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();

            manutencaoBio.Funcionario = new List<pxyFuncionarioGrid>();

            if (id != 0)
            {

                Modelo.EnvioDadosRep envioDadosRep = new BLL.EnvioDadosRep(conn, usr).GetAllById(id);
                manutencaoBio.Enviar = envioDadosRep.TipoComunicacao == "E" ? true : false;

                Modelo.REP rep = new BLL.REP(conn, usr).LoadObject(envioDadosRep.idRelogioSelecionado);
                manutencaoBio.idRelogioSelecionado = rep.Id;
                manutencaoBio.nomeRelogioSelecionado = rep.Codigo + " | " + rep.Local;

                List<Modelo.EnvioDadosRepDet> envioDadosRepDetalhe = new BLL.EnvioDadosRepDet(conn, usr).getByIdEnvioDadosRep(envioDadosRep.Id);
                var funcionarios = envioDadosRepDetalhe.Where(w => w.idFuncionario != null && w.idFuncionario > 0).Select(s => s.idFuncionario ?? default(int));
                manutencaoBio.idsFuncionariosSelecionados = String.Join(",", funcionarios);

                BLL.EquipamentoTipoBiometria bllEquipamentoTipoBiometria = new BLL.EquipamentoTipoBiometria(conn, usr);
                var TipoBiometria = bllEquipamentoTipoBiometria.GetAllList(rep.IdEquipamentoHomologado).Where(x => x.Id == rep.IdEquipamentoTipoBiometria.ToString()).SingleOrDefault();
                manutencaoBio.TipoBiometria = TipoBiometria.Id + " | " + TipoBiometria.Descricao;
            }

            return View("Index", manutencaoBio);
        }
        protected ActionResult Salvar(ManutencaoBiometrica obj)
        {
            try
            {
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.EnvioDadosRep bllEnvioEmpresaFuncionariosRep = new BLL.EnvioDadosRep(conn, usr);

                ValidaFuncionariosEmpresaSelecionados(obj, conn, usr);

                Dictionary<string, string> erros = new Dictionary<string, string>();

                ValidarForm(ref obj);

                PreencheObjFilhosParaSalvar(ref obj, conn, usr);
                var envioDadosRep = new EnvioDadosRep()
                {
                    Codigo = obj.Codigo == 0 ? 1 : obj.Codigo,
                    relogioSelecionado = obj.relogioSelecionado,
                    listEnvioDadosRepDet = obj.ListEnvioDadosRepDet,
                    bEnviarEmpresa = false,
                    TipoComunicacao = obj.Enviar ? "E" : "R",
                    idRelogioSelecionado = obj.relogioSelecionado.Id
                };
                bllEnvioEmpresaFuncionariosRep.Salvar(envioDadosRep);

                return RedirectToAction("Grid", "EnvioDadosRep");
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
                return View("Index", obj);
            }
        }
        private void PreencheObjFilhosParaSalvar(ref ManutencaoBiometrica obj, string conn, Modelo.Cw_Usuario usr)
        {
            BLL.EnvioDadosRepDet bllEnvioDadosRepDet = new BLL.EnvioDadosRepDet(conn, usr);
            obj.ListEnvioDadosRepDet = obj.ListEnvioDadosRepDet == null ? new List<EnvioDadosRepDet>() : obj.ListEnvioDadosRepDet;

            Modelo.EnvioDadosRepDet envDetLocal;

            List<string> idsFuncionarios = obj.idsFuncionariosSelecionados.Split(',').ToList();
            int ultimoCodigo = bllEnvioDadosRepDet.MaxCodigo();
            foreach (var idFuncionario in idsFuncionarios)
            {
                envDetLocal = new EnvioDadosRepDet();
                envDetLocal.idEmpresa = null;
                envDetLocal.idFuncionario = Convert.ToInt32(idFuncionario);
                envDetLocal.acao = Acao.Incluir;
                envDetLocal.Codigo = ultimoCodigo++;
                obj.ListEnvioDadosRepDet.Add(envDetLocal);
            }
        }
        private void ValidaFuncionariosEmpresaSelecionados(ManutencaoBiometrica obj, string conn, UsuarioPontoWeb usr)
        {
            if (String.IsNullOrEmpty(obj.idsFuncionariosSelecionados))
            {
                ModelState["idsFuncionariosSelecionados"].Errors.Add("Para enviar funcionário ao menos um deve ser selecionado.");
            }
        }
        protected void ValidarForm(ref ManutencaoBiometrica obj)
        {
            obj.relogioSelecionado = RetornaRelogio(obj.nomeRelogioSelecionado);
            if (obj.relogioSelecionado == null)
            {
                ModelState["nomeRelogioSelecionado"].Errors.Add("Relógio não cadastrado no sistema.");
            }
        }
        private static Modelo.REP RetornaRelogio(string nomeRelogio)
        {
            Modelo.REP retorno;
            try
            {
                int codigo;
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                BLL.REP bllRelogio = new BLL.REP(conn, Usuario.GetUsuarioPontoWebLogadoCache());
                string codigoStr = nomeRelogio.Split('|') == null ? "0" : nomeRelogio.Split('|').FirstOrDefault().Trim();
                Int32.TryParse(codigoStr, out codigo);

                retorno = bllRelogio.LoadObjectByCodigo(codigo);
            }
            catch (Exception)
            {
                retorno = new Modelo.REP();
            }

            return retorno;
        }
    }
}