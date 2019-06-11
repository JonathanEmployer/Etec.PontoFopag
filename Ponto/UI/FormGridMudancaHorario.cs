using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridMudancaHorario : UI.Base.GridBase
    {
        private BLL.MudancaHorario bllMudancaHorario;
        private int cwIdFuncionario { get; set; }

        public FormGridMudancaHorario(int pIdFuncionario)
        {
            InitializeComponent();
            bllMudancaHorario = new BLL.MudancaHorario();
            this.Name = "FormGridMudancaHorario";
            cwIdFuncionario = pIdFuncionario;
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllMudancaHorario.GetPorFuncionario(cwIdFuncionario);
            OrdenaGrid("data", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["data"].Caption = "Data";
            dataGridView1.Columns["data"].Width = 80;
            dataGridView1.Columns["descricaohorario_ant"].Caption = "Turno Antigo";
            dataGridView1.Columns["descricaohorario_ant"].Width = 283;
            dataGridView1.Columns["tipohorario_ant"].Caption = "Tipo Horário";
            dataGridView1.Columns["tipohorario_ant"].Width = 100;
            dataGridView1.Columns["descricaohorario"].Caption = "Turno Novo";
            dataGridView1.Columns["descricaohorario"].Width = 283;
            dataGridView1.Columns["tipohorario"].Caption = "Tipo Horário";
            dataGridView1.Columns["tipohorario"].Width = 101;
        }
        

        private void sbExcluirMudancaHorario_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            if (id > 0)
            {
                if (MessageBox.Show("Tem certeza de que deseja excluir a mudança de horário selecionada?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FormProgressBar2 pb = new FormProgressBar2();
                    pb.Show();
                    if (bllMudancaHorario.ExcluiMudanca(id, pb.ObjProgressBar))
                    {
                        CarregaGrid("");
                        pb.Close();
                    }
                    else
                    {
                        pb.Close();
                        MessageBox.Show("Só é permitido excuir a última mudança de horário do funcionário.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
        }
    }
}