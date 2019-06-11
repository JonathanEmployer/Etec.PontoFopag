using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutDiasCompensacao : UI.Base.ManutBase
    {
        private BLL.DiasCompensacao bllDiasCompensacao;
        private Modelo.DiasCompensacao objDiasCompensacao;
        private Modelo.Compensacao objCompensacao;

        public FormManutDiasCompensacao(Modelo.Compensacao pCompensacao)
        {
            InitializeComponent();
            bllDiasCompensacao = new BLL.DiasCompensacao();
            objCompensacao = pCompensacao;
            this.Name = "FormManutDiasCompensacao";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objDiasCompensacao = new Modelo.DiasCompensacao();
                    objDiasCompensacao.Codigo = bllDiasCompensacao.MaxCodigo(objCompensacao.DiasC);
                    objDiasCompensacao.Datacompensada = null;
                    break;
                default:
                    foreach (Modelo.DiasCompensacao cp in objCompensacao.DiasC)
                    {
                        if (cp.Codigo == cwID)
                        {
                            objDiasCompensacao = cp;
                            break;
                        }
                    }
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objDiasCompensacao, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataCompensada.DataBindings.Add("DateTime", objDiasCompensacao, "DataCompensada", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();

        }
        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllDiasCompensacao.Salvar(objDiasCompensacao, objCompensacao.DiasC, cwAcao);
        }

        protected override void ChamaHelp()
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, "formmanutcompensacao.htm");
        }
    }       
}
