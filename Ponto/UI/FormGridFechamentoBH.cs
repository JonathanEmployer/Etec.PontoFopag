using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridFechamentoBH : UI.Base.GridBase
    {
        private BLL.FechamentoBH bllFechamentoBH;

        public FormGridFechamentoBH()
        {
            InitializeComponent();
            bllFechamentoBH = new BLL.FechamentoBH();
            this.Name = "FormGridFechamentoBH";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllFechamentoBH.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutFechamentoBH form = new FormManutFechamentoBH();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Fechamento Banco de Horas";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 100;
            dataGridView1.Columns["data"].Caption = "Data";
            dataGridView1.Columns["data"].Width = 140;
            dataGridView1.Columns["data"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo";
            dataGridView1.Columns["tipo"].Width = 180;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dataGridView1.Columns["nome"].Width = 427;
            //dataGridView1.Columns["nome"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;            
        }

        private void sbFuncionarios_Click(object sender, EventArgs e)
        {
            FormGridFechamentoBHD form = new FormGridFechamentoBHD();
            form.cwId = RegistroSelecionado();
            form.cwTabela = "Fechamento por Funcionário";
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void sbAcerto_Click(object sender, EventArgs e)
        {
            FormManutAcerto form = new FormManutAcerto();
            form.Text = "Acerto do Banco de Horas";
            form.ShowDialog();
            this.CarregaGrid(null);
        }
    }
}
