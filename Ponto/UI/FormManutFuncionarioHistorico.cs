using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutFuncionarioHistorico : UI.Base.ManutBase
    {
        private BLL.FuncionarioHistorico bllFuncionarioHistorico;
        private Modelo.FuncionarioHistorico objFuncionarioHistorico;
        private Modelo.Funcionario objFuncionario;

        public FormManutFuncionarioHistorico(Modelo.Funcionario pFuncionario)
        {
            InitializeComponent();
            bllFuncionarioHistorico = new BLL.FuncionarioHistorico();
            objFuncionario = pFuncionario;
            this.Name = "FormManutFuncionarioHistorico";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objFuncionarioHistorico = new Modelo.FuncionarioHistorico();
                    objFuncionarioHistorico.Codigo = bllFuncionarioHistorico.MaxCodigo(objFuncionario.Historico);
                    objFuncionarioHistorico.Data = null;
                    objFuncionarioHistorico.Hora = null;
                    break;
                default:
                    foreach (Modelo.FuncionarioHistorico cp in objFuncionario.Historico)
                    {
                        if (cp.Codigo == cwID)
                        {
                            objFuncionarioHistorico = cp;
                            break;
                        }
                    }
                    break;
            }            

            txtCodigo.DataBindings.Add("EditValue", objFuncionarioHistorico, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtData.DataBindings.Add("DateTime", objFuncionarioHistorico, "Data", true, DataSourceUpdateMode.OnPropertyChanged);
            txtHora.DataBindings.Add("EditValue", objFuncionarioHistorico, "Hora", true, DataSourceUpdateMode.OnPropertyChanged);
            txtHistorico.DataBindings.Add("EditValue", objFuncionarioHistorico, "Historico", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {            
            base.Salvar();
            return bllFuncionarioHistorico.Salvar(objFuncionarioHistorico, objFuncionario.Historico, cwAcao);
        }
        
    }
}
