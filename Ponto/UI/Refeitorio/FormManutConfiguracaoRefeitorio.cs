using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutConfiguracaoRefeitorio : UI.Base.ManutBase
    {
        private BLL.ConfiguracaoRefeitorio bllConfiguracaoRefeitorio;
        private Modelo.ConfiguracaoRefeitorio objConfiguracaoRefeitorio;

        public FormManutConfiguracaoRefeitorio()
        {
            InitializeComponent();
            bllConfiguracaoRefeitorio = new BLL.ConfiguracaoRefeitorio();
        }

        public override void CarregaObjeto()
        {
            cwID = 1;

            objConfiguracaoRefeitorio = bllConfiguracaoRefeitorio.LoadObject(cwID);

            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objConfiguracaoRefeitorio.TipoConexao = -1;
                    objConfiguracaoRefeitorio.Porta = -1;
                    break;
            }

            rgTipoConexao.DataBindings.Add("EditValue", objConfiguracaoRefeitorio, "TipoConexao", true, DataSourceUpdateMode.OnPropertyChanged);
            rgPorta.DataBindings.Add("EditValue", objConfiguracaoRefeitorio, "Porta", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPortaTCP.DataBindings.Add("EditValue", objConfiguracaoRefeitorio, "PortaTCP", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbCarregaBiometria.DataBindings.Add("Checked", objConfiguracaoRefeitorio, "CarregaBiometria", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbEntrarDiretoOnline.DataBindings.Add("Checked", objConfiguracaoRefeitorio, "EntrarDiretoOnline", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbNaoPassarDuasVezesEntrada.DataBindings.Add("Checked", objConfiguracaoRefeitorio, "NaoPassarDuasVezesEntrada", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbNaoPassarDuasVezesSaida.DataBindings.Add("Checked", objConfiguracaoRefeitorio, "NaoPassarDuasVezesSaida", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbSomenteUmaVezEntradaSaida.DataBindings.Add("Checked", objConfiguracaoRefeitorio, "SomenteUmaVezEntradaSaida", true, DataSourceUpdateMode.OnPropertyChanged);
            //txtIntervaloPassadasEntrada.DataBindings.Add("DateTime", objConfiguracaoRefeitorio, "IntervaloPassadasEntrada", true, DataSourceUpdateMode.OnPropertyChanged);
            //txtIntervaloPassadasSaida.DataBindings.Add("DateTime", objConfiguracaoRefeitorio, "IntervaloPassadasSaida", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCartaoMestre.DataBindings.Add("EditValue", objConfiguracaoRefeitorio, "CartaoMestre", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtDias.DataBindings.Add("EditValue", objConfiguracaoRefeitorio, "QtDias", true, DataSourceUpdateMode.OnPropertyChanged);

            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            return bllConfiguracaoRefeitorio.Salvar(Modelo.Acao.Alterar, objConfiguracaoRefeitorio);
        }

        private void rgTipoConexao_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((int)rgTipoConexao.EditValue)
            {
                case 0:
                    rgPorta.Enabled = true;
                    rgPorta.SelectedIndex = 0;
                    txtPortaTCP.Enabled = false;
                    txtPortaTCP.EditValue = 0;
                    break;
                case 1:
                    rgPorta.EditValue = -1;
                    rgPorta.Enabled = false;
                    txtPortaTCP.Enabled = false;
                    break;
                case 2:
                    rgPorta.EditValue = -1;
                    rgPorta.Enabled = false;
                    txtPortaTCP.Enabled = true;
                    break;
            }
        }
    }
}
