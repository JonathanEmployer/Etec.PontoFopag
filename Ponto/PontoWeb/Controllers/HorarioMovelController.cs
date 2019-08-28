using Modelo;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PontoWeb.Controllers.BLLWeb;
using BLL_N.JobManager.Hangfire;

namespace PontoWeb.Controllers
{
    public class HorarioMovelController : IControllerPontoWeb<Horario>
    {
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
            return View(new Modelo.Proxy.PxyGridHorarioFlexivel());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Horario bllHorario = new BLL.Horario(usr.ConnectionString, usr);
                List<Modelo.Proxy.PxyGridHorarioFlexivel> dados = bllHorario.GetAllGrid(2);
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

        [PermissoesFiltro(Roles = "HorarioMovelConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "HorarioMovelConsultar")]
        public ActionResult ConsultarExt(int id, string ctrl, string acao)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(conn, usr);
            BLL.Parametros bllParametros = new BLL.Parametros(conn, usr);
            List<SelectListItem> listaDiaSemanaIniFolga;
            ViewBag.Consultar = 1;
            ViewBag.RetController = ctrl;
            ViewBag.RetAcao = acao;


            Horario horario = new Horario();
            horario = bllHorario.LoadObject(id);
            if (id == 0)
            {
                bllHorario.InicializaHorarioMovel(ref horario);
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
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();
                horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                horario.Limitemin = "03:00";
                horario.Limitemax = "03:00";
                horario.Tipoacumulo = 1;

                listaDiaSemanaIniFolga = GerarListaDiaSEmanaIniFolga(horario);
            }
            else
            {
                Parametros param = bllParametros.LoadObject(horario.Idparametro);
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                List<Jornada> jornadas = bllJornada.GetAllList(horario.HorariosFlexiveis.Select(s => s.Idjornada.GetValueOrDefault()).Distinct().ToList());
                foreach (var item in horario.HorariosFlexiveis.Where(w => w.Idjornada.GetValueOrDefault() > 0))
                {
                    Jornada jornada = jornadas.Where(w => w.Id == item.Idjornada).FirstOrDefault();
                    item.DescJornada = jornada.Codigo + " | " + jornada.horarios;
                    item.Acao = Modelo.Acao.Alterar;
                }
                horario.LHorariosDetalhe = horario.HorariosFlexiveis.ToList();
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();

                if (horario.HorariosAItinere[0] == null)
                    horario.InicializaHorarioAItinere();
                else
                    horario.LHorariosAItinere = horario.HorariosAItinere.ToList();

                if (horario.HorariosFlexiveis.Count > 0)
                {
                    horario.DescJornadaCopiar = horario.HorariosFlexiveis[0].DescJornada;
                }

                listaDiaSemanaIniFolga = GerarListaDiaSEmanaIniFolga(horario);
            }

            ViewBag.ListaDiaSemanaIniFolga = listaDiaSemanaIniFolga;

            return PartialView("Cadastrar", horario);

        }


        [PermissoesFiltro(Roles = "HorarioMovelCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "HorarioMovelCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar([JsonBinder]Horario horario)
        {
            return Salvar(horario);
        }

        [PermissoesFiltro(Roles = "HorarioMovelAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "HorarioMovelAlterar")]
        [HttpPost]
        public override ActionResult Alterar([JsonBinder]Horario horario)
        {
            return Salvar(horario);
        }

        [PermissoesFiltro(Roles = "HorarioMovelExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);
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
                else
                {
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase, "", "", "/Horario/Grid");
                    string parametrosExibicao = String.Format("Horário {0} | {1}.", horario.Codigo, horario.Descricao);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por exclusão de horário flexível", parametrosExibicao,4, horario.Id, horario.DataInicial.Value, horario.DataFinal.Value);
                    return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                string erro = "";
                if (ex.Message.Contains("FK_mudancahorario_horario"))
                    erro = "Antes de excluir o horário exclua a mudança de horário vinculada ao mesmo!";
                else erro = ex.Message;
                return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AdicionarHorariosFlexiveis([JsonBinder]Horario horario)
        {
            try
            {
                //Horario horarioObj = JsonConvert.DeserializeObject<Horario>(horario,
                //   new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                //Horario objHorario = DeserializaObjHorario(form);
                TryUpdateModel(horario);
                if (ModelState.IsValid)
                {
                    ChamaRotinaGerar(horario);
                }
                if (!ModelState.IsValid)
                {
                    string erro = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    ModelState.AddModelError("CustomErrorHorarioFlexivel", erro);
                }
                return PartialView("_GridHorariosFlexiveis", horario);
            }
            //catch (JsonSerializationException e) // Ocorre ao passar um valor alfanumerico numa propriedade do tio int
            catch (Exception e) // Ocorre ao passar um valor alfanumerico numa propriedade do tio int
            {
                throw e;
                // Mensagem - Favor informar um valor numérico no campo Número  
            }
        }

        /// <summary>
        /// Chama a rotina que gera os horários automáticamente
        /// </summary>
        /// <param name="tipo">
        /// 1 = 5/1 | 2 = 12/36 | 3 = 24/48 |
        /// 4 = 6/2 | 5 = Turno Completo
        /// </param>
        private void ChamaRotinaGerar(Horario objHorario)
        {
            Modelo.HorarioDetalheMovel objHorarioDMovel = AuxGerar(objHorario);
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.HorarioDetalhe bllHorarioDetalhe = new BLL.HorarioDetalhe(usr.ConnectionString, usr);
            Dictionary<string, string> cwErro = new Dictionary<string, string>();
            switch (objHorario.controleHorariosFlexiveis)
            {
                case "5/1":
                    cwErro = bllHorarioDetalhe.Gerar5_1(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                case "12/36":
                    cwErro = bllHorarioDetalhe.Gerar12_36(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                case "24/48":
                    cwErro = bllHorarioDetalhe.Gerar24_48(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                case "6/1":
                    cwErro = bllHorarioDetalhe.Gerar6_1(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                case "6/2":
                    cwErro = bllHorarioDetalhe.Gerar6_2(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                case "TCompleto":
                    cwErro = bllHorarioDetalhe.GerarTurnoCompleto(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                case "12/60":
                    cwErro = bllHorarioDetalhe.Gerar12_60(objHorarioDMovel, objHorario.DataInicial, objHorario.DataFinal, objHorario);
                    break;
                default:
                    DateTime data = new DateTime();
                    if (DateTime.TryParse(objHorario.controleHorariosFlexiveis, out data))
                    {
                        bllHorarioDetalhe.ExcluirHorariosMovel(objHorario.HorariosFlexiveis, data);
                    }
                    break;
            }
            if (cwErro.Count() > 0)
            {
                string erro = string.Join(";", cwErro.Select(x => x.Key + "=" + x.Value).ToArray());
                erro = erro.Replace("txtDataInicial=", "").Replace("txtDataFinal=", "");
                ModelState.AddModelError("CustomErrorHorarioFlexivel", erro);
            }
        }

        private Modelo.HorarioDetalheMovel AuxGerar(Horario objHorario)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
            Modelo.HorarioDetalheMovel objHorarioDMovel = new Modelo.HorarioDetalheMovel();
            objHorarioDMovel.Idjornada = VerificaJornada(objHorario);
            if (objHorarioDMovel.Idjornada == 0)
            {
                ModelState.AddModelError("CustomErrorHorarioFlexivel", "Para Gerar os Horários é necessário informar o campo Jornada");
            }
            else
            {

                Jornada jornada = bllJornada.LoadObject(objHorarioDMovel.Idjornada.GetValueOrDefault());
                objHorario.DescJornadaCopiar = jornada.Codigo + " | " + jornada.Descricao;
                objHorario.Horariodescricao_1 = jornada.Entrada_1;
                objHorario.Horariodescricao_2 = jornada.Entrada_2;
                objHorario.Horariodescricao_3 = jornada.Entrada_3;
                objHorario.Horariodescricao_4 = jornada.Entrada_4;
                objHorario.Horariodescricaosai_1 = jornada.Saida_1;
                objHorario.Horariodescricaosai_2 = jornada.Saida_2;
                objHorario.Horariodescricaosai_3 = jornada.Saida_3;
                objHorario.Horariodescricaosai_4 = jornada.Saida_4;
            }
            objHorarioDMovel.Entrada_1 = objHorario.Horariodescricao_1;
            objHorarioDMovel.Entrada_2 = objHorario.Horariodescricao_2;
            objHorarioDMovel.Entrada_3 = objHorario.Horariodescricao_3;
            objHorarioDMovel.Entrada_4 = objHorario.Horariodescricao_4;

            objHorarioDMovel.Saida_1 = objHorario.Horariodescricaosai_1;
            objHorarioDMovel.Saida_2 = objHorario.Horariodescricaosai_2;
            objHorarioDMovel.Saida_3 = objHorario.Horariodescricaosai_3;
            objHorarioDMovel.Saida_4 = objHorario.Horariodescricaosai_4;

            objHorarioDMovel.Totaltrabalhadadiurna = objHorario.CargaDiurna;
            objHorarioDMovel.Totaltrabalhadanoturna = objHorario.CargaNoturna;
            objHorarioDMovel.Cargahorariamista = objHorario.CargaMista;
            objHorarioDMovel.Marcacargahorariamista = objHorario.Marcacargahorariamista;

            objHorarioDMovel.Intervaloautomatico = objHorario.Intervaloautomatico;
            objHorarioDMovel.Preassinaladas1 = objHorario.Preassinaladas1;
            objHorarioDMovel.Preassinaladas2 = objHorario.Preassinaladas2;
            objHorarioDMovel.Preassinaladas3 = objHorario.Preassinaladas3;
            return objHorarioDMovel;
        }

        #region métodos auxiliares para exibir/salvar página
        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(conn, usr);
            BLL.Parametros bllParametros = new BLL.Parametros(conn, usr);
            Horario horario = new Horario();
            horario = bllHorario.LoadObject(id);
            if (id == 0)
            {
                bllHorario.InicializaHorarioMovel(ref horario);
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
                horario.LHorariosPHExtra = horario.HorariosPHExtra.ToList();
                horario.LHorariosAItinere = horario.HorariosAItinere.ToList();
                horario.Limitemin = "03:00";
                horario.Limitemax = "03:00";
                horario.Tipoacumulo = 1;
                horario.LimiteHorasTrabalhadasDia = "10:00";
                horario.LimiteMinimoHorasAlmoco = "01:00";
                horario.LimiteInterjornada = "11:00";
                horario.Ativo = true;
            }
            else
            {
                Parametros param = bllParametros.LoadObject(horario.Idparametro);
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                bllHorario.AtribuiHDHorario(horario);
                List<Modelo.Jornada> jornadas = bllJornada.GetAllList();
                foreach (var item in horario.HorariosFlexiveis)
                {
                    Jornada jornada = jornadas.Where(w => w.Id == item.Idjornada).FirstOrDefault();
                    if (jornada != null)
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

                horario.LHorariosDetalhe = horario.HorariosFlexiveis.ToList();
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

                if (horario.HorariosFlexiveis.Count > 0)
                {
                    horario.DescJornadaCopiar = horario.HorariosFlexiveis[0].DescJornada;
                }

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
            #region Lista dia da semana para considerar primeira folga da escala

            var listaDiaSemanaIniFolga = GerarListaDiaSEmanaIniFolga(horario);

            ViewBag.ListaDiaSemanaIniFolga = listaDiaSemanaIniFolga;
            horario.HorarioRestricao = horario.HorarioRestricao ?? new List<HorarioRestricao>();
            #endregion
            return View("Cadastrar", horario);
        }

        /// <summary>
        /// Retorna uma lista com as folgas
        /// </summary>
        /// <param name="horario"></param>
        /// <returns>List<SelectListItem> ListaDiaSemanaIniFolga</returns>
        private List<SelectListItem> GerarListaDiaSEmanaIniFolga(Horario horario)
        {
            List<SelectListItem> listaDiaSemanaIniFolga = new List<SelectListItem>();
            int diaPrimeiraFolga = 0;
            if (horario.LHorariosDetalhe != null && horario.LHorariosDetalhe.Count > 0)
            {
                HorarioDetalhe hd1Folga = horario.LHorariosDetalhe.Where(w => w.Flagfolga == 1).FirstOrDefault();
                if (hd1Folga != null)
                {
                    diaPrimeiraFolga = horario.LHorariosDetalhe.Where(w => w.Flagfolga == 1).FirstOrDefault().Dia;
                }

            }

            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "",              Value = "" });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Segunda",       Value = "Seg",  Selected = (diaPrimeiraFolga == 1) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Terça",         Value = "Ter",  Selected = (diaPrimeiraFolga == 2) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Quarta",        Value = "Qua",  Selected = (diaPrimeiraFolga == 3) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Quinta",        Value = "Qui",  Selected = (diaPrimeiraFolga == 4) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Sexta",         Value = "Sex",  Selected = (diaPrimeiraFolga == 5) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Sábado",        Value = "Sab",  Selected = (diaPrimeiraFolga == 6) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Domingo",       Value = "Dom",  Selected = (diaPrimeiraFolga == 7) });
            listaDiaSemanaIniFolga.Add(new SelectListItem { Text = "Não se aplica", Value = "N",    Selected = (diaPrimeiraFolga == 0) });

            return listaDiaSemanaIniFolga;
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

        protected override ActionResult Salvar(Horario obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);
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
                    }
                    return Json(new { redirectTo = Url.Action("Grid") });
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                    return Json(new
                    {
                        Valid = ModelState.IsValid,
                        Errors = GetErrorsFromModelState()
                    });
                }
            }
            else
            {
                TryUpdateModel(obj);
                ValidarForm(obj);
                //ValidaHorario(obj);
                if (ModelState.IsValid)
                {
                    try
                    {
                        Acao acao = new Acao();
                        if (obj.Id == 0)
                            acao = Acao.Incluir;
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
                            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase, "", "", "/HorarioMovel/Grid");
                            string parametrosExibicao = String.Format("Horário {0} | {1}.", obj.Codigo, obj.Descricao);
                            Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por exclusão de horário flexível", parametrosExibicao, 4, obj.Id, obj.DataInicial.Value, obj.DataFinal.Value);
                            return Json(new { redirectTo = Url.Action("Grid") });
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                        return Json(new
                        {
                            Valid = ModelState.IsValid,
                            Errors = GetErrorsFromModelState()
                        });
                    }
                }
                return Json(new
                {
                    Valid = ModelState.IsValid,
                    Errors = GetErrorsFromModelState()
                }); 
            }
        }
        private static Horario DeserializaObjHorario(string form)
        {
            form = form.Replace("\"TipoAcumulo\":null", "\"TipoAcumulo\":\"0\"");
            Horario objHorario = JsonConvert.DeserializeObject<Horario>(form, new IsoDateTimeConverter());
            return objHorario;
        }

        public Dictionary<string, object> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors;
                }
            }

            return errors;
        }
        #endregion

        [PermissoesFiltro(Roles = "HorarioMovelCadastrar")]
        public ActionResult CopiarHorario(int idHorario)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.Horario bllhorario = new BLL.Horario(conn, usr);
            BLL.Parametros bllParms = new BLL.Parametros(conn, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(conn, usr);
            Horario novoHorario = new Horario();
            Horario horarioantigo = bllhorario.LoadObject(idHorario);

            novoHorario = HorarioController.CopiaHorario(usr, bllhorario, bllParms, bllJornada, novoHorario, horarioantigo);

            var listaDiaSemanaIniFolga = GerarListaDiaSEmanaIniFolga(horarioantigo);
            ViewBag.ListaDiaSemanaIniFolga = listaDiaSemanaIniFolga;

            return View("Cadastrar", novoHorario);
        }

        #region Tramentos e Validações
        private static void TrataDados(Horario horario)
        {
            foreach (HorarioDetalhe hd in horario.HorariosDetalhe)
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
            }

            foreach (HorarioPHExtra HPHExtra in horario.LHorariosPHExtra)
            {
                HPHExtra.Quantidadeextra = String.IsNullOrEmpty(HPHExtra.Quantidadeextra) ? "--:--" : HPHExtra.Quantidadeextra;
            }
            horario.HorariosDetalhe = horario.HorariosDetalhe.ToArray();
            horario.HorariosPHExtra = horario.LHorariosPHExtra.ToArray();
            if (horario.LHorariosAItinere == null || horario.LHorariosAItinere.Count() == 0)
            {
                horario.InicializaHorarioAItinere();
            }
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            Dictionary<string, string> erroTratado = new Dictionary<string, string>();
            erroTratado = erros.Where(x => x.Key.Equals("cbIdentificacao")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbIdentificacao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                ModelState.AddModelError("DescParametro", string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloSab")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloSab")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[6].TipoAcumulo";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("DescClassificacao")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("DescClassificacao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "DescClassificacao";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("QtdHEPreClassificadas")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("QtdHEPreClassificadas")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "QtdHEPreClassificadas";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloDom")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloDom")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[7].TipoAcumulo";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloFer")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloFer")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[8].TipoAcumulo";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            erroTratado = erros.Where(x => x.Key.Equals("cbTipoAcumuloFol")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("cbTipoAcumuloFol")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTratado.Count > 0)
            {
                string campoErro = "LHorariosPHExtra[9].TipoAcumulo";
                ModelState.AddModelError(campoErro, string.Join(";", erroTratado.Select(x => x.Value).ToArray()));
            }

            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                if (erro.Contains("txtEntrada_1="))
                    erro = erro.Replace("txtEntrada_1=", "");
                ModelState.AddModelError("CustomError", erro);
            }
        }

        public ActionResult DadosParametros(string parametro)
        {
            int id = ParametroController.BuscaIdParametro(parametro);
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Parametros bllParametro = new BLL.Parametros(usr.ConnectionString, usr);
            Parametros param = bllParametro.LoadObject(id);
            return Json(new { inicioAdNoturno = param.InicioAdNoturno, fimAdNoturno = param.FimAdNoturno });
        }

        private void ValidaHorario(Horario horario)
        {
            VerificaParametro(horario);
        }

        private int VerificaJornada(Horario horario)
        {
            if (!String.IsNullOrEmpty(horario.DescJornadaCopiar))
            {
                int idJornada = JornadaController.BuscaIdJornada(horario.DescJornadaCopiar);
                if (idJornada > 0)
                {
                    return idJornada;
                }
                else
                {
                    ModelState.AddModelError("Jornada", "Jornada " + horario.DescJornadaCopiar + " não cadastrada!");
                }
            }
            return 0;
        }

        private void VerificaParametro(Horario horario)
        {
            int idParam = ParametroController.BuscaIdParametro(horario.DescParametro);
            if (idParam > 0)
                horario.Idparametro = idParam;
            else
                ModelState.AddModelError("DescParametro", "Parâmetro " + horario.DescParametro + " não cadastrado!");
        }
        private void ValidaClassificacao(Horario horario)
        {
            if (!String.IsNullOrEmpty(horario.DescClassificacao))
            {
                int idClassificacao = ClassificacaoController.BuscaIdClassificacao(horario.DescClassificacao);
                if (idClassificacao > 0)
                { horario.IdClassificacao = idClassificacao; }
                else
                { ModelState.AddModelError("DescClassificacao", "Descrição Classificação " + horario.DescClassificacao + " não cadastrado!"); }
            }
            else
            {
                horario.IdClassificacao = null;
            }

        }
        protected override void ValidarForm(Horario obj)
        {
            ValidaHorario(obj);
            ValidaClassificacao(obj);
        }
        #endregion

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
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
                    lHorario = bllHorario.GetHorarioNormalMovelList(2, true).Where(w => w.Ativo).ToList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lHorario = lHorario.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Horário";
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
    }
}