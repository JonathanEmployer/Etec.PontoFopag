using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Modelo;
using RegistradorBiometrico.Integracao.Veridis.EventListener.Base;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Service;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View.Base;
using RegistradorBiometrico.ViewModel;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Veridis.Biometric;

namespace RegistradorBiometrico.View
{
    public partial class FormConfiguracao : FormBase
    {
        #region Atributos
        private Configuracao objConfiguracao { get; set; }
        private Funcionario objFuncionario { get; set; }

        #endregion

        public FormConfiguracao()
        {
            InitializeComponent();
            PreencherComboFusoHorario();
            InicializaObjetos();
        }

        #region Métodos

        private void InicializaObjetos()
        {
            txtCPF.Text = lblNome.Text = lblBio1.Text = lblBio2.Text = String.Empty;
            txtCPF.Focus();
            objConfiguracao = Configuracao.AbrirConfiguracoes();
            objFuncionario = new Funcionario();
        }

        #region Fuso Horário

        private void PreencherComboFusoHorario()
        {
            ComboBoxItemCollection coll = cbUTC.Properties.Items;
            coll.BeginUpdate();
            try
            {
                coll.AddRange(Fusos.FusosSistema);
            }
            finally
            {
                coll.EndUpdate();
            }
        } 

        #endregion

        #region Biometrias

        private async void VerificarCpfBiometria()
        {
            try
            {
                String cpf = txtCPF.Text;
                HabilitaOuDesabilitaCampos("Aguarde", false);

                if (!String.IsNullOrEmpty(cpf))
                {
                    RegistradorService objRegistradorService = new RegistradorService();
                    objFuncionario = await Task.Factory.StartNew(() => objRegistradorService.GetListaBiometriasPorCPF(cpf)).Result;

                    BeginInvoke(new Action(() =>
                    {
                        lblNome.Text = objFuncionario.Nome;
                        if (objFuncionario.Biometrias != null)
                        {
                            Biometria primBiometria = objFuncionario.Biometrias.ElementAtOrDefault(0);
                            if (primBiometria != null)
                            {
                                lblBio1.Text = "Cadastrado";
                                lblBio1.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            { 
                                lblBio1.Text = "Não cadastrado";
                                lblBio1.ForeColor = System.Drawing.Color.Red;
                            }
                            Biometria secBiometria = objFuncionario.Biometrias.ElementAtOrDefault(1);
                            if (secBiometria != null)
                            {
                                lblBio2.Text = "Cadastrado";
                                lblBio2.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            { 
                                lblBio2.Text = "Não cadastrado";
                                lblBio2.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        else
                        {
                            lblBio1.Text = "Não cadastrado";
                            lblBio1.ForeColor = System.Drawing.Color.Red;
                            lblBio2.Text = "Não cadastrado";
                            lblBio2.ForeColor = System.Drawing.Color.Red;
                        }
                    }));
                }
                else
                {
                    MessageBox.Show("Nenhum funcionário selecionado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                TrataExcecoes(ex);
                BeginInvoke(new Action(() =>
                {
                    InicializaObjetos();
                }));
            }
            finally
            {
                HabilitaOuDesabilitaCampos("Verificar CPFs e Biometrias", true);
            }
        }



        private void CadastrarBiometria(Int32 indice, LabelControl label)
        {
            try
            {
                FormCapturaBiometria form = new FormCapturaBiometria(objFuncionario);
                form.ShowDialog();

                if (form.Biometria != null)
                {
                    Byte[] valorBiometria = form.Biometria;

                    if (objFuncionario.Biometrias.ElementAtOrDefault(indice) == null)
                    {
                        Modelo.Biometria objBiometria = new Biometria(valorBiometria, objFuncionario.Id);
                        objFuncionario.Biometrias.Add(objBiometria);
                    }
                    else
                    {
                        objFuncionario.Biometrias[indice].valorBiometria = valorBiometria;
                    }

                    label.Text = String.IsNullOrEmpty(label.Text) ? "Cadastrado" : "Alterado";
                    label.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                TrataExcecoes("Erro ao enviar as biometrias", ex);
            }
        }

        private async Task<Boolean> EnviaBiometria(Funcionario objFuncionario)
        {
            Boolean retorno = false;
            try
            {
                if ((objFuncionario != null) && (objFuncionario.Id > 0))
                {
                    RegistradorService objRegistradorService = new RegistradorService();

                    retorno = await Task.Factory.StartNew((() => objRegistradorService.CadastrarBiometria(objFuncionario))).Result;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Erro ao enviar as Biometria/s para o funcionário {0}", objFuncionario.Nome), ex);
            }
        }

        private async Task SalvarBiometrias()
        {
            if ((objFuncionario != null) && (objFuncionario.Id > 0))
            {
                try
                {
                    HabilitaOuDesabilitaCampos("Aguarde", false);

                    await EnviaBiometria(objFuncionario);
                    MessageBox.Show(String.Format("Biometria/s para o funcionário {0} enviada/s", objFuncionario.Nome), "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    TrataExcecoes(ex);
                }
                finally
                {
                    InicializaObjetos();
                    HabilitaOuDesabilitaCampos("Verificar CPFs e Biometrias", true);
                }
            }
        } 

        #endregion

        #region Métodos Tela

        public void HabilitaOuDesabilitaCampos(String mensagemButton, Boolean bHabilitado)
        {
            BeginInvoke(new Action(() =>
            {
                btnVerificaCpfBiometria.Text = mensagemButton;
                base.HabilitaOuDesabilitaCampos(this, bHabilitado);
            }));
        } 

        #endregion

        #endregion

        #region Eventos

        private void btnCadastrarBiometria1_Click(object sender, System.EventArgs e)
        {
            if (objFuncionario != null && objFuncionario.Id > 0)
            {
                CadastrarBiometria(0, lblBio1);
            }
            else
            {
                MessageBox.Show("Nenhum funcionário selecionado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCadastrarBiometria2_Click(object sender, System.EventArgs e)
        {
            if (objFuncionario != null && objFuncionario.Id > 0)
            {
                CadastrarBiometria(1, lblBio2);
            }
            else
            {
                MessageBox.Show("Nenhum funcionário selecionado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnVerificaCpfBiometria_Click(object sender, System.EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() => VerificarCpfBiometria());
            }
            catch (Exception ex)
            {
                TrataExcecoes(ex.Message, ex);
            }
        }

        private async void btnSalvar_Click(object sender, System.EventArgs e)
        {
            try
            {
                objConfiguracao.FusoHorario = cbUTC.SelectedItem == null ? String.Empty : ((Fusos)cbUTC.SelectedItem).ID;
                objConfiguracao.Local = txtLocal.Text;
                Configuracao.SalvarConfiguracoes(objConfiguracao);

                await SalvarBiometrias();

                FecharTela();
            }
            catch (Exception ex)
            {
                TrataExcecoes("Erro ao salvar a configuração", ex);
            }
        }

        private void btnCancelar_Click(object sender, System.EventArgs e)
        {
            FecharTela();
        }

        private void FormConfiguracao_Shown(object sender, EventArgs e)
        {
            Fusos objFusos = Fusos.FusosSistema.FirstOrDefault(s => s.ID == objConfiguracao.FusoHorario);
            cbUTC.SelectedIndex = (objFusos == null) ? -1 : objFusos.Indice;
            txtLocal.Text = objConfiguracao.Local;
        }

        private async void btnSalvarBiometria_Click(object sender, EventArgs e)
        {
            await SalvarBiometrias();
        }


        #endregion

    }
}
