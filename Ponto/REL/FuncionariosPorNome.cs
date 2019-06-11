using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class FuncionariosPorNome : REL.FormBase
    {
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();

        public FuncionariosPorNome()
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

                Dt = bllFuncionario.GetOrdenadoPorNomeRel((string)txtLetraInicial.EditValue, (string)txtLetraFinal.EditValue, MontaStringEmpresas());
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("ordenacao", "Nome");
                parametros.Add(p1);
                parametros.Add(p2);
                nomerel = "rptFuncionarios.rdlc";
                ds = "dsFuncionarios_Funcionarios";
                FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Funcionários por Nome";
                form.Show();
                this.Close();
        }
    }
}
