using DevExpress.XtraEditors;
using Microsoft.Win32;
using Modelo;
using Modelo.Registrador;
using RegistradorBiometrico.Integracao.Veridis;
using RegistradorBiometrico.Integracao.Veridis.EventListener;
using RegistradorBiometrico.Integracao.Veridis.EventListener.Base;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Service;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View.Base;
using RegistradorPonto.Util;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Veridis.Biometric;

namespace RegistradorBiometrico.View
{
    public partial class FormRegistraPonto : FormBase
    {
        #region Propriedades

        private DateTime dataHoraBatida { get; set; }
        private Boolean bMinimizar { get; set; }
        private Boolean bConectividade { get; set; }
        private Boolean bVerificadorConectividade { get; set; }
        private EnumeraveisUtil.SituacaoBotaoRegistrar sbSituacaoBotaoRegistrar { get; set; }

        #endregion

        #region Atributos

        private VeridisEventListenerRegistrar objEventListener;

        private VeridisEquipamento<VeridisEventListenerRegistrar> objEquipamentoVeridis;

        private System.Windows.Forms.Timer objTimer = new System.Windows.Forms.Timer();

        private NotifyIcon notifyIcon1;

        #endregion

        public FormRegistraPonto(bool pBMinimizar)
        {
            InitializeComponent();

            InicializaObjetosForm();
            InstalarLicencaVeridis();
            InicializarNotifyIcon();

            bMinimizar = pBMinimizar;
        }

        private void InicializaObjetosForm()
        {
            try
            {
                InicializarLeitorBiometrico();

                bVerificadorConectividade = true;
            }
            catch (Exception ex)
            {
                TrataExcecoes("Erro ao iniciar o sistema", ex);
            }
        }

        #region NotifyIcon

        private void InicializarNotifyIcon()
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();

            contextMenu.MenuItems.Add(CriaMenuItemNotifyIcon("S&air", 0, menuItemFechar_Click));
            contextMenu.MenuItems.Add(CriaMenuItemNotifyIcon("R&estaurar", 1, menuItemRestaurar_Click));

            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon();
            notifyIcon1.ContextMenu = contextMenu;
            notifyIcon1.Icon = RegistradorBiometrico.Properties.Resources.logo;
            notifyIcon1.MouseDoubleClick += NotifyIcon1_MouseDoubleClick;
        }

        private MenuItem CriaMenuItemNotifyIcon(String texto, int indice, Action<object, EventArgs> menuItem_Click)
        {
            MenuItem menuItem = new MenuItem();

            menuItem.Index = indice;
            menuItem.Text = texto;
            menuItem.Click += new System.EventHandler(menuItem_Click);

            return menuItem;
        }

        #endregion

        #region Relógio

        private void TickRelogio(object sender, EventArgs e)
        {
            dataHoraBatida = dataHoraBatida.AddSeconds(1);
            String hora = dataHoraBatida.ToString("HH:mm:ss");

            if (lblHorario.InvokeRequired)
                BeginInvoke(new Action(() => { lblHorario.Text = hora; }));
            else
                lblHorario.Text = hora;
        }


        private async void InicializarDataHora()
        {
            try
            {
                RegistradorService registradorService = new RegistradorService();
                dataHoraBatida = await Task.Factory.StartNew(() => registradorService.AtualizarHora(new CancellationToken())).Result;
            }
            catch (Exception ex)
            {
                TrataExcecoes("Erro ao atualizar a hora.", ex);
            }
        }

        public async Task AtualizaDataHora(Configuracao objConfiguracao)
        {
            try
            {
                DateTime dtUltimaSincronizacao;

                DateTime.TryParse(objConfiguracao.UltimaAtualizacaoHora, out dtUltimaSincronizacao);

                Int32 totalHorasUltimaSincronizacao = (DateTime.Now - dtUltimaSincronizacao).Hours;

                if (totalHorasUltimaSincronizacao > 1)
                {
                    await Task.Factory.StartNew(() => InicializarDataHora());

                    objConfiguracao.UltimaAtualizacaoHora = dataHoraBatida.ToString();
                    Configuracao.SalvarConfiguracoes(objConfiguracao);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Aplicativo

        private void EncerrarAplicativo()
        {
            notifyIcon1.Visible = false;
            objTimer.Stop();

            ChamaParadaEquipamento();
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void MaximizarTela()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        #endregion

        #region Equipamento

        private void InstalarLicencaVeridis()
        {
            InicializarLeitorBiometrico();
            objEquipamentoVeridis.InstalarLicenca();
        }

        private void InicializarLeitorBiometrico()
        {
            try
            {
                objEventListener = new VeridisEventListenerRegistrar(this);
                objEquipamentoVeridis = new VeridisEquipamento<VeridisEventListenerRegistrar>(objEventListener);
            }
            catch (Exception ex)
            {
                TrataExcecoes("Não foi possível se comunicar com o equipamento.", ex);
            }
        }

        protected async Task ChamaInicializacaoEquipamento()
        {
            await Task.Factory.StartNew(() => InicializarLeitorBiometrico());

            SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.AguardandoBiometria, String.Empty);

            BeginInvoke(new Action(() =>
            {
                objEquipamentoVeridis.IniciarReceptorEquipamento();
            }));
        }

        private void ChamaParadaEquipamento()
        {
            InicializarLeitorBiometrico();

            BeginInvoke(new Action(() =>
            {
                objEquipamentoVeridis.PararReceptorEquipamento();
            }));
        }

        #endregion

        #region Registrar Ponto

        public async Task<RegistraPonto> RegistrarPonto(Configuracao objConfiguracao)
        {
            RegistraPonto objRegistraPonto = null;

            if (sbSituacaoBotaoRegistrar == EnumeraveisUtil.SituacaoBotaoRegistrar.AguardandoBiometria)
            {
                try
                {
                    SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrandoPonto, String.Empty);

                    RegistradorService registradorService = new RegistradorService();
                    List<Biometria> Biometrias = await registradorService.GetBiometriasPorUsuarioSistema();

                    Int32 idFuncionario = BLL.Biometria.GetIDFuncionarioByBiometrias(Biometrias, bioSample);

                    if (idFuncionario > 0)
                    {
                        BilheteBioEnvio objBilhete = new BilheteBioEnvio(idFuncionario, dataHoraBatida, objConfiguracao.Usuario, objConfiguracao.Senha);
                        objRegistraPonto = await Task.Factory.StartNew(() => registradorService.RegistrarPonto(objBilhete)).Result;
                    }
                    else
                    {
                        objRegistraPonto = null;
                        SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, String.Empty);
                        MessageBox.Show("Funcionário não encontrado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    objRegistraPonto = null;
                    SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, String.Empty);
                    throw new Exception("Erro ao registrar a marcação", ex);
                }
            }

            return objRegistraPonto;
        }

        public void ImprimirComprovante(RegistraPonto objRegistraPonto, Configuracao objConfiguracao)
        {
            if (objRegistraPonto != null)
            {
                try
                {
                    ComprovanteRegistro objComprovanteRegistro = new ComprovanteRegistro(objRegistraPonto, objConfiguracao);

                    FormComprovanteRegistro formComprovante = new FormComprovanteRegistro(objComprovanteRegistro);
                    formComprovante.ShowDialog();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao Imprimir o comprovante.", ex);
                }
                finally
                {
                    SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, String.Empty);
                }
            }
        }

        public async Task ValidaTimeOutBotaoRegistrar()
        {
            for (int i = 0; i < 10; i++)
            {
                if (bVerificadorConectividade)
                    return;

                Thread.Sleep(1000);
            }

            if (!bVerificadorConectividade)
            {
                await Task.Factory.StartNew(() => SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, String.Empty));
            }
        }

        #endregion

        #region Problemas de Conectividade

        public async Task<Boolean> ValidarConexao()
        {
            String msg;
            Boolean conectividadeInternet = await ConectividadeUtil.FazVerificacaoInternet();
            Boolean conectividadeWS = await ConectividadeUtil.FazVerificacaoWS();
            if (conectividadeInternet)
            {
                if (conectividadeWS)
                {
                    return true;
                }
                else
                {
                    msg = "Não é possível comunicar com o servidor. Aguarde alguns minutos, caso o problema persista contate o Suporte.";
                    SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.SemConexao, msg);
                }
            }
            else
            {
                msg = "Verifique sua conexão com a Internet! Nâo será possível importar/exportar dados enquanto estiver off-line.";
                SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.SemInternet, msg);
            }
            return false;
        }


        private async Task VerificadorProblemasConectividade()
        {
            Int64 contador = 0;
            String msg = String.Empty;

            while (true)
            {
                if (bVerificadorConectividade)
                {
                    Boolean conectividadeInternet = await ConectividadeUtil.FazVerificacaoInternet();
                    if (conectividadeInternet)
                    {
                        msg = String.Empty;
                        if (contador % 10 == 0)
                        {
                            Boolean conectividadeWS = await ConectividadeUtil.FazVerificacaoWS();
                            if (!conectividadeWS)
                            {
                                msg = "Não é possível comunicar com o servidor. Aguarde alguns minutos, caso o problema persista contate o Suporte.";
                                SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.SemConexao, msg);
                            }
                            else
                            {
                                msg = String.Empty;
                                SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, msg);
                            }
                        }
                        else
                        {
                            msg = String.Empty;
                            SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, msg);
                        }
                    }
                    else
                    {
                        msg = "Verifique sua conexão com a Internet! Nâo será possível importar/exportar dados enquanto estiver off-line.";
                        SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.SemInternet, msg);
                    }

                    contador = VerificaValorMaximoInt64(contador);
                    contador++;
                }

                Thread.Sleep(30000);
            }
        }

        private static long VerificaValorMaximoInt64(Int64 contador)
        {
            if (contador == Int64.MaxValue)
                contador = 0;
            return contador;
        }

        #region Mensagens de Erro

        private void SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar tpSituacaoBotaoRegistrar, String msgToolTip)
        {
            if (sbRegistrarPonto.InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SetarMsgTela(tpSituacaoBotaoRegistrar, msgToolTip);
                }));
            }
            else
            {
                sbSituacaoBotaoRegistrar = tpSituacaoBotaoRegistrar;
                if (FormWindowState.Minimized == this.WindowState)
                {
                    SetarMsgTelaMinimizada(msgToolTip, notifyIcon1);
                }
                else
                {
                    SetarMsgTelaMaximizada(msgToolTip, notifyIcon1);
                }
            }
        }

        private void SetarMsgTelaMaximizada(string msgToolTip, NotifyIcon notifyIconComponenteErro)
        {
            try
            {
                switch (sbSituacaoBotaoRegistrar)
                {
                    case EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto:
                        sbRegistrarPonto.Text = "Registrar Ponto";
                        sbRegistrarPonto.ToolTip = msgToolTip;

                        sbRegistrarPonto.Click += sbRegistrarPonto_Click;
                        bVerificadorConectividade = true;

                        if (!bConectividade)
                            InicializarDataHora();

                        bConectividade = true;
                        break;
                    case EnumeraveisUtil.SituacaoBotaoRegistrar.AguardandoBiometria:
                        sbRegistrarPonto.Text = "Aguardando Biometria";
                        sbRegistrarPonto.ToolTip = msgToolTip;

                        RemoveClickEvent(sbRegistrarPonto);
                        bVerificadorConectividade = false;

                        bConectividade = true;
                        break;
                    case EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrandoPonto:
                        sbRegistrarPonto.Text = "Registrando Ponto";
                        sbRegistrarPonto.ToolTip = msgToolTip;

                        RemoveClickEvent(sbRegistrarPonto);
                        bVerificadorConectividade = false;

                        bConectividade = true;
                        break;
                    case EnumeraveisUtil.SituacaoBotaoRegistrar.SemInternet:
                        sbRegistrarPonto.Text = "Sem Internet";
                        sbRegistrarPonto.ToolTip = msgToolTip;

                        RemoveClickEvent(sbRegistrarPonto);
                        bVerificadorConectividade = true;

                        bConectividade = false;
                        break;
                    case EnumeraveisUtil.SituacaoBotaoRegistrar.SemConexao:
                        sbRegistrarPonto.Text = "Sem Conexão";
                        sbRegistrarPonto.ToolTip = msgToolTip;

                        RemoveClickEvent(sbRegistrarPonto);
                        bVerificadorConectividade = true;

                        bConectividade = false;
                        break;
                }
                notifyIconComponenteErro.Visible = false;
            }
            catch (Exception ex)
            {
                TrataExcecoes(ex);
            }
        }

        private void SetarMsgTelaMinimizada(string msg, NotifyIcon notifyIconComponenteErro)
        {
            if (sbSituacaoBotaoRegistrar != EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto)
            {
                notifyIconComponenteErro.BalloonTipTitle = "Pontofopag Registrador";
                notifyIconComponenteErro.BalloonTipText = msg;
                notifyIconComponenteErro.Visible = true;
                notifyIconComponenteErro.ShowBalloonTip(5000);
            }
        }

        #endregion

        #endregion

        #region Eventos Form

        private void menuItemFechar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void menuItemRestaurar_Click(object sender, EventArgs e)
        {
            MaximizarTela();
        }

        private void FormRegistraPonto_Resize(object sender, EventArgs e)
        {
            notifyIcon1.Text = "Registrador Pontofopag";
            notifyIcon1.BalloonTipTitle = "Registrador Pontofopag";
            notifyIcon1.BalloonTipText = @"O aplicativo foi minimizado para a barra de tarefas. Para restaurar, dê um clique duplo sobre este ícone.";
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

        private void FormRegistraPonto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Deseja encerrar o Registrador Pontofopag?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                    System.Windows.Forms.DialogResult.Yes)
                {
                    EncerrarAplicativo();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void FormRegistraPonto_Load(object sender, EventArgs e)
        {
            try
            {
                objTimer.Interval = 1000;
                objTimer.Tick += new EventHandler(TickRelogio);
                objTimer.Start();

                InicializarDataHora();
            }
            catch (Exception ex)
            {
                TrataExcecoes(ex);
            }
        }

        private void FormRegistraPonto_Shown(object sender, EventArgs e)
        {
            if (!bwJobs.IsBusy)
            {
                bwJobs.RunWorkerAsync();
            }
        }

        #endregion

        #region Eventos NotifyIcon1

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MaximizarTela();
        }

        #endregion

        #region Eventos variados

        private async void btnConfiguracao_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean conectado = await ValidarConexao();

                if (conectado)
                {
                    FormLoginConfiguracao form = new FormLoginConfiguracao();
                    form.ShowDialog();
                    if (form.UsuarioLogado)
                    {
                        FormConfiguracao formConfig = new FormConfiguracao();
                        formConfig.ShowDialog();

                        await Task.Factory.StartNew(() => InicializarDataHora());
                        InicializarLeitorBiometrico();
                    }
                }
            }
            catch (Exception ex)
            {
                TrataExcecoes(ex);
            }
        }

        private async void sbRegistrarPonto_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean conectado = await ValidarConexao();
                if (conectado)
                {
                    Configuracao objConfiguracao = Configuracao.AbrirConfiguracoes();

                    await ChamaInicializacaoEquipamento();
                    await AtualizaDataHora(objConfiguracao);
                    await Task.Factory.StartNew(() => ValidaTimeOutBotaoRegistrar());
                }
            }
            catch (Exception ex)
            {
                SetarMsgTela(EnumeraveisUtil.SituacaoBotaoRegistrar.RegistrarPonto, String.Empty);
                TrataExcecoes(ex);
            }
        }

        private void bwJobs_DoWork(object sender, DoWorkEventArgs e)
        {
            Task.Factory.StartNew(async () => await VerificadorProblemasConectividade()).ConfigureAwait(false);
        }

        #endregion


    }
}
