using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionariosAtivosInativos : UI.Relatorios.Base.FormBase
    {
        private BLL.Funcionario bllFuncionario;

        public FuncionariosAtivosInativos()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FuncionariosAtivosInativos";
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
            rgTipo.SelectedIndex = 1;
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;

            base.btOk_Click(sender, e);
            bool Tipo;
            string Ordenacao;
            if (rgTipo.SelectedIndex == 0)
            {
                Tipo = true;
                Ordenacao = "Ativos";
            }
            else
            {
                Tipo = false;
                Ordenacao = "Inativos";
            }

            Dt = bllFuncionario.GetAtivosInativosRel(Tipo, MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("ordenacao", Ordenacao);
            parametros.Add(p1);
            parametros.Add(p2);
            nomerel = "rptFuncionariosAtivosInativos.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários Ativos/Inativos";
            form.Show();
            this.Close();

            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }


    }
}
