using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;


namespace UI.Relatorios
{
    public partial class FormRelatorioOcorrenciaold : Form
    {
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.Ocorrencia bllOcorrencia;
        private bool[] pegaOcorrencias = new bool[6];
        private int idTipo = -1;

        public List<string> TelasAbertas { get; set; }

        public FormRelatorioOcorrenciaold()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncionario = new BLL.Funcionario();
            bllOcorrencia = new BLL.Ocorrencia();
            this.Name = "FormRelatorioOcorrencia";
        }

        private void ckOcorrencia_CheckedChanged(object sender, EventArgs e)
        {
            comboOcorrencia.Properties.DataSource = bllOcorrencia.GetAllComOpcaoTodos();

            if (ckOcorrencia.Checked)
                comboOcorrencia.Enabled = true;
            else
            {
                comboOcorrencia.Enabled = false;
                comboOcorrencia.Properties.DataSource = null;
            }
        }

        private void rgTipoRelatorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboEmpresa.EditValue = 0;
            comboDepartamento.EditValue = 0;
            comboFuncionario.EditValue = 0;
            switch (rgTipoRelatorio.SelectedIndex)
            {
                case 0: //Por Empresa
                    comboEmpresa.Enabled = true;
                    btEmpresa.Enabled = true;
                    comboDepartamento.Enabled = false;
                    btDepartamento.Enabled = false;
                    comboFuncionario.Enabled = false;
                    btFuncionario.Enabled = false;
                    comboEmpresa.Properties.DataSource = bllEmpresa.GetAll();
                    break;
                case 1: //Por Departamento
                    comboEmpresa.Enabled = true;
                    btEmpresa.Enabled = true;
                    comboDepartamento.Enabled = true;
                    btDepartamento.Enabled = true;
                    comboFuncionario.Enabled = false;
                    btFuncionario.Enabled = false;
                    comboEmpresa.Properties.DataSource = bllEmpresa.GetAll();
                    comboDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)comboEmpresa.EditValue);
                    break;
                case 2://Por Funcionario
                    comboEmpresa.Enabled = false;
                    btEmpresa.Enabled = false;
                    comboDepartamento.Enabled = false;
                    btDepartamento.Enabled = false;
                    comboFuncionario.Enabled = true;
                    btFuncionario.Enabled = true;                    
                    comboFuncionario.Properties.DataSource = bllFuncionario.GetAll();
                    break;
                default:
                    comboEmpresa.Enabled = false;
                    btEmpresa.Enabled = false;
                    comboDepartamento.Enabled = false;
                    btDepartamento.Enabled = false;
                    comboFuncionario.Enabled = false;
                    btFuncionario.Enabled = false;
                    break;
            }
        }

        private void btEmpresa_Click(object sender, EventArgs e)
        {
            UI.FormGridEmpresa form = new UI.FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)comboEmpresa.EditValue;
            GridSelecao(form, comboEmpresa, bllEmpresa);
        }

        private void btDepartamento_Click(object sender, EventArgs e)
        {
            UI.FormGridDepartamento form = new UI.FormGridDepartamento();
            form.cwTabela = "Departamento";
            form.cwId = (int)comboDepartamento.EditValue;
            GridSelecao(form, comboDepartamento, bllDepartamento);
        }

        private void btFuncionario_Click(object sender, EventArgs e)
        {
            UI.FormGridFuncionario form = new UI.FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = (int)comboFuncionario.EditValue;
            GridSelecao(form, comboFuncionario, bllFuncionario);
        }

        private void btOcorrencia_Click(object sender, EventArgs e)
        {
            UI.FormGridOcorrencia form = new UI.FormGridOcorrencia();
            form.cwTabela = "Ocorrência";
            form.cwId = (int)comboOcorrencia.EditValue;
            GridSelecao(form, comboOcorrencia, bllOcorrencia);
        }

        private void btCancela_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if (rgTipoRelatorio.SelectedIndex == 1 || rgTipoRelatorio.SelectedIndex == 2)
            {
                if ((int)comboEmpresa.EditValue > 0)
                {
                    comboDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)comboEmpresa.EditValue);
                    comboDepartamento.EditValue = 0;
                    comboDepartamento.Enabled = true;
                    comboDepartamento.Enabled = true;
                }
                else if ((int)comboEmpresa.EditValue == 0)
                {
                    comboDepartamento.Properties.DataSource = bllDepartamento.GetAll();
                    comboDepartamento.EditValue = 0;
                    comboDepartamento.Enabled = false;
                    comboDepartamento.Enabled = false;
                }
            }
        }

        private void comboDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)comboDepartamento.EditValue > 0 && rgTipoRelatorio.SelectedIndex == 2)
            {
                comboFuncionario.Properties.DataSource = bllFuncionario.GetPorDepartamento((int)comboEmpresa.EditValue, (int)comboDepartamento.EditValue, false);
            }
        }

        private void GridSelecao<T>(UI.Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
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

        private void btImprime_Click(object sender, EventArgs e)
        {
            //btImprime.Enabled = false;
            //btCancela.Enabled = false;
            //preencheDados();
            //if (pegaOcorrencias.Any(o => o == true))
            //{
            //    if (ValidaCampos())
            //    {
            //        UI.FormProgressBar pbRelatorioOcorrencia = new UI.FormProgressBar();
            //        //pbRelatorioOcorrencia.Show();
            //        try
            //        {
            //            bllRelatorioOcorrencia = new BLL.RelatorioOcorrencia(dataInicial.DateTime, dataFinal.DateTime, rgTipoRelatorio.SelectedIndex, idTipo, (int)comboOcorrencia.EditValue, rgOrdena.SelectedIndex, pegaOcorrencias, pbRelatorioOcorrencia.progressBar);

            //            DataTable dt = bllRelatorioOcorrencia.GeraRelatorio();

            //            List<Microsoft.Reporting.WinForms.ReportParameter> parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            //            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", dataInicial.DateTime.ToShortDateString());
            //            parametros.Add(p1);
            //            Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", dataFinal.DateTime.ToShortDateString());
            //            parametros.Add(p2);

            //            string nomerel, texto;
            //            if (rgOrdena.SelectedIndex == 0)
            //            {
            //                nomerel = "rptOcorrenciaPorDataFuncionario.rdlc";
            //                texto = "Relatório de Ocorrências por Data/Funcionário";
            //            }
            //            else
            //            {
            //                nomerel = "rptOcorrenciaPorFuncionarioData.rdlc";
            //                texto = "Relatório de Ocorrências por Funcionário/Data";
            //            }

            //            string ds = "dsOcorrencia_marcacao";

            //            UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, dt, parametros);
            //            form.Text = texto;
            //            form.Show();
            //            this.Close();
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //        finally
            //        {
            //            //pbRelatorioOcorrencia.Close();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Preencha os campos corretamente.");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Selecione pelo menos um tipo de ocorrência.");
            //}
            //btImprime.Enabled = true;
            //btCancela.Enabled = true;
        }

        //Na hora que vai imprimir preenche a lista de coisas a serem pegadas da ocorrencia
        private void preencheDados()
        {
            pegaOcorrencias[0] = ckEntrAtrasada.Checked;
            pegaOcorrencias[1] = ckSaidaAntecipada.Checked;
            pegaOcorrencias[2] = ckFalta.Checked;
            pegaOcorrencias[3] = ckDebitoBH.Checked;
            pegaOcorrencias[4] = ckOcorrencia.Checked;
            pegaOcorrencias[5] = ckMarcIncorretas.Checked;

            switch (rgTipoRelatorio.SelectedIndex)
            {
                case 0: idTipo = (int)comboEmpresa.EditValue; break;
                case 1: idTipo = (int)comboDepartamento.EditValue; break;
                case 2: idTipo = (int)comboFuncionario.EditValue; break;
                default: break;
            }
        }

        private bool ValidaCampos()
        {

            bool ret = true;

            if ((int)comboOcorrencia.EditValue == -1 && ckOcorrencia.Checked)
            {
                dxErrorProvider1.SetError(comboOcorrencia, "Escolha o tipo de ocorrencia.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(comboOcorrencia, "");
            }

            if ((int)rgTipoRelatorio.SelectedIndex > -1)
                dxErrorProvider1.SetError(rgTipoRelatorio, "");
            else
            {
                dxErrorProvider1.SetError(rgTipoRelatorio, "Escolha o tipo do relatório.");
                ret = false;
            }

            if ((int)rgOrdena.SelectedIndex > -1)
                dxErrorProvider1.SetError(rgOrdena, "");
            else
            {
                dxErrorProvider1.SetError(rgOrdena, "Escolha o tipo de ordenação.");
                ret = false;
            }


            if (comboEmpresa.Enabled)
            {
                if ((int)comboEmpresa.EditValue > 0)
                    dxErrorProvider1.SetError(comboEmpresa, "");
                else
                {
                    dxErrorProvider1.SetError(comboEmpresa, "Escolha a empresa.");
                    ret = false;
                }
            }

            if (comboDepartamento.Enabled)
            {
                if ((int)comboDepartamento.EditValue > 0)
                    dxErrorProvider1.SetError(comboDepartamento, "");
                else
                {
                    dxErrorProvider1.SetError(comboDepartamento, "Escolha o departamento.");
                    ret = false;
                }
            }

            if (comboFuncionario.Enabled)
            {
                if ((int)comboFuncionario.EditValue > 0)
                    dxErrorProvider1.SetError(comboFuncionario, "");
                else
                {
                    dxErrorProvider1.SetError(comboFuncionario, "Escolha o funcionario.");
                    ret = false;
                }
            }

            if (dataInicial.DateTime != new DateTime() && dataFinal.DateTime != new DateTime())
            {
                dxErrorProvider1.SetError(dataInicial, "");
                if (dataInicial.DateTime > dataFinal.DateTime)
                {
                    dxErrorProvider1.SetError(dataFinal, "A data final deve ser maior ou igual a data inicial.");
                    ret = false;
                }
                else
                {
                    dxErrorProvider1.SetError(dataFinal, "");
                }
            }
            else
            {
                if (dataInicial.DateTime != new DateTime())
                {
                    dxErrorProvider1.SetError(dataInicial, "");
                }
                else
                {
                    dxErrorProvider1.SetError(dataInicial, "Selecione a data inicial.");
                    ret = false;
                }


                if (dataFinal.DateTime != new DateTime())
                {
                    dxErrorProvider1.SetError(dataFinal, "");
                }
                else
                {
                    dxErrorProvider1.SetError(dataFinal, "Selecione a data final.");
                    ret = false;
                }
            }            

            return ret;
        }

        private void FormRelatorioOcorrencia_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btImprime.Focus();
                    btImprime_Click(sender, e);
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela sem salvar as alterações?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        btCancela.Focus();
                        btCancela_Click(sender, e);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, this.Text);
                    break;
                case Keys.F1:
                    btAjuda_Click(sender, e);
                    break;
            }
        }

        private void FormRelatorioOcorrencia_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void btAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }
    }
}
