using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutJornadaAlternativa : UI.Base.ManutBase
    {
        #region Atributos
        
        private Modelo.JornadaAlternativa objJornadaAlternativa;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcao bllFuncao;
        private BLL.Funcionario bllFuncionario;
        private BLL.Jornada bllJornada;
        private BLL.JornadaAlternativa bllJornadaAlternativa;
        private BLL.Parametros bllParametros;
        private Modelo.Jornada objJornada;
        private string InicioHNoturna { get; set; }
        private string FimHNotura { get; set; }
        private bool bParametroConfigurado { get; set; }
        private bool bCarregado { get; set; }
        #endregion

        public FormManutJornadaAlternativa()
        {
            InitializeComponent();
            bllDepartamento = new BLL.Departamento();
            bllEmpresa = new BLL.Empresa();
            bllFuncao = new BLL.Funcao();
            bllFuncionario = new BLL.Funcionario();
            bllJornada = new BLL.Jornada();
            bllJornadaAlternativa = new BLL.JornadaAlternativa();
            bllParametros = new BLL.Parametros();
            this.Name = "FormManutJornadaAlternativa";
        }

        public override void CarregaObjeto()
        {
            bCarregado = false;
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:

                    objJornadaAlternativa = new Modelo.JornadaAlternativa();
                    objJornadaAlternativa.DiasJA = new List<Modelo.DiasJornadaAlternativa>();
                    objJornadaAlternativa.Codigo = bllJornadaAlternativa.MaxCodigo();
                    objJornadaAlternativa.LimiteMin = "03:00";
                    objJornadaAlternativa.LimiteMax = "03:00";
                    chbHorasNormais.Checked = true;
                    chbAdNoturno.Enabled = false;

                    break;
                default:
                    objJornadaAlternativa = bllJornadaAlternativa.LoadObject(cwID);
                    objJornadaAlternativa.Tipo_Ant = objJornadaAlternativa.Tipo;
                    objJornadaAlternativa.DataInicial_Ant = objJornadaAlternativa.DataInicial;
                    objJornadaAlternativa.DataFinal_Ant = objJornadaAlternativa.DataFinal;
                    objJornadaAlternativa.Identificacao_Ant = objJornadaAlternativa.Identificacao;

                    chbMarcacargahorariamista.Checked = Convert.ToBoolean(objJornadaAlternativa.CargaMista);
                    chbHorasNormais.Checked = Convert.ToBoolean(objJornadaAlternativa.HorasNormais);
                    break;
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

            LoadDiasJA();

            txtCodigo.DataBindings.Add("EditValue", objJornadaAlternativa, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipo.DataBindings.Add("EditValue", objJornadaAlternativa, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdIdentificacao.DataBindings.Add("EditValue", objJornadaAlternativa, "Identificacao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataInicial.DataBindings.Add("DateTime", objJornadaAlternativa, "DataInicial", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataFinal.DataBindings.Add("DateTime", objJornadaAlternativa, "DataFinal", true, DataSourceUpdateMode.OnPropertyChanged);
            chbSomenteCargaHoraria.DataBindings.Add("Checked", objJornadaAlternativa, "SomenteCargaHoraria", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteMin.DataBindings.Add("EditValue", objJornadaAlternativa, "LimiteMin", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteMax.DataBindings.Add("EditValue", objJornadaAlternativa, "LimiteMax", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_1.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_2.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_3.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_4.DataBindings.Add("EditValue", objJornadaAlternativa, "Entrada_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_1.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_2.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_3.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_4.DataBindings.Add("EditValue", objJornadaAlternativa, "Saida_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalTrabalhadaDiurna.DataBindings.Add("EditValue", objJornadaAlternativa, "TotalTrabalhadaDiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalTrabalhadaNoturna.DataBindings.Add("EditValue", objJornadaAlternativa, "TotalTrabalhadaNoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaHorariaMista.DataBindings.Add("EditValue", objJornadaAlternativa, "TotalMista", true, DataSourceUpdateMode.OnPropertyChanged);

            cbIdjornada.EditValue = objJornadaAlternativa.Idjornada;
            chbIntervaloAutomatico.Checked = Convert.ToBoolean(objJornadaAlternativa.Intervaloautomatico);
            chbPreassinaladas1.Checked = Convert.ToBoolean(objJornadaAlternativa.Preassinaladas1);
            chbPreassinaladas2.Checked = Convert.ToBoolean(objJornadaAlternativa.Preassinaladas2);
            chbPreassinaladas3.Checked = Convert.ToBoolean(objJornadaAlternativa.Preassinaladas3);

            chbAdNoturno.Enabled = false;

            chbConversaoHoraNoturna.Checked = Convert.ToBoolean(objJornadaAlternativa.ConversaoHoraNoturna);
            chbAdNoturno.Checked = Convert.ToBoolean(objJornadaAlternativa.CalculoAdicionalNoturno);

            base.CarregaObjeto();
            bCarregado = true;
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            objJornadaAlternativa.Idjornada = Convert.ToInt32(cbIdjornada.EditValue);
            objJornadaAlternativa.CargaMista = Convert.ToInt16(chbMarcacargahorariamista.Checked);
            objJornadaAlternativa.HorasNormais = Convert.ToInt16(chbHorasNormais.Checked);
            objJornadaAlternativa.Intervaloautomatico = Convert.ToInt16(chbIntervaloAutomatico.Checked);
            objJornadaAlternativa.Preassinaladas1 = Convert.ToInt16(chbPreassinaladas1.Checked);
            objJornadaAlternativa.Preassinaladas2 = Convert.ToInt16(chbPreassinaladas2.Checked);
            objJornadaAlternativa.Preassinaladas3 = Convert.ToInt16(chbPreassinaladas3.Checked);
            objJornadaAlternativa.ConversaoHoraNoturna = Convert.ToInt16(chbConversaoHoraNoturna.Checked);
            objJornadaAlternativa.CalculoAdicionalNoturno = Convert.ToInt16(chbAdNoturno.Checked);

            base.Salvar();
            FormProgressBar2 pb = new FormProgressBar2();
            pb.Show(this);
            bllJornadaAlternativa.ObjProgressBar = pb.ObjProgressBar;
            ret = bllJornadaAlternativa.Salvar(cwAcao, objJornadaAlternativa);
            pb.Close();

            return ret;
        }

        #region Dias Jornada Alternativa

        private void LoadDiasJA()
        {
            List<Modelo.DiasJornadaAlternativa> lista = new List<Modelo.DiasJornadaAlternativa>();
            foreach (Modelo.DiasJornadaAlternativa dja in objJornadaAlternativa.DiasJA)
            {
                if (dja.Acao != Modelo.Acao.Excluir)
                {
                    lista.Add(dja);
                }
            }
            gcDiasJornadaAlternativa.DataSource = lista;
        }

        protected override void ChamaHelp()
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, "formmanutdiasjornadaalternativa.htm");
        }

        private void CarregaFormDiasJornadaAlternativa(Modelo.Acao pAcao, int pCodigo)
        {
            if (pAcao != Modelo.Acao.Incluir && pCodigo == 0)
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
            else
            {
                UI.FormManutDiasJornadaAlternativa form = new UI.FormManutDiasJornadaAlternativa(objJornadaAlternativa);
                form.cwAcao = pAcao;
                form.cwID = pCodigo;
                form.cwTabela = "Dias Jornada Alternativa";
                form.ShowDialog();

                if (pAcao != Modelo.Acao.Consultar)
                {
                    LoadDiasJA();
                }
            }
        }

        private Int32 DiasJASelecionado()
        {
            Int32 seq;
            try
            {
                seq = (int)gvDiasJornadaAlternativa.GetFocusedRowCellValue("Codigo");
            }
            catch (Exception)
            {
                seq = 0;
            }
            return seq;
        }

        private void sbIncluirDiasJornadaAlternativa_Click(object sender, EventArgs e)
        {
            CarregaFormDiasJornadaAlternativa(Modelo.Acao.Incluir, 0);
        }

        private void sbAlterarDiasJornadaAlternativa_Click(object sender, EventArgs e)
        {
            CarregaFormDiasJornadaAlternativa(Modelo.Acao.Alterar, DiasJASelecionado());
        }

        private void sbExcluirDiasJornadaAlternativa_Click(object sender, EventArgs e)
        {
            CarregaFormDiasJornadaAlternativa(Modelo.Acao.Excluir, DiasJASelecionado());
        }

        private void gcDiasJornadaAlternativa_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CarregaFormDiasJornadaAlternativa(Modelo.Acao.Alterar, DiasJASelecionado());
                    break;
            }
        }

        private void gcDiasJornadaAlternativa_DoubleClick(object sender, EventArgs e)
        {
            CarregaFormDiasJornadaAlternativa(Modelo.Acao.Alterar, DiasJASelecionado());
        }

        #endregion

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

        #endregion

        #region Intervalo Automático

        private void chbIntervaloAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }

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

        #endregion

        #region Outros Eventos
        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIdIdentificacao.Properties.DataSource = null;
            switch ((int)rgTipo.EditValue)
            {
                case -1:
                    cbIdIdentificacao.Properties.DataSource = null;
                    break;
                case 0: // Empresa
                    cbIdIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdIdentificacao.Properties.DataSource = bllEmpresa.GetAll();
                    cbIdIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdIdentificacao.Properties.DisplayMember = "nome";
                    break;
                case 1: // Departamento
                    cbIdIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdIdentificacao.Properties.DataSource = bllDepartamento.GetAll();
                    cbIdIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdIdentificacao.Properties.Columns["empresa"].Visible = true;
                    break;
                case 2:
                    // Funcionário
                    cbIdIdentificacao.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    cbIdIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdIdentificacao.Properties.DisplayMember = "nome";
                    break;
                case 3:  // Função
                    cbIdIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdIdentificacao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdIdentificacao.Properties.DisplayMember = "descricao";
                    break;
            }
            cbIdIdentificacao.EditValue = 0;
        }

        private void sbIdIdentificacao_Click(object sender, EventArgs e)
        {
            switch ((int)rgTipo.EditValue)
            {
                case 0:
                    FormGridEmpresa formEmp = new FormGridEmpresa();
                    formEmp.cwTabela = "Empresa";
                    formEmp.cwId = (int)cbIdIdentificacao.EditValue;
                    GridSelecao(formEmp, cbIdIdentificacao, bllEmpresa);
                    break;
                case 1:
                    FormGridDepartamento formDep = new FormGridDepartamento();
                    formDep.cwTabela = "Departamento";
                    formDep.cwId = (int)cbIdIdentificacao.EditValue;
                    GridSelecao(formDep, cbIdIdentificacao, bllDepartamento);
                    break;
                case 2:
                    FormGridFuncionario formFun = new FormGridFuncionario();
                    formFun.cwTabela = "Funcionário";
                    formFun.cwId = (int)cbIdIdentificacao.EditValue;
                    GridSelecao(formFun, cbIdIdentificacao, bllFuncionario);
                    break;
                case 3:
                    FormGridFuncao formFuncao = new FormGridFuncao();
                    formFuncao.cwTabela = "Função";
                    formFuncao.cwId = (int)cbIdIdentificacao.EditValue;
                    GridSelecao(formFuncao, cbIdIdentificacao, bllFuncao);
                    break;
                default:
                    break;
            }
        }

        private void chbMarcacargahorariamista_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMarcacargahorariamista.Checked)
            {
                chbHorasNormais.Checked = false;
                CalculaHoras();
                txtTotalTrabalhadaDiurna.Text = "--:--";
                txtTotalTrabalhadaNoturna.Text = "--:--";


            }
            else
            {
                chbHorasNormais.Checked = true;
                txtCargaHorariaMista.Text = "--:--";
                CalculaHoras();

            }
        }

        private void chbHorasNormais_CheckedChanged(object sender, EventArgs e)
        {
            if (chbHorasNormais.Checked)
            {
                chbMarcacargahorariamista.Checked = false;
                txtCargaHorariaMista.Text = "--:--";
                CalculaHoras();
            }
            else
            {
                chbMarcacargahorariamista.Checked = true;
                txtTotalTrabalhadaDiurna.Text = "--:--";
                txtTotalTrabalhadaNoturna.Text = "--:--";
                CalculaHoras();
            }

        }

        private void chbConversaoHoraNoturna_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConversaoHoraNoturna.Checked)
            {
                chbAdNoturno.Enabled = true;
            }
            else
            {
                chbAdNoturno.Checked = false;
                chbAdNoturno.Enabled = false;
                chbConversaoHoraNoturna.Checked = false;
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
        #endregion
    }
}
