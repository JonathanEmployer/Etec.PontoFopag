using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridFechamentoBHD : UI.Base.GridBase
    {
        private BLL.FechamentoBHD bllFechamentoBHD;
        public FormGridFechamentoBHD()
        {
            InitializeComponent();
            bllFechamentoBHD = new BLL.FechamentoBHD();
            this.Name = "FormGridFechamentoBHD";
            sbIncluir.Visible = false;
            sbExcluir.Visible = false;
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllFechamentoBHD.GetFuncionariosFechamento(this.cwId.Value);
            OrdenaGrid("seq", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutFechamentoBHD form = new FormManutFechamentoBHD();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Fechamento por Funcionário";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dataGridView1.Columns["nome"].Width = 247;
            dataGridView1.Columns["seq"].Caption = "Seq.";
            dataGridView1.Columns["seq"].Width = 80;
            dataGridView1.Columns["seq"].SortOrder = DevExpress.Data.ColumnSortOrder.None;
            dataGridView1.Columns["identificacao"].Caption = "Identificação";
            dataGridView1.Columns["identificacao"].Width = 100;
            dataGridView1.Columns["credito"].Caption = "Crédito";
            dataGridView1.Columns["credito"].Width = 100;
            dataGridView1.Columns["debito"].Caption = "Débito"; 
            dataGridView1.Columns["debito"].Width = 100;
            dataGridView1.Columns["saldo"].Caption = "Horas Pagas";
            dataGridView1.Columns["saldo"].Width = 100;
            dataGridView1.Columns["saldobh"].Caption = "Saldo BH";
            dataGridView1.Columns["saldobh"].Width = 120;
           
        }
    }
}
