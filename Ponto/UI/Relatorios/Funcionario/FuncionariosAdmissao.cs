using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionariosAdmissao : UI.Relatorios.Base.FormBase
    {
        private BLL.Funcionario bllFuncionario;
        public FuncionariosAdmissao()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FuncionariosAdmissao";
        }
        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            DTInicial.EditValue = null;
            DTFinal.EditValue = null;
            base.FormBase_Load(sender, e);
            Carrega();
        }

        private bool ValidaCampos()
        {
            if (DTInicial.DateTime == new DateTime())
            {
                dxErrorProvider1.SetError(DTInicial, "A data inicial deve ser preenchida.");
                return false;
            }

            if (DTFinal.DateTime == new DateTime())
            {
                dxErrorProvider1.SetError(DTFinal, "A data final deve ser preenchida.");
                return false;
            }

            if (DTInicial.DateTime != new DateTime() && DTFinal.DateTime != new DateTime())
            {
                if (DTInicial.DateTime.Date > DTFinal.DateTime.Date)
                {
                    dxErrorProvider1.SetError(DTFinal, "A data final deve ser maior que a data inicial.");
                    return false;
                }
            }

            return true;

        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            if (ValidaCampos())
                ChamaRelatorio(sender, e);
            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }

        private void ChamaRelatorio(object sender, EventArgs e)
        {
            base.btOk_Click(sender, e);

            Dt = bllFuncionario.GetPorDataAdmissaoRel(DTInicial.DateTime, DTFinal.DateTime, MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            parametros.Add(p1);
            nomerel = "rptFuncionariosAdmissao.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários Admitidos";
            form.Show();
            this.Close();
        }
    }
}
