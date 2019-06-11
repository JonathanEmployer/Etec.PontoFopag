using iTextSharp.text;
using iTextSharp.text.pdf;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelCartaoPontoCustomController : Controller
    {
        // GET: RelCartaoPontoCustom
        public ActionResult Index()
        {
            //string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            //var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            //BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            //BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usr);
            //List<Funcionario> funcs = bllFuncionario.GetAllList(false);
            //BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, usr);
            //IList<pxyCartaoPontoEmployer> cps = new List<pxyCartaoPontoEmployer>();
            //foreach (Funcionario func in funcs)
            //{
            //    Empresa emp = bllEmpresa.LoadObject(func.Idempresa);
            //    pxyCartaoPontoEmployer cp = new pxyCartaoPontoEmployer();
            //    cp.NomeEmpresa = func.Empresa;
            //    cp.Endereco = emp.Endereco;
            //    cp.Cidade = emp.Cidade;
            //    cp.Funcionario = func.nomeCodigo;
            //    cp.Departamento = func.Departamento;
            //    cp.DataAdmissao = func.Dataadmissao.GetValueOrDefault().ToShortDateString();

            //    List<Modelo.Marcacao> marcs = bllMarcacao.GetPorFuncionario(func.Id, Convert.ToDateTime("01/03/2016"), Convert.ToDateTime("31/03/2016"), true);
            //    cp.Marcacao = marcs;
            //    cps.Add(cp);
            //}
            //return View(cps.ToList());
            return View();
        }

        public ActionResult CartaoPonto(pxyCartaoPontoEmployer cpImp)
        {
            return PartialView(cpImp);
        }

        public class CartoesParciais
        {
            public int Parte { get; set; }
            public byte[] Cartao { get; set; }
        }
        public ActionResult Impressao()
        {
            //RetornoErro retErro = new RetornoErro();
            //try
            //{
            //    Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
            //    BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(userPW);

            //    if (ModelState.IsValid)
            //    {
            //        PxyFileResult arquivo = bllDB1.GetRelatorio(rec, ConfigurationManager.AppSettings["ApiDB1"]);

            //        if (arquivo.Erros != null && arquivo.Erros.Count > 0)
            //        {
            //            retErro.erroGeral = "Erro encontrado, verifique os detalhes dos erro!";
            //            retErro.ErrosDetalhados = new List<ErroDetalhe>();
            //            foreach (KeyValuePair<string, string> Erro in arquivo.Erros)
            //            {
            //                retErro.ErrosDetalhados.Add(new ErroDetalhe() { campo = Erro.Key, erro = Erro.Value });
            //            }
            //        }
            //        else
            //        {
            //            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            //            response.Content = new ByteArrayContent(arquivo.Arquivo);
            //            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //            response.Content.Headers.ContentDisposition.FileName = arquivo.FileName;
            //            response.Content.Headers.ContentType = new MediaTypeHeaderValue(arquivo.MimeType);

            //            return response;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    retErro.erroGeral = ex.Message;
            //}

            //return TrataErroModelState(retErro);
            byte[] buffer1 = null;
            return File(buffer1, "application/PDF");
        }

        private static double CalculaTempo(DateTime Iniciou)
        {
            TimeSpan span = DateTime.Now - Iniciou;
            double totalMinutes = span.TotalSeconds;
            return totalMinutes;
        }
    }
}