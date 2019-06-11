using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;

namespace UI
{
    public partial class FormManutHorarioMovel : UI.Base.ManutBase
    {
        #region Atributos
        private List<Modelo.LimiteDDsr> LimitesDDsrProporcionais;

        public bool[] checaLimite = new bool[6];
        public int _Limite = 0;
        private bool carregado;
        private BLL.Parametros bllParametro;
        private BLL.Horario bllHorario;
        private BLL.HorarioDetalhe bllHorarioDetalhe;
        private BLL.Jornada bllJornada;
        private BLL.Marcacao bllMarcacao;
        private Modelo.Jornada objJornada;
        private Modelo.Horario objHorario;
        private Modelo.Parametros objParametros;
        private string InicioHNoturna { get; set; }
        private string FimHNotura { get; set; }
        private string horaD { get; set; }
        private string horaN { get; set; }
        private string horaM { get; set; }
        #endregion

        public FormManutHorarioMovel()
        {
            InitializeComponent();
            bllParametro = new BLL.Parametros();
            bllHorario = new BLL.Horario();
            bllHorarioDetalhe = new BLL.HorarioDetalhe();
            bllJornada = new BLL.Jornada();
            bllMarcacao = new BLL.Marcacao();
            this.Name = "FormManutHorarioMovel";
        }

        private void InicializaHorariosPHExtra()
        {
            objHorario.HorariosPHExtra = new Modelo.HorarioPHExtra[10];

            for (int i = 0; i < objHorario.HorariosPHExtra.Length ; i++)
            {
                objHorario.HorariosPHExtra[i] = new Modelo.HorarioPHExtra();
                objHorario.HorariosPHExtra[i].Codigo = i;
                objHorario.HorariosPHExtra[i].Quantidadeextra = "---:--";
            }

        }

        private void AtribuiHorariosPHExtra()
        {
            chbMarcapercentualextra50.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[0].Marcapercentualextra);
            txtPercentualextra50.DataBindings.Add("Value", objHorario.HorariosPHExtra[0], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextra50.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[0], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);

            chbMarcapercentualextra60.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[1].Marcapercentualextra);
            txtPercentualextra60.DataBindings.Add("Value", objHorario.HorariosPHExtra[1], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextra60.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[1], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);

            chbMarcapercentualextra70.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[2].Marcapercentualextra);
            txtPercentualextra70.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[2], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextra70.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[2], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);

            chbMarcapercentualextra80.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[3].Marcapercentualextra);
            txtPercentualextra80.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[3], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextra80.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[3], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);

            chbMarcapercentualextra90.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[4].Marcapercentualextra);
            txtPercentualextra90.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[4], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextra90.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[4], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);

            chbMarcapercentualextra100.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[5].Marcapercentualextra);
            txtPercentualextra100.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[5], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextra100.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[5], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);

            chbMarcapercentualextraSabado.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[6].Marcapercentualextra);
            txtPercentualextraSabado.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[6], "PercentualExtraSegundo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextraSabado.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[6], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTipoAcumuloSab.DataBindings.Add("SelectedIndex", objHorario.HorariosPHExtra[6], "TipoAcumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbPercentualExtraSab.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[6], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);


            chbMarcapercentualextraDomingo.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[7].Marcapercentualextra);
            txtPercentualextraDomingo.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[7], "PercentualExtraSegundo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextraDomingo.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[7], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTipoAcumuloDom.DataBindings.Add("SelectedIndex", objHorario.HorariosPHExtra[7], "TipoAcumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbPercentualExtraDom.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[7], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);


            chbMarcapercentualextraFeriado.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[8].Marcapercentualextra);
            txtPercentualextraFeriado.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[8], "PercentualExtraSegundo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextraFeriado.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[8], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTipoAcumuloFer.DataBindings.Add("SelectedIndex", objHorario.HorariosPHExtra[8], "TipoAcumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbPercentualExtraFer.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[8], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);



            chbMarcapercentualextraFolga.Checked = Convert.ToBoolean(objHorario.HorariosPHExtra[9].Marcapercentualextra);
            txtPercentualextraFolga.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[9], "PercentualExtraSegundo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuantidadeextraFolga.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[9], "Quantidadeextra", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTipoAcumuloFol.DataBindings.Add("SelectedIndex", objHorario.HorariosPHExtra[9], "TipoAcumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbPercentualExtraFol.DataBindings.Add("EditValue", objHorario.HorariosPHExtra[9], "Percentualextra", true, DataSourceUpdateMode.OnPropertyChanged);


        }

        private void InicializaHorario()
        {
            objHorario = new Modelo.Horario();
            objHorario.Codigo = bllHorario.MaxCodigo();
            objHorario.Limitemin = "--:--";
            objHorario.Limitemax = "--:--";
            objHorario.Refeicao_01 = null;
            objHorario.Refeicao_02 = null;
            objHorario.Qtdhorasdsr = null;
            objHorario.Limiteperdadsr = null;
            objHorario.Tipoacumulo = -1;
            objHorario.Diasemanadsr = -1;
            objHorario.Horasnormais = 1;
            objHorario.TipoHorario = 2;
            objHorario.Descricao = "";

            objHorario.Horariodescricao_1 = "--:--";
            objHorario.Horariodescricao_2 = "--:--";
            objHorario.Horariodescricao_3 = "--:--";
            objHorario.Horariodescricao_4 = "--:--";
            objHorario.Horariodescricaosai_1 = "--:--";
            objHorario.Horariodescricaosai_2 = "--:--";
            objHorario.Horariodescricaosai_3 = "--:--";
            objHorario.Horariodescricaosai_4 = "--:--";

            InicializaHorariosPHExtra();
        }

        public override void CarregaObjeto()
        {
            carregado = false;
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    InicializaHorario();
                    objHorario.HorariosFlexiveis = new List<Modelo.HorarioDetalhe>();
                    objParametros = bllParametro.LoadPrimeiro();
                    objHorario.Idparametro = objParametros.Id;
                    objHorario.Limitemin = "03:00";
                    objHorario.Limitemax = "03:00";
                    objHorario.HorariosPHExtra[6].TipoAcumulo = -1;
                    objHorario.HorariosPHExtra[7].TipoAcumulo = -1;
                    objHorario.HorariosPHExtra[8].TipoAcumulo = -1;
                    objHorario.HorariosPHExtra[9].TipoAcumulo = -1;

                    VerificaPreenchimentobancoHorasPercentual();
                    break;
                default:
                    objHorario = bllHorario.LoadObject(cwID);
                    objParametros = bllParametro.LoadObject(objHorario.Idparametro);
                    if (cwAcao == Modelo.Acao.Consultar)
                    {
                        sbAlterar.Enabled = false;
                        sbExcluiHorarios.Enabled = false;
                        sb5_1.Enabled = false;
                        sb12_36.Enabled = false;
                        sb24_48.Enabled = false;
                        sb6_2.Enabled = false;
                        sbTurnoCompleto.Enabled = false;
                    }

                    VerificaPreenchimentobancoHorasPercentual();
                    objHorario.LimitesDDsrProporcionais.ForEach(i => i.Acao = Modelo.Acao.Alterar);
                    break;
            }

            cbIdparametro.Properties.DataSource = bllParametro.GetAll();
            cbIdjornada.Properties.DataSource = bllJornada.GetAll();

            CarregaGrid();

            txtCodigo.DataBindings.Add("EditValue", objHorario, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objHorario, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdparametro.DataBindings.Add("EditValue", objHorario, "Idparametro", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_1.DataBindings.Add("EditValue", objHorario, "Horariodescricao_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_2.DataBindings.Add("EditValue", objHorario, "Horariodescricao_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_3.DataBindings.Add("EditValue", objHorario, "Horariodescricao_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEntrada_4.DataBindings.Add("EditValue", objHorario, "Horariodescricao_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_1.DataBindings.Add("EditValue", objHorario, "Horariodescricaosai_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_2.DataBindings.Add("EditValue", objHorario, "Horariodescricaosai_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_3.DataBindings.Add("EditValue", objHorario, "Horariodescricaosai_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSaida_4.DataBindings.Add("EditValue", objHorario, "Horariodescricaosai_4", true, DataSourceUpdateMode.OnPropertyChanged);

            txtDataInicial.DataBindings.Add("EditValue", objHorario, "DataInicial", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataFinal.DataBindings.Add("EditValue", objHorario, "DataFinal", true, DataSourceUpdateMode.OnPropertyChanged);

            chbHabilitaperiodo01.Checked = Convert.ToBoolean(objHorario.Habilitaperiodo01);
            chbHabilitaperiodo02.Checked = Convert.ToBoolean(objHorario.Habilitaperiodo02);
            chbDescontacafemanha.Checked = Convert.ToBoolean(objHorario.Descontacafemanha);
            chbDescontacafetarde.Checked = Convert.ToBoolean(objHorario.Descontacafetarde);
            chbOrdem_ent.Checked = Convert.ToBoolean(objHorario.Ordem_ent);

            chbIntervaloAutomatico.Checked = Convert.ToBoolean(objHorario.Intervaloautomatico);
            chbPreassinaladas1.Checked = Convert.ToBoolean(objHorario.Preassinaladas1);
            chbPreassinaladas2.Checked = Convert.ToBoolean(objHorario.Preassinaladas2);
            chbPreassinaladas3.Checked = Convert.ToBoolean(objHorario.Preassinaladas3);

            chbDdrsProporcional.Checked = objHorario.bUtilizaDDSRProporcional;

            chbConversaohoranoturna.DataBindings.Add("Checked", objHorario, "Conversaohoranoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            chbConsideraadhtrabalhadas.DataBindings.Add("Checked", objHorario, "Consideraadhtrabalhadas", true, DataSourceUpdateMode.OnPropertyChanged);
            chbOrdenabilhetesaida.DataBindings.Add("Checked", objHorario, "Ordenabilhetesaida", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimitemax.DataBindings.Add("EditValue", objHorario, "Limitemax", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimitemin.DataBindings.Add("EditValue", objHorario, "Limitemin", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipoacumulo.DataBindings.Add("EditValue", objHorario, "Tipoacumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_1.DataBindings.Add("Checked", objHorario, "Dias_cafe_1", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_2.DataBindings.Add("Checked", objHorario, "Dias_cafe_2", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_3.DataBindings.Add("Checked", objHorario, "Dias_cafe_3", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_4.DataBindings.Add("Checked", objHorario, "Dias_cafe_4", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_5.DataBindings.Add("Checked", objHorario, "Dias_cafe_5", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_6.DataBindings.Add("Checked", objHorario, "Dias_cafe_6", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_7.DataBindings.Add("Checked", objHorario, "Dias_cafe_7", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDesconsiderarFeriado.DataBindings.Add("Checked", objHorario, "Desconsiderarferiado", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDescontafalta50.DataBindings.Add("Checked", objHorario, "Descontafalta50", true, DataSourceUpdateMode.OnPropertyChanged);
            chbConsiderasabadosemana.DataBindings.Add("Checked", objHorario, "Considerasabadosemana", true, DataSourceUpdateMode.OnPropertyChanged);
            chbConsideradomingosemana.DataBindings.Add("Checked", objHorario, "Consideradomingosemana", true, DataSourceUpdateMode.OnPropertyChanged);
            txtObs.DataBindings.Add("EditValue", objHorario, "Obs", true, DataSourceUpdateMode.OnPropertyChanged);

            chbHorasnormais.Checked = Convert.ToBoolean(objHorario.Horasnormais);
            chbMarcacargahorariamista.Checked = Convert.ToBoolean(objHorario.Marcacargahorariamista);
            chbDescontardsr.Checked = Convert.ToBoolean(objHorario.Descontardsr);

            txtQtdhorasdsr.DataBindings.Add("EditValue", objHorario, "Qtdhorasdsr", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteperdadsr.DataBindings.Add("EditValue", objHorario, "Limiteperdadsr", true, DataSourceUpdateMode.OnPropertyChanged);

            txtLimiteHorasTrabalhadasDia.EditValue = BuscaLimiteHorasTrabalhadas();
            txtLimiteMinimoHorasAlmoco.EditValue = BuscaLimiteMinimoHorasAlmoco();

            chbSegundaPercBanco.DataBindings.Add("Checked", objHorario, "MarcaSegundaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbTercaPercBanco.DataBindings.Add("Checked", objHorario, "MarcaTercaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbQuartaPercBanco.DataBindings.Add("Checked", objHorario, "MarcaQuartaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbQuintaPercBanco.DataBindings.Add("Checked", objHorario, "MarcaQuintaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbSextaPercBanco.DataBindings.Add("Checked", objHorario, "MarcaSextaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbSabadoPercBanco.DataBindings.Add("Checked", objHorario, "MarcaSabadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDomingoPercBanco.DataBindings.Add("Checked", objHorario, "MarcaDomingoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFeriadoPercBanco.DataBindings.Add("Checked", objHorario, "MarcaFeriadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFolgaPercBanco.DataBindings.Add("Checked", objHorario, "MarcaFolgaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSegundaPercBanco.DataBindings.Add("EditValue", objHorario, "SegundaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTercaPercBanco.DataBindings.Add("EditValue", objHorario, "TercaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuartaPercBanco.DataBindings.Add("EditValue", objHorario, "QuartaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuintaPercBanco.DataBindings.Add("EditValue", objHorario, "QuintaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSextaPercBanco.DataBindings.Add("EditValue", objHorario, "SextaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSabadoPercBanco.DataBindings.Add("EditValue", objHorario, "SabadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDomingoPercBanco.DataBindings.Add("EditValue", objHorario, "DomingoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFeriadoPercBanco.DataBindings.Add("EditValue", objHorario, "FeriadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFolgaPercBanco.DataBindings.Add("EditValue", objHorario, "FolgaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);

            objHorario.HorariosPHExtra[6].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[6].TipoAcumulo - 1);
            objHorario.HorariosPHExtra[7].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[7].TipoAcumulo - 1);
            objHorario.HorariosPHExtra[8].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[8].TipoAcumulo - 1);
            objHorario.HorariosPHExtra[9].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[9].TipoAcumulo - 1);

            gcLimiteDDsr.DataSource = LimitesDDsrProporcionais = objHorario.LimitesDDsrProporcionais;
            HabilitaDesabilitaDDSRProporcional();

            AtribuiHorariosPHExtra();

            if (cwAcao != Modelo.Acao.Incluir)
            {
                CalculoCargaHoraria();
            }

            base.CarregaObjeto();
            carregado = true;
        }

        private string BuscaLimiteHorasTrabalhadas()
        {
            string retorno = String.Empty;
            if (String.IsNullOrEmpty(objHorario.LimiteHorasTrabalhadasDia))
                retorno = "10:00";
            else if (objHorario.LimiteHorasTrabalhadasDia == "--:--")
                retorno = "10:00";
            else
                retorno = objHorario.LimiteHorasTrabalhadasDia;

            return retorno;
        }

        private string BuscaLimiteMinimoHorasAlmoco()
        {
            string retorno = String.Empty;
            if (String.IsNullOrEmpty(objHorario.LimiteHorasTrabalhadasDia))
                retorno = "01:00";
            else if (objHorario.LimiteMinimoHorasAlmoco == "--:--")
                retorno = "01:00";
            else
                retorno = objHorario.LimiteMinimoHorasAlmoco;

            return retorno;
        }

        public override Dictionary<string, string> Salvar()
        {
            FormProgressBar2 formPBRecalcula = new FormProgressBar2();
            Dictionary<string, string> ret = new Dictionary<string, string>();

            objHorario.Descricao = objHorario.Descricao.TrimEnd();

            if ((!chbHorasnormais.Checked) && (!chbMarcacargahorariamista.Checked))
            {
                MessageBox.Show("Preencha o campo do tipo do horário corretamente!");
            }

            else
            {
                objHorario.DesconsiderarFeriado = Convert.ToInt16(chbDesconsiderarFeriado.Checked);
                objHorario.bUtilizaDDSRProporcional = chbDdrsProporcional.Checked;
                objHorario.Descontacafemanha = Convert.ToInt16(chbDescontacafemanha.Checked);
                objHorario.Descontacafetarde = Convert.ToInt16(chbDescontacafetarde.Checked);
                objHorario.Habilitaperiodo01 = Convert.ToInt16(chbHabilitaperiodo01.Checked);
                objHorario.Habilitaperiodo02 = Convert.ToInt16(chbHabilitaperiodo02.Checked);
                objHorario.Ordem_ent = Convert.ToInt16(chbOrdem_ent.Checked);

                objHorario.Horasnormais = Convert.ToInt16(chbHorasnormais.Checked);
                objHorario.Marcacargahorariamista = Convert.ToInt16(chbMarcacargahorariamista.Checked);
                objHorario.Descontardsr = Convert.ToInt16(chbDescontardsr.Checked);

                objHorario.Intervaloautomatico = Convert.ToInt16(chbIntervaloAutomatico.Checked);
                objHorario.Preassinaladas1 = Convert.ToInt16(chbPreassinaladas1.Checked);
                objHorario.Preassinaladas2 = Convert.ToInt16(chbPreassinaladas2.Checked);
                objHorario.Preassinaladas3 = Convert.ToInt16(chbPreassinaladas3.Checked);

                objHorario.LimiteHorasTrabalhadasDia = Convert.ToString(txtLimiteHorasTrabalhadasDia.EditValue);
                objHorario.LimiteMinimoHorasAlmoco = Convert.ToString(txtLimiteMinimoHorasAlmoco.EditValue);

                objHorario.HorariosPHExtra[0].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextra50.Checked);
                objHorario.HorariosPHExtra[1].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextra60.Checked);
                objHorario.HorariosPHExtra[2].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextra70.Checked);
                objHorario.HorariosPHExtra[3].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextra80.Checked);
                objHorario.HorariosPHExtra[4].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextra90.Checked);
                objHorario.HorariosPHExtra[5].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextra100.Checked);
                objHorario.HorariosPHExtra[6].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextraSabado.Checked);
                objHorario.HorariosPHExtra[7].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextraDomingo.Checked);
                objHorario.HorariosPHExtra[8].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextraFeriado.Checked);
                objHorario.HorariosPHExtra[9].Marcapercentualextra = Convert.ToInt16(chbMarcapercentualextraFolga.Checked);

                objHorario.HorariosPHExtra[6].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[6].TipoAcumulo + 1);
                objHorario.HorariosPHExtra[7].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[7].TipoAcumulo + 1);
                objHorario.HorariosPHExtra[8].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[8].TipoAcumulo + 1);
                objHorario.HorariosPHExtra[9].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[9].TipoAcumulo + 1);

                if (((txtEntrada_1.Text != "--:--") && (txtSaida_1.Text == "--:--")) || ((txtEntrada_1.Text == "--:--") && (txtSaida_1.Text != "--:--")))
                {
                    MessageBox.Show("Você deve Preencher os campos entrada e saida corretamente!");
                    ret.Add("", "");
                }
                else if (((txtEntrada_2.Text != "--:--") && (txtSaida_2.Text == "--:--")) || ((txtEntrada_2.Text == "--:--") && (txtSaida_2.Text != "--:--")))
                {
                    MessageBox.Show("Você deve Preencher os campos entrada e saida corretamente!");
                    ret.Add("", "");
                }
                else if (((txtEntrada_3.Text != "--:--") && (txtSaida_3.Text == "--:--")) || ((txtEntrada_3.Text == "--:--") && (txtSaida_3.Text != "--:--")))
                {
                    MessageBox.Show("Você deve Preencher os campos entrada e saida corretamente!");
                    ret.Add("", "");
                }
                else if (((txtEntrada_4.Text != "--:--") && (txtSaida_4.Text == "--:--")) || ((txtEntrada_4.Text == "--:--") && (txtSaida_4.Text != "--:--")))
                {
                    MessageBox.Show("Você deve Preencher os campos entrada e saida corretamente!");
                    ret.Add("", "");
                }
                else if ((txtEntrada_1.Text == "--:--") && (txtSaida_1.Text == "--:--") && (txtEntrada_2.Text == "--:--") && (txtSaida_2.Text == "--:--") && (txtEntrada_3.Text == "--:--") && (txtSaida_3.Text == "--:--") && (txtEntrada_4.Text == "--:--") && (txtSaida_4.Text == "--:--"))
                {
                    MessageBox.Show("Você deve Preencher os campos entrada e saida corretamente!");
                    ret.Add("", "");
                }
                else
                {
                    if (objHorario.bUtilizaDDSRProporcional)
                    {
                        objHorario.LimitesDDsrProporcionais = LimitesDDsrProporcionais;
                        foreach (var LimiteDDsrProporcional in objHorario.LimitesDDsrProporcionais)
                        {
                            DateTime dtAtual = DateTime.Now.Date;
                            string[] dthora = new string[2] { "00", "00" };
                            DateTime dtVazia = new DateTime(dtAtual.Year, dtAtual.Month, dtAtual.Day, Convert.ToInt32(dthora[0]), Convert.ToInt32(dthora[1]), 0);

                            if ((LimiteDDsrProporcional.DTLimitePerdaDsr == dtVazia) ||
                                (LimiteDDsrProporcional.DTQtdHorasDsr == dtVazia))
                            {
                                string erro = "Quando a opção Desconto DSR Proporcional estiver marcada não podem existir limites de perda e quantidade de horas zeradas!";
                                MessageBox.Show(erro + Environment.NewLine + "Por favor verifique.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ret.Add("gcLimiteDDsr", erro);

                                return ret;
                            }
                        }
                    }
                    base.Salvar();
                    ret = bllHorario.Salvar(cwAcao, objHorario);
                    if (ret.Count == 0)
                    {
                        formPBRecalcula.Show();

                        this.Enabled = false;
                        bllMarcacao.RecalculaMarcacao(4, objHorario.Id, txtDataInicial.DateTime, txtDataFinal.DateTime, formPBRecalcula.ObjProgressBar);

                        formPBRecalcula.Close();
                    }
                }
            }
            return ret;
        }

        private void IncluiHorarioPHExtra(DevExpress.XtraEditors.CalcEdit pTxtPercentual)
        {
            if (pTxtPercentual.EditValue.ToString() != "0")
            {
                if (!cbPercentualExtraSab.Properties.Items.Contains(pTxtPercentual.EditValue))
                {
                    cbPercentualExtraSab.Properties.Items.Add(pTxtPercentual.EditValue);
                    cbPercentualExtraDom.Properties.Items.Add(pTxtPercentual.EditValue);
                    cbPercentualExtraFer.Properties.Items.Add(pTxtPercentual.EditValue);
                    cbPercentualExtraFol.Properties.Items.Add(pTxtPercentual.EditValue);
                }
            }
        }

        private void RemoveHorarioPHExtra(DevExpress.XtraEditors.CalcEdit pTxtPercentual)
        {
            if (pTxtPercentual.EditValue.ToString() != "0")
            {
                if (VerificaQuantosPercentuaisSaoIguais(pTxtPercentual) <= 1 || VerificaSeTemItemsRepetidosNoCombo() > 0)
                {
                    cbPercentualExtraSab.Properties.Items.Remove(pTxtPercentual.EditValue);
                    cbPercentualExtraDom.Properties.Items.Remove(pTxtPercentual.EditValue);
                    cbPercentualExtraFer.Properties.Items.Remove(pTxtPercentual.EditValue);
                    cbPercentualExtraFol.Properties.Items.Remove(pTxtPercentual.EditValue);
                }
            }
        }

        private int VerificaQuantosPercentuaisSaoIguais(DevExpress.XtraEditors.CalcEdit pTxtPercentual)
        {
            int cont = 0;
            if (Convert.ToInt16(pTxtPercentual.EditValue) == Convert.ToInt16(txtPercentualextra50.EditValue))
            {
                cont++;
            }
            if (Convert.ToInt16(pTxtPercentual.EditValue) == Convert.ToInt16(txtPercentualextra60.EditValue))
            {
                cont++;
            }
            if (Convert.ToInt16(pTxtPercentual.EditValue) == Convert.ToInt16(txtPercentualextra70.EditValue))
            {
                cont++;
            }
            if (Convert.ToInt16(pTxtPercentual.EditValue) == Convert.ToInt16(txtPercentualextra80.EditValue))
            {
                cont++;
            }
            if (Convert.ToInt16(pTxtPercentual.EditValue) == Convert.ToInt16(txtPercentualextra90.EditValue))
            {
                cont++;
            }
            if (Convert.ToInt16(pTxtPercentual.EditValue) == Convert.ToInt16(txtPercentualextra100.EditValue))
            {
                cont++;
            }
            return cont;
        }

        private int VerificaSeTemItemsRepetidosNoCombo()
        {
            int cont = 0;
            for (int i = 0; i < cbPercentualExtraDom.Properties.Items.Count; i++)
            {
                for (int k = (cbPercentualExtraDom.Properties.Items.Count - 1); k >= 0; k--)
                {
                    if (i != k)
                    {
                        if (Convert.ToInt16(cbPercentualExtraDom.Properties.Items[i]) == Convert.ToInt16(cbPercentualExtraDom.Properties.Items[k]))
                        {
                            cont++;
                        }
                    }
                }
            }
            return cont;
        }

        #region Eventos

        #region Horario Normal

        private void chbHorasnormais_CheckedChanged(object sender, EventArgs e)
        {
            if (chbHorasnormais.Checked)
            {
                chbMarcacargahorariamista.Checked = false;
                CalculoCargaHoraria();
                txtCargaMista.Text = "--:--";
            }
        }

        private void chbMarcacargahorariamista_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMarcacargahorariamista.Checked)
            {
                chbHorasnormais.Checked = false;
                txtCargaDiurna.Text = "--:--";
                txtCargaNoturna.Text = "--:--";
                CalculoCargaHoraria();
            }
        }

        #endregion

        #region Grid Horários

        private void sbAlterar_Click(object sender, EventArgs e)
        {
            if (cwAcao != Modelo.Acao.Consultar)
            {
                CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
            }
        }

        #endregion

        #region Gerar Horários

        private Modelo.HorarioDetalheMovel AuxGerar()
        {
            Modelo.HorarioDetalheMovel objHorarioDMovel = new Modelo.HorarioDetalheMovel();
            objHorarioDMovel.Entrada_1 = objHorario.Horariodescricao_1;
            objHorarioDMovel.Entrada_2 = objHorario.Horariodescricao_2;
            objHorarioDMovel.Entrada_3 = objHorario.Horariodescricao_3;
            objHorarioDMovel.Entrada_4 = objHorario.Horariodescricao_4;

            objHorarioDMovel.Saida_1 = objHorario.Horariodescricaosai_1;
            objHorarioDMovel.Saida_2 = objHorario.Horariodescricaosai_2;
            objHorarioDMovel.Saida_3 = objHorario.Horariodescricaosai_3;
            objHorarioDMovel.Saida_4 = objHorario.Horariodescricaosai_4;

            objHorarioDMovel.Totaltrabalhadadiurna = horaD;
            objHorarioDMovel.Totaltrabalhadanoturna = horaN;
            objHorarioDMovel.Cargahorariamista = horaM;
            objHorarioDMovel.Marcacargahorariamista = Convert.ToInt16(chbMarcacargahorariamista.Checked);

            objHorarioDMovel.Intervaloautomatico = Convert.ToInt16(chbIntervaloAutomatico.Checked);
            objHorarioDMovel.Preassinaladas1 = Convert.ToInt16(chbPreassinaladas1.Checked);
            objHorarioDMovel.Preassinaladas2 = Convert.ToInt16(chbPreassinaladas2.Checked);
            objHorarioDMovel.Preassinaladas3 = Convert.ToInt16(chbPreassinaladas3.Checked);
            objHorarioDMovel.Idjornada = objJornada.Id;

            return objHorarioDMovel;
        }

        private void sb5_1_Click(object sender, EventArgs e)
        {
            ChamaRotinaGerar(1);
        }

        private void sb12_36_Click(object sender, EventArgs e)
        {
            ChamaRotinaGerar(2);
        }

        private void sb24_48_Click(object sender, EventArgs e)
        {
            ChamaRotinaGerar(3);
        }

        private void sb6_2_Click(object sender, EventArgs e)
        {
            ChamaRotinaGerar(4);
        }

        private void sbTurnoCompleto_Click(object sender, EventArgs e)
        {
            ChamaRotinaGerar(5);
        }

        private void sb12_60_Click(object sender, EventArgs e)
        {
            ChamaRotinaGerar(6);
        }

        /// <summary>
        /// Chama a rotina que gera os horários automáticamente
        /// </summary>
        /// <param name="tipo">
        /// 1 = 5/1 | 2 = 12/36 | 3 = 24/48 |
        /// 4 = 6/2 | 5 = Turno Completo
        /// </param>
        private void ChamaRotinaGerar(int tipo)
        {
            objHorario.Habilitaperiodo01 = Convert.ToInt16(chbHabilitaperiodo01.Checked);
            objHorario.Habilitaperiodo02 = Convert.ToInt16(chbHabilitaperiodo02.Checked);
            //if (((chbHorasnormais.Checked) && ((string)txtCargaDiurna.EditValue == "--:--" && (string)txtCargaNoturna.EditValue == "--:--"))
            //    || (chbMarcacargahorariamista.Checked && (string)txtCargaMista.EditValue == "--:--"))
            if (objJornada == null)
            {
                MessageBox.Show("Selecione a jornada para gerar os horários flexíveis.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Modelo.HorarioDetalheMovel objHorarioDMovel = AuxGerar();
                switch (tipo)
                {
                    case 1:
                        cwErro = bllHorarioDetalhe.Gerar5_1(objHorarioDMovel, txtDataInicial.DateTime, txtDataFinal.DateTime, objHorario);
                        break;
                    case 2:
                        cwErro = bllHorarioDetalhe.Gerar12_36(objHorarioDMovel, txtDataInicial.DateTime, txtDataFinal.DateTime, objHorario);
                        break;
                    case 3:
                        cwErro = bllHorarioDetalhe.Gerar24_48(objHorarioDMovel, txtDataInicial.DateTime, txtDataFinal.DateTime, objHorario);
                        break;
                    case 4:
                        cwErro = bllHorarioDetalhe.Gerar6_2(objHorarioDMovel, txtDataInicial.DateTime, txtDataFinal.DateTime, objHorario);
                        break;
                    case 5:
                        cwErro = bllHorarioDetalhe.GerarTurnoCompleto(objHorarioDMovel, txtDataInicial.DateTime, txtDataFinal.DateTime, objHorario);
                        break;
                    case 6:
                        cwErro = bllHorarioDetalhe.Gerar12_60(objHorarioDMovel, txtDataInicial.DateTime, txtDataFinal.DateTime, objHorario);
                        break;
                }

                setErro(this.Controls);
                CarregaGrid();
            }
        }

        private void sbExcluiHorarios_Click(object sender, EventArgs e)
        {
            DateTime data = Convert.ToDateTime(gvHorarios.GetFocusedRowCellValue("Data"));
            bllHorarioDetalhe.ExcluirHorariosMovel(objHorario.HorariosFlexiveis, data);
            CarregaGrid();
        }

        #endregion

        #region Parâmetros de Horário

        private void chb50_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbMarcapercentualextra50.Checked)
            {
                RemoveHorarioPHExtra(txtPercentualextra50);
            }
            txtPercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
            txtQuantidadeextra50.Enabled = chbMarcapercentualextra50.Checked;

            checaLimite[0] = chbMarcapercentualextra50.Checked;
            checaLimite[1] = chbMarcapercentualextra60.Checked;
            checaLimite[2] = chbMarcapercentualextra70.Checked;
            checaLimite[3] = chbMarcapercentualextra80.Checked;
            checaLimite[4] = chbMarcapercentualextra90.Checked;
            checaLimite[5] = chbMarcapercentualextra100.Checked;

            _Limite = 0;

            for (int i = 0; i <= 5; i++)
            {
                if (checaLimite[i])
                {
                    _Limite++;
                    if (_Limite >= 4)
                    {
                        chbMarcapercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
                        chbMarcapercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
                        chbMarcapercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
                        chbMarcapercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
                        chbMarcapercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
                        chbMarcapercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
                        break;
                    }
                    else
                    {
                        chbMarcapercentualextra50.Enabled = true;
                        chbMarcapercentualextra60.Enabled = true;
                        chbMarcapercentualextra70.Enabled = true;
                        chbMarcapercentualextra80.Enabled = true;
                        chbMarcapercentualextra90.Enabled = true;
                        chbMarcapercentualextra100.Enabled = true;
                    }
                }
            }

            if (carregado)
            {
                if (txtPercentualextra50.Enabled)
                {
                    objHorario.HorariosPHExtra[0].Percentualextra = 050;
                    txtPercentualextra50.Value = 050;
                }
                else
                {
                    objHorario.HorariosPHExtra[0].Percentualextra = 0;
                    objHorario.HorariosPHExtra[0].Quantidadeextra = "---:--";
                    txtPercentualextra50.Value = 0;
                }
            }
        }

        private void chb60_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbMarcapercentualextra60.Checked)
            {
                RemoveHorarioPHExtra(txtPercentualextra60);
            }
            txtPercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
            txtQuantidadeextra60.Enabled = chbMarcapercentualextra60.Checked;

            checaLimite[0] = chbMarcapercentualextra50.Checked;
            checaLimite[1] = chbMarcapercentualextra60.Checked;
            checaLimite[2] = chbMarcapercentualextra70.Checked;
            checaLimite[3] = chbMarcapercentualextra80.Checked;
            checaLimite[4] = chbMarcapercentualextra90.Checked;
            checaLimite[5] = chbMarcapercentualextra100.Checked;

            _Limite = 0;

            for (int i = 0; i <= 5; i++)
            {
                if (checaLimite[i])
                {
                    _Limite++;
                    if (_Limite >= 4)
                    {
                        chbMarcapercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
                        chbMarcapercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
                        chbMarcapercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
                        chbMarcapercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
                        chbMarcapercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
                        chbMarcapercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
                        break;
                    }
                    else
                    {
                        chbMarcapercentualextra50.Enabled = true;
                        chbMarcapercentualextra60.Enabled = true;
                        chbMarcapercentualextra70.Enabled = true;
                        chbMarcapercentualextra80.Enabled = true;
                        chbMarcapercentualextra90.Enabled = true;
                        chbMarcapercentualextra100.Enabled = true;
                    }
                }
            }

            if (carregado)
            {
                if (txtPercentualextra60.Enabled)
                {
                    objHorario.HorariosPHExtra[1].Percentualextra = 060;
                    txtPercentualextra60.Value = 060;
                }
                else
                {
                    objHorario.HorariosPHExtra[1].Percentualextra = 0;
                    objHorario.HorariosPHExtra[1].Quantidadeextra = "---:--";
                    txtPercentualextra60.Value = 0;
                }
            }
        }

        private void chb70_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbMarcapercentualextra70.Checked)
            {
                RemoveHorarioPHExtra(txtPercentualextra70);
            }
            txtPercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
            txtQuantidadeextra70.Enabled = chbMarcapercentualextra70.Checked;

             checaLimite[0] = chbMarcapercentualextra50.Checked;
            checaLimite[1] = chbMarcapercentualextra60.Checked;
            checaLimite[2] = chbMarcapercentualextra70.Checked;
            checaLimite[3] = chbMarcapercentualextra80.Checked;
            checaLimite[4] = chbMarcapercentualextra90.Checked;
            checaLimite[5] = chbMarcapercentualextra100.Checked;

            _Limite = 0;

            for (int i = 0; i <= 5; i++)
            {
                if (checaLimite[i])
                {
                    _Limite++;
                    if (_Limite >= 4)
                    {
                        chbMarcapercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
                        chbMarcapercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
                        chbMarcapercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
                        chbMarcapercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
                        chbMarcapercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
                        chbMarcapercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
                        break;
                    }
                    else
                    {
                        chbMarcapercentualextra50.Enabled = true;
                        chbMarcapercentualextra60.Enabled = true;
                        chbMarcapercentualextra70.Enabled = true;
                        chbMarcapercentualextra80.Enabled = true;
                        chbMarcapercentualextra90.Enabled = true;
                        chbMarcapercentualextra100.Enabled = true;
                    }
                }
            }

            if (carregado)
            {
                if (txtPercentualextra70.Enabled)
                {
                    objHorario.HorariosPHExtra[2].Percentualextra = 070;
                    txtPercentualextra70.Value = 070;
                }
                else
                {
                    objHorario.HorariosPHExtra[2].Percentualextra = 0;
                    objHorario.HorariosPHExtra[2].Quantidadeextra = "---:--";
                    txtPercentualextra70.Value = 0;
                }
            }
        }

        private void chb80_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbMarcapercentualextra80.Checked)
            {
                RemoveHorarioPHExtra(txtPercentualextra80);
            }
            txtPercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
            txtQuantidadeextra80.Enabled = chbMarcapercentualextra80.Checked;

            checaLimite[0] = chbMarcapercentualextra50.Checked;
            checaLimite[1] = chbMarcapercentualextra60.Checked;
            checaLimite[2] = chbMarcapercentualextra70.Checked;
            checaLimite[3] = chbMarcapercentualextra80.Checked;
            checaLimite[4] = chbMarcapercentualextra90.Checked;
            checaLimite[5] = chbMarcapercentualextra100.Checked;

            _Limite = 0;

            for (int i = 0; i <= 5; i++)
            {
                if (checaLimite[i])
                {
                    _Limite++;
                    if (_Limite >= 4)
                    {
                        chbMarcapercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
                        chbMarcapercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
                        chbMarcapercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
                        chbMarcapercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
                        chbMarcapercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
                        chbMarcapercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
                        break;
                    }
                    else
                    {
                        chbMarcapercentualextra50.Enabled = true;
                        chbMarcapercentualextra60.Enabled = true;
                        chbMarcapercentualextra70.Enabled = true;
                        chbMarcapercentualextra80.Enabled = true;
                        chbMarcapercentualextra90.Enabled = true;
                        chbMarcapercentualextra100.Enabled = true;
                    }
                }
            }

            if (carregado)
            {
                if (txtPercentualextra80.Enabled)
                {
                    objHorario.HorariosPHExtra[3].Percentualextra = 080;
                    txtPercentualextra80.Value = 080;
                }
                else
                {
                    objHorario.HorariosPHExtra[3].Percentualextra = 0;
                    objHorario.HorariosPHExtra[3].Quantidadeextra = "---:--";
                    txtPercentualextra80.Value = 0;
                }
            }
        }

        private void chb90_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbMarcapercentualextra90.Checked)
            {
                RemoveHorarioPHExtra(txtPercentualextra90);
            }
            txtPercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
            txtQuantidadeextra90.Enabled = chbMarcapercentualextra90.Checked;

             checaLimite[0] = chbMarcapercentualextra50.Checked;
            checaLimite[1] = chbMarcapercentualextra60.Checked;
            checaLimite[2] = chbMarcapercentualextra70.Checked;
            checaLimite[3] = chbMarcapercentualextra80.Checked;
            checaLimite[4] = chbMarcapercentualextra90.Checked;
            checaLimite[5] = chbMarcapercentualextra100.Checked;

            _Limite = 0;

            for (int i = 0; i <= 5; i++)
            {
                if (checaLimite[i])
                {
                    _Limite++;
                    if (_Limite >= 4)
                    {
                        chbMarcapercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
                        chbMarcapercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
                        chbMarcapercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
                        chbMarcapercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
                        chbMarcapercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
                        chbMarcapercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
                        break;
                    }
                    else
                    {
                        chbMarcapercentualextra50.Enabled = true;
                        chbMarcapercentualextra60.Enabled = true;
                        chbMarcapercentualextra70.Enabled = true;
                        chbMarcapercentualextra80.Enabled = true;
                        chbMarcapercentualextra90.Enabled = true;
                        chbMarcapercentualextra100.Enabled = true;
                    }
                }
            }

            if (carregado)
            {
                if (txtPercentualextra90.Enabled)
                {
                    objHorario.HorariosPHExtra[4].Percentualextra = 090;
                    txtPercentualextra90.Value = 090;
                }
                else
                {
                    objHorario.HorariosPHExtra[4].Percentualextra = 0;
                    objHorario.HorariosPHExtra[4].Quantidadeextra = "---:--";
                    txtPercentualextra90.Value = 0;
                }
            }
        }

        private void chb100_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbMarcapercentualextra100.Checked)
            {
                RemoveHorarioPHExtra(txtPercentualextra100);
            }
            txtPercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
            txtQuantidadeextra100.Enabled = chbMarcapercentualextra100.Checked;

            checaLimite[0] = chbMarcapercentualextra50.Checked;
            checaLimite[1] = chbMarcapercentualextra60.Checked;
            checaLimite[2] = chbMarcapercentualextra70.Checked;
            checaLimite[3] = chbMarcapercentualextra80.Checked;
            checaLimite[4] = chbMarcapercentualextra90.Checked;
            checaLimite[5] = chbMarcapercentualextra100.Checked;

            _Limite = 0;

            for (int i = 0; i <= 5; i++)
            {
                if (checaLimite[i])
                {
                    _Limite++;
                    if (_Limite >= 4)
                    {
                        chbMarcapercentualextra50.Enabled = chbMarcapercentualextra50.Checked;
                        chbMarcapercentualextra60.Enabled = chbMarcapercentualextra60.Checked;
                        chbMarcapercentualextra70.Enabled = chbMarcapercentualextra70.Checked;
                        chbMarcapercentualextra80.Enabled = chbMarcapercentualextra80.Checked;
                        chbMarcapercentualextra90.Enabled = chbMarcapercentualextra90.Checked;
                        chbMarcapercentualextra100.Enabled = chbMarcapercentualextra100.Checked;
                        break;
                    }
                    else
                    {
                        chbMarcapercentualextra50.Enabled = true;
                        chbMarcapercentualextra60.Enabled = true;
                        chbMarcapercentualextra70.Enabled = true;
                        chbMarcapercentualextra80.Enabled = true;
                        chbMarcapercentualextra90.Enabled = true;
                        chbMarcapercentualextra100.Enabled = true;
                    }
                }
            }

            if (carregado)
            {
                if (txtPercentualextra100.Enabled)
                {
                    objHorario.HorariosPHExtra[5].Percentualextra = 100;
                    txtPercentualextra100.Value = 100;
                }
                else
                {
                    objHorario.HorariosPHExtra[5].Percentualextra = 0;
                    objHorario.HorariosPHExtra[5].Quantidadeextra = "---:--";
                    txtPercentualextra100.Value = 0;
                }
            }
        }

        private void chbSabado_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraSabado.Enabled = chbMarcapercentualextraSabado.Checked;
            txtQuantidadeextraSabado.Enabled = chbMarcapercentualextraSabado.Checked;
            cbPercentualExtraSab.Enabled = chbMarcapercentualextraSabado.Checked;
            cbTipoAcumuloSab.Enabled = chbMarcapercentualextraSabado.Checked;
            cbPercentualExtraSab.EditValue = 0;
            cbTipoAcumuloSab.EditValue = null;
            if (chbMarcapercentualextraSabado.Checked)
                cbTipoAcumuloSab.SelectedIndex = 0;


            if (txtPercentualextraSabado.Enabled)
            {
                objHorario.HorariosPHExtra[6].PercentualExtraSegundo = 100;
                txtPercentualextraSabado.Value = 100;
            }
            else
            {
                objHorario.HorariosPHExtra[6].PercentualExtraSegundo = 0;
                objHorario.HorariosPHExtra[6].Quantidadeextra = "---:--";
                txtPercentualextraSabado.Value = 0;
            }
        }

        private void chbDomingo_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraDomingo.Enabled = chbMarcapercentualextraDomingo.Checked;
            txtQuantidadeextraDomingo.Enabled = chbMarcapercentualextraDomingo.Checked;
            cbPercentualExtraDom.Enabled = chbMarcapercentualextraDomingo.Checked;
            cbTipoAcumuloDom.Enabled = chbMarcapercentualextraDomingo.Checked;
            cbPercentualExtraDom.EditValue = 0;
            cbTipoAcumuloDom.EditValue = null;
            if (chbMarcapercentualextraDomingo.Checked)
                cbTipoAcumuloDom.SelectedIndex = 0;


            if (txtPercentualextraDomingo.Enabled)
            {
                objHorario.HorariosPHExtra[7].PercentualExtraSegundo = 100;
                txtPercentualextraDomingo.Value = 100;
            }
            else
            {
                objHorario.HorariosPHExtra[7].PercentualExtraSegundo = 0;
                objHorario.HorariosPHExtra[7].Quantidadeextra = "---:--";
                txtPercentualextraDomingo.Value = 0;
            }
        }

        private void chbFeriado_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraFeriado.Enabled = chbMarcapercentualextraFeriado.Checked;
            txtQuantidadeextraFeriado.Enabled = chbMarcapercentualextraFeriado.Checked;
            cbPercentualExtraFer.Enabled = chbMarcapercentualextraFeriado.Checked;
            cbTipoAcumuloFer.Enabled = chbMarcapercentualextraFeriado.Checked;
            cbPercentualExtraFer.EditValue = 0;
            cbTipoAcumuloFer.EditValue = null;
            if (chbMarcapercentualextraFeriado.Checked)
                cbTipoAcumuloFer.SelectedIndex = 0;


            if (txtPercentualextraFeriado.Enabled)
            {
                objHorario.HorariosPHExtra[8].PercentualExtraSegundo = 100;
                txtPercentualextraFeriado.Value = 100;
            }
            else
            {
                objHorario.HorariosPHExtra[8].PercentualExtraSegundo = 0;
                objHorario.HorariosPHExtra[8].Quantidadeextra = "---:--";
                txtPercentualextraFeriado.Value = 0;
            }
        }

        private void chbFolga_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraFolga.Enabled = chbMarcapercentualextraFolga.Checked;
            txtQuantidadeextraFolga.Enabled = chbMarcapercentualextraFolga.Checked;
            cbPercentualExtraFol.Enabled = chbMarcapercentualextraFolga.Checked;
            cbTipoAcumuloFol.Enabled = chbMarcapercentualextraFolga.Checked;
            cbPercentualExtraFol.EditValue = 0;
            cbTipoAcumuloFol.EditValue = null;
            if (chbMarcapercentualextraFolga.Checked)
                cbTipoAcumuloFol.SelectedIndex = 0;


            if (txtPercentualextraFolga.Enabled)
            {
                objHorario.HorariosPHExtra[9].PercentualExtraSegundo = 100;
                txtPercentualextraFolga.Value = 100;
            }
            else
            {
                objHorario.HorariosPHExtra[9].PercentualExtraSegundo = 0;
                objHorario.HorariosPHExtra[9].Quantidadeextra = "---:--";
                txtPercentualextraFolga.Value = 0;
                //txtQuantidadeextraFolga.EditValue = "---:--";
            }
        }

        private void chbHabilitaperiodo01_CheckedChanged(object sender, EventArgs e)
        {
            CalculoCargaHoraria();
            if (chbHabilitaperiodo01.Checked && chbDescontacafemanha.Checked)
            {
                chbDescontacafemanha.Checked = false;
            }

        }

        private void chbDescontacafemanha_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDescontacafemanha.Checked && chbHabilitaperiodo01.Checked)
            {
                chbHabilitaperiodo01.Checked = false;
            }
        }

        private void chbHabilitaperiodo02_CheckedChanged(object sender, EventArgs e)
        {
            CalculoCargaHoraria();
            if (chbHabilitaperiodo02.Checked && chbDescontacafetarde.Checked)
            {
                chbDescontacafetarde.Checked = false;
            }
        }

        private void chbDescontacafetarde_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDescontacafetarde.Checked && chbHabilitaperiodo02.Checked)
            {
                chbHabilitaperiodo02.Checked = false;
            }
        }

        #endregion

        #region DSR

        private void chbDescontardsr_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDescontardsr.Checked == true)
                chbDdrsProporcional.Enabled = true;
            else
            {
                chbDdrsProporcional.EditValue = null;
                chbDdrsProporcional.Enabled = false;
            }

            txtQtdhorasdsr.Enabled = txtLimiteperdadsr.Enabled = chbDescontardsr.Checked;

            if (!chbDescontardsr.Checked)
            {
                txtQtdhorasdsr.EditValue = null;
                txtLimiteperdadsr.EditValue = null;
            }
        }

        #endregion

        #region Outros Eventos

        private void sbIdparametro_Click(object sender, EventArgs e)
        {
            FormGridParametros form = new FormGridParametros();
            form.cwTabela = "Parâmetros";
            form.cwId = (int)cbIdparametro.EditValue;
            GridSelecao(form, cbIdparametro, bllParametro);
        }

        private void gcHorarios_DoubleClick(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
        }

        private void gcHorarios_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
                    break;
            }
        }

        private void cbIdparametro_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdparametro.EditValue != objParametros.Id)
            {
                objParametros = bllParametro.LoadObject((int)cbIdparametro.EditValue);
                CalculoCargaHoraria();
            }
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

        private void chbIntervaloAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }

        private void txtSaida_1_Leave(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }

        private void chbDias_cafe_CheckedChanged(object sender, EventArgs e)
        {
            CalculoCargaHoraria();
        }

        private void chbConsiderasabadosemana_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConsiderasabadosemana.Checked)
            {
                chbMarcapercentualextraSabado.Checked = false;
                txtPercentualextraSabado.Enabled = false;
                txtQuantidadeextraSabado.Enabled = false;
                chbMarcapercentualextraSabado.Enabled = false;
                objHorario.HorariosPHExtra[6].Percentualextra = 0;
                objHorario.HorariosPHExtra[6].Quantidadeextra = "---:--";
                txtPercentualextraSabado.Value = 0;

            }
            else
            {
                chbMarcapercentualextraSabado.Enabled = true;
            }

        }

        private void chbConsideradomingosemana_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConsideradomingosemana.Checked)
            {
                chbMarcapercentualextraDomingo.Checked = false;
                txtPercentualextraDomingo.Enabled = false;
                txtQuantidadeextraDomingo.Enabled = false;
                chbMarcapercentualextraDomingo.Enabled = false;
                objHorario.HorariosPHExtra[7].Percentualextra = 0;
                objHorario.HorariosPHExtra[7].Quantidadeextra = "---:--";
                txtPercentualextraDomingo.Value = 0;
            }
            else
            {
                chbMarcapercentualextraDomingo.Enabled = true;
            }
        }

        private void chbConversaohoranoturna_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConversaohoranoturna.Checked)
            {
                chbConsideraadhtrabalhadas.Enabled = true;
            }
            else
            {
                chbConsideraadhtrabalhadas.Checked = false;
                chbConversaohoranoturna.Checked = false;
                chbConsideraadhtrabalhadas.Enabled = false;
            }
        }

        #endregion

        #endregion

        #region Métodos Auxiliares

        protected void CarregaGrid()
        {
            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            foreach (Modelo.HorarioDetalhe dja in objHorario.HorariosFlexiveis)
            {
                if (dja.Acao != Modelo.Acao.Excluir)
                {
                    lista.Add(dja);
                }
            }
            gcHorarios.DataSource = lista;
        }

        protected virtual void CarregaFormulario(Modelo.Acao pAcao, DateTime pData)
        {
            UI.FormRegistro form = new FormRegistro(objHorario, pData);
            form.cwAcao = pAcao;
            form.cwTabela = "Horário Detalhe";
            form.ShowDialog();
        }

        public void SelecionaRegistroPorID(string col, int ID)
        {
            int posicao = gvHorarios.LocateByDisplayText(0, gvHorarios.Columns.ColumnByFieldName(col), ID.ToString());
            if (posicao >= 0)
            {
                if (posicao > gvHorarios.RowCount - 1)
                {
                    posicao = gvHorarios.RowCount - 1;
                }
                gvHorarios.FocusedRowHandle = posicao;
                gvHorarios.SelectRow(posicao);
            }
            else
            {
                gvHorarios.ClearSelection();
                gvHorarios.SelectRow(0);
                gvHorarios.FocusedRowHandle = 0;
            }
        }

        public void SelecionaRegistroPorPos(int posicao)
        {
            if (posicao >= 0)
            {
                if (posicao > gvHorarios.RowCount - 1)
                {
                    posicao = gvHorarios.RowCount - 1;
                }
                gvHorarios.FocusedRowHandle = posicao;
                gvHorarios.SelectRow(posicao);
            }
            else
            {
                gvHorarios.ClearSelection();
                gvHorarios.SelectRow(0);
                gvHorarios.FocusedRowHandle = 0;
            }
        }

        protected DateTime RegistroSelecionado()
        {
            DateTime id;
            try
            {
                id = Convert.ToDateTime(gvHorarios.GetFocusedRowCellValue("Data"));
            }
            catch (Exception)
            {
                id = new DateTime();
            }
            return id;
        }

        private void CarregarManutencao(Modelo.Acao pAcao, DateTime pData)
        {
            try
            {
                if (pData == new DateTime() && pAcao != Modelo.Acao.Incluir)
                {
                    MessageBox.Show("Nenhum registro selecionado.");
                }
                else
                {
                    CarregaFormulario(pAcao, pData);
                }

                int pos = gvHorarios.FocusedRowHandle;
                CarregaGrid();
                if (!(gvHorarios.GetFocusedRow() == null || gvHorarios.FocusedRowHandle < 0))
                {
                    SelecionaRegistroPorPos(pos);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        #endregion

        #region Caraga Horaria

        private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN)
        {
            InicioHNoturna = objParametros.InicioAdNoturno;
            FimHNotura = objParametros.FimAdNoturno;

            totalD = "";
            totalN = "";

            BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        }

        private void CalculaHoras(string[] pEntrada, string[] pSaida)
        {
            if ((int)cbIdparametro.EditValue != 0)
            {
                string totalD;
                string totalN;
                AuxCalculaHoras(pEntrada, pSaida, out totalD, out totalN);

                //Não atribui o café para essas variáveis, pois elas serão utilizadas para calcular os totais dos horários detalhe
                horaD = totalD;
                horaN = totalN;                

                //Se tiver algum dia café ele calcula o total geral com café
                //if (chbDias_cafe_1.Checked || chbDias_cafe_2.Checked || chbDias_cafe_3.Checked || chbDias_cafe_4.Checked || chbDias_cafe_5.Checked || chbDias_cafe_6.Checked || chbDias_cafe_7.Checked)
                if (objHorario.Dias_cafe_1 == 1 || objHorario.Dias_cafe_2 == 1 || objHorario.Dias_cafe_3 == 1 
                    || objHorario.Dias_cafe_4 == 1 || objHorario.Dias_cafe_5 == 1 || objHorario.Dias_cafe_6 == 1 || objHorario.Dias_cafe_7 == 1)
                {
                    BLL.Horario.CalculaCafe(pEntrada, pSaida, chbHabilitaperiodo01.Checked, chbHabilitaperiodo02.Checked, ref totalD, ref totalN);
                }

                txtCargaDiurna.EditValue = (totalD != "00:00" ? totalD : "--:--");
                txtCargaNoturna.EditValue = (totalN != "00:00" ? totalN : "--:--");

                if (chbMarcacargahorariamista.Checked)
                {
                    horaM = BLL.CalculoHoras.OperacaoHoras('+', horaD, horaN);
                    horaD = "--:--";
                    horaN = "--:--";
                    txtCargaMista.EditValue = BLL.CalculoHoras.OperacaoHoras('+', totalD, totalN);
                    txtCargaDiurna.EditValue = "--:--";
                    txtCargaNoturna.EditValue = "--:--";
                }
                else
                {
                    horaM = "--:--";
                    txtCargaMista.EditValue = horaM;
                }
            }
        }

        private void CalculoCargaHoraria()
        {
            string[] entradas = new string[] { (string)txtEntrada_1.EditValue, (string)txtEntrada_2.EditValue, (string)txtEntrada_3.EditValue, (string)txtEntrada_4.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1.EditValue, (string)txtSaida_2.EditValue, (string)txtSaida_3.EditValue, (string)txtSaida_4.EditValue };

            CalculaHoras(entradas, saidas);
        }

        private void txtEntrada_1_EditValueChanged(object sender, EventArgs e)
        {
            CalculoCargaHoraria();
        }

        #endregion

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

                HabilitaIntervaloAutomatico();
                chbHorasnormais.Checked = true;
                chbHorasnormais_CheckedChanged(null, new EventArgs());
            }
        }

        private void txtPercentualextra50_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtPercentualextra50.EditValue != null)
            {
                try
                {
                    RemoveHorarioPHExtra(txtPercentualextra50);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra50_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPercentualextra50.EditValue != null)
            {
                try
                {
                    IncluiHorarioPHExtra(txtPercentualextra50);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra60_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtPercentualextra60.EditValue != null)
            {
                try
                {
                    RemoveHorarioPHExtra(txtPercentualextra60);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra60_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPercentualextra60.EditValue != null)
            {
                try
                {
                    IncluiHorarioPHExtra(txtPercentualextra60);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra70_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPercentualextra70.EditValue != null)
            {
                try
                {
                    IncluiHorarioPHExtra(txtPercentualextra70);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra80_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPercentualextra80.EditValue != null)
            {
                try
                {
                    IncluiHorarioPHExtra(txtPercentualextra80);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra90_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPercentualextra90.EditValue != null)
            {
                try
                {
                    IncluiHorarioPHExtra(txtPercentualextra90);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra100_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPercentualextra100.EditValue != null)
            {
                try
                {
                    IncluiHorarioPHExtra(txtPercentualextra100);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra70_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtPercentualextra70.EditValue != null)
            {
                try
                {
                    RemoveHorarioPHExtra(txtPercentualextra70);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra80_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtPercentualextra80.EditValue != null)
            {
                try
                {
                    RemoveHorarioPHExtra(txtPercentualextra80);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra90_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtPercentualextra90.EditValue != null)
            {
                try
                {
                    RemoveHorarioPHExtra(txtPercentualextra90);
                }
                catch
                {
                }
            }
        }

        private void txtPercentualextra100_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (txtPercentualextra100.EditValue != null)
            {
                try
                {
                    RemoveHorarioPHExtra(txtPercentualextra100);
                }
                catch
                {
                }
            }
        }

        private void FormManutHorarioMovel_Shown(object sender, EventArgs e)
        {
            xtraTabControl2.SelectedTabPage = tabParametrosHorario;
            xtraTabControl2.SelectedTabPage = tabTurnoNormal;
            
        }

        private void VerificaPreenchimentobancoHorasPercentual()
        {
            if (!chbSegundaPercBanco.Checked)
                txtSegundaPercBanco.Enabled = false;
            if (!chbTercaPercBanco.Checked)
                txtTercaPercBanco.Enabled = false;
            if (!chbQuartaPercBanco.Checked)
                txtQuartaPercBanco.Enabled = false;
            if (!chbQuintaPercBanco.Checked)
                txtQuintaPercBanco.Enabled = false;
            if (!chbSextaPercBanco.Checked)
                txtSextaPercBanco.Enabled = false;
            if (!chbSabadoPercBanco.Checked)
                txtSabadoPercBanco.Enabled = false;
            if (!chbDomingoPercBanco.Checked)
                txtDomingoPercBanco.Enabled = false;
            if (!chbFeriadoPercBanco.Checked)
                txtFeriadoPercBanco.Enabled = false;
            if (!chbFolgaPercBanco.Checked)
                txtFolgaPercBanco.Enabled = false;
        }

       
        private void chbSegundaPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbSegundaPercBanco.CheckState == CheckState.Unchecked)
            {
                txtSegundaPercBanco.EditValue = null;
                txtSegundaPercBanco.Enabled = false;
            }
            else
            {
                txtSegundaPercBanco.Enabled = true;
            }
        }

        private void chbTercaPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbTercaPercBanco.CheckState == CheckState.Unchecked)
            {
                txtTercaPercBanco.EditValue = null;
                txtTercaPercBanco.Enabled = false;
            }
            else
            {
                txtTercaPercBanco.Enabled = true;
            }
        }

        private void chbQuartaPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbQuartaPercBanco.CheckState == CheckState.Unchecked)
            {
                txtQuartaPercBanco.EditValue = null;
                txtQuartaPercBanco.Enabled = false;
            }
            else
            {
                txtQuartaPercBanco.Enabled = true;
            }
        }

        private void chbQuintaPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbQuintaPercBanco.CheckState == CheckState.Unchecked)
            {
                txtQuintaPercBanco.EditValue = null;
                txtQuintaPercBanco.Enabled = false;
            }
            else
            {
                txtQuintaPercBanco.Enabled = true;
            }
        }

        private void chbSextaPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbSextaPercBanco.CheckState == CheckState.Unchecked)
            {
                txtSextaPercBanco.EditValue = null;
                txtSextaPercBanco.Enabled = false;
            }
            else
            {
                txtSextaPercBanco.Enabled = true;
            }
        }

        private void chbSabadoPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbSabadoPercBanco.CheckState == CheckState.Unchecked)
            {
                txtSabadoPercBanco.EditValue = null;
                txtSabadoPercBanco.Enabled = false;
            }
            else
            {
                txtSabadoPercBanco.Enabled = true;
            }
        }

        private void chbDomingoPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbDomingoPercBanco.CheckState == CheckState.Unchecked)
            {
                txtDomingoPercBanco.EditValue = null;
                txtDomingoPercBanco.Enabled = false;
            }
            else
            {
                txtDomingoPercBanco.Enabled = true;
            }
        }

        private void chbFeriadoPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbFeriadoPercBanco.CheckState == CheckState.Unchecked)
            {
                txtFeriadoPercBanco.EditValue = null;
                txtFeriadoPercBanco.Enabled = false;
            }
            else
            {
                txtFeriadoPercBanco.Enabled = true;
            }
        }

        private void chbFolgaPercBanco_CheckStateChanged(object sender, EventArgs e)
        {
            if (chbFolgaPercBanco.CheckState == CheckState.Unchecked)
            {
                txtFolgaPercBanco.EditValue = null;
                txtFolgaPercBanco.Enabled = false;
            }
            else
            {
                txtFolgaPercBanco.Enabled = true;
            }
        }

        private void AtualizaGridLimitesDDsrProporcionais()
        {
            var listaFiltrada = LimitesDDsrProporcionais.Where(s => s.Delete != true && s.Acao != Modelo.Acao.Excluir);
            if (listaFiltrada != null)
                listaFiltrada = listaFiltrada.ToList();

            gcLimiteDDsr.DataSource = listaFiltrada;
            gcLimiteDDsr.RefreshDataSource();
            gcLimiteDDsr.Refresh();
            gvLimiteDDsr.ClearColumnErrors();
        }

        private void chbDdrsProporcional_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaDesabilitaDDSRProporcional();
            if (chbDdrsProporcional.Checked)
                IncluirLinhaLimiteDDsr();
            else
                gcLimiteDDsr.DataSource = null;
        }

        private void HabilitaDesabilitaDDSRProporcional()
        {
            if (chbDdrsProporcional.Checked == true)
            {
                gcLimiteDDsr.Enabled = true;
                sbIncluirLimiteDDsr.Enabled = sbRemoverLimiteDDsr.Enabled = true;

                txtQtdhorasdsr.Enabled = txtLimiteperdadsr.Enabled = false;
                txtQtdhorasdsr.EditValue = txtLimiteperdadsr.EditValue = null;
            }
            else
            {
                LimitesDDsrProporcionais = new List<Modelo.LimiteDDsr>();
                AtualizaGridLimitesDDsrProporcionais();

                gcLimiteDDsr.Enabled = sbIncluirLimiteDDsr.Enabled = sbRemoverLimiteDDsr.Enabled = false;
                txtQtdhorasdsr.Enabled = txtLimiteperdadsr.Enabled = true;
            }
        }

        private void sbIncluirLimiteDDsr_Click(object sender, EventArgs e)
        {
            IncluirLinhaLimiteDDsr();
        }

        private void IncluirLinhaLimiteDDsr()
        {
            if (LimitesDDsrProporcionais == null)
            {
                LimitesDDsrProporcionais = new List<Modelo.LimiteDDsr>();
            }
            LimitesDDsrProporcionais.Add(new Modelo.LimiteDDsr());
            AtualizaGridLimitesDDsrProporcionais();
        }

        private void sbRemoverLimiteDDsr_Click(object sender, EventArgs e)
        {
            int[] rows = gvLimiteDDsr.GetSelectedRows();
            if (rows.Length > 0)
            {
                Modelo.LimiteDDsr objSelecionado;
                objSelecionado = (Modelo.LimiteDDsr)gvLimiteDDsr.GetRow(rows[0]);

                if (LimitesDDsrProporcionais.Where(s => s.Id == objSelecionado.Id).FirstOrDefault() != null)
                {
                    LimitesDDsrProporcionais.Where(s => s.Id == objSelecionado.Id).FirstOrDefault().Delete = true;
                    LimitesDDsrProporcionais.Where(s => s.Id == objSelecionado.Id).FirstOrDefault().Acao = Modelo.Acao.Excluir;
                }

                AtualizaGridLimitesDDsrProporcionais();

            }
        }
    }
}
