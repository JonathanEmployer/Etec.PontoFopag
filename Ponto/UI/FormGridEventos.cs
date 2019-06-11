using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridEventos : UI.Base.GridBase
    {
        private BLL.Eventos bllEventos;

        public FormGridEventos()
        {
            InitializeComponent();
            bllEventos = new BLL.Eventos();
            this.Name = "FormGridEventos";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllEventos.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutEventos form = new FormManutEventos();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Eventos";
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
