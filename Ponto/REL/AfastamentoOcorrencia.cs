using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class AfastamentoOcorrencia : REL.FormBaseGridFiltro1
    {
        private BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia();
        protected List<Modelo.Afastamento> listaAfastamentos = new List<Modelo.Afastamento>();
        protected List<int> listaRowHandleAfastamento = new List<int>();

        protected BLL.Afastamento bllAfastamento = new BLL.Afastamento();
        protected Modelo.Afastamento objAfastamento = new Modelo.Afastamento();

        public AfastamentoOcorrencia()
        {
            InitializeComponent();
        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            base.FormBase_Load(sender, e);
            rgTipo.SelectedIndex = -1;
            Carrega();
            cbIdOcorrencia.Properties.DataSource = bllOcorrencia.GetAll();            
            setaNomeArquivo(this.Name);
            LeXML();
        }

       protected virtual void tipo()
       {
           if(rgTipo.SelectedIndex == 0)
           {
               Tipo = "Empresa";
           }
           else if (rgTipo.SelectedIndex == 1)
           {
               Tipo = "Departamento";
           }
           else if(rgTipo.SelectedIndex == 2)
           {
               Tipo = "Funcionário";
           }   
       }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                base.btOk_Click(sender, e);
                tipo();
                
                Dt = bllAfastamento.GetAfastamentoPorOcorrenciaRel(MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo(), (int)cbIdOcorrencia.EditValue);
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                parametros.Add(p1);
                Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("tipo", Tipo);
                parametros.Add(p2);
                nomerel = "rptAfastamentoPorOcorrencia.rdlc";
                ds = "dsAfastamento_DataTable1";
                FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Afastamento por Ocorrência";
                form.Show();
                this.Close();
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
