using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridUsuario : UI.Base.GridBase
    {
        private BLL.Cw_Usuario bllCw_Usuario = BLL.Cw_Usuario.GetInstancia;

        public FormGridUsuario()
        {
            InitializeComponent();
            this.Name = "FormGridUsuario";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllCw_Usuario.GetAll();
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutUsuario form = new FormManutUsuario();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Usuario";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 100;
            dataGridView1.Columns["login"].Caption = "Login";
            dataGridView1.Columns["login"].Width = 170;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 407;
            dataGridView1.Columns["grupo"].Caption = "Grupo";
            dataGridView1.Columns["grupo"].Width = 170;
        }
    }
}
