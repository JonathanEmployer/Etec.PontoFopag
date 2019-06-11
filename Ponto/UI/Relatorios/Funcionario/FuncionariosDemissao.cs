using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionariosDemissao : UI.Relatorios.Base.FormBase
    {
        private BLL.Funcionario bllFuncionario;

        public FuncionariosDemissao()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            this.Name = "FuncionariosDemissao";
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
            if (DTInicial.EditValue == null || DTFinal.EditValue == null)
            {
                MessageBox.Show("Preencha o período do relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (DTFinal.DateTime < DTInicial.DateTime)
            {
                MessageBox.Show("A data inicial não pode ser maior do que a data final.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                btOk.Enabled = false;
                btCancelar.Enabled = false;
                base.btOk_Click(sender, e);
                try
                {
                    Dt = bllFuncionario.GetPorDataDemissaoRel(DTInicial.DateTime, DTFinal.DateTime, MontaStringEmpresas());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao gerar relatório: " + Environment.NewLine + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    btOk.Enabled = true;
                    btCancelar.Enabled = true;
                    return;
                }
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                parametros.Add(p1);
                nomerel = "rptFuncionariosDemissao.rdlc";
                ds = "dsFuncionarios_Funcionarios";
                UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Funcionários Demitidos";
                form.Show();
                this.Close();
                btOk.Enabled = true;
                btCancelar.Enabled = true;
            }
        }
    }
}
