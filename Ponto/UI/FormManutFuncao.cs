using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutFuncao : UI.Base.ManutBase
    {
        private BLL.Funcao bllFuncao;
        private Modelo.Funcao objFuncao; 

        public FormManutFuncao()
        {
            InitializeComponent();
            bllFuncao = new BLL.Funcao();
            this.Name = "FormManutFuncao";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objFuncao = new Modelo.Funcao();
                    objFuncao.Codigo = bllFuncao.MaxCodigo();
                    objFuncao.Descricao = "";
                    break;
                default:
                    objFuncao = bllFuncao.LoadObject(cwID);
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objFuncao, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objFuncao, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            objFuncao.Descricao = objFuncao.Descricao.TrimEnd();
            base.Salvar();
            return bllFuncao.Salvar(cwAcao, objFuncao);
        }
    }
}
