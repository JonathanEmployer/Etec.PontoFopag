using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoLoteController : IControllerPontoWeb<LancamentoLote>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        #region Gets
        [PermissoesFiltro(Roles = "LancamentoLote")]
        public override ActionResult Grid()
        {
            return View(new Modelo.LancamentoLote());
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        protected override ActionResult GetPagina(int id)
        {
            ViewBag.Title = "Lançamento em Lote";
            ViewBag.Controller = "LancamentoLotea";
            return View("Cadastrar", GetDadosPagina(id, TipoLancamento.Folga));
        }
        #endregion

        #region Posts
        [PermissoesFiltro(Roles = "LancamentoLoteFolgaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteExcluir")]
        public override ActionResult Excluir(int id)
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
            LancamentoLote ll = bllLancamentoLote.LoadObject(id);
            BLL.LancamentoLoteFuncionario bllLancamentoLoteFuncionario = new BLL.LancamentoLoteFuncionario(_usr.ConnectionString, _usr);
            ll.LancamentoLoteFuncionarios = bllLancamentoLoteFuncionario.GetListWhere("and idLancamentoLote = " + ll.Id.ToString());
            ll.DataLancamentoAnt = ll.DataLancamento;

            switch (ll.TipoLancamento)
            {
                case (int)TipoLancamento.Folga:
                    DadosFolga(ll);
                    break;
                case (int)TipoLancamento.Afastamento:
                    DadosAfastamento(ll);
                    break;
                case (int)TipoLancamento.MudancaHorario:
                    DadosMudancaHorario(ll);
                    break;
                case (int)TipoLancamento.InclusaoBanco:
                    DadosInclusaoBanco(ll);
                    break;
                default:
                    break;
            }
            int qtdRegistrosExcluir = ll.LancamentoLoteFuncionarios.Count();
            string erro = string.Empty;
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllLancamentoLote.Salvar(Acao.Excluir, ll);
                ll.Acao = Acao.Excluir;
                if (erros.Count > 0)
                {
                    erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return new JsonResult { Data = new { success = false, Erro = erro } };
                }
                else
                {
                    RecalcularMarcacaoes(ll);
                    int naoExc = ll.LancamentoLoteFuncionarios.Where(w => w.Efetivado == false).Count();
                    if (naoExc > 0)
                    {
                        if (naoExc > 1)
                            erro = naoExc + " registros do(s) " + qtdRegistrosExcluir + " não foram excluídos.";
                        else erro = naoExc + " registro do(s) " + qtdRegistrosExcluir + " não foi excluído.";
                        erro += " Verifique os detalhes consultando o registro!";
                        return new JsonResult { Data = new { success = false, Erro = erro, Aviso = true } };
                    }
                    return new JsonResult { Data = new { success = true } };
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return new JsonResult { Data = new { success = false, Erro = ex.Message } };
            }
        }

        protected override ActionResult Salvar(LancamentoLote obj)
        {
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    string erro = String.Empty;
                    bool salvou = SalvarLote(obj, out erro);
                    if (!salvou)
                    {
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            SetaDadosPadrao(obj, _usr.ConnectionString, _usr);

            return View("Cadastrar", obj);
        }

        public bool SalvarLote(LancamentoLote obj, out string erro)
        {
            erro = String.Empty;
            Acao acao;
            BLL.LancamentoLoteFuncionario bllLancamentoLoteFunc = new BLL.LancamentoLoteFuncionario(_usr.ConnectionString, _usr);
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
            List<int> idsFuncionariosSelecionados = new List<int>();
            try
            {
                if (!String.IsNullOrEmpty(obj.idSelecionados))
                {
                    idsFuncionariosSelecionados = obj.idSelecionados.Split(',').ToList().Select(s => int.Parse(s)).ToList();
                }
                else
                {
                    throw new Exception("Nenhum funcionário informado para o lote");
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw new Exception("Erro ao carregar a lista de funcionários selecionados");
            }

            acao = new Acao();
            if (obj.Id == 0)
            {
                acao = Acao.Incluir;
                obj.LancamentoLoteFuncionarios = new List<LancamentoLoteFuncionario>();
                obj.Codigo = bllLancamentoLote.MaxCodigo();
                obj.DataLancamentoAnt = obj.DataLancamento;
            }
            else
            {
                acao = Acao.Alterar;
                obj.LancamentoLoteFuncionarios = bllLancamentoLoteFunc.GetListWhere("and idLancamentoLote = " + obj.Id.ToString());
                //Seta todo mundo para ser excluido e efetivado = true (e efetivado será validado novamente antes de salvar)
                obj.LancamentoLoteFuncionarios.ToList().ForEach(i => { i.Acao = Acao.Excluir; i.EfetivadoAnt = i.Efetivado; i.UltimaAcao = (int)Acao.Consultar; });
                //os que foram selecionados seta para alterar
                obj.LancamentoLoteFuncionarios.Where(w => idsFuncionariosSelecionados.Contains(w.IdFuncionario)).ToList().ForEach(f => { f.Acao = Acao.Alterar; f.EfetivadoAnt = f.Efetivado; });
                //Retiro dos selecionados os que já existiam
                idsFuncionariosSelecionados.RemoveAll(x => obj.LancamentoLoteFuncionarios.Select(s => s.IdFuncionario).Contains(x));
            }
            int codigo = bllLancamentoLoteFunc.MaxCodigo();
            //Adiciona os novos funcionarios selecionados
            foreach (int idFunc in idsFuncionariosSelecionados)
            {
                LancamentoLoteFuncionario fpf = new LancamentoLoteFuncionario();
                fpf.IdLancamentoLote = obj.Id;
                fpf.IdFuncionario = idFunc;
                fpf.Acao = Acao.Incluir;
                fpf.Codigo = codigo;
                codigo++;
                fpf.Efetivado = true;
                fpf.EfetivadoAnt = fpf.Efetivado;
                if (obj.LancamentoLoteFuncionarios == null)
                {
                    obj.LancamentoLoteFuncionarios = new List<LancamentoLoteFuncionario>();
                }
                obj.LancamentoLoteFuncionarios.Add(fpf);
            }

            List<int> idsInclusao = obj.LancamentoLoteFuncionarios.ToList().Where(i => i.Acao == Acao.Incluir).Select(p => p.IdFuncionario).ToList();
            if (idsInclusao.Count > 0)
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(_usr.ConnectionString, _usr);
                bllCartaoPonto.TratarMarcacoes(obj.DataLancamento, obj.DataLancamento, idsInclusao, bllFuncionario);
            }

            obj.Acao = acao;
            Dictionary<string, string> erros = bllLancamentoLote.Salvar(acao, obj);
            if (erros.Count > 0)
            {
                erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                return false;
            }
            else
            {
                RecalcularMarcacaoes(obj);
                return true;
            }
        }
        #endregion

        private void RecalcularMarcacaoes(LancamentoLote obj)
        {
            List<int> FuncRecalc = BLL.LancamentoLote.IdsFuncionariosRecalcularLote(obj);

            string descLote = "";
            string parametrosExibicao = "";
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase);
            switch (obj.TipoLancamento)
            {
                case (int)Modelo.TipoLancamento.Folga:
                    descLote = "Folga";
                    parametrosExibicao = String.Format("Lote de {0} código: {1}, descrição: {2}, funcionários: {3}", descLote, obj.Codigo, obj.Descricao, (obj.LancamentoLoteFuncionarios != null ? obj.LancamentoLoteFuncionarios.Count : 0));
                    hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/LancamentoLoteFolga/Grid");
                    hfm.CalculaMarcacaoLancamentoLoteDiaJob(String.Format("Recalculo de marcações por {0} de Lote de {1}", obj.AcaoDescricao, descLote), parametrosExibicao, obj);
                    break;
                case (int)Modelo.TipoLancamento.InclusaoBanco:
                    descLote = "Inclusão de Banco de Horas";
                    parametrosExibicao = String.Format("Lote de {0} código: {1}, descrição: {2}, funcionários: {3}", descLote, obj.Codigo, obj.Descricao, (obj.LancamentoLoteFuncionarios != null ? obj.LancamentoLoteFuncionarios.Count : 0));
                    hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/LancamentoLoteInclusaoBanco/Grid");
                    hfm.CalculaMarcacaoLancamentoLoteDiaJob(String.Format("Recalculo de marcações por {0} de Lote de {1}", obj.AcaoDescricao, descLote), parametrosExibicao, obj);
                    break;
                case (int)Modelo.TipoLancamento.BilhetesImp:
                    descLote = "Bilhetes";
                    parametrosExibicao = String.Format("Lote de {0} código: {1}, descrição: {2}, funcionários: {3}", descLote, obj.Codigo, obj.Descricao, (obj.LancamentoLoteFuncionarios != null ? obj.LancamentoLoteFuncionarios.Count : 0));
                    hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/LancamentoLoteBilhetesImp/Grid");
                    hfm.LancamentoLoteBilhetesProcessar(String.Format("Recalculo de marcações por {0} de Lote de {1}", obj.AcaoDescricao, descLote), parametrosExibicao, obj);
                    break;
                case (int)Modelo.TipoLancamento.Afastamento:
                    descLote = "Afastamento";
                    parametrosExibicao = String.Format("Lote de {0} código: {1}, descrição: {2}, funcionários: {3}", descLote, obj.Codigo, obj.Descricao, (obj.LancamentoLoteFuncionarios != null ? obj.LancamentoLoteFuncionarios.Count : 0));
                    hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/LancamentoLoteAfastamento/Grid");
                    obj.LancamentoLoteAfastamento.DataF = (obj.LancamentoLoteAfastamento.DataF == null ? DateTime.Now.AddMonths(1) : obj.LancamentoLoteAfastamento.DataF);
                    obj.LancamentoLoteAfastamento.DataF_Ant = (obj.LancamentoLoteAfastamento.DataF_Ant == null ? DateTime.Now.AddMonths(1) : obj.LancamentoLoteAfastamento.DataF_Ant);
                    hfm.RecalculaMarcacao(String.Format("Recalculo de marcações por {0} de Lote de {1}", obj.AcaoDescricao, descLote), parametrosExibicao, FuncRecalc, obj.LancamentoLoteAfastamento.DataI.GetValueOrDefault(), obj.LancamentoLoteAfastamento.DataF.GetValueOrDefault(), obj.LancamentoLoteAfastamento.DataI_Ant.GetValueOrDefault(), obj.LancamentoLoteAfastamento.DataF_Ant.GetValueOrDefault());
                    break;
                case (int)Modelo.TipoLancamento.MudancaHorario:
                    descLote = "Mudança de Horário";
                    DateTime dataIni = ((Modelo.LancamentoLote)obj).DataLancamento > ((Modelo.LancamentoLote)obj).DataLancamentoAnt ? ((Modelo.LancamentoLote)obj).DataLancamento : ((Modelo.LancamentoLote)obj).DataLancamentoAnt;
                    DateTime dataFin = DateTime.Now.AddMonths(1);
                    parametrosExibicao = String.Format("Lote de {0} código: {1}, descrição: {2}, funcionários: {3}", descLote, obj.Codigo, obj.Descricao, (obj.LancamentoLoteFuncionarios != null ? obj.LancamentoLoteFuncionarios.Count : 0));
                    hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/LancamentoLoteMudancaHorario/Grid");
                    hfm.RecalculaMarcacao(String.Format("Recalculo de marcações por {0} de Lote de {1}", obj.AcaoDescricao, descLote), parametrosExibicao, FuncRecalc, dataIni, dataFin);
                    break;
                default: break;
            }
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                BLL.Jornada bllJornada = new BLL.Jornada(_usr.ConnectionString, _usr);
                IList<Modelo.Proxy.pxyFuncionariosLote> dados = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(_usr.ConnectionString)).GetListagemFuncionariosLote(_usr, id);
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

        public static void SetaDadosPadrao(LancamentoLote obj, string conexao, UsuarioPontoWeb userpw)
        {
            if (obj.LancamentoLoteFuncionarios != null && obj.LancamentoLoteFuncionarios.Count() > 0)
            {
                obj.idSelecionados = String.Join(",", obj.LancamentoLoteFuncionarios.Select(x => x.IdFuncionario).ToArray());
            }

            obj.UtilizaControleContrato = userpw.UtilizaControleContratos;

        }

        public LancamentoLote GetPagina(int id, Modelo.TipoLancamento tpLancamento)
        {
            LancamentoLote ll = GetDadosPagina(id, tpLancamento);
            switch (tpLancamento)
            {
                case TipoLancamento.Folga:
                    DadosFolga(ll);
                    break;
                case TipoLancamento.Afastamento:
                    DadosAfastamento(ll);
                    break;
                case TipoLancamento.MudancaHorario:
                    DadosMudancaHorario(ll);
                    break;
                case TipoLancamento.InclusaoBanco:
                    DadosInclusaoBanco(ll);
                    break;
                case TipoLancamento.BilhetesImp:
                    DadosBilhetesImp(ll);
                    break;
                default:
                    break;
            }

            return (ll);
        }

        public LancamentoLote GetDadosPagina(int id, TipoLancamento tipoLancamento)
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
            LancamentoLote fp = new LancamentoLote();
            fp.TipoLancamento = (int)tipoLancamento;
            if (id == 0)
            {
                fp.Codigo = bllLancamentoLote.MaxCodigo();
                fp.DataLancamento = DateTime.Now;
            }
            else
            {
                fp = bllLancamentoLote.LoadObject(id);
                BLL.LancamentoLoteFuncionario bllLancamentoLoteFuncionario = new BLL.LancamentoLoteFuncionario(_usr.ConnectionString, _usr);
                fp.LancamentoLoteFuncionarios = bllLancamentoLoteFuncionario.GetListWhere("and idLancamentoLote = " + fp.Id.ToString());
                fp.DataLancamentoAnt = fp.DataLancamento;
            }
            SetaDadosPadrao(fp, _usr.ConnectionString, _usr);
            return fp;
        }

        private void DadosMudancaHorario(LancamentoLote ll)
        {
            if (ll.Id == 0)
            {
                ll.LancamentoLoteMudancaHorario = new LancamentoLoteMudancaHorario();
                ll.LancamentoLoteMudancaHorario.Tipohorario = 1;
            }
            else
            {
                BLL.LancamentoLoteMudancaHorario bllLancamentoLoteMudancaHorario = new BLL.LancamentoLoteMudancaHorario(_usr.ConnectionString, _usr);
                ll.LancamentoLoteMudancaHorario = bllLancamentoLoteMudancaHorario.LoadByIdLote(ll.Id);
                ll.LancamentoLoteMudancaHorario.Idhorario_ant = ll.LancamentoLoteMudancaHorario.Idhorario;
            }
        }

        private void DadosInclusaoBanco(LancamentoLote ll)
        {
            if (ll.Id == 0)
            {
                ll.LancamentoLoteInclusaoBanco = new LancamentoLoteInclusaoBanco();
                ll.LancamentoLoteInclusaoBanco.Tipocreditodebito = 0;
            }
            else
            {
                BLL.LancamentoLoteInclusaoBanco bllLancamentoLoteInclusaoBanco = new BLL.LancamentoLoteInclusaoBanco(_usr.ConnectionString, _usr);
                ll.LancamentoLoteInclusaoBanco = bllLancamentoLoteInclusaoBanco.LoadByIdLote(ll.Id);
                ll.LancamentoLoteInclusaoBanco.Credito_ant = ll.LancamentoLoteInclusaoBanco.Credito;
                ll.LancamentoLoteInclusaoBanco.Debito_ant = ll.LancamentoLoteInclusaoBanco.Debito;
            }
        }

        private void DadosBilhetesImp(LancamentoLote ll)
        {
            if (ll.Id == 0)
            {
                ll.LancamentoLoteBilhetesImp = new LancamentoLoteBilhetesImp();
                ll.LancamentoLoteBilhetesImp.Ocorrencia = "I";
                ll.LancamentoLoteBilhetesImp.Relogio = "MA";
            }
            else
            {
                BLL.LancamentoLoteBilhetesImp bllLancamentoLoteBilhetesImp = new BLL.LancamentoLoteBilhetesImp(_usr.ConnectionString, _usr);
                ll.LancamentoLoteBilhetesImp = bllLancamentoLoteBilhetesImp.LoadByIdLote(ll.Id);
                BLL.Justificativa bllJustificativa = new BLL.Justificativa(_usr.ConnectionString, _usr);
                if (ll.LancamentoLoteBilhetesImp.Idjustificativa > 0)
                {
                    Justificativa jus = bllJustificativa.LoadObject(ll.LancamentoLoteBilhetesImp.Idjustificativa);
                    if (jus != null && jus.Id > 0)
                    {
                        ll.LancamentoLoteBilhetesImp.DescJustificativa = jus.Codigo + " | " + jus.Descricao;
                    }
                }
                ll.LancamentoLoteBilhetesImp.Hora_Ant = ll.LancamentoLoteBilhetesImp.Hora;
            }
        }

        private void DadosFolga(LancamentoLote ll)
        {
        }

        protected override void ValidarForm(LancamentoLote obj)
        {

        }

        private void DadosAfastamento(LancamentoLote ll)
        {
            BLL.LancamentoLoteAfastamento bllLancamentoLoteAfastamento = new BLL.LancamentoLoteAfastamento(_usr.ConnectionString, _usr);
            if (ll.Id == 0)
            {
                ll.LancamentoLoteAfastamento = new LancamentoLoteAfastamento();
                ll.LancamentoLoteAfastamento.Codigo = bllLancamentoLoteAfastamento.MaxCodigo();
            }
            else
            {
                ll.LancamentoLoteAfastamento = bllLancamentoLoteAfastamento.LoadByIdLote(ll.Id);
                ll.LancamentoLoteAfastamento.AbonoDiurno_Ant = ll.LancamentoLoteAfastamento.AbonoDiurno;
                ll.LancamentoLoteAfastamento.AbonoNoturno_Ant = ll.LancamentoLoteAfastamento.AbonoNoturno;
                ll.LancamentoLoteAfastamento.BAbonado_Ant = ll.LancamentoLoteAfastamento.BAbonado;
                ll.LancamentoLoteAfastamento.BParcial_Ant = ll.LancamentoLoteAfastamento.BParcial;
                ll.LancamentoLoteAfastamento.BSemCalculo_Ant = ll.LancamentoLoteAfastamento.BSemCalculo;
                ll.LancamentoLoteAfastamento.BSuspensao_Ant = ll.LancamentoLoteAfastamento.BSuspensao;
                ll.LancamentoLoteAfastamento.DataI_Ant = ll.LancamentoLoteAfastamento.DataI;
                ll.LancamentoLoteAfastamento.DataF_Ant = ll.LancamentoLoteAfastamento.DataF;
                ll.LancamentoLoteAfastamento.OcorrenciaAnt = ll.LancamentoLoteAfastamento.Ocorrencia;
                ll.LancamentoLoteAfastamento.IdOcorrencia_Ant = ll.LancamentoLoteAfastamento.IdOcorrencia;
            }
        }
    }
}