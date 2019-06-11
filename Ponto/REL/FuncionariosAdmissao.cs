using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class FuncionariosAdmissao : REL.FormBase
    {
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();
        public FuncionariosAdmissao()
        {
            InitializeComponent();
        }
        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            DTInicial.EditValue = null;
            DTFinal.EditValue = null;
            base.FormBase_Load(sender, e);
            Carrega();           
        }
        
        protected override void btOk_Click(object sender, EventArgs e)
        {

            base.btOk_Click(sender, e);

            Dt = bllFuncionario.GetPorDataAdmissaoRel(DTInicial.DateTime, DTFinal.DateTime, MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            parametros.Add(p1);
            nomerel = "rptFuncionariosAdmissao.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários Admitidos";
            form.Show();
            this.Close();
        }
    }
}
