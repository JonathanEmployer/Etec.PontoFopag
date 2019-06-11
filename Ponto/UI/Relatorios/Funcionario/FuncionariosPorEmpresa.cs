using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionariosPorEmpresa : UI.Relatorios.Base.FormBase
    {
        private BLL.Funcionario bllFuncionario;

        public FuncionariosPorEmpresa()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FuncionariosPorEmpresa";
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            base.btOk_Click(sender, e);

            Dt = bllFuncionario.GetRelatorio(MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            parametros.Add(p1);
            nomerel = "rptFuncionariosPorEmpresa.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários por Empresa";
            form.Show();
            this.Close();
            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }
    }
}
