using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutDepartamento : UI.Base.ManutBase
    {
        private BLL.Departamento bllDepartamento;
        private Modelo.Departamento objDepartamento;
        private BLL.Empresa bllEmpresa;

        public FormManutDepartamento()
        {
            InitializeComponent();
            bllDepartamento = new BLL.Departamento();
            bllEmpresa = new BLL.Empresa();
            this.Name = "FormManutDepartamento";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objDepartamento = new Modelo.Departamento();
                    objDepartamento.Codigo = bllDepartamento.MaxCodigo();
                    objDepartamento.Descricao = "";
                    break;
                default:
                    objDepartamento = bllDepartamento.LoadObject(cwID);
                    break;
            }
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();

            txtCodigo.DataBindings.Add("EditValue", objDepartamento, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objDepartamento, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdEmpresa.DataBindings.Add("EditValue", objDepartamento, "IdEmpresa", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentualMaximoHorasExtras.DataBindings.Add("EditValue", objDepartamento, "PercentualMaximoHorasExtras", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (cwAcao == Modelo.Acao.Alterar && bllDepartamento.PossuiFuncionarios(objDepartamento.Id))
            {
                cbIdEmpresa.Enabled = false;
                sbIdEmpresa.Enabled = false;
                MessageBox.Show("Como já existem funcionários cadastrados para este departamento,\n sua empresa não poderá ser alterada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            objDepartamento.Descricao = objDepartamento.Descricao.TrimEnd();
            base.Salvar();
            return bllDepartamento.Salvar(cwAcao, objDepartamento);
        }

        private void sbIdEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);
        }
    }
}
