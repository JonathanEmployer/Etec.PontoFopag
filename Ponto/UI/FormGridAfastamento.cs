using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridAfastamento : UI.Base.GridBase
    {
        private BLL.Afastamento bllAfastamento;

        public FormGridAfastamento()
        {
            InitializeComponent();
            this.Name = "FormGridAfastamento";
            bllAfastamento = new BLL.Afastamento();
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllAfastamento.GetAll();
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutAfastamento form = new FormManutAfastamento();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Afastamento";           
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 230;
            dataGridView1.Columns["ocorrencia"].Caption = "Ocorrência";
            dataGridView1.Columns["ocorrencia"].Width = 220;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["datai"].Caption = "Data Inicial";
            dataGridView1.Columns["datai"].Width = 90;
            dataGridView1.Columns["datai"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["dataf"].Caption = "Data Final";
            dataGridView1.Columns["dataf"].Width = 90;
            dataGridView1.Columns["dataf"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo Afastamento";
            dataGridView1.Columns["tipo"].Width = 137;
            dataGridView1.Columns["tipo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
    }
}
