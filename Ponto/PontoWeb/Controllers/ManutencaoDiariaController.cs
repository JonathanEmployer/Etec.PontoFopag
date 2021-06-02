using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Security;
using PontoWeb.Utils;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ManutencaoDiariaController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "MarcacaoAlterar")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult TabelaMarcacao()
        {
            bool UtilizaControleContrato = _usr.UtilizaControleContratos;
            ViewBag.UtilizaControleContrato = UtilizaControleContrato;

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            BLL.ConfiguracoesGerais bllConfiguracoes = new BLL.ConfiguracoesGerais(_usr.ConnectionString, _usr);
            Marcacao marc = new Marcacao();

            marc.Departamento = "";
            marc.Contrato = "";

            return View(marc);
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult GridMarcacoes(string empresa, string departamento, string contrato, string dataInicial, string dataFinal)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            int idEmp = 0;
            int idDep = 0;
            int idCont = 0;
            string erro = "";
            Modelo.ProgressBar pb = new Modelo.ProgressBar();
            pb.incrementaPB = this.IncrementaProgressBar;
            pb.setaMensagem = this.SetaMensagem;
            pb.setaMinMaxPB = this.SetaMinMaxProgressBar;
            pb.setaValorPB = this.SetaValorProgressBar;
            bllMarcacao.ObjProgressBar = pb;
            DateTime dtInicial, dtFinal;
            if ((DateTime.TryParse(dataInicial, out dtInicial)) && (DateTime.TryParse(dataFinal, out dtFinal)))
            {
                if (bllFuncionario.ValidaEmpDepCont(empresa, departamento, contrato, ref idEmp, ref idDep, ref idCont, ref erro))
                {
                    bool UtilizaControleContrato = _usr.UtilizaControleContratos;
                    ViewBag.UtilizaControleContrato = UtilizaControleContrato;
                    if (idEmp == 0)//Todos
                    {
                        return PartialView(bllMarcacao.GerarGridMarcacoes(bllMarcacao.GetPorDataManutDiaria(dtInicial, dtFinal, false, false)));
                    }
                    else if (idDep > 0)//Departamento
                    {
                        return PartialView(bllMarcacao.GerarGridMarcacoes(bllMarcacao.GetPorManutDiariaDep(idDep, dtInicial, dtFinal, false, false)));
                    }
                    else if (idCont > 0)//Contrato
                    {
                        return PartialView(bllMarcacao.GerarGridMarcacoes(bllMarcacao.GetPorManutDiariaCont(idCont, dtInicial, dtFinal, false, false)));
                    }
                    else if (idDep == 0 && idEmp > 0)//Por empresa
                    {
                        return PartialView(bllMarcacao.GerarGridMarcacoes(bllMarcacao.GetPorManutDiariaEmp(idEmp, dtInicial, dtFinal, false, false)));
                    }
                    else if (idCont == 0 && idEmp > 0)//Por empresa
                    {
                        return PartialView(bllMarcacao.GerarGridMarcacoes(bllMarcacao.GetPorManutDiariaEmp(idEmp, dtInicial, dtFinal, false, false)));
                    }
                    else
                    {
                        return PartialView(bllMarcacao.GerarGridMarcacoes(bllMarcacao.GetPorManutDiariaEmp(idEmp, dtInicial, dtFinal, false, true)));
                    }
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
        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult VisualizacaoResumoHoras(string empresa, string departamento, string funcionario, string contrato, string dataInicial, string dataFinal)
        {
            string erro = "";
            Modelo.Funcionario func = ValidaEmpDepFunc(empresa, departamento, funcionario, ref erro);
            Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
            BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
            Modelo.Parametros objParametros = bllParametros.LoadPrimeiro();
            if (erro == "" && func != null && func.Id > 0)
            {
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(func, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal), Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
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

        [Authorize]
        [HttpPost]
        public ActionResult RecalculaMarcacao(string empresa, string departamento, string contrato, string dataInicial, string dataFinal)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            string erro = "";
            int idEmp = 0;
            int idDep = 0;
            DateTime dtInicial, dtFinal;
            try
            {
                if ((DateTime.TryParse(dataInicial, out dtInicial)) && (DateTime.TryParse(dataFinal, out dtFinal)))
                {
                    int? tipo = null;
                    int identificador = 0;
                    if (bllFuncionario.ValidaEmpDep(empresa, departamento, ref idEmp, ref idDep, ref erro))
                    {
                        if (idEmp == 0)//Todos
                        {
                            identificador = idEmp;
                        }
                        else if (idDep > 0)//Departamento
                        {
                            tipo = 1;
                            identificador = idDep;
                        }
                        else if (idDep == 0 && idEmp > 0)//Por empresa
                        {
                            tipo = 0;
                            identificador = idEmp;
                        }
                        else
                        {
                            tipo = 1;
                            identificador = idDep;
                        }
                        HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase);
                        string parametrosExibicao = String.Format("Recalculo de marcacao da manutenção diária período: {0} a {1}", dataInicial, dataFinal);
                        Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações da manutenção diária", parametrosExibicao, tipo, identificador, Convert.ToDateTime(dataInicial), Convert.ToDateTime(dataFinal));
                        return new JsonResult
                        {
                            Data = new
                            {
                                success = true,
                                job = ret
                            }
                        };
                    }
                    else
                    {
                        return new JsonResult { Data = new { success = false, Erro = erro } };
                    }
                }
                else
                {
                    return new JsonResult { Data = new { success = false, Erro = "Data Inválida" } };
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return new JsonResult { Data = new { success = false, Erro = ex.Message } };
            }
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult ManutMarcacao(int id)
        {
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(_usr.ConnectionString, _usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(_usr.ConnectionString, _usr);
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(_usr.ConnectionString, _usr);
            Modelo.Marcacao objMarcacao = bllMarcacao.LoadObject(id);
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
            return View(objMarcacao);
        }

        [PermissoesFiltro(Roles = "FuncionarioConsultar")]
        public ActionResult ConsultaHorario(int id)
        {
            BLL.Horario bllHorario = new BLL.Horario(_usr.ConnectionString, _usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            if (id > 0)
            {
                Marcacao m = bllMarcacao.LoadObject(id);
                if (m.Idhorario > 0)
                {
                    Horario h = bllHorario.LoadObject(m.Idhorario);
                    if (h.TipoHorario == 1) // normal
                    {
                        return RedirectToAction("ConsultarExt", "Horario", new { id = h.Id, ctrl = "ManutencaoDiaria", acao = "TabelaMarcacao" });
                    }
                    else
                    {
                        return RedirectToAction("ConsultarExt", "HorarioMovel", new { id = h.Id, ctrl = "ManutencaoDiaria", acao = "TabelaMarcacao" });
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
        [HttpPost]
        public ActionResult ManutMarcacao(Modelo.Marcacao marcacao)
        {
            //try
            //{
            //    if (marcacao.Afastamento.Id == 0)
            //    {
            //        ModelState.Remove("Afastamento.Datai");
            //        ModelState.Remove("Afastamento.Dataf");
            //        ModelState.Remove("BilhetesMarcacao[0].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[1].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[2].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[3].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[4].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[5].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[6].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[7].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[8].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[9].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[10].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[11].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[12].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[13].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[14].Ocorrencia");
            //        ModelState.Remove("BilhetesMarcacao[15].Ocorrencia");
            //        if (!marcacao.Afastamento.BAbonado && !marcacao.Afastamento.BSemCalculo && !marcacao.Afastamento.bSuspensao && string.IsNullOrEmpty(marcacao.Afastamento.ocorrencia))
            //        {
            //            ModelState.Remove("Afastamento.ocorrencia");
            //        }
            //    }
            //    if (ModelState.IsValid)
            //    {
            //        List<BilhetesImp> BilValidos = new List<BilhetesImp>();
            //        foreach (BilhetesImp bil in marcacao.BilhetesMarcacao)
            //        {
            //            if ((!String.IsNullOrEmpty(bil.Mar_hora) && bil.Mar_hora != "--:--") || bil.Acao == Acao.Excluir)
            //            {
            //                bil.Idjustificativa = JustificativaController.BuscaIdJustificativa(bil.DescJustificativa);
            //                if (!Char.IsLetterOrDigit(bil.Ocorrencia))
            //                {
            //                    bil.Ocorrencia = '\0';
            //                }
            //                BilValidos.Add(bil);
            //            }
            //        }
            //        marcacao.BilhetesMarcacao = BilValidos;


            //        if (marcacao.Afastamento.ocorrencia != marcacao.Afastamento.ocorrenciaAnt)
            //        {
            //            if (!string.IsNullOrEmpty(marcacao.Afastamento.ocorrencia))
            //            {
            //                marcacao.Afastamento.IdOcorrencia = OcorrenciaController.BuscaIdOcorrencia(marcacao.Afastamento.ocorrencia);
            //                if (string.IsNullOrEmpty(marcacao.Afastamento.ocorrenciaAnt))
            //                {
            //                    marcacao.Afastamento.Acao = Modelo.Acao.Incluir;
            //                    marcacao.Afastamento.Datai = marcacao.Data;
            //                    marcacao.Afastamento.Dataf = marcacao.Data;
            //                    marcacao.Afastamento.IdFuncionario = marcacao.Idfuncionario;
            //                    BLL.Afastamento bllAfastamento = new BLL.Afastamento(_usr.ConnectionString, _usr);
            //                    marcacao.Afastamento.Codigo = bllAfastamento.MaxCodigo();
            //                    if (string.IsNullOrEmpty(marcacao.Afastamento.Horai))
            //                        marcacao.Afastamento.Horai = "--:--";
            //                    if (string.IsNullOrEmpty(marcacao.Afastamento.Horaf))
            //                        marcacao.Afastamento.Horaf = "--:--";
            //                }
            //                else
            //                    marcacao.Afastamento.Acao = Modelo.Acao.Alterar;
            //            }
            //            else
            //            {
            //                if (string.IsNullOrEmpty(marcacao.Afastamento.ocorrencia) && !string.IsNullOrEmpty(marcacao.Afastamento.ocorrenciaAnt))
            //                {
            //                    marcacao.Afastamento.Acao = Modelo.Acao.Excluir;
            //                }
            //            }
            //        }

            //        JobController jc = new JobController();
            //        Job jb = jc.RecalculaMarcacaoJob(marcacao, _usr.ConnectionString, _usr);
            //        return Json(new
            //        {
            //            JobId = jb.Id,
            //            Progress = jb.Progress,
            //            Erro = ""
            //        });
            //    }
            //    return Json(new
            //    {
            //        JobId = "",
            //        Progress = "",
            //        Erro = string.Join("; ", ModelState.Values
            //                            .SelectMany(x => x.Errors)
            //                            .Select(x => x.ErrorMessage))
            //    });
            //}
            //catch (Exception ex)
            //{
            //    BLL.cwkFuncoes.LogarErro(ex);
            //    return Json(new
            //    {
            //        JobId = "",
            //        Progress = "",
            //        Erro = ex.Message
            //    });
            //}
            MarcacaoController marcacaoController = new MarcacaoController();
            return marcacaoController.ManutMarcacao(marcacao);
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

                    UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW);
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

        #region Registrador de ponto
        public Modelo.Funcionario ValidaEmpDepFunc(string empresa, string departamento, string funcionario, ref string erro)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            Modelo.Funcionario func = new Modelo.Funcionario();

            BLL.Empresa bllEmp = new BLL.Empresa(conn, usr);
            Modelo.Empresa e = new Modelo.Empresa();
            int codEmpR;
            string codEmp = empresa.Split('|')[0].Trim();
            if (int.TryParse(codEmp, out codEmpR))
            {
                if (codEmpR != 0)
                {
                    e = bllEmp.LoadObjectByCodigo(codEmpR);
                }

                if (codEmpR != 0 && (e == null || e.Id <= 0))
                {
                    erro = "Empresa " + empresa + " não encontrada!";
                    return func;
                }
            }

            BLL.Departamento bllDep = new BLL.Departamento(conn, usr);
            Modelo.Departamento dep = new Modelo.Departamento();
            int codDepR;
            string codDep = departamento.Split('|')[0].Trim();
            if (int.TryParse(codDep, out codDepR))
            {
                if (codDepR != 0)
                {
                    dep = bllDep.LoadObjectByCodigo(codDepR);
                }

                if (codDepR != 0 && (dep == null || dep.Id <= 0))
                {
                    erro = "Departamento " + departamento + " não encontrado!";
                    return func;
                }
            }

            if (codDepR > 0 && codEmpR > 0)
            {
                if (dep.IdEmpresa != e.Id)
                {
                    erro = "Departamento " + departamento + " não pertence a empresa " + empresa;
                    return func;
                }
            }

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            int codFuncR;
            string codFunc = funcionario.Split('|')[0].Trim();
            if (int.TryParse(codFunc, out codFuncR))
            {
                if (codFuncR != 0)
                {
                    func = bllFuncionario.LoadObject(codFuncR);
                }

                if (func == null || func.Id <= 0)
                {
                    erro = "Funcionario " + funcionario + " não encontrado!";
                    return func;
                }

                if (codDepR > 0)
                {
                    if (func.Iddepartamento != dep.Id)
                    {
                        erro = "Funcionario " + funcionario + " não pertence ao departamento " + departamento;
                        return func;
                    }
                }

                if (codEmpR > 0)
                {
                    if (func.Idempresa != e.Id)
                    {
                        erro = "Funcionario " + funcionario + " não pertence a empresa " + empresa;
                        return func;
                    }
                }
            }
            return func;
        }


        public void IncrementaProgressBar(int incremento)
        {

        }

        public void SetaValorProgressBar(int valor)
        {

        }

        public void SetaMinMaxProgressBar(int min, int max)
        {

        }

        public void SetaMensagem(string mensagem)
        {

        }

        private Funcionario ValidaFuncionario(RegistraPonto regPonto)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Funcionario func = bllFuncionario.LoadPorCPF(regPonto.CPF);
            if (func.Nome == null)
                ModelState["CPF"].Errors.Add("Não existe funcionário cadastrado com este CPF!");
            else
            {
                if (func.Mob_Senha != regPonto.Senha)
                    ModelState["Senha"].Errors.Add("Senha incorreta!");
            }
            return func;
        }
        #endregion

        #region Impressão Cartão Ponto
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "cartaopdf")]
        public ActionResult CartaoPDF(RegistraPonto regPonto)
        {
            Funcionario func = ValidaFuncionario(regPonto);
            if (ModelState.IsValid)
            {
                try
                {
                    string tipoArquivo = "PDF";
                    ActionResult resultado = BuscarCartao(func, tipoArquivo);
                    if (resultado != null)
                    {
                        return resultado;
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            @ViewBag.comprovante = false;
            regPonto.HoraRegistroPonto = DateTime.Now.ToLongTimeString();
            return View(regPonto);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "cartaoexcel")]
        public ActionResult CartaoExcel(RegistraPonto regPonto)
        {
            Funcionario func = ValidaFuncionario(regPonto);
            if (ModelState.IsValid)
            {
                try
                {
                    string tipoArquivo = "Excel";
                    ActionResult resultado = BuscarCartao(func, tipoArquivo);
                    if (resultado != null)
                    {
                        return resultado;
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            @ViewBag.comprovante = false;
            regPonto.HoraRegistroPonto = DateTime.Now.ToLongTimeString();
            return View(regPonto);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "cartaoword")]
        public ActionResult CartaoWord(RegistraPonto regPonto)
        {
            Funcionario func = ValidaFuncionario(regPonto);
            if (ModelState.IsValid)
            {
                try
                {
                    string tipoArquivo = "Word";
                    ActionResult resultado = BuscarCartao(func, tipoArquivo);
                    if (resultado != null)
                    {
                        return resultado;
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            @ViewBag.comprovante = false;
            regPonto.HoraRegistroPonto = DateTime.Now.ToLongTimeString();
            return View(regPonto);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "cartaoimage")]
        public ActionResult CartaoImage(RegistraPonto regPonto)
        {
            Funcionario func = ValidaFuncionario(regPonto);
            if (ModelState.IsValid)
            {
                try
                {
                    string tipoArquivo = "Image";
                    ActionResult resultado = BuscarCartao(func, tipoArquivo);
                    if (resultado != null)
                    {
                        return resultado;
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            @ViewBag.comprovante = false;
            regPonto.HoraRegistroPonto = DateTime.Now.ToLongTimeString();
            return View(regPonto);
        }

        private static ActionResult BuscarCartao(Funcionario func, string tipoArquivo)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            DateTime? dataIni;
            DateTime? dataFin;
            DateTime hoje = DateTime.Now;
            dataIni = new DateTime(hoje.Year, hoje.Month, 1);
            dataFin = hoje;
            InicioFimDataFechamentoCartao(ref dataIni, ref dataFin);

            RelatoriosController rc = new RelatoriosController();
            ActionResult resultado = rc.ImprimirCartaoPonto(tipoArquivo, dataIni.GetValueOrDefault(), dataFin.GetValueOrDefault(), func, func.Idhorario, false, conn, usr);
            return resultado;
        }

        public static void InicioFimDataFechamentoCartao(ref DateTime? dataIni, ref DateTime? dataFin)
        {
            DateTime hoje = DateTime.Now;
            BLL.Parametros bllParametros = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Parametros param = bllParametros.LoadPrimeiro();
            if (param.DiaFechamentoInicial > 0 && param.DiaFechamentoFinal > 0)
            {
                if (param != null && param.DiaFechamentoInicial > 0)
                {
                    if (hoje.Day >= param.DiaFechamentoInicial)
                    {
                        dataIni = new DateTime(hoje.Year, hoje.Month, param.DiaFechamentoInicial);
                        dataFin = new DateTime(hoje.Year, hoje.AddMonths(1).Month, param.DiaFechamentoFinal);
                    }
                    else
                    {
                        dataIni = new DateTime(hoje.Year, hoje.AddMonths(-1).Month, param.DiaFechamentoInicial);
                        dataFin = new DateTime(hoje.Year, hoje.Month, param.DiaFechamentoFinal);
                    }
                }
            }
        }
        #endregion

    }
}