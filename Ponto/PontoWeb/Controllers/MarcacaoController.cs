using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class MarcacaoController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "Marcacao")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult TabelaMarcacao()
        {
            List<string> permissoes = Usuario.GetAcessoCache();

            if (permissoes.Exists(s => s.Contains("MarcacaoConsultar")) && !permissoes.Exists(s => s.Contains("MarcacaoAlterar")))
            {
                ViewBag.consultar = 1;
            }
            else
            {
                ViewBag.Consultar = 0;
            }

            bool UtilizaControleContrato = _usr.UtilizaControleContratos;
            ViewBag.UtilizaControleContrato = UtilizaControleContrato;
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            DateTime dataIni = new DateTime();
            DateTime dataFin = new DateTime();
            bool carregouPeriodoFechamento = false;
            bool mudadataautomaticamente;

            Marcacao marc = new Marcacao();

            marc.Empresa = "0 | TODAS AS EMPRESAS";
            marc.Departamento = "0 | TODOS OS DEPARTAMENTOS";
            marc.Contrato = "0 | TODOS OS CONTRATOS";

            BLL.Empresa bllEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr);
            List<Empresa> empresas = bllEmpresa.GetAllList();
            if (empresas.Count == 1)
            {
                marc.Empresa = empresas.FirstOrDefault().Codigo + " | " + empresas.FirstOrDefault().Nome;
                if (!_usr.UtilizaControleContratos)
                {
                    BLL.Departamento bllDepartamento = new BLL.Departamento(_usr.ConnectionString, _usr);
                    List<Departamento> departs = bllDepartamento.LoadPEmpresa(empresas.FirstOrDefault().Id);
                    if (departs.Count() == 1)
                    {
                        marc.Departamento = departs.FirstOrDefault().Codigo + " | " + departs.FirstOrDefault().Descricao;
                    }
                }
                else
                {
                    BLL.Contrato bllContrato = new BLL.Contrato(_usr.ConnectionString, _usr);
                    List<Contrato> contratos = bllContrato.GetAllListPorEmpresa(empresas.FirstOrDefault().Id);
                    if (contratos.Count() == 1)
                    {
                        marc.Contrato = contratos.FirstOrDefault().Codigo + " | " + contratos.FirstOrDefault().DescricaoContrato;
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
                marc.DataInicial = dataIni;
            }
            if (dataFin != new DateTime())
            {
                marc.DataFinal = dataFin;
            }
            return View(marc);

        }

        [Authorize]
        public ActionResult DadosFuncionario(string empresa, string departamento, string funcionario)
        {
            string erro = "";
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Modelo.Funcionario func = bllFuncionario.ValidaEmpDepFunc(empresa, departamento, funcionario, ref erro);
            if (erro == "" && func != null && func.Id > 0)
            {
                return PartialView(func);
            }
            else
            {
                return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult GridMarcacoes(string empresa, string departamento, string funcionario, string dataInicial, string dataFinal)
        {
            try
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                string erro = "";
                Modelo.Funcionario func = bllFuncionario.ValidaEmpDepFunc(empresa, departamento, funcionario, ref erro);
                if (erro == "" && func != null && func.Id > 0)
                {
                    bool UtilizaControleContrato = _usr.UtilizaControleContratos;
                    ViewBag.UtilizaControleContrato = UtilizaControleContrato;
                    List<Modelo.MarcacaoLista> marcs = bllMarcacao.GetMarcacaoListaPorFuncionario(func.Id, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
                    Modelo.GridMarcacoes marc = bllMarcacao.GerarGridMarcacoes(marcs);
                    return PartialView(marc);
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

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult VisualizacaoResumoHoras(string empresa, string departamento, string funcionario, string dataInicial, string dataFinal)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            string erro = "";
            Modelo.Funcionario func = bllFuncionario.ValidaEmpDepFunc(empresa, departamento, funcionario, ref erro);
            Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
            BLL.Parametros bllParametros = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            BLL.Horario bllHorario = new BLL.Horario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Horario objHorario = bllHorario.LoadObject(func.Idhorario);
            Modelo.Parametros objParametros = bllParametros.LoadObject(objHorario.Idparametro);
            if (erro == "" && func != null && func.Id > 0)
            {
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(func, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal), _usr.ConnectionString, _usr);
                totalizadorHoras.TotalizeHorasEBancoHoras(objTotalHoras);
                objTotalHoras.lRateioHorasExtras = new List<RateioHorasExtras>();
                foreach (var rateio in objTotalHoras.RateioHorasExtras)
                {
                    RateioHorasExtras nRateio = new RateioHorasExtras();
                    nRateio.percentual = rateio.Key;
                    nRateio.diurnoMin = rateio.Value.Diurno;
                    nRateio.noturnoMin = rateio.Value.Noturno;
                    nRateio.diurno = Modelo.cwkFuncoes.ConvertMinutosHora(4, rateio.Value.Diurno);
                    nRateio.noturno = Modelo.cwkFuncoes.ConvertMinutosHora(4, rateio.Value.Noturno);
                    objTotalHoras.lRateioHorasExtras.Add(nRateio);
                }
                ViewBag.Periodo = Convert.ToDateTime(dataInicial).ToShortDateString() + " à " + Convert.ToDateTime(dataFinal).ToShortDateString();
                objTotalHoras.funcionario = func;
                totalizadorHoras.TotalizeGruposInItinere(objTotalHoras);
                objTotalHoras.HabilitarControleInItinere = objParametros != null && objParametros.HabilitarControleInItinere;
                return PartialView(objTotalHoras);
            }
            else
            {
                return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
            }
        }

        [PermissoesFiltro(Roles = "FuncionarioConsultar")]
        public ActionResult ConsultaHorario(int id)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            BLL.Horario bllHorario = new BLL.Horario(_usr.ConnectionString, _usr);
            if (id > 0)
            {
                Marcacao m = bllMarcacao.LoadObject(id);
                if (m.Idhorario > 0)
                {
                    Horario h = bllHorario.LoadObject(m.Idhorario);
                    if (h.TipoHorario == 1) // normal
                    {
                        return RedirectToAction("ConsultarExt", "Horario", new { id = h.Id, ctrl = "Marcacao", acao = "TabelaMarcacao" });
                    }
                    else
                    {
                        return RedirectToAction("ConsultarExt", "HorarioMovel", new { id = h.Id, ctrl = "Marcacao", acao = "TabelaMarcacao" });
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None", VaryByCustom = "User")]
        public void CarregaIdporData(int idFuncionario, DateTime data, bool avanca)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();           
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, usr);

            DateTime dateNew = new DateTime(data.Year, data.Month, data.Day);

            if (avanca)
            {
                dateNew = dateNew.AddDays(1);
            }else
            {
                dateNew = dateNew.AddDays(-1);
            }

            Response.Redirect("~/Marcacao/ManutMarcacao/" + bllMarcacao.retornaIdMarcacao(idFuncionario, dateNew).ToString());
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None", VaryByCustom = "User")]
        public ActionResult ManutMarcacao(int id)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(_usr.ConnectionString, _usr);
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(_usr.ConnectionString, _usr);

            Modelo.Marcacao objMarcacao = bllMarcacao.LoadObject(id);
            objMarcacao.FolgaAnt = objMarcacao.Folga;
            foreach (BilhetesImp bilhete in objMarcacao.BilhetesMarcacao)
            {
                if (bilhete.Idjustificativa > 0)
                {
                    Justificativa jus = bllJustificativa.LoadObject(bilhete.Idjustificativa);
                    if (jus != null && jus.Id > 0)
                    {
                        bilhete.DescJustificativa = jus.Codigo + " | " + jus.Descricao;
                    }
                }
            }

            #region preenche Bilhetes que ainda não existem e podem existir após edição.
            string Ent_saida = "";
            string Ordem = "";
            for (int i = 0; i < 16; i++)
            {
                if (i % 2 == 0)
                {
                    Ent_saida = "E";
                    Ordem = "010";
                }
                else
                {
                    Ent_saida = "S";
                    Ordem = "011";
                }
                double pos = (i + 1.0) / 2.0;
                pos = Math.Ceiling(pos);
                if (objMarcacao.BilhetesMarcacao.Where(x => x.Ent_sai == Ent_saida && x.Posicao == Convert.ToInt32(pos)).Count() == 0)
                {
                    BilhetesImp bl = new BilhetesImp();
                    bl.Ent_sai = Ent_saida;
                    bl.Ordem = Ordem;
                    bl.Posicao = Convert.ToInt32(pos);
                    bl.Codigo = bllBilhetesImp.MaxCodigo();
                    bl.Acao = Modelo.Acao.Incluir;
                    bl.Data = objMarcacao.Data;
                    bl.Mar_data = objMarcacao.Data;
                    bl.DsCodigo = objMarcacao.Dscodigo;
                    bl.Func = objMarcacao.Dscodigo;
                    bl.Relogio = "MA";
                    bl.Mar_relogio = "MA";
                    bl.Importado = 1;
                    objMarcacao.BilhetesMarcacao.Add(bl);
                }
            }

            PropertyInfo[] propsMarcacao = objMarcacao.GetType().GetProperties();
            foreach (BilhetesImp bil in objMarcacao.BilhetesMarcacao.OrderBy(x => x.Ent_sai).ThenBy(n => n.Posicao))
            {
                string campo = "";
                if (bil.Ent_sai == "E")
                {
                    campo = "Ent_Legenda_" + bil.Posicao;
                }
                else
                {
                    campo = "Sai_Legenda_" + bil.Posicao;
                }
                PropertyInfo prop = propsMarcacao.FirstOrDefault(f => f.Name == campo);
                if (prop != null)
                {
                    if (!String.IsNullOrEmpty(bil.Ocorrencia.ToString()) && bil.Ocorrencia.ToString() != "\0")
                    {
                        prop.SetValue(objMarcacao, bil.Ocorrencia.ToString(), null);
                    }
                    else
                    {
                        prop.SetValue(objMarcacao, " ", null);
                    }

                }
            }
            #endregion

            #region Afastamento
            if (objMarcacao.Afastamento.IdOcorrencia > 0)
            {
                Ocorrencia oco = bllOcorrencia.LoadObject(objMarcacao.Afastamento.IdOcorrencia);
                objMarcacao.Afastamento.ocorrencia = oco.Codigo + " | " + oco.Descricao;
                objMarcacao.Afastamento.ocorrenciaAnt = oco.Codigo + " | " + oco.Descricao;
                objMarcacao.Afastamento.Acao = Modelo.Acao.Alterar;
            }
            #endregion
            TempData["Consultar"] = 0;
            TempData.Keep("Consultar");
            if (objMarcacao.Idfechamentobh > 0 || objMarcacao.IdFechamentoPonto > 0 || objMarcacao.DocumentoWorkflowAberto)
            {
                TempData["Consultar"] = 1;
                if (objMarcacao.Idfechamentobh > 0)
                {
                    BLL.FechamentoBH bllBH = new BLL.FechamentoBH(_usr.ConnectionString, _usr);
                    FechamentoBH fechamentoBH = new FechamentoBH();
                    fechamentoBH = bllBH.LoadObject(objMarcacao.Idfechamentobh);
                    ViewBag.MensagemBloqueio = "Marcação não pode ser alterada, já existe um fechamento de banco de horas no dia " + fechamentoBH.Data.GetValueOrDefault().ToShortDateString();
                }
                else if (objMarcacao.IdFechamentoPonto > 0)
                {
                    BLL.FechamentoPonto bllFP = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
                    FechamentoPonto fechamentoPonto = new FechamentoPonto();
                    fechamentoPonto = bllFP.LoadObject(objMarcacao.IdFechamentoPonto);
                    ViewBag.MensagemBloqueio = "Marcação não pode ser alterada, já existe um fechamento de ponto no dia " + fechamentoPonto.DataFechamento.ToShortDateString();
                }
                else if (objMarcacao.DocumentoWorkflowAberto)
                {
                    #region Verifica se existe permissão para liberar o fluxo no painel do rh
                    BLL.UsuarioPontoWeb bllUsuario = new BLL.UsuarioPontoWeb(_usr.ConnectionString, _usr);
                    Modelo.UsuarioPontoWeb usuario = bllUsuario.LoadObjectByCodigo(_usr.Codigo);
                    ViewBag.PermissaoConcluirFluxoPnlRh = usuario.PermissaoConcluirFluxoPnl == true ? true : false;
                    #endregion

                    ViewBag.MensagemBloqueio = "Marcação não pode ser alterada pois está pendente de aprovação no Painel do RH. Verifique!";
                }
            }
            if (objMarcacao.BilhetesMarcacao != null && objMarcacao.BilhetesMarcacao.Count > 0)
            {
                objMarcacao.BilhetesMarcacao = objMarcacao.BilhetesMarcacao.OrderBy(O => O.Posicao).ThenBy(o => o.Ent_sai).ToList();
            }
            return View(objMarcacao);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ManutMarcacao(Modelo.Marcacao marcacao)
        {
            try
            {
                if (marcacao.Afastamento.Id == 0)
                {
                    ModelState.Remove("Afastamento.Datai");
                    ModelState.Remove("Afastamento.Dataf");
                    ModelState.Remove("BilhetesMarcacao[0].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[1].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[2].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[3].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[4].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[5].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[6].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[7].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[8].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[9].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[10].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[11].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[12].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[13].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[14].Ocorrencia");
                    ModelState.Remove("BilhetesMarcacao[15].Ocorrencia");
                    if (!marcacao.Afastamento.BAbonado && !marcacao.Afastamento.BSemCalculo && !marcacao.Afastamento.bSuspensao && string.IsNullOrEmpty(marcacao.Afastamento.ocorrencia))
                    {
                        ModelState.Remove("Afastamento.ocorrencia");
                    }
                }
                RetirarOcorrenciaBilhete(marcacao);
                ValidarMarcacaoManualJustificativa(marcacao);
                ValidaAlteracoesNosBilhetes(marcacao, _usr);
                if (ModelState.IsValid)
                {
                    List<BilhetesImp> BilValidos = new List<BilhetesImp>();
                    foreach (BilhetesImp bil in marcacao.BilhetesMarcacao.Where(w => ((!String.IsNullOrEmpty(w.Mar_hora) && w.Mar_hora != "--:--") || w.Acao == Acao.Excluir)))
                    {
                        bil.Idjustificativa = JustificativaController.BuscaIdJustificativa(bil.DescJustificativa);
                        if (!Char.IsLetterOrDigit(bil.Ocorrencia))
                        {
                            bil.Ocorrencia = '\0';
                        }
                        if (bil.Incdata == DateTime.MinValue)
                        {
                            bil.Incdata = DateTime.Now.Date;
                            bil.Inchora = DateTime.Now;
                        }
                        if (bil.Acao == 0)
                        {
                            if (bil.Id > 0)
                                bil.Acao = Acao.Alterar;
                            else
                                bil.Acao = Acao.Incluir;
                        }
                        BilValidos.Add(bil);
                    }

                    marcacao.BilhetesMarcacao = BilValidos;

                    if (marcacao.BilhetesMarcacao.Where(w => w.IdFuncionario == 0 || String.IsNullOrEmpty(w.PIS)).Count() > 0)
                    {
                        BLL.Funcionario bllFunc = new BLL.Funcionario(_usr.ConnectionString, _usr);
                        Modelo.Funcionario func = bllFunc.LoadObject(marcacao.Idfuncionario);
                        marcacao.BilhetesMarcacao.Where(w => w.IdFuncionario == 0 || String.IsNullOrEmpty(w.PIS)).ToList().ForEach(f => { f.PIS = func.Pis; f.IdFuncionario = func.Id; });
                    }

                    if (string.IsNullOrEmpty(marcacao.Afastamento.Horai))
                        marcacao.Afastamento.Horai = "--:--";
                    if (string.IsNullOrEmpty(marcacao.Afastamento.Horaf))
                        marcacao.Afastamento.Horaf = "--:--";
                    if (marcacao.Afastamento.Abonado == 1 && (marcacao.Afastamento.Horai != "--:--" || marcacao.Afastamento.Horaf != "--:--"))
                    {
                        marcacao.Afastamento.Parcial = 1;
                    }

                    if (marcacao.Afastamento.ocorrencia != marcacao.Afastamento.ocorrenciaAnt)
                    {
                        if (!string.IsNullOrEmpty(marcacao.Afastamento.ocorrencia))
                        {
                            marcacao.Afastamento.IdOcorrencia = OcorrenciaController.BuscaIdOcorrencia(marcacao.Afastamento.ocorrencia);
                            if (string.IsNullOrEmpty(marcacao.Afastamento.ocorrenciaAnt))
                            {
                                marcacao.Afastamento.Acao = Modelo.Acao.Incluir;
                                marcacao.Afastamento.Datai = marcacao.Data;
                                marcacao.Afastamento.Dataf = marcacao.Data;
                                marcacao.Afastamento.IdFuncionario = marcacao.Idfuncionario;
                                BLL.Afastamento bllAfastamento = new BLL.Afastamento(_usr.ConnectionString, _usr);
                                marcacao.Afastamento.Codigo = bllAfastamento.MaxCodigo();
                            }
                            else
                                marcacao.Afastamento.Acao = Modelo.Acao.Alterar;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(marcacao.Afastamento.ocorrencia) && !string.IsNullOrEmpty(marcacao.Afastamento.ocorrenciaAnt))
                            {
                                marcacao.Afastamento.Acao = Modelo.Acao.Excluir;
                            }
                        }
                    }

                    #region Tratamento campo Bloquear Edição Painel
                    if (marcacao.BloquearEdicaoPnlRh == true)
                    {
                        marcacao.DataBloqueioEdicaoPnlRh = DateTime.Now;
                        marcacao.LoginBloqueioEdicaoPnlRh = _usr.Login;
                    }
                    else
                    {
                        marcacao.DataBloqueioEdicaoPnlRh = null;
                        marcacao.LoginBloqueioEdicaoPnlRh = null;
                    }

                    #endregion

                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                    Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
                    objProgressBar.incrementaPB = this.IncrementaProgressBar;
                    objProgressBar.setaMensagem = this.SetaMensagem;
                    objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
                    objProgressBar.setaValorPB = this.SetaValorProgressBar;
                    try
                    {
                        bllMarcacao.ObjProgressBar = objProgressBar;
                        Dictionary<string, string> Erros = new Dictionary<string, string>();
                        Erros = bllMarcacao.Salvar(Modelo.Acao.Alterar, marcacao);
                        if (Erros.Count() > 0)
                        {
                            throw new Exception("Erro ao salvar Marcações, Detalhe: " + String.Join(" </br> ", Erros.Select(x => x.Value)));
                        }
                    }
                    catch (Exception e)
                    {
                        BLL.cwkFuncoes.LogarErro(e);
                        throw;
                    }

                }
                var err = ModelState.Values.Where(w => w.Errors.Count > 0);
                return Json(new
                {
                    JobId = "",
                    Progress = "",
                    Erro = string.Join("; ", err
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage))
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

        private void ValidaAlteracoesNosBilhetes(Marcacao marcacao, UsuarioPontoWeb usr)
        {
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(usr.ConnectionString, usr);
            List<BilhetesImp> BilsAnt = bllBilhetesImp.LoadObject(marcacao.BilhetesMarcacao.Select(s => s.Id).ToList());
            if (marcacao.BilhetesMarcacao.Count > 0)
            {
                foreach (var objBilheteImp in marcacao.BilhetesMarcacao)
                {
                    Modelo.BilhetesImp bilAnt = BilsAnt.Where(w => w.Id == objBilheteImp.Id).FirstOrDefault();
                    if (bilAnt != null && bilAnt.Id > 0 && bilAnt.Mar_relogio != "MA" && bilAnt.Mar_relogio != "PE" && (objBilheteImp.Mar_data != bilAnt.Mar_data || objBilheteImp.Mar_hora != bilAnt.Mar_hora || objBilheteImp.Mar_relogio != bilAnt.Mar_relogio))
                        ModelState.AddModelError(
                            "CustomError",
                            "A " + (objBilheteImp.Ent_sai.Equals("E") ? "Entrada " : "Saida ") + objBilheteImp.Posicao + ", hora " + objBilheteImp.Hora + ", não é um registro que pode ser editado."
                        );
                };
            }
        }

        private void RetirarOcorrenciaBilhete(Modelo.Marcacao marcacao)
        {
            int i = 0;
            foreach (var bilhetes in marcacao.BilhetesMarcacao)
            {
                if (bilhetes.Ocorrencia.ToString() == "\0")
                {
                    string bilhetesRemove = "BilhetesMarcacao[" + i + "].Ocorrencia";
                    ModelState.Remove(bilhetesRemove);
                }
                i++;
            }
        }

        private void ValidarMarcacaoManualJustificativa(Marcacao marcacao)
        {
            // Validar Marcação Manual / Justificativa
            if (marcacao.BilhetesMarcacao.Count > 0)
            {
                Parallel.ForEach(marcacao.BilhetesMarcacao, objBilheteImp =>
                {
                    if (!string.IsNullOrWhiteSpace(objBilheteImp.Relogio) && objBilheteImp.Relogio.Equals("MA") &&
                    !string.IsNullOrWhiteSpace(objBilheteImp.Hora) && string.IsNullOrWhiteSpace(objBilheteImp.DescJustificativa) && objBilheteImp.Acao != Modelo.Acao.Excluir)
                        ModelState.AddModelError(
                            "CustomError",
                            "A " + (objBilheteImp.Ent_sai.Equals("E") ? "Entrada " : "Saida ") + objBilheteImp.Posicao + ", hora " + objBilheteImp.Hora + ", deve conter uma justificativa."
                        );
                });
            }
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

        private void IncrementaProgressBar(int incremento)
        {
        }



        public ActionResult OrdenaMarcacaoIndividual(int id)
        {
            if (id > 0)
            {
                try
                {
                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                    Modelo.Marcacao objMarcacao = bllMarcacao.LoadObject(id);

                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                    Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(objMarcacao.Idfuncionario);

                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase);
                    string parametrosExibicao = String.Format("Ordenada Marcação do dia: {0}, do funcionário: {1} | {2}", objMarcacao.Data.ToString("dd/MM/yyyy"), objFuncionario.Dscodigo, objFuncionario.Nome);
                    Modelo.Proxy.PxyJobReturn ret = hfm.OrdenaMarcacao("Recalculo de marcações para ordenação de registros", parametrosExibicao, objMarcacao);
                    return new JsonResult { Data = new { success = true, job = ret } };
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    return new JsonResult { Data = new { success = false, Erro = ex.Message } };
                }
            }
            else
            {
                return new JsonResult { Data = new { success = false, Erro = "Nenhum registro selecionado." } };
            }
        }

        public ActionResult OrdenaMarcacaoPeriodo(string empresa, string departamento, string funcionario, string dataInicial, string dataFinal)
        {
            string erro = "";
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Modelo.Funcionario func = bllFuncionario.ValidaEmpDepFunc(empresa, departamento, funcionario, ref erro);
            if (erro == "" && func != null && func.Id > 0)
            {
                try
                {
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase);
                    string parametrosExibicao = String.Format("Ordenada Marcação do período: {0} a {1}, do funcionário: {2} | {3}", dataInicial, dataFinal, func.Dscodigo, func.Nome);
                    Modelo.Proxy.PxyJobReturn ret = hfm.OrdenaMarcacaoPeriodo("Recalculo de marcações para ordenação de registros", parametrosExibicao, func.Id, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
                    return new JsonResult { Data = new { success = true, job = ret } };
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    erro = ex.Message;
                }
            }
            return new JsonResult { Data = new { success = false, Erro = erro } };
        }

        [Authorize]
        [PermissoesFiltro(Roles = "BloqueioEdicaoPnlRhCadastrar")]
        public ActionResult BloquearEdicaoLotePainelRHPartial(string id, string funcionario, string dataInicial, string dataFinal)
        {
            BLL.Funcionario bllFunc = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Modelo.BloquearLiberarEdicaoPnlRh parms = new Modelo.BloquearLiberarEdicaoPnlRh();

            if (!String.IsNullOrEmpty(dataInicial) && !String.IsNullOrEmpty(dataFinal))
            {
                try
                {
                    parms.DataInicial = Convert.ToDateTime(dataInicial);
                    parms.DataFinal = Convert.ToDateTime(dataFinal);
                }
                catch (Exception)
                {
                    string erro = String.Empty;
                    erro = "Não foi possível realizar a conversão das datas, por favor verifique!";
                    if (!String.IsNullOrEmpty(erro))
                    {
                        return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
                    }
                    throw;
                }
            }

            Modelo.Funcionario func;
            if (!String.IsNullOrEmpty(funcionario))
            {
                var codFunc = Regex.Match(funcionario, @"\d+").Value;
                func = bllFunc.LoadObjectByCodigo(Convert.ToInt32(codFunc));
                parms.idFunc = func.Id;
            }
            else
	        {
                 string erro = String.Empty;
                    erro = "Funcionário não encontrado, por favor verifique!";
                    if (!String.IsNullOrEmpty(erro))
                    {
                        return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
                    }
	        }

            return PartialView(parms);

        }

        [Authorize]
        [HttpPost]
        [PermissoesFiltro(Roles = "BloqueioEdicaoPnlRhCadastrar")]
        public ActionResult BloquearLiberarEdicaoLotePainelRH(Modelo.BloquearLiberarEdicaoPnlRh parms)
        {
            try
            {
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                DateTime? ultimaDataMarc = bllMarcacao.GetLastDateMarcacao(parms.idFunc);
                if (ultimaDataMarc != null && parms.DataFinal > ultimaDataMarc)
                {
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();

                    objFuncionario = bllFuncionario.LoadObject(parms.idFunc);

                    bllMarcacao.AtualizaData(parms.DataInicial, parms.DataFinal, objFuncionario);
                }

                bllMarcacao.BloqLibEdicaoPnlRh(parms.DataInicial, parms.DataFinal, parms.idFunc, parms.TipoSolicitacao);

                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false, Erro = "Erro ao realizar o lançamento de Bloqueio/Liberação no banco de dados. Entre em contato com o administrador do sistema." }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ConcluirFluxoPnlRh(int idMarcacao, string idFuncionario)
        {
            try
            {
                string token, erro;
                BLL.IntegracaoPainel.EmpregadoImportar bllPainelRh = new BLL.IntegracaoPainel.EmpregadoImportar(_usr.ConnectionString, _usr);
                BLL.Funcionario bllFunc = new BLL.Funcionario(_usr.ConnectionString, _usr);
                BLL.ParametroPainelRH bllParametrosPnlRh = new BLL.ParametroPainelRH(_usr.ConnectionString, _usr);
                IList<Modelo.Funcionario> func = null;
                Modelo.ParametroPainelRH parmsPnlRh = bllParametrosPnlRh.GetAllList().FirstOrDefault();

                if (!String.IsNullOrEmpty(idFuncionario))
	            {
                    func = bllFunc.GetFuncionariosPorIds(idFuncionario);
	            }

                bool validaLogin = bllPainelRh.ValidarLogin(func.FirstOrDefault(), out token, out erro, parmsPnlRh);
                if (validaLogin == true)
                {
                    BLL.IntegracaoPainel.WorkflowPnlRh bllWorkflow = new BLL.IntegracaoPainel.WorkflowPnlRh(_usr.ConnectionString, _usr);
                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);

                    int idDocumentoWorkflow = bllMarcacao.GetIdDocumentoWorkflow(idMarcacao);
                    bool ret = bllWorkflow.ConcluirWorkflow(idMarcacao, idDocumentoWorkflow, _usr.CPFUsuario, token);
                    if (ret == false)
                    {
                        throw new Exception("Erro ao enviar o objeto para a API do Painel. Por favor, verifique!");
                    }
                    else
                    {
                        bllMarcacao.IncluiUsrDtaConclusaoFluxoPnlRh(idMarcacao, DateTime.Now, _usr.Login);
                    }
                    return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false, Erro = "Problema ao validar login, por favor, verifique os parâmetros informados!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}
