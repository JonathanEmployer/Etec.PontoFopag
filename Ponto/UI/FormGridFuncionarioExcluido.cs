using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormGridFuncionarioExcluido : UI.Base.GridBase
    {
        private BLL.Funcionario bllFuncionario;
        private BLL.Empresa bllEmpresa;

        public FormGridFuncionarioExcluido()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FormGridFuncionarioExcluido";
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllFuncionario.GetExcluidos();
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
            
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["nome"].Width = 240;
            dataGridView1.Columns["dscodigo"].Caption = "Código";
            dataGridView1.Columns["dscodigo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["dscodigo"].Width = 80;
            dataGridView1.Columns["dscodigo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["matricula"].Caption = "Matrícula";
            dataGridView1.Columns["matricula"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["matricula"].Width = 80;
            dataGridView1.Columns["matricula"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["jornada"].Caption = "Jornada";
            dataGridView1.Columns["jornada"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["jornada"].Width = 135;
            dataGridView1.Columns["empresa"].Caption = "Empresa";
            dataGridView1.Columns["empresa"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["empresa"].Width = 180;
            dataGridView1.Columns["departamento"].Caption = "Departamento";
            dataGridView1.Columns["departamento"].Width = 130;
            dataGridView1.Columns["carteira"].Caption = "Carteira";
            dataGridView1.Columns["carteira"].Width = 130;
            dataGridView1.Columns["carteira"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["dataadmissao"].Caption = "Data Admissão";
            dataGridView1.Columns["dataadmissao"].Width = 130;
            dataGridView1.Columns["dataadmissao"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }

        private void sbExcluiFunc_Click(object sender, EventArgs e)
        {
            
            //Modelo.Funcionario objfunc = bllFuncionario.LoadObject(RegistroSelecionado());
            //bllFuncionario.ExcluirDefinitivamente(objfunc);
            //CarregaGrid("");
        }

        private void sbRestauraFunc_Click_1(object sender, EventArgs e)
        {
            int limitefunc = 0;
            if (bllEmpresa.ValidaLicenca(out limitefunc, false))
            {
                FormProgressBar2 pb = new FormProgressBar2();
                pb.Show();
                try
                {
                    string mensagem = "";
                    bllFuncionario.ObjProgressBar = pb.ObjProgressBar;
                    Modelo.Funcionario objfunc = bllFuncionario.LoadObject(RegistroSelecionado());
                    if (bllFuncionario.PisUtilizado(objfunc, Convert.ToString(objfunc.Pis), out mensagem))
                    {
                        MessageBox.Show(mensagem);
                    }
                    else
                    {
                        objfunc.Excluido = 0;
                        objfunc.Funcionarioativo = 1;
                        objfunc.ImportarMarcacoes = true;
                        bllFuncionario.Salvar1(Modelo.Acao.Alterar, objfunc, 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    pb.Close();
                }
                CarregaGrid("");
            }
            else
            {
                MessageBox.Show("A quantidade de funcionários chegou no limite de " + limitefunc + " funcionários ativos. Entre em contato com a revenda.");
            }
        }
    }
}
