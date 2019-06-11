using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios
{
    public partial class EspelhoPonto : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private BLL.Marcacao bllMarcacao;
        private BLL.RelatorioEspelho bllEspelho;
        private bool bCarrega { get; set; }

        public EspelhoPonto()
        {
            InitializeComponent();
            bllMarcacao = new BLL.Marcacao();
            bllEspelho = new BLL.RelatorioEspelho();
            this.Name = "EspelhoPonto";
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
            if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
            {
                MessageBox.Show("Data inicial não pode ser maior que Data final.");
            }
            else
            {
                this.Focus();
                FormProgressBar2 pb = new FormProgressBar2();
                try
                {
                    if (ValidaCampos())
                    {
                        this.Enabled = false;
                        pb.Show(this);
                        base.btOk_Click(sender, e);
                        string ids = "";
                        switch (rgTipo.SelectedIndex)
                        {
                                //Empresa
                            case 0:
                                ids = MontaStringEmpresas();
                                break;
                                //Departamento
                            case 1:
                                ids = MontaStringDepartamentos();
                                break;
                                //Funcionário
                            case 2:
                                ids = MontaStringFuncionarios();
                                break;
                        }
                        List<string> jornadas = new List<string>();
                        Dt = bllEspelho.GetEspelhoPontoRel(txtPeriodoI.DateTime, txtPeriodoF.DateTime, ids, rgTipo.SelectedIndex, pb.ObjProgressBar, jornadas);
                        pb.Dispose();
                        if (Dt.Rows.Count == 0)
                            throw new Exception("Não existem marcações registradas no período consultado.");
                        DataTable DtJornadas = bllEspelho.GetJornadasEspelho(jornadas, rgTipo.SelectedIndex);

                        nomerel = "rptEspelhoPonto.rdlc";
                        ds = "dsCartaoPonto_Espelho";
                        string dsJornada = "dsCartaoPonto_Jornada";

                        parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                        Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", txtPeriodoI.DateTime.ToShortDateString());
                        parametros.Add(p1);
                        Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", txtPeriodoF.DateTime.ToShortDateString());
                        parametros.Add(p2);

                        UI.Relatorios.FormRelatorioEspelho form = new UI.Relatorios.FormRelatorioEspelho(nomerel, ds, Dt, dsJornada, DtJornadas, parametros);

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
                    this.Close();
                }
                finally
                {
                    pb.Dispose();
                }
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
    }
}
