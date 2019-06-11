using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionariosPorCodigo : UI.Relatorios.Base.FormBase
    {
        private BLL.Funcionario bllFuncionario;

        public FuncionariosPorCodigo()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FuncionariosPorCodigo";
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (ValidaCampos())
            {
                base.btOk_Click(sender, e);

                Dt = bllFuncionario.GetOrdenadoPorCodigoRel(txtCodigoInicial.Value.ToString(), txtCodigoFinal.Value.ToString(), MontaStringEmpresas());
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("ordenacao", "Código");
                parametros.Add(p1);
                parametros.Add(p2);
                nomerel = "rptFuncionarios.rdlc";
                ds = "dsFuncionarios_Funcionarios";
                UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Funcionários por Código";
                form.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Preencha os campos corretamente.");
            }
            this.Enabled = true;
        }

        protected bool ValidaCampos()
        {
            bool ret = true;

            if (txtCodigoInicial.Value > txtCodigoFinal.Value)
            {
                dxErrorProvider1.SetError(txtCodigoFinal, "O código final deve ser maior do que o código inicial.");
                ret = false;
            }
            else
            {
                if (txtCodigoInicial.Value < 0)
                {
                    dxErrorProvider1.SetError(txtCodigoInicial, "O código deve ser maior do que zero.");
                    ret = false;
                }

                if (txtCodigoFinal.Value < 0)
                {
                    dxErrorProvider1.SetError(txtCodigoFinal, "O código deve ser maior do que zero.");
                    ret = false;
                }
            }

            return ret;
        }
    }
}
