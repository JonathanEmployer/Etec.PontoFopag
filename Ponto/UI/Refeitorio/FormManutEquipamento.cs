using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutEquipamento : UI.Base.ManutBase
    {
        private BLL.Equipamento bllEquipamento;
        private Modelo.Equipamento objEquipamento;

        public FormManutEquipamento()
        {
            InitializeComponent();
            bllEquipamento = new BLL.Equipamento();
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objEquipamento = new Modelo.Equipamento();
                    objEquipamento.Codigo = bllEquipamento.MaxCodigo();

                    objEquipamento.TipoCartao = -1;
                    objEquipamento.TipoLeitorOn = -1;
                    objEquipamento.OperaLeitor1On = -1;
                    objEquipamento.Acionamento1On = -1;
                    objEquipamento.Acionamento2On = -1;
                    objEquipamento.FormasEntradas = -1;
                    break;
                default:
                    objEquipamento = bllEquipamento.LoadObject(cwID);
                    break;
            }

            #region TabDados

            txtCodigo.DataBindings.Add("EditValue", objEquipamento, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNumInner.DataBindings.Add(new Binding("EditValue", objEquipamento, "NumInner", true, DataSourceUpdateMode.OnPropertyChanged));
            txtDescricao.DataBindings.Add(new Binding("EditValue", objEquipamento, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged));
            txtMensagemPadrao.DataBindings.Add(new Binding("EditValue", objEquipamento, "MensagemPadrao", true, DataSourceUpdateMode.OnPropertyChanged));
            txtEntrada.DataBindings.Add(new Binding("EditValue", objEquipamento, "Entrada", true, DataSourceUpdateMode.OnPropertyChanged));
            txtSaida.DataBindings.Add(new Binding("EditValue", objEquipamento, "Saida", true, DataSourceUpdateMode.OnPropertyChanged));
            rgTipoCartao.DataBindings.Add(new Binding("EditValue", objEquipamento, "TipoCartao", true, DataSourceUpdateMode.OnPropertyChanged));

            chbListaAcesso.DataBindings.Add(new Binding("Checked", objEquipamento, "ListaAcesso", true, DataSourceUpdateMode.OnPropertyChanged));
            chbAtivaOnline.DataBindings.Add(new Binding("Checked", objEquipamento, "AtivaOnLine", true, DataSourceUpdateMode.OnPropertyChanged));
            chbMostrarDataHora.DataBindings.Add(new Binding("Checked", objEquipamento, "MostrarDataHora", true, DataSourceUpdateMode.OnPropertyChanged));
            chbUtilizaCatraca.DataBindings.Add(new Binding("Checked", objEquipamento, "UtilizaCatraca", true, DataSourceUpdateMode.OnPropertyChanged));

            #endregion

            #region TabOnLine

            rgTipoLeitorOn.DataBindings.Add(new Binding("EditValue", objEquipamento, "TipoLeitorOn", true, DataSourceUpdateMode.OnPropertyChanged));
            rgOperacaoLeitor1On.DataBindings.Add(new Binding("EditValue", objEquipamento, "OperaLeitor1On", true, DataSourceUpdateMode.OnPropertyChanged));
            rgAcionamento1On.DataBindings.Add(new Binding("EditValue", objEquipamento, "Acionamento1On", true, DataSourceUpdateMode.OnPropertyChanged));
            rgAcionamento2On.DataBindings.Add(new Binding("EditValue", objEquipamento, "Acionamento2On", true, DataSourceUpdateMode.OnPropertyChanged));
            txtTempoAciona1On.DataBindings.Add(new Binding("EditValue", objEquipamento, "TempoAciona1On", true, DataSourceUpdateMode.OnPropertyChanged));
            txtTempoAciona2On.DataBindings.Add(new Binding("EditValue", objEquipamento, "TempoAciona2On", true, DataSourceUpdateMode.OnPropertyChanged));
            txtNumeroDigitosOn.DataBindings.Add(new Binding("EditValue", objEquipamento, "NumeroDigitosOn", true, DataSourceUpdateMode.OnPropertyChanged));
            txtCodEmpMenosOn.DataBindings.Add(new Binding("EditValue", objEquipamento, "CodEmpMenosOn", true, DataSourceUpdateMode.OnPropertyChanged));
            txtNivelControleOn.DataBindings.Add(new Binding("EditValue", objEquipamento, "NivelControleOn", true, DataSourceUpdateMode.OnPropertyChanged));

            chbAceitaTecladoOn.DataBindings.Add(new Binding("Checked", objEquipamento, "AceitaTecladoOn", true, DataSourceUpdateMode.OnPropertyChanged));
            chbEcoTeclado.DataBindings.Add(new Binding("Checked", objEquipamento, "EcoTeclado", true, DataSourceUpdateMode.OnPropertyChanged));

            #endregion

            #region TabFormasEntradas

            txtTempoMaximo.DataBindings.Add(new Binding("EditValue", objEquipamento, "TempoMaximo", true, DataSourceUpdateMode.OnPropertyChanged));
            txtPosicaoCursor.DataBindings.Add(new Binding("EditValue", objEquipamento, "PosicaoCursor", true, DataSourceUpdateMode.OnPropertyChanged));
            txtTotalDigitos.DataBindings.Add(new Binding("EditValue", objEquipamento, "TotalDigitos", true, DataSourceUpdateMode.OnPropertyChanged));
            rgFormasEntradas.DataBindings.Add(new Binding("EditValue", objEquipamento, "FormasEntradas", true, DataSourceUpdateMode.OnPropertyChanged));

            #endregion

            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();

            return bllEquipamento.Salvar(cwAcao, objEquipamento);
        }
    }
}
