using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios
{
    public partial class FormRelatorioAcessoFuncionario : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private bool bCarrega { get; set; }
        private BLL.Empresa bllEmpresa;
        private BLL.MarcacaoAcesso marcacaoAcesso;

        public FormRelatorioAcessoFuncionario()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            marcacaoAcesso = new BLL.MarcacaoAcesso();
            rgTipo.SelectedIndex = -1;
            rgTipoRelatorio.SelectedIndex = 0;
            bCarrega = true;
        }

        public FormRelatorioAcessoFuncionario(DateTime dataInicial, DateTime dataFinal, Modelo.Funcionario pFuncionario)
        {
            InitializeComponent();
            rgTipoRelatorio.SelectedIndex = 0;
            txtPeriodoI.EditValue = dataInicial;
            txtPeriodoF.EditValue = dataFinal;
            objFuncionario = pFuncionario;
            bCarrega = false;
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

        private void SelecionarFuncionario()
        {
            rgTipo.SelectedIndex = 2;
            SelecionaRegistroPorID("id", objFuncionario.Idempresa, gvEmpresas);
            SelecionaRegistroPorID("id", objFuncionario.Iddepartamento, gvDepartamentos);
            SelecionaRegistroPorID("id", objFuncionario.Id, gvFuncionarios);
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            FormProgressBar2 pb = new FormProgressBar2();
            try
            {
                if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
                    MessageBox.Show("Data inicial não pode ser maior que Data final.");
                else
                {
                    if (ValidaCampos())
                    {
                        TimeSpan ts = txtPeriodoF.DateTime - txtPeriodoI.DateTime;

                        if (ts.Days <= 30)
                        {
                            pb.Show(this);
                            pb.SetaMensagem("Carregando dados...");
                            base.btOk_Click(sender, e);

                            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                            Microsoft.Reporting.WinForms.ReportParameter p1;
                            Microsoft.Reporting.WinForms.ReportParameter p2;
                            Microsoft.Reporting.WinForms.ReportParameter p3;

                            var empresa = bllEmpresa.GetEmpresaPrincipal();
                            if (rgTipoRelatorio.SelectedIndex == 0) // Analítico (por horário)
                            {
                                Dt = marcacaoAcesso.GetAcessosAnaliticos(txtPeriodoI.DateTime, txtPeriodoF.DateTime, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo());
                                nomerel = "rptAcessoFuncionariosAnalitico.rdlc";
                                ds = "dsAcessoFuncionarioAnalitico_Analitico";                                
                            }
                            else // Sintático (por ticket)
                            {
                                Dt = marcacaoAcesso.GetAcessosSintaticos(txtPeriodoI.DateTime, txtPeriodoF.DateTime, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo());
                                nomerel = "rptAcessoFuncionariosSintetico.rdlc";
                                ds = "dsAcessoFuncionarioAnalitico_Sintetico";
                            }

                            p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", txtPeriodoI.DateTime.Date.ToString());
                            p3 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", txtPeriodoF.DateTime.Date.ToString());
                            p2 = new Microsoft.Reporting.WinForms.ReportParameter("empresaprincipal", empresa.Nome);

                            parametros.Add(p1);
                            parametros.Add(p2);
                            parametros.Add(p3);
                            pb.Dispose();

                            UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                            form.Text = "Relatório de Acesso Refeitório";
                            form.Show();
                            this.Close();
                        }
                        else
                            MessageBox.Show("O intervalo entre as datas não pode ser maior do que 31 dias.");
                    }
                    else
                        MessageBox.Show("Preencha os campos corretamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pb.Dispose();
                btOk.Enabled = true;
                btCancelar.Enabled = true;
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
                dxErrorProvider1.SetError(txtPeriodoI, "");

            if (txtPeriodoF.DateTime == new DateTime() || txtPeriodoF.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoF, "Campo obrigatório.");
                ret = false;
            }
            else
                dxErrorProvider1.SetError(txtPeriodoF, "");

            return ret;
        }

    }
}
