using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutMudancaHorario : UI.Base.ManutBase
    {
        private BLL.MudancaHorario bllMudancaHorario;
        private Modelo.MudancaHorario objMudancaHorario;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.Horario bllHorario;
        private BLL.Funcao bllFuncao;
       

        public FormManutMudancaHorario()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncao = new BLL.Funcao();
            bllHorario = new BLL.Horario();
            bllFuncionario = new BLL.Funcionario();
            bllMudancaHorario = new BLL.MudancaHorario();
            this.Name = "FormManutMudancaHorario";
            rgTipo.EditValue = -1;
            rgTipoHorario.EditValue = -1;
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objMudancaHorario = new Modelo.MudancaHorario();
                    objMudancaHorario.Tipohorario = -1;
                    objMudancaHorario.Data = null;
                    break;
                default:
                    objMudancaHorario = bllMudancaHorario.LoadObject(cwID);
                    break;
            }   
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = null;
            FormProgressBar2 BarraProgresso = new FormProgressBar2();
            if ((int)rgTipo.EditValue == 2)
            {
                if (MessageBox.Show("Exclusão da mudança de horário por empresa será individual, Deseja continuar?","Aviso", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    try
                    {
                        BarraProgresso.Show(this);
                        this.Enabled = false;
                        bllMudancaHorario.ObjProgressBar = BarraProgresso.ObjProgressBar;
                        ret = bllMudancaHorario.MudarHorario((int)rgTipo.EditValue,(int)cbIdFuncao.EditValue, (int)cbIdEmpresa.EditValue, (int)cbIdDepartamento.EditValue, (int)cbIdFuncionario.EditValue, (int)rgTipoHorario.EditValue, (int)cbIdTurnoNormal.EditValue, (DateTime?)txtData.EditValue);

                        if (ret.Count == 0)
                        {
                            MessageBox.Show("Mudança de horário executada com sucesso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        BarraProgresso.Close();
                        this.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        BarraProgresso.Close();
                        MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                    }
                }
            }
            else
            {
                try
                {
                    BarraProgresso.Show(this);
                    this.Enabled = false;
                    bllMudancaHorario.ObjProgressBar = BarraProgresso.ObjProgressBar;
                    ret = bllMudancaHorario.MudarHorario((int)rgTipo.EditValue,(int)cbIdFuncao.EditValue, (int)cbIdEmpresa.EditValue, (int)cbIdDepartamento.EditValue, (int)cbIdFuncionario.EditValue, (int)rgTipoHorario.EditValue, (int)cbIdTurnoNormal.EditValue, (DateTime?)txtData.EditValue);

                    BarraProgresso.Close();
                    if (ret.Count == 0)
                    {
                        MessageBox.Show("Mudança de horário executada com sucesso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Enabled = true;
                }
                catch (Exception ex)
                {
                    BarraProgresso.Close();
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                }
            }
            return ret;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((int)rgTipo.EditValue)
            {
                case 0://Individual
                    cbIdEmpresa.Enabled = false;
                    sbIdEmpresa.Enabled = false;
                    cbIdEmpresa.EditValue = 0;
                    cbIdEmpresa.Properties.DataSource = null;

                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdDepartamento.EditValue = 0;
                    cbIdDepartamento.Properties.DataSource = null;

                    cbIdFuncionario.Enabled = true;
                    sbIdFuncionario.Enabled = true;
                    cbIdFuncionario.EditValue = 0;
                    cbIdFuncionario.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();

                    cbIdFuncao.Enabled = false;
                    sbIdFuncao.Enabled = false;
                    cbIdFuncao.EditValue = 0;
                    cbIdFuncao.Properties.DataSource = null;
                    break;
                
                case 1: //Departamento
                    cbIdEmpresa.Enabled = true;
                    sbIdEmpresa.Enabled = true;
                    cbIdEmpresa.EditValue = 0;
                    cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();

                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdDepartamento.EditValue = 0;
                    cbIdDepartamento.Properties.DataSource = null;

                    cbIdFuncionario.Enabled = false;
                    sbIdFuncionario.Enabled = false;
                    cbIdFuncionario.EditValue = 0;
                    cbIdFuncionario.Properties.DataSource = null;

                    cbIdFuncao.Enabled = false;
                    sbIdFuncao.Enabled = false;
                    cbIdFuncao.EditValue = 0;
                    cbIdFuncao.Properties.DataSource = null;
                    break;

                case 2: //Empresa
                    cbIdEmpresa.Enabled = true;
                    sbIdEmpresa.Enabled = true;
                    cbIdEmpresa.EditValue = 0;
                    cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();

                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdDepartamento.EditValue = 0;
                    cbIdDepartamento.Properties.DataSource = null;

                    cbIdFuncionario.Enabled = false;
                    sbIdFuncionario.Enabled = false;                    
                    cbIdFuncionario.EditValue = 0;
                    cbIdFuncionario.Properties.DataSource = null;

                    cbIdFuncao.Enabled = false;
                    sbIdFuncao.Enabled = false;
                    cbIdFuncao.EditValue = 0;
                    cbIdFuncao.Properties.DataSource = null;

                    break;

                case 3: //Funcao 
                    cbIdEmpresa.Enabled = false;
                    sbIdEmpresa.Enabled = false;
                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdFuncionario.Enabled = false;
                    sbIdFuncionario.Enabled = false;
                    cbIdFuncao.Enabled = true;
                    sbIdFuncao.Enabled = true;

                    cbIdFuncao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdDepartamento.Properties.DataSource = null;
                    cbIdEmpresa.Properties.DataSource = null;
                    cbIdFuncionario.Properties.DataSource = null;
                    break;

                default:
                    break;
            }
        }

        private void sbIdEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);
        }

        private void sbIdDepartamento_Click(object sender, EventArgs e)
        {
            FormGridDepartamento form = new FormGridDepartamento();
            form.cwTabela = "Departamento";
            form.cwId = (int)cbIdDepartamento.EditValue;
            GridSelecao(form, cbIdDepartamento, bllDepartamento);
        }

        private void sbIdFuncionario_Click(object sender, EventArgs e)
        {
            FormGridFuncionario form = new FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = (int)cbIdFuncionario.EditValue;
            GridSelecao(form, cbIdFuncionario, bllFuncionario);
        }

        private void rgTipoHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)rgTipoHorario.EditValue == 1 || (int)rgTipoHorario.EditValue == 2)
            {
                cbIdTurnoNormal.Enabled = true;
                sbIdTurnoNormal.Enabled = true;

                cbIdTurnoNormal.Properties.DataSource = bllHorario.GetHorarioTipo((int)rgTipoHorario.EditValue);
                cbIdTurnoNormal.EditValue = 0;
            }
            else
            {
                cbIdTurnoNormal.Enabled = false;
                sbIdTurnoNormal.Enabled = false;
                cbIdTurnoNormal.EditValue = 0;
            }
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)rgTipo.EditValue == 1) //Departamento
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
                cbIdDepartamento.Enabled = true;
                sbIdDepartamento.Enabled = true;
            }    
        }

        private void sbIdTurnoNormal_Click(object sender, EventArgs e)
        {
            if (rgTipoHorario.SelectedIndex == 0)
            {
                FormGridHorario form = new FormGridHorario();
                form.cwTabela = "Horário Normal";
                form.cwId = (int)cbIdTurnoNormal.EditValue;
                GridSelecao(form, cbIdTurnoNormal, bllHorario);
            }

            else if (rgTipoHorario.SelectedIndex == 1)
            {
                FormGridHorarioMovel form = new FormGridHorarioMovel();
                form.cwTabela = "Horário Móvel";
                form.cwId = (int)cbIdTurnoNormal.EditValue;
                GridSelecao(form, cbIdTurnoNormal, bllHorario);
            }
        }

        private void sbIdFuncao_Click(object sender, EventArgs e)
        {
            FormGridFuncao form = new FormGridFuncao();
            form.cwTabela = "Função";
            form.cwId = (int)cbIdFuncao.EditValue;
            GridSelecao(form, cbIdFuncao, bllFuncao);
        }

        private void FormManutMudancaHorario_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    sbCancelar.Focus();
                    sbCancelar_Click(sender, e);
                    break;
                case Keys.Enter:
                    sbGravar.Focus();
                    sbGravar_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, cwTabela);
                    break;
            }
        }

       
    }
}
