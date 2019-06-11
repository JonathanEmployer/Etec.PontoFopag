using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace UI
{
    public partial class FormManutHorario : UI.Base.ManutBase
    {
        #region Atributos e Propriedades

        private List<Modelo.LimiteDDsr> LimitesDDsrProporcionais;

        public bool[] checaLimite = new bool[6];
        public int _Limite = 0;
        private bool carregado;
        private bool bmarcar;
        private Componentes.devexpress.cwkEditHora[,] entradas_saidas = new Componentes.devexpress.cwkEditHora[7, 8];
        private BLL.Parametros bllParametro;
        private BLL.Horario bllHorario;
        private BLL.HorarioDetalhe bllHorarioDetalhe;
        private BLL.Jornada bllJornada;
        private BLL.Marcacao bllMarcacao;
        private Modelo.Horario objHorario;
        private Modelo.Parametros objParametros;
        private Modelo.Jornada objJornada;

        private string InicioHNoturna { get; set; }
        private string FimHNotura { get; set; }
        #endregion

        public FormManutHorario()
        {
            InitializeComponent();
            bllHorario = new BLL.Horario();
            bllParametro = new BLL.Parametros();
            bllHorarioDetalhe = new BLL.HorarioDetalhe();
            bllJornada = new BLL.Jornada();
            bllMarcacao = new BLL.Marcacao();
            this.Name = "FormManutHorario";
            entradas_saidas[0, 0] = txtEntrada_1Segunda;
            entradas_saidas[0, 1] = txtSaida_1Segunda;
            entradas_saidas[0, 2] = txtEntrada_2Segunda;
            entradas_saidas[0, 3] = txtSaida_2Segunda;
            entradas_saidas[0, 4] = txtEntrada_3Segunda;
            entradas_saidas[0, 5] = txtSaida_3Segunda;
            entradas_saidas[0, 6] = txtEntrada_4Segunda;
            entradas_saidas[0, 7] = txtSaida_4Segunda;

            entradas_saidas[1, 0] = txtEntrada_1Terca;
            entradas_saidas[1, 1] = txtSaida_1Terca;
            entradas_saidas[1, 2] = txtEntrada_2Terca;
            entradas_saidas[1, 3] = txtSaida_2Terca;
            entradas_saidas[1, 4] = txtEntrada_3Terca;
            entradas_saidas[1, 5] = txtSaida_3Terca;
            entradas_saidas[1, 6] = txtEntrada_4Terca;
            entradas_saidas[1, 7] = txtSaida_4Terca;

            entradas_saidas[2, 0] = txtEntrada_1Quarta;
            entradas_saidas[2, 1] = txtSaida_1Quarta;
            entradas_saidas[2, 2] = txtEntrada_2Quarta;
            entradas_saidas[2, 3] = txtSaida_2Quarta;
            entradas_saidas[2, 4] = txtEntrada_3Quarta;
            entradas_saidas[2, 5] = txtSaida_3Quarta;
            entradas_saidas[2, 6] = txtEntrada_4Quarta;
            entradas_saidas[2, 7] = txtSaida_4Quarta;

            entradas_saidas[3, 0] = txtEntrada_1Quinta;
            entradas_saidas[3, 1] = txtSaida_1Quinta;
            entradas_saidas[3, 2] = txtEntrada_2Quinta;
            entradas_saidas[3, 3] = txtSaida_2Quinta;
            entradas_saidas[3, 4] = txtEntrada_3Quinta;
            entradas_saidas[3, 5] = txtSaida_3Quinta;
            entradas_saidas[3, 6] = txtEntrada_4Quinta;
            entradas_saidas[3, 7] = txtSaida_4Quinta;

            entradas_saidas[4, 0] = txtEntrada_1Sexta;
            entradas_saidas[4, 1] = txtSaida_1Sexta;
            entradas_saidas[4, 2] = txtEntrada_2Sexta;
            entradas_saidas[4, 3] = txtSaida_2Sexta;
            entradas_saidas[4, 4] = txtEntrada_3Sexta;
            entradas_saidas[4, 5] = txtSaida_3Sexta;
            entradas_saidas[4, 6] = txtEntrada_4Sexta;
            entradas_saidas[4, 7] = txtSaida_4Sexta;

            entradas_saidas[5, 0] = txtEntrada_1Sabado;
            entradas_saidas[5, 1] = txtSaida_1Sabado;
            entradas_saidas[5, 2] = txtEntrada_2Sabado;
            entradas_saidas[5, 3] = txtSaida_2Sabado;
            entradas_saidas[5, 4] = txtEntrada_3Sabado;
            entradas_saidas[5, 5] = txtSaida_3Sabado;
            entradas_saidas[5, 6] = txtEntrada_4Sabado;
            entradas_saidas[5, 7] = txtSaida_4Sabado;

            entradas_saidas[6, 0] = txtEntrada_1Domingo;
            entradas_saidas[6, 1] = txtSaida_1Domingo;
            entradas_saidas[6, 2] = txtEntrada_2Domingo;
            entradas_saidas[6, 3] = txtSaida_2Domingo;
            entradas_saidas[6, 4] = txtEntrada_3Domingo;
            entradas_saidas[6, 5] = txtSaida_3Domingo;
            entradas_saidas[6, 6] = txtEntrada_4Domingo;
            entradas_saidas[6, 7] = txtSaida_4Domingo;
        }

        public override void CarregaObjeto()
        {
            chbConsideraadhtrabalhadas.Enabled = false;
            carregado = false;
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    bllHorario.InicializaHorario(ref objHorario);
                    rgTipoacumulo.SelectedIndex = -1;
                    InicializaPercBH();
                    objParametros = bllParametro.LoadPrimeiro();
                    objHorario.Idparametro = objParametros.Id;
                    objHorario.Limitemin = "03:00";
                    objHorario.Limitemax = "03:00";
                    checaLimite[0] = false;
                    checaLimite[1] = false;
                    checaLimite[2] = false;
                    checaLimite[3] = false;
                    checaLimite[4] = false;
                    checaLimite[5] = false;
                    objHorario.HorariosPHExtra[6].TipoAcumulo = -1;
                    objHorario.HorariosPHExtra[7].TipoAcumulo = -1;
                    objHorario.HorariosPHExtra[8].TipoAcumulo = -1;
                    objHorario.HorariosPHExtra[9].TipoAcumulo = -1;

                    break;
                default:
                    objHorario = bllHorario.LoadObject(cwID);
                    cbIdJornada.EditValue = objHorario.HorariosDetalhe[0].Idjornada;
                    objHorario.LimitesDDsrProporcionais.ForEach(i => i.Acao = Modelo.Acao.Alterar);
                    break;
            }

            cbIdparametro.Properties.DataSource = bllParametro.GetAll();
            AtualizaJornadas();

            txtCodigo.DataBindings.Add("EditValue", objHorario, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objHorario, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdparametro.DataBindings.Add("EditValue", objHorario, "Idparametro", true, DataSourceUpdateMode.OnPropertyChanged);

            chbHabilitaperiodo01.Checked = Convert.ToBoolean(objHorario.Habilitaperiodo01);
            chbHabilitaperiodo02.Checked = Convert.ToBoolean(objHorario.Habilitaperiodo02);
            chbDescontacafemanha.Checked = Convert.ToBoolean(objHorario.Descontacafemanha);
            chbDescontacafetarde.Checked = Convert.ToBoolean(objHorario.Descontacafetarde);

            chbDdrsProporcional.Checked = objHorario.bUtilizaDDSRProporcional;

            chbIntervaloAutomatico.Checked = Convert.ToBoolean(objHorario.Intervaloautomatico);

            chbOrdem_ent.DataBindings.Add("Checked", objHorario, "Ordem_ent", true, DataSourceUpdateMode.OnPropertyChanged);
            chbOrdenabilhetesaida.DataBindings.Add("Checked", objHorario, "Ordenabilhetesaida", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimitemin.DataBindings.Add("EditValue", objHorario, "Limitemin", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimitemax.DataBindings.Add("EditValue", objHorario, "Limitemax", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipoacumulo.DataBindings.Add("EditValue", objHorario, "Tipoacumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_1.DataBindings.Add("Checked", objHorario, "Dias_cafe_1", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_2.DataBindings.Add("Checked", objHorario, "Dias_cafe_2", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_3.DataBindings.Add("Checked", objHorario, "Dias_cafe_3", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_4.DataBindings.Add("Checked", objHorario, "Dias_cafe_4", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_5.DataBindings.Add("Checked", objHorario, "Dias_cafe_5", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_6.DataBindings.Add("Checked", objHorario, "Dias_cafe_6", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_cafe_7.DataBindings.Add("Checked", objHorario, "Dias_cafe_7", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDescontafalta50.DataBindings.Add("Checked", objHorario, "Descontafalta50", true, DataSourceUpdateMode.OnPropertyChanged);
            txtObs.DataBindings.Add("EditValue", objHorario, "Obs", true, DataSourceUpdateMode.OnPropertyChanged);

            chbConversaohoranoturna.Checked = Convert.ToBoolean(objHorario.Conversaohoranoturna);
            chbConsideraadhtrabalhadas.Checked = Convert.ToBoolean(objHorario.Consideraadhtrabalhadas);
            chbConsiderasabadosemana.Checked = Convert.ToBoolean(objHorario.Considerasabadosemana);
            chbConsideradomingosemana.Checked = Convert.ToBoolean(objHorario.Consideradomingosemana);
            chbHorasnormais.Checked = Convert.ToBoolean(objHorario.Horasnormais);
            chbMarcacargahorariamista.Checked = Convert.ToBoolean(objHorario.Marcacargahorariamista);
            chbSomentecargahoraria.Checked = Convert.ToBoolean(objHorario.Somentecargahoraria);
            chbDescontardsr.Checked = Convert.ToBoolean(objHorario.Descontardsr);

            txtQtdhorasdsr.DataBindings.Add("EditValue", objHorario, "Qtdhorasdsr", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteperdadsr.DataBindings.Add("EditValue", objHorario, "Limiteperdadsr", true, DataSourceUpdateMode.OnPropertyChanged);
            rgDiasemanadsr.DataBindings.Add("EditValue", objHorario, "Diasemanadsr", true, DataSourceUpdateMode.OnPropertyChanged);

            txtLimiteHorasTrabalhadasDia.EditValue = BuscaLimiteHorasTrabalhadas();
            txtLimiteMinimoHorasAlmoco.EditValue = BuscaLimiteMinimoHorasAlmoco();

            objHorario.HorariosPHExtra[6].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[6].TipoAcumulo - 1);
            objHorario.HorariosPHExtra[7].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[7].TipoAcumulo - 1);
            objHorario.HorariosPHExtra[8].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[8].TipoAcumulo - 1);
            objHorario.HorariosPHExtra[9].TipoAcumulo = Convert.ToInt16(objHorario.HorariosPHExtra[9].TipoAcumulo - 1);

            gcLimiteDDsr.DataSource = LimitesDDsrProporcionais = objHorario.LimitesDDsrProporcionais;
            HabilitaDesabilitaDDSRProporcional();

            AtribuiHorariosDetalhe();
            AtribuiHorariosPHExtra();
            AtribuiPercentualBH();


            bmarcar = !(chbDiasSegunda.Checked && chbDiasTerca.Checked && chbDiasQuarta.Checked && chbDiasQuinta.Checked && chbDiasSexta.Checked && chbDiasSabado.Checked && chbDiasDomingo.Checked);

            base.CarregaObjeto();
            carregado = true;            
        }

        private string BuscaLimiteHorasTrabalhadas()
        {
            string retorno = String.Empty;
            if (objHorario.Id > 0)
            {
                retorno = objHorario.LimiteHorasTrabalhadasDia;
            }
            else
            {
                if (String.IsNullOrEmpty(objHorario.LimiteHorasTrabalhadasDia))
                    retorno = "10:00";
                else if (objHorario.LimiteHorasTrabalhadasDia == "--:--")
                    retorno = "10:00";
                else
                    retorno = objHorario.LimiteHorasTrabalhadasDia;

            }
            return retorno;
        }

        private string BuscaLimiteMinimoHorasAlmoco()
        {
            string retorno = String.Empty;
            if (objHorario.Id > 0)
            {
                retorno = objHorario.LimiteMinimoHorasAlmoco;
            }
            else
            {
                if (String.IsNullOrEmpty(objHorario.LimiteHorasTrabalhadasDia))
                    retorno = "01:00";
                else if (objHorario.LimiteMinimoHorasAlmoco == "--:--")
                    retorno = "01:00";
                else
                    retorno = objHorario.LimiteMinimoHorasAlmoco;

            }
           
            return retorno;
        }

        private void InicializaPercBH()
        {

            Boolean estado = false;
            chbPercBHSegunda.Checked = estado;
            chbPercBHTerca.Checked = estado;
            chbPercBHQuarta.Checked = estado;
            chbPercBHQuinta.Checked = estado;
            chbPercBHSexta.Checked = estado;
            chbPercBHSabado.Checked = estado;
            chbPercBHDomingo.Checked = estado;

            txtSegundaPercBanco.Enabled = estado;
            txtTercaPercBanco.Enabled = estado;
            txtQuartaPercBanco.Enabled = estado;
            txtQuintaPercBanco.Enabled = estado;
            txtSextaPercBanco.Enabled = estado;
            txtSabadoPercBanco.Enabled = estado;
            txtDomingoPercBanco.Enabled = estado;

        }

        private void AtribuiPercentualBH()
        {

            chbPercBHSegunda.DataBindings.Add("Checked", objHorario, "MarcaSegundaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHTerca.DataBindings.Add("Checked", objHorario, "MarcaTercaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHQuarta.DataBindings.Add("Checked", objHorario, "MarcaQuartaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHQuinta.DataBindings.Add("Checked", objHorario, "MarcaQuintaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHSexta.DataBindings.Add("Checked", objHorario, "MarcaSextaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHSabado.DataBindings.Add("Checked", objHorario, "MarcaSabadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHDomingo.DataBindings.Add("Checked", objHorario, "MarcaDomingoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHFeriado.DataBindings.Add("Checked", objHorario, "MarcaFeriadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            chbPercBHFolga.DataBindings.Add("Checked", objHorario, "MarcaFolgaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);

            txtSegundaPercBanco.DataBindings.Add("EditValue", objHorario, "SegundaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTercaPercBanco.DataBindings.Add("EditValue", objHorario, "TercaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuartaPercBanco.DataBindings.Add("EditValue", objHorario, "QuartaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQuintaPercBanco.DataBindings.Add("EditValue", objHorario, "QuintaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSextaPercBanco.DataBindings.Add("EditValue", objHorario, "SextaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSabadoPercBanco.DataBindings.Add("EditValue", objHorario, "SabadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDomingoPercBanco.DataBindings.Add("EditValue", objHorario, "DomingoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFeriadoPercBanco.DataBindings.Add("EditValue", objHorario, "FeriadoPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFolgaPercBanco.DataBindings.Add("EditValue", objHorario, "FolgaPercBanco", true, DataSourceUpdateMode.OnPropertyChanged);

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

        private void AtribuiHorariosDetalhe()
        {

            #region Segunda

            if (objHorario.HorariosDetalhe[0].Id > 0)
            {
                objHorario.HorariosDetalhe[0].Acao = Modelo.Acao.Alterar;
            }
            chbDiasSegunda.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[0].bCarregar);
            chbNeutroSegunda.Checked = objHorario.HorariosDetalhe[0].Neutro;
            chbPASeg1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[0].Preassinaladas1);
            chbPASeg2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[0].Preassinaladas2);
            chbPASeg3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[0].Preassinaladas3);
            for (int i = 0; i < 7; i++)
            {
                entradas_saidas[i, 0].EditValue = objHorario.HorariosDetalhe[i].Entrada_1;
                entradas_saidas[i, 1].EditValue = objHorario.HorariosDetalhe[i].Saida_1;
                entradas_saidas[i, 2].EditValue = objHorario.HorariosDetalhe[i].Entrada_2;
                entradas_saidas[i, 3].EditValue = objHorario.HorariosDetalhe[i].Saida_2;
                entradas_saidas[i, 4].EditValue = objHorario.HorariosDetalhe[i].Entrada_3;
                entradas_saidas[i, 5].EditValue = objHorario.HorariosDetalhe[i].Saida_3;
                entradas_saidas[i, 6].EditValue = objHorario.HorariosDetalhe[i].Entrada_4;
                entradas_saidas[i, 7].EditValue = objHorario.HorariosDetalhe[i].Saida_4;
            }

            txtTotaltrabalhadadiurna1.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[0], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna1.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[0], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista1.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[0], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaSeg.EditValue = objHorario.HorariosDetalhe[0].Idjornada;
            #endregion

            #region Terca

            if (objHorario.HorariosDetalhe[1].Id > 0)
            {
                objHorario.HorariosDetalhe[1].Acao = Modelo.Acao.Alterar;
            }

            chbDiasTerca.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[1].bCarregar);
            chbNeutroTerca.Checked = objHorario.HorariosDetalhe[1].Neutro;
            chbPATer1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[1].Preassinaladas1);
            chbPATer2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[1].Preassinaladas2);
            chbPATer3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[1].Preassinaladas3);

            txtTotaltrabalhadadiurna2.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[1], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna2.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[1], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista2.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[1], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaTer.EditValue = objHorario.HorariosDetalhe[1].Idjornada;
            #endregion

            #region Quarta

            if (objHorario.HorariosDetalhe[2].Id > 0)
            {
                objHorario.HorariosDetalhe[2].Acao = Modelo.Acao.Alterar;
            }

            chbDiasQuarta.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[2].bCarregar);
            chbNeutroQuarta.Checked = objHorario.HorariosDetalhe[2].Neutro;
            chbPAQua1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[2].Preassinaladas1);
            chbPAQua2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[2].Preassinaladas2);
            chbPAQua3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[2].Preassinaladas3);

            txtTotaltrabalhadadiurna3.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[2], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna3.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[2], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista3.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[2], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaQua.EditValue = objHorario.HorariosDetalhe[2].Idjornada;
            #endregion

            #region Quinta

            if (objHorario.HorariosDetalhe[3].Id > 0)
            {
                objHorario.HorariosDetalhe[3].Acao = Modelo.Acao.Alterar;
            }

            chbDiasQuinta.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[3].bCarregar);
            chbNeutroQuinta.Checked = objHorario.HorariosDetalhe[3].Neutro;
            chbPAQui1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[3].Preassinaladas1);
            chbPAQui2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[3].Preassinaladas2);
            chbPAQui3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[3].Preassinaladas3);

            txtTotaltrabalhadadiurna4.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[3], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna4.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[3], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista4.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[3], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaQui.EditValue = objHorario.HorariosDetalhe[3].Idjornada;
            #endregion

            #region Sexta

            if (objHorario.HorariosDetalhe[4].Id > 0)
            {
                objHorario.HorariosDetalhe[4].Acao = Modelo.Acao.Alterar;
            }

            chbDiasSexta.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[4].bCarregar);
            chbNeutroSexta.Checked = objHorario.HorariosDetalhe[4].Neutro;
            chbPASex1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[4].Preassinaladas1);
            chbPASex2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[4].Preassinaladas2);
            chbPASex3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[4].Preassinaladas3);

            txtTotaltrabalhadadiurna5.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[4], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna5.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[4], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista5.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[4], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaSex.EditValue = objHorario.HorariosDetalhe[4].Idjornada;
            #endregion

            #region Sabado

            if (objHorario.HorariosDetalhe[5].Id > 0)
            {
                objHorario.HorariosDetalhe[5].Acao = Modelo.Acao.Alterar;
            }

            chbDiasSabado.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[5].bCarregar);
            chbNeutroSabado.Checked = objHorario.HorariosDetalhe[5].Neutro;
            chbPASab1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[5].Preassinaladas1);
            chbPASab2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[5].Preassinaladas2);
            chbPASab3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[5].Preassinaladas3);

            txtTotaltrabalhadadiurna6.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[5], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna6.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[5], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista6.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[5], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaSab.EditValue = objHorario.HorariosDetalhe[5].Idjornada;
            #endregion

            #region Domingo

            if (objHorario.HorariosDetalhe[6].Id > 0)
            {
                objHorario.HorariosDetalhe[6].Acao = Modelo.Acao.Alterar;
            }

            chbDiasDomingo.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[6].bCarregar);
            chbNeutroDomingo.Checked = objHorario.HorariosDetalhe[6].Neutro;
            chbPADom1.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[6].Preassinaladas1);
            chbPADom2.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[6].Preassinaladas2);
            chbPADom3.Checked = Convert.ToBoolean(objHorario.HorariosDetalhe[6].Preassinaladas3);

            txtTotaltrabalhadadiurna7.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[6], "Totaltrabalhadadiurna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotaltrabalhadanoturna7.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[6], "Totaltrabalhadanoturna", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCargaMista7.DataBindings.Add("EditValue", objHorario.HorariosDetalhe[6], "Cargahorariamista", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdjornadaDom.EditValue = objHorario.HorariosDetalhe[6].Idjornada;
            #endregion

        }

        public override Dictionary<string, string> Salvar()
        {
            FormProgressBar2 formPBRecalcula = new FormProgressBar2();
            Dictionary<string, string> ret = new Dictionary<string, string>();
            DateTime datainicial;
            DateTime datafinal;

            objHorario.bUtilizaDDSRProporcional = chbDdrsProporcional.Checked;
            objHorario.Descontacafemanha = Convert.ToInt16(chbDescontacafemanha.Checked);
            objHorario.Descontacafetarde = Convert.ToInt16(chbDescontacafetarde.Checked);
            objHorario.Habilitaperiodo01 = Convert.ToInt16(chbHabilitaperiodo01.Checked);
            objHorario.Habilitaperiodo02 = Convert.ToInt16(chbHabilitaperiodo02.Checked);

            objHorario.Intervaloautomatico = Convert.ToInt16(chbIntervaloAutomatico.Checked);

            objHorario.Considerasabadosemana = Convert.ToInt16(chbConsiderasabadosemana.Checked);
            objHorario.Consideradomingosemana = Convert.ToInt16(chbConsideradomingosemana.Checked);
            objHorario.Horasnormais = Convert.ToInt16(chbHorasnormais.Checked);
            objHorario.Marcacargahorariamista = Convert.ToInt16(chbMarcacargahorariamista.Checked);
            objHorario.Somentecargahoraria = Convert.ToInt16(chbSomentecargahoraria.Checked);
            objHorario.Descontardsr = Convert.ToInt16(chbDescontardsr.Checked);
            objHorario.Conversaohoranoturna = Convert.ToInt16(chbConversaohoranoturna.Checked);
            objHorario.Consideraadhtrabalhadas = Convert.ToInt16(chbConsideraadhtrabalhadas.Checked);

            objHorario.LimiteHorasTrabalhadasDia = Convert.ToString(txtLimiteHorasTrabalhadasDia.EditValue);
            objHorario.LimiteMinimoHorasAlmoco = Convert.ToString(txtLimiteMinimoHorasAlmoco.EditValue);

            for (int i = 0; i < objHorario.HorariosDetalhe.Length; i++)
            {
                objHorario.HorariosDetalhe[i].Intervaloautomatico = objHorario.Intervaloautomatico;
                objHorario.HorariosDetalhe[i].Marcacargahorariamista = objHorario.Marcacargahorariamista;

                objHorario.HorariosDetalhe[i].Entrada_1 = entradas_saidas[i, 0].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Saida_1 = entradas_saidas[i, 1].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Entrada_2 = entradas_saidas[i, 2].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Saida_2 = entradas_saidas[i, 3].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Entrada_3 = entradas_saidas[i, 4].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Saida_3 = entradas_saidas[i, 5].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Entrada_4 = entradas_saidas[i, 6].EditValue.ToString();
                objHorario.HorariosDetalhe[i].Saida_4 = entradas_saidas[i, 7].EditValue.ToString();
            }

            objHorario.HorariosDetalhe[0].bCarregar = Convert.ToInt16(chbDiasSegunda.Checked);
            objHorario.HorariosDetalhe[0].Neutro = chbNeutroSegunda.Checked;
            objHorario.HorariosDetalhe[0].Flagfolga = Convert.ToInt16(!chbDiasSegunda.Checked);
            objHorario.HorariosDetalhe[0].Idjornada = Convert.ToInt32(cbIdjornadaSeg.EditValue);
            objHorario.HorariosDetalhe[0].Preassinaladas1 = Convert.ToInt16(chbPASeg1.Checked);
            objHorario.HorariosDetalhe[0].Preassinaladas2 = Convert.ToInt16(chbPASeg2.Checked);
            objHorario.HorariosDetalhe[0].Preassinaladas3 = Convert.ToInt16(chbPASeg3.Checked);

            objHorario.HorariosDetalhe[1].bCarregar = Convert.ToInt16(chbDiasTerca.Checked);
            objHorario.HorariosDetalhe[1].Neutro = chbNeutroTerca.Checked;
            objHorario.HorariosDetalhe[1].Flagfolga = Convert.ToInt16(!chbDiasTerca.Checked);
            objHorario.HorariosDetalhe[1].Idjornada = Convert.ToInt32(cbIdjornadaTer.EditValue);
            objHorario.HorariosDetalhe[1].Preassinaladas1 = Convert.ToInt16(chbPATer1.Checked);
            objHorario.HorariosDetalhe[1].Preassinaladas2 = Convert.ToInt16(chbPATer2.Checked);
            objHorario.HorariosDetalhe[1].Preassinaladas3 = Convert.ToInt16(chbPATer3.Checked);

            objHorario.HorariosDetalhe[2].bCarregar = Convert.ToInt16(chbDiasQuarta.Checked);
            objHorario.HorariosDetalhe[2].Neutro = chbNeutroQuarta.Checked;
            objHorario.HorariosDetalhe[2].Flagfolga = Convert.ToInt16(!chbDiasQuarta.Checked);
            objHorario.HorariosDetalhe[2].Idjornada = Convert.ToInt32(cbIdjornadaQua.EditValue);
            objHorario.HorariosDetalhe[2].Preassinaladas1 = Convert.ToInt16(chbPAQua1.Checked);
            objHorario.HorariosDetalhe[2].Preassinaladas2 = Convert.ToInt16(chbPAQua2.Checked);
            objHorario.HorariosDetalhe[2].Preassinaladas3 = Convert.ToInt16(chbPAQua3.Checked);

            objHorario.HorariosDetalhe[3].bCarregar = Convert.ToInt16(chbDiasQuinta.Checked);
            objHorario.HorariosDetalhe[3].Neutro = chbNeutroQuinta.Checked;
            objHorario.HorariosDetalhe[3].Flagfolga = Convert.ToInt16(!chbDiasQuinta.Checked);
            objHorario.HorariosDetalhe[3].Idjornada = Convert.ToInt32(cbIdjornadaQui.EditValue);
            objHorario.HorariosDetalhe[3].Preassinaladas1 = Convert.ToInt16(chbPAQui1.Checked);
            objHorario.HorariosDetalhe[3].Preassinaladas2 = Convert.ToInt16(chbPAQui2.Checked);
            objHorario.HorariosDetalhe[3].Preassinaladas3 = Convert.ToInt16(chbPAQui3.Checked);

            objHorario.HorariosDetalhe[4].bCarregar = Convert.ToInt16(chbDiasSexta.Checked);
            objHorario.HorariosDetalhe[4].Neutro = chbNeutroSexta.Checked;
            objHorario.HorariosDetalhe[4].Flagfolga = Convert.ToInt16(!chbDiasSexta.Checked);
            objHorario.HorariosDetalhe[4].Idjornada = Convert.ToInt32(cbIdjornadaSex.EditValue);
            objHorario.HorariosDetalhe[4].Preassinaladas1 = Convert.ToInt16(chbPASex1.Checked);
            objHorario.HorariosDetalhe[4].Preassinaladas2 = Convert.ToInt16(chbPASex2.Checked);
            objHorario.HorariosDetalhe[4].Preassinaladas3 = Convert.ToInt16(chbPASex3.Checked);

            objHorario.HorariosDetalhe[5].bCarregar = Convert.ToInt16(chbDiasSabado.Checked);
            objHorario.HorariosDetalhe[5].Neutro = chbNeutroSabado.Checked;
            objHorario.HorariosDetalhe[5].Flagfolga = Convert.ToInt16(!chbDiasSabado.Checked);
            objHorario.HorariosDetalhe[5].Idjornada = Convert.ToInt32(cbIdjornadaSab.EditValue);
            objHorario.HorariosDetalhe[5].Preassinaladas1 = Convert.ToInt16(chbPASab1.Checked);
            objHorario.HorariosDetalhe[5].Preassinaladas2 = Convert.ToInt16(chbPASab2.Checked);
            objHorario.HorariosDetalhe[5].Preassinaladas3 = Convert.ToInt16(chbPASab3.Checked);

            objHorario.HorariosDetalhe[6].bCarregar = Convert.ToInt16(chbDiasDomingo.Checked);
            objHorario.HorariosDetalhe[6].Neutro = chbNeutroDomingo.Checked;
            objHorario.HorariosDetalhe[6].Flagfolga = Convert.ToInt16(!chbDiasDomingo.Checked);
            objHorario.HorariosDetalhe[6].Idjornada = Convert.ToInt32(cbIdjornadaDom.EditValue);
            objHorario.HorariosDetalhe[6].Preassinaladas1 = Convert.ToInt16(chbPADom1.Checked);
            objHorario.HorariosDetalhe[6].Preassinaladas2 = Convert.ToInt16(chbPADom2.Checked);
            objHorario.HorariosDetalhe[6].Preassinaladas3 = Convert.ToInt16(chbPADom3.Checked);

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
            objHorario.Descricao = objHorario.Descricao.TrimEnd();

            if (DateTime.Now.Month == 1)
            {
                datainicial = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(-1).Year), Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                datafinal = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
            }
            else if (DateTime.Now.Month == 12)
            {
                datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                datafinal = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(1).Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
            }
            else
            {
                datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                datafinal = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(1).Month), DateTime.DaysInMonth(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.AddMonths(1).Month)));
            }

            if (objHorario.bUtilizaDDSRProporcional)
            {
                objHorario.LimitesDDsrProporcionais.AddRange(LimitesDDsrProporcionais);
                objHorario.LimitesDDsrProporcionais = objHorario.LimitesDDsrProporcionais.Distinct().ToList();
                foreach (var LimiteDDsrProporcional in objHorario.LimitesDDsrProporcionais)
                {
                    DateTime dtAtual = DateTime.Now.Date;
                    string[] dthora = new string[2] { "00", "00" };
                    DateTime dtVazia = new DateTime(dtAtual.Year, dtAtual.Month, dtAtual.Day, Convert.ToInt32(dthora[0]), Convert.ToInt32(dthora[1]), 0);
          
                    if ((LimiteDDsrProporcional.DTLimitePerdaDsr == dtVazia) || 
                        (LimiteDDsrProporcional.DTQtdHorasDsr == dtVazia ))
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
                formPBRecalcula.Show(this);

                this.Enabled = false;

                bllMarcacao.RecalculaMarcacao(4, objHorario.Id, datainicial, datafinal, formPBRecalcula.ObjProgressBar);
                formPBRecalcula.Close();
            }

            return ret;
        }
        #region Eventos

        #region Horário Normal

        private void chbHorasnormais_CheckedChanged(object sender, EventArgs e)
        {
            if (!chbHorasnormais.Checked)
            {
                chbMarcacargahorariamista.Checked = true;
            }
            else
            {
                chbMarcacargahorariamista.Checked = false;
                chbSomentecargahoraria.Checked = false;
                AuxCalculoDomingo();
                AuxCalculoSegunda();
                AuxCalculoTerca();
                AuxCalculoQuarta();
                AuxCalculoQuinta();
                AuxCalculoSexta();
                AuxCalculoSabado();
            }
        }

        private void chbSomentecargahoraria_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSomentecargahoraria.Checked)
            {
                chbMarcacargahorariamista.Checked = false;
                chbHorasnormais.Checked = false;
            }
        }

        private void chbMarcacargahorariamista_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMarcacargahorariamista.Checked)
            {
                chbSomentecargahoraria.Checked = false;
                chbHorasnormais.Checked = false;

                AuxCalculoDomingo();
                AuxCalculoQuarta();
                AuxCalculoQuinta();
                AuxCalculoSabado();
                AuxCalculoSegunda();
                AuxCalculoSexta();
                AuxCalculoTerca();
                //apaga os campos da carga horaria diurna
                txtTotaltrabalhadadiurna1.EditValue = "--:--";
                txtTotaltrabalhadadiurna2.EditValue = "--:--";
                txtTotaltrabalhadadiurna3.EditValue = "--:--";
                txtTotaltrabalhadadiurna4.EditValue = "--:--";
                txtTotaltrabalhadadiurna5.EditValue = "--:--";
                txtTotaltrabalhadadiurna6.EditValue = "--:--";
                txtTotaltrabalhadadiurna7.EditValue = "--:--";
                //apaga os campos da carga horaria noturna
                txtTotaltrabalhadanoturna1.EditValue = "--:--";
                txtTotaltrabalhadanoturna2.EditValue = "--:--";
                txtTotaltrabalhadanoturna3.EditValue = "--:--";
                txtTotaltrabalhadanoturna4.EditValue = "--:--";
                txtTotaltrabalhadanoturna5.EditValue = "--:--";
                txtTotaltrabalhadanoturna6.EditValue = "--:--";
                txtTotaltrabalhadanoturna7.EditValue = "--:--";
            }
            else
            {
                txtCargaMista1.EditValue = "--:--";
                txtCargaMista2.EditValue = "--:--";
                txtCargaMista3.EditValue = "--:--";
                txtCargaMista4.EditValue = "--:--";
                txtCargaMista5.EditValue = "--:--";
                txtCargaMista6.EditValue = "--:--";
                txtCargaMista7.EditValue = "--:--";
                chbHorasnormais.Checked = true;
            }
        }

        private void sbMarcaDesmarca_Click(object sender, EventArgs e)
        {
            chbDiasDomingo.Checked = bmarcar;
            chbDiasSegunda.Checked = bmarcar;
            chbDiasTerca.Checked = bmarcar;
            chbDiasQuarta.Checked = bmarcar;
            chbDiasQuinta.Checked = bmarcar;
            chbDiasSexta.Checked = bmarcar;
            chbDiasSabado.Checked = bmarcar;

            bmarcar = !bmarcar;
        }

        private void AuxCopiaHorarios(string[] entradas, string[] saidas, bool pCafe, string totalD, string totalN, Modelo.HorarioDetalhe pHorarioDetalhe, Componentes.devexpress.cwkEditHora pTrabD, Componentes.devexpress.cwkEditHora pTrabN, Componentes.devexpress.cwkEditHora pTrabM)
        {
            if (pCafe)
            {
                string TD = totalD, TN = totalN;
                BLL.Horario.CalculaCafe(entradas, saidas, chbHabilitaperiodo01.Checked, chbHabilitaperiodo02.Checked, ref TD, ref TN);
                if (chbMarcacargahorariamista.Checked)
                {
                    pHorarioDetalhe.Cargahorariamista = BLL.CalculoHoras.OperacaoHoras('+', TD, TN);
                    pTrabM.EditValue = pHorarioDetalhe.Cargahorariamista;
                }
                else
                {
                    pHorarioDetalhe.Totaltrabalhadadiurna = TD;
                    pHorarioDetalhe.Totaltrabalhadanoturna = TN;

                    pTrabD.EditValue = TD;
                    pTrabN.EditValue = TN;
                }
            }
            else
            {
                //verifica se a carga mista esta selecionada para atribuir os valores somente para ela
                if (chbMarcacargahorariamista.Checked)
                {
                    pHorarioDetalhe.Cargahorariamista = BLL.CalculoHoras.OperacaoHoras('+', totalD, totalN);
                    pTrabM.EditValue = pHorarioDetalhe.Cargahorariamista;
                }
                else
                {
                    pHorarioDetalhe.Totaltrabalhadadiurna = totalD;
                    pHorarioDetalhe.Totaltrabalhadanoturna = totalN;
                    pTrabD.EditValue = totalD;
                    pTrabN.EditValue = totalN;
                }
            }
        }

        private void sbCopiarHorariosNormal_Click(object sender, EventArgs e)
        {
            int id = (int)cbIdJornada.EditValue;
            if (id > 0)
            {
                if (cbIdjornadaSeg.Enabled)
                    cbIdjornadaSeg.EditValue = id;
                if (cbIdjornadaTer.Enabled)
                    cbIdjornadaTer.EditValue = id;
                if (cbIdjornadaQua.Enabled)
                    cbIdjornadaQua.EditValue = id;
                if (cbIdjornadaQui.Enabled)
                    cbIdjornadaQui.EditValue = id;
                if (cbIdjornadaSex.Enabled)
                    cbIdjornadaSex.EditValue = id;
                if (cbIdjornadaSab.Enabled)
                    cbIdjornadaSab.EditValue = id;
                if (cbIdjornadaDom.Enabled)
                    cbIdjornadaDom.EditValue = id;
            }
            else
            {
                MessageBox.Show("Selecione a jornada para copiar.");
            }
        }

        private void chbDiasSegunda_CheckedChanged(object sender, EventArgs e)
        {
            lblFSegunda.Visible = !chbDiasSegunda.Checked;
            cbIdjornadaSeg.Enabled = chbDiasSegunda.Checked;
            sbIdjornadaSeg.Enabled = cbIdjornadaSeg.Enabled;
            chbNeutroSegunda.Enabled = chbDiasSegunda.Checked;
            if (!chbDiasSegunda.Checked)
            {
                chbNeutroSegunda.Checked = false;
                cbIdjornadaSeg.EditValue = 0;
                txtEntrada_1Segunda.EditValue = "--:--";
                txtEntrada_2Segunda.EditValue = "--:--";
                txtEntrada_3Segunda.EditValue = "--:--";
                txtEntrada_4Segunda.EditValue = "--:--";

                txtSaida_1Segunda.EditValue = "--:--";
                txtSaida_2Segunda.EditValue = "--:--";
                txtSaida_3Segunda.EditValue = "--:--";
                txtSaida_4Segunda.EditValue = "--:--";

                txtTotaltrabalhadadiurna1.EditValue = "--:--";
                txtTotaltrabalhadanoturna1.EditValue = "--:--";
                txtCargaMista1.EditValue = "--:--";
            }
        }

        private void chbDiasTerca_CheckedChanged(object sender, EventArgs e)
        {
            lblFTerca.Visible = !chbDiasTerca.Checked;
            cbIdjornadaTer.Enabled = chbDiasTerca.Checked;
            sbIdjornadaTer.Enabled = cbIdjornadaTer.Enabled;
            chbNeutroTerca.Enabled = chbDiasTerca.Checked;
            if (!chbDiasTerca.Checked)
            {
                chbNeutroTerca.Checked = false;
                cbIdjornadaTer.EditValue = 0;
                txtEntrada_1Terca.EditValue = "--:--";
                txtEntrada_2Terca.EditValue = "--:--";
                txtEntrada_3Terca.EditValue = "--:--";
                txtEntrada_4Terca.EditValue = "--:--";

                txtSaida_1Terca.EditValue = "--:--";
                txtSaida_2Terca.EditValue = "--:--";
                txtSaida_3Terca.EditValue = "--:--";
                txtSaida_4Terca.EditValue = "--:--";

                txtTotaltrabalhadadiurna2.EditValue = "--:--";
                txtTotaltrabalhadanoturna2.EditValue = "--:--";
                txtCargaMista2.EditValue = "--:--";
            }
        }

        private void chbDiasQuarta_CheckedChanged(object sender, EventArgs e)
        {
            lblFQuarta.Visible = !chbDiasQuarta.Checked;
            cbIdjornadaQua.Enabled = chbDiasQuarta.Checked;
            sbIdjornadaQua.Enabled = cbIdjornadaQua.Enabled;
            chbNeutroQuarta.Enabled = chbDiasQuarta.Checked;
            if (!chbDiasQuarta.Checked)
            {
                chbNeutroQuarta.Checked = false;
                cbIdjornadaQua.EditValue = 0;
                txtEntrada_1Quarta.EditValue = "--:--";
                txtEntrada_2Quarta.EditValue = "--:--";
                txtEntrada_3Quarta.EditValue = "--:--";
                txtEntrada_4Quarta.EditValue = "--:--";

                txtSaida_1Quarta.EditValue = "--:--";
                txtSaida_2Quarta.EditValue = "--:--";
                txtSaida_3Quarta.EditValue = "--:--";
                txtSaida_4Quarta.EditValue = "--:--";

                txtTotaltrabalhadadiurna3.EditValue = "--:--";
                txtTotaltrabalhadanoturna3.EditValue = "--:--";
                txtCargaMista3.EditValue = "--:--";
            }
        }

        private void chbDiasQuinta_CheckedChanged(object sender, EventArgs e)
        {
            lblFQuinta.Visible = !chbDiasQuinta.Checked;
            cbIdjornadaQui.Enabled = chbDiasQuinta.Checked;
            sbIdjornadaQui.Enabled = cbIdjornadaQui.Enabled;
            chbNeutroQuinta.Enabled = chbDiasQuinta.Checked;
            if (!chbDiasQuinta.Checked)
            {
                chbNeutroQuinta.Checked = false;
                cbIdjornadaQui.EditValue = 0;
                txtEntrada_1Quinta.EditValue = "--:--";
                txtEntrada_2Quinta.EditValue = "--:--";
                txtEntrada_3Quinta.EditValue = "--:--";
                txtEntrada_4Quinta.EditValue = "--:--";

                txtSaida_1Quinta.EditValue = "--:--";
                txtSaida_2Quinta.EditValue = "--:--";
                txtSaida_3Quinta.EditValue = "--:--";
                txtSaida_4Quinta.EditValue = "--:--";

                txtTotaltrabalhadadiurna4.EditValue = "--:--";
                txtTotaltrabalhadanoturna4.EditValue = "--:--";
                txtCargaMista4.EditValue = "--:--";
            }
        }

        private void chbDiasSexta_CheckedChanged(object sender, EventArgs e)
        {
            lblFSexta.Visible = !chbDiasSexta.Checked;
            cbIdjornadaSex.Enabled = chbDiasSexta.Checked;
            sbIdjornadaSex.Enabled = cbIdjornadaSex.Enabled;
            chbNeutroSexta.Enabled = chbDiasSexta.Checked;
            if (!chbDiasSexta.Checked)
            {
                chbNeutroSexta.Checked = false;
                cbIdjornadaSex.EditValue = 0;
                txtEntrada_1Sexta.EditValue = "--:--";
                txtEntrada_2Sexta.EditValue = "--:--";
                txtEntrada_3Sexta.EditValue = "--:--";
                txtEntrada_4Sexta.EditValue = "--:--";

                txtSaida_1Sexta.EditValue = "--:--";
                txtSaida_2Sexta.EditValue = "--:--";
                txtSaida_3Sexta.EditValue = "--:--";
                txtSaida_4Sexta.EditValue = "--:--";

                txtTotaltrabalhadadiurna5.EditValue = "--:--";
                txtTotaltrabalhadanoturna5.EditValue = "--:--";
                txtCargaMista5.EditValue = "--:--";
            }
        }

        private void chbDiasSabado_CheckedChanged(object sender, EventArgs e)
        {
            lblFSabado.Visible = !chbDiasSabado.Checked;
            cbIdjornadaSab.Enabled = chbDiasSabado.Checked;
            sbIdjornadaSab.Enabled = cbIdjornadaSab.Enabled;
            chbNeutroSabado.Enabled = chbDiasSabado.Checked;
            if (!chbDiasSabado.Checked)
            {
                chbNeutroSabado.Checked = false;
                cbIdjornadaSab.EditValue = 0;
                txtEntrada_1Sabado.EditValue = "--:--";
                txtEntrada_2Sabado.EditValue = "--:--";
                txtEntrada_3Sabado.EditValue = "--:--";
                txtEntrada_4Sabado.EditValue = "--:--";

                txtSaida_1Sabado.EditValue = "--:--";
                txtSaida_2Sabado.EditValue = "--:--";
                txtSaida_3Sabado.EditValue = "--:--";
                txtSaida_4Sabado.EditValue = "--:--";

                txtTotaltrabalhadadiurna6.EditValue = "--:--";
                txtTotaltrabalhadanoturna6.EditValue = "--:--";
                txtCargaMista6.EditValue = "--:--";
            }
        }

        private void chbDiasDomingo_CheckedChanged(object sender, EventArgs e)
        {
            lblFDomingo.Visible = !chbDiasDomingo.Checked;
            cbIdjornadaDom.Enabled = chbDiasDomingo.Checked;
            sbIdjornadaDom.Enabled = cbIdjornadaDom.Enabled;
            chbNeutroDomingo.Enabled = chbDiasDomingo.Checked;
            if (!chbDiasDomingo.Checked)
            {
                chbNeutroDomingo.Checked = false;
                cbIdjornadaDom.EditValue = 0;
                txtEntrada_1Domingo.EditValue = "--:--";
                txtEntrada_2Domingo.EditValue = "--:--";
                txtEntrada_3Domingo.EditValue = "--:--";
                txtEntrada_4Domingo.EditValue = "--:--";

                txtSaida_1Domingo.EditValue = "--:--";
                txtSaida_2Domingo.EditValue = "--:--";
                txtSaida_3Domingo.EditValue = "--:--";
                txtSaida_4Domingo.EditValue = "--:--";

                txtTotaltrabalhadadiurna7.EditValue = "--:--";
                txtTotaltrabalhadanoturna7.EditValue = "--:--";
                txtCargaMista7.EditValue = "--:--";
            }
        }

        #endregion

        #region DSR

        private void chbDescontardsr_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDescontardsr.Checked == true)
                chbDdrsProporcional.Enabled = true;
            else
                chbDdrsProporcional.Checked = chbDdrsProporcional.Enabled = false;
            

            txtQtdhorasdsr.Enabled = txtLimiteperdadsr.Enabled = rgDiasemanadsr.Enabled = chbDescontardsr.Checked;

            if (!chbDescontardsr.Checked)
            {
                txtQtdhorasdsr.EditValue = txtLimiteperdadsr.EditValue = null;
                rgDiasemanadsr.SelectedIndex = -1;
            }
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
                    //txtQuantidadeextra50.EditValue = "---:--";
                }
            }

            //if (chbMarcapercentualextra50.Checked)
            //{
            //    IncluiHorarioPHExtra(txtPercentualextra50);
            //}           
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
                    //txtQuantidadeextra60.EditValue = "---:--";
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
                    //txtQuantidadeextra70.EditValue = "---:--";
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
                    //txtQuantidadeextra80.EditValue = "---:--";
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
                    //txtQuantidadeextra90.EditValue = "---:--";
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
                    //txtQuantidadeextra100.EditValue = "---:--";
                }
            }

        }

        private void chbSabado_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraSabado.Enabled = chbMarcapercentualextraSabado.Checked;
            txtQuantidadeextraSabado.Enabled = chbMarcapercentualextraSabado.Checked;
            cbPercentualExtraSab.Enabled = chbMarcapercentualextraSabado.Checked;
            cbTipoAcumuloSab.Enabled = chbMarcapercentualextraSabado.Checked;
            cbPercentualExtraSab.SelectedIndex = -1;
            cbPercentualExtraSab.EditValue = 0;
            cbTipoAcumuloSab.EditValue = null;
            if (chbMarcapercentualextraSabado.Checked)
                cbTipoAcumuloSab.SelectedIndex = 0;


            if (carregado)
            {
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
                    //txtQuantidadeextraSabado.EditValue = "---:--";
                }
            }
        }

        private void chbDomingo_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraDomingo.Enabled = chbMarcapercentualextraDomingo.Checked;
            txtQuantidadeextraDomingo.Enabled = chbMarcapercentualextraDomingo.Checked;
            cbPercentualExtraDom.Enabled = chbMarcapercentualextraDomingo.Checked;
            cbTipoAcumuloDom.Enabled = chbMarcapercentualextraDomingo.Checked;
            cbPercentualExtraDom.SelectedIndex = -1;
            cbPercentualExtraDom.EditValue = 0;
            cbTipoAcumuloDom.EditValue = null;

            if (chbMarcapercentualextraDomingo.Checked)
                cbTipoAcumuloDom.SelectedIndex = 0;

            if (carregado)
            {
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
                    //txtQuantidadeextraDomingo.EditValue = "---:--";
                }
            }
        }

        private void chbFeriado_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraFeriado.Enabled = chbMarcapercentualextraFeriado.Checked;
            txtQuantidadeextraFeriado.Enabled = chbMarcapercentualextraFeriado.Checked;
            cbPercentualExtraFer.Enabled = chbMarcapercentualextraFeriado.Checked;
            cbTipoAcumuloFer.Enabled = chbMarcapercentualextraFeriado.Checked;
            cbPercentualExtraFer.SelectedIndex = -1;
            cbPercentualExtraFer.EditValue = 0;
            cbTipoAcumuloFer.EditValue = null;
            if (chbMarcapercentualextraFeriado.Checked)
                cbTipoAcumuloFer.SelectedIndex = 0;

            if (carregado)
            {
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
                    //txtQuantidadeextraFeriado.EditValue = "---:--";
                }
            }
        }

        private void chbFolga_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentualextraFolga.Enabled = chbMarcapercentualextraFolga.Checked;
            txtQuantidadeextraFolga.Enabled = chbMarcapercentualextraFolga.Checked;
            cbPercentualExtraFol.Enabled = chbMarcapercentualextraFolga.Checked;
            cbTipoAcumuloFol.Enabled = chbMarcapercentualextraFolga.Checked;
            cbPercentualExtraFol.SelectedIndex = -1;
            cbPercentualExtraFol.EditValue = 0;
            cbTipoAcumuloFol.EditValue = null;
            if (chbMarcapercentualextraFolga.Checked)
                cbTipoAcumuloFol.SelectedIndex = 0;

            if (carregado)
            {
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
        }

        #endregion

        #region Carga Mista
        private void txtCargaMista1_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }

        private void txtCargaMista2_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }

        private void txtCargaMista3_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }

        private void txtCargaMista4_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }

        private void txtCargaMista5_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }

        private void txtCargaMista6_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }

        private void txtCargaMista7_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaMista();
        }
        #endregion

        #region Intervalo Automatico
        private void chbIntervaloAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaIntervaloAutomatico();
        }

        private void HabilitaIntervaloAutomatico()
        {
            bool hab = chbIntervaloAutomatico.Checked;

            HabilitaIASegunda(hab);
            HabilitaIATerca(hab);
            HabilitaIAQuarta(hab);
            HabilitaIAQuinta(hab);
            HabilitaIASexta(hab);
            HabilitaIASabado(hab);
            HabilitaIADomingo(hab);
        }

        private void HabilitaIADomingo(bool hab)
        {
            chbPADom1.Enabled = hab && (string)txtSaida_1Domingo.EditValue != "--:--" && (string)txtEntrada_2Domingo.EditValue != "--:--";
            chbPADom2.Enabled = hab && (string)txtSaida_2Domingo.EditValue != "--:--" && (string)txtEntrada_3Domingo.EditValue != "--:--";
            chbPADom3.Enabled = hab && (string)txtSaida_3Domingo.EditValue != "--:--" && (string)txtEntrada_4Domingo.EditValue != "--:--";
            chbPADom1.Checked = chbPADom1.Enabled ? chbPADom1.Checked : chbPADom1.Enabled;
            chbPADom2.Checked = chbPADom2.Enabled ? chbPADom2.Checked : chbPADom2.Enabled;
            chbPADom3.Checked = chbPADom3.Enabled ? chbPADom3.Checked : chbPADom3.Enabled;
        }

        private void HabilitaIASabado(bool hab)
        {
            chbPASab1.Enabled = hab && (string)txtSaida_1Sabado.EditValue != "--:--" && (string)txtEntrada_2Sabado.EditValue != "--:--";
            chbPASab2.Enabled = hab && (string)txtSaida_2Sabado.EditValue != "--:--" && (string)txtEntrada_3Sabado.EditValue != "--:--";
            chbPASab3.Enabled = hab && (string)txtSaida_3Sabado.EditValue != "--:--" && (string)txtEntrada_4Sabado.EditValue != "--:--";
            chbPASab1.Checked = chbPASab1.Enabled ? chbPASab1.Checked : chbPASab1.Enabled;
            chbPASab2.Checked = chbPASab2.Enabled ? chbPASab2.Checked : chbPASab2.Enabled;
            chbPASab3.Checked = chbPASab3.Enabled ? chbPASab3.Checked : chbPASab3.Enabled;
        }

        private void HabilitaIASexta(bool hab)
        {
            chbPASex1.Enabled = hab && (string)txtSaida_1Sexta.EditValue != "--:--" && (string)txtEntrada_2Sexta.EditValue != "--:--";
            chbPASex2.Enabled = hab && (string)txtSaida_2Sexta.EditValue != "--:--" && (string)txtEntrada_3Sexta.EditValue != "--:--";
            chbPASex3.Enabled = hab && (string)txtSaida_3Sexta.EditValue != "--:--" && (string)txtEntrada_4Sexta.EditValue != "--:--";
            chbPASex1.Checked = chbPASex1.Enabled ? chbPASex1.Checked : chbPASex1.Enabled;
            chbPASex2.Checked = chbPASex2.Enabled ? chbPASex2.Checked : chbPASex2.Enabled;
            chbPASex3.Checked = chbPASex3.Enabled ? chbPASex3.Checked : chbPASex3.Enabled;
        }

        private void HabilitaIAQuinta(bool hab)
        {
            chbPAQui1.Enabled = hab && (string)txtSaida_1Quinta.EditValue != "--:--" && (string)txtEntrada_2Quinta.EditValue != "--:--";
            chbPAQui2.Enabled = hab && (string)txtSaida_2Quinta.EditValue != "--:--" && (string)txtEntrada_3Quinta.EditValue != "--:--";
            chbPAQui3.Enabled = hab && (string)txtSaida_3Quinta.EditValue != "--:--" && (string)txtEntrada_4Quinta.EditValue != "--:--";
            chbPAQui1.Checked = chbPAQui1.Enabled ? chbPAQui1.Checked : chbPAQui1.Enabled;
            chbPAQui2.Checked = chbPAQui2.Enabled ? chbPAQui2.Checked : chbPAQui2.Enabled;
            chbPAQui3.Checked = chbPAQui3.Enabled ? chbPAQui3.Checked : chbPAQui3.Enabled;
        }

        private void HabilitaIAQuarta(bool hab)
        {
            chbPAQua1.Enabled = hab && (string)txtSaida_1Quarta.EditValue != "--:--" && (string)txtEntrada_2Quarta.EditValue != "--:--";
            chbPAQua2.Enabled = hab && (string)txtSaida_2Quarta.EditValue != "--:--" && (string)txtEntrada_3Quarta.EditValue != "--:--";
            chbPAQua3.Enabled = hab && (string)txtSaida_3Quarta.EditValue != "--:--" && (string)txtEntrada_4Quarta.EditValue != "--:--";
            chbPAQua1.Checked = chbPAQua1.Enabled ? chbPAQua1.Checked : chbPAQua1.Enabled;
            chbPAQua2.Checked = chbPAQua2.Enabled ? chbPAQua2.Checked : chbPAQua2.Enabled;
            chbPAQua3.Checked = chbPAQua3.Enabled ? chbPAQua3.Checked : chbPAQua3.Enabled;
        }

        private void HabilitaIATerca(bool hab)
        {
            chbPATer1.Enabled = hab && (string)txtSaida_1Terca.EditValue != "--:--" && (string)txtEntrada_2Terca.EditValue != "--:--";
            chbPATer2.Enabled = hab && (string)txtSaida_2Terca.EditValue != "--:--" && (string)txtEntrada_3Terca.EditValue != "--:--";
            chbPATer3.Enabled = hab && (string)txtSaida_3Terca.EditValue != "--:--" && (string)txtEntrada_4Terca.EditValue != "--:--";
            chbPATer1.Checked = chbPATer1.Enabled ? chbPATer1.Checked : chbPATer1.Enabled;
            chbPATer2.Checked = chbPATer2.Enabled ? chbPATer2.Checked : chbPATer2.Enabled;
            chbPATer3.Checked = chbPATer3.Enabled ? chbPATer3.Checked : chbPATer3.Enabled;
        }

        private void HabilitaIASegunda(bool hab)
        {
            chbPASeg1.Enabled = hab && (string)txtSaida_1Segunda.EditValue != "--:--" && (string)txtEntrada_2Segunda.EditValue != "--:--";
            chbPASeg2.Enabled = hab && (string)txtSaida_2Segunda.EditValue != "--:--" && (string)txtEntrada_3Segunda.EditValue != "--:--";
            chbPASeg3.Enabled = hab && (string)txtSaida_3Segunda.EditValue != "--:--" && (string)txtEntrada_4Segunda.EditValue != "--:--";
            chbPASeg1.Checked = chbPASeg1.Enabled ? chbPASeg1.Checked : chbPASeg1.Enabled;
            chbPASeg2.Checked = chbPASeg2.Enabled ? chbPASeg2.Checked : chbPASeg2.Enabled;
            chbPASeg3.Checked = chbPASeg3.Enabled ? chbPASeg3.Checked : chbPASeg3.Enabled;
        }
        #endregion

        #region Dias Café
        private void chbDias_cafe_1_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoSegunda();
        }

        private void chbDias_cafe_2_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoTerca();
        }

        private void chbDias_cafe_3_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoQuarta();
        }

        private void chbDias_cafe_4_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoQuinta();
        }

        private void chbDias_cafe_5_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoSexta();
        }

        private void chbDias_cafe_6_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoSabado();
        }

        private void chbDias_cafe_7_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoDomingo();
        }
        #endregion

        #region Outros Eventos

        private void FormManutHorario_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    sbCopiarHorariosNormal_Click(sender, e);
                    break;
            }
        }

        private void sbIdparametro_Click(object sender, EventArgs e)
        {
            FormGridParametros form = new FormGridParametros();
            form.cwTabela = "Parâmetros";
            form.cwId = (int)cbIdparametro.EditValue;
            GridSelecao(form, cbIdparametro, bllParametro);
        }

        private void chbHabilitaperiodo01_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoSegunda();
            AuxCalculoTerca();
            AuxCalculoQuarta();
            AuxCalculoQuinta();
            AuxCalculoSexta();
            AuxCalculoSabado();
            AuxCalculoDomingo();
            if (chbHabilitaperiodo01.Checked && chbDescontacafemanha.Checked)
            {
                chbDescontacafemanha.Checked = false;
            }

        }

        private void chbHabilitaperiodo02_CheckedChanged(object sender, EventArgs e)
        {
            AuxCalculoSegunda();
            AuxCalculoTerca();
            AuxCalculoQuarta();
            AuxCalculoQuinta();
            AuxCalculoSexta();
            AuxCalculoSabado();
            AuxCalculoDomingo();
            if (chbDescontacafetarde.Checked && chbHabilitaperiodo02.Checked)
            {
                chbDescontacafetarde.Checked = false;
            }
        }

        private void chbDescontacafemanha_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDescontacafemanha.Checked && chbHabilitaperiodo01.Checked)
            {
                chbHabilitaperiodo01.Checked = false;
            }
        }

        private void chbDescontacafetarde_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDescontacafetarde.Checked && chbHabilitaperiodo02.Checked)
            {
                chbHabilitaperiodo02.Checked = false;
            }
        }

        private void chbConsiderasabadosemana_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConsiderasabadosemana.Checked)
            {
                chbMarcapercentualextraSabado.Checked = false;
            }
            chbMarcapercentualextraSabado.Enabled = !chbConsiderasabadosemana.Checked;
        }

        private void chbConsideradomingosemana_CheckedChanged(object sender, EventArgs e)
        {

            if (chbConsideradomingosemana.Checked)
            {
                chbConsiderasabadosemana.Checked = true;
                chbMarcapercentualextraDomingo.Checked = false;
            }
            chbConsiderasabadosemana.Enabled = !chbConsideradomingosemana.Checked;
            chbMarcapercentualextraDomingo.Enabled = !chbConsideradomingosemana.Checked;
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
                chbConsideraadhtrabalhadas.Enabled = false;
                chbConversaohoranoturna.Checked = false;
            }
        }

        #endregion

        #endregion

        #region Métodos Auxiliares

        private void LoadHorariosDetalhe()
        {
            if (objHorario.HorariosDetalhe[0] != null)
            {
                txtEntrada_1Segunda.EditValue = objHorario.HorariosDetalhe[0].Entrada_1;
            }
        }

        private void CalculaHoras(int index, string[] pEntrada, string[] pSaida, Componentes.devexpress.cwkEditHora pDiurna, Componentes.devexpress.cwkEditHora pNoturna, Componentes.devexpress.cwkEditHora pMista, bool pCalculaMista, bool pCafe)
        {
            if (objHorario.Idparametro != 0)
            {
                string totalD;
                string totalN;
                AuxCalculaHoras(pEntrada, pSaida, out totalD, out totalN);

                if (pCafe)
                {
                    BLL.Horario.CalculaCafe(pEntrada, pSaida, chbHabilitaperiodo01.Checked, chbHabilitaperiodo02.Checked, ref totalD, ref totalN);
                }

                if (pCalculaMista)
                {
                    pMista.EditValue = BLL.CalculoHoras.OperacaoHoras('+', totalD, totalN);
                }
                else
                {
                    pDiurna.EditValue = (totalD != "00:00" ? totalD : "--:--");
                    pNoturna.EditValue = (totalN != "00:00" ? totalN : "--:--");
                    pMista.EditValue = "--:--";
                }
            }
        }

        private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN)
        {
            Modelo.Parametros objParametros = bllParametro.LoadObject(objHorario.Idparametro);
            InicioHNoturna = objParametros.InicioAdNoturno;
            FimHNotura = objParametros.FimAdNoturno;

            totalD = "";
            totalN = "";

            BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        }

        #endregion

        #region Jornada
        private bool CarregarJornada(int id)
        {
            bool carregar = false;
            if (objJornada == null)
                carregar = true;
            else if (objJornada.Id != id)
                carregar = true;
            return carregar;
        }

        private void AtribuiHorarioJornada(int id, int index)
        {
            if (CarregarJornada(id))
                objJornada = bllJornada.LoadObject(id);
            objHorario.HorariosDetalhe[index].Entrada_1 = objJornada.Entrada_1;
            objHorario.HorariosDetalhe[index].Entrada_2 = objJornada.Entrada_2;
            objHorario.HorariosDetalhe[index].Entrada_3 = objJornada.Entrada_3;
            objHorario.HorariosDetalhe[index].Entrada_4 = objJornada.Entrada_4;
            objHorario.HorariosDetalhe[index].Saida_1 = objJornada.Saida_1;
            objHorario.HorariosDetalhe[index].Saida_2 = objJornada.Saida_2;
            objHorario.HorariosDetalhe[index].Saida_3 = objJornada.Saida_3;
            objHorario.HorariosDetalhe[index].Saida_4 = objJornada.Saida_4;
        }

        private void cbIdjornadaSeg_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaSeg.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Segunda.EditValue = objJornada.Entrada_1;
                txtEntrada_2Segunda.EditValue = objJornada.Entrada_2;
                txtEntrada_3Segunda.EditValue = objJornada.Entrada_3;
                txtEntrada_4Segunda.EditValue = objJornada.Entrada_4;
                txtSaida_1Segunda.EditValue = objJornada.Saida_1;
                txtSaida_2Segunda.EditValue = objJornada.Saida_2;
                txtSaida_3Segunda.EditValue = objJornada.Saida_3;
                txtSaida_4Segunda.EditValue = objJornada.Saida_4;

                CalculoSegunda(txtEntrada_1Segunda, txtSaida_1Segunda);
                HabilitaIASegunda(chbIntervaloAutomatico.Checked);
            }
        }

        private void cbIdjornadaTer_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaTer.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Terca.EditValue = objJornada.Entrada_1;
                txtEntrada_2Terca.EditValue = objJornada.Entrada_2;
                txtEntrada_3Terca.EditValue = objJornada.Entrada_3;
                txtEntrada_4Terca.EditValue = objJornada.Entrada_4;
                txtSaida_1Terca.EditValue = objJornada.Saida_1;
                txtSaida_2Terca.EditValue = objJornada.Saida_2;
                txtSaida_3Terca.EditValue = objJornada.Saida_3;
                txtSaida_4Terca.EditValue = objJornada.Saida_4;

                CalculoTerca(txtEntrada_1Terca, txtSaida_1Terca);
                HabilitaIATerca(chbIntervaloAutomatico.Checked);
            }
        }

        private void cbIdjornadaQua_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaQua.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Quarta.EditValue = objJornada.Entrada_1;
                txtEntrada_2Quarta.EditValue = objJornada.Entrada_2;
                txtEntrada_3Quarta.EditValue = objJornada.Entrada_3;
                txtEntrada_4Quarta.EditValue = objJornada.Entrada_4;
                txtSaida_1Quarta.EditValue = objJornada.Saida_1;
                txtSaida_2Quarta.EditValue = objJornada.Saida_2;
                txtSaida_3Quarta.EditValue = objJornada.Saida_3;
                txtSaida_4Quarta.EditValue = objJornada.Saida_4;

                CalculoQuarta(txtEntrada_1Quarta, txtSaida_1Quarta);
                HabilitaIAQuarta(chbIntervaloAutomatico.Checked);
            }
        }

        private void cbIdjornadaQui_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaQui.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Quinta.EditValue = objJornada.Entrada_1;
                txtEntrada_2Quinta.EditValue = objJornada.Entrada_2;
                txtEntrada_3Quinta.EditValue = objJornada.Entrada_3;
                txtEntrada_4Quinta.EditValue = objJornada.Entrada_4;
                txtSaida_1Quinta.EditValue = objJornada.Saida_1;
                txtSaida_2Quinta.EditValue = objJornada.Saida_2;
                txtSaida_3Quinta.EditValue = objJornada.Saida_3;
                txtSaida_4Quinta.EditValue = objJornada.Saida_4;

                CalculoQuinta(txtEntrada_1Quinta, txtSaida_1Quinta);
                HabilitaIAQuinta(chbIntervaloAutomatico.Checked);
            }
        }

        private void cbIdjornadaSex_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaSex.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Sexta.EditValue = objJornada.Entrada_1;
                txtEntrada_2Sexta.EditValue = objJornada.Entrada_2;
                txtEntrada_3Sexta.EditValue = objJornada.Entrada_3;
                txtEntrada_4Sexta.EditValue = objJornada.Entrada_4;
                txtSaida_1Sexta.EditValue = objJornada.Saida_1;
                txtSaida_2Sexta.EditValue = objJornada.Saida_2;
                txtSaida_3Sexta.EditValue = objJornada.Saida_3;
                txtSaida_4Sexta.EditValue = objJornada.Saida_4;

                CalculoSexta(txtEntrada_1Sexta, txtSaida_1Sexta);
                HabilitaIASexta(chbIntervaloAutomatico.Checked);
            }
        }

        private void cbIdjornadaSab_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaSab.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Sabado.EditValue = objJornada.Entrada_1;
                txtEntrada_2Sabado.EditValue = objJornada.Entrada_2;
                txtEntrada_3Sabado.EditValue = objJornada.Entrada_3;
                txtEntrada_4Sabado.EditValue = objJornada.Entrada_4;
                txtSaida_1Sabado.EditValue = objJornada.Saida_1;
                txtSaida_2Sabado.EditValue = objJornada.Saida_2;
                txtSaida_3Sabado.EditValue = objJornada.Saida_3;
                txtSaida_4Sabado.EditValue = objJornada.Saida_4;

                CalculoSabado(txtEntrada_1Sabado, txtSaida_1Sabado);
                HabilitaIASabado(chbIntervaloAutomatico.Checked);
            }
        }

        private void cbIdjornadaDom_EditValueChanged(object sender, EventArgs e)
        {
            int id = (int)cbIdjornadaDom.EditValue;
            if (id > 0)
            {
                if (CarregarJornada(id))
                    objJornada = bllJornada.LoadObject(id);

                txtEntrada_1Domingo.EditValue = objJornada.Entrada_1;
                txtEntrada_2Domingo.EditValue = objJornada.Entrada_2;
                txtEntrada_3Domingo.EditValue = objJornada.Entrada_3;
                txtEntrada_4Domingo.EditValue = objJornada.Entrada_4;
                txtSaida_1Domingo.EditValue = objJornada.Saida_1;
                txtSaida_2Domingo.EditValue = objJornada.Saida_2;
                txtSaida_3Domingo.EditValue = objJornada.Saida_3;
                txtSaida_4Domingo.EditValue = objJornada.Saida_4;

                CalculoDomingo(txtEntrada_1Domingo, txtSaida_1Domingo);
                HabilitaIADomingo(chbIntervaloAutomatico.Checked);
            }
        }
        #endregion

        #region Carga Horaria
        #region Horarios Segunda

        private void CalculoSegunda(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoSegunda();
            }
        }

        private void AuxCalculoSegunda()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Segunda.EditValue, (string)txtEntrada_2Segunda.EditValue, (string)txtEntrada_3Segunda.EditValue, (string)txtEntrada_4Segunda.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Segunda.EditValue, (string)txtSaida_2Segunda.EditValue, (string)txtSaida_3Segunda.EditValue, (string)txtSaida_4Segunda.EditValue };

            CalculaHoras(0, entradas, saidas, txtTotaltrabalhadadiurna1, txtTotaltrabalhadanoturna1, txtCargaMista1, chbMarcacargahorariamista.Checked, chbDias_cafe_1.Checked);
        }

        #endregion

        #region Horarios Terca

        private void CalculoTerca(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoTerca();
            }
        }

        private void AuxCalculoTerca()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Terca.EditValue, (string)txtEntrada_2Terca.EditValue, (string)txtEntrada_3Terca.EditValue, (string)txtEntrada_4Terca.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Terca.EditValue, (string)txtSaida_2Terca.EditValue, (string)txtSaida_3Terca.EditValue, (string)txtSaida_4Terca.EditValue };

            CalculaHoras(1, entradas, saidas, txtTotaltrabalhadadiurna2, txtTotaltrabalhadanoturna2, txtCargaMista2, chbMarcacargahorariamista.Checked, chbDias_cafe_2.Checked);
        }

        #endregion

        #region Horarios Quarta

        private void CalculoQuarta(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoQuarta();
            }
        }

        private void AuxCalculoQuarta()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Quarta.EditValue, (string)txtEntrada_2Quarta.EditValue, (string)txtEntrada_3Quarta.EditValue, (string)txtEntrada_4Quarta.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Quarta.EditValue, (string)txtSaida_2Quarta.EditValue, (string)txtSaida_3Quarta.EditValue, (string)txtSaida_4Quarta.EditValue };
            CalculaHoras(2, entradas, saidas, txtTotaltrabalhadadiurna3, txtTotaltrabalhadanoturna3, txtCargaMista3, chbMarcacargahorariamista.Checked, chbDias_cafe_3.Checked);
        }

        #endregion

        #region Horarios Quinta

        private void CalculoQuinta(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoQuinta();
            }
        }

        private void AuxCalculoQuinta()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Quinta.EditValue, (string)txtEntrada_2Quinta.EditValue, (string)txtEntrada_3Quinta.EditValue, (string)txtEntrada_4Quinta.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Quinta.EditValue, (string)txtSaida_2Quinta.EditValue, (string)txtSaida_3Quinta.EditValue, (string)txtSaida_4Quinta.EditValue };
            CalculaHoras(3, entradas, saidas, txtTotaltrabalhadadiurna4, txtTotaltrabalhadanoturna4, txtCargaMista4, chbMarcacargahorariamista.Checked, chbDias_cafe_4.Checked);
        }

        #endregion

        #region Horarios Sexta

        private void CalculoSexta(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoSexta();
            }
        }

        private void AuxCalculoSexta()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Sexta.EditValue, (string)txtEntrada_2Sexta.EditValue, (string)txtEntrada_3Sexta.EditValue, (string)txtEntrada_4Sexta.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Sexta.EditValue, (string)txtSaida_2Sexta.EditValue, (string)txtSaida_3Sexta.EditValue, (string)txtSaida_4Sexta.EditValue };
            CalculaHoras(4, entradas, saidas, txtTotaltrabalhadadiurna5, txtTotaltrabalhadanoturna5, txtCargaMista5, chbMarcacargahorariamista.Checked, chbDias_cafe_5.Checked);
        }

        #endregion

        #region Horarios Sabado

        private void CalculoSabado(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoSabado();
            }
        }

        private void AuxCalculoSabado()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Sabado.EditValue, (string)txtEntrada_2Sabado.EditValue, (string)txtEntrada_3Sabado.EditValue, (string)txtEntrada_4Sabado.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Sabado.EditValue, (string)txtSaida_2Sabado.EditValue, (string)txtSaida_3Sabado.EditValue, (string)txtSaida_4Sabado.EditValue };
            CalculaHoras(5, entradas, saidas, txtTotaltrabalhadadiurna6, txtTotaltrabalhadanoturna6, txtCargaMista6, chbMarcacargahorariamista.Checked, chbDias_cafe_6.Checked);
        }

        #endregion

        #region Horarios Domingo

        private void CalculoDomingo(Componentes.devexpress.cwkEditHora pEntrada, Componentes.devexpress.cwkEditHora pSaida)
        {
            if (!String.IsNullOrEmpty((string)pEntrada.EditValue) && !String.IsNullOrEmpty((string)pSaida.EditValue))
            {
                AuxCalculoDomingo();
            }
        }

        private void AuxCalculoDomingo()
        {
            string[] entradas = new string[] { (string)txtEntrada_1Domingo.EditValue, (string)txtEntrada_2Domingo.EditValue, (string)txtEntrada_3Domingo.EditValue, (string)txtEntrada_4Domingo.EditValue };
            string[] saidas = new string[] { (string)txtSaida_1Domingo.EditValue, (string)txtSaida_2Domingo.EditValue, (string)txtSaida_3Domingo.EditValue, (string)txtSaida_4Domingo.EditValue };
            CalculaHoras(6, entradas, saidas, txtTotaltrabalhadadiurna7, txtTotaltrabalhadanoturna7, txtCargaMista7, chbMarcacargahorariamista.Checked, chbDias_cafe_7.Checked);
        }

        #endregion

        private void SomaCargaHorariaDiurnaSemana()
        {
            int tot = 0;

            if ((string)txtTotaltrabalhadadiurna1.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna1.EditValue);
            }

            if ((string)txtTotaltrabalhadadiurna2.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna2.EditValue);
            }

            if ((string)txtTotaltrabalhadadiurna3.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna3.EditValue);
            }

            if ((string)txtTotaltrabalhadadiurna4.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna4.EditValue);
            }

            if ((string)txtTotaltrabalhadadiurna5.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna5.EditValue);
            }

            if ((string)txtTotaltrabalhadadiurna6.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna6.EditValue);
            }

            if ((string)txtTotaltrabalhadadiurna7.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadadiurna7.EditValue);
            }

            txtTotalTrabalhadaDSemana.EditValue = Modelo.cwkFuncoes.ConvertMinutosHora(3, tot);
        }

        private void SomaCargaHorariaMista()
        {
            int tot = 0;

            if ((string)txtCargaMista1.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista1.EditValue);
            }

            if ((string)txtCargaMista2.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista2.EditValue);
            }

            if ((string)txtCargaMista3.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista3.EditValue);
            }

            if ((string)txtCargaMista4.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista4.EditValue);
            }

            if ((string)txtCargaMista5.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista5.EditValue);
            }

            if ((string)txtCargaMista6.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista6.EditValue);
            }

            if ((string)txtCargaMista7.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtCargaMista7.EditValue);
            }


            txtTotalMista.EditValue = Modelo.cwkFuncoes.ConvertMinutosHora(3, tot);
        }

        private void SomaCargaHorariaNoturnaSemana()
        {
            int tot = 0;

            if ((string)txtTotaltrabalhadanoturna1.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna1.EditValue);
            }

            if ((string)txtTotaltrabalhadanoturna2.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna2.EditValue);
            }

            if ((string)txtTotaltrabalhadanoturna3.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna3.EditValue);
            }

            if ((string)txtTotaltrabalhadanoturna4.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna4.EditValue);
            }

            if ((string)txtTotaltrabalhadanoturna5.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna5.EditValue);
            }

            if ((string)txtTotaltrabalhadanoturna6.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna6.EditValue);
            }

            if ((string)txtTotaltrabalhadanoturna7.EditValue != "--:--")
            {
                tot += Modelo.cwkFuncoes.ConvertHorasMinuto((string)txtTotaltrabalhadanoturna7.EditValue);
            }

            txtTotalTrabalhadaNSemana.EditValue = Modelo.cwkFuncoes.ConvertMinutosHora(3, tot);
        }

        private void txtTotaltrabalhadadiurna1_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadadiurna2_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadadiurna3_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadadiurna4_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadadiurna5_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadadiurna6_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadadiurna7_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaDiurnaSemana();
        }

        private void txtTotaltrabalhadanoturna1_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        private void txtTotaltrabalhadanoturna2_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        private void txtTotaltrabalhadanoturna3_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        private void txtTotaltrabalhadanoturna4_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        private void txtTotaltrabalhadanoturna5_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        private void txtTotaltrabalhadanoturna6_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        private void txtTotaltrabalhadanoturna7_EditValueChanged(object sender, EventArgs e)
        {
            SomaCargaHorariaNoturnaSemana();
        }

        #endregion

        private void AtualizaJornadas()
        {
            DataTable jornadas = bllJornada.GetAll();
            cbIdJornada.Properties.DataSource = jornadas;
            cbIdjornadaSeg.Properties.DataSource = jornadas;
            cbIdjornadaTer.Properties.DataSource = jornadas;
            cbIdjornadaQua.Properties.DataSource = jornadas;
            cbIdjornadaQui.Properties.DataSource = jornadas;
            cbIdjornadaSex.Properties.DataSource = jornadas;
            cbIdjornadaSab.Properties.DataSource = jornadas;
            cbIdjornadaDom.Properties.DataSource = jornadas;
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

        private void sbIdjornadaSeg_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaSeg.EditValue;
            GridSelecao(formJornada, cbIdjornadaSeg, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbIdjornadaTer_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaTer.EditValue;
            GridSelecao(formJornada, cbIdjornadaTer, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbIdjornadaQua_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaQua.EditValue;
            GridSelecao(formJornada, cbIdjornadaQua, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbIdjornadaQui_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaQui.EditValue;
            GridSelecao(formJornada, cbIdjornadaQui, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbIdjornadaSex_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaSex.EditValue;
            GridSelecao(formJornada, cbIdjornadaSex, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbIdjornadaSab_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaSab.EditValue;
            GridSelecao(formJornada, cbIdjornadaSab, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbIdjornadaDom_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdjornadaDom.EditValue;
            GridSelecao(formJornada, cbIdjornadaDom, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
        }

        private void sbJornada_Click(object sender, EventArgs e)
        {
            FormGridJornada formJornada = new FormGridJornada();
            formJornada.cwTabela = "Jornada";
            formJornada.cwId = (int)cbIdJornada.EditValue;
            GridSelecao(formJornada, cbIdJornada, bllJornada);
            if (formJornada.cwAtualiza)
                AtualizaJornadas();
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

        private void FormManutHorario_Shown(object sender, EventArgs e)
        {
            XtraTab.SelectedTabPage = tabParametrosHorario;
            XtraTab.SelectedTabPage = tabTurnoNormal;
        }

        private void chbPercBHSegunda_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHSegunda.Checked)
            {
                txtSegundaPercBanco.EditValue = null;
                txtSegundaPercBanco.Enabled = false;
            }
            else
            {
                txtSegundaPercBanco.Enabled = true;
            }
        }

        private void chbPercBHTerca_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHTerca.Checked)
            {
                txtTercaPercBanco.EditValue = null;
                txtTercaPercBanco.Enabled = false;
            }
            else
            {
                txtTercaPercBanco.Enabled = true;
            }
        }

        private void chbPercBHQuarta_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHQuarta.Checked)
            {
                txtQuartaPercBanco.EditValue = null;
                txtQuartaPercBanco.Enabled = false;
            }
            else
            {
                txtQuartaPercBanco.Enabled = true;
            }
        }

        private void chbPercBHQuinta_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHQuinta.Checked)
            {
                txtQuintaPercBanco.EditValue = null;
                txtQuintaPercBanco.Enabled = false;
            }
            else
            {
                txtQuintaPercBanco.Enabled = true;
            }
        }

        private void chbPercBHSexta_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHSexta.Checked)
            {
                txtSextaPercBanco.EditValue = null;
                txtSextaPercBanco.Enabled = false;
            }
            else
            {
                txtSextaPercBanco.Enabled = true;
            }
        }

        private void chbPercBHSabado_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHSabado.Checked)
            {
                txtSabadoPercBanco.EditValue = null;
                txtSabadoPercBanco.Enabled = false;
            }
            else
            {
                txtSabadoPercBanco.Enabled = true;
            }
        }

        private void chbPercBHDomingo_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHDomingo.Checked)
            {
                txtDomingoPercBanco.EditValue = null;
                txtDomingoPercBanco.Enabled = false;
            }
            else
            {
                txtDomingoPercBanco.Enabled = true;
            }
        }

        private void chbPercBHFeriado_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHFeriado.Checked)
            {
                txtFeriadoPercBanco.EditValue = null;
                txtFeriadoPercBanco.Enabled = false;
            }
            else
            {
                txtFeriadoPercBanco.Enabled = true;
            }
        }

        private void chbPercBHFolga_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chbPercBHFolga.Checked)
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
            gvLimiteDDsr.MoveLastVisible();
        }

        private void chbDdrsProporcional_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDdrsProporcional.Checked)
            {
                HabilitaDesabilitaDDSRProporcional();
                IncluirLinhaLimiteDDsr();
            }
            else
            {
                LimitesDDsrProporcionais.ForEach(s => s.Delete = true);
                gcLimiteDDsr.DataSource = null;
                HabilitaDesabilitaDDSRProporcional();
            }
          
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
            RemoverLimitesDDSRProporcional();
        }

        private void RemoverLimitesDDSRProporcional()
        {
            int[] rows = gvLimiteDDsr.GetSelectedRows();
            if (rows.Length > 0)
            {
                Modelo.LimiteDDsr objSelecionado;
                objSelecionado = (Modelo.LimiteDDsr)gvLimiteDDsr.GetRow(rows[0]);

                if (LimitesDDsrProporcionais.Where(s => s == objSelecionado).FirstOrDefault() != null)
                {
                    LimitesDDsrProporcionais.Where(s => s == objSelecionado).FirstOrDefault().Delete = true;
                    LimitesDDsrProporcionais.Where(s => s == objSelecionado).FirstOrDefault().Acao = Modelo.Acao.Excluir;
                }

                AtualizaGridLimitesDDsrProporcionais();

            }
        }

    }
}
