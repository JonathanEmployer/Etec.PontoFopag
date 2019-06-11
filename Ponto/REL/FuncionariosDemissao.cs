using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class FuncionariosDemissao : REL.FormBase
    {
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();

        public FuncionariosDemissao()
        {
            InitializeComponent();
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
            DTInicial.EditValue = null;
            DTFinal.EditValue = null;
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            base.btOk_Click(sender, e);
            Dt = bllFuncionario.GetPorDataDemissaoRel(DTInicial.DateTime, DTFinal.DateTime, MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            parametros.Add(p1);
            nomerel = "rptFuncionariosDemissao.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários Demitidos";
            form.Show();
            this.Close();
        }
    }
}
