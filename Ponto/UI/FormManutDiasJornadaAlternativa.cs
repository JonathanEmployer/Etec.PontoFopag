using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutDiasJornadaAlternativa : UI.Base.ManutBase
    {
        private BLL.DiasJornadaAlternativa bllDiasJornadaAlternativa;
        private Modelo.DiasJornadaAlternativa objDiasJornadaAlternativa;
        private Modelo.JornadaAlternativa objJornadaAlternativa;

        public FormManutDiasJornadaAlternativa(Modelo.JornadaAlternativa pJornadaAlternativa)
        {
            InitializeComponent();
            bllDiasJornadaAlternativa = new BLL.DiasJornadaAlternativa();
            objJornadaAlternativa = pJornadaAlternativa;
            this.Name = "FormManutDiasJornadaAlternativa";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objDiasJornadaAlternativa = new Modelo.DiasJornadaAlternativa();
                    objDiasJornadaAlternativa.Codigo = bllDiasJornadaAlternativa.MaxCodigo(objJornadaAlternativa.DiasJA);
                    objDiasJornadaAlternativa.DataCompensada = null;
                    break;
                default:
                    foreach (Modelo.DiasJornadaAlternativa cp in objJornadaAlternativa.DiasJA)
                    {
                        if (cp.Codigo == cwID)
                        {
                            objDiasJornadaAlternativa = cp;
                            break;
                        }
                    }
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objDiasJornadaAlternativa, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataCompensada.DataBindings.Add("DateTime", objDiasJornadaAlternativa, "DataCompensada", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllDiasJornadaAlternativa.Salvar(objDiasJornadaAlternativa, objJornadaAlternativa.DiasJA, cwAcao);
        }
    }
}
