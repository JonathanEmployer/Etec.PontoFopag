using CentralCliente;
using bllWeb = PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelo.Proxy;

namespace PontoWeb.Controllers
{
    public class ServicoPontoComController : Controller
    {
        // GET: ServicoPontoCom
        public ActionResult Grid()
        {
            return View(new PxyGridServicoPontoCom());
        }

        public JsonResult DadosGrid()
        {
            try
            {
                List<PxyGridServicoPontoCom> centroServicos = new List<PxyGridServicoPontoCom>();
                Modelo.UsuarioPontoWeb usr = bllWeb.Usuario.GetUsuarioPontoWebLogadoCache();
                using (var db = new CentralCliente.CENTRALCLIENTEEntities())
                {
                    CentroServico cs = db.CentroServico.Where(x => x.DataBaseName == usr.DataBase).FirstOrDefault();

                    if (cs == null)
                    {
                        cs = new CentroServico();
                        System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(usr.ConnectionString);
                        cs.DataBaseName = builder.InitialCatalog;
                        cs.Inchora = DateTime.Now;
                        cs.Incusuario = usr.Login;
                        cs.Codigo = 0;
                        cs.Descricao = usr.EmpresaPrincipal.Nome;
                        cs.Instancia = builder.DataSource;
                        db.CentroServico.Add(cs);
                        db.SaveChanges();
                    }
                    List<ComunicadorServico> comunicadorServicos = db.ComunicadorServico.Where(w => w.CentroServico.Id == cs.Id).ToList();
                    comunicadorServicos.ForEach(f =>
                    {
                        centroServicos.Add(new PxyGridServicoPontoCom(f.Id,
                                                                      f.Codigo,
                                                                      f.Descricao,
                                                                      f.Observacao,
                                                                      f.Inchora,
                                                                      f.Incusuario,
                                                                      f.Althora,
                                                                      f.Altusuario,
                                                                      f.ComunicadorServidor.Nome,
                                                                      f.ComunicadorServidor.MAC));
                    });
                }
                JsonResult jsonResult = Json(new { data = centroServicos }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }
    }
}