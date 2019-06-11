using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridHorarioMovel : UI.Base.GridBase
    {
        private BLL.Horario bllHorario;

        public FormGridHorarioMovel()
        {
            InitializeComponent();
            bllHorario = new BLL.Horario();
            this.Name = "FormGridHorarioMovel";
        }
        
        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllHorario.GetHorarioMovel();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
            
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutHorarioMovel form = new FormManutHorarioMovel();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Horário Flexível";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 616;
            dataGridView1.Columns["descricao"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["codigo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["datainicial"].Caption = "Dt. Inicial";
            dataGridView1.Columns["datainicial"].Width = 75;
            dataGridView1.Columns["datainicial"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["datainicial"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["datafinal"].Caption = "Dt. Final";
            dataGridView1.Columns["datafinal"].Width = 75;
            dataGridView1.Columns["datafinal"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["datafinal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
    }
}
