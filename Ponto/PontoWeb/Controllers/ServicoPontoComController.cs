using CentralCliente;
using bllWeb = PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PontoWeb.Security;
using Modelo;
using Modelo.Proxy.CentralCliente;
using System.Data.Entity.Migrations;
using System.Configuration;

namespace PontoWeb.Controllers
{
    public class ServicoPontoComController : Controller
    {
        Modelo.UsuarioPontoWeb _usr = bllWeb.Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "ServicoPontoCom")]
        public ActionResult Grid()
        {
            return View(new PxyGridComunicadorServico());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                List<PxyGridComunicadorServico> lista = new List<PxyGridComunicadorServico>();
                using (var db = new CENTRALCLIENTEEntities())
                {
                    CentroServico cs = db.CentroServico.Where(x => x.DataBaseName == _usr.DataBase).FirstOrDefault();

                    if (cs == null)
                    {
                        cs = new CentroServico();
                        System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(_usr.ConnectionString);
                        cs.DataBaseName = builder.InitialCatalog;
                        cs.Inchora = DateTime.Now;
                        cs.Incusuario = _usr.Login;
                        cs.Codigo = 0;
                        cs.Descricao = _usr.EmpresaPrincipal.Nome;
                        cs.Instancia = builder.DataSource;
                        db.CentroServico.Add(cs);
                        db.SaveChanges();
                    }
                    List<ComunicadorServico> comunicadorServicos = db.ComunicadorServico.Where(w => w.CentroServico.Id == cs.Id).ToList();
                    comunicadorServicos.ForEach(f =>
                    {
                        lista.Add(new PxyGridComunicadorServico(f.Id,
                                                                      f.Codigo,
                                                                      f.Descricao,
                                                                      f.Observacao,
                                                                      f.Inchora.GetValueOrDefault().ToString("dd/MM/yyyy HH:ss"),
                                                                      f.Incusuario,
                                                                      f.Althora == null ? "" : f.Althora.GetValueOrDefault().ToString("dd/MM/yyyy HH:ss"),
                                                                      f.Altusuario,
                                                                      f.ComunicadorServidor.Nome,
                                                                      f.ComunicadorServidor.MAC));
                    });
                }
                JsonResult jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "ServicoPontoComConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ServicoPontoComCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ServicoPontoComAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ServicoPontoComCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(PxyComunicadorServico obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ServicoPontoComAlterar")]
        [HttpPost]
        public ActionResult Alterar(PxyComunicadorServico obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ServicoPontoComExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            try
            {
                using (var db = new CENTRALCLIENTEEntities())
                {
                    ComunicadorServico cs = db.ComunicadorServico.Where(x => x.Id == id).FirstOrDefault();
                    if (cs != null && !cs.Rep.Any())
                    {
                        db.ComunicadorServico.Remove(cs);
                        return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
                    }
                    else if (cs != null && cs.Rep.Any())
                    {
                        return Json(new { Success = false, Erro = "Não foi possível remover o serviço, existem Relógios vinculados." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Success = false, Erro = "Serviço não encontrado" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected ActionResult Salvar(PxyComunicadorServico obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = obj.Id == 0 ? Acao.Incluir : Acao.Alterar;
                    ComunicadorServico comServico;
                    using (var db = new CENTRALCLIENTEEntities())
                    {
                        comServico = db.ComunicadorServico.Find(obj.Id);
                        comServico = comServico ?? new ComunicadorServico();
                        var cs = db.CentroServico.Where(w => w.DataBaseName == _usr.DataBase).FirstOrDefault();
                        if (cs == null || cs.Id == 0)
                        {
                            db.CentroServico.Add(new CentroServico() { Inchora = DateTime.Now, Codigo = 0, Althora = null, Altusuario = null,  });
                        }

                        ComunicadorServidor servidorSelecionado = ValidaServidor(obj.MAC, obj.Id, db, out string erro);
                        if (!string.IsNullOrEmpty(erro))
                        {
                            ModelState.AddModelError("MAC", erro);
                        }

                        if (ModelState.IsValid)
                        {
                            comServico.Codigo = obj.Codigo;
                            comServico.Descricao = obj.Descricao;
                            comServico.Observacao = obj.Observacao;
                            comServico.IdCentroServico = cs.Id;
                            comServico.CentroServico = cs;
                            comServico.IdComunicadorServidor = servidorSelecionado.Id;
                            comServico.ComunicadorServidor = servidorSelecionado;
                            comServico.Inchora = comServico.Inchora == null ? DateTime.Now : comServico.Inchora;
                            comServico.Incusuario = comServico.Incusuario == null ? _usr.Login : comServico.Incusuario;
                            comServico.Althora = DateTime.Now;
                            comServico.Altusuario = _usr.Login;
                            db.ComunicadorServico.AddOrUpdate(comServico);

                            db.SaveChanges();
                            new BLL.RabbitMQ.RabbitMQ().EnviarMensagemServicoPontoCom(comServico.ComunicadorServidor.MAC, Enumeradores.PontoComFuncoes.Atualizar);

                            return RedirectToAction("Grid", "ServicoPontoCom");
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", obj);
        }

        private ComunicadorServidor ValidaServidor(string mac, int idServico, CENTRALCLIENTEEntities db, out string erro)
        {
            erro = "";
            ComunicadorServidor servidorSelecionado = db.ComunicadorServidor.Where(w => w.MAC == mac).FirstOrDefault();
            if (servidorSelecionado == null || servidorSelecionado.Id == 0)
            {
                erro = $"Servidor com o MAC {mac} não foi encontrado";
            }
            else
            {
                if (servidorSelecionado.ComunicadorServico != null && servidorSelecionado.ComunicadorServico.Any() && servidorSelecionado.ComunicadorServico.FirstOrDefault().Id > 0)
                {
                    if (servidorSelecionado.ComunicadorServico.FirstOrDefault().CentroServico != null && servidorSelecionado.ComunicadorServico.FirstOrDefault().CentroServico.DataBaseName != _usr.DataBase)
                    {
                        erro = $"Servidor com o MAC {mac} já foi reivindicado, verifique o número digitado.";
                    }
                    else if (servidorSelecionado.ComunicadorServico.FirstOrDefault().Id != idServico)
                    {
                        erro = $"Servidor com o MAC {mac} esta sendo utilizado por outro serviço";
                    }
                }
            }

            return servidorSelecionado;
        }

        protected ActionResult GetPagina(int id)
        {
            ComunicadorServico cs = new ComunicadorServico();
            PxyComunicadorServico pxyComunicadorServico;
            using (var db = new CENTRALCLIENTEEntities())
            {
                if (id > 0)
                {
                    cs = db.ComunicadorServico.Find(id);
                    pxyComunicadorServico = new PxyComunicadorServico(cs.Id, cs.Codigo, cs.Descricao, cs.Observacao, cs.ComunicadorServidor.MAC, cs.ComunicadorServidor.Nome);
                }
                else
                {
                    pxyComunicadorServico = new PxyComunicadorServico();
                    int? codigo = db.ComunicadorServico.Max(m => m.Codigo);
                    pxyComunicadorServico.Codigo = codigo.GetValueOrDefault() + 1;
                }
            }
            return View("Cadastrar", pxyComunicadorServico);
        }

        [Authorize]
        public ActionResult BuscaServer(string mac, int idServico)
        {
            try
            {
                using (var db = new CENTRALCLIENTEEntities())
                {
                    ComunicadorServidor servidorSelecionado = ValidaServidor(mac, idServico, db, out string erro);
                    if (!string.IsNullOrEmpty(erro))
                    {
                        return Json(new { Successo = false, Erro = erro, ServerName = "" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Successo = true, Erro = " ", ServerName = servidorSelecionado.Nome }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Successo = false, Erro = ex.Message, ServerName = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            IList<ComunicadorServico> lComunicadorServico = new List<ComunicadorServico>();
            UsuarioPontoWeb usr = bllWeb.Usuario.GetUsuarioPontoWebLogadoCache();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            using (var db = new CENTRALCLIENTEEntities())
            {
                if (codigo != -1)
                {
                    ComunicadorServico comunicadorServico = db.ComunicadorServico.Where(w => w.CentroServico.DataBaseName == usr.DataBase && w.Codigo == codigo).FirstOrDefault();
                    if (comunicadorServico != null && comunicadorServico.Id > 0)
                    {
                        lComunicadorServico.Add(comunicadorServico);
                    }
                }

                if (lComunicadorServico.Count == 0)
                {
                    if (!String.IsNullOrEmpty(consulta))
                    {
                        lComunicadorServico = db.ComunicadorServico.Where(w => w.CentroServico.DataBaseName == usr.DataBase && w.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                    }
                    else
                    {
                        lComunicadorServico = db.ComunicadorServico.Where(w => w.CentroServico.DataBaseName == usr.DataBase).ToList();
                    }
                }
            }
            ViewBag.Title = "Pesquisar Serviço PontoCom";
            return View(lComunicadorServico);
        }
        #endregion
    }
}