using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormAbono : UI.Base.ManutBase
    {
        private BLL.Afastamento bllAfastamento;
        private string TotalDiurna, TotalNoturna;
        public Modelo.Marcacao cwMarcacao; 

        public FormAbono()
        {
            InitializeComponent();
            this.Name = "FormAbono";
            bllAfastamento = new BLL.Afastamento();
        }

        public override void CarregaObjeto()
        {
            if (cwMarcacao.Afastamento.Id == 0 || String.IsNullOrEmpty(cwMarcacao.Afastamento.Horai) || String.IsNullOrEmpty(cwMarcacao.Afastamento.Horaf))
            {
                TotalDiurna = cwMarcacao.Horasfaltas;
                TotalNoturna = cwMarcacao.Horasfaltanoturna;                
            }
            else
            {
                TotalDiurna = BLL.CalculoHoras.OperacaoHoras('+', cwMarcacao.Horasfaltas, cwMarcacao.Afastamento.Horai);
                TotalNoturna = BLL.CalculoHoras.OperacaoHoras('+', cwMarcacao.Horasfaltanoturna, cwMarcacao.Afastamento.Horaf);
                
                txtAbonoDiurno.EditValue = cwMarcacao.Afastamento.Horai;
                txtAbonoNoturno.EditValue = cwMarcacao.Afastamento.Horaf;
                txtFaltaDiurna.EditValue = cwMarcacao.Horasfaltas;
                txtFaltaNoturna.EditValue = cwMarcacao.Horasfaltanoturna;
            }

            txtAbonoDiurno.DataBindings.Add("EditValue", cwMarcacao.Afastamento, "Horai", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAbonoNoturno.DataBindings.Add("EditValue", cwMarcacao.Afastamento, "Horaf", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFaltaDiurna.DataBindings.Add("EditValue", cwMarcacao, "Horasfaltas", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFaltaNoturna.DataBindings.Add("EditValue", cwMarcacao, "Horasfaltanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (Modelo.cwkFuncoes.ConvertHorasMinuto(TotalDiurna) < Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtAbonoDiurno.EditValue))
            {
                ret.Add("txtAbonoDiurno", "Abono diurno deve ser menor ou igual a falta diurna.");
            }

            if (Modelo.cwkFuncoes.ConvertHorasMinuto(TotalNoturna) < Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtAbonoNoturno.EditValue))
            {
                ret.Add("txtAbonoNoturno", "Abono noturno deve ser menor ou igual a falta noturna.");
            }

            if (ret.Count == 0)
            {
                cwMarcacao.Horastrabalhadas = BLL.CalculoHoras.OperacaoHoras('+', cwMarcacao.Horastrabalhadas, (string)txtAbonoDiurno.EditValue);
                cwMarcacao.Horastrabalhadasnoturnas = BLL.CalculoHoras.OperacaoHoras('+', cwMarcacao.Horastrabalhadasnoturnas, (string)txtAbonoNoturno.EditValue); 

                cwMarcacao.Afastamento.Horaf = (string)txtAbonoNoturno.EditValue;
                cwMarcacao.Afastamento.Horai = (string)txtAbonoDiurno.EditValue;

                cwMarcacao.Horasfaltas = (string)txtFaltaDiurna.EditValue;
                cwMarcacao.Horasfaltanoturna = (string)txtFaltaNoturna.EditValue;
            }


            return ret;
        }

        private void txtAbonoDiurno_Validating(object sender, CancelEventArgs e)
        {
            if (Modelo.cwkFuncoes.ConvertHorasMinuto(TotalDiurna) >= Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtAbonoDiurno.EditValue))
            {
                txtFaltaDiurna.EditValue = BLL.CalculoHoras.OperacaoHoras('-', TotalDiurna, (string)txtAbonoDiurno.EditValue);
            }
            else
            {
                dxErroProvider.SetError(txtAbonoDiurno, "Abono diurno deve ser menor ou igual a falta diurna.");
            }
        }

        private void txtAbonoNoturno_Validating(object sender, CancelEventArgs e)
        {
            if (Modelo.cwkFuncoes.ConvertHorasMinuto(TotalNoturna) >= Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtAbonoNoturno.EditValue))
            {
                txtFaltaNoturna.EditValue = BLL.CalculoHoras.OperacaoHoras('-', TotalNoturna, (string)txtAbonoNoturno.EditValue);
            }
            else
            {
                dxErroProvider.SetError(txtAbonoNoturno, "Abono noturno deve ser menor ou igual a falta noturna.");
            }
        }
    }
}
