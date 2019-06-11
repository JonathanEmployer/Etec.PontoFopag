using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridREP : UI.Base.GridBase
    {
        private BLL.REP bllREP;

        public FormGridREP()
        {
            InitializeComponent();
            bllREP = new BLL.REP();
            this.Name = "FormGridREP";
        }
        
        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllREP.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutREP form = new FormManutREP();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "REP";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 100;
            dataGridView1.Columns["numserie"].Caption = "Número Série";
            dataGridView1.Columns["numserie"].Width = 170;
            dataGridView1.Columns["local"].Caption = "Local";
            dataGridView1.Columns["local"].Width = 237;
            dataGridView1.Columns["numrelogio"].Caption = " Número Relógio";
            dataGridView1.Columns["numrelogio"].Width = 170;
            dataGridView1.Columns["empresa"].Caption = "Empresa";
            dataGridView1.Columns["empresa"].Width = 170;
        }
    }
}
