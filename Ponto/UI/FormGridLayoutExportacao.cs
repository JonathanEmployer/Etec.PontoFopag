using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace UI
{
    public partial class FormGridLayoutExportacao : UI.Base.GridBase
    {
        private BLL.LayoutExportacao bllLayoutExportacao;
        public FormGridLayoutExportacao()
        {
            InitializeComponent();
            bllLayoutExportacao = new BLL.LayoutExportacao();
            this.Name = "FormGridLayoutExportacao";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllLayoutExportacao.GetAll();
            OrdenaGrid("codigo", DevExpress.Data.ColumnSortOrder.Ascending);

        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutLayoutExportacao form = new FormManutLayoutExportacao();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Layout para Exportação";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Width = 640;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 206;
        }

        protected override void sbIncluir_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Incluir, 0);
        }

        private void sbHistoricoMudancaHorario_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            if (id > 0)
            {
                FormManutExportacaoFolha form = new FormManutExportacaoFolha(id);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
        }
    }
}
