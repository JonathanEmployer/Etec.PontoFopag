using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridProvisorio : UI.Base.GridBase
    {
        private BLL.Provisorio bllProvisorio;
        public FormGridProvisorio()
        {
            InitializeComponent();
            bllProvisorio = new BLL.Provisorio();
            this.Name = "FormGridProvisorio";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllProvisorio.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutProvisorio form = new FormManutProvisorio();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Provisório";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 167;
            dataGridView1.Columns["codigo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["dsfuncionarionovo"].Caption = "Código Temporário";
            dataGridView1.Columns["dsfuncionarionovo"].Width = 170;
            dataGridView1.Columns["dsfuncionarionovo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["dsfuncionario"].Caption = "Código Funcionário";
            dataGridView1.Columns["dsfuncionario"].Width = 170;
            dataGridView1.Columns["dsfuncionario"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["dt_inicial"].Caption = "Data Inicial";
            dataGridView1.Columns["dt_inicial"].Width = 170;
            dataGridView1.Columns["dt_inicial"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["dt_final"].Caption = "Data Final";
            dataGridView1.Columns["dt_final"].Width = 170;
            dataGridView1.Columns["dt_final"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
    }
}
