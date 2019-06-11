using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class AfastamentoTipo : REL.FormBaseGridFiltro1
    {
        protected List<Modelo.Afastamento> listaAfastamentos = new List<Modelo.Afastamento>();
        protected List<int> listaRowHandleAfastamento = new List<int>();

        protected BLL.Afastamento bllAfastamento = new BLL.Afastamento();
        protected Modelo.Afastamento objAfastamento = new Modelo.Afastamento();

        public AfastamentoTipo()
        {
            InitializeComponent();
        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            base.FormBase_Load(sender, e);
            rgTipo.SelectedIndex = -1;
            txtPeriodoI.EditValue = null;
            txtPeriodoF.EditValue = null;      
            Carrega();
            setaNomeArquivo(this.Name);
            LeXML();
        }

        public bool ValidaCampos()
        {
            bool ret = true;

            if (txtPeriodoI.DateTime == new DateTime() || txtPeriodoI.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoI, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoI, "");
            }

            if (txtPeriodoF.DateTime == new DateTime() || txtPeriodoF.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoF, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoF, "");
            }

            return ret;
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos())
                {

                    base.btOk_Click(sender, e);

                    string Tipo = "";
                    switch (rgTipo.SelectedIndex)
                    {
                        case 0:
                            Tipo = "Empresa";
                            break;
                        case 1:
                            Tipo = "Departamento";
                            break;
                        case 2:
                            Tipo = "Funcionário";
                            break;
                    }

                    Dt = bllAfastamento.GetPorAfastamentoRel(MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo());
                    parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                    Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                    parametros.Add(p1);
                    Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("tipo", Tipo);
                    parametros.Add(p2);
                    nomerel = "rptAfastamentoPorTipo.rdlc";
                    ds = "dsAfastamento_DataTable1";
                    FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                    form.Text = "Relatório de Afastamento por Tipo";
                    form.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Preencha os campos corretamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }
    }
}
