using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionariosPorNome : UI.Relatorios.Base.FormBase
    {
        private BLL.Funcionario bllFuncionario;

        public FuncionariosPorNome()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FuncionariosPorNome";
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

            Dt = bllFuncionario.GetOrdenadoPorNomeRel((string)txtLetraInicial.EditValue, (string)txtLetraFinal.EditValue, MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("ordenacao", "Nome");
            parametros.Add(p1);
            parametros.Add(p2);
            nomerel = "rptFuncionarios.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários por Nome";
            form.Show();
            this.Close();
            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }
    }
}
