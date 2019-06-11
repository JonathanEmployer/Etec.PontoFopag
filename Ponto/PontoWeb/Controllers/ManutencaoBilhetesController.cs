using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ManutencaoBilhetesController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [Authorize]
        [PermissoesFiltro(Roles = "ManutencaoBilhetes")]
        public ActionResult Grid(int id)
        {  
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Marcacao mar = bllMarcacao.LoadObject(id);
            Funcionario func = bllFuncionario.LoadObject(mar.Idfuncionario);
            List<Funcionario> funcs = bllFuncionario.GetAllListPorPis(new List<string>() { func.Pis});
            funcs = funcs.Where(w => w.Excluido == 0).ToList();
            ViewBag.possuiMultiploRegistro = funcs.Count() > 1 ? 1 : 0;
            ViewBag.FuncionarioAlocado = func;
            Funcionario funcAlocar = funcs.Where(w => w.Id != func.Id).OrderByDescending(o => o.bFuncionarioativo).ThenByDescending(t => t.Dataadmissao).FirstOrDefault();
            if (funcAlocar != null && funcAlocar.Id > 0)
            {
                ViewBag.FuncionarioAlocar = bllFuncionario.LoadObject(funcAlocar.Id);    
            }
            else
            {
                ViewBag.FuncionarioAlocar = new Funcionario();
            }
            IList<BilhetesImp> bilhetes = bllBilhetesImp.LoadManutencaoBilhetes(mar.Dscodigo, mar.Data, false);
            BLL.LocalizacaoRegistroPonto bllLoc = new BLL.LocalizacaoRegistroPonto(_usr.ConnectionString, _usr);
            List<Localizacao> localizacoes = new List<Localizacao>();
            foreach (BilhetesImp bilhete in bilhetes)
            {
                LocalizacaoRegistroPonto loc = bllLoc.GetPorBilhete(bilhete.Id);
                if (loc != null && loc.Id > 0)
                {
                    Localizacao l = new Localizacao();
                    l.Latitude = loc.Latitude.ToString();
                    l.Longitude = loc.Longitude.ToString();
                    l.Horario = bilhete.Mar_data.ToString() +' '+ bilhete.Mar_hora;
                    localizacoes.Add(l);
                }
            }
            ViewBag.Localizacoes =  Newtonsoft.Json.JsonConvert.SerializeObject(localizacoes);
            return View(bilhetes);
        }

        //[PermissoesFiltro(Roles = "TransferirBilhetesDeContratoCadastrar")]
        [HttpPost]
        public ActionResult TransferirDeContrato(int idMarcacao, string selecionados)
        {
            try
            {
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
                BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(_usr.ConnectionString, _usr);
                Marcacao marcFuncAlocado = new Marcacao();
                Marcacao marcFuncTransferir = new Marcacao();
                Funcionario funcAlocado = new Funcionario();
                Funcionario funcTransferir = new Funcionario();
                List<Modelo.BilhetesImp> bilhetesAlocar = new List<BilhetesImp>();
                //Carrega os dados e valida para alteração dos bilhetes
                CarregaEValidaDadosTransferenciaBilhetes(idMarcacao, selecionados, bllMarcacao, bllFuncionario, bllBilhetesImp, ref marcFuncAlocado, ref marcFuncTransferir, ref funcAlocado, ref funcTransferir, bilhetesAlocar);

                try
                {
                    //Altera os bilhetes para o proximo registro
                    bilhetesAlocar.ForEach(f => { f.Func = funcTransferir.Dscodigo; f.DsCodigo = funcTransferir.Dscodigo; f.IdFuncionario = funcTransferir.Id; f.PIS = funcTransferir.Pis; });

                    bllBilhetesImp.Salvar(Modelo.Acao.Alterar, bilhetesAlocar);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao transferir os bilhetes de contrato, erro: "+e.Message);
                }
                Modelo.ProgressBar pb = new Modelo.ProgressBar();
                JobController jobController = new JobController();
                pb.incrementaPB = jobController.IncrementaProgressBarVazio;
                pb.setaMensagem = jobController.SetaMensagemVazio;
                pb.setaMinMaxPB = jobController.SetaMinMaxProgressBarVazio;
                pb.setaValorPB = jobController.SetaValorProgressBarVazio;
                bllBilhetesImp.ObjProgressBar = pb;
                //Recalcular as marcações
                try
                {
                    bllBilhetesImp.RecalculaAlteracaoBilhete(marcFuncAlocado, bllMarcacao, bllImportaBilhetes, 0, 0);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao reprocessar a marcação de origem, erro: "+e.Message);
                }

                try
                {
                    bllBilhetesImp.RecalculaAlteracaoBilhete(marcFuncTransferir, bllMarcacao, bllImportaBilhetes, 0, 0);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao reprocessar a marcação de destino, erro: "+e.Message);
                }

                return Json(new { Success = true, Erro = "", Mensagem = "Bilhetes transferidos com sucesso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private static void CarregaEValidaDadosTransferenciaBilhetes(int idMarcacao, string selecionados, BLL.Marcacao bllMarcacao, BLL.Funcionario bllFuncionario, BLL.BilhetesImp bllBilhetesImp, ref Marcacao marcAlocado,ref Marcacao marcFuncTransferir, ref Funcionario funcAlocado, ref Funcionario funcTransferir, List<Modelo.BilhetesImp> bilhetesAlocar)
        {
            marcAlocado = bllMarcacao.LoadObject(idMarcacao);
            if (marcAlocado == null || marcAlocado.Id == 0)
                throw new Exception("Erro ao carregar a marcação alocada");

            funcAlocado = bllFuncionario.LoadObject(marcAlocado.Idfuncionario);
            if (funcAlocado == null || funcAlocado.Id == 0)
                throw new Exception("Erro ao carregar o funcionário com as batidas alocadas");

            int idf = funcAlocado.Id;
            funcTransferir = bllFuncionario.GetAllListPorPis(new List<string>() { funcAlocado.Pis }).Where(w => w.Id != idf).OrderByDescending(o => o.bFuncionarioativo).ThenByDescending(t => t.Dataadmissao).FirstOrDefault();
            if (funcTransferir == null || funcTransferir.Id == 0)
                throw new Exception("Erro ao carregar o funcionário a ser alocado as batidas");


            marcFuncTransferir = bllMarcacao.GetPorFuncionario(funcTransferir.Id, marcAlocado.Data, marcAlocado.Data, true).FirstOrDefault();
            if (marcFuncTransferir == null)
            {
                List<Modelo.MarcacaoLista> marcs = bllMarcacao.GetMarcacaoListaPorFuncionario(funcTransferir.Id, marcAlocado.Data.AddDays(-1), marcAlocado.Data.AddDays(1));
                marcFuncTransferir = bllMarcacao.GetPorFuncionario(funcTransferir.Id, marcAlocado.Data, marcAlocado.Data, true).FirstOrDefault();
            }
            if (marcFuncTransferir == null || marcFuncTransferir.Id == 0)
                throw new Exception("Erro ao carregar a marcação a ser alocada");

            foreach (int idBilhete in selecionados.Split(',').Select(s => Convert.ToInt32(s)))
            {
                bilhetesAlocar.Add(bllBilhetesImp.LoadObject(idBilhete));
            }

            if (bilhetesAlocar == null || bilhetesAlocar.Count == 0)
                throw new Exception("Erro ao carregar os bilhetes a serem alocados");
        }

        [PermissoesFiltro(Roles = "ManutencaoBilhetesCadastrar")]
        public ActionResult Cadastrar(int id, int idMarcacao)
        {
            return GetPagina(id, idMarcacao);
        }

        [PermissoesFiltro(Roles = "ManutencaoBilhetesAlterar")]
        public ActionResult Alterar(int id, int idMarcacao)
        {
            return GetPagina(id, idMarcacao);
        }

        [PermissoesFiltro(Roles = "ManutencaoBilhetesCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(BilhetesImp obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ManutencaoBilhetesAlterar")]
        [HttpPost]
        public ActionResult Alterar(BilhetesImp obj)
        {
            return Salvar(obj);
        }

        protected ActionResult Salvar(BilhetesImp obj)
        {
            try
            {
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                Marcacao mar = bllMarcacao.LoadObject(obj.idMarcacao);
                
                obj.Ocorrencia = Regex.Replace(obj.Ocorrencia.ToString(), "[^0-9a-zA-Z]+", "").ToCharArray().FirstOrDefault();

                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
                bllBilhetesImp.ManutencaoBilhete(mar, obj, obj.tipoManutencao - 1);
                return Json(new
                {
                    JobId = "",
                    Progress = "",
                    Erro = ""
                });
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new
                {
                    JobId = "",
                    Progress = "",
                    Erro = ex.Message
                });
            }
        }

        protected ActionResult GetPagina(int id, int idMarcacao)
        {
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
            BilhetesImp bilhetesImp = new BilhetesImp();
            bilhetesImp = bllBilhetesImp.LoadObject(id);
            bilhetesImp.idMarcacao = idMarcacao;
            return View("Cadastrar", bilhetesImp);
        }
    }

    public class Localizacao
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Horario { get; set; }
    }
}