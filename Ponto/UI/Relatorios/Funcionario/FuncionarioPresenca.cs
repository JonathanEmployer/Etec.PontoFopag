﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Funcionario
{
    public partial class FuncionarioPresenca : UI.Relatorios.Base.FormBaseGridFiltro1
    {        
        public FuncionarioPresenca()
        {
            InitializeComponent();
            this.Name = "FuncionarioPresenca";

        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            base.FormBase_Load(sender, e);
            txtPeriodoI.EditValue = null;
            Carrega();
            rgTipo.SelectedIndex = -1;
            setaNomeArquivo(this.Name);
            LeXML();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                btOk.Enabled = false;
                btCancelar.Enabled = false;
                if (txtPeriodoI.DateTime != new DateTime())
                {
                    string data;
                    data = Convert.ToString(txtPeriodoI.DateTime);

                    base.btOk_Click(sender, e);

                    Dt = bllFuncionario.GetListaPresenca((DateTime)txtPeriodoI.EditValue, MontaIntTipo(), MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios());
                    parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                    Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                    parametros.Add(p1);
                    Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("data", data);
                    parametros.Add(p2);
                    nomerel = "rptFuncionariosPresenca.rdlc";
                    ds = "Presenca_DataTablePresenca";
                    UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                    form.Text = "Relatório de Presença por Funcionário";
                    form.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Preencha a data.");
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
