using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class ImprimeCartaoPontoDiaria : REL.FormBaseSemEmpresa
    {
        private BLL.Marcacao bllMarcacao = new BLL.Marcacao();
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();
        private string idFuncionario;
        private int tipoHorario;

        public ImprimeCartaoPontoDiaria(DateTime pDataf, Modelo.Funcionario objFuncionario)
        {
            objFuncionario = bllFuncionario.LoadObject(objFuncionario.Id);
            InitializeComponent();
            txtDataf.DateTime = pDataf;
            idFuncionario = "(" + Convert.ToString(objFuncionario.Id) + ")";
            tipoHorario = objFuncionario.Tipohorario;
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            Dt = bllMarcacao.GetCartaoPontoRel(txtDataf.DateTime, txtDataf.DateTime, "", "", idFuncionario, 2, tipoHorario);
            nomerel = "rptCartaoPontoDiaria.rdlc";
            ds = "dsCartaoPonto_DataTable1";
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal",txtDataf.DateTime.ToShortDateString());
            parametros.Add(p1);
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Cartão Ponto Individual";
            form.Show();
            this.Close();
        }
    }
}
