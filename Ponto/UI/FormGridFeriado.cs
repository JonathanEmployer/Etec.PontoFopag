using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridFeriado : UI.Base.GridBase
    {
        private BLL.Feriado bllFeriado;

        public FormGridFeriado()
        {
            InitializeComponent();
            bllFeriado = new BLL.Feriado();
            this.Name = "FormGridFeriado";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllFeriado.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutFeriado form = new FormManutFeriado();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Feriado";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 320;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["data"].Caption = "Data"; 
            dataGridView1.Columns["data"].Width = 80;
            dataGridView1.Columns["data"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo";
            dataGridView1.Columns["tipo"].Width = 100;
            dataGridView1.Columns["tipo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 250;
        }
    }
}
