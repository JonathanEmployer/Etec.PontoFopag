using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class FuncionariosPorEmpresa : REL.FormBase
    {
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();

        public FuncionariosPorEmpresa()
        {
            InitializeComponent();
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
                base.btOk_Click(sender, e);

                Dt = bllFuncionario.GetRelatorio(MontaStringEmpresas());
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                parametros.Add(p1);
                nomerel = "rptFuncionariosPorEmpresa.rdlc";
                ds = "dsFuncionarios_Funcionarios";
                FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Funcionários por Empresa";
                form.Show();
                this.Close();
        }
    }
}
