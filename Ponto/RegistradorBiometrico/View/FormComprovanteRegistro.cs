using DevExpress.XtraReports.UI;
using RegistradorBiometrico.Integracao.Veridis;
using RegistradorBiometrico.Integracao.Veridis.EventListener;
using RegistradorBiometrico.Integracao.Veridis.EventListener.Base;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View.Base;
using RegistradorBiometrico.ViewModel;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


namespace RegistradorBiometrico.View
{
    public partial class FormComprovanteRegistro : FormBase
    {
        #region Propriedades

        private ComprovanteRegistro ComprovanteRegistro;

        private VeridisEquipamento<VeridisEventListenerBase> objEquipamentoVeridis;
        private VeridisEventListenerBase objEventListener;

        #endregion

        public FormComprovanteRegistro(ComprovanteRegistro pComprovanteRegistro)
        {
            InitializeComponent();
            InicializarLeitorBiometrico();

            ComprovanteRegistro = pComprovanteRegistro;

            lblRazaoSocial.Text = ComprovanteRegistro.RazaoSocial;
            lblLocal.Text = ComprovanteRegistro.Local;
            lblCNPJ.Text = ComprovanteRegistro.CNPJ;
            lblNome.Text = ComprovanteRegistro.Nome;
            lblPIS.Text = ComprovanteRegistro.PIS;
            lblData.Text = ComprovanteRegistro.Data;
            lblHora.Text = ComprovanteRegistro.Hora;
            lblNSR.Text = ComprovanteRegistro.NSR;
            lblChave.Text = ComprovanteRegistro.Chave;
        }

        private void InicializarLeitorBiometrico()
        {
            try
            {
                objEventListener = new VeridisEventListenerBase(this);
                objEquipamentoVeridis = new VeridisEquipamento<VeridisEventListenerBase>(objEventListener);
            }
            catch (Exception ex)
            {
                TrataExcecoes("Não foi possível se comunicar com o equipamento.", ex);
            }
        }

        #region Equipamento

        private void ChamaInicializacaoEquipamento()
        {
            InicializarLeitorBiometrico();
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

        #region Eventos

        private void btnEnviarPorEmail_Click(object sender, EventArgs e)
        {
            FormEnviarEmailComprovante form = new FormEnviarEmailComprovante(ComprovanteRegistro);
            form.ShowDialog();
        }

        private void btnSalvarEmPDF_Click(object sender, EventArgs e)
        {
            String caminhoArquivo = Path.Combine(VariaveisGlobais.diretorioAplicativo, "Comprovantes");

            String nomeArquivo = String.Concat("Comprovante_",
                                               ComprovanteRegistro.Nome.Replace(' ', '_') + "_",
                                               ComprovanteRegistro.Data.Replace('/', '_') + "_",
                                               ComprovanteRegistro.Hora.Replace(':', '_'), 
                                               ".pdf");

            ImpressaoComprovante.CriarArquivoPDF(caminhoArquivo, nomeArquivo, ComprovanteRegistro);
        }

       

        private void btnSalvarComoImagem_Click(object sender, EventArgs e)
        {
            String caminhoArquivo = Path.Combine(VariaveisGlobais.diretorioAplicativo, "Comprovantes");

            String nomeArquivo = String.Concat("Comprovante_", 
                                               ComprovanteRegistro.Nome.Replace(' ', '_') + "_",
                                               ComprovanteRegistro.Data.Replace('/', '_') + "_",
                                               ComprovanteRegistro.Hora.Replace(':', '_'), 
                                               ".png");

            ImpressaoComprovante.CriarArquivoPNG(caminhoArquivo, nomeArquivo, ComprovanteRegistro);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void FormComprovanteRegistro_Shown(object sender, EventArgs e)
        {
            ChamaInicializacaoEquipamento();
        }

        private void FormComprovanteRegistro_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChamaParadaEquipamento();
        }

        #endregion

    }
}
