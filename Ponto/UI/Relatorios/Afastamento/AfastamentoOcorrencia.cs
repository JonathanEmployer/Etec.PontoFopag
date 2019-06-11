using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios.Afastamento
{
    public partial class AfastamentoOcorrencia : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private BLL.Ocorrencia bllOcorrencia;
        protected List<Modelo.Afastamento> listaAfastamentos = new List<Modelo.Afastamento>();
        protected List<int> listaRowHandleAfastamento = new List<int>();

        protected BLL.Afastamento bllAfastamento;
        protected Modelo.Afastamento objAfastamento = new Modelo.Afastamento();

        public AfastamentoOcorrencia()
        {
            InitializeComponent();
            bllAfastamento = new BLL.Afastamento();
            bllOcorrencia = new BLL.Ocorrencia();
            this.Name = "AfastamentoOcorrencia";
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

       private bool ValidaCampos()
       {
           if ((int)cbIdOcorrencia.EditValue == 0)
           {
               MessageBox.Show("Escolha uma ocorrência.");
               return false;
           }
           
           return true;
       }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            if (ValidaCampos())
                ChamaRelatorio(sender, e);
            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }

        private void ChamaRelatorio(object sender, EventArgs e)
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
                UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
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

        private void AfastamentoOcorrencia_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
