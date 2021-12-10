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
    public class ClassificacaoHorasExtrasController : Controller
    {
        //[PermissoesFiltro(Roles = "ClassificacaoHorasExtras")]
        public ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Proxy.pxyClassHorasExtrasMarcacao());
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                IList<pxyClassHorasExtrasMarcacao> dados = bllClassificacaoHorasExtras.GetClassificacoesMarcacao(id);
                JsonResult jsonResult = Json(new { data = dados.Where(w => w.IdClassificacaoHorasExtras > 0) }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "ClassificacaoHorasExtrasConsultar")]
        public ActionResult Consultar(int id, int idMarc)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id, idMarc);
        }

        [PermissoesFiltro(Roles = "ClassificacaoHorasExtrasCadastrar")]
        public ActionResult Cadastrar(int id = 0, int idMarc = 0)
        {
            return GetPagina(0, idMarc);
        }

        [PermissoesFiltro(Roles = "ClassificacaoHorasExtrasAlterar")]
        public ActionResult Alterar(int id = 0, int idMarc = 0)
        {
            return GetPagina(id, idMarc);
        }

        [PermissoesFiltro(Roles = "ClassificacaoHorasExtrasCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(ClassificacaoHorasExtras obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ClassificacaoHorasExtrasAlterar")]
        [HttpPost]
        public ActionResult Alterar(ClassificacaoHorasExtras obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ClassificacaoHorasExtrasExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ClassificacaoHorasExtras ClassificacaoHorasExtras = bllClassificacaoHorasExtras.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllClassificacaoHorasExtras.Salvar(Acao.Excluir, ClassificacaoHorasExtras);
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
                return TratarErros(ex);
            }
        }

        private ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_Funcionario_ClassificacaoHorasExtras"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Salvar(ClassificacaoHorasExtras obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ValidarForm(obj);
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
                    erros = bllClassificacaoHorasExtras.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        foreach (KeyValuePair<string,string> item in erros)
                        {
                            ModelState[item.Key].Errors.Add(item.Value);
                        }
                    }
                    else
                    {
                        return RedirectToAction("ClassificadasMarcacao", new { id = obj.IdMarcacao });
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return PartialView("Cadastrar", obj);
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult HorasExtrasAClassificar(string empresa, string departamento, string contrato, string funcionario, string dataInicial, string dataFinal)
        {
            try
            {
                departamento = departamento == null ? "" : departamento;
                contrato = contrato == null ? "" : contrato;
                funcionario = funcionario == null ? "" : funcionario;
                empresa = empresa == null ? "" : empresa;
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
                int idEmp = 0;
                int idDep = 0;
                int idCont = 0;
                int idFunc = 0;
                string erro = "";
                DateTime dtInicial, dtFinal;
                if ((DateTime.TryParse(dataInicial, out dtInicial)) && (DateTime.TryParse(dataFinal, out dtFinal)))
                {
                    if (bllFuncionario.ValidaEmpDepContFunc(empresa, departamento, contrato, funcionario, ref idEmp, ref idDep, ref idCont, ref idFunc, ref erro))
                    {
                        pxyClassHorasExtras classHorasExtras = new pxyClassHorasExtras();
                        List<int> idsFuncs = new List<int>();
                        if (idFunc > 0)
	                    {
		                     idsFuncs.Add(idFunc);
                             ViewBag.ExibirColunaFunc = false;
	                    }
                        else
                        {
                            ViewBag.ExibirColunaFunc = true;
                            idsFuncs = bllFuncionario.GetIdsFuncsPorIdsEmpOuDepOuContra(idDep, idCont, idEmp).ToList();
                        }
                        if (idsFuncs.Count == 0)
                        {
                            throw new Exception("Não foi possível carregar os funcionários.");
                        }
                        BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(conn, usr);
                        classHorasExtras.pxyClassHorasExtrasMarcacoes = bllClassificacaoHorasExtras.GetMarcacoesClassificar(idsFuncs, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
                        return PartialView(classHorasExtras);
                    }
                    else
                    {
                        return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { erro = true, mensagemErro = "Data Inválida" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception w)
            {
                BLL.cwkFuncoes.LogarErro(w);
                return Json(new { erro = true, mensagemErro = w.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult ClassificadasMarcacao(int id)
        {
            try
            {
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(conn, usr);
                IList<pxyClassHorasExtrasMarcacao> classificadas = bllClassificacaoHorasExtras.GetClassificacoesMarcacao(id);
                return PartialView(classificadas);
            }
            catch (Exception w)
            {
                BLL.cwkFuncoes.LogarErro(w);
                return Json(new { erro = true, mensagemErro = w.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected ActionResult GetPagina(int id, int idMarc)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(conn, usr);
            ClassificacaoHorasExtras classificacaoHorasExtras = new ClassificacaoHorasExtras();
            classificacaoHorasExtras = bllClassificacaoHorasExtras.LoadObject(id);
            IList<pxyClassHorasExtrasMarcacao> classMarc = bllClassificacaoHorasExtras.GetClassificacoesMarcacao(idMarc);
            if (id == 0)
            {
                classificacaoHorasExtras.Codigo = bllClassificacaoHorasExtras.MaxCodigo();
                classificacaoHorasExtras.IdMarcacao = idMarc;
                classificacaoHorasExtras.QtdNaoClassificadaDiurna = classMarc.FirstOrDefault().NaoClassificadasDiurna;
                classificacaoHorasExtras.QtdNaoClassificadaNoturna = classMarc.FirstOrDefault().NaoClassificadasNoturna;
                classificacaoHorasExtras.Tipo = 1;
            }
            else
            {
                BLL.Classificacao bllClassificacao = new BLL.Classificacao(conn, usr);
                Classificacao classi = bllClassificacao.LoadObject(classificacaoHorasExtras.IdClassificacao);
                classificacaoHorasExtras.CodigoDescricaoClassificacao = classi.Codigo + " | " + classi.Descricao;
                int classificadasDiurna = Modelo.cwkFuncoes.ConvertHorasMinuto(classMarc.Where(w => w.IdClassificacaoHorasExtras == classificacaoHorasExtras.Id).FirstOrDefault().ClassificadasDiurna);
                if (classificadasDiurna > classMarc.FirstOrDefault().HorasExtrasRealizadaDiurnaMin)
	            {
		             classificadasDiurna = classMarc.FirstOrDefault().HorasExtrasRealizadaDiurnaMin;
	            }
                classificacaoHorasExtras.QtdNaoClassificadaDiurna = Modelo.cwkFuncoes.ConvertMinutosHora2(2, (classMarc.FirstOrDefault().NaoClassificadasDiurnaMin + classificadasDiurna));

                int classificadasNoturna = Modelo.cwkFuncoes.ConvertHorasMinuto(classMarc.Where(w => w.IdClassificacaoHorasExtras == classificacaoHorasExtras.Id).FirstOrDefault().ClassificadasNoturna);
                if (classificadasNoturna > classMarc.FirstOrDefault().HorasExtrasRealizadaNoturnaMin)
                {
                    classificadasNoturna = classMarc.FirstOrDefault().HorasExtrasRealizadaNoturnaMin;
                }

                classificacaoHorasExtras.QtdNaoClassificadaNoturna = Modelo.cwkFuncoes.ConvertMinutosHora2(2, (classMarc.FirstOrDefault().NaoClassificadasNoturnaMin + classificadasNoturna));
            }
            if (classificacaoHorasExtras.Tipo == 2)
            {
                ViewBag.Consultar = 1;
            }
            return View("Cadastrar", classificacaoHorasExtras);
        }

        protected void ValidarForm(ClassificacaoHorasExtras obj)
        {
            if (!String.IsNullOrEmpty(obj.CodigoDescricaoClassificacao))
            {
                BLL.Classificacao bllClass = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                int? idClass = null;
                int codigo;
                string classificacao = obj.CodigoDescricaoClassificacao.Split('|')[0].Trim();
                if (int.TryParse(classificacao, out codigo))
                {
                    idClass = bllClass.GetIdPorCod(codigo);
                }
                if (idClass.GetValueOrDefault() > 0)
                {
                    obj.IdClassificacao = idClass.GetValueOrDefault();
                }
                else
                {
                    ModelState["CodigoDescricaoClassificacao"].Errors.Add("Classificação " + classificacao + " não cadastrada!");
                }
            }

            if (Modelo.cwkFuncoes.ConvertHorasMinuto(obj.QtdHoraClassificadaDiurna) > Modelo.cwkFuncoes.ConvertHorasMinuto(obj.QtdNaoClassificadaDiurna))
            {
                ModelState["QtdHoraClassificadaDiurna"].Errors.Add("Quantidade classificada maior que a disponível!");
            }
            if (Modelo.cwkFuncoes.ConvertHorasMinuto(obj.QtdHoraClassificadaNoturna) > Modelo.cwkFuncoes.ConvertHorasMinuto(obj.QtdNaoClassificadaNoturna))
            {
                ModelState["QtdHoraClassificadaNoturna"].Errors.Add("Quantidade classificada maior que a disponível!");
            }
        }
    }
}
