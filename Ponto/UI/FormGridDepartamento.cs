using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridDepartamento : UI.Base.GridBase
    {
        private BLL.Departamento bllDepartamento;

        public FormGridDepartamento()
        {
            InitializeComponent();
            bllDepartamento = new BLL.Departamento();
            this.Name = "FormGridDepartamento";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllDepartamento.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutDepartamento form = new FormManutDepartamento();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Departamento";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 450;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["empresa"].Caption = "Empresa";
            dataGridView1.Columns["empresa"].Width = 317;
        }
    }
}