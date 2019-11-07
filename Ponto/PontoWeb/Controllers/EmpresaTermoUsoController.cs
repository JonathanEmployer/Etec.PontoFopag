using BLL.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class EmpresaTermoUsoController : IControllerPontoWeb<EmpresaTermoUso>
    {
        private UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();
        #region Métodos Padrões
        [PermissoesFiltro(Roles = "EmpresaTermoUso")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(bllEmpresaTermoUso.GetAllList());
        }

        [PermissoesFiltro(Roles = "EmpresaTermoUsoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EmpresaTermoUsoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "EmpresaTermoUsoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EmpresaTermoUsoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(EmpresaTermoUso obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EmpresaTermoUsoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(EmpresaTermoUso obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EmpresaTermoUsoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            EmpresaTermoUso EmpresaTermoUso = bllEmpresaTermoUso.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllEmpresaTermoUso.Salvar(Acao.Excluir, EmpresaTermoUso);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return TratarErros(ex);
            }
        }

        private ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_Funcionario_EmpresaTermoUso"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(EmpresaTermoUso obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllEmpresaTermoUso.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "EmpresaTermoUso");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            EmpresaTermoUso EmpresaTermoUso = new EmpresaTermoUso();
            EmpresaTermoUso = bllEmpresaTermoUso.LoadObject(id);
            if (id == 0)
            {
                EmpresaTermoUso.Codigo = bllEmpresaTermoUso.MaxCodigo();
            }
            return View("Cadastrar", EmpresaTermoUso);
        }

        protected override void ValidarForm(EmpresaTermoUso obj)
        {
            throw new NotImplementedException();
        } 
        #endregion

        public ActionResult VisualizarTermo(int codigo, string cei, string cnpj, string cpf, string nome, string endereco, string cidade, string estado, string cep)
        {
            try
            {
                BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(_user.ConnectionString, _user);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                string html = bllEmpresaTermoUso.GetTermoResponsabilidade(codigo, cei, cnpj, cpf, nome, endereco, cidade, cep, out erros);

                if (erros.Count > 0)
                {
                    return Json(new { Success = false, Erro = String.Join(" <br /> ", erros.Select(s => s.Value).Distinct().ToList()) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = true, Erro = " ", HTML = html }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult ImprimirTermoUso(int id)
        {
            BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(_user.ConnectionString, _user);
            Dictionary<string, string> erros = new Dictionary<string, string>();
            var termoUsos = bllEmpresaTermoUso.LoadObjectsByIdsEmpresa(new List<int>() { id});
            EmpresaTermoUso empresaTermoUso = termoUsos.Where(w => w.Tipo == 1).FirstOrDefault();

            HtmlReport htmlReport = new HtmlReport();
            byte[] buffer = htmlReport.RenderPDF(empresaTermoUso.TermoAceito, true, true);
            return File(buffer, "application/pdf", "Termo Responsabilidade APP.pdf");
        }

        public ActionResult GerarTermoUso(int id)
        {
            try
            {
                BLL.EmpresaTermoUso bllEmpresaTermoUso = new BLL.EmpresaTermoUso(_user.ConnectionString, _user);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                var termoUsos = bllEmpresaTermoUso.LoadObjectsByIdsEmpresa(new List<int>() { id });
                if (termoUsos == null || termoUsos.Count == 0 || termoUsos.Where(w => w.Tipo == 1).FirstOrDefault().Id == 0)
                {
                    return Json(new { success = false, erro = "Empresa não possui termo de responsabilidade aceito" }, JsonRequestBehavior.AllowGet);
                }
                EmpresaTermoUso empresaTermoUso = termoUsos.Where(w => w.Tipo == 1).FirstOrDefault();

                //StringReader sr = new StringReader(empresaTermoUso.TermoAceito);

                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //byte[] buffer = null;
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                //    pdfDoc.Open();

                //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                //    pdfDoc.Close();

                //    buffer = memoryStream.ToArray();
                //    memoryStream.Close();
                //}

                HtmlReport htmlReport = new HtmlReport();
                byte[] buffer = htmlReport.RenderPDF(empresaTermoUso.TermoAceito, true, false);
                var fName = string.Format("File-{0}.pdf", DateTime.Now.ToString("s"));
                Session[fName] = buffer;
                return Json(new { success = true, fName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, erro = "Erro ao gerar o termo de responsabilidade, tente novamente, caso o erro persista entre em contato com o suporte!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadTermoUso(string fName)
        {
            var file = Session[fName] as byte[];
            if (file == null)
                return new EmptyResult();
            Session[fName] = null;
            return File(file, "application/pdf", "Termo Responsabilidade APP.pdf");
        }
    }
}
