using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridExportacaoCampos : UI.Base.GridBase
    {
        private BLL.ExportacaoCampos bllExportacaoCampos;

        public FormGridExportacaoCampos()
        {
            InitializeComponent();
            bllExportacaoCampos = new BLL.ExportacaoCampos();
            this.Name = "FormGridExportacaoCampos";
            lblCampos.Text = "";
            AtualizaString();
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllExportacaoCampos.GetAll();
            OrdenaGrid("posicao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            //FormManutExportacaoCampos form = new FormManutExportacaoCampos();
            //form.cwAcao = pAcao;
            //form.cwID = pID;
            //form.cwTabela = "Exportação Campos";
            //form.ShowDialog();

            //AtualizaString();
        }

        private void AtualizaString()
        {
            List<Modelo.ExportacaoCampos> lista = bllExportacaoCampos.GetAllList();
            lblCampos.Text = BLL.ExportacaoCampos.MontaStringExportacao(lista);
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["tipo"].Caption = "Tipo";
            dataGridView1.Columns["tipo"].Width = 127;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["tamanho"].Caption = "Tamanho";
            dataGridView1.Columns["tamanho"].Width = 80;
            dataGridView1.Columns["posicao"].Caption = "Posição";
            dataGridView1.Columns["posicao"].Width = 80;
            dataGridView1.Columns["delimitador"].Caption = "Delimitador";
            dataGridView1.Columns["delimitador"].Width = 80;
            dataGridView1.Columns["qualificador"].Caption = "Qualificador";
            dataGridView1.Columns["qualificador"].Width = 80;
            dataGridView1.Columns["texto"].Caption = "Texto";
            dataGridView1.Columns["texto"].Width = 160;
            dataGridView1.Columns["cabecalho"].Caption = "Cabeçalho";
            dataGridView1.Columns["cabecalho"].Width = 160;
        }

        private void sbExportacaoFolha_Click(object sender, EventArgs e)
        {
            //FormManutExportacaoFolha form = new FormManutExportacaoFolha();
            //form.ShowDialog();
        }
    }
}
