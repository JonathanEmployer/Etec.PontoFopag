using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutFeriado : UI.Base.ManutBase
    {
        private BLL.Feriado bllFeriado;
        private Modelo.Feriado objFeriado;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;

        public FormManutFeriado()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFeriado = new BLL.Feriado();
            this.Name = "FormManutFeriado";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objFeriado = new Modelo.Feriado();
                    objFeriado.Codigo = bllFeriado.MaxCodigo();
                    objFeriado.TipoFeriado = -1;
                    objFeriado.Data = null;
                    objFeriado.Descricao = "";
                    break;
                default:
                    objFeriado = bllFeriado.LoadObject(cwID);
                    objFeriado.TipoFeriado_Ant = objFeriado.TipoFeriado;
                    objFeriado.IdEmpresa_Ant = objFeriado.IdEmpresa;
                    objFeriado.IdDepartamento_Ant = objFeriado.IdDepartamento;
                    objFeriado.Data_Ant = objFeriado.Data;
                    break;
            }
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();
            cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAll();            

            txtCodigo.DataBindings.Add("EditValue", objFeriado, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objFeriado, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtData.DataBindings.Add("DateTime", objFeriado, "Data", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipoFeriado.DataBindings.Add("EditValue", objFeriado, "TipoFeriado", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdEmpresa.DataBindings.Add("EditValue", objFeriado, "IdEmpresa", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdDepartamento.DataBindings.Add("EditValue", objFeriado, "IdDepartamento", true, DataSourceUpdateMode.OnPropertyChanged);            
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            objFeriado.Descricao = objFeriado.Descricao.TrimEnd();
            base.Salvar();            
            FormProgressBar2 pb = new FormProgressBar2();
            pb.Show(this);
            bllFeriado.ObjProgressBar = pb.ObjProgressBar;
            Dictionary<string, string> ret = bllFeriado.Salvar(cwAcao, objFeriado);

            pb.Close();
            return ret;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((int)rgTipoFeriado.EditValue)
            {
                case 0: //Geral
                    cbIdEmpresa.Enabled = false;
                    sbIdEmpresa.Enabled = false;
                    cbIdEmpresa.EditValue = 0;
                    cbIdEmpresa.Properties.DataSource = null;

                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdDepartamento.EditValue = 0;
                    cbIdDepartamento.Properties.DataSource = null;
                     break;

                case 1: //Empresa
                    cbIdEmpresa.Enabled = true;
                    sbIdEmpresa.Enabled = true;
                    cbIdEmpresa.EditValue = 0;
                    cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();

                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdDepartamento.EditValue = 0;
                    cbIdDepartamento.Properties.DataSource = null;
                    break;

                default: //Departamento
                    cbIdEmpresa.Enabled = true;
                    sbIdEmpresa.Enabled = true;
                    cbIdEmpresa.EditValue = 0;
                    cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();

                    cbIdDepartamento.Enabled = false;
                    sbIdDepartamento.Enabled = false;
                    cbIdDepartamento.EditValue = 0;
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

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)rgTipoFeriado.EditValue == 2)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
                cbIdDepartamento.Enabled = true;
                sbIdDepartamento.Enabled = true;
            }
        }
    }
}
