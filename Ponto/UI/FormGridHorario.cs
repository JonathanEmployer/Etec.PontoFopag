using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridHorario : UI.Base.GridBase
    {
        private BLL.Horario bllHorario;

        public FormGridHorario()
        {
            InitializeComponent();
            bllHorario = new BLL.Horario();
            this.Name = "FormGridHorario";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllHorario.GetHorarioNormal();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutHorario form = new FormManutHorario();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Horário Normal";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 560;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 266;

            dataGridView1.Columns["datainicial"].Visible = false;
            dataGridView1.Columns["datafinal"].Visible = false;
        }
    }
}
