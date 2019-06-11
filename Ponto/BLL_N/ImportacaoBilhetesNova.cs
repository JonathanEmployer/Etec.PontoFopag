using Hangfire;
using Hangfire.Server;
using Hangfire.States;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BLL_N.JobManager.Hangfire;
using BLL_N.JobManager.Hangfire.Job;
using Modelo.EntityFramework.MonitorPontofopag;

namespace BLL_N
{
    public class ImportacaoBilhetesNova
    {
        private SqlConnectionStringBuilder conexao = new SqlConnectionStringBuilder();
        private Modelo.Cw_Usuario userPF;

        private PerformContext hangfireContext;

        public PerformContext HangfireContext
        {
            get { return hangfireContext; }
            set { hangfireContext = value; }
        }

        private JobControl jobReport;

        public JobControl JobReport
        {
            get { return jobReport; }
            set { jobReport = value; }
        }


        public ImportacaoBilhetesNova(String db, string usuario)
        {
            conexao = BLL.cwkFuncoes.ConstroiConexao(db);
            userPF = new Modelo.Cw_Usuario();
            userPF.Login = usuario;
        }

        public ImportacaoBilhetesNova(String conn, Modelo.Cw_Usuario usuarioLogado)
        {
            conexao = new SqlConnectionStringBuilder(conn);
            userPF = usuarioLogado;
        }

        public void ImportarRegistroPonto(List<int> idRegistros)
        {
            BLL.RegistroPonto bllRegPonto = new BLL.RegistroPonto(conexao.ConnectionString, userPF);
            List<Modelo.RegistroPonto> registros = bllRegPonto.GetAllListByIds(idRegistros, new List<Enumeradores.SituacaoRegistroPonto>() { Enumeradores.SituacaoRegistroPonto.Processando });
            if (registros.Count() > 0)
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(conexao.ConnectionString, userPF);

                List<Modelo.BilhetesImp> bilhetesNovos = GerarBilhetesPorRegistros(registros, bllBilhetesImp);

                bllBilhetesImp.DesconsiderarBilhetesPorRegistroPonto(registros);

                List<BilhetesImp> BilhetesProcessar = bllBilhetesImp.LoadPorRegistroPonto(registros.Select(s => s.Id).ToList()); //Recupera todos os Bilhetes existentes, os que foram inseridos nesso processo, e os que podem ter sido inseridos anteriormente

                bllBilhetesImp.ReconsiderarBilhetesPorRegistroPonto(BilhetesProcessar, registros);
                
                #region Seta como erro os registros que não conseguiu incluir
                List<int> idsRegistrosBilhetesNaoInc = bilhetesNovos.Where(w => !BilhetesProcessar.Select(s => s.IdRegistroPonto).Contains(w.IdRegistroPonto)).Select(s => s.IdRegistroPonto.GetValueOrDefault()).ToList();
                if (idsRegistrosBilhetesNaoInc.Count > 0)
                    bllRegPonto.SetarSituacaoRegistros(idsRegistrosBilhetesNaoInc, Enumeradores.SituacaoRegistroPonto.Erro);
                #endregion

                if (BilhetesProcessar.Count > 0)
                {
                    if (bilhetesNovos.Count > 0) //Se existe novos bilhetes, verifica se é necessário incluir localização
                    {
                        bilhetesNovos.Where(w => BilhetesProcessar.Select(s => s.IdRegistroPonto).Contains(w.IdRegistroPonto)).ToList().ForEach(f => f.Id = BilhetesProcessar.Where(w => w.IdRegistroPonto == f.IdRegistroPonto).Select(s => s.Id).FirstOrDefault()); // Seta os ids nos registros que acabaram de ser salvos
                        InserirLocalizacaoBilhete(bilhetesNovos.Where(w => BilhetesProcessar.Select(s => s.IdRegistroPonto).Contains(w.IdRegistroPonto)).ToList()); //Inclui localização apenas dos registros que conseguiu inserir, ou seja, apenas dos registros que conseguiram ser carregados apos a inclusão
                    }

                    List<Int32> idsBilhetesNecessitamImportar = BilhetesProcessar.Where(w => w.Id > 0 && w.Importado == 2).Select(s => s.Id).ToList(); //Verifica os bilhetes que não foram importados ainda. (Como a rotina pode estar passando por reprocesso, alguns podem já ter sido importados em outra ocasição)
                    if (idsBilhetesNecessitamImportar.Count > 0)
                    {
                        BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(conexao.ConnectionString, userPF);
                        DateTime dtIni = DateTime.Now.Date;
                        DateTime dtFim = DateTime.Now.Date;
                        bool importou = bllImportaBilhetes.ImportarBilhetesNovo(idsBilhetesNecessitamImportar, out dtIni, out dtFim);

                        CalcularRegistros(BilhetesProcessar, dtIni, dtFim);
                    }
                    if (BilhetesProcessar.Count > 0)
                    {
                        bllRegPonto.SetarSituacaoRegistros(BilhetesProcessar.Select(s => s.IdRegistroPonto.GetValueOrDefault()).ToList(), Enumeradores.SituacaoRegistroPonto.Concluido);
                    }
                }
            }
        }

        private List<BilhetesImp> GerarBilhetesPorRegistros(List<Modelo.RegistroPonto> registros, BLL.BilhetesImp bllBilhetesImp)
        {
            //Verifica o que já foi inserido em outro reprocesso, pode ser que seja um reprocesso, e já tenha importado algo parcialmente anteriormente.
            DataTable bilhetesJaInseridos = bllBilhetesImp.GetIdsBilhetesByIdRegistroPonto(registros.Select(s => s.Id).ToList());
            List<int> idsRegistrosJaIncluidosBilhete = bilhetesJaInseridos.AsEnumerable()
                            .Select(i => Convert.ToInt32(i["IdRegistroPonto"])).ToList();
            List<Modelo.BilhetesImp> bilhetes = bllBilhetesImp.GerarBilhetesPelosRegistrosPonto(registros.Where(w => !idsRegistrosJaIncluidosBilhete.Contains(w.Id)).ToList()); //Bilhetes a serem inseridos
            int ret = 0;
            if (bilhetes.Count() > 0)
            {
                ret = bllBilhetesImp.Salvar(Acao.Incluir, bilhetes, userPF.Login, conexao.ConnectionString); //Salva os Bilhetes, por usar bookcopy não é possivel recuperar dados do que foi inserido. Ret significa a quantidade de registros que foi adicionado no bookcopy
            }
            return bilhetes;
        }

        private void CalcularRegistros(List<BilhetesImp> BilhetesProcessar, DateTime dtIni, DateTime dtFim)
        {
            List<int> idsFuncionariosCalcular = BilhetesProcessar.Select(s => s.IdFuncionario).Distinct().ToList();
            try
            {
                //Caso o primeiro recalculo de erro, joga para fila tentar novamente
                JobManager.CalculoMarcacoes.CalculaMarcacoes(conexao.InitialCatalog, userPF.Login, idsFuncionariosCalcular, dtIni, dtFim);
            }
            catch (Exception)
            {
                try
                {
                    var client = new BackgroundJobClient();
                    var state = new EnqueuedState("critico");
                    var jobId = client.Create(() => JobManager.CalculoMarcacoes.CalculaMarcacoes(conexao.InitialCatalog, userPF.Login, idsFuncionariosCalcular, dtIni, dtFim), state);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        private void InserirLocalizacaoBilhete(List<BilhetesImp> bilhetes)
        {
            BLL.LocalizacaoRegistroPonto bllLrp = new BLL.LocalizacaoRegistroPonto(conexao.ConnectionString, userPF);
            bilhetes.Where(w => w.localizacaoRegistroPonto != null).ToList().ForEach(f => f.localizacaoRegistroPonto.IdBilhetesImp = f.Id);
            bllLrp.InserirRegistros(bilhetes.Where(w => w.localizacaoRegistroPonto != null).Select(s => s.localizacaoRegistroPonto).ToList());
        }

        public List<string> ImportacaoBilhete(Modelo.ProgressBar pb, List<Modelo.TipoBilhetes> listaTipoBilhetes, string diretorio, int bilhete, bool bIndividual,
                                                 string dsCodFuncionario, DateTime? datai, DateTime? dataf, Modelo.UsuarioPontoWeb usuarioLogado)
        {
            List<string> log = new List<string>();
            try
            {
                BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(conexao.ConnectionString, usuarioLogado);
                string mensagem = String.Empty;
                log.Add("Importação de Bilhetes");
                log.Add("Data = " + DateTime.Now.ToShortDateString());
                log.Add("Hora = " + DateTime.Now.ToShortTimeString());

                bllBilhetesImp.ObjProgressBar = pb;
                bool temBilhetes = bllBilhetesImp.ImportacaoBilhetes(listaTipoBilhetes, diretorio, bilhete, bIndividual, dsCodFuncionario, ref datai, ref dataf, log, String.Empty, usuarioLogado);

                BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(conexao.ConnectionString, usuarioLogado);
                DateTime? dataInicial;
                DateTime? dataFinal;

                pb.setaMensagem("Iniciando Importação de Bilhetes");
                if (temBilhetes)
                {
                    List<string> FuncsProcessados = new List<string>();

                    pb.incrementaPBCMensagem(20, "Importando Bilhetes...");
                    if (bllImportaBilhetes.ImportarBilhetes(dsCodFuncionario, false, datai, dataf, out dataInicial, out dataFinal, pb, log, out FuncsProcessados))
                    {
                        BLL.Funcionario bllFuncionario = new BLL.Funcionario(conexao.ConnectionString, usuarioLogado);
                        List<int> idsFuncs = bllFuncionario.GetIDsByDsCodigos(FuncsProcessados);
                        pb.setaValorPB(100);
                        pb.setaMensagem("Importação de bilhetes concluída, calculando Marcacoes...");
                        if (hangfireContext != null && jobReport != null)
                        {
                            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(conexao.ConnectionString);
                            string database = builder.InitialCatalog;
                            int idJob;
                            int.TryParse(hangfireContext.BackgroundJob.Id, out idJob);
                            jobReport.JobId = idJob;
                            JobControl jobControl = HangfireManagerBase.GerarJobControl("Calculo de marcações de bilhetes importados", jobReport);
                            var childHangfireJobId = new BackgroundJobClient().ContinueWith<CalculosJob>(hangfireContext.BackgroundJob.Id, x => x.RecalculaMarcacao(null, jobControl, database, usuarioLogado.Login, idsFuncs, dataInicial.GetValueOrDefault(), dataFinal.GetValueOrDefault()), new EnqueuedState("normal"));
                        }
                        else
                        {
                            BLL_N.JobManager.CalculoMarcacoes.CalculaMarcacoes(idsFuncs, dataInicial.GetValueOrDefault(), dataFinal.GetValueOrDefault(), conexao.ConnectionString, usuarioLogado, pb);
                        }

                    }
                    else
                    {
                        if (!temBilhetes)
                        {
                            log.Add("O Arquivo não tem bilhetes para importar ou\não está com layout correto.\nVerifique.");
                            log.Add("---------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            log.Add("Arquivo de bilhetes importado com sucesso. Nenhuma marcação processada.");
                            log.Add("---------------------------------------------------------------------------------------");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                log.Add(ex.Message);
            }
            return log;
        }
    }
}
