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
    public partial class FormGridCompensacao : UI.Base.GridBase
    {
        private BLL.Compensacao bllCompensacao;

        public FormGridCompensacao()
        {
            InitializeComponent();
            this.Name = "FormGridCompensacao";
            bllCompensacao = new BLL.Compensacao();
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllCompensacao.GetAll();
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutCompensacao form = new FormManutCompensacao();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Compensação";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Width = 340;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Width = 100;
            dataGridView1.Columns["periodoinicial"].Caption = "Período Inicial";
            dataGridView1.Columns["periodoinicial"].Width = 140;
            dataGridView1.Columns["periodoinicial"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["periodofinal"].Caption = "Período Final";
            dataGridView1.Columns["periodofinal"].Width = 140;
            dataGridView1.Columns["periodofinal"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["tipo"].Caption = "Tipo";
            dataGridView1.Columns["tipo"].Width = 127;
            dataGridView1.Columns["tipo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void sbFecharCompensacao_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            int id = RegistroSelecionado();
            if (id > 0)
            {
                FormProgressBar2 pb = new FormProgressBar2();
                pb.Show(this);
                bllCompensacao.ObjProgressBar = pb.ObjProgressBar;
                string log;
                if (!bllCompensacao.FechaCompensacao(id, out log))
                    MessageBox.Show("Não existe horas compensadas para essa compensação.");
                else
                {
                    pb.Close();
                    if (log.Length > 0)
                    {
                        MessageBox.Show(log);
                    }
                    else
                    {
                        MessageBox.Show("Fechamento de horas realizado com sucesso.");
                    }
                }

                CarregaGrid("");
                pb.Dispose();
            }
            else
            {
                MessageBox.Show("Nenhuma compensação está selecionada.");
            }

            this.Enabled = true;
        }

        private void sbDesfazerCompensacao_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            int id = RegistroSelecionado();
            if (id > 0)
            {
                FormProgressBar2 pb = new FormProgressBar2();
                pb.Show(this);
                bllCompensacao.ObjProgressBar = pb.ObjProgressBar;
                bllCompensacao.DesfazCompensacao(id);
                pb.Dispose();
                MessageBox.Show("A compensação foi desfeita com sucesso.");
                CarregaGrid("");
            }
            else
            {
                MessageBox.Show("Nenhuma compensação está selecionada.");
            }

            this.Enabled = true;
        }
    }
}
