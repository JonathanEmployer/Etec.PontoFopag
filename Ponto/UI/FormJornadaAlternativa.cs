using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormJornadaAlternativa : UI.Base.ManutBase
    {
        #region Atributos
        private Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();
        private BLL.Jornada bllJornada;
        private BLL.JornadaAlternativa bllJornadaAlternativa;
        private BLL.Parametros bllParametros;
        private Modelo.Jornada objJornada;
        private string InicioHNoturna { get; set; }
        private string FimHNotura { get; set; }
        private bool bParametroConfigurado { get; set; }
        public Modelo.Marcacao cwMarcacao { get; set; }
        #endregion

        public FormJornadaAlternativa()
        {
            InitializeComponent();
            bllJornada = new BLL.Jornada();
            bllJornadaAlternativa = new BLL.JornadaAlternativa();
            bllParametros = new BLL.Parametros();
            this.Name = "FormJornadaAlternativa";
        }

        public override void CarregaObjeto()
        {
            objJornadaAlternativa = bllJornadaAlternativa.LoadParaUmaMarcacao(cwMarcacao.Data, 2, cwMarcacao.Idfuncionario);
            if (objJornadaAlternativa.Id == 0)
            {
                cwAcao = Modelo.Acao.Incluir;
                objJornadaAlternativa.DataInicial = cwMarcacao.Data;
                objJornadaAlternativa.DataFinal = cwMarcacao.Data;
                objJornadaAlternativa.Identificacao = cwMarcacao.Idfuncionario;
                objJornadaAlternativa.Tipo = 2;
                objJornadaAlternativa.LimiteMin = "03:00";
                objJornadaAlternativa.LimiteMax = "03:00";
                objJornadaAlternativa.HorasNormais = 1;
                objJornadaAlternativa.HabilitaTolerancia = 0;
                objJornadaAlternativa.SomenteCargaHoraria = 0;
                objJornadaAlternativa.OrdenaBilheteSaida = 0;
                objJornadaAlternativa.Codigo = bllJornadaAlternativa.MaxCodigo();
                objJornadaAlternativa.Entrada_1 = "--:--";
                objJornadaAlternativa.Entrada_2 = "--:--";
                objJornadaAlternativa.Entrada_3 = "--:--";
                objJornadaAlternativa.Entrada_4 = "--:--";
                objJornadaAlternativa.Saida_1 = "--:--";
                objJornadaAlternativa.Saida_2 = "--:--";
                objJornadaAlternativa.Saida_3 = "--:--";
                objJornadaAlternativa.Saida_4 = "--:--";
                objJornadaAlternativa.TotalTrabalhadaDiurna = "--:--";
                objJornadaAlternativa.TotalTrabalhadaNoturna = "--:--";
                chbHorasnormais.Checked = true;
                chbCalcAdNoturno.Enabled = false;
            }
            else
            {
                cwAcao = Modelo.Acao.Alterar;

                objJornadaAlternativa.Tipo_Ant = objJornadaAlternativa.Tipo;
                objJornadaAlternativa.DataInicial_Ant = objJornadaAlternativa.DataInicial;
                objJornadaAlternativa.DataFinal_Ant = objJornadaAlternativa.DataFinal;
                objJornadaAlternativa.Identificacao_Ant = objJornadaAlternativa.Identificacao;
                chbMarcacargahorariamista.Checked = Convert.ToBoolean(objJornadaAlternativa.CargaMista);
                chbHorasnormais.Checked = Convert.ToBoolean(objJornadaAlternativa.HorasNormais);
            }

            Modelo.Parametros objParametros = bllParametros.LoadPrimeiro();
            if (objParametros.Id == 0 || (String.IsNullOrEmpty(objParametros.InicioAdNoturno) || String.IsNullOrEmpty(objParametros.FimAdNoturno)))
            {
                InicioHNoturna = "--:--";
                FimHNotura = "--:--";
                bParametroConfigurado = false;
            }
            else
            {
                InicioHNoturna = objParametros.InicioAdNoturno;
                FimHNotura = objParametros.FimAdNoturno;
                bParametroConfigurado = true;
            }
            cbIdjornada.Properties.DataSource = bllJornada.GetAll();

            txtEntrada_1.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_2.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_3.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_4.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_1.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_2.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_3.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_4.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteMin.DataBindings.Add("EditValue", objJornadaAlternativa, "LimiteMin", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteMax.DataBindings.Add("EditValue", objJornadaAlternativa, "LimiteMax", true, DataSourceUpdateMode.OnPropertyChanged);

            txtTotalTrabalhadaDiurna.EditValue = objJornadaAlternativa.TotalTrabalhadaDiurna;
            txtTotalTrabalhadaNoturna.EditValue = objJornadaAlternativa.TotalTrabalhadaNoturna;
            txtCargaHorariaMista.EditValue = objJornadaAlternativa.TotalMista;
            cbIdjornada.EditValue = objJornadaAlternativa.Idjornada;
            chbIntervaloAutomatico.Checked = Convert.ToBoolean(objJornadaAlternativa.Intervaloautomatico);
            chbPreassinaladas1.Checked = Convert.ToBoolean(objJornadaAlternativa.Preassinaladas1);
            chbPreassinaladas2.Checked = Convert.ToBoolean(objJornadaAlternativa.Preassinaladas2);
            chbPreassinaladas3.Checked = Convert.ToBoolean(objJornadaAlternativa.Preassinaladas3);
            chbCalcAdNoturno.Enabled = false;
            chbConversaohoranoturna.Checked = Convert.ToBoolean(objJornadaAlternativa.ConversaoHoraNoturna);
            chbCalcAdNoturno.Checked = Convert.ToBoolean(objJornadaAlternativa.CalculoAdicionalNoturno);

            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            objJornadaAlternativa.TotalMista = Convert.ToString(txtCargaHorariaMista.EditValue);
            objJornadaAlternativa.TotalTrabalhadaDiurna = Convert.ToString(txtTotalTrabalhadaDiurna.EditValue);
            objJornadaAlternativa.TotalTrabalhadaNoturna = Convert.ToString(txtTotalTrabalhadaNoturna.EditValue);
            objJornadaAlternativa.Idjornada = Convert.ToInt32(cbIdjornada.EditValue);
            objJornadaAlternativa.CargaMista = Convert.ToInt16(chbMarcacargahorariamista.Checked);
            objJornadaAlternativa.HorasNormais = Convert.ToInt16(chbHorasnormais.Checked);
            objJornadaAlternativa.Intervaloautomatico = Convert.ToInt16(chbIntervaloAutomatico.Checked);
            objJornadaAlternativa.Preassinaladas1 = Convert.ToInt16(chbPreassinaladas1.Checked);
            objJornadaAlternativa.Preassinaladas2 = Convert.ToInt16(chbPreassinaladas2.Checked);
            objJornadaAlternativa.Preassinaladas3 = Convert.ToInt16(chbPreassinaladas3.Checked);
            objJornadaAlternativa.ConversaoHoraNoturna = Convert.ToInt16(chbConversaohoranoturna.Checked);
            objJornadaAlternativa.CalculoAdicionalNoturno = Convert.ToInt16(chbCalcAdNoturno.Checked);

            FormProgressBar2 progressbar = new FormProgressBar2();
            base.Salvar();
            bllJornadaAlternativa.ObjProgressBar = progressbar.ObjProgressBar;

            ret = bllJornadaAlternativa.Salvar(cwAcao, objJornadaAlternativa);

            progressbar.Dispose();

            return ret;


        }

        #region Carga Horária

        private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN)
        {
            Modelo.Parametros objParametros = new Modelo.Parametros();

            objParametros = bllParametros.LoadPrimeiro();

            InicioHNoturna = objParametros.InicioAdNoturno;
            FimHNotura = objParametros.FimAdNoturno;

            totalD = "";
            totalN = "";

            BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        }

        private void CalculoCargaHoraria(string[] pEntrada, string[] pSaida, Componentes.devexpress.cwkEditHora pDiurna, Componentes.devexpress.cwkEditHora pNoturna, Componentes.devexpress.cwkEditHora pMista, bool pCalculaMista)
        {

            string totalD;
            string totalN;
            AuxCalculaHoras(pEntrada, pSaida, out totalD, out totalN);

            pDiurna.EditValue = (totalD != "00:00" ? totalD : "--:--");
            pNoturna.EditValue = (totalN != "00:00" ? totalN : "--:--");

            if (pCalculaMista)
            {
                pDiurna.EditValue = "--:--";
                pNoturna.EditValue = "--:--";
                pMista.EditValue = BLL.CalculoHoras.OperacaoHoras('+', totalD, totalN);
            }
            else
            {
                pMista.EditValue = "--:--";
            }

        }

        private void CalculaHoras()
        {
            string[] entradas = new string[] { (string)txtEntrada_1.EditValue, (string)txtEntrada_2.EditValue, (string)txtEntrada_3.EditValue, (string)txtEntrada_4.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1.EditValue, (string)txtSaida_2.EditValue, (string)txtSaida_3.EditValue, (string)txtSaida_4.EditValue };

            CalculoCargaHoraria(entradas, saidas, txtTotalTrabalhadaDiurna, txtTotalTrabalhadaNoturna, txtCargaHorariaMista, chbMarcacargahorariamista.Checked);
        }

        //private void txtEntrada_1_EditValueChanged(object sender, EventArgs e)
        //{
        //    if ((string)txtEntrada_1.EditValue != "--:--" && (string)txtSaida_1.EditValue != "--:--")
        //    {
        //        CalculaHoras();
        //    }
        //}

        #endregion

        #region Intervalo Automático
        private void HabilitaIntervaloAutomatico()
        {
            bool hab = chbIntervaloAutomatico.Checked;
            chbPreassinaladas1.Enabled = hab && (string)txtSaida_1.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtSaida_1.EditValue) && (string)txtEntrada_2.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtEntrada_2.EditValue);
            chbPreassinaladas2.Enabled = hab && (string)txtSaida_2.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtSaida_2.EditValue) && (string)txtEntrada_3.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtEntrada_3.EditValue);
            chbPreassinaladas3.Enabled = hab && (string)txtSaida_3.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtSaida_3.EditValue) && (string)txtEntrada_4.EditValue != "--:--" && !String.IsNullOrEmpty((string)txtEntrada_4.EditValue);

            chbPreassinaladas1.Checked = chbPreassinaladas1.Enabled ? chbPreassinaladas1.Checked : chbPreassinaladas1.Enabled;
            chbPreassinaladas2.Checked = chbPreassinaladas2.Enabled ? chbPreassinaladas2.Checked : chbPreassinaladas2.Enabled;
            chbPreassinaladas3.Checked = chbPreassinaladas3.Enabled ? chbPreassinaladas3.Checked : chbPreassinaladas3.Enabled;
        }

        private void chbIntervaloAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }
        #endregion

        private void chbMarcacargahorariamista_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMarcacargahorariamista.Checked)
            {
                chbHorasnormais.Checked = false;
                CalculaHoras();
                txtTotalTrabalhadaDiurna.Text = "--:--";
                txtTotalTrabalhadaNoturna.Text = "--:--";
            }
            else
            {
                txtCargaHorariaMista.Text = "--:--";
                chbHorasnormais.Checked = true;
                CalculaHoras();

            }

        }

        private void chbHorasnormais_CheckedChanged(object sender, EventArgs e)
        {
            if (chbHorasnormais.Checked)
            {
                chbMarcacargahorariamista.Checked = false;
                txtCargaHorariaMista.Text = "--:--";
                CalculaHoras();
            }
            else
            {
                txtTotalTrabalhadaDiurna.Text = "--:--";
                txtTotalTrabalhadaNoturna.Text = "--:--";
                chbMarcacargahorariamista.Checked = true;
                CalculaHoras();
            }

        }

        private void chbConversaohoranoturna_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConversaohoranoturna.Checked)
            {
                chbCalcAdNoturno.Enabled = true;
            }
            else
            {
                chbCalcAdNoturno.Checked = false;
                chbCalcAdNoturno.Enabled = false;
                chbConversaohoranoturna.Checked = false;
            }
        }

        private void sbIdjornada_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornada.EditValue;
            GridSelecao(formJornada, cbIdjornada, bllJornada);
        }

        private void cbIdjornada_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornada.EditValue;
            if (id > 0)
            {
                objJornada = bllJornada.LoadObject(id);
                txtEntrada_1.EditValue = objJornada.Entrada_1;
                txtEntrada_2.EditValue = objJornada.Entrada_2;
                txtEntrada_3.EditValue = objJornada.Entrada_3;
                txtEntrada_4.EditValue = objJornada.Entrada_4;
                txtSaida_1.EditValue = objJornada.Saida_1;
                txtSaida_2.EditValue = objJornada.Saida_2;
                txtSaida_3.EditValue = objJornada.Saida_3;
                txtSaida_4.EditValue = objJornada.Saida_4;

                chbIntervaloAutomatico.Enabled = true;

                CalculaHoras();
            }
        }

    }
}
