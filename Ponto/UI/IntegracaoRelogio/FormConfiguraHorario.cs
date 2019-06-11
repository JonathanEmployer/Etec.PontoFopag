using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections;

namespace UI.IntegracaoRelogio
{
    public partial class FormConfiguraHorario : Form
    {
        private Modelo.REP objRep = null;
        protected BLL.Empresa bllEmpresa;
        protected BLL.REP bllREP;
        public List<string> TelasAbertas { get; set; }

        public FormConfiguraHorario()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllREP = new BLL.REP();
            this.Name = "FormConfiguraHorario";
            cbIdRep.Properties.DataSource = bllREP.GetAll();
            gcDataHoraRelogio.Enabled = false;
            gcHorarioVerao.Enabled = false;
        }

        protected virtual void GridSelecao<T>(Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {
            UI.Util.Funcoes.ChamaGridSelecao(pGrid);
            if (pGrid.cwAtualiza == true)
            {
                pCb.Properties.DataSource = bll.GetAll();
            }
            if (pGrid.cwRetorno != 0)
            {
                pCb.EditValue = pGrid.cwRetorno;
            }
            pCb.Focus();
        }

        private void FormImportacaoBilhetes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    if (sbExportar.Enabled == true)
                    {
                        sbExportar.Focus();
                        sbExportar_Click(sender, e);
                    }
                    break;
                case Keys.Enter:
                    if (sbExportar.Enabled == true)
                    {
                        sbExportar.Focus();
                        sbExportar_Click(sender, e);
                    }
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbCancelar.Focus();
                        sbCancelar_Click(sender, e);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Importação de Bilhetes");
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
            }
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormEnviarFuncionarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void sbExportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaDados() && MessageBox.Show("Deseja enviar as configurações de data e hora para o relógio?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sbCancelar.Enabled = false;
                    sbExportar.Enabled = false;
                    bool exibirLog = false;

                    var configuracao = new BLL.IntegracaoRelogio.ConfiguracaoHorario((int)cbIdRep.EditValue, null, cwkControleUsuario.Facade.getUsuarioLogado);
                    configuracao.SetDataHoraAtual((DateTime?)txtDataHoraAtual.EditValue);
                    configuracao.SetHorarioVerao((DateTime?)txtInicioHorarioVerao.EditValue, (DateTime?)txtTerminoHorarioVerao.EditValue);
                    configuracao.EnviarDataHoraComputador = chbDataHoraComputador.Checked;
                    if (configuracao.Enviar(out exibirLog))
                    {
                        MessageBox.Show("Envio efetuado com sucesso!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else if (exibirLog && MessageBox.Show("Ocorreram erros ao configurar o horário do relógio. Deseja visualizar o arquivo de log?", "Mensagem", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(configuracao.ARQUIVO_LOG);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível abrir o arquivo de log:\n" + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreram erros ao realizar o envio: " + Environment.NewLine + ex.Message, "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sbCancelar.Enabled = true;
                sbExportar.Enabled = true;
            }
        }

        private bool ValidaDados()
        {
            dxErrorProvider1.ClearErrors();
            StringBuilder str = new StringBuilder();

            if (!chbEnviarDataEHoraAtual.Checked && !chbEnviarHorarioDeVerao.Checked)
            {
                str.AppendLine("Opções: Selecione pelo menos uma opção de envio.");
            }

            if ((int)cbIdRep.EditValue == 0)
            {
                dxErrorProvider1.SetError(cbIdRep, "Campo obrigatóio.");
                str.AppendLine("Relógio: Selecione o relógio que receberá os dados.");
            }
            else
            {
                Modelo.REP objRep = bllREP.LoadObject((int)cbIdRep.EditValue);
                if (objRep.Relogio <= 0)
                {
                    dxErrorProvider1.SetError(cbIdRep, "O Relógio não está configurado para realizar esta operação.");
                    str.AppendLine("Relógio: Configure os parâmetros de comunicação antes de realizar esta operação.");
                }
                else
                    dxErrorProvider1.SetError(cbIdRep, String.Empty);
            }

            if (chbEnviarDataEHoraAtual.Checked && !chbDataHoraComputador.Checked)
            {
                if (txtDataHoraAtual.EditValue == null)
                {
                    dxErrorProvider1.SetError(txtDataHoraAtual, "Campo obrigatóio.");
                    str.AppendLine("Data e Hora Atual: Selecione a data e a hora atual.");
                }
            }

            if (chbEnviarHorarioDeVerao.Checked)
            {
                if (txtInicioHorarioVerao.EditValue == null)
                {
                    dxErrorProvider1.SetError(txtInicioHorarioVerao, "Campo obrigatóio.");
                    str.AppendLine("Início: Selecione a data e a hora de início do horário de verão.");
                }

                if (txtTerminoHorarioVerao.EditValue == null)
                {
                    dxErrorProvider1.SetError(txtTerminoHorarioVerao, "Campo obrigatóio.");
                    str.AppendLine("Término: Selecione a data e a hora de término do horário de verão.");
                }
            }

            if (str.Length > 0)
            {
                MessageBox.Show("Preencha os campos corretamente:" + Environment.NewLine + str.ToString(), "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }

        private void sbIdRep_Click(object sender, EventArgs e)
        {
            FormGridREP formRep = new FormGridREP();
            formRep.cwTabela = "REP";
            formRep.cwId = (int)cbIdRep.EditValue;
            GridSelecao(formRep, cbIdRep, bllREP);
        }

        private void chbEnviarHorarioDeVerao_CheckedChanged(object sender, EventArgs e)
        {
            if (chbEnviarHorarioDeVerao.Checked)
                gcHorarioVerao.Enabled = true;
            else
            {
                txtInicioHorarioVerao.EditValue = null;
                txtTerminoHorarioVerao.EditValue = null;
                gcHorarioVerao.Enabled = false;
            }
        }

        private void chbEnviarDataEHoraAtual_CheckedChanged(object sender, EventArgs e)
        {
            if (chbEnviarDataEHoraAtual.Checked)
            {
                gcDataHoraRelogio.Enabled = true;
            }
            else
            {
                chbDataHoraComputador.Checked = false;
                txtDataHoraAtual.EditValue = null;
                gcDataHoraRelogio.Enabled = false;
            }
            VerificarConfiguracoesRelogio();
        }

        private void chbDataHoraComputador_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDataHoraComputador.Checked)
            {
                txtDataHoraAtual.Enabled = false;
                txtDataHoraAtual.EditValue = null;
            }
            else
                txtDataHoraAtual.Enabled = true;
        }

        private void cbIdRep_EditValueChanged(object sender, EventArgs e)
        {
            var idRep = (int)cbIdRep.EditValue;
            if (idRep > 0)
            {
                objRep = bllREP.LoadObject(idRep);
            }
            else
                objRep = null;
            //VerificarConfiguracoesRelogio();
        }

        private void VerificarConfiguracoesRelogio()
        {
            //TopData
            if (objRep != null && (objRep.Relogio == 1 || objRep.Relogio == 21))
            {
                chbEnviarHorarioDeVerao.Checked = false;
                chbEnviarHorarioDeVerao.Enabled = false;

                chbEnviarDataEHoraAtual.Checked = true;
                chbDataHoraComputador.Checked = true;
                chbDataHoraComputador.Enabled = false;
                chbEnviarDataEHoraAtual.Enabled = false;
            }
            else
            {
                chbEnviarHorarioDeVerao.Enabled = true;
                chbEnviarDataEHoraAtual.Enabled = true;
                chbDataHoraComputador.Enabled = true;
            }
        }
    }
}
