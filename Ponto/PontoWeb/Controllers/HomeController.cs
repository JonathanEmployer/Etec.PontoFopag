using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Modelo.EntityFramework;

namespace PontoWeb.Controllers
{

    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Página Principal";
            return View();
        }

        public ActionResult Landing()
        {
            ViewBag.Title = "Página Principal";
            return View();
        }

        [Authorize]
        public ActionResult Erro(string mensagem)
        {
            ViewBag.Erro = mensagem;
            return View();
        }

        [Authorize]
        public ActionResult HttpError404(string mensagem)
        {
            ViewBag.Titulo = "Página não encontrada.";
            mensagem = mensagem.Replace("O controlador do", "O");
            mensagem = mensagem.Replace(" ou não implementa IController", "");
            ViewBag.Erro = mensagem;
            return View();
        }

        public ActionResult Suporte()
        {
            Modelo.UsuarioPontoWeb Usuario = BLLWeb.Usuario.GetUsuarioPontoWebLogadoCache();
            string nome = Usuario.Nome;
            string cpf = Usuario.Cpf;
            string email = Usuario.Email;
            string empresa = Usuario.EmpresaPrincipal.Nome;

            string urlAloWeb = String.Format(@"https://v4.aloweb.com.br/chat/atendimentos/standalone?token=vKb8IBsjAUp0PKvqj3wsUJ3bpw5CJ9a9e9tAGBJk&language=pt_br&id=718&name={0}&email={1}", nome, email);
            return Json(new { url = urlAloWeb, div= "alo49939200", config = "scrollbars=no ,width=522, height=533 , top=50 , left=50"}, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //public ActionResult Teste()
        //{
        //    Teste teste = new Teste();
        //    teste.UltimaExecucao = teste.DataExecucao = teste.Hora = DateTime.Now;
        //    return View(teste);
        //}

        [Authorize]
        [AsyncTimeout(54000000)]
        public ActionResult Reprocessar()
        {
            HttpContext.Server.ScriptTimeout = 900;
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(conn, usr);
            BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(conn, usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, usr);
            DateTime? dataInicial;
            DateTime? dataFinal;
            List<string> log = new List<string>();
            DataTable listaFuncionarios = new DataTable();
            Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            DateTime? dtini = Convert.ToDateTime("2018-03-04");
            DateTime? dtfin = Convert.ToDateTime("2018-03-13");

            //Método responsável para retornar todos os funcionários com bilhetes importados = 0 na data informada
            DataTable dt = bllImportaBilhetes.ReturnFuncsWithoutDsCod(dtini, dtfin);

            string dsCodigo = "";
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dr.ItemArray.Length; i++)
                {
                    dsCodigo += dsCodigo == "" ? dr.ItemArray[i].ToString() : ("," + dr.ItemArray[i].ToString());
                }
            }
            //Linha abaixo reprecessa apenas os bilhetes como bilhetesimp = 0 e reimporta ele
            bllImportaBilhetes.ImportarBilhetesWebApi(dsCodigo, false, dtini, dtfin, out dataInicial, out dataFinal, objProgressBar, log, usr.Login, ref listaFuncionarios);

            //Linha abaixo reprocessa todos os funcionários
            //listaFuncionarios = bllFuncionario.GetAll();
            //dataInicial = dtini;
            //dataFinal = dtfin;
            int contador = 0;
            foreach (DataRow funcionario in listaFuncionarios.Rows)
            {
                contador++;
                BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(2, Convert.ToInt32(funcionario["id"]), dataInicial.Value, dataFinal.Value.AddDays(1), objProgressBar, false, conn, true, false);
                bllCalculaMarcacao.CalculaMarcacoesWebApi(usr.Login);
            }
            listaFuncionarios.Dispose();
            return Index();
        }

        private void SetaValorProgressBar(int valor)
        {
        }

        private void SetaMinMaxProgressBar(int min, int max)
        {
        }

        private void SetaMensagem(string mensagem)
        {
        }

        private void IncrementaProgressBar(int incrementaProgressBar)
        {
        }

        public ActionResult Calcular(string usuario)
        {
            PontoWeb.Controllers.JobManager.HangfireManager hangfireManager = new JobManager.HangfireManager(User.Identity.Name);
            Modelo.Proxy.PxyJobReturn retorno = hangfireManager.Calculo();
            return Json(retorno, JsonRequestBehavior.DenyGet);
        }
    }

    public class Teste
    {
        [Display(Name = "Última Execução")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? UltimaExecucao { get; set; }

        [Display(Name = "Última Execução")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataExecucao { get; set; }


        [Display(Name = "Hora Execução")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? Hora { get; set; }
    }
}
