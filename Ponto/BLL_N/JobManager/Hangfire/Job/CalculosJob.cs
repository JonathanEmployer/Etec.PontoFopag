using cwkPontoMT.Integracao;
using DAL.SQL;
using Hangfire;
using Hangfire.Server;
using Hangfire.States;
using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL_N.JobManager.Hangfire.Job
{
    public class CalculosJob : JobBase
    {
        public CalculosJob() : base()
        {

        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, List<PxyIdPeriodo> funcsPeriodo, bool considerarInativos = false)
        {
            try
            {

                var grupo = funcsPeriodo.GroupBy(u => new { u.InicioPeriodo, u.FimPeriodo }).Select(s => new { DataInicial = s.Key.InicioPeriodo, DataFinal = s.Key.FimPeriodo, Dados = s.ToList() }).ToList();
                foreach (var item in grupo)
                {
                    RecalculaMarcacao(context, jobReport, db, usuario, item.Dados.Select(s => s.Id).ToList(), (DateTime)item.DataInicial, (DateTime)item.DataFinal, considerarInativos);
                }

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, List<PxyFuncionariosRecalcular> funcsRecalculo, bool considerarInativos = false)
        {
            try
            {
                List<PxyFuncionariosRecalcular> FuncsSemDataIni = funcsRecalculo.Where(w => w.DataInicio == null).ToList();
                if (FuncsSemDataIni.Count > 0)
                {
                    string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
                    DAL.SQL.Marcacao dalMarcacao = new DAL.SQL.Marcacao(new DataBase(conexao));
                    DataTable dt = dalMarcacao.GetDataPrimeiraMarcacaoFuncionarioConsiderandoFechamentos(funcsRecalculo.Select(s => s.IdFuncionario).ToList());
                    dt.AsEnumerable().ToList().ForEach(f => FuncsSemDataIni.Where(w => w.IdFuncionario == f.Field<int>("idfuncionario")).ToList().ForEach(fi => fi.DataInicio = f.Field<DateTime>("data")));
                }

                List<PxyFuncionariosRecalcular> FuncsSemDataFim = funcsRecalculo.Where(w => w.DataFim == null).ToList();
                if (FuncsSemDataFim.Count > 0)
                {
                    string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
                    DAL.SQL.Marcacao dalMarcacao = new DAL.SQL.Marcacao(new DataBase(conexao));
                    DataTable dt = dalMarcacao.GetDataUltimaMarcacaoFuncionario(funcsRecalculo.Select(s => s.IdFuncionario).ToList());
                    dt.AsEnumerable().ToList().ForEach(f => FuncsSemDataFim.Where(w => w.IdFuncionario == f.Field<int>("idfuncionario")).ToList().ForEach(fi => fi.DataFim = f.Field<DateTime>("data")));
                }

                foreach (var grupo in funcsRecalculo.Where(w => w.DataFim != null && w.DataInicio <= w.DataFim).GroupBy(g => new {
                    g.DataInicio,
                    g.DataFim
                }))
                {
                    RecalculaMarcacao(context, jobReport, db, usuario, grupo.Select(s => s.IdFuncionario).ToList(), grupo.Key.DataInicio.GetValueOrDefault(), grupo.Key.DataFim.GetValueOrDefault(), considerarInativos);
                }

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, List<PxyFuncionariosRecalcular> funcsRecalculo)
        {
            try
            {
                List<PxyFuncionariosRecalcular> FuncsSemDataFim = funcsRecalculo.Where(w => w.DataFim == null).ToList();
                if (FuncsSemDataFim.Count > 0)
                {
                    string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
                    DAL.SQL.Marcacao dalMarcacao = new DAL.SQL.Marcacao(new DataBase(conexao));
                    DataTable dt = dalMarcacao.GetDataUltimaMarcacaoFuncionario(funcsRecalculo.Select(s => s.IdFuncionario).ToList());
                    dt.AsEnumerable().ToList().ForEach(f => FuncsSemDataFim.Where(w => w.IdFuncionario == f.Field<int>("idfuncionario")).ToList().ForEach(fi => fi.DataFim = f.Field<DateTime>("data")));
                }

                foreach (var grupo in funcsRecalculo.Where(w => w.DataFim != null && w.DataInicio <= w.DataFim).GroupBy(g => new {
                    g.DataInicio,
                    g.DataFim
                }))
                {
                    RecalculaMarcacao(context, jobReport, db, usuario, grupo.Select(s => s.IdFuncionario).ToList(), grupo.Key.DataInicio.GetValueOrDefault(), grupo.Key.DataFim.GetValueOrDefault());
                }

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, int? pTipo, int pIdsTipo, DateTime dataInicial, DateTime dataFinal)
        {
            RecalculaMarcacao(context, jobReport, db, usuario, pTipo, new List<int> { pIdsTipo }, dataInicial, dataFinal);
        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, int? pTipo, List<int> pIdsTipo, DateTime dataInicial, DateTime dataFinal)
        {
            string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
            Modelo.Cw_Usuario userPF = new Modelo.Cw_Usuario();
            userPF.Login = usuario;
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conexao, userPF);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(pTipo, pIdsTipo, false, false);
            RecalculaMarcacao(context, jobReport, db, usuario, idsFuncionarios, dataInicial, dataFinal, null, null);
        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, bool considerarInativos = false)
        {
            RecalculaMarcacao(context, jobReport, db, usuario, idsFuncionario, dataInicial, dataFinal, null, null, considerarInativos);
        }

        public void RecalculaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, DateTime? dataInicial_Ant, DateTime? dataFinal_Ant, bool considerarInativos = false)
        {
            SetParametersBase(context, jobReport, db, usuario);

            try
            {
                List<Tuple<DateTime, DateTime>> periodos = BLL.cwkFuncoes.ComparePeriod(new Tuple<DateTime, DateTime>(dataInicial, dataFinal), new Tuple<DateTime?, DateTime?>(dataInicial_Ant, dataFinal_Ant));

                foreach (var p in periodos)
                {
                    CalculaMarcacao(idsFuncionario, p.Item1, p.Item2, considerarInativos);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        private void CalculaMarcacao(List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, bool considerarInativos = false)
        {
            dataInicial = dataInicial.Date;
            dataFinal = dataFinal.Date;
            int qtdSteps = 7;

            pb.setaMinMaxPB(0, qtdSteps);

            Modelo.ProgressBar pbInt = new Modelo.ProgressBar();
            pbInt.incrementaPB = new Modelo.IncrementaProgressBar(SetaValorProgressBarVazio);
            pbInt.setaMensagem = new Modelo.SetaMensagem(SetaMensagemVazio);
            pbInt.setaMinMaxPB = new Modelo.SetaMinMaxProgressBar(SetaMinMaxProgressBarVazio);
            pbInt.setaValorPB = new Modelo.SetaValorProgressBar(SetaValorProgressBarVazio);
            pb.incrementaPBCMensagem(1, "Carregando dados para calculo");

            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(userPF.ConnectionString, userPF);
            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(userPF.ConnectionString, userPF);
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(userPF.ConnectionString, userPF);
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(userPF.ConnectionString, userPF);
            CalculaMarcacao dalCalculaMarcacao = new CalculaMarcacao(new DataBase(userPF.ConnectionString));
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(userPF.ConnectionString, userPF);
            BLL.JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(userPF.ConnectionString, userPF);
            dalCalculaMarcacao.UsuarioLogado = userPF;

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            List<Task> threads = new List<Task>();
            Hashtable jornadaAlternativaList = new Hashtable();
            List<Modelo.FechamentoBHD> fechamentoBHDList = new List<Modelo.FechamentoBHD>();
            Hashtable ocorrenciaList = new Hashtable();
            List<Modelo.Compensacao> compensacaoList = new List<Modelo.Compensacao>();
            DataTable dtMarcacoes = new DataTable();
            List<Modelo.BilhetesImp> tratamentomarcacaoList = new List<Modelo.BilhetesImp>();
            List<PxyJornadaSubstituirCalculo> pxyJornadaSubstituirCalculosList = new List<PxyJornadaSubstituirCalculo>();


            threads.Add(new Task(() => { 
                jornadaAlternativaList = bllJornadaAlternativa.GetHashIdObjeto(dataInicial, dataFinal, 2, idsFuncionario); 
            }, token));

            threads.Add(new Task(() => {
                fechamentoBHDList = bllFechamentoBHD.getPorPeriodo(dataInicial, dataFinal, 2, idsFuncionario);
            }, token));

            threads.Add(new Task(() => {
                ocorrenciaList = bllOcorrencia.GetHashIdDescricao();
            }, token));

            threads.Add(new Task(() => {
                compensacaoList = bllCompensacao.GetPeriodo(dataInicial, dataFinal, 2, idsFuncionario);
            }, token));

            threads.Add(new Task(() => {
                dtMarcacoes = (DataTable)ExecuteMethodThredCancellation(() => dalCalculaMarcacao.GetMarcacoesCalculo(idsFuncionario, dataInicial, dataFinal, considerarInativos, false));
                List<int> idsHorario = dtMarcacoes.AsEnumerable().Where(r => !r.IsNull("idhorario")).Select(s => s.Field<int>("idhorario")).Distinct().ToList();
                bool gerouRegistroPonto = false;
                if (idsHorario.Count > 0)
                {
                    BLL.CalculoMarcacoes.PontoPorExcecao pontoPorExcecao = new BLL.CalculoMarcacoes.PontoPorExcecao(userPF.ConnectionString, userPF);
                    pb.setaMensagem("Gerando ponto por exceção");
                    List<Modelo.RegistroPonto> registroPontos = pontoPorExcecao.CriarRegistroPontoPorExcecao(new List<int>(), idsHorario);
                    if (registroPontos.Any())
                    {
                        gerouRegistroPonto = true;
                        //Aguarda a importação dos registros para continar
                        BLL.RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(userPF.ConnectionString, userPF);
                        Dictionary<int, string> situacaoRegistro = new Dictionary<int, string>();
                        pb.setaMensagem("Aguardando geração do ponto por excessão...");
                        do
                        {
                            Thread.Sleep(1000);
                            situacaoRegistro = bllRegistroPonto.GetSituacaoByLote(registroPontos.Select(s => s.Lote).LastOrDefault());
                        } while (situacaoRegistro.Any() && situacaoRegistro.FirstOrDefault().Value != "C");
                    }
                }
                BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(userPF.ConnectionString, userPF);
                if (bllHorarioDinamico.GerarHorariosDetalhesAPartirMarcacoes(dtMarcacoes) || gerouRegistroPonto)
                {
                    dtMarcacoes = (DataTable)ExecuteMethodThredCancellation(() => dalCalculaMarcacao.GetMarcacoesCalculo(idsFuncionario, dataInicial, dataFinal, considerarInativos, false));
                }

                tratamentomarcacaoList = (List<Modelo.BilhetesImp>)ExecuteMethodThredCancellation(() => bllBilhetesImp.GetImportadosPeriodo(idsFuncionario, dataInicial, dataFinal, false));
            }, token));

            threads.Add(new Task(() => {
                pxyJornadaSubstituirCalculosList = (List<PxyJornadaSubstituirCalculo>)ExecuteMethodThredCancellation(() => bllJornadaSubstituir.GetPxyJornadaSubstituirCalculo(dataInicial, dataFinal, idsFuncionario));
            }, token));

            threads.ForEach(f => f.Start());

            while (threads.Count != threads.Where(w => w.IsCompleted || w.IsCanceled || w.IsFaulted).Count())
            {
                try
                {
                    VerificaCancelamentoJob(context);
                }
                catch (OperationCanceledException)
                {
                    source.Cancel();
                    throw;
                }
                Thread.Sleep(250);
            }
            

            pb.setaMensagem("Carregando banco de horas");
            List<int> idsBH = dtMarcacoes.AsEnumerable().Where(r => !r.IsNull("idbancohoras")).Select(s => s.Field<int>("idbancohoras")).Distinct().ToList();
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(userPF.ConnectionString, userPF);
            Hashtable bancoHorasList = (Hashtable)ExecuteMethodThredCancellation(() => bllBancoHoras.GetHashIdObjeto(dataInicial, dataFinal, idsBH));

            pb.incrementaPBCMensagem(1, "Separando dados para calculo");
            var MarcsFuncs = dtMarcacoes.AsEnumerable().GroupBy(row => new
            {
                idfuncionario = row.Field<int>("idfuncionario"),
                dscodigo = row.Field<string>("dscodigo"),
                nomeFunc = row.Field<string>("nomeFuncionario"),
            }).OrderBy(o => o.Key.nomeFunc);

            ConcurrentBag<LoteMarcacaoProcessar> lote = new ConcurrentBag<LoteMarcacaoProcessar>();
            foreach (var group in MarcsFuncs)
            {
                LoteMarcacaoProcessar l = new LoteMarcacaoProcessar();
                l.IdFuncionario = group.Key.idfuncionario;
                l.NomeFuncionario = group.Key.nomeFunc;
                l.DtMarcacoes = group.CopyToDataTable();
                l.Marcacoes = new List<Modelo.Marcacao>();
                l.Bilhetes = new List<Modelo.BilhetesImp>();
                lote.Add(l);
            }

            int qtdLote = lote.Count();
            int stepCalc = 0;
            pb.incrementaPBCMensagem(1, String.Format("Calculando dados (Quantidade {0}/{1})", stepCalc, qtdLote));
            Parallel.ForEach(lote, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (currentFile) =>
            {
                Interlocked.Increment(ref stepCalc);
                pb.incrementaPBCMensagem(0, String.Format("Calculando dados (Quantidade {0}/{1})", stepCalc, qtdLote));
                BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(currentFile.DtMarcacoes, tratamentomarcacaoList, bancoHorasList, jornadaAlternativaList, fechamentoBHDList, ocorrenciaList, compensacaoList, pxyJornadaSubstituirCalculosList, pbInt, userPF.ConnectionString, userPF);
                LoteMarcacaoProcessar lt = bllCalculaMarcacao.CalcularMarcacoes();
                currentFile.Marcacoes = lt.Marcacoes;
                currentFile.Bilhetes = lt.Bilhetes;
            });

            string msgSalvando = "Salvando dados calculados";
            String.Format(msgSalvando, valorcorrenteprogress, maxprogress);
            pb.incrementaPBCMensagem(1, msgSalvando);
            BLL.CalculaMarcacao bllCalculaMarcacaoAnt = new BLL.CalculaMarcacao(dtMarcacoes, tratamentomarcacaoList, bancoHorasList, jornadaAlternativaList, fechamentoBHDList, ocorrenciaList, compensacaoList, pxyJornadaSubstituirCalculosList, pb, userPF.ConnectionString, userPF);
            bllCalculaMarcacaoAnt.SalvarMarcacoesCalculadas(lote.ToList(), pb, msgSalvando);

            pb.incrementaPBCMensagem(1, "Pré classificando horas");
            bllCalculaMarcacaoAnt.PreClassificarHorasExtras(lote.ToList());

            qtdLote = lote.Count();
            stepCalc = 1;
            pb.incrementaPBCMensagem(1, String.Format("Calculando DSR (Quantidade {0}/{1})", stepCalc++, qtdLote));
            Parallel.ForEach(lote, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (currentFile) =>
            {
                if (currentFile.DtMarcacoes.Rows.Count > 0)
                {
                    Interlocked.Increment(ref stepCalc);
                    pb.incrementaPBCMensagem(0, String.Format("Calculando DSR (Quantidade {0}/{1})", stepCalc, qtdLote));
                    BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(currentFile.DtMarcacoes, tratamentomarcacaoList, bancoHorasList, jornadaAlternativaList, fechamentoBHDList, ocorrenciaList, compensacaoList, pxyJornadaSubstituirCalculosList, pbInt, userPF.ConnectionString, userPF);
                    List<Modelo.Marcacao> lt = bllCalculaMarcacao.CalculaDSR(false, false);
                    currentFile.Marcacoes = lt;
                    currentFile.Bilhetes = new List<Modelo.BilhetesImp>();
                }
            });


            pb.incrementaPBCMensagem(1, "Salvando DSR");
            bllCalculaMarcacaoAnt.SalvarMarcacoesCalculadas(lote.ToList(), pb, msgSalvando);
        }

        public void RecalculaExclusaoMudHorario(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.MudancaHorario objMudancaHorario)
        {
            userPF = new Modelo.UsuarioPontoWeb() { Login = usuario, ConnectionString = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString };
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
            DateTime? dataFinal = bllMarcacao.GetUltimaDataFuncionario(objMudancaHorario.Idfuncionario);
            if (dataFinal != null)
            {
                RecalculaMarcacao(context, jobReport, db, usuario, 2, objMudancaHorario.Idfuncionario, objMudancaHorario.Data.GetValueOrDefault(), dataFinal.Value);
            }
        }

        public void AtualizaMarcacaoJornadaAlternativa(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.JornadaAlternativa jornada)
        {
            DateTime dataInicial = jornada.DataInicial.Value;
            DateTime dataFinal = jornada.DataFinal.Value;

            //CRNC - Data 09/01/2010 (Tirado o else)
            if (jornada.DiasJA.Count > 0)
            {
                DateTime dataIDJA = jornada.DiasJA.Min(d => d.DataCompensada).Value;
                DateTime dataFDJA = jornada.DiasJA.Max(d => d.DataCompensada).Value;

                dataInicial = (dataIDJA < dataInicial ? dataIDJA : dataInicial);
                dataFinal = (dataFDJA > dataFinal ? dataFDJA : dataFinal);
            }
            RecalculaMarcacao(context, jobReport, db, usuario, jornada.Tipo, jornada.Identificacao, dataInicial, dataFinal);

            // AtualizaDadosAnterior
            if (jornada.Acao == Acao.Alterar)
            {
                dataInicial = jornada.DataInicial_Ant.Value;
                dataFinal = jornada.DataFinal_Ant.Value;

                //CRNC - Data 09/01/2010 (Tirado o else)
                if (jornada.DiasJA.Count > 0)
                {
                    DateTime dataIDJA = jornada.DiasJA.Min(d => d.DataCompensada).Value;
                    DateTime dataFDJA = jornada.DiasJA.Max(d => d.DataCompensada).Value;

                    dataInicial = (dataIDJA < dataInicial ? dataIDJA : dataInicial);
                    dataFinal = (dataFDJA > dataFinal ? dataFDJA : dataFinal);
                } 

                if ((jornada.Identificacao_Ant > 0 && jornada.Tipo_Ant != jornada.Tipo || jornada.Identificacao_Ant != jornada.Identificacao) || (jornada.DataInicial != jornada.DataInicial_Ant || jornada.DataFinal != jornada.DataFinal_Ant))
                    RecalculaMarcacao(context, jobReport, db, usuario, jornada.Tipo_Ant, jornada.Identificacao_Ant, dataInicial, dataFinal);
            }
        }


        public void AtualizarMarcacoesFeriado(PerformContext context, JobControl jobReport, string db, string usuario, Acao pAcao, Modelo.Feriado pFeriado)
        {
            try
            {
                int? tipoRecalculo = 0;
                int idRecalculo = 0;
                bool bAlterado = (pFeriado.TipoFeriado_Ant != pFeriado.TipoFeriado) || (pFeriado.Data_Ant != pFeriado.Data)
                        || (pFeriado.IdDepartamento_Ant != pFeriado.IdDepartamento) || (pFeriado.IdEmpresa_Ant != pFeriado.IdEmpresa)
                        || (pFeriado.IdsFeriadosFuncionariosSelecionados != pFeriado.IdsFeriadosFuncionariosSelecionados_Ant)
                        || (pFeriado.Parcial != pFeriado.ParcialAnt)
                        || (pFeriado.HoraInicio != pFeriado.HoraInicioAnt)
                        || (pFeriado.HoraFim != pFeriado.HoraFimAnt);
                #region AtualizaDadosAnterior
                if (pAcao == Modelo.Acao.Alterar) //Funcionário
                {
                    //Caso algum dado tenha sido alterado
                    if (bAlterado)
                    {
                        if (pFeriado.TipoFeriado_Ant != 3) // Funcionário
                        {
                            if (pFeriado.TipoFeriado_Ant == 0)//Geral
                            {
                                tipoRecalculo = null;
                            }
                            else if (pFeriado.TipoFeriado_Ant == 1) //Empresa
                            {
                                tipoRecalculo = 0;
                                idRecalculo = pFeriado.IdEmpresa_Ant;
                            }
                            else if (pFeriado.TipoFeriado_Ant == 2) //Departamento
                            {
                                tipoRecalculo = 1;
                                idRecalculo = pFeriado.IdDepartamento_Ant;
                            }
                            RecalculaMarcacao(context, jobReport, db, usuario, tipoRecalculo, idRecalculo, (DateTime)pFeriado.Data_Ant, (DateTime)pFeriado.Data_Ant);
                        }
                    }
                }
                #endregion

                #region AtualizaDadosAlterados
                if (bAlterado || pAcao == Modelo.Acao.Excluir || pAcao == Modelo.Acao.Incluir)
                {
                    if (pFeriado.TipoFeriado != 3)//Funcionário
                    {
                        if (pFeriado.TipoFeriado == 0)//Geral
                        {
                            tipoRecalculo = null;
                        }
                        else if (pFeriado.TipoFeriado == 1) //Empresa
                        {
                            tipoRecalculo = 0;
                            idRecalculo = pFeriado.IdEmpresa;
                        }
                        else if (pFeriado.TipoFeriado == 2) //Departamento
                        {
                            tipoRecalculo = 1;
                            idRecalculo = pFeriado.IdDepartamento;
                        }

                        RecalculaMarcacao(context, jobReport, db, usuario, tipoRecalculo, idRecalculo, (DateTime)pFeriado.Data, (DateTime)pFeriado.Data);
                    }
                }
                #endregion

                List<int> funcsRecalc = new List<int>();
                #region recalculo quando feriado foi/é por funcionário
                if (pFeriado.Data != pFeriado.Data_Ant && pAcao == Acao.Alterar) // Se mudou a data do feriado
                {
                    // Recalcula os antigos na data antiga
                    if (!String.IsNullOrEmpty(pFeriado.IdsFeriadosFuncionariosSelecionados_Ant))
                    {
                        funcsRecalc = pFeriado.IdsFeriadosFuncionariosSelecionados_Ant.Split(',').Select(Int32.Parse).ToList();
                        RecalculaMarcacao(context, jobReport, db, usuario, funcsRecalc, (DateTime)pFeriado.Data_Ant, (DateTime)pFeriado.Data_Ant);
                    }

                    // Recalcula os funcionarios que ficaram na data nova
                    if (!String.IsNullOrEmpty(pFeriado.IdsFeriadosFuncionariosSelecionados))
                    {
                        funcsRecalc = pFeriado.IdsFeriadosFuncionariosSelecionados.Split(',').Select(Int32.Parse).ToList();
                        RecalculaMarcacao(context, jobReport, db, usuario, funcsRecalc, (DateTime)pFeriado.Data, (DateTime)pFeriado.Data);
                    }
                }
                else
                {
                    if (pAcao == Acao.Excluir) // Se estiver excluindo o feriado, não preciso olhar os anteriores, apenas recalculo o que esta como "novo"
                    {
                        pFeriado.IdsFeriadosFuncionariosSelecionados_Ant = "";
                    }
                    List<int> idsFunc_ant = new List<int>();
                    List<int> idsFunc = new List<int>();
                    if (!String.IsNullOrEmpty(pFeriado.IdsFeriadosFuncionariosSelecionados_Ant))
                    {
                        idsFunc_ant = pFeriado.IdsFeriadosFuncionariosSelecionados_Ant.Split(',').Select(Int32.Parse).ToList();
                    }
                    if (!String.IsNullOrEmpty(pFeriado.IdsFeriadosFuncionariosSelecionados))
                    {
                        idsFunc = pFeriado.IdsFeriadosFuncionariosSelecionados.Split(',').Select(Int32.Parse).ToList();
                    }
                    // Pega os Funcionarios atuais e tira os antigos para criar uma lista apenas com os novos funcionarios
                    List<int> FuncsAdd = new List<int>();
                    FuncsAdd = idsFunc.Except(idsFunc_ant).ToList();
                    // Pega os Funcionarios antigos e tira os novos para criar uma lista apenas os funcionarios removidos
                    List<int> FuncsRem = new List<int>();
                    FuncsRem = idsFunc_ant.Except(idsFunc).ToList();
                    // Junta os novos e os removidos (descartou os que não sobreram alteração)
                    funcsRecalc = FuncsAdd;
                    funcsRecalc.AddRange(FuncsRem);
                    if (bAlterado)
                    {
                        funcsRecalc.AddRange(idsFunc);
                        funcsRecalc = funcsRecalc.Distinct().ToList();
                    }
                    if (funcsRecalc.Count > 0)
                    {
                        RecalculaMarcacao(context, jobReport, db, usuario, funcsRecalc, (DateTime)pFeriado.Data, (DateTime)pFeriado.Data);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void AtualizaMarcacoesCompensacao(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Acao pAcao, Modelo.Compensacao pCompensacao)
        {
            SetParametersBase(context, jobReport, db, usuario);
            pb.setaMensagem("Carregando dados para recalcular as marcações...");

            if (pCompensacao.Tipo != pCompensacao.Tipo_Ant || pCompensacao.Identificacao != pCompensacao.Identificacao_Ant
                || pCompensacao.Periodoinicial != pCompensacao.Periodoinicial_Ant || pCompensacao.Periodofinal != pCompensacao.Periodofinal_Ant)
            {
                #region AtualizaDadosAnterior
                if (pAcao != Modelo.Acao.Incluir)
                {
                    pb.setaMensagem("Recalculando marcações do período anterior...");
                    RecalculaMarcacao(context, jobReport, db, usuario, pCompensacao.Tipo_Ant, pCompensacao.Identificacao_Ant, (DateTime)pCompensacao.Periodoinicial_Ant, (DateTime)pCompensacao.Periodofinal_Ant);
                }
                #endregion
            }

            #region AtualizaDadosAlterados

            pb.setaValorPB(-1);
            pb.setaMensagem("Recalculando as marcações do período atual...");
            RecalculaMarcacao(context, jobReport, db, usuario, pCompensacao.Tipo, pCompensacao.Identificacao, (DateTime)pCompensacao.Periodoinicial, (DateTime)pCompensacao.Periodofinal);
            #endregion
        }

        /// <summary>
        /// Método que desfaz a compensação
        /// </summary>
        /// <param name="pIdCompensacao">ID da compensaçao</param>
        //PAM - 15/12/2009
        public void DesfazCompensacao(PerformContext context, JobControl jobReport, string db, string usuario, int pIdCompensacao)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(userPF.ConnectionString, userPF);
            bllCompensacao.ObjProgressBar = this.pb;

            Modelo.Compensacao objCompensacao = bllCompensacao.LoadObject(pIdCompensacao);
            bllCompensacao.RetornaHorasParaFalta(objCompensacao);
            bllMarcacao.SetaIdCompensadoNulo(objCompensacao.Id);
            DateTime datai, dataf;
            if (objCompensacao.Diacompensarinicial.HasValue && objCompensacao.Diacompensarfinal.HasValue)
            {
                datai = objCompensacao.Diacompensarinicial.Value;
                dataf = objCompensacao.Diacompensarfinal.Value;
            }
            else
            {
                datai = new DateTime();
                dataf = new DateTime();
            }
            if (objCompensacao.DiasC.Count > 0)
            {
                foreach (Modelo.DiasCompensacao dia in objCompensacao.DiasC)
                {
                    if (dia.Datacompensada.HasValue && (dia.Datacompensada < datai || datai == new DateTime()))
                        datai = dia.Datacompensada.Value;
                    if (dia.Datacompensada > dataf && dia.Datacompensada.HasValue)
                        dataf = dia.Datacompensada.Value;
                }
            }
            RecalculaMarcacao(context, jobReport, db, usuario, objCompensacao.Tipo, objCompensacao.Identificacao, datai, dataf);
        }

        /// <summary>
        /// Esse método faz o fechamento da compensação
        /// </summary>
        /// <param name="pIdCompensacao">ID da compensacao</param>
        //PAM - 11/12/2009
        public bool FechaCompensacao(PerformContext context, JobControl jobReport, string db, string usuario, int pIdCompensacao)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(userPF.ConnectionString, userPF);
            bllCompensacao.ObjProgressBar = pb;
            //Desfaz a compensação para manter a consistência dos dados, caso haja um fechamento anterior
            DesfazCompensacao(context, jobReport, db, usuario, pIdCompensacao);
            DataTable totalCompensado = bllCompensacao.GetTotalCompensado(pIdCompensacao);
            List<string> auxLog = bllCompensacao.RateioHorasCompensadas(totalCompensado, pIdCompensacao);
            if (auxLog.Count > 0)
            {
                StringBuilder str = new StringBuilder("O fechamento foi realizado com sucesso.\nAlguns funcionários já possuem fechamento de outra compensação.\nVerifique.");
                auxLog.ForEach(f => str.Append(f));
                string caminho = CaminhoArquivo();
                caminho = Path.Combine(caminho, "Compensacao");
                if (!Directory.Exists(caminho))
                    Directory.CreateDirectory(caminho);
                caminho += String.Format(@"\Compensacao{0}_{1}.txt", pIdCompensacao, DateTime.Now.ToString("ddMMyyyyHHmmss"));
                System.IO.File.WriteAllText(caminho, str.ToString());
                JobControlManager.UpdateFileDownload(context, caminho);
            }
            else
            {
            }
            return true;
        }

        public void CalculaAfastamento(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Afastamento pAfastamento)
        {
            int tipoRecalculo = 0, idRecalculo = 0;
            pAfastamento.Dataf = (pAfastamento.Dataf == null ? DateTime.Now.AddMonths(1) : pAfastamento.Dataf.Value);
            pAfastamento.Dataf_Ant = (pAfastamento.Dataf_Ant == null ? DateTime.Now.AddMonths(1) : pAfastamento.Dataf_Ant.Value);

            #region AtualizaDadosAnterior
            if (pAfastamento.Acao != Modelo.Acao.Incluir)
            {
                if (pAfastamento.Tipo_Ant == 0)//Funcionario
                {
                    tipoRecalculo = 2;
                    idRecalculo = pAfastamento.IdFuncionario_Ant;
                }
                else if (pAfastamento.Tipo_Ant == 2) //Empresa
                {
                    tipoRecalculo = 0;
                    idRecalculo = pAfastamento.IdEmpresa_Ant;
                }
                else if (pAfastamento.Tipo_Ant == 1) //Departamento
                {
                    tipoRecalculo = 1;
                    idRecalculo = pAfastamento.IdDepartamento_Ant;
                }
                else if (pAfastamento.Tipo_Ant == 3) //Contrato
                {
                    tipoRecalculo = 6;
                    idRecalculo = pAfastamento.IdContrato_Ant.GetValueOrDefault();
                }

                RecalculaMarcacao(context, jobReport, db, usuario, tipoRecalculo, idRecalculo, (DateTime)pAfastamento.Datai_Ant, (DateTime)pAfastamento.Dataf_Ant);
            }
            #endregion

            #region AtualizaDadosAlterados
            if (pAfastamento.Tipo == 0)//Funcionario
            {
                tipoRecalculo = 2;
                idRecalculo = pAfastamento.IdFuncionario;
            }
            else if (pAfastamento.Tipo == 2) //Empresa
            {
                tipoRecalculo = 0;
                idRecalculo = pAfastamento.IdEmpresa;
            }
            else if (pAfastamento.Tipo == 1) //Departamento
            {
                tipoRecalculo = 1;
                idRecalculo = pAfastamento.IdDepartamento;
            }
            else if (pAfastamento.Tipo == 3) //Contrato
            {
                tipoRecalculo = 6;
                idRecalculo = pAfastamento.IdContrato.GetValueOrDefault();
            }

            if (pAfastamento.Acao == Modelo.Acao.Incluir)
            {
                SetParametersBase(context, jobReport, db, usuario);
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
                bllMarcacao.InsereMarcacoesNaoExistentes(tipoRecalculo, idRecalculo, pAfastamento.Datai.Value, pAfastamento.Dataf.Value, pb, false);
            }

            RecalculaMarcacao(context, jobReport, db, usuario, tipoRecalculo, idRecalculo, (DateTime)pAfastamento.Datai, (DateTime)pAfastamento.Dataf);
            #endregion
        }

        public void CalculaBancoHoras(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Acao pAcao, Modelo.BancoHoras pBancoHoras)
        {
            SetParametersBase(context, jobReport, db, usuario);
            pb.setaMensagem("Carregando dados para recalcular as marcações...");

            if (pBancoHoras.Tipo != pBancoHoras.Tipo_Ant || pBancoHoras.Identificacao != pBancoHoras.Identificacao_Ant
                || pBancoHoras.DataInicial != pBancoHoras.DataInicial_Ant || pBancoHoras.DataFinal != pBancoHoras.DataFinal_Ant)
            {
                #region AtualizaDadosAnterior
                if (pAcao != Modelo.Acao.Incluir)
                {
                    pb.setaMinMaxPB(0, 100);
                    pb.setaValorPB(0);
                    pb.setaMensagem("Recalculando marcações do período anterior...");
                    RecalculaMarcacao(context, jobReport, db, usuario, pBancoHoras.Tipo_Ant, pBancoHoras.Identificacao_Ant, (DateTime)pBancoHoras.DataInicial_Ant, (DateTime)pBancoHoras.DataFinal_Ant);
                }
                #endregion
            }

            #region AtualizaDadosAlterados

            pb.setaMinMaxPB(0, 100);
            pb.setaValorPB(0);
            pb.setaMensagem("Recalculando as marcações do período atual...");
            RecalculaMarcacao(context, jobReport, db, usuario, pBancoHoras.Tipo, pBancoHoras.Identificacao, (DateTime)pBancoHoras.DataInicial, (DateTime)pBancoHoras.DataFinal);

            #endregion
        }

        public void CalculaLancamentoCreditoDebito(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.InclusaoBanco pInclusaoBanco)
        {
            #region AtualizaDadosAnterior
            if (pInclusaoBanco.Acao != Modelo.Acao.Incluir)
            {
                RecalculaMarcacao(context, jobReport, db, usuario, pInclusaoBanco.Tipo_Ant, pInclusaoBanco.Identificacao_Ant, (DateTime)pInclusaoBanco.Data_Ant, (DateTime)pInclusaoBanco.Data_Ant);
            }
            #endregion

            #region AtualizaDadosAlterados
            RecalculaMarcacao(context, jobReport, db, usuario, pInclusaoBanco.Tipo, pInclusaoBanco.Identificacao, (DateTime)pInclusaoBanco.Data, (DateTime)pInclusaoBanco.Data);
            #endregion
        }

        public void CalculaParametro(PerformContext context, JobControl jobReport, string db, string usuario, int idParametro, DateTime dataInicial, DateTime dataFinal)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Horario bllHorario = new BLL.Horario(userPF.ConnectionString, userPF);
            IList<Modelo.Horario> ListaHorarios = bllHorario.getPorParametro(idParametro);
            RecalculaMarcacao(context, jobReport, db, usuario, 4, ListaHorarios.Select(x => x.Id).ToList(), dataInicial, dataFinal);
        }

        public void CalculaMudancaHorario(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.MudancaHorario mudHorario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
            DateTime? dataFinal = null;
            int? tipoRecalculo = 0;
            int idRecalculo = 0;
            switch (mudHorario.Tipo)
            {
                case 0:
                    dataFinal = bllMarcacao.GetUltimaDataFuncionario(mudHorario.Idfuncionario);
                    if (dataFinal != null)
                    {
                        tipoRecalculo = 2;
                        idRecalculo = mudHorario.Idfuncionario;
                    }
                    break;
                case 1:
                    dataFinal = bllMarcacao.GetUltimaDataDepartamento(mudHorario.IdDepartamento);
                    if (dataFinal != null)
                    {
                        tipoRecalculo = 1;
                        idRecalculo = mudHorario.IdDepartamento;
                    }
                    break;
                case 2:
                    dataFinal = bllMarcacao.GetUltimaDataEmpresa(mudHorario.IdEmpresa);
                    if (dataFinal != null)
                    {
                        tipoRecalculo = 0;
                        idRecalculo = mudHorario.IdEmpresa;
                    }
                    break;
                case 3:
                    dataFinal = bllMarcacao.GetUltimaDataFuncao(mudHorario.IdFuncao);
                    if (dataFinal != null)
                    {
                        tipoRecalculo = 3;
                        idRecalculo = mudHorario.IdFuncao;
                    }
                    break;
            }

            if (dataFinal != null && mudHorario.Data != null)
            {
                RecalculaMarcacao(context, jobReport, db, usuario, tipoRecalculo, idRecalculo, mudHorario.Data.Value, dataFinal.Value);
            }
        }

        public void OrdenaMarcacao(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Marcacao marcacao)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
            bllMarcacao.ObjProgressBar = pb;

            try
            {
                bllMarcacao.OrdenaMarcacao(marcacao, true);
                bllMarcacao.Salvar(Modelo.Acao.Alterar, marcacao);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void OrdenaMarcacaoPeriodo(PerformContext context, JobControl jobReport, string db, string usuario, int id, DateTime dataInicial, DateTime dataFinal)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
            bllMarcacao.ObjProgressBar = pb;

            try
            {
                bllMarcacao.OrdenaTodasMarcacoes(dataInicial, dataFinal, id);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void ManutencaoBilhete(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Marcacao objMarcacao, Modelo.BilhetesImp objBilheteImp, int tipoManutencao)
        {
            SetParametersBase(context, jobReport, db, usuario);
            try
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(userPF.ConnectionString, userPF);
                bllBilhetesImp.ObjProgressBar = pb;
                bllBilhetesImp.ManutencaoBilhete(objMarcacao, objBilheteImp, tipoManutencao);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void LancamentoLoteBilhetesProcessar(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.LancamentoLote LLFunc)
        {
            SetParametersBase(context, jobReport, db, usuario);
            Modelo.ProgressBar objProgressBar2 = new Modelo.ProgressBar();
            objProgressBar2.incrementaPB = this.IncrementaProgressBarVazio;
            objProgressBar2.setaMensagem = this.SetaMensagemVazio;
            objProgressBar2.setaMinMaxPB = this.SetaMinMaxProgressBarVazio;
            objProgressBar2.setaValorPB = this.SetaValorProgressBarVazio;

            DateTime? dataInicial, dataFinal;

            List<int> funcsEdits = LLFunc.LancamentoLoteFuncionarios.Where(w => w.UltimaAcao == (int)Modelo.Acao.Alterar && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsExc = LLFunc.LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Excluir && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsInc = LLFunc.LancamentoLoteFuncionarios.Where(w => (w.Acao == Modelo.Acao.Incluir || (w.Acao == Modelo.Acao.Alterar && w.UltimaAcao == (int)Modelo.Acao.Incluir)) && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsRec = new List<int>();
            funcsRec.AddRange(funcsEdits);
            funcsRec.AddRange(funcsExc);
            funcsRec.AddRange(funcsInc);
            if (funcsRec.Count() > 0)
            {
                pb.setaMinMaxPB(1, funcsRec.Count());
                BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(userPF.ConnectionString, userPF);
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPF.ConnectionString, userPF);

                IList<Modelo.Funcionario> funcs = bllFuncionario.GetFuncionariosPorIds(String.Join(",", funcsRec));
                List<string> pLog = new List<string>();
                foreach (Modelo.Funcionario func in funcs)
                {
                    pb.incrementaPB(1);
                    pb.setaMensagem("Gerando marcação para: " + func.Nome);
                    bllImportaBilhetes.ImportarBilhetes(func.Dscodigo, false, LLFunc.DataLancamento, LLFunc.DataLancamento, out dataInicial, out dataFinal, objProgressBar2, pLog);
                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(userPF.ConnectionString, userPF);
                    bllMarcacao.ObjProgressBar = objProgressBar2;
                    bllMarcacao.OrdenaTodasMarcacoes(LLFunc.DataLancamento, LLFunc.DataLancamento, func.Id);
                }
            }
        }

        public void CalculaMarcacaoLancamentoLoteDiaJob(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.LancamentoLote LLFunc)
        {
            SetParametersBase(context, jobReport, db, usuario);
            List<int> funcsEdits = LLFunc.LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Alterar && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsExc = LLFunc.LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Excluir && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            List<int> funcsInc = LLFunc.LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Incluir && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
            if ((LLFunc.DataLancamento != LLFunc.DataLancamentoAnt && funcsEdits.Count() > 0) || (funcsExc.Count() > 0) || (funcsInc.Count() > 0))
            {
                try
                {
                    BLL.CalculaMarcacao bllMarcacao;
                    // Se alterou a data do lançamento e existe funcionários para alteração altera as folgas na marcação
                    if (LLFunc.DataLancamento != LLFunc.DataLancamentoAnt && funcsEdits.Count() > 0)
                    {
                        pb.setaMensagem("Desvinculando o(s) " + funcsEdits.Count() + " funcionário(s) editado(s) do lote da data " + LLFunc.DataLancamentoAnt.ToShortDateString());
                        bllMarcacao = new BLL.CalculaMarcacao(funcsEdits, LLFunc.DataLancamentoAnt, LLFunc.DataLancamentoAnt, pb, false, userPF.ConnectionString, userPF);
                        bllMarcacao.CalculaMarcacoes();

                        pb.setaMensagem("Vinculando o(s) " + funcsEdits.Count() + " funcionário(s) editado(s) do lote da data " + LLFunc.DataLancamentoAnt.ToShortDateString());
                        bllMarcacao = new BLL.CalculaMarcacao(funcsEdits, LLFunc.DataLancamento, LLFunc.DataLancamento, pb, false, userPF.ConnectionString, userPF);
                        bllMarcacao.CalculaMarcacoes();
                    }

                    // Retira as folgas dos funcionários retirados do lote
                    if (funcsExc.Count() > 0)
                    {
                        pb.setaMensagem("Desvinculando o(s) " + funcsExc.Count() + " funcionário(s) excluídos(s) do lote");
                        bllMarcacao = new BLL.CalculaMarcacao(funcsExc, LLFunc.DataLancamentoAnt, LLFunc.DataLancamentoAnt, pb, false, userPF.ConnectionString, userPF);
                        bllMarcacao.CalculaMarcacoes();
                    }

                    // Adiciona as folgas dos funcionários incluidos do lote
                    if (funcsInc.Count() > 0)
                    {
                        pb.setaMensagem("Vinculando o(s) " + funcsInc.Count() + " funcionário(s) incluído(s) do lote");
                        bllMarcacao = new BLL.CalculaMarcacao(funcsInc, LLFunc.DataLancamento, LLFunc.DataLancamento, pb, false, userPF.ConnectionString, userPF);
                        bllMarcacao.CalculaMarcacoes();
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
        }

        public void RestauraFuncionarioJob(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.Funcionario objfunc)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPF.ConnectionString, userPF);
            objfunc.Excluido = 0;
            objfunc.Funcionarioativo = 1;
            bllFuncionario.ObjProgressBar = pb;
            try
            {
                bool importou = false;
                DateTime datai;
                DateTime dataf;
                List<string> log = new List<string>();
                if ((objfunc.Funcionarioativo != objfunc.Funcionarioativo_Ant) && (objfunc.Funcionarioativo == 1))
                {
                    importou = bllFuncionario.ImportacaoBilhete(objfunc, out datai, out dataf, log);
                }
                else
                {
                    datai = new DateTime();
                    dataf = new DateTime();
                }
                if (importou || (objfunc.Funcionarioativo != objfunc.Funcionarioativo_Ant) || (objfunc.Iddepartamento_Ant != objfunc.Iddepartamento)
                    || (objfunc.Idempresa_Ant != objfunc.Idempresa) || (objfunc.Idfuncao_Ant != objfunc.Idfuncao)
                    || (objfunc.Dataadmissao_Ant != objfunc.Dataadmissao) || (objfunc.Datademissao_Ant != objfunc.Datademissao)
                    || (objfunc.Naoentrarbanco_Ant != objfunc.Naoentrarbanco) || (objfunc.Naoentrarcompensacao_Ant != objfunc.Naoentrarcompensacao))
                {
                    bllFuncionario.AtualizaMarcacao(objfunc.Id, datai, dataf);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public void TransferirBilhetes(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.TransferenciaBilhetes transferenciaBilhetes)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.TransferenciaBilhetes bllTransferenciaBilhetes = new BLL.TransferenciaBilhetes(userPF.ConnectionString, userPF);
            bllTransferenciaBilhetes.TransferirBilhetes(transferenciaBilhetes.Id);
            BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(userPF.ConnectionString, userPF);
            List<string> log = new List<string>();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPF.ConnectionString, userPF);
            string dscodigoOrigem = bllFuncionario.GetDsCodigosByIDs(new List<int>() { transferenciaBilhetes.IdFuncionarioOrigem }).FirstOrDefault();
            DateTime dtInicioTB = transferenciaBilhetes.DataInicio.GetValueOrDefault().AddDays(-1);
            DateTime dtFimTB = transferenciaBilhetes.DataFim.GetValueOrDefault().AddDays(1);
            bllImportaBilhetes.ImportarBilhetes(dscodigoOrigem, false, dtInicioTB, dtFimTB, out DateTime? pdataiO, out DateTime? pdatafO, pb, log);
            string dscodigoDestino = bllFuncionario.GetDsCodigosByIDs(new List<int>() { transferenciaBilhetes.IdFuncionarioDestino }).FirstOrDefault();
            bllImportaBilhetes.ImportarBilhetes(dscodigoDestino, false, dtInicioTB, dtFimTB, out DateTime? pdataiD, out DateTime? pdatafD, pb, log);
            List<DateTime?> dts = new List<DateTime?>() { pdataiO, pdatafO, pdataiD, pdatafD };
            if (dts.Where(d => d != null).Any())
            {
                RecalculaMarcacao(context, jobReport, db, usuario, new List<int>() { transferenciaBilhetes.IdFuncionarioOrigem, transferenciaBilhetes.IdFuncionarioDestino }, dts.Min().GetValueOrDefault(), dts.Max().GetValueOrDefault(), true); 
            }
        }

        public void ImportarBilhetes(PerformContext context, JobControl jobReport, string db, string usuario, int idFuncionario, DateTime dtIni, DateTime dtFim)
        {
            SetParametersBase(context, jobReport, db, usuario);
            BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(userPF.ConnectionString, userPF);
            List<string> log = new List<string>();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPF.ConnectionString, userPF);
            string dscodigoDestino = bllFuncionario.GetDsCodigosByIDs(new List<int>() { idFuncionario }).FirstOrDefault();
            bllImportaBilhetes.ImportarBilhetes(dscodigoDestino, false, dtIni, dtFim, out DateTime? pdataiD, out DateTime? pdatafD, pb, log);
            RecalculaMarcacao(context, jobReport, db, usuario, new List<int>() { idFuncionario }, pdataiD.GetValueOrDefault(), pdatafD.GetValueOrDefault(), true);
        }

        public void FechamentoBH(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.FechamentoBH objFechamentoBH, Modelo.BancoHoras objBancoHoras)
        {
            SetParametersBase(context, jobReport, db, usuario);
            string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
            Modelo.Cw_Usuario userPF = new Modelo.Cw_Usuario();
            userPF.Login = usuario;
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conexao, userPF);
            //Atualiza as marcações
            pb.setaValorPBCMensagem(-1, "Verificando registros");
            bllMarcacao.InsereMarcacoesNaoExistentes(objFechamentoBH.Tipo, objFechamentoBH.Identificacao, objFechamentoBH.Data.Value, objFechamentoBH.Data.Value, pb, false);
            IList<Modelo.FechamentoBHDPercentual> listaobjFechamentoBHDPercentual = new List<Modelo.FechamentoBHDPercentual>();
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(conexao, userPF);
            //Realiza o fechamento do banco de horas por funcionario
            pb.setaValorPBCMensagem(-1, "Realizando fechamento de Banco de Horas, aguarde... (Pode levar alguns minutos)");
            bllFechamentoBH.ChamaCalculaFechamento(objBancoHoras, objFechamentoBH, ref listaobjFechamentoBHDPercentual, objFechamentoBH.PagamentoHoraCreAuto, objFechamentoBH.LimiteHorasPagamentoCredito, objFechamentoBH.PagamentoHoraDebAuto, objFechamentoBH.LimiteHorasPagamentoDebito, ref pb);
            pb.setaValorPBCMensagem(-1, "Iniciando Calculo");
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conexao, userPF);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(objFechamentoBH.Tipo, new List<int>() { objFechamentoBH.Identificacao }, false, false);
            List<PxyFuncionariosRecalcular> pxyFuncionariosRecalcular = idsFuncionarios.Select(s => new PxyFuncionariosRecalcular() { IdFuncionario = s, DataInicio = objFechamentoBH.Data, DataFim = objFechamentoBH.Data }).ToList();

            try
            {
                RecalculaMarcacao(context, jobReport, db, userPF.Login, pxyFuncionariosRecalcular);
            }
            catch (Exception)
            {
                string originalQueue = HangfireManagerBase.GetOriginalQueue(context);
                JobControl jobControlNovo = HangfireManagerBase.GerarJobControl("Calculo de marcações da inclusão de fechamento de Banco de Horas", jobReport);
                new BackgroundJobClient().ContinueJobWith<CalculosJob>(context.BackgroundJob.Id, x => x.RecalculaMarcacao(context, jobReport, db, userPF.Login, pxyFuncionariosRecalcular), new EnqueuedState(string.IsNullOrEmpty(originalQueue) ? "normal" : originalQueue));
            }
        }

        public void ExcluirFechamentoBH(PerformContext context, JobControl jobReport, string db, string usuario, Modelo.FechamentoBH objFechamentoBH)
        {
            SetParametersBase(context, jobReport, db, usuario);
            pb.setaValorPBCMensagem(-1, "Desfazendo fechamento de Banco de Horas, aguarde... (Pode levar alguns minutos)");
            string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
            Modelo.Cw_Usuario userPF = new Modelo.Cw_Usuario();
            userPF.Login = usuario;
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(conexao, userPF);
            bllFechamentoBH.ExcluirFechamento(objFechamentoBH.Id);
            pb.setaValorPBCMensagem(-1, "Recalculando dados");

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conexao, userPF);
            List<int> idsFuncionarios = bllFuncionario.GetIDsByTipo(objFechamentoBH.Tipo, new List<int>() { objFechamentoBH.Identificacao }, false, false);
            List<PxyFuncionariosRecalcular> pxyFuncionariosRecalcular = idsFuncionarios.Select(s => new PxyFuncionariosRecalcular() { IdFuncionario = s, DataInicio = objFechamentoBH.Data, DataFim = objFechamentoBH.Data }).ToList();

            try
            {
                RecalculaMarcacao(context, jobReport, db, userPF.Login, pxyFuncionariosRecalcular);
            }
            catch (Exception)
            {
                string originalQueue = HangfireManagerBase.GetOriginalQueue(context);
                JobControl jobControlNovo = HangfireManagerBase.GerarJobControl("Calculo de marcações da exclusão de fechamento de Banco de Horas", jobReport);
                new BackgroundJobClient().ContinueJobWith<CalculosJob>(context.BackgroundJob.Id, x => x.RecalculaMarcacao(null, jobReport, db, userPF.Login, pxyFuncionariosRecalcular), new EnqueuedState(string.IsNullOrEmpty(originalQueue) ? "normal" : originalQueue));
            }
        }
    }
}
