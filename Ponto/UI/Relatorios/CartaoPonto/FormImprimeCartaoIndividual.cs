using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Modelo.Proxy;

namespace UI.Relatorios.CartaoPonto
{
    public partial class FormImprimeCartaoIndividual : UI.Relatorios.Base.FormBaseSemEmpresa
    {
        private BLL.CartaoPonto bllCartaoPonto;
        private BLL.Parametros bllParametro;
        private string idFuncionario;
        private int idHorario;

        public FormImprimeCartaoIndividual(DateTime pDatai, DateTime pDataf, Modelo.Funcionario objFuncionario)
        {
            InitializeComponent();
            this.Name = "FormImprimeCartaoIndividual";
            if (pDatai != new DateTime())
                txtDatai.DateTime = pDatai;
            else
                txtDatai.DateTime = DateTime.Now.Date;
            if (pDataf != new DateTime())
                txtDataf.DateTime = pDataf;
            else
                txtDataf.DateTime = DateTime.Now.Date;
            idFuncionario = "(" + Convert.ToString(objFuncionario.Id) + ")";
            idHorario = objFuncionario.Idhorario;
            bllCartaoPonto = new BLL.CartaoPonto();
            bllParametro = new BLL.Parametros();
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            if (txtDatai.DateTime > txtDataf.DateTime)
            {
                MessageBox.Show("Data inicial não pode ser maior que Data final.");
            }

            else
            {
                TimeSpan ts = txtDataf.DateTime - txtDatai.DateTime;

                if (ts.Days <= 30)
                {

                    btOk.Enabled = false;
                    btCancelar.Enabled = false;
                    try
                    {
                        FormProgressBar2 pb = new FormProgressBar2();
                        Dt = bllCartaoPonto.GetCartaoPontoRel(txtDatai.DateTime, txtDataf.DateTime, "", "", idFuncionario, 2, 0, idHorario, pb.ObjProgressBar, false, "");
                        pb.Dispose();
                        nomerel = "rptCartaoPontoIndividual.rdlc";
                        ds = "dsCartaoPonto_DataTable1";

                        Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                        string observacao = objParametro.CampoObservacao;

                        parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                        Microsoft.Reporting.WinForms.ReportParameter p1 = 
                            new Microsoft.Reporting.WinForms.ReportParameter("datainicial", txtDatai.DateTime.ToShortDateString());
                        parametros.Add(p1);
                        Microsoft.Reporting.WinForms.ReportParameter p2 = 
                            new Microsoft.Reporting.WinForms.ReportParameter("datafinal", txtDataf.DateTime.ToShortDateString());
                        parametros.Add(p2);
                        Microsoft.Reporting.WinForms.ReportParameter p3 = 
                            new Microsoft.Reporting.WinForms.ReportParameter("observacao", objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                        parametros.Add(p3);
                        Microsoft.Reporting.WinForms.ReportParameter p4 = 
                            new Microsoft.Reporting.WinForms.ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                        parametros.Add(p4);
                        Microsoft.Reporting.WinForms.ReportParameter p5 =
                                new Microsoft.Reporting.WinForms.ReportParameter("ordenadepartamento", false.ToString());
                        parametros.Add(p5);
                        Microsoft.Reporting.WinForms.ReportParameter p6 =
                                new Microsoft.Reporting.WinForms.ReportParameter("visible", true.ToString());
                        parametros.Add(p6); 

                        UI.Relatorios.Base.FormRelatorioBase form =
                            new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                        form.Text = "Relatório de Cartão Ponto Individual";
                        form.formParaClose = this;
                        form.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("O intervalo entre as datas não pode ser maior do que 31 dias.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        protected override void ChamaHelp()
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, "CartaoPonto.htm");
        }
    }
}
