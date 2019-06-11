using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutJustificativa : UI.Base.ManutBase
    {
        private BLL.Justificativa bllJustificativa;
        private Modelo.Justificativa objJustificativa;

        public FormManutJustificativa()
        {
            InitializeComponent();
            bllJustificativa = new BLL.Justificativa();
            this.Name = "FormManutJustificativa";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objJustificativa = new Modelo.Justificativa();
                    objJustificativa.Codigo = bllJustificativa.MaxCodigo();
                    break;
                default:
                    objJustificativa = bllJustificativa.LoadObject(cwID);
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objJustificativa, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objJustificativa, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllJustificativa.Salvar(cwAcao, objJustificativa);
        }
    }
}
