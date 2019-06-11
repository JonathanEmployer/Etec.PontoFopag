using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class CartaoPontoDiariaPorEmpresa : REL.FormBaseSemEmpresa
    {
        private BLL.Marcacao bllMarcacao = new BLL.Marcacao();
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();
        private string idFuncionario;
        private int tipoHorario;
        
           



        public CartaoPontoDiariaPorEmpresa(DateTime pDataf, Modelo.Marcacao objMarcacao)
        {
            

            InitializeComponent();
            txtDataf.DateTime = pDataf;
            idFuncionario = "(" + Convert.ToString(objMarcacao.Id) + ")";
            tipoHorario = objMarcacao.Idhorario;
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            Dt = bllMarcacao.GetCartaoPontoRel(txtDataf.DateTime, txtDataf.DateTime, "", "", idFuncionario, 2, tipoHorario);
            nomerel = "rptCartaoPontoDiariaPorEmpresa.rdlc";
            ds = "dsCartaoPonto_DataTable1";
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Cartão Ponto Individual";
            form.Show();
            this.Close();
        }
    }
}
