using Modelo.Acesso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Modelo;

namespace ServicoServico
{
    public partial class ServicoBloqueadorPontofopag : ServiceBase
    {

        #region Windows Service
        public static readonly string ServiceID = "ServicoBloqueioEstacoes";
        public static readonly string ServName = "Pontofopag.ServBloqueioEstacoes";
        public static readonly string ServDescription = "Serviço para bloqueio de estações";

        public ServicoBloqueadorPontofopag()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Iniciar();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Dispose();
            base.OnStop();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }

        public new void Dispose()
        {
            Parar();
            base.Dispose();
        }

        #endregion

        #region Execução
        private bool _disposed = false;
        private object Lock = new object();
        private Thread Executor;

        internal void Iniciar(bool console = false)
        {
            #if DEBUG
                        System.Diagnostics.Debugger.Launch();
            #endif

            Modelo.Utils.Utils.EscreveLog("Log", "Iniciando Serviço");
            lock (Lock)
            {
                if (Executor != null)
                    return;
                Executor = new Thread(Executar);
                Executor.Start();
            }
            if (console)
            {
                string input = string.Empty;
                while ((input = Console.ReadLine()).Length < 0) { }
                Parar();
            }
        }

        public void Parar()
        {
            lock (Lock)
            {
                _disposed = true;
                Monitor.PulseAll(Lock);
            }
        }

        #endregion

        #region Scheduler

        protected void Executar()
        {
            while (!_disposed)
            {
                Modelo.Utils.Utils.EscreveLog("Log", "Iniciando Integração");
                BLL.AcessoAPI bllAcesso = new BLL.AcessoAPI();
                Modelo.Acesso.AcessoAPI objAcesso = new Modelo.Acesso.AcessoAPI();
                try
                {
                    objAcesso = bllAcesso.CarregarDadosAcesso();
                    Modelo.Utils.Utils.EscreveLog("Log", "Dados de acesso carregados com sucesso, conectado na api = " + objAcesso.Url);
                    Modelo.Utils.Utils.EscreveLog("Log", "Processar Bloqueios");
                    ProcessarBloqueios(objAcesso);
                    Modelo.Utils.Utils.EscreveLog("Log", "Bloqueios Processados");
                }
                catch (Exception e)
                {
                    Modelo.Utils.Utils.EscreveLog("Log", "Erro na integração, erro = " + string.Concat(e.InnerExceptions().Select(s => s.Message + "; ") + " stack Trace = " + e.StackTrace));
                }
                lock (Lock)
                {
                    Modelo.Utils.Utils.EscreveLog("Log", "Dorme");
                    if (objAcesso.Timer <= 0)
                    {
                        objAcesso.Timer = 1;
                    }
                    Monitor.Wait(Lock, objAcesso.Timer * 60 * 1000);
                }
            }
            Executor = null;
        }

        #endregion

        #region Carga de bloqueios

        private int ProcessarBloqueios(AcessoAPI objAcesso)
        {

            try
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario();
                Modelo.Utils.Utils.EscreveLog("Log", "Carregando funcionários");
                List<Modelo.RegraBloqueio.Funcionario> listaFuncionarios = bllFuncionario.CarregarServico();
                Modelo.Utils.Utils.EscreveLog("Log", "Funcionário encontrados = " + listaFuncionarios.Count());
                BLL.HistoricoBloqueio bllHistorico = new BLL.HistoricoBloqueio();

                if (listaFuncionarios.Count > 0)
                {
                    using (BLL.PontofopagAPI.PontofopagAPI api = new BLL.PontofopagAPI.PontofopagAPI(objAcesso))
                    {
                        api.AoAtualizarToken += TokenAtualizado;
                        //Acessar o método de carga na API
                        Modelo.Utils.Utils.EscreveLog("Log", "Carregar Bloqueios");
                        List<BLL.PontofopagAPI.ModeloAPI.EstadoBloqueioFuncionario> bloqueios = api.CarregarBloqueios(listaFuncionarios.Select(func => func.CPF).ToList());
                        if (bloqueios == null || bloqueios.Count <= 0)
                        {
                            Modelo.Utils.Utils.EscreveLog("Log", "Nenhum bloqueio encontrado");
                            return objAcesso.Timer;
                        }
                        Modelo.Utils.Utils.EscreveLog("Log", "Processando bloqueios");
                        foreach (BLL.PontofopagAPI.ModeloAPI.EstadoBloqueioFuncionario bloqueio in bloqueios)
                        {
                            Modelo.Utils.Utils.EscreveLog("Log", "Processando bloqueio,  CPF = " + bloqueio.CPF + ", Bloqueado = " + bloqueio.Bloqueado + ", Regra = " + bloqueio.RegraBloqueio + ", Previsao Liberacao = " + bloqueio.PrevisaoLiberacao + ", Mensagem = " + bloqueio.Mensagem);
                            Modelo.RegraBloqueio.Funcionario funcionario = listaFuncionarios.First(func => func.CPF.Equals(bloqueio.CPF));
                            Modelo.Utils.Utils.EscreveLog("Log", "Comparando Situação do funcionário atualmente (Bloqueio = " + funcionario.Bloqueado + ", Regra do bloqueio = " + funcionario.RegraBloqueio + ") ");
                            if (funcionario.Bloqueado != bloqueio.Bloqueado
                                || (funcionario.Bloqueado && funcionario.RegraBloqueio != bloqueio.RegraBloqueio) 
                                || (funcionario.Mensagem != bloqueio.Mensagem))
                            {
                                Modelo.Utils.Utils.EscreveLog("Log", "Inserindo historico do bloqueio");
                                //Inserir histórico da mudança de estado
                                Modelo.RegraBloqueio.HistoricoBloqueio historico = new Modelo.RegraBloqueio.HistoricoBloqueio();
                                historico.Funcionario = funcionario;
                                historico.Bloqueado = bloqueio.Bloqueado;
                                historico.Liberacao = string.IsNullOrEmpty(bloqueio.PrevisaoLiberacao) ? (DateTime?)null : Convert.ToDateTime(bloqueio.PrevisaoLiberacao);
                                historico.Mensagem = bloqueio.Mensagem;
                                historico.RegraBloqueio = bloqueio.Bloqueado ? bloqueio.RegraBloqueio : (int?)null;
                                bllHistorico.Gravar(historico);
                                funcionario.AlertaEnviado = null;
                            }
                            Modelo.Utils.Utils.EscreveLog("Log", "Atualizando Funcionário");
                            //Atualizar o funcionário
                            funcionario.Bloqueado = bloqueio.Bloqueado;
                            funcionario.Liberacao = string.IsNullOrEmpty(bloqueio.PrevisaoLiberacao) ? (DateTime?)null : Convert.ToDateTime(bloqueio.PrevisaoLiberacao);
                            funcionario.Mensagem = bloqueio.Mensagem;
                            funcionario.RegraBloqueio = bloqueio.Bloqueado ? bloqueio.RegraBloqueio : (int?)null;
                            bllFuncionario.AtualizarBloqueioServico(funcionario);
                        }
                    }
                }
                return objAcesso.Timer;
            }
            catch (Exception e)
            {
                Modelo.Utils.Utils.EscreveLog("Log", "Erro ao processar bloqueio, erro = " + string.Concat(e.InnerExceptions().Select(s => s.Message + "; ") + " stack Trace = "+e.StackTrace));
                throw e;
            }
        }

        protected void TokenAtualizado(AcessoAPI acesso)
        {
            BLL.AcessoAPI bll = new BLL.AcessoAPI();
            bll.AtualizarToken(acesso);
        }

        #endregion

    }
}
