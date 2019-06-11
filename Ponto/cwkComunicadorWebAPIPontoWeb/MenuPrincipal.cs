using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.BLL.Jobs;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using cwkComunicadorWebAPIPontoWeb.Forms;
using Ionic.Zip;
using System.IO;
using System.Xml.Linq;

namespace cwkComunicadorWebAPIPontoWeb
{
    public partial class MenuPrincipal : Form
    {
        private readonly Dictionary<string, Form> telasAbertas = new Dictionary<string, Form>();
        private static FileSystemMonitor<ReportaErro> monitor;
        public Progress<ReportaErro> progress;
        public CancellationTokenSource cts;

        public bool bErroInternet;
        public bool bErroRelogio;
        public bool bErroWs;
        private bool _minimizar;

        public MenuPrincipal(bool minimizar)
        {
            InitializeComponent();
            progress = new Progress<ReportaErro>(ReportaProgresso);
            cts = new CancellationTokenSource();
            bErroInternet = false;
            bErroRelogio = false;
            bErroWs = false;
            _minimizar = minimizar;
        }

        private void MenuPrincipal_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Pontofopag Comunicador";
            notifyIcon1.BalloonTipText = "O aplicativo foi minimizado para a barra de tarefas. Para restaurar, dê um clique duplo sobre este ícone.";
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
        }

        private void MenuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                if (MessageBox.Show("Deseja encerrar o Pontofopag Comunicador?", "Fechando Pontofopag Comunicador", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                    this.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    if (notifyIcon1 != null)
                    {
                        notifyIcon1.Visible = false;
                        notifyIcon1.Dispose();
                    }
                }
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listagemDeREPsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridRep grid = new GridRep();
            grid.ShowInTaskbar = true;
            grid.ShowDialog();
        }

        private void sairMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listagemDeREPsMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridRep grid = new GridRep();
            ShowForm(grid);
        }

        #region Métodos Auxiliares
        private void ShowForm(Form form)
        {
            if (TentarMostrarFormAberto(form))
                return;

            form.MdiParent = this;
            form.Show();



            if (telasAbertas.ContainsKey(form.Text))
                telasAbertas.Remove(form.Text);
            telasAbertas.Add(form.Text, form);
        }

        private bool TentarMostrarFormAberto(Form form)
        {
            var formAberto = BuscarFormAberto(form);
            if (formAberto != null)
            {
                form.Dispose();
                SetarFocoForm(formAberto);
                return true;
            }
            return false;
        }

        private void SetarFocoForm(Form formAberto)
        {
            if (formAberto.WindowState == FormWindowState.Minimized)
                formAberto.WindowState = FormWindowState.Normal;
            formAberto.Focus();
        }

        private Form BuscarFormAberto(Form form)
        {
            if (telasAbertas.ContainsKey(form.Text))
            {
                var t = telasAbertas[form.Text];
                if (t.IsDisposed)
                    telasAbertas.Remove(form.Text);
                else
                    return t;
            }
            return null;
        }
        #endregion

        private async void bwJobs_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;

            Task.Factory.StartNew(async () => await VerificadorProblemasConectividade()).ConfigureAwait(false);
            Task.Factory.StartNew(async () => await VerificadorProblemasComRelogio()).ConfigureAwait(false);

            ImportacaoBLL bllImport = new ImportacaoBLL();
            CancellationToken ct = new CancellationToken();

            if (Program.scheduler == null)
            {
                Program.scheduler = StdSchedulerFactory.GetDefaultScheduler();
            }
            if (!Program.scheduler.IsStarted)
            {
                Program.scheduler.Start();
            }
            List<Tuple<IJobDetail, ITrigger>> listaJobs = await bllImport.GetJobsImportacaoAsync(ct, progress);
            foreach (var item in listaJobs)
            {
                Program.scheduler.ScheduleJob(item.Item1, item.Item2);
            }
            monitor = new FileSystemMonitor<ReportaErro>();
            monitor.MonitorarPasta(CwkUtils.FileLogStringUtil("AFDsImportados"), "*.txt", BLL.Workers.EnviaBilheteWorker.Enviar, progress);
            monitor.Start();
            if (AtualizarAplicativo.AtualizacaoAutomatica())
	        {
		          AgendaAtualizacaoSistema(progress);
	        }
            Thread.Sleep(1000);
        }

        private void bwJobs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            notifyIcon1.ShowBalloonTip(10000, "Pontofopag Comunicador", "As tarefas foram agendadas", ToolTipIcon.Info);
        }

        private void bwJobs_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


        }

        private void MenuPrincipal_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            if (!bwJobs.IsBusy)
            {
                bwJobs.RunWorkerAsync();
            }
            if (ViewModels.VariaveisGlobais.URL_WS != ViewModels.VariaveisGlobais.WsProducao)
            {
                lbInfoServer.Text = "Servidor: " + ViewModels.VariaveisGlobais.URL_WS;
            }
            else
            {
                lbInfoServer.Text = "";
            }
            AtualizarComunicador();
        }

        private void AtualizarComunicador()
        {
            try
            {
                LoginBLL loginBLL = new LoginBLL();
                XDocument xD = loginBLL.GetXmlConf();

                    AtualizarAplicativo AtualizarApp = new AtualizarAplicativo(progress);
                    String versaoAtual, versaoFTP;
                    if (AtualizarAplicativo.AtualizacaoAutomatica())
                    {
                        List<ArquivosAtualizacaoViewModel> lArquivosAtualizar = AtualizarApp.VerificaAtualizacao(out versaoAtual, out versaoFTP);
                    
                        if (lArquivosAtualizar.Count() > 0)
                        {
                            Forms.FormAtualizacaoAplicacao form = new Forms.FormAtualizacaoAplicacao(progress, true);
                            form.ShowDialog();
                        }
                    }
                    else
                    {
                        ReportaProgresso(new ReportaErro() { Mensagem = "Atualização automática desabilita.", TipoMsg = TipoMensagem.Aviso });
                    }
            }
            catch (Exception)
            {
               
            }
        }

        public void AgendaAtualizacaoSistema(Progress<ReportaErro> progress)
        {
            Forms.FormAtualizacaoAplicacao form = new Forms.FormAtualizacaoAplicacao(progress, true);
            try
            {
                IDictionary<string, object> DadosAtualizacao = new Dictionary<string, object>();
                DadosAtualizacao.Add("progress", progress);
                DadosAtualizacao.Add("form", form);

                Random r = new Random();
                DateTime dtAgendamento = CwkUtils.RandomDate(r, Convert.ToDateTime("10/03/2015 00:01"), Convert.ToDateTime("10/03/2015 23:59"));
                string msg = "(Job) Atualização do sistema: Agendado em: " + DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss") + " Para todos os dias às " + dtAgendamento.ToShortTimeString();
                string dt = DateTime.Now.ToString("ddMMyyy_HHmmss");
                IJobDetail job = JobBuilder.Create<AtualizaComunicadorJob>()
                .WithIdentity("AtualizacaoSistema" + "_Instant_" + dt, "AtualizacaoJobGroup")
                .WithDescription(msg)
                .UsingJobData(new JobDataMap(DadosAtualizacao))
                .RequestRecovery(true)
                .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("TriggerAtualizacao" + "_Instant_" + dt, "AtualizacaoTriggerGroup")
                    .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                    .StartAt(DateBuilder.TomorrowAt(dtAgendamento.Hour, dtAgendamento.Minute, dtAgendamento.Second))
                    .Build();
                ReportaProgresso(new ReportaErro() { Mensagem = msg, TipoMsg = TipoMensagem.Info });

                if (Program.scheduler == null)
                {
                    Program.scheduler = StdSchedulerFactory.GetDefaultScheduler();
                }
                if (!Program.scheduler.IsStarted)
                {
                    Program.scheduler.Start();
                }

                Program.scheduler.ScheduleJob(job, trigger);

            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    ReportaProgresso(new ReportaErro() { Mensagem = "Operação cancelada pelo usuário.", TipoMsg = TipoMensagem.Erro });
                }
                else
                {
                    ReportaProgresso(new ReportaErro() { Mensagem = "Erro ao executar operação: " + e.Message, TipoMsg = TipoMensagem.Erro });
                }
            }

        }

        private void tspBtnReps_Click(object sender, EventArgs e)
        {
            GridRep grid = new GridRep();
            grid.progress = this.progress;
            ShowForm(grid);
        }

        private void tsbImportarDados_Click(object sender, EventArgs e)
        {
            GridDadosExportacaoRep grid = new GridDadosExportacaoRep(progress);
            ShowForm(grid);
        }

        private async void tsbExportarDados_Click(object sender, EventArgs e)
        {
            ImportacaoBLL bllImp = new ImportacaoBLL();

            BeginInvoke(new Action(() =>
            {
                ToolStripButton tsb = (ToolStripButton)sender;
                tsb.Enabled = false;
            }));

            await bllImp.AgendaJobsImportacaoExecucaoInstantaneaAsync(cts.Token, progress);

            BeginInvoke(new Action(() =>
            {
                ToolStripButton tsb = (ToolStripButton)sender;
                tsb.Enabled = true;
            }));
        }

        private void tsbSair_Click_2(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ReportaProgresso(ReportaErro progresso)
        {
            BeginInvoke(new Action(() =>
                RichTextBoxExtensions.NewLineText(rtbLog, progresso.ToString(), progresso.CorFonte.HasValue ? progresso.CorFonte.Value : SystemColors.WindowText)
            ));
        }

        private void importarDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridDadosExportacaoRep grid = new GridDadosExportacaoRep(progress);
            grid.ShowInTaskbar = true;
            ShowForm(grid);
        }

        private async Task VerificadorProblemasConectividade()
        {
            Int64 contador = 0;
            while (true)
            {
                await FazVerificacaoInternet();
                if ((sbSitInternet.Image == Properties.Resources.InternetOk64) &&
                    (!bErroInternet))
                {
                    if (contador % 10 == 0)
                    {
                        FazVerificacaoWS(false);
                    }
                }
                contador = VerificaValorMaximoInt64(contador);
                contador++;
                Thread.Sleep(30000); // espera 30 segundos para verificar novamente
            }
        }

        private static long VerificaValorMaximoInt64(Int64 contador)
        {
            if (contador == Int64.MaxValue)
                contador = 0;
            return contador;
        }

        private async Task<Task> VerificadorProblemasComRelogio()
        {
            RepBLL repBll = new RepBLL();
            ListaRepsViewModel lReps = new ListaRepsViewModel();
            List<RepViewModel> Reps = new List<RepViewModel>();
            while (true)
            {
                lReps = await repBll.GetXmlRepDataAsync();
                Reps = lReps.Reps.Where(s => s.ImportacaoAtivada && !String.IsNullOrEmpty(s.Ip)).ToList();
                await FazVerificacaoRelogio(Reps, false); 
                Thread.Sleep(30000); // espera 30 segundos para verificar novamente
            }
        }

        private async Task FazVerificacaoInternet()
        {
            try
            {
                if (!(await CwkUtils.InternetDisponivel()))
                {
                    BeginInvoke(new Action(() =>
                    {



                        string msg = "Verifique sua conexão com a Internet! Nâo será possível importar/exportar dados enquanto estiver off-line.";
                        if (FormWindowState.Minimized == this.WindowState)
                        {
                            SetarMsgTelaMinimizada(msg, notifyIcon1, CwkUtils.tipoErro.Internet);
                        }
                        else
                        {
                            SetarMsgTelaMaximizada(msg, notifyIcon1, CwkUtils.tipoErro.Internet);
                        }
                        bErroInternet = true;
                    }
                    ));
                }
                else
                {
                    BeginInvoke(new Action(() =>
                    {
                        sbSitInternet.Image = Properties.Resources.InternetOk64;
                        sbSitInternet.ToolTip = "Internet OK";
                        bErroInternet = false;
                    }));
                }
            }
            catch (Exception e)
            {
                BeginInvoke(new Action(() =>
                {
                    sbSitInternet.Image = Properties.Resources.InternetOk64;
                    sbSitInternet.ToolTip = "Internet OK";
                    bErroInternet = false;
                }));
            }

            
        }

        private async Task FazVerificacaoRelogio(List<RepViewModel> lReps, bool bMostraMessageBox)
        {
            StringBuilder msgBuilder = new StringBuilder();
            msgBuilder.Append(Environment.NewLine);
            foreach (var item in lReps)
            {
                try
                {
                    string enderecoRelogio = item.Ip;
                    if (!(await CwkUtils.EnderecoDisponivel(enderecoRelogio)))
                    {
                        msgBuilder.Append(" - Código: " + item.Codigo + ", Local: " + item.EnderecoEmpregador + ", IP: " + item.Ip + "." + Environment.NewLine);
                    }
                }
                catch (Exception e)
                {
                    BeginInvoke(new Action(() =>
                    {
                        sbSitRep.Image = Properties.Resources.RepOk64;
                        sbSitRep.ToolTip = "REP OK";
                        bErroRelogio = false;
                        RetornaMessageBox(bMostraMessageBox, sbSitRep, "REP OK", "REP Erro");
                    }));
                }
            }
            if (msgBuilder.ToString() != Environment.NewLine)
            {
                BeginInvoke(new Action(() =>
                {
                    string msg = "Os Relógios: " + msgBuilder.ToString() + "não estão disponíveis. Verifique as configurações.";
                    if (FormWindowState.Minimized == this.WindowState)
                    {
                        SetarMsgTelaMinimizada(msg, notifyIcon1, Utils.CwkUtils.tipoErro.Relogio);
                    }
                    else
                    {
                        SetarMsgTelaMaximizada(msg, notifyIcon1, Utils.CwkUtils.tipoErro.Relogio);
                    }
                    bErroRelogio = true;
                    RetornaMessageBox(bMostraMessageBox, sbSitRep, "REP OK", "REP Erro");
                }));
            }
            else
            {
                BeginInvoke(new Action(() =>
                {
                    sbSitRep.Image = Properties.Resources.RepOk64;
                    sbSitRep.ToolTip = "REP OK";
                    bErroRelogio = false;
                    RetornaMessageBox(bMostraMessageBox, sbSitRep, "REP OK", "REP Erro");
                }));
            }
            
           
        }

        private void RetornaMessageBox(bool bMostraMessageBox, DevExpress.XtraEditors.SimpleButton botao, string msgOK, string msgErro)
        {
            if (bMostraMessageBox)
            {
                if (botao.ToolTip == msgOK)
                {
                    MessageBox.Show(msgOK, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(msgErro, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FazVerificacaoWS(bool bMostraMessageBox)
        {
            try
            {
                if (!CwkUtils.VerificaConexaoEndereco(VariaveisGlobais.WsProducao))
                {
                    BeginInvoke(new Action(() =>
                    {
                        string msg = "Não é possível comunicar com o servidor. Contate o Suporte.";
                        if (FormWindowState.Minimized == this.WindowState)
                        {
                            SetarMsgTelaMinimizada(msg, notifyIcon1, CwkUtils.tipoErro.WS);
                        }
                        else
                        {
                            SetarMsgTelaMaximizada(msg, notifyIcon1, CwkUtils.tipoErro.WS);
                        }
                        bErroWs = true;
                        RetornaMessageBox(bMostraMessageBox, sbSitWs, "WebService OK", "WebService Erro");
                    }
                    ));
                }
                else
                {
                    BeginInvoke(new Action(() =>
                    {
                        sbSitWs.Image = Properties.Resources.WebserviceOK64;
                        sbSitWs.ToolTip = "WebService OK";
                        bErroWs = false;
                        RetornaMessageBox(bMostraMessageBox, sbSitWs, "WebService OK", "WebService Erro");
                    }));
                }
            }
            catch (Exception e)
            {
                BeginInvoke(new Action(() =>
                {
                    sbSitWs.Image = Properties.Resources.WebserviceOK64;
                    sbSitWs.ToolTip = "WebService OK";
                    bErroWs = false;
                    RetornaMessageBox(bMostraMessageBox, sbSitWs, "WebService OK", "WebService Erro");
                }));
            }
            
        }

        private void SetarMsgTelaMaximizada(string msg, NotifyIcon notifyIconComponenteErro, Utils.CwkUtils.tipoErro tpErro)
        {
            switch (tpErro)
            {
                case CwkUtils.tipoErro.Internet:
                    sbSitInternet.Image = Properties.Resources.InternetErro64;
                    sbSitInternet.ToolTip = msg;
                    break;
                case CwkUtils.tipoErro.Relogio:
                    sbSitRep.Image = Properties.Resources.RepErro64;
                    sbSitRep.ToolTip = msg;
                    break;
                case CwkUtils.tipoErro.WS:
                    sbSitWs.Image = Properties.Resources.WebserviceErro64;
                    sbSitWs.ToolTip = msg;
                    break;
            }
            notifyIconComponenteErro.Visible = false;
        }

        private void SetarMsgTelaMinimizada(string msg, NotifyIcon notifyIconComponenteErro, Utils.CwkUtils.tipoErro tpErro)
        {
            notifyIconComponenteErro.BalloonTipTitle = "Pontofopag Comunicador";
            notifyIconComponenteErro.BalloonTipText = msg;
            notifyIconComponenteErro.Visible = true;
            notifyIconComponenteErro.ShowBalloonTip(5000);
        }

        private void tsbTrocarUsuario_Click(object sender, EventArgs e)
        {
            LoginBLL.LimpaConfiguracaoCwork();
            CwkUtils.ReiniciarSistema();
        }

        private void sbSitInternet_Click(object sender, EventArgs e)
        {
            FormSituacaoInternet form = new FormSituacaoInternet();
            ShowForm(form);

        }

        private async void sbSitRep_Click(object sender, EventArgs e)
        {
            RepBLL repBll = new RepBLL();
            ListaRepsViewModel lReps = await repBll.GetXmlRepDataAsync();
            List<RepViewModel> Reps = lReps.Reps.Where(s => s.ImportacaoAtivada && !String.IsNullOrEmpty(s.Ip)).ToList();
            await FazVerificacaoRelogio(Reps, true);
        }

        private void sbSitWs_Click(object sender, EventArgs e)
        {
            FazVerificacaoWS(true);
        }
        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            if (_minimizar)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += " " + fvi.FileVersion;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sbAtualizarAplicacao_Click(object sender, EventArgs e)
        {
            Forms.FormAtualizacaoAplicacao form = new Forms.FormAtualizacaoAplicacao(progress, false);
            ShowForm(form);
        }

        private void MenuPrincipal_Activated(object sender, EventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void rtbLog_TextChanged(object sender, EventArgs e)
        {
            rtbLog.SelectionStart = rtbLog.Text.Length; 
            rtbLog.ScrollToCaret(); 
        }

        private void tsbDataHora_Click(object sender, EventArgs e)
        {
            Forms.FormConfiguracoesDataHora form = new Forms.FormConfiguracoesDataHora(progress);
            ShowForm(form);
        }

        private void btnEnviarLog_Click(object sender, EventArgs e)
        {
            Forms.PopupEnvioLogsEmail form = new Forms.PopupEnvioLogsEmail();
            ShowForm(form);
        }


    }
}
