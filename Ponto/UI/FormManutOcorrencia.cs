using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutOcorrencia : UI.Base.ManutBase
    {
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.Empresa bllEmpresa;
        private Modelo.Ocorrencia objOcorrencia; 

        public FormManutOcorrencia()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllOcorrencia = new BLL.Ocorrencia();
            this.Name = "FormManutOcorrencia";
            chbAbsenteismo.Visible = bllEmpresa.RelatorioAbsenteismoLiberado();
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objOcorrencia= new Modelo.Ocorrencia();
                    objOcorrencia.Codigo = bllOcorrencia.MaxCodigo();
                    objOcorrencia.Descricao = "";
                    break;
                default:
                    objOcorrencia= bllOcorrencia.LoadObject(cwID);
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objOcorrencia, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objOcorrencia, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            chbAbsenteismo.DataBindings.Add("Checked", objOcorrencia, "Absenteismo", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            objOcorrencia.Descricao = objOcorrencia.Descricao.TrimEnd();
            base.Salvar();
            return bllOcorrencia.Salvar(cwAcao, objOcorrencia);
        }
    }
}
