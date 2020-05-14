using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoCartaoPontoController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        //[PermissoesFiltro(Roles = "LancamentoCartaoPontoCadastrar")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult Index()
        {
            bool UtilizaControleContrato = _usr.UtilizaControleContratos;
            ViewBag.UtilizaControleContrato = UtilizaControleContrato;
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            DateTime dataIni = new DateTime();
            DateTime dataFin = new DateTime();
            bool carregouPeriodoFechamento = false;
            bool mudadataautomaticamente;

            LancamentoCartaoPonto lancamentoCartaoPonto = new LancamentoCartaoPonto();

            lancamentoCartaoPonto.Empresa = "0 | TODAS AS EMPRESAS";
            lancamentoCartaoPonto.Departamento = "0 | TODOS OS DEPARTAMENTOS";
            lancamentoCartaoPonto.Contrato = "0 | TODOS OS CONTRATOS";

            BLL.Empresa bllEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr);
            List<Empresa> empresas = bllEmpresa.GetAllList();
            if (empresas.Count == 1)
            {
                lancamentoCartaoPonto.Empresa = empresas.FirstOrDefault().Codigo + " | " + empresas.FirstOrDefault().Nome;
                if (!_usr.UtilizaControleContratos)
                {
                    BLL.Departamento bllDepartamento = new BLL.Departamento(_usr.ConnectionString, _usr);
                    List<Departamento> departs = bllDepartamento.LoadPEmpresa(empresas.FirstOrDefault().Id);
                    if (departs.Count() == 1)
                    {
                        lancamentoCartaoPonto.Departamento = departs.FirstOrDefault().Codigo + " | " + departs.FirstOrDefault().Descricao;
                    }
                }
                else
                {
                    BLL.Contrato bllContrato = new BLL.Contrato(_usr.ConnectionString, _usr);
                    List<Contrato> contratos = bllContrato.GetAllListPorEmpresa(empresas.FirstOrDefault().Id);
                    if (contratos.Count() == 1)
                    {
                        lancamentoCartaoPonto.Contrato = contratos.FirstOrDefault().Codigo + " | " + contratos.FirstOrDefault().DescricaoContrato;
                        carregouPeriodoFechamento = BLL.cwkFuncoes.AtribuiPeriodoFechamentoPonto(out dataIni, out dataFin, contratos.FirstOrDefault().DiaFechamentoInicial, contratos.FirstOrDefault().DiaFechamentoFinal);
                    }
                }

                if (!carregouPeriodoFechamento)
                {
                    carregouPeriodoFechamento = BLL.cwkFuncoes.AtribuiPeriodoFechamentoPonto(out dataIni, out dataFin, empresas.FirstOrDefault().DiaFechamentoInicial, empresas.FirstOrDefault().DiaFechamentoFinal);
                }
            }

            if (!carregouPeriodoFechamento)
            {
                BLL.ConfiguracoesGerais bllConfiguracoes = new BLL.ConfiguracoesGerais(_usr.ConnectionString, _usr);
                bllConfiguracoes.AtribuiDatas(String.Empty, out dataIni, out dataFin, out mudadataautomaticamente);
            }

            if (dataIni != new DateTime())
            {
                lancamentoCartaoPonto.DataInicial = dataIni.ToShortDateString();
            }
            if (dataFin != new DateTime())
            {
                lancamentoCartaoPonto.DataFinal = dataFin.ToShortDateString();
            }
            return View(lancamentoCartaoPonto);
        }

        //[PermissoesFiltro(Roles = "LancamentoCartaoPontoCadastrar")]
        [HttpPost]
        public ActionResult Index(LancamentoCartaoPonto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<BilhetesImp> bilhetes = new List<BilhetesImp>();
                    bool virouDia = false;

                    var registrosSalvar = obj.Regs.Where(w => w.Editavel).ToArray();
                    LancamentoCartaoPontoRegistros reg = new LancamentoCartaoPontoRegistros();
                    BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                    int idJustificativa = JustificativaController.BuscaIdJustificativa(obj.DescJustificativa);
                    int idFuncionario = FuncionarioController.BuscaIdFuncionario(obj.Funcionario);
                    Funcionario funcionario = bllFuncionario.LoadObject(idFuncionario);
                    string regAnt = "";
                    string ent = "";
                    string sai = "";
                    int maxCodigoBilhetes = bllBilhetesImp.MaxCodigo();
                    for (int i = 0; i < registrosSalvar.Count(); i++)
                    {
                        reg = registrosSalvar[i];
                        virouDia = false;
                        for (int j = 1; j < 9; j++)
                        {
                            var ev = reg.GetType().GetProperty("E" + j).GetValue(reg, null);
                            ent = ev == null ? "" : ev.ToString();
                            AddBilhete(bilhetes, reg.Data, !virouDia ? reg.Data : reg.Data.AddDays(+1), ent, "E", funcionario, idJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                            ValidaViradaDia(ref virouDia, ref regAnt, ent);
                            var sv = reg.GetType().GetProperty("S" + j).GetValue(reg, null);
                            sai = sv == null ? "" : sv.ToString();
                            AddBilhete(bilhetes, reg.Data, !virouDia ? reg.Data : reg.Data.AddDays(+1), sai, "S", funcionario, idJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                            ValidaViradaDia(ref virouDia, ref regAnt, ent);
                        }
                    }

                    if (bilhetes.Count > 0)
                    {
                        int ret = bllBilhetesImp.Salvar(Acao.Incluir, bilhetes);
                        HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase, _usr.Login, "", "/LancamentoCartaoPonto/Grid");
                        Modelo.Proxy.PxyJobReturn job = hfm.ImportarBilhetes("Processamento de Cartão Ponto Manual", $"Processando Funcionário { funcionario.Dscodigo } | { funcionario.Nome } para { bilhetes.Count() } registros lançados", funcionario.Id, bilhetes.Min(m => m.Data), bilhetes.Max(m => m.Data));
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "Nenhum registro novo lançado para o funcionário");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("CustomError", e.Message);
            }
            
            return View(obj);
        }

        private static void ValidaViradaDia(ref bool virouDia, ref string regAnt, string hora)
        {
            if (!string.IsNullOrEmpty(regAnt) && !string.IsNullOrEmpty(regAnt) && regAnt.ConvertHorasMinuto() < hora.ConvertHorasMinuto())
            {
                virouDia = true;
            }
            regAnt = hora;
        }

        private void AddBilhete(List<BilhetesImp> bilhetes, DateTime dt, DateTime dt_mar, string hora, string entSai, Funcionario funcionario, int idJustificativa, string motivo, int codigo, int posicao)
        {
            if (!string.IsNullOrEmpty(hora))
            {
                bilhetes.Add(new BilhetesImp()
                {
                    Acao = Acao.Incluir,
                    Codigo = codigo,
                    Data = dt,
                    Mar_data = dt_mar,
                    Hora = hora,
                    Mar_hora = hora,
                    Relogio = "MA",
                    Mar_relogio = "MA",
                    Idjustificativa = idJustificativa,
                    IdFuncionario = funcionario.Id,
                    PIS = funcionario.Pis,
                    DsCodigo = funcionario.Dscodigo,
                    Func = funcionario.Dscodigo,
                    Ent_sai = entSai,
                    Importado = 0,
                    Ordem = entSai == "E" ? "010" : "011",
                    Posicao = posicao,
                    Incdata = DateTime.Now.Date,
                    Inchora = DateTime.Now,
                    Incusuario = _usr.Login,
                    Ocorrencia = 'I',
                    Motivo = motivo
                });
            }
        }

        public ActionResult GridMarcacoes(LancamentoCartaoPonto lcp)
        {
            try
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                string erro = "";
                Modelo.Funcionario func = bllFuncionario.ValidaEmpDepFunc(lcp.Empresa, lcp.Departamento, lcp.Funcionario, ref erro);
                if (erro == "" && func != null && func.Id > 0)
                {
                    bool UtilizaControleContrato = _usr.UtilizaControleContratos;
                    ViewBag.UtilizaControleContrato = UtilizaControleContrato;
                    List<LancamentoCartaoPontoRegistros> regs = bllMarcacao.GetLancamentoCartaoPonto(func.Id, Convert.ToDateTime(lcp.DataInicial), Convert.ToDateTime(lcp.DataFinal));
                    lcp.Regs = regs;
                    return PartialView(lcp);
                }
                else
                {
                    return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception w)
            {
                BLL.cwkFuncoes.LogarErro(w);
                return Json(new { erro = true, mensagemErro = w.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}