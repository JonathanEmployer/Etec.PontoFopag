using Modelo;
using RegistradorBiometrico.Integracao.Veridis;
using RegistradorBiometrico.Integracao.Veridis.EventListener;
using RegistradorBiometrico.Service;
using RegistradorBiometrico.View.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Veridis.Biometric;

namespace RegistradorBiometrico.View
{
    public partial class FormCapturaBiometria : FormBase
    {
        private bool estaConsolidando;
        private BiometricSampleMerge mergedImage; // Imagem a ser retornada e salva no banco, consolidada com 3 ou mais imagens distintas do mesmo dedo
        private int toCapture;

        public Byte[] Biometria { get; private set; }
        private Funcionario objFuncionario { get; set; }

        private VeridisEquipamento<VeridisEventListenerCapturar> objEquipamentoVeridis;
        private VeridisEventListenerCapturar objEventListener;

        public FormCapturaBiometria(Funcionario pObjFuncionario)
        {
            InitializeComponent();

            this.estaConsolidando = true;
            toCapture = 3;

            objFuncionario = pObjFuncionario;
        }

        private void InicializarLeitorBiometrico()
        {
            try
            {
                objEventListener = new VeridisEventListenerCapturar(this);
                objEquipamentoVeridis = new VeridisEquipamento<VeridisEventListenerCapturar>(objEventListener);
            }
            catch (Exception ex)
            {
                TrataExcecoes("Não foi possível se comunicar com o equipamento.", ex);
            }
        }

        #region Imagem

        public void SetImage(Bitmap value)
        {
            try
            {
                if (toCapture > 0)
                {
                    if (!estaConsolidando)
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new Action<Bitmap>(SetImage), new object[] { value });
                            return;
                        }
                        this.pbFinger.Image = value;
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new Action<Bitmap>(SetImage), new object[] { value });
                            return;
                        }
                        this.pbFinger.Image = value;
                        this.AddNewImage(this.bioSample);

                        if (toCapture == 0)
                        {
                            MessageBox.Show("Conjunto de biometrias formado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sbInserir.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível capturar a Biometria", ex);
            }
        }

        public void InicializaObjetos()
        {
            this.pbFinger.Image = null;
            this.toCapture = 3;

            cbConsolidacao1.Checked = false;
            cbConsolidacao2.Checked = false;
            cbConsolidacao3.Checked = false;

            this.mergedImage = new BiometricSampleMerge();

            sbCancelar.Enabled = true;
        }

        public void AddNewImage(BiometricSample sample)
        {
            if (sample != null)
            {
                this.toCapture--;
                this.SetCheckBoxes(toCapture);
                this.mergedImage.Add(sample); // Este mátodo adiciona uma nova imagem à consolidação.
            }
        }

        private void SetCheckBoxes(int indiceCheckBox)
        {
            switch (indiceCheckBox)
            {
                case 2:
                    cbConsolidacao1.Checked = true;
                    break;
                case 1:
                    cbConsolidacao2.Checked = true;
                    break;
                case 0:
                    cbConsolidacao3.Checked = true;
                    break;
            }
        }

        #endregion

        #region Equipamento

        private void ChamaInicializacaoEquipamento()
        {
            InicializarLeitorBiometrico();
            BeginInvoke(new Action(() =>
            {
                objEquipamentoVeridis.IniciarReceptorEquipamento();

                this.mergedImage = new BiometricSampleMerge();
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

        private async void sbInserir_Click(object sender, EventArgs e)
        {
            try
            {
                sbInserir.Enabled = false;
                sbCancelar.Enabled = false;

                BiometricTemplate bioTemplateMerge = mergedImage.GetTemplate();
                RegistradorService registradorService = new RegistradorService();

                if (bioTemplateMerge != null && bioTemplateMerge.Buffer != null)
                {
                    List<Biometria> Biometrias = await registradorService.GetBiometriasPorUsuarioSistema();
                    Int32 idFuncionario = BLL.Biometria.GetIDFuncionarioByBiometrias(Biometrias, bioTemplateMerge);

                    if ((idFuncionario <= 0) || ((idFuncionario > 0) && (idFuncionario == objFuncionario.Id)))
                    {
                        Biometria = bioTemplateMerge.Buffer;
                        FecharTela();
                    }
                    else
                    {
                        MessageBox.Show("Conjunto de biometrias já relacionados com outro funcionário", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        InicializaObjetos();
                    }
                }
                else
                {
                    MessageBox.Show("Nenhuma biometria selecionada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                TrataExcecoes(ex);
            }
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            Biometria = null;
            FecharTela();
        }

        private void FormCapturaBiometria_Shown(object sender, EventArgs e)
        {
            sbInserir.Enabled = false;

            InicializarLeitorBiometrico();
            ChamaInicializacaoEquipamento();
        }

        private void FormCapturaBiometria_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChamaParadaEquipamento();
        }

        #endregion
    }
}
