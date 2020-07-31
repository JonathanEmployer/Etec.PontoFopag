using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class HorarioController : IControllerPontoWeb<Horario>
    {
        [PermissoesFiltro(Roles = "Horario")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Horario());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
                List<Modelo.Horario> dados = bllHorario.GetAllList(false, false, 1, true);
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "HorarioConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "HorarioConsultar")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult ConsultarExt(int id, string ctrl, string acao)
        {
            UsuarioPontoWeb usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
            BLL.Parametros bllParametros = new BLL.Parametros(usr.ConnectionString, usr);
            ViewBag.Consultar = 1;
            ViewBag.RetController = ctrl;
            ViewBag.RetAcao = acao;

            Horario horario = new Horario();
            horario = bllHorario.LoadObject(id);
            if (id == 0)
            {
                bllHorario.InicializaHorario(ref horario);
                horario.Codigo = bllHorario.MaxCodigo();
                Parametros param = bllParametros.LoadPrimeiro();
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                horario.Idparametro = param.Id;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                horario.HorariosPHExtra[6].TipoAcumulo = -1;
                horario.HorariosPHExtra[7].TipoAcumulo = -1;
                horario.HorariosPHExtra[8].TipoAcumulo = -1;
                horario.HorariosPHExtra[9].TipoAcumulo = -1;
                horario.LHorariosDetalhe = horario.HorariosDetalhe.ToList();
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();
                horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                horario.Limitemin = "03:00";
                horario.Limitemax = "03:00";
                horario.Tipoacumulo = 0;
            }
            else
            {
                Parametros param = bllParametros.LoadObject(horario.Idparametro);
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                bllHorario.AtribuiHDHorario(horario);
                foreach (var item in horario.HorariosDetalhe)
                {
                    Jornada jornada = bllJornada.LoadObject(item.Idjornada.GetValueOrDefault());
                    item.DescJornada = jornada.Codigo + " | " + jornada.horarios;
                    item.Acao = Modelo.Acao.Alterar;
                }
                horario.LHorariosDetalhe = horario.HorariosDetalhe.ToList();
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();

                if (horario.HorariosAItinere == null || horario.HorariosAItinere.Count() == 0 || horario.HorariosAItinere[0] == null)
                {
                    horario.InicializaHorarioAItinere();
                    horario.HorariosAItinere = horario.LHorariosAItinere.ToArray();
                }
                else
                {
                    horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                }
                horario.DescJornadaCopiar = horario.HorariosDetalhe[0].DescJornada;
            }
            horario.InicioVigencia_Ant = horario.InicioVigencia;
            return PartialView("Cadastrar", horario);

        }

       
       

        [PermissoesFiltro(Roles = "HorarioCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }


        [PermissoesFiltro(Roles = "HorarioCadastrar")]
        public ActionResult HistoricoAlteracao(int id)
        {
            return GridAlteracaoHorario(id);
        }

        [PermissoesFiltro(Roles = "HorarioAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "HorarioCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Horario obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "HorarioAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Horario obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "HorarioExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            Horario horario = bllHorario.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllHorario.Salvar(Acao.Excluir, horario);
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
                return TrataErros(ex);
            }
        }

        private ActionResult TrataErros(Exception ex)
        {
            string msgTratada = String.Empty;
            if (ex.Message.Contains("FK_mudancahorario_horario"))
            {
                msgTratada = "Não é possivel excluir esse registro pois ele está relacionado à um registro de mudança de horário";
                return Json(new { Success = false, Erro = msgTratada }, JsonRequestBehavior.AllowGet);
            }
            else if (ex.Message.Contains("FK_funcionario_horario"))
            {
                msgTratada = "Não é possivel excluir esse registro pois ele está relacionado à um funcionário";
                return Json(new { Success = false, Erro = msgTratada }, JsonRequestBehavior.AllowGet);
            }
            else if (ex.Message.Contains("FK_marcacao_horario"))
            {
                msgTratada = "Não é possível excluir esse registro pois ele está relacionado a marcação de funcionário";
                return Json(new { Success = false, Erro = msgTratada }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(Horario obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            BLL.HorarioDetalhe bllHorarioDetalhe = new BLL.HorarioDetalhe(usr.ConnectionString, usr);
            BLL.HorarioPHExtra bllHorarioPHExtra = new BLL.HorarioPHExtra(usr.ConnectionString, usr);

            if (obj.PossuiFechamento) // Se possuir fechamento permite alterar apenas alguns campos
            {
                try
                {
                    Modelo.Horario horarioAnt = bllHorario.LoadObject(obj.Id);
                    if (horarioAnt.Ativo != obj.Ativo || horarioAnt.Descricao != obj.Descricao || (obj.HorarioRestricao != null && obj.HorarioRestricao.Where(w => w.Id == 0 || w.Excluir).Count() > 0))
                    {
                        horarioAnt.Ativo = obj.Ativo;
                        horarioAnt.Descricao = obj.Descricao;
                        horarioAnt.PossuiFechamento = obj.PossuiFechamento;
                        horarioAnt.HorarioRestricao = obj.HorarioRestricao;
                        obj = horarioAnt;
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllHorario.Salvar(Modelo.Acao.Alterar, obj);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                    }
                    return RedirectToAction("Grid", "Horario");
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            else
            {

                ValidarForm(obj);
                if (ModelState.IsValid)
                {
                    try
                    {
                        Acao acao = new Acao();
                        if (obj.Id == 0)
                        {
                            acao = Acao.Incluir;
                            //if (obj.IdHorarioOrigem != null)
                            //{
                            //    obj.HorariosDetalhe.ToList().ForEach(f => { f.Id = 0; f.Idhorario = 0; f.Acao = Acao.Incluir; });
                            //    obj.HorariosPHExtra.ToList().ForEach(f => { f.Id = 0; f.Idhorario = 0; });
                            //    obj.LimitesDDsrProporcionais.ToList().ForEach(f => { f.Id = 0; f.IdHorario = 0; f.Acao = Acao.Incluir; });
                            //    obj.LHorariosDetalhe.ToList().ForEach(f => { f.Id = 0; f.Idhorario = 0; f.Acao = Acao.Incluir; });
                            //    obj.LHorariosAItinere.ToList().ForEach(f => { f.Id = 0; f.Idhorario = 0; });
                            //}
                        }
                        else
                            acao = Acao.Alterar;

                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        TrataDados(obj);
                        erros = bllHorario.Salvar(acao, obj);

                        if (erros.Count > 0)
                        {
                            TrataErros(erros);      
                        }
                        else
                        {
                            if (!obj.PossuiFechamento)
                            {
                                List<int> idsFuncs = new List<int>();
                                List<Funcionario> funcs = FuncionariosVinculadosAosHorariosValidade(obj, usr.ConnectionString, usr);
                                idsFuncs = funcs.Select(s => s.Id).ToList();

                                if (idsFuncs.Count() != 0)
                                {
                                    DateTime datainicial;
                                    DateTime datafinal;
                                    if (DateTime.Now.Month == 1)
                                    {
                                        datainicial = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(-1).Year), Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                                        datafinal = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
                                    }
                                    else if (DateTime.Now.Month == 12)
                                    {
                                        datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                                        datafinal = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(1).Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
                                    }
                                    else
                                    {
                                        datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                                        datafinal = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
                                    }

                                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase, "", "", "/Horario/Grid");
                                    string parametrosExibicao = String.Format("Horário {0} | {1}, {2} funcionários.", obj.Codigo, obj.Descricao, idsFuncs.Count);
                                    Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por horário", parametrosExibicao, idsFuncs, datainicial, datafinal);
                                }
                            }
                            return RedirectToAction("Grid", "Horario");
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                    }
                }
            }
            return View("Cadastrar", obj);
        }

        private static List<Funcionario> FuncionariosVinculadosAosHorariosValidade(Horario obj, string conn, UsuarioPontoWeb usr)
        {
            List<Funcionario> funcs = new List<Funcionario>();
            BLL.Funcionario bllfunc = new BLL.Funcionario(conn, usr);
            int idhorario = 0;
            if (obj.IdHorarioOrigem != null && obj.IdHorarioOrigem > 0)
            {
                idhorario = obj.IdHorarioOrigem.GetValueOrDefault();
            }
            else
            {
                idhorario = obj.Id;
            }
            funcs = bllfunc.GetPorHorarioVigencia(idhorario);
            return funcs;
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
            BLL.Parametros bllParms = new BLL.Parametros(usr.ConnectionString, usr);
            BLL.HorarioAItinere bllHorasInItinere = new BLL.HorarioAItinere(usr.ConnectionString, usr);
            Horario horario = new Horario();
            horario = bllHorario.LoadObject(id);
            horario.InicioVigencia_Ant = horario.InicioVigencia;
            horario.Id_Ant = horario.Id;
            if (id == 0)
            {
                bllHorario.InicializaHorario(ref horario);
                horario.Codigo = bllHorario.MaxCodigo();
                Parametros param = bllParms.LoadPrimeiro();
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                horario.Idparametro = param.Id;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                horario.HorariosPHExtra[6].TipoAcumulo = -1;
                horario.HorariosPHExtra[7].TipoAcumulo = -1;
                horario.HorariosPHExtra[8].TipoAcumulo = -1;
                horario.HorariosPHExtra[9].TipoAcumulo = -1;
                horario.LHorariosDetalhe = horario.HorariosDetalhe.ToList();
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();
                horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                horario.Limitemin = "03:00";
                horario.Limitemax = "03:00";
                horario.Tipoacumulo = 0;
                horario.LimiteHorasTrabalhadasDia = "10:00";
                horario.LimiteMinimoHorasAlmoco = "01:00";
                horario.LimiteInterjornada = "11:00";
                horario.Ativo = true;
                horario.Tipoacumulo = 1;
            }
            else
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(usr.ConnectionString, usr);
                //Passado false fixo para não chamar a rotina de vigencia de alteração de horário
                horario.PossuiFuncionario = false;//bllFuncionario.GetPorHorario(horario.Id).Count() > 0 ? true : false;
                Parametros param = bllParms.LoadObject(horario.Idparametro);
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                bllHorario.AtribuiHDHorario(horario);
                foreach (var item in horario.HorariosDetalhe)
                {
                    Jornada jornada = bllJornada.LoadObject(item.Idjornada.GetValueOrDefault());
                    if (jornada.Id > 0)
                    {
                        item.DescJornada = jornada.Codigo + " | " + jornada.horarios;
                    }
                    item.Acao = Modelo.Acao.Alterar;
                }
                if (horario.LimitesDDsrProporcionais != null)
                {
                    foreach (var item in horario.LimitesDDsrProporcionais)
                    {
                        item.Acao = Acao.Alterar;
                    }
                }
                horario.LHorariosDetalhe = horario.HorariosDetalhe.ToList();
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();
                if (horario.HorariosAItinere == null || horario.HorariosAItinere.Count() == 0 || horario.HorariosAItinere[0] == null)
                {
                    horario.InicializaHorarioAItinere();
                    horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                }
                else
                {
                    horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                }


                horario.DescJornadaCopiar = horario.HorariosDetalhe[0].DescJornada;
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    string erro = "";
                    if (!validaFechamento(bllHorario, horario.Id, out erro))
                    {
                        @ViewBag.MensagemBloqueio = erro;
                        horario.PossuiFechamento = true;
                    }
                }
                #endregion 
            }

            if (horario.HorarioRestricao == null)
            {
                horario.HorarioRestricao = new List<HorarioRestricao>(); 
            }
            
            return View("Cadastrar", horario);
        }

        public ActionResult GridAlteracaoHorario(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            Horario horario = new Horario();
            horario = bllHorario.LoadObject(id);
            List<Modelo.Proxy.pxyHistoricoAlteracaoHorario> historicoalteracao = new List<Modelo.Proxy.pxyHistoricoAlteracaoHorario>();
            if (horario.IdHorarioOrigem != 0)
            {
                historicoalteracao = bllHorario.GetHistoricoAlteracaoHorario(horario.IdHorarioOrigem.GetValueOrDefault());
            }
            return View("GridAlteracaoHorario", historicoalteracao);
        }

        private bool validaFechamento(BLL.Horario bllHorario, int id, out string erro)
        {
            List<FechamentoPonto> lFechamentoPonto = bllHorario.FechamentoPontoHorario(new List<int> { id });
            if (lFechamentoPonto.Count > 0)
            {
                erro = "Operação não permitida, existe fechamento de ponto relacionado a esse registro. Fechamento: <br/> " + String.Join("<br/> ", lFechamentoPonto.Select(s => "Código: " + s.Codigo + " Data: " + s.DataFechamento.ToShortDateString()));
                return false;
            }
            erro = "";
            return true;
        }

        protected override void ValidarForm(Horario obj)
        {
            VerificaParametro(obj);
            VerificaJornada(obj);
            ValidaClassificacao(obj);           
            if (ModelState.ContainsKey("DataInicial"))
            {
                ModelState.Remove("DataInicial");
            }
            if (ModelState.ContainsKey("DataFinal"))
            {
                ModelState.Remove("DataFinal");
            }
            if (ModelState.ContainsKey("DiaSemanaInicioFolga"))
            {
                ModelState.Remove("DiaSemanaInicioFolga");
            }
            if (ModelState.ContainsKey("HorarioRestricao"))
            {
                ModelState.Remove("HorarioRestricao");
            }
            
            if (!obj.SeparaExtraNoturnaPercentual)
            {
                for (int i = 0; i < obj.LHorariosPHExtra.Count; i++)
                {
                    obj.LHorariosPHExtra[i].PercentualExtraNoturna = 0;
                    if (ModelState.ContainsKey("LHorariosPHExtra[" + i + "].PercentualExtraNoturna"))
                    {
                        ModelState.Remove("LHorariosPHExtra[" + i + "].PercentualExtraNoturna");
                    }
                }                
            }      
        }

        private static void TrataDados(Horario horario)
        {
            foreach (HorarioDetalhe hd in horario.LHorariosDetalhe)
            {
                hd.Flagfolga = Convert.ToInt16(!hd.bCarregarBool);
                hd.Intervaloautomatico = horario.Intervaloautomatico;
                hd.Entrada_1 = String.IsNullOrEmpty(hd.Entrada_1) ? "--:--" : hd.Entrada_1;
                hd.Entrada_2 = String.IsNullOrEmpty(hd.Entrada_2) ? "--:--" : hd.Entrada_2;
                hd.Entrada_3 = String.IsNullOrEmpty(hd.Entrada_3) ? "--:--" : hd.Entrada_3;
                hd.Entrada_4 = String.IsNullOrEmpty(hd.Entrada_4) ? "--:--" : hd.Entrada_4;
                hd.Saida_1 = String.IsNullOrEmpty(hd.Saida_1) ? "--:--" : hd.Saida_1;
                hd.Saida_2 = String.IsNullOrEmpty(hd.Saida_2) ? "--:--" : hd.Saida_2;
                hd.Saida_3 = String.IsNullOrEmpty(hd.Saida_3) ? "--:--" : hd.Saida_3;
                hd.Saida_4 = String.IsNullOrEmpty(hd.Saida_4) ? "--:--" : hd.Saida_4;
                hd.Totaltrabalhadadiurna = String.IsNullOrEmpty(hd.Totaltrabalhadadiurna) ? "--:--" : hd.Totaltrabalhadadiurna;
                hd.Totaltrabalhadanoturna = String.IsNullOrEmpty(hd.Totaltrabalhadanoturna) ? "--:--" : hd.Totaltrabalhadanoturna;
                hd.Cargahorariamista = String.IsNullOrEmpty(hd.Cargahorariamista) ? "--:--" : hd.Cargahorariamista;
                hd.Marcacargahorariamista = Convert.ToInt16(horario.MarcacargahorariamistaBool);
            }

            foreach (HorarioPHExtra HPHExtra in horario.LHorariosPHExtra)
            {
                HPHExtra.Quantidadeextra = String.IsNullOrEmpty(HPHExtra.Quantidadeextra) ? "--:--" : HPHExtra.Quantidadeextra;
            }
            horario.HorariosDetalhe = horario.LHorariosDetalhe.ToArray();
            horario.HorariosPHExtra = horario.LHorariosPHExtra.ToArray();

            if (horario.LHorariosAItinere == null || horario.LHorariosAItinere.Count() == 0)
                horario.InicializaHorarioAItinere();
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            Dictionary<string, string> erroTratado = new Dictionary<string, string>();
            erroTratado = erros.Where(x => x.Key.Equals("cbIdentificacao")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbIdentificacao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                ModelState["DescParametro"].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("DescClassificacao")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("DescClassificacao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                ModelState["DescClassificacao"].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("QtdHEPreClassificadas")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("QtdHEPreClassificadas")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                ModelState["QtdHEPreClassificadas"].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("txtEntrada_1")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtEntrada_1")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosDetalhe[0].DescJornada";
                ModelState[campoErro].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloSab")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloSab")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[6].TipoAcumulo";
                ModelState[campoErro].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloDom")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloDom")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[7].TipoAcumulo";
                ModelState[campoErro].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloFer")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloFer")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[8].TipoAcumulo";
                ModelState[campoErro].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloFol")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloFol")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[9].TipoAcumulo";
                ModelState[campoErro].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("Diasemanadsr")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("Diasemanadsr")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "Diasemanadsr";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
                //ModelState[campoErro].Errors.Add(string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }
            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        public ActionResult DadosParametros(string parametro)
        {
            int id = ParametroController.BuscaIdParametro(parametro);
            UsuarioPontoWeb usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Parametros bllParametros = new BLL.Parametros(usr.ConnectionString, usr);
            Parametros param = bllParametros.LoadObject(id);
            return Json(new { inicioAdNoturno = param.InicioAdNoturno, fimAdNoturno = param.FimAdNoturno });
        }

        private void VerificaJornada(Horario horario)
        {
            foreach (HorarioDetalhe hd in horario.LHorariosDetalhe)
            {
                if (!String.IsNullOrEmpty(hd.DescJornada))
                {
                    int idJornada = JornadaController.BuscaIdJornada(hd.DescJornada);
                    if (idJornada > 0)
                    {
                        hd.Idjornada = idJornada;
                    }
                    else
                    {
                        string campoErro = "LHorariosDetalhe[" + horario.LHorariosDetalhe.IndexOf(hd).ToString() + "].DescJornada";
                        ModelState[campoErro].Errors.Add("Jornada " + hd.DescJornada + " não cadastrada!");
                    }
                }
            }

        }

        private void VerificaParametro(Horario horario)
        {
            int idParam = ParametroController.BuscaIdParametro(horario.DescParametro);
            if (idParam > 0)
            {
                horario.Idparametro = idParam;
            }
            else
            {
                ModelState["DescParametro"].Errors.Add("Parâmetro " + horario.DescParametro + " não cadastrado!");
            }
        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            if (String.IsNullOrEmpty(filtro))
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
                IList<Horario> lHorario = new List<Horario>();
                IList<Horario> horarios = new List<Horario>();
                int codigo = -1;
                try { codigo = Int32.Parse(consulta); }
                catch (Exception) { codigo = -1; }
                if (codigo != -1)
                {
                    int id = bllHorario.GetIdPorCodigo(codigo, true).GetValueOrDefault();
                    Horario horario = bllHorario.LoadObject(id);
                    if (horario != null && horario.Id > 0 && horario.Ativo)
                    {
                        lHorario.Add(horario);
                    }
                }

                if (lHorario.Count == 0)
                {
                    if (horarios.Count > 0)
                        lHorario = horarios;
                    else
                        lHorario = bllHorario.GetAllListOrigem(false, false).Where(w => w.TipoHorario == 1 && w.Ativo).ToList();
                    if (!String.IsNullOrEmpty(consulta))
                    {
                        lHorario = lHorario.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                    }
                }
                ViewBag.Title = "Pesquisar Horário";
                return View(lHorario);
            }
            else
            {
                switch (filtro)
                {
                    case "1":
                        return BuscaHorariosNormais(consulta);
                    case "2":
                        return BuscaHorariosMoveis(consulta);
                    default:
                        return BuscaHorariosNormais(consulta);
                }
            }

        }

        private ActionResult BuscaHorariosNormais(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            IList<Horario> lHorario = new List<Horario>();
            IList<Horario> horarios = new List<Horario>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllHorario.GetIdPorCodigo(codigo, true).GetValueOrDefault();
                Horario horario = bllHorario.LoadObject(id);

                if (horario != null && horario.Id > 0 && horario.TipoHorario == 1 && horario.Ativo)
                    lHorario.Add(horario);
            }

            if (lHorario.Count == 0)
            {
                if (horarios.Count > 0)
                    lHorario = horarios;
                else
                    lHorario = bllHorario.GetHorarioNormalMovelList(1, true).Where(w => w.Ativo).ToList();

                if (!String.IsNullOrEmpty(consulta))
                {
                    lHorario = lHorario.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Horário Normal";
            return View(lHorario);
        }

        private ActionResult BuscaHorariosMoveis(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            IList<Horario> lHorario = new List<Horario>();
            IList<Horario> horarios = new List<Horario>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllHorario.GetIdPorCodigo(codigo, true).GetValueOrDefault();
                List<Horario> lHorarioMovel = new List<Horario>();
                Horario horario = bllHorario.LoadObject(id);

                if (horario != null && horario.Id > 0 && horario.TipoHorario == 2 && horario.IdHorarioDinamico == null)
                    lHorario.Add(horario);
            }

            if (lHorario.Count == 0)
            {
                if (horarios.Count > 0)
                    lHorario = horarios;
                else
                    lHorario = bllHorario.GetHorarioNormalMovelList(2, true);
                if (!String.IsNullOrEmpty(consulta))
                {
                    lHorario = lHorario.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Horário Móvel";
            return View(lHorario);
        }


        public static int BuscaIdHorario(string horario)
        {
            Horario h = new Horario();
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            string codigo = horario.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            int? idHorario = bllHorario.GetIdPorCodigo(cod, true);
            return idHorario.GetValueOrDefault();
        }
        #endregion

        [Authorize]
        public ActionResult CalculadoraHora()
        {
            return View();
        }

        [Authorize]
        public ActionResult ValidaJornada(string jornada)
        {
            int id = JornadaController.BuscaIdJornada(jornada);
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
            Jornada objJornada = bllJornada.LoadObject(id);
            if (objJornada == null || objJornada.Id == 0)
            {
                return Json(new { erro = true, msgErro = "Jornada " + jornada + " não cadastrada!", descJornada = "", idJornada = 0 });
            }
            else
            {
                return Json(new { erro = false, msgErro = "", descJornada = objJornada.Codigo + " | " + objJornada.horarios, idJornada = objJornada.Id });
            }
        }

        [PermissoesFiltro(Roles = "HorarioCadastrar")]
        public ActionResult CopiarHorario(int idHorario)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllhorario = new BLL.Horario(usr.ConnectionString, usr);
            BLL.Parametros bllParms = new BLL.Parametros(usr.ConnectionString, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
            Horario novoHorario = new Horario();
            Horario horarioantigo = bllhorario.LoadObject(idHorario);

            novoHorario = CopiaHorario(usr, bllhorario, bllParms, bllJornada, novoHorario, horarioantigo);

            return View("Cadastrar", novoHorario);
        }

        public static Horario CopiaHorario(UsuarioPontoWeb usr, BLL.Horario bllhorario, BLL.Parametros bllParms, BLL.Jornada bllJornada, Horario novoHorario, Horario horarioantigo)
        {
            AutoMapper.Mapper.CreateMap<Horario, Horario>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioDetalhe, HorarioDetalhe>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.Idhorario, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioPHExtra, HorarioPHExtra>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.Idhorario, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioAItinere, HorarioAItinere>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.Idhorario, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioRestricao, HorarioRestricao>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.IdHorario, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());

            var _horario = AutoMapper.Mapper.Map<Modelo.Horario, Modelo.Horario>(horarioantigo, novoHorario);
            _horario.Codigo = bllhorario.MaxCodigo();
            _horario.Ativo = true;
            _horario.Descricao = "";
            _horario.HorariosPHExtra.ToList().ForEach(f => { f.Id = 0; f.Idhorario = 0; f.Inchora = null; f.Incdata = null; f.Incusuario = null; f.Althora = null; f.Altdata = null; f.Altusuario = null; });

            _horario.LHorariosDetalhe = (_horario.HorariosDetalhe.Count() > 0) ? _horario.HorariosDetalhe.ToList() : new List<HorarioDetalhe>();
            _horario.LHorariosPHExtra = (_horario.HorariosPHExtra.Count() > 0) ? _horario.HorariosPHExtra.ToList() : new List<HorarioPHExtra>();

            if (_horario.HorariosAItinere.Count() > 0)
            {
                _horario.LHorariosAItinere = _horario.HorariosAItinere.ToList();
            }
            else
            {
                _horario.InicializaHorarioAItinere();
            }

            Modelo.Parametros param1 = bllParms.LoadObject(horarioantigo.Idparametro);
            _horario.DescParametro = param1.Codigo + " | " + param1.Descricao;

            if (horarioantigo.TipoHorario == 1)
            {
                List<Modelo.Jornada> jornadas = new List<Jornada>();
                jornadas = bllJornada.GetAllList(_horario.HorariosDetalhe.Select(s => s.Idjornada.GetValueOrDefault()).Distinct().ToList());
                _horario.HorariosDetalhe.ToList().ForEach((hdet) =>
                {
                    hdet.Acao = Acao.Incluir;
                    if (hdet.Idjornada.GetValueOrDefault() > 0)
                    {
                        Jornada jornada = jornadas.Where(w => w.Id == hdet.Idjornada.GetValueOrDefault()).FirstOrDefault();
                        hdet.DescJornada = (jornada.Codigo != 0) ? jornada.Codigo + " | " + jornada.horarios : string.Empty;
                    }
                    else
                    {
                        hdet.DescJornada = string.Empty;
                    }
                });
                _horario.DescJornadaCopiar = _horario.HorariosDetalhe.FirstOrDefault().DescJornada;
            }
            else if (horarioantigo.TipoHorario == 2)
            {
                List<Modelo.Jornada> jornadas = new List<Jornada>();
                jornadas = bllJornada.GetAllList(_horario.HorariosFlexiveis.Select(s => s.Idjornada.GetValueOrDefault()).Distinct().ToList());
                _horario.HorariosFlexiveis.ForEach((hflex) =>
                {
                    hflex.Acao = Acao.Incluir;
                    if (hflex.Idjornada.GetValueOrDefault() > 0)
                    {
                        Jornada jornada = jornadas.Where(w => w.Id == hflex.Idjornada.GetValueOrDefault()).FirstOrDefault();
                        hflex.DescJornada = (jornada.Codigo != 0) ? jornada.Codigo + " | " + jornada.horarios : string.Empty; 
                    }
                    else
                    {
                        hflex.DescJornada = string.Empty;
                    }
                });
                _horario.DescJornadaCopiar = _horario.HorariosFlexiveis.FirstOrDefault().DescJornada;
            }

            return _horario;// novoHorario;
        }

        private void ValidaClassificacao(Horario horario)
        {
            if (!String.IsNullOrEmpty(horario.DescClassificacao))
            {
                int idClassificacao = ClassificacaoController.BuscaIdClassificacao(horario.DescClassificacao);
                if (idClassificacao > 0)
                { horario.IdClassificacao = idClassificacao; }
                else
                { ModelState["DescClassificacao"].Errors.Add("Descrição Classificada " + horario.IdClassificacao + " não cadastrada!"); }
            }
            else
            {
                horario.IdClassificacao = null;
            }

        }        

        [PermissoesFiltro(Roles = "HorarioCadastrar")]
        [HttpPost]
        public ActionResult CopiarHorario(Horario horario)
        {

            return Salvar(horario);
        }

        //private static bool Comparar<Horario>(Horario Object1, Horario object2)
        //{
        //    //Get the type of the object
        //    Type type = typeof(Horario);

        //    //return false if any of the object is false
        //    if (Object1 == null || object2 == null)
        //        return false;

        //    //Loop through each properties inside class and get values for the property from both the objects and compare
        //    foreach (System.Reflection.PropertyInfo property in type.GetProperties())
        //    {
        //        if (property.Name != "ExtensionData" && property.Name != "IdHorarioOrigem" && property.Name != "Descricao" && property.Name != "Idhorario" && property.Name != "Acao" && property.Name != "InicioVigencia")
        //        {
        //            string Object1Value = string.Empty;
        //            string Object2Value = string.Empty;
        //            if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
        //                Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
        //            if (type.GetProperty(property.Name).GetValue(object2, null) != null)
        //                Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
        //            if (Object1Value.Trim() != Object2Value.Trim())
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        [HttpPost]
        public ActionResult AdicionaNovaRestricao(int index, int tipoRestricao, string restricao, int idHorario)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                int? idEmpresa = null;
                int? idContrato = null;
                string codigo = "";
                int codigoInt;
                try
                {
                    codigo = restricao.Split('|')[0];
                    int.TryParse(codigo, out codigoInt);
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Valor ({0}) informado para pesquisa inválido, valor esperado no formato \"0 | descricao\"", restricao));
                }

                if (tipoRestricao == 0)
                {
                    idEmpresa = new BLL.Empresa(usr.ConnectionString, usr).GetIdsPorCodigos(new List<int>() { codigoInt }).FirstOrDefault();
                    if (idEmpresa.GetValueOrDefault() == 0) throw new Exception(String.Format("Empresa {0} não encontrato", restricao));
                }
                else
                {
                    idContrato = new BLL.Contrato(usr.ConnectionString, usr).getId(codigoInt, null, null);
                    if (idContrato.GetValueOrDefault() == 0) throw new Exception(String.Format("Contrato {0} não encontrato", restricao));
                }

                var horario = new Modelo.Horario();
                horario.HorarioRestricao = new List<Modelo.HorarioRestricao>();
                horario.HorarioRestricao.Add(
                    new HorarioRestricao()
                    {
                        IdEmpresa = idEmpresa,
                        IdContrato = idContrato,
                        DescEmpresa = restricao,
                        IdHorario = idHorario
                    }
                ); ;
                string novo = RenderViewToString("AddNovaRestricao", horario);
                novo = novo.Replace("HorarioRestricao_0__", "HorarioRestricao_" + index + "__");
                novo = novo.Replace("HorarioRestricao[0].", "HorarioRestricao[" + index + "].");
                return Json ( new { Success = true, HTML = novo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}