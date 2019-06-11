using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridOcorrencia : UI.Base.GridBase
    {
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.Empresa bllEmpresa;
        public FormGridOcorrencia()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllOcorrencia = new BLL.Ocorrencia();
            this.Name = "FormGridOcorrencia";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllOcorrencia.GetAll();
            OrdenaGrid("descricao", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutOcorrencia form = new FormManutOcorrencia();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Ocorrência";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 150;
            dataGridView1.Columns["descricao"].Caption = "Descrição";

            if (bllEmpresa.RelatorioAbsenteismoLiberado())
            {
                dataGridView1.Columns["descricao"].Width = 580;
                dataGridView1.Columns["absenteismo"].Caption = "Absenteísmo";
                dataGridView1.Columns["absenteismo"].Width = 100;
            }
            else
            {
                dataGridView1.Columns["descricao"].Width = 680;
                dataGridView1.Columns["absenteismo"].Visible = false;
            }
        }
    }
}
