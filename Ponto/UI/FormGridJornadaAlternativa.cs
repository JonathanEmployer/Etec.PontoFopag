using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridJornadaAlternativa : UI.Base.GridBase
    {
        private BLL.JornadaAlternativa bllJornadaAlternativa;

        public FormGridJornadaAlternativa()
        {
            InitializeComponent();
            bllJornadaAlternativa = new BLL.JornadaAlternativa();
            this.Name = "FormGridJornadaAlternativa";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllJornadaAlternativa.GetAll();
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutJornadaAlternativa form = new FormManutJornadaAlternativa();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Jornada Alternativa";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["datainicial"].Caption = "Data Inicial";
            dataGridView1.Columns["datainicial"].Width = 85;
            dataGridView1.Columns["datainicial"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["datafinal"].Caption = "Data Final";
            dataGridView1.Columns["datafinal"].Width = 85;
            dataGridView1.Columns["datafinal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo";
            dataGridView1.Columns["tipo"].Width = 120;
            dataGridView1.Columns["tipo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 237;
            dataGridView1.Columns["entrada_1"].Caption = "Ent.01";
            dataGridView1.Columns["entrada_1"].Width = 60;
            dataGridView1.Columns["entrada_1"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["saida_1"].Caption = "Sai.01";
            dataGridView1.Columns["saida_1"].Width = 60;
            dataGridView1.Columns["saida_1"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["entrada_2"].Caption = "Ent.02";
            dataGridView1.Columns["entrada_2"].Width = 60;
            dataGridView1.Columns["entrada_2"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["saida_2"].Caption = "Sai.02";
            dataGridView1.Columns["saida_2"].Width = 60;
            dataGridView1.Columns["saida_2"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
    }
}
