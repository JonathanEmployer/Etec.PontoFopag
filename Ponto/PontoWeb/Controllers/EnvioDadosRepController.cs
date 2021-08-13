using PontoWeb.Utils;
using Ionic.Zip;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class EnvioDadosRepController : Controller
    {
        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRep")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRep")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRep")]
        [HttpPost]
        public ActionResult Cadastrar(EnvioDadosRep obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRep")]
        [HttpPost]
        public ActionResult ExcluirFuncionarios(EnvioDadosRep obj)
        {
            obj.bOperacao = 1;
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRep")]
        [HttpPost]
        public ActionResult DownloadArquivo(EnvioDadosRep obj)
        {
            return DownloadArquivoGerado(obj);
        }

        public ActionResult Grid()
        {
            return View(new Modelo.Proxy.PxyEnvioDadosRepGrid());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.EnvioDadosRep bllEnvioDadosRep = new BLL.EnvioDadosRep(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                List<Modelo.Proxy.PxyEnvioDadosRepGrid> envioDadosRep = bllEnvioDadosRep.GetGrid();
                JsonResult jsonResult = Json(new { data = envioDadosRep }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        private ActionResult DownloadArquivoGerado(EnvioDadosRep obj)
        {
            string connection = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.REP bllREP = new BLL.REP(connection, usr);
            BLL.EnvioDadosRep bllEnvioDadosRep = new BLL.EnvioDadosRep(connection, usr);
            Modelo.REP rep = RetornaRelogio(obj.nomeRelogioSelecionado);
            ValidaFuncionariosEmpresaSelecionados(obj, connection, usr);
            ValidarForm(ref obj);

            if (ModelState.IsValid)
            {
                try
                {
                    PreencheObjFilhosParaSalvar(ref obj, connection, usr);
                    CarregarEmpresasFuncionarios(obj, connection, usr);
                    return GeraArquivo(ref obj, rep, bllEnvioDadosRep);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex);
                    return RetornaPaginaComErro(obj);
                }
            }
            else
            {
                return RetornaPaginaComErro(obj);
            }
        }

        private ActionResult RetornaPaginaComErro(EnvioDadosRep obj)
        {
            obj.pxyEmpresas = obj.pxyEmpresas == null ? new List<Modelo.Proxy.pxyEmpresa>() : obj.pxyEmpresas;
            obj.pxyFuncionarios = obj.pxyFuncionarios == null ? new List<Modelo.Proxy.pxyFuncionarioRelatorio>() : obj.pxyFuncionarios;
            return View("Cadastrar", obj);
        }

        private static void CarregarEmpresasFuncionarios(EnvioDadosRep obj, string connection, Modelo.Cw_Usuario usr)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(connection, usr);
            obj.Funcionarios = obj.Funcionarios == null ? new List<Modelo.Funcionario>() : obj.Funcionarios;
            obj.Empresas = obj.Empresas == null ? new List<Modelo.Empresa>() : obj.Empresas;
            obj.Funcionarios = bllFuncionario.GetAllListByIds("(" + obj.idsFuncionariosSelecionados + ")");
            BLL.Empresa bllEmpresa = new BLL.Empresa(connection, usr);
            List<string> lIdEmpresa = obj.idsEmpresasSelecionadas.Split(',').ToList();
            foreach (string idEmpresa in lIdEmpresa)
            {
                obj.Empresas.Add(bllEmpresa.LoadObject(Convert.ToInt32(idEmpresa)));
            }

            if (obj.Funcionarios.Any())
            {
                BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(connection, usr);
                List<FuncionarioRFID> proximidades = bllFuncionarioRFID.GetAllListByFuncionario(obj.Funcionarios.Select(s => s.Id).ToList(), true);
                proximidades.ForEach(f => obj.Funcionarios.Where(w => w.Id == f.IdFuncionario).ToList().ForEach(x => { x.RFID = f.RFID; x.MIFARE = f.MIFARE; }));
            }

            obj.Funcionarios.ForEach(x => x.Selecionado = true);
            obj.Empresas.ForEach(x => x.Selecionado = true);
        }

        private ActionResult GeraArquivo(ref EnvioDadosRep objEnv, Modelo.REP objRep, BLL.EnvioDadosRep bllEnvioDadosRep)
        {
            Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EnvioDadosRep bllEnvioEmpresaFuncionariosRep = new BLL.EnvioDadosRep(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Dictionary<string, string> erros = new Dictionary<string, string>();

            try
            {
                erros = bllEnvioEmpresaFuncionariosRep.ValidaObjeto(objEnv);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());

                    ModelState.AddModelError("CustomError", erro);

                    return RetornaPaginaComErro(objEnv);
                }
                else
                {
                    DirectoryInfo pasta;
                    JobController jc = new JobController();
                    string caminho = PegaPastaDoRep(objRep, out pasta);
                    BLL.ExportaArquivos bllExpArquivos = new BLL.ExportaArquivos(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

                    string nomeArquivo;

                    Dictionary<string, string> retorno = bllEnvioDadosRep.GerarArquivoDeEnvioParaDownload(ref caminho, pasta, objEnv, objRep, objProgressBar, out nomeArquivo, Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);

                    MemoryStream arquivoZipado = new MemoryStream();
                    string nomeArquivoZIP = "ExportacaoEmpregadorEmpregado.zip";
                    using (ZipFile zip = new ZipFile())
                    {
                        FileStream fs;
                        foreach (var key in retorno.Keys)
                        {
                            FileInfo file = new FileInfo(caminho + "\\" + key);
                            fs = file.Open(FileMode.Open);
                            string nome = retorno[key];

                            if (key.Split('.').Count() > 1)
                            {
                                string extensao = key.Split('.').LastOrDefault().Trim();
                                nome += "." + extensao;
                            }

                            zip.AddEntry(nome, fs);
                        }
                        zip.Save(arquivoZipado);
                    }
                    arquivoZipado.Position = 0;
                    FileStreamResult fileResult = new FileStreamResult(arquivoZipado, MediaTypeNames.Application.Zip);
                    fileResult.FileDownloadName = nomeArquivoZIP;
                    ApagarArquivos(caminho, retorno);
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
            }
            return RetornaPaginaComErro(objEnv);
        }

        private void ApagarArquivos(string caminho, Dictionary<string, string> retorno)
        {
            foreach (var key in retorno.Keys)
            {
                FileInfo file = new FileInfo(caminho + "\\" + key);
                file.Delete();
            }
        }

        private void SetaValorProgressBar(int valor) { }

        private void SetaMinMaxProgressBar(int min, int max) { }

        private void SetaMensagem(string mensagem) { }

        private void IncrementaProgressBar(int incremento) { }

        private string PegaPastaDoRep(REP objRep, out DirectoryInfo pasta)
        {
            string caminho = String.Format(Server.MapPath("~/App_Data/Arquivos/ExportacaoREPs/{0}"), objRep.modeloNome);

            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }

            pasta = new DirectoryInfo(caminho);

            return caminho;
        }

        protected ActionResult Salvar(EnvioDadosRep obj)
        {
            try
            {
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.EnvioDadosRep bllEnvioEmpresaFuncionariosRep = new BLL.EnvioDadosRep(conn, usr);

                ValidaFuncionariosEmpresaSelecionados(obj, conn, usr);

                Dictionary<string, string> erros = new Dictionary<string, string>();

                ValidarForm(ref obj);
                if (ModelState.IsValid)
                {
                    PreencheObjFilhosParaSalvar(ref obj, conn, usr);
                    Acao act = obj.Acao;
                    erros = bllEnvioEmpresaFuncionariosRep.Salvar(act, obj);

                    if (erros.Count == 0)
                    {
                        try
                        {
                            BLL.REP bllRep = new BLL.REP(conn, usr);
                            Modelo.REP rep = bllRep.LoadObject(obj.idRelogioSelecionado);

                            if (rep.Relogio == 17 || rep.EquipamentoHomologado.ServicoComunicador || rep.RegistradorEmMassa)
                            {
                                using (var db = new CentralCliente.CENTRALCLIENTEEntities())
                                {
                                    CentralCliente.Rep repCC = db.Rep.Where(x => x.numSerie == rep.NumSerie).FirstOrDefault();
                                    if (repCC != null)
                                    {
                                        if (obj.bEnviarEmpresa && obj.listEnvioDadosRepDet.Where(x => x.idEmpresa != null).ToList().Count > 0)
                                        {
                                            repCC.temEmpresaExportar = true;
                                        }
                                        if (obj.bEnviarFunc && obj.listEnvioDadosRepDet.Where(x => x.idFuncionario != null).ToList().Count > 0)
                                        {
                                            repCC.temFuncionarioExportar = true;
                                        }
                                        db.Entry(repCC).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }

                                if (rep.RegistradorEmMassa)
                                {
                                    using (var RabbitMqController = new RabbitMqController())
                                    {
                                        RabbitMqController.SendRegistradorEmMassa(rep.NumSerie, obj.Id);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        return RedirectToAction("Grid", "EnvioDadosRep");
                    }

                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);

                        return RetornaPaginaComErro(obj);
                    }
                    return RedirectToAction("Grid", "EnvioDadosRep");
                }
                else
                {
                    return RetornaPaginaComErro(obj);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
                return RetornaPaginaComErro(obj);
            }
        }

        private void ValidaFuncionariosEmpresaSelecionados(EnvioDadosRep obj, string conn, UsuarioPontoWeb usr)
        {
            if (obj.bEnviarFunc)
            {
                if (String.IsNullOrEmpty(obj.idsFuncionariosSelecionados))
                {
                    ModelState["idsFuncionariosSelecionados"].Errors.Add("Para enviar funcionário ao menos um deve ser selecionado.");
                }
            }

            if (obj.bEnviarEmpresa)
            {
                if (String.IsNullOrEmpty(obj.idsEmpresasSelecionadas))
                {
                    ModelState["idsEmpresasSelecionadas"].Errors.Add("Para enviar empresa ao menos uma deve ser selecionada.");
                }
            }
        }

        private void PreencheObjFilhosParaSalvar(ref EnvioDadosRep obj, string conn, Modelo.Cw_Usuario usr)
        {
            BLL.EnvioDadosRepDet bllEnvioDadosRepDet = new BLL.EnvioDadosRepDet(conn, usr);
            obj.listEnvioDadosRepDet = obj.listEnvioDadosRepDet == null ? new List<EnvioDadosRepDet>() : obj.listEnvioDadosRepDet;

            Modelo.EnvioDadosRepDet envDetLocal;
            if (obj.bEnviarEmpresa)
            {
                envDetLocal = new EnvioDadosRepDet();
                envDetLocal.idEmpresa = obj.relogioSelecionado.IdEmpresa;
                envDetLocal.idFuncionario = null;
                envDetLocal.Acao = Acao.Incluir;
                envDetLocal.Codigo = bllEnvioDadosRepDet.MaxCodigo();
                obj.listEnvioDadosRepDet.Add(envDetLocal);
            }

            if (obj.bEnviarFunc)
            {
                List<string> idsFuncionarios = obj.idsFuncionariosSelecionados.Split(',').ToList();
                int ultimoCodigo = bllEnvioDadosRepDet.MaxCodigo() + 1;
                foreach (var idFuncionario in idsFuncionarios)
                {
                    envDetLocal = new EnvioDadosRepDet();
                    envDetLocal.idEmpresa = null;
                    envDetLocal.idFuncionario = Convert.ToInt32(idFuncionario);
                    envDetLocal.Acao = Acao.Incluir;
                    envDetLocal.Codigo = ultimoCodigo++;
                    obj.listEnvioDadosRepDet.Add(envDetLocal);
                }
            }
        }

        protected ActionResult GetPagina(int id)
        {
            EnvioDadosRep env = new EnvioDadosRep();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EnvioDadosRep bllEnvioEmpresaFuncionariosRep = new BLL.EnvioDadosRep(conn, usr);

            if (id == 0)
            {
                env.Codigo = bllEnvioEmpresaFuncionariosRep.MaxCodigo();
                env.pxyEmpresas = new List<Modelo.Proxy.pxyEmpresa>();
                env.pxyFuncionarios = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
                env.bEnviarEmpresa = false;
                env.bEnviarFunc = true;
            }
            else
            {
                Modelo.REP retorno;
                BLL.REP bllRelogio = new BLL.REP(conn, usr);
                Modelo.EnvioDadosRep envioDadosRep = bllEnvioEmpresaFuncionariosRep.GetAllById(id);
                BLL.EnvioDadosRepDet bllEnvioEmpresaFuncionariosRepDet = new BLL.EnvioDadosRepDet(conn, usr);
                List<Modelo.EnvioDadosRepDet> envioDadosRepDetalhe = bllEnvioEmpresaFuncionariosRepDet.getByIdEnvioDadosRep(envioDadosRep.Id);
                Modelo.REP rep = bllRelogio.LoadObject(envioDadosRep.idRelogioSelecionado);
                env.Codigo = bllEnvioEmpresaFuncionariosRep.MaxCodigo();
                env.nomeRelogioSelecionado = rep.Codigo + " | " + rep.Local;
                env.bEnviarEmpresa = false;
                env.bEnviarFunc = true;
                env.pxyEmpresas = new List<Modelo.Proxy.pxyEmpresa>();
                env.pxyFuncionarios = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
                env.idsFuncionariosSelecionados = String.Join(",", envioDadosRepDetalhe.Where(w => w.idFuncionario != null && w.idFuncionario > 0).Select(s => s.idFuncionario));
                List<int> idEmpresas = envioDadosRepDetalhe.Where(w => w.idEmpresa != null && w.idEmpresa > 0).Select(s => s.idEmpresa.GetValueOrDefault()).ToList();
                if (idEmpresas.Count == 0)
                {
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
                    List<Modelo.Funcionario> funs = bllFuncionario.GetAllListByIds("(" + env.idsFuncionariosSelecionados + ")");
                    idEmpresas = funs.Select(s => s.Idempresa).Distinct().ToList();
                }

                if (idEmpresas.Count == 0)
                {
                    idEmpresas.Add(rep.IdEmpresa);

                }
                env.idsEmpresasSelecionadas = String.Join(",", idEmpresas);
            }
            return View("Cadastrar", env);
        }

        protected void ValidarForm(ref EnvioDadosRep obj)
        {
            obj.relogioSelecionado = RetornaRelogio(obj.nomeRelogioSelecionado);
            if (obj.relogioSelecionado == null)
            {
                ModelState["nomeRelogioSelecionado"].Errors.Add("Relógio não cadastrado no sistema.");
            }
            else
            {
                obj.idRelogioSelecionado = obj.relogioSelecionado.Id;

                PreencheObjetoParaValidar(ref obj);

                if (obj.bOperacao == 1)
                {
                    string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                    BLL.REP bllREP = new BLL.REP(conn, Usuario.GetUsuarioPontoWebLogadoCache());

                    if (!bllREP.ExclusaoHabilitada(obj.idRelogioSelecionado))
                    {
                        ModelState["nomeRelogioSelecionado"].Errors.Add("Função não disponível para este modelo de relógio.");
                    }
                }
                else
                {


                    if (obj.idRelogioSelecionado == 0)
                    {
                        ModelState["nomeRelogioSelecionado"].Errors.Add("É Obrigatório selecionar um relógio");
                    }
                    else
                    {
                        if (obj.relogioSelecionado.Relogio <= 0)
                        {
                            ModelState["nomeRelogioSelecionado"].Errors.Add("O Relógio não está configurado para realizar esta operação. Configure os parâmetros de comunicação antes de realizar esta operação.");
                        }
                        else if (obj.relogioSelecionado.IdEmpresa == 0)
                        {
                            ModelState["nomeRelogioSelecionado"].Errors.Add("A empresa não está configurada no cadastro do relógio. Configure a empresa antes de realizar esta operação.");
                        }

                        if (!obj.bEnviarEmpresa && !obj.bEnviarFunc)
                        {
                            ModelState["bEnviarEmpresa"].Errors.Add("Opções de Envio: Selecione pelo menos uma opção.");
                        }
                    }
                }
            }
        }

        private static void PreencheObjetoParaValidar(ref EnvioDadosRep obj)
        {
            if (obj.relogioSelecionado.Relogio == 1)
                obj.bEnviarEmpresa = obj.bEnviarFunc = true;
            obj.pxyEmpresas = obj.pxyEmpresas == null ? new List<Modelo.Proxy.pxyEmpresa>() : obj.pxyEmpresas;
            obj.pxyFuncionarios = obj.pxyFuncionarios == null ? new List<Modelo.Proxy.pxyFuncionarioRelatorio>() : obj.pxyFuncionarios;
            obj.Acao = Acao.Incluir;

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

        public ActionResult BuscarNumeroRelogio(String rel)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.REP bllREP = new BLL.REP(conn, Usuario.GetUsuarioPontoWebLogadoCache());
            try
            {
                Modelo.REP rep = new Modelo.REP();
                Modelo.EnvioDadosRep env = new Modelo.EnvioDadosRep();

                rep = RetornaRelogio(rel);
                bool exportacaoHabilitada = bllREP.ExportacaoHabilitada(rep.Id);


                return Json(new { Successo = true, Erro = " ", NumRelogio = rep.Relogio, ExpHab = exportacaoHabilitada }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Successo = false, Erro = ex.Message, NumRelogio = " " }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult CarregarGridEmpresa(string relogio)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EnvioDadosRep bllEnvioDadosRep = new BLL.EnvioDadosRep(conn, usr);

            Modelo.REP rep = new Modelo.REP();
            rep = RetornaRelogio(relogio);
            Modelo.EnvioDadosRep env = new Modelo.EnvioDadosRep();
            if (rep != null && rep.IdEmpresa > 0)
            {
                BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usr);
                env.pxyEmpresas = bllEmpresa.GetAllListPxyEmpresa("");
                env.pxyEmpresas.Where(w => w.Id == rep.IdEmpresa).ToList().ForEach(f => f.SelecionadoStr = "S");
                env.idEmpresaRelogio = rep.IdEmpresa;
            }
            else
            {
                env.pxyEmpresas = new List<Modelo.Proxy.pxyEmpresa>();
            }
            return View("gvGridEmpresa", env);
        }

        public ActionResult CarregarGridFuncionario(string IdsEmpresas, string idsFuncionariosSelecionados)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            Modelo.EnvioDadosRep env = new Modelo.EnvioDadosRep();
            env.UtilizaControleContrato = usr.EmpresaPrincipal.UtilizaControleContratos;
            BLL.Funcionario bllFunc = new BLL.Funcionario(conn, usr);
            if (!String.IsNullOrEmpty(IdsEmpresas))
            {
                string filtro = " and e.id in (" + IdsEmpresas + ") and (f.pis <> '' and f.pis is not null) and funcionarioativo = 1 ";
                env.pxyFuncionarios = bllFunc.GetRelFuncionariosRelatorios(filtro).ToList();
            }
            else
            {
                env.pxyFuncionarios = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
            }
            env.idsFuncionariosSelecionados = idsFuncionariosSelecionados;
            return View("gvGridFuncionario", env);
        }

        public ActionResult GridComunicador()
        {
            return View(new Modelo.Proxy.PxyGridLogComunicador());
        }

        [Authorize]
        public JsonResult DadosGridComunicador()
        {
            try
            {
                BLL.EnvioDadosRep bllEnvioDadosREP = new BLL.EnvioDadosRep(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                List<Modelo.Proxy.PxyGridLogComunicador> logComunicador = bllEnvioDadosREP.GetGridLogImportacaoWebAPI();
                JsonResult jsonResult = Json(new { data = logComunicador }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRep")]
        public ActionResult ConsultarLogComunicador(int id)
        {
            ViewBag.Consultar = 1;
            return GetPaginaLogConsultar(id);
        }

        protected ActionResult GetPaginaLogConsultar(int id)
        {
            EnvioDadosRep env = new EnvioDadosRep();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EnvioDadosRep bllEnvioEmpresaFuncionariosRep = new BLL.EnvioDadosRep(conn, usr);
            List<Modelo.Proxy.PxyGridLogComunicador> logComunicador = bllEnvioEmpresaFuncionariosRep.GetGridLogImportacaoWebAPIById(id).Where(x => x.Id == id).ToList();
            List<string> linhasDoArquivo = logComunicador.FirstOrDefault().DescricaoLog.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            logComunicador.FirstOrDefault().ArquivoImportado = linhasDoArquivo.FirstOrDefault((x => x.Contains("Arquivo Importado:"))).ToString().Replace(" ", string.Empty).Substring(17);

            logComunicador.FirstOrDefault().Lidos = getInfoLog(linhasDoArquivo, "Lidos", 6, " ");
            logComunicador.FirstOrDefault().Processados = getInfoLog(linhasDoArquivo, "Processados", 12, " ");
            logComunicador.FirstOrDefault().Errados = getInfoLog(linhasDoArquivo, "Errados", 8, " ");
            logComunicador.FirstOrDefault().Repetidos = getInfoLog(linhasDoArquivo, "Repetidos", 10, " ");
            logComunicador.FirstOrDefault().PontoFechado = getInfoLog(linhasDoArquivo, "Ponto Fechado", 13, " ");
            logComunicador.FirstOrDefault().MarcacoesProcessadas = getInfoLog(linhasDoArquivo, "Marcações Processadas =", 21, " ");


            int posicao = linhasDoArquivo.FindLastIndex(x => x.StartsWith("-----------"));
            if (posicao > 0)
            {
                posicao++;
                List<string> resultado = new List<string>();
                for (int i = posicao; i < linhasDoArquivo.Count; i++)
                {
                    resultado.Add(linhasDoArquivo[i]);
                }

                logComunicador.FirstOrDefault().ResultImport = String.Join(Environment.NewLine, resultado);
            }


            return View("ConsultarLogComunicador", logComunicador);
        }

        private static string getInfoLog(List<string> linhasDoArquivo, string campo, int tamanhoInfo, string replace)
        {
            string linha = "";
            if (linhasDoArquivo.Where(x => x.Contains(campo)).Count() > 0)
            {
                linha = linhasDoArquivo.Where(x => x.Contains(campo)).FirstOrDefault().ToString().Replace(replace, string.Empty);
                if (linha.Length > tamanhoInfo && tamanhoInfo > 0)
                {
                    linha = linha.Substring(tamanhoInfo);
                }
            }
            return linha;
        }

        [PermissoesFiltro(Roles = "EnvioEmpresaFuncionariosRepExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            Cw_Usuario usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EnvioDadosRep bllEnvioDadosRep = new BLL.EnvioDadosRep(conn, usr);
            try
            {
                Modelo.EnvioDadosRep env = bllEnvioDadosRep.LoadObject(id);
                //BLL.EnvioDadosRepDet bllEnvioDadosRepDet = new BLL.EnvioDadosRepDet(conn, usr);
                //List<Modelo.EnvioDadosRepDet> detalhes = bllEnvioDadosRepDet.getByIdEnvioDadosRep(env.Id);
                Dictionary<string, string> erros = new Dictionary<string, string>();

                try
                {
                    if (env != null && env.Id > 0)
                    {
                        bllEnvioDadosRep.ExluirEnvioDadosRepEDetalhes(env.Id);
                    }
                }
                catch (Exception e)
                {
                    erros.Add("Erro", e.Message);
                }
                //foreach (Modelo.EnvioDadosRepDet det in detalhes)
                //{
                //    foreach (var item in bllEnvioDadosRepDet.Salvar(Acao.Excluir, det))
                //    {
                //        erros.Add(item.Key, item.Value);
                //    } 
                //}
                //if (erros.Count == 0)
                //{
                //    erros = bllEnvioDadosRep.Salvar(Acao.Excluir, env);
                //}


                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}