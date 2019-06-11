using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridParametros : UI.Base.GridBase
    {
        private BLL.Parametros bllParametros;

        public FormGridParametros()
        {
            InitializeComponent();
            bllParametros = new BLL.Parametros();
            this.Name = "FormGridParametros";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllParametros.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutParametros form = new FormManutParametros();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Parâmetros";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["descricao"].Caption = "Descrição";
            dataGridView1.Columns["descricao"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["descricao"].Width = 495;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["inicioadnoturno"].Caption = "Início Adicional";
            dataGridView1.Columns["inicioadnoturno"].Width = 135;
            dataGridView1.Columns["inicioadnoturno"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["fimadnoturno"].Caption = "Fim Adicional";
            dataGridView1.Columns["fimadnoturno"].Width = 135;
            dataGridView1.Columns["fimadnoturno"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["thoraextra"].Caption = "Hora Extra";
            dataGridView1.Columns["thoraextra"].Width = 135;
            dataGridView1.Columns["thoraextra"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["thorafalta"].Caption = "Falta";
            dataGridView1.Columns["thorafalta"].Width = 135;
            dataGridView1.Columns["thorafalta"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
    }
}
