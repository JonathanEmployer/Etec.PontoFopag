using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Modelo;
using System.Xml;
using System.IO;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionarioHistorico : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private BLL.FuncionarioHistorico bllFuncionarioHistorico;

        public FuncionarioHistorico()
        {
            InitializeComponent();
            bllFuncionarioHistorico = new BLL.FuncionarioHistorico();
            this.Name = "FuncionarioHistorico";
        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            txtPeriodoI.EditValue = null;
            txtPeriodoF.EditValue = null;
            base.FormBase_Load(sender, e);
            Carrega();
            rgTipo.SelectedIndex = -1;
            setaNomeArquivo(this.Name);
            LeXML();
            
        }
        private bool ValidaCampos()
        {
            if (txtPeriodoI.DateTime == new DateTime() || txtPeriodoF.DateTime == new DateTime())
            {
                MessageBox.Show("O relatorio deve ter um período inicial e final");
                return false;
            }
            if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
            {
                MessageBox.Show("Período inicial deve ser menor que o final");
                return false;
            }

            return true;
        }
        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                btOk.Enabled = false;
                btCancelar.Enabled = false;
                if (ValidaCampos())
                {
                    base.btOk_Click(sender, e);
                    Dt = bllFuncionarioHistorico.LoadRelatorio(txtPeriodoI.DateTime, txtPeriodoF.DateTime, rgTipo.SelectedIndex, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios());
                    parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                    Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                    parametros.Add(p1);
                    nomerel = "rptHistorico.rdlc";
                    ds = "dsHistorico_DataTable1";
                    UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                    form.Text = "Relatório de Histórico Funcionário";
                    form.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btOk.Enabled = true;
                btCancelar.Enabled = true;
            }
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }
    }
}

