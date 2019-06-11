using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutAfastamento : UI.Base.ManutBase
    {
        private BLL.Afastamento bllAfastamento;
        private Modelo.Afastamento objAfastamento;
        private BLL.Departamento bllDepartamento;
        private BLL.Empresa bllEmpresa;
        private BLL.Funcionario bllFuncionario;
        private BLL.Ocorrencia bllOcorrencia;

        public FormManutAfastamento()
        {
            InitializeComponent();
            bllAfastamento = new BLL.Afastamento();
            bllDepartamento = new BLL.Departamento();
            bllEmpresa = new BLL.Empresa();
            bllFuncionario = new BLL.Funcionario();
            bllOcorrencia = new BLL.Ocorrencia();
            this.Name = "FormManutAfastamento";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objAfastamento = new Modelo.Afastamento();
                    objAfastamento.Codigo = bllAfastamento.MaxCodigo();
                    objAfastamento.Tipo = -1;
                    objAfastamento.Datai = null;
                    objAfastamento.Dataf = null;
                    objAfastamento.Horai = "--:--";
                    objAfastamento.Horaf = "--:--";
                    break;
                default:
                    objAfastamento = bllAfastamento.LoadObject(cwID);

                    if (objAfastamento.Tipo == 0)
                    {
                        Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(objAfastamento.IdFuncionario);
                        if (objFuncionario.Excluido == 1)
                        {
                            this.Close();
                            MessageBox.Show("O funcionário " + objFuncionario.Nome + " está excluído. \nPara alterar este afastamento é necessário restaurar o funcionário.");
                        }
                    }

                    objAfastamento.Tipo_Ant = objAfastamento.Tipo;
                    objAfastamento.Datai_Ant = objAfastamento.Datai;
                    objAfastamento.Dataf_Ant = objAfastamento.Dataf;
                    objAfastamento.IdEmpresa_Ant = objAfastamento.IdEmpresa;
                    objAfastamento.IdDepartamento_Ant = objAfastamento.IdDepartamento;
                    objAfastamento.IdFuncionario_Ant = objAfastamento.IdFuncionario;
                    break;
            }
            cbIdOcorrencia.Properties.DataSource = bllOcorrencia.GetAll();

            txtCodigo.DataBindings.Add("EditValue", objAfastamento, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipo.DataBindings.Add("EditValue", objAfastamento, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdEmpresa.DataBindings.Add("EditValue", objAfastamento, "IdEmpresa", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdDepartamento.DataBindings.Add("EditValue", objAfastamento, "IdDepartamento", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdFuncionario.DataBindings.Add("EditValue", objAfastamento, "IdFuncionario", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdOcorrencia.DataBindings.Add("EditValue", objAfastamento, "IdOcorrencia", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDatai.DataBindings.Add("DateTime", objAfastamento, "Datai", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataf.DataBindings.Add("DateTime", objAfastamento, "Dataf", true, DataSourceUpdateMode.OnPropertyChanged);
            chbSemCalculo.DataBindings.Add("Checked", objAfastamento, "SemCalculo", true, DataSourceUpdateMode.OnPropertyChanged);
            chbAbonado.DataBindings.Add("Checked", objAfastamento, "Abonado", true, DataSourceUpdateMode.OnPropertyChanged);
            chbParcial.DataBindings.Add("Checked", objAfastamento, "Parcial", true, DataSourceUpdateMode.OnPropertyChanged);

            chbSuspensao.Checked = objAfastamento.bSuspensao;
            txtHorai.EditValue = objAfastamento.Horai;
            txtHoraf.EditValue = objAfastamento.Horaf;
            //txtHorai.DataBindings.Add("EditValue", objAfastamento, "Horai", true, DataSourceUpdateMode.OnPropertyChanged);
            //txtHoraf.DataBindings.Add("EditValue", objAfastamento, "Horaf", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }
        
        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            FormProgressBar2 pb = new FormProgressBar2();
            pb.Show(this);
            bllAfastamento.ObjProgressBar = pb.ObjProgressBar;
            objAfastamento.Horai = Convert.ToString(txtHorai.EditValue);
            objAfastamento.Horaf = Convert.ToString(txtHoraf.EditValue);
            objAfastamento.bSuspensao = chbSuspensao.Checked;
            Dictionary<string, string> ret = bllAfastamento.Salvar(cwAcao, objAfastamento);
            pb.Close();
            return ret;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((int)rgTipo.EditValue)
            {
                case 0: //Funcionário
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
                    break;

                default:
                    break;
            }
        }

        private void sbIdFuncionario_Click(object sender, EventArgs e)
        {
            FormGridFuncionario form = new FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = (int)cbIdFuncionario.EditValue;
            GridSelecao(form, cbIdFuncionario, bllFuncionario);
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
        
        private void sbIdOcorrencia_Click(object sender, EventArgs e)
        {
            FormGridOcorrencia form = new FormGridOcorrencia();
            form.cwTabela = "Ocorrência";
            form.cwId = (int)cbIdOcorrencia.EditValue;
            GridSelecao(form, cbIdOcorrencia, bllOcorrencia);
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)rgTipo.EditValue == 1)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
                cbIdDepartamento.Enabled = true;
                sbIdDepartamento.Enabled = true;
            }
        }

        private void chbSemCalculo_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSemCalculo.Checked == true)
            {
                chbAbonado.Checked = false;
                chbParcial.Checked = false;
                chbSemCalculo.Checked = true;
            }
        }

        private void chbAbonado_CheckedChanged(object sender, EventArgs e)
        {
            if (chbAbonado.Checked == true)
            {
                chbParcial.Checked = false;
                chbSemCalculo.Checked = false;
                chbAbonado.Checked = true;
            }

        }

        private void chbParcial_CheckedChanged(object sender, EventArgs e)
        {
            if (chbParcial.Checked == true)
            {
                chbAbonado.Checked = true;
                chbAbonado.Enabled = false;
                chbSemCalculo.Checked = false;
                chbParcial.Checked = true;
                txtHorai.Enabled = true;
                txtHoraf.Enabled = true;
            }
            else
            {
                chbAbonado.Enabled = true;
                txtHorai.Enabled = false;
                txtHoraf.Enabled = false;
                txtHorai.EditValue = "--:--";
                txtHoraf.EditValue = "--:--";
            }
        }

        private void chbSuspensao_CheckedChanged(object sender, EventArgs e)
        {
            chbSemCalculo.Checked = false;
            chbAbonado.Checked = false;
            chbParcial.Checked = false;
            if (chbSuspensao.Checked == true)
            {
                chbSemCalculo.Enabled = false;
                chbAbonado.Enabled = false;
                chbParcial.Enabled = false;

                chbSemCalculo.Properties.ReadOnly = true;
                chbAbonado.Properties.ReadOnly = true;
                chbParcial.Properties.ReadOnly = true;
            }
            else
            {
                chbSemCalculo.Enabled = true;
                chbAbonado.Enabled = true;
                chbParcial.Enabled = true;

                chbSemCalculo.Properties.ReadOnly = false;
                chbAbonado.Properties.ReadOnly = false;
                chbParcial.Properties.ReadOnly = false;

            }
            txtHorai.EditValue = "--:--";
            txtHoraf.EditValue = "--:--";
            txtHorai.Enabled = false;
            txtHorai.Properties.ReadOnly = true;
            txtHoraf.Enabled = false;
            txtHoraf.Properties.ReadOnly = true;
        }
    } 
}

