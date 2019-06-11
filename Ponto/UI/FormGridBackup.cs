using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridBackup : UI.Base.GridBase
    {
        private BLL.Backup bllBackup;
        
        public FormGridBackup()
        {
            InitializeComponent();
            bllBackup = new BLL.Backup();
            this.Name = "FormGridBackup";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllBackup.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;                        
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 440;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 100;
            dataGridView1.Columns["diretorio"].Caption = "Diretório";
            dataGridView1.Columns["diretorio"].Width = 306;
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutBackup form = new FormManutBackup();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Backup";
            form.ShowDialog();
        }

    }
}
