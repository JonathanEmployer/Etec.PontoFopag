using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridMudancaCodigo : UI.Base.GridSimplesBase
    {
        private BLL.MudCodigoFunc bllMudaCodigo;

        public FormGridMudancaCodigo()
        {
            InitializeComponent();
            bllMudaCodigo = new BLL.MudCodigoFunc();
            this.Name = "FormGridMudancaCodigo";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllMudaCodigo.GetAll();
            OrdenaGrid("data", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 120;
            dataGridView1.Columns["data"].Caption = "Data";
            dataGridView1.Columns["data"].Width = 120;
            dataGridView1.Columns["data"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["NomeFuncionario"].Caption = "Funcionário";
            dataGridView1.Columns["NomeFuncionario"].Width = 367;
            dataGridView1.Columns["dscodigoantigo"].Caption = "Código Antigo";
            dataGridView1.Columns["dscodigoantigo"].Width = 120;
            dataGridView1.Columns["dscodigonovo"].Caption = "Código Novo";
            dataGridView1.Columns["dscodigonovo"].Width = 120;
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormMudancaCodigo form = new FormMudancaCodigo();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Mudança de Código do Funcionário";
            form.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            CarregaFormulario(Modelo.Acao.Incluir, 0);
            CarregaGrid("");
            SelecionaRegistroPorPos(dataGridView1.RowCount - 1);
        }

        private void FormGridMudancaCodigo_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
