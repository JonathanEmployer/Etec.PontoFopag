using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class EspelhoPonto : REL.FormBaseGridFiltro1
    {
        private BLL.Marcacao bllMarcacao = new BLL.Marcacao();
        private bool bCarrega { get; set; }
        private Modelo.Funcionario objFuncionario = null;

        public EspelhoPonto()
        {
            InitializeComponent();
            rgTipo.SelectedIndex = -1;            
            bCarrega = true;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }

        private void SelecionarFuncionario()
        {
            rgTipo.SelectedIndex = 2;
            SelecionaRegistroPorID("id", objFuncionario.Idempresa, gvEmpresas);
            SelecionaRegistroPorID("id", objFuncionario.Iddepartamento, gvDepartamentos);
            SelecionaRegistroPorID("id", objFuncionario.Id, gvFuncionarios);
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            setaNomeArquivo(this.Name);
            if (bCarrega)
            {
                Carrega();
                LeXML();
            }
            else
            {
                SelecionarFuncionario();
            }

        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidaCampos())
                {
                    base.btOk_Click(sender, e);

                    Dt = bllMarcacao.GetEspelhoPontoRel(txtPeriodoI.DateTime, txtPeriodoF.DateTime, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo());
                    parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                    Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", txtPeriodoI.DateTime.ToShortDateString());
                    parametros.Add(p1);
                    Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", txtPeriodoF.DateTime.ToShortDateString());
                    parametros.Add(p2);
                    nomerel = "rptEspelhoPonto.rdlc";
                    ds = "dsCartaoPonto_Espelho";
                    FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                    form.Text = "Relatório Espelho de Ponto Eletrônico";
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

        private void btOk_Click_1(object sender, EventArgs e)
        {

        }
    }
}
