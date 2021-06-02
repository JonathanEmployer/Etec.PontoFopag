using Modelo;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PontoWeb.Models;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PontoWeb.Models.Helpers;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Utils;
using BLL_N.JobManager.Hangfire;
using Modelo.Proxy;

namespace PontoWeb.Controllers
{
    public class HorarioDinamicoController : IControllerPontoWeb<HorarioDinamico>
    {
        private UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        public override ActionResult Grid()
        {
            return View(new Modelo.Proxy.PxyGridHorarioDinamico());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.HorarioDinamico bllHorario = new BLL.HorarioDinamico(_usr.ConnectionString, _usr);
                List<Modelo.Proxy.PxyGridHorarioDinamico> dados = bllHorario.GridHorarioDinamico(-1);
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

        [PermissoesFiltro(Roles = "HorarioDinamicoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "HorarioDinamicoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "HorarioDinamicoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar([JsonBinder]HorarioDinamico horario)
        {
            return Salvar(horario);
        }

        [PermissoesFiltro(Roles = "HorarioDinamicoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "HorarioDinamicoAlterar")]
        [HttpPost]
        public override ActionResult Alterar([JsonBinder]HorarioDinamico horario)
        {
            return Salvar(horario);
        }

        [PermissoesFiltro(Roles = "HorarioDinamicoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(_usr.ConnectionString, _usr);
            try
            {
                bllHorarioDinamico.ExcluirListAndAllChildren(new List<int>() { id });
                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                string erro = "";
                if (ex.Message.Contains("FK_mudancahorario_horario"))
                {
                    erro = "Antes de excluir o horário exclua a mudança de horário vinculada ao mesmo!";
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.Message.Contains("FK_funcionario_horario") || ex.Message.Contains("FK__funcionar"))
                {
                    erro = "Não é possivel excluir esse registro pois ele está relacionado à um funcionário";
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                else if (ex.Message.Contains("FK_marcacao_horario"))
                {
                    erro = "Não é possível excluir esse registro pois ele está relacionado a marcação de funcionário";
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                else erro = ex.Message;
                return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
            }
        }

        #region métodos auxiliares para exibir/salvar página
        protected override ActionResult GetPagina(int id)
        {
            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(_usr.ConnectionString, _usr);
            BLL.Jornada bllJornada = new BLL.Jornada(_usr.ConnectionString, _usr);
            BLL.Parametros bllParametros = new BLL.Parametros(_usr.ConnectionString, _usr);
            HorarioDinamico horario = new HorarioDinamico();

            if (id == 0)
            {
                horario.Codigo = bllHorarioDinamico.MaxCodigo();
                Parametros param = bllParametros.LoadPrimeiro();
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                horario.Idparametro = param.Id;
                horario.Limitemin = "03:00";
                horario.Limitemax = "03:00";
                horario.Tipoacumulo = 1;
                horario.LimiteHorasTrabalhadasDia = "10:00";
                horario.LimiteMinimoHorasAlmoco = "01:00";
                horario.LimiteInterjornada = "11:00";
                horario.Ativo = true;
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 0, QuantidadeExtra = "---:--" });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 1, QuantidadeExtra = "---:--" });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 2, QuantidadeExtra = "---:--" });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 3, QuantidadeExtra = "---:--" });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 4, QuantidadeExtra = "---:--" });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 5, QuantidadeExtra = "---:--" });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 6, QuantidadeExtra = "---:--", TipoAcumulo = -1 });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 7, QuantidadeExtra = "---:--", TipoAcumulo = -1 });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 8, QuantidadeExtra = "---:--", TipoAcumulo = -1 });
                horario.LHorariosDinamicosPHExtra.Add(new HorarioDinamicoPHExtra { Codigo = 9, QuantidadeExtra = "---:--", TipoAcumulo = -1 });
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
            }
            else
            {
                horario = bllHorarioDinamico.LoadObjectAllChildren(id);
                Parametros param = bllParametros.LoadObject(horario.Idparametro);
                horario.DescParametro = param.Codigo + " | " + param.Descricao;
                @ViewBag.InicioAdNoturno = param.InicioAdNoturno;
                @ViewBag.FimAdNoturno = param.FimAdNoturno;
                List<Modelo.Jornada> jornadas = bllJornada.GetAllList();

                foreach (var item in horario.LHorarioCiclo)
                {
                    Jornada jornada = jornadas.Where(w => w.Id == item.Idjornada).FirstOrDefault();
                    if (jornada != null)
                    {
                        item.DescJornada = jornada.Codigo + " | " + jornada.horarios;
                    }
                }

                if (horario.LimitesDDsrProporcionais != null)
                {
                    foreach (var item in horario.LimitesDDsrProporcionais)
                    {
                        item.Acao = Acao.Alterar;
                    }
                }

                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    string erro = "";
                    if (!validaFechamento(bllHorarioDinamico, horario.Id, out erro))
                    {
                        //@ViewBag.Consultar = 1;
                        @ViewBag.MensagemBloqueio = erro;
                        horario.PossuiFechamento = true;
                    }
                    else
                    {
                        if (bllHorarioDinamico.QuantidadeMarcacoesVinculadas(horario.Id) > 0)
                        {
                            @ViewBag.MensagemBloqueio = "Os ciclos e sequências do horário não poderão ser alterados, pois foram vinculados a funcionários.";
                            @ViewBag.BloquearCiclos = 1;
                        }
                    }
                }
                #endregion
            }

            if (horario.HorarioDinamicoRestricao == null)
            {
                horario.HorarioDinamicoRestricao = new List<HorarioDinamicoRestricao>();
            }

            return View("Cadastrar", horario);
        }

        private bool validaFechamento(BLL.HorarioDinamico bllHorarioDinamico, int id, out string erro)
        {
            List<FechamentoPonto> lFechamentoPonto = bllHorarioDinamico.FechamentoPontoHorario(new List<int> { id });
            if (lFechamentoPonto.Count > 0)
            {
                erro = "Operação não permitida, existe fechamento de ponto relacionado a esse registro. Fechamento: <br/> " + String.Join("<br/> ", lFechamentoPonto.Select(s => "Código: " + s.Codigo + " Data: " + s.DataFechamento.ToShortDateString()));
                return false;
            }
            erro = "";
            return true;
        }

        protected override ActionResult Salvar(HorarioDinamico obj)
        {
            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(_usr.ConnectionString, _usr);
            if (obj.PossuiFechamento) // Se possuir fechamento permite alterar apenas alguns campos
            {
                try
                {
                    Modelo.HorarioDinamico horarioAnt = bllHorarioDinamico.LoadObjectAllChildren(obj.Id);
                    if (horarioAnt.Ativo != obj.Ativo || horarioAnt.Descricao != obj.Descricao || (obj.HorarioDinamicoRestricao != null && obj.HorarioDinamicoRestricao.Where(w => w.Id == 0 || w.Excluir).Count() > 0))
                    {
                        horarioAnt.Ativo = obj.Ativo;
                        horarioAnt.Descricao = obj.Descricao;
                        horarioAnt.PossuiFechamento = obj.PossuiFechamento;
                        horarioAnt.HorarioDinamicoRestricao = obj.HorarioDinamicoRestricao;
                        obj = horarioAnt;
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllHorarioDinamico.Salvar(Modelo.Acao.Alterar, obj);
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
                try
                {
                    TryUpdateModel(obj);
                    ValidarForm(obj);
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
                        erros = bllHorarioDinamico.Salvar(acao, obj);

                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            List<PxyIdPeriodo> funcsPeriodo = bllHorarioDinamico.FuncionariosParaRecalculoObject(obj.Id);
                            UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                            HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW, "", "/HorarioDinamico/Grid");
                            string parametrosExibicao = String.Format("Horário dinâmico {0} | {1}, {2} funcionários.", obj.Codigo, obj.Descricao, funcsPeriodo.Count);
                            Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por horário", parametrosExibicao, funcsPeriodo);
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
            }
            return Json(new
            {
                Valid = ModelState.IsValid,
                Errors = GetErrorsFromModelState()
            });
        }

        private List<Modelo.HorarioDinamicoCiclo> AtualizarListaCiclos(List<Modelo.HorarioDinamicoCiclo> ListaCiclos, int idHorarioDinamico)
        {
            foreach (var ciclo in ListaCiclos)
            {
                ciclo.IdhorarioDinamico = idHorarioDinamico;
            }

            return ListaCiclos;
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

        [PermissoesFiltro(Roles = "HorarioDinamicoCadastrar")]
        public ActionResult CopiarHorario(int idHorario)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = usr.ConnectionString;
            BLL.HorarioDinamico bllhorarioDinamico = new BLL.HorarioDinamico(conn, usr);
            BLL.Parametros bllParms = new BLL.Parametros(conn, usr);
            BLL.Jornada bllJornada = new BLL.Jornada(conn, usr);
            HorarioDinamico novoHorarioDinamico = new HorarioDinamico();
            HorarioDinamico horarioantigo = bllhorarioDinamico.LoadObjectAllChildren(idHorario);

            novoHorarioDinamico = CopiaHorario(usr, bllhorarioDinamico, bllParms, bllJornada, novoHorarioDinamico, horarioantigo);

            return View("Cadastrar", novoHorarioDinamico);
        }
        public static HorarioDinamico CopiaHorario(UsuarioPontoWeb usr, BLL.HorarioDinamico bllhorarioDinamico, BLL.Parametros bllParms, BLL.Jornada bllJornada, HorarioDinamico novoHorarioDinamico, HorarioDinamico horariDinamicoAntigo)
        {
            AutoMapper.Mapper.CreateMap<HorarioDinamico, HorarioDinamico>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioDinamicoCiclo, HorarioDinamicoCiclo>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.IdhorarioDinamico, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioDinamicoCicloSequencia, HorarioDinamicoCicloSequencia>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.IdHorarioDinamicoCiclo, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioDinamicoPHExtra, HorarioDinamicoPHExtra>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.IdHorarioDinamico, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<HorarioDinamicoRestricao, HorarioDinamicoRestricao>().ForMember(x => x.Id, opt => opt.Ignore()).ForMember(x => x.IdHorarioDinamico, opt => opt.Ignore()).ForMember(x => x.Altdata, opt => opt.Ignore()).ForMember(x => x.Althora, opt => opt.Ignore()).ForMember(x => x.Altusuario, opt => opt.Ignore()).ForMember(x => x.Incdata, opt => opt.Ignore()).ForMember(x => x.Inchora, opt => opt.Ignore()).ForMember(x => x.Incusuario, opt => opt.Ignore()).ForMember(x => x.Codigo, opt => opt.Ignore());

            var _horarioDinamico = AutoMapper.Mapper.Map<Modelo.HorarioDinamico, Modelo.HorarioDinamico>(horariDinamicoAntigo, novoHorarioDinamico);
            _horarioDinamico.Codigo = bllhorarioDinamico.MaxCodigo();
            _horarioDinamico.Ativo = true;
            _horarioDinamico.Descricao = "";
            Modelo.Parametros param1 = bllParms.LoadObject(horariDinamicoAntigo.Idparametro);
            _horarioDinamico.DescParametro = param1.Codigo + " | " + param1.Descricao;
            List<Modelo.Jornada> jornadas = new List<Jornada>();
            jornadas = bllJornada.GetAllList(_horarioDinamico.LHorarioCiclo.Select(s => s.Idjornada).Distinct().ToList());
            _horarioDinamico.LHorarioCiclo.ForEach((hdin) =>
            {
                hdin.Acao = Acao.Incluir;
                if (hdin.Idjornada > 0)
                {
                    Jornada jornada = jornadas.Where(w => w.Id == hdin.Idjornada.GetHashCode()).FirstOrDefault();
                    hdin.DescJornada = (jornada.Codigo != 0) ? jornada.Codigo + " | " + jornada.horarios : string.Empty;
                }
                else
                {
                    hdin.DescJornada = string.Empty;
                }
            });
            return _horarioDinamico;// novoHorarioDinamico;
        }
        #endregion

        #region Tramentos e Validações

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
            BLL.Parametros bllParametro = new BLL.Parametros(_usr.ConnectionString, _usr);
            Parametros param = bllParametro.LoadObject(id);
            return Json(new { inicioAdNoturno = param.InicioAdNoturno, fimAdNoturno = param.FimAdNoturno });
        }

        private void ValidaHorario(HorarioDinamico horario)
        {
            VerificaParametro(horario);
        }

        private int VerificaJornada(HorarioDinamico horario)
        {
            foreach (var ciclo in horario.LHorarioCiclo)
            {
                if (!String.IsNullOrEmpty(ciclo.DescJornada))
                {
                    int idJornada = JornadaController.BuscaIdJornada(ciclo.DescJornada);
                    if (idJornada > 0)
                    {
                        ciclo.Idjornada = idJornada;
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "Jornada " + ciclo.DescJornada + " não cadastrada!");
                    }
                }
            }

            return 0;
        }

        private void VerificaParametro(HorarioDinamico horario)
        {
            int idParam = ParametroController.BuscaIdParametro(horario.DescParametro);
            if (idParam > 0)
                horario.Idparametro = idParam;
            else
                ModelState.AddModelError("DescParametro", "Parâmetro " + horario.DescParametro + " não cadastrado!");
        }
        private void ValidaClassificacao(HorarioDinamico horario)
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
        protected override void ValidarForm(HorarioDinamico obj)
        {
            ValidaHorario(obj);
            ValidaClassificacao(obj);
            VerificaJornada(obj);

            //valida Campos Parametro
            ValidaPercentualParametros(obj);
        }
        #endregion

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(_usr.ConnectionString, _usr);
            IList<HorarioDinamico> lHorarioDinamico = new List<HorarioDinamico>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                HorarioDinamico horarioDinamico = bllHorarioDinamico.LoadObjectByCodigo(codigo, true);
                if (horarioDinamico != null && horarioDinamico.Id > 0 && horarioDinamico.Ativo)
                {
                    lHorarioDinamico.Add(horarioDinamico);
                }
            }

            if (lHorarioDinamico.Count == 0)
            {
                if (!String.IsNullOrEmpty(consulta))
                {
                    lHorarioDinamico = bllHorarioDinamico.GetPorDescricao(consulta, true);
                }
                else
                {
                    lHorarioDinamico = bllHorarioDinamico.GetAllList(true);
                }
                if (lHorarioDinamico.Count > 0)
                {
                    lHorarioDinamico = lHorarioDinamico.Where(w => w.Ativo).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Horário Dinamico";
            return View(lHorarioDinamico);
        }

        public static int BuscaIdHorario(string horario)
        {
            HorarioDinamico h = new HorarioDinamico();
            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
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
            int? idHorario = bllHorarioDinamico.getId(cod, null, null);
            return idHorario.GetValueOrDefault();
        }

        public ActionResult Selecionar(string horario, int? idHorarioDinamico)
        {
            int id = 0;
            if (!String.IsNullOrEmpty(horario))
            {
                id = BuscaIdHorario(horario);
            }
            else
            {
                id = idHorarioDinamico.GetValueOrDefault();
            }


            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(_usr.ConnectionString, _usr);
            Modelo.HorarioDinamico hd = bllHorarioDinamico.LoadObjectAllChildren(id);
            if (hd != null && hd.Id > 0)
            {
                if (hd.LHorarioCiclo != null && hd.LHorarioCiclo.Count > 0)
                {
                    BLL.Jornada bllJornada = new BLL.Jornada(_usr.ConnectionString, _usr);
                    List<Modelo.Jornada> jornadas = bllJornada.GetAllList(hd.LHorarioCiclo.Select(s => s.Idjornada).ToList());
                    if (jornadas != null && jornadas.Count() > 0)
                    {
                        hd.LHorarioCiclo.ToList().ForEach(f => f.Jornada = jornadas.Where(w => w.Id == f.Idjornada).FirstOrDefault());
                    }
                }
            }


            return View("HorarioDinamicoSelecionar", hd);
        }
        #endregion

        public ActionResult AdicionaNovaRestricao(int index, int tipoRestricao, string restricao, int idHorario)
        {
            try
            {
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
                    idEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr).GetIdsPorCodigos(new List<int>() { codigoInt }).FirstOrDefault();
                    if (idEmpresa.GetValueOrDefault() == 0) throw new Exception(String.Format("Empresa {0} não encontrato", restricao));
                }
                else
                {
                    idContrato = new BLL.Contrato(_usr.ConnectionString, _usr).getId(codigoInt, null, null);
                    if (idContrato.GetValueOrDefault() == 0) throw new Exception(String.Format("Contrato {0} não encontrato", restricao));
                }

                var horario = new Modelo.HorarioDinamico();
                horario.HorarioDinamicoRestricao = new List<Modelo.HorarioDinamicoRestricao>();
                horario.HorarioDinamicoRestricao.Add(
                    new HorarioDinamicoRestricao()
                    {
                        IdEmpresa = idEmpresa,
                        IdContrato = idContrato,
                        DescEmpresa = restricao,
                        IdHorarioDinamico = idHorario
                    }
                ); ;
                string novo = RenderViewToString("AdicionaNovaRestricao", horario);
                novo = novo.Replace("HorarioDinamicoRestricao_0__", "HorarioDinamicoRestricao_" + index + "__");
                novo = novo.Replace("HorarioDinamicoRestricao[0].", "HorarioDinamicoRestricao[" + index + "].");
                return Json(new { Success = true, HTML = novo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidaPercentualParametros(HorarioDinamico horario)
        {
            for (int i = 0; i < horario.LHorariosDinamicosPHExtra.Count(); i++)
            {
                var item = horario.LHorariosDinamicosPHExtra[i];
                if (item.MarcaPercentualExtraBool)
                {
                    if (item.PercentualExtra == 0)
                    {
                        ModelState.AddModelError(("LHorariosDinamicosPHExtra[" + i + "].PercentualExtra"), "O campo deve ter um valor maior que 0!");
                        //ModelState["LHorariosDinamicosPHExtra[" + i + "].PercentualExtra"].Errors.Add("O campo deve ter um valor maior que 0!");
                    }
                }
            }
        }

    }
}