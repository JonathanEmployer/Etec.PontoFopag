using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridTipoBilhetes : UI.Base.GridBase
    {
        private BLL.TipoBilhetes bllTipoBilhetes;

        public FormGridTipoBilhetes()
        {
            InitializeComponent();
            bllTipoBilhetes = new BLL.TipoBilhetes();
            this.Name = "FormGridTipoBilhetes";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllTipoBilhetes.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutTipoBilhetes form = new FormManutTipoBilhetes();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Tipo de Bilhetes";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 347;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 135;
            dataGridView1.Columns["formatobilhete"].Caption = "Formato Bilhete";
            dataGridView1.Columns["formatobilhete"].Width = 230;
            dataGridView1.Columns["importar"].Caption = "Importar";
            dataGridView1.Columns["importar"].Width = 135;
            dataGridView1.Columns["diretorio"].Visible = false;
        }
    }
}
