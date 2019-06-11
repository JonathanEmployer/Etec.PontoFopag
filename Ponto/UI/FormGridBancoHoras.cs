using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridBancoHoras : UI.Base.GridBase
    {
        private BLL.BancoHoras bllBancoHoras;

        public FormGridBancoHoras()
        {
            InitializeComponent();
            bllBancoHoras = new BLL.BancoHoras();
            this.Name = "FormGridBancoHoras";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllBancoHoras.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutBancoHoras form = new FormManutBancoHoras();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Banco de Horas";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["datainicial"].Caption = "Início Banco Horas";
            dataGridView1.Columns["datainicial"].Width = 130;
            dataGridView1.Columns["datainicial"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["datafinal"].Caption = "Término Banco Horas";
            dataGridView1.Columns["datafinal"].Width = 130;
            dataGridView1.Columns["datafinal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo do Banco";
            dataGridView1.Columns["tipo"].Width = 120;
            dataGridView1.Columns["tipo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 387;
        }
    }
}
