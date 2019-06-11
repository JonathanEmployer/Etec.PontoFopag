using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridInclusaoBanco : UI.Base.GridBase
    {
        private BLL.InclusaoBanco bllInclusaoBanco;

        public FormGridInclusaoBanco()
        {
            InitializeComponent();
            bllInclusaoBanco = new BLL.InclusaoBanco();
            this.Name = "FormGridInclusaoBanco";
        }
        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllInclusaoBanco.GetAll();
            OrdenaGrid("data", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutInclusaoBanco form = new FormManutInclusaoBanco();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Crédito/Débito";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["data"].Caption = "Data Inclusão";
            dataGridView1.Columns["data"].Width = 110;
            dataGridView1.Columns["data"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo";
            dataGridView1.Columns["tipo"].Width = 120;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 222;
            dataGridView1.Columns["tipocreditodebito"].Caption = "Tipo Crédito / Débito";
            dataGridView1.Columns["tipocreditodebito"].Width = 135;
            dataGridView1.Columns["credito"].Caption = "Crédito";
            dataGridView1.Columns["credito"].Width = 90;
            dataGridView1.Columns["credito"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["debito"].Caption = "Débito";
            dataGridView1.Columns["debito"].Width = 90;
            dataGridView1.Columns["debito"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            
        }
    }
}
