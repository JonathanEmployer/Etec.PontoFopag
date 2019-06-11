using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;

namespace UI
{
    public partial class FormVisualizacaoResumoHoras : UI.Base.ManutBase
    {
        private Modelo.Funcionario objFuncionario;
        private DateTime dataInicial, dataFinal;

        public FormVisualizacaoResumoHoras(Modelo.Funcionario pFuncionario, DateTime pDataInicial, DateTime pDataFinal)
        {
            InitializeComponent();
            this.Name = "FormVisualizacaoResumoHoras";
            objFuncionario = pFuncionario;
            dataInicial = pDataInicial;
            dataFinal = pDataFinal;
        }

        private void FormVisualizacaoResumoHoras_Load(object sender, EventArgs e)
        {
            this.Text = "Resumo de Horas ";
            LimparLabelsPercentuais();

            lbldatai.Text = dataInicial.ToShortDateString();
            lbldataf.Text = dataFinal.ToShortDateString();
            var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(objFuncionario, dataInicial, dataFinal, null, null);
            Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(dataInicial, dataFinal);
            totalizadorHoras.TotalizeHorasEBancoHoras(objTotalHoras);

            vlrFuncionario.Text = objFuncionario.Nome;
            vlrCodigo.Text = objFuncionario.Dscodigo;
            vlrMatricula.Text = objFuncionario.Matricula;

            int Numerolabel = 1;
            foreach (var percentual in objTotalHoras.RateioHorasExtras.Keys.OrderBy(p=> p))
            {
                if (Numerolabel <= 5)
                {
                    SetaLabelsPercentual(splitContainer10.Panel1.Controls, objTotalHoras, ref Numerolabel, percentual);
                }
                else
                {
                    SetaLabelsPercentual(splitContainer10.Panel2.Controls, objTotalHoras, ref Numerolabel, percentual);
                }
            }

            vlrHeDiurna.Text = objTotalHoras.horasExtraDiurna;
            vlrHeNoturna.Text = objTotalHoras.horasExtraNoturna;

            vlrHfDiurna.Text = objTotalHoras.horasFaltaDiurna;
            vlrHfNoturna.Text = objTotalHoras.horasFaltaNoturna;

            vlrHtDiurna.Text = objTotalHoras.horasTrabDiurna;
            vlrHtNoturna.Text = objTotalHoras.horasTrabNoturna;

            vlrHfDsr.Text = objTotalHoras.horasDDSR;

            vlrSaldoAnterior.Text = objTotalHoras.saldoAnteriorBH + " " + (objTotalHoras.sinalSaldoAnteriorBH == '+' ? "Crédito" : (objTotalHoras.sinalSaldoAnteriorBH == '-' ? "Débito" : ""));
            vlrSaldoAtual.Text = objTotalHoras.saldoBHAtual + " " + (objTotalHoras.sinalSaldoBHAtual == '+' ? "Crédito" : (objTotalHoras.sinalSaldoBHAtual == '-' ? "Débito" : ""));
            vlrSaldoMes.Text = objTotalHoras.saldoBHPeriodo + " " + (objTotalHoras.sinalSaldoBHPeriodo == '+' ? "Crédito" : (objTotalHoras.sinalSaldoBHPeriodo == '-' ? "Débito" : ""));
            vlrCreditoBH.Text = objTotalHoras.creditoBHPeriodo;
            vlrDebitoBH.Text = objTotalHoras.debitoBHPeriodo;
        }

        private void LimparLabelsPercentuais()
        {
            string[] labelsTitulos = new string[] { "lblTituloD1", "lblTituloD2", "lblTituloN1", "lblTituloN2" };

            foreach (Control item in splitContainer10.Panel1.Controls)
            {
                if (!labelsTitulos.Contains(item.Name))
                    item.Text = String.Empty;
            }
            foreach (Control item in splitContainer10.Panel2.Controls)
            {
                if (!labelsTitulos.Contains(item.Name))
                    item.Text = String.Empty;
            }
        }

        private void SetaLabelsPercentual(Control.ControlCollection controls, Modelo.TotalHoras objTotalHoras, ref int Numerolabel, int percentual)
        {
            controls.Find("lblHe" + Numerolabel.ToString(), false)
                .First().Text = "Hora Extra " + String.Format("{0:000}", percentual) + "%:";

            controls.Find("vlrHe" + Numerolabel.ToString() + "D", false)
            .First().Text = Modelo.cwkFuncoes.ConvertMinutosHora(4, objTotalHoras.RateioHorasExtras[percentual].Diurno);


            controls.Find("vlrHe" + Numerolabel.ToString() + "N", false)
            .First().Text = Modelo.cwkFuncoes.ConvertMinutosHora(4, objTotalHoras.RateioHorasExtras[percentual].Noturno);

            Numerolabel++;
        }

    }
}
