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
    public partial class CartaoPonto : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private BLL.Marcacao bllMarcacao;
        private BLL.Parametros bllParametro;
        private BLL.CartaoPonto bllCartaoPonto;
        private bool bCarrega { get; set; }

        public CartaoPonto()
        {
            InitializeComponent();
            this.Name = "CartaoPonto";
            rgTipo.SelectedIndex = -1;
            rgOrientacao.SelectedIndex = 0;
            rgTipoTurno.SelectedIndex = 0;
            bCarrega = true;
            bllMarcacao = new BLL.Marcacao();
            bllParametro = new BLL.Parametros();
            bllCartaoPonto = new BLL.CartaoPonto();
        }

        public CartaoPonto(DateTime dataInicial, DateTime dataFinal, Modelo.Funcionario pFuncionario)
        {
            InitializeComponent();
            rgTipoTurno.SelectedIndex = 0;
            rgOrientacao.SelectedIndex = 0;
            txtPeriodoI.EditValue = dataInicial;
            txtPeriodoF.EditValue = dataFinal;
            objFuncionario = pFuncionario;
            bCarrega = false;
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
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            FormProgressBar2 pb = new FormProgressBar2();
            try
            {
                if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
                {
                    MessageBox.Show("Data inicial não pode ser maior que Data final.");
                }
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
                            Dt = bllCartaoPonto.GetCartaoPontoRel(txtPeriodoI.DateTime, txtPeriodoF.DateTime, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo(), rgTipoTurno.SelectedIndex, 0, pb.ObjProgressBar, chkAgruparPorDepartamento.Checked, "");
                            pb.Dispose();

                            Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                            Microsoft.Reporting.WinForms.ReportParameter p1 = 
                                new Microsoft.Reporting.WinForms.ReportParameter("datainicial", 
                                    txtPeriodoI.DateTime.ToShortDateString());
                            parametros.Add(p1);
                            Microsoft.Reporting.WinForms.ReportParameter p2 = 
                                new Microsoft.Reporting.WinForms.ReportParameter("datafinal", 
                                    txtPeriodoF.DateTime.ToShortDateString());
                            parametros.Add(p2);
                            Microsoft.Reporting.WinForms.ReportParameter p3 = 
                                new Microsoft.Reporting.WinForms.ReportParameter("observacao", 
                                    objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                            parametros.Add(p3);
                            Microsoft.Reporting.WinForms.ReportParameter p4 = 
                                new Microsoft.Reporting.WinForms.ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                            parametros.Add(p4);

                            Microsoft.Reporting.WinForms.ReportParameter p5 =
                                new Microsoft.Reporting.WinForms.ReportParameter("ordenadepartamento", chkAgruparPorDepartamento.Checked.ToString());
                            parametros.Add(p5);

                            Microsoft.Reporting.WinForms.ReportParameter p6 =
                                new Microsoft.Reporting.WinForms.ReportParameter("visible", true.ToString());
                            parametros.Add(p6);

                            if (rgOrientacao.SelectedIndex == 0)
                                nomerel = "rptCartaoPontoIndividual.rdlc";
                            else
                                nomerel = "rptCartaoPontoIndividualPaisagem.rdlc";
                           
                            ds = "dsCartaoPonto_DataTable1";

                            UI.Relatorios.Base.FormRelatorioBase form =
                                new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                            form.Text = "Relatório de Cartão Ponto Individual";
                            form.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("O intervalo entre as datas não pode ser maior do que 31 dias.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Preencha os campos corretamente.");
                    }
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


        public DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

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

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }

        private void CartaoPonto_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
