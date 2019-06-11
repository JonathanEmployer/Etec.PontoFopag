using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridJornada : UI.Base.GridBase
    {
        private BLL.Jornada bllJornada;

        public FormGridJornada()
        {
            InitializeComponent();
            bllJornada = new BLL.Jornada();
            this.Name = "FormGridJornada";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllJornada.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;                        
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["horarios"].Caption = "Horários";
            dataGridView1.Columns["horarios"].Width = 646;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 180;
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutJornada form = new FormManutJornada();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Jornada";
            form.ShowDialog();
        }

    }
}
