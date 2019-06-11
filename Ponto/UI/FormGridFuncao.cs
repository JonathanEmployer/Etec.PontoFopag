using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridFuncao : UI.Base.GridBase
    {
        private BLL.Funcao bllFuncao;

        public FormGridFuncao()
        {
            InitializeComponent();
            bllFuncao = new BLL.Funcao();
            this.Name = "FormGridFuncao";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllFuncao.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutFuncao form = new FormManutFuncao();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Função";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 680;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 150;
        }
    }
}
