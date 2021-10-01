using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = DAL.SQL;
using Modelo;
using Modelo.Proxy;
using System.Collections;
using System.Globalization;

namespace BLL
{
    public class HoraExtra
    {

        private DataTable dtMarcacoes;
        /// <summary>
        ///     Construtor que recebe um DataTable de marcações para o calculo das horas extras.
        /// </summary>
        /// <param name="dtMarcacoes"> DataTable de marcações </param>
        public HoraExtra(DataTable dtMarcacoes)
        {
            this.dtMarcacoes = dtMarcacoes;
        }

        /// <summary>
        /// Construtor para quando não possui o DataTable de marcações, esse construtor carrega o datatable de acordo com a lista de funcionários e período.
        /// Quando já possuir o DataTable de marcações utilizar o construtor HoraExtra(DataTable dtMarcacoes)
        /// </summary>
        /// <param name="idsFuncionarios">String com os ids de funcinários separados por virgula ex: "1,2,3,125,578"</param>
        /// <param name="pdataInicial">Data inicial do período</param>
        /// <param name="pDataFinal">Data Final do período</param>
        /// <param name="CodsLocalReps">String com os códigos dos locais do rep separados por vírgula, para trazer os registros que não possuam essa informação passar -1. Ex: '-1,1,2,3,25,36'</param>
        public HoraExtra(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            D.Marcacao dalMarcacao = new D.Marcacao(new D.DataBase(""));
            DataTable dtMarcacoes = dalMarcacao.GetRelatorioObras(idsFuncionarios, pdataInicial, pDataFinal, "");
        }

        /// <summary>
        /// Calcula as Horas Extras por dia de acordo com um DataTable de marcação
        /// </summary>
        /// <returns>Retorna uma lista com as horas extras por funcionário e data</returns>
        public IList<HorasExtrasPorDia> CalcularHoraExtraDiaria()
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
            IList<HorasExtrasPorDia> horasExtrasDoPeriodo = new List<HorasExtrasPorDia>();
            foreach (DataRow drMarcacao in dtMarcacoes.Rows)
            {
                int horaExtraNoturna = Modelo.cwkFuncoes.ConvertHorasMinuto((string)drMarcacao["horasextranoturna"]);
                int horaExtraDiurna = Modelo.cwkFuncoes.ConvertHorasMinuto((string)drMarcacao["horasextrasdiurna"]);
                int horaExtraRealizada = horaExtraDiurna + horaExtraNoturna;
                DateTime dataMarc = Convert.ToDateTime(drMarcacao["data"], culture);
                if (horaExtraRealizada > 0) // Verifica se existe hora extra no dia para realizar o cálculo.
                {
                    #region Parametros para calculo
                    PercentualHoraExtra[] horariosPHExtra = new PercentualHoraExtra[10];
                    BLL.TotalizadorHorasFuncionario.AtribuaPercentuaisExtra(horariosPHExtra, drMarcacao);
                    int idFuncionario = Convert.ToInt32(drMarcacao["idFuncionario"]);
                    int flagfolganormal = drMarcacao["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(drMarcacao["flagfolganormal"]);
                    int flagfolgaflexivel = drMarcacao["flagfolgaflexivel"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(drMarcacao["flagfolgaflexivel"]);
                    int flagFolga = flagfolganormal == 1 || flagfolgaflexivel == 1 ? 1 : 0;
                    short consideraSabadoSemana = Convert.ToInt16(drMarcacao["considerasabadosemana"]);
                    short consideraDomingoSemana = Convert.ToInt16(drMarcacao["consideradomingosemana"]);
                    short tipoAcumulo = Convert.ToInt16(drMarcacao["tipoacumulo"]);
                    int diaFinalSemana = BLL.PercentualHorasExtras.SetarDiaFinalSemana(consideraSabadoSemana, consideraDomingoSemana);
                    TipoDiaAcumulo tipoDia = BLL.PercentualHorasExtras.SetarTipoDia(drMarcacao, flagFolga, consideraSabadoSemana, consideraDomingoSemana, true);
                    #endregion

                    //Rotina esta preparada apenas para calcular horas extras acumuladas Diariamente ou Mensalmente (Semanalmente deve ser desenvolvida ainda)
                    if (tipoAcumulo == 1 || tipoAcumulo == 3 || tipoAcumulo == 2)
                    {
                        //Inicializa o Objeto que guardara as horas extras por dia
                        HorasExtrasPorDia horasExtrasDoDia = InicializaHorasExtrasPorDia(horasExtrasDoPeriodo, idFuncionario, dataMarc, tipoDia);

                        if (tipoDia == TipoDiaAcumulo.Geral)//Cálculo hora extra dias Geral (Seg/Ter/Qua/Qui/Sex)
                        {
                            for (int i = 0; i < 6; i++) //Pega os percentuais gerais
                            {
                                horasExtrasDoDia.TipoAcumulo = tipoAcumulo;
                                PercentualHoraExtra horarioPHExtra = horariosPHExtra[i];
                                CalcularHoraExtra(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
                            }
                        }

                        if (tipoDia == TipoDiaAcumulo.Sabado) //Cálculo hora extra Sábado
                        {
                            PercentualHoraExtra horarioPHExtra = horariosPHExtra[6]; //Pega os percentuais Referente ao Sábado
                            horasExtrasDoDia.TipoAcumulo = horarioPHExtra.TipoAcumulo;
                            CalcularHoraExtra(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
                        }

                        if (tipoDia == TipoDiaAcumulo.Domingo) //Cálculo hora extra Domingo
                        {
                            PercentualHoraExtra horarioPHExtra = horariosPHExtra[7]; //Pega os percentuais Referente ao Domingo
                            horasExtrasDoDia.TipoAcumulo = horarioPHExtra.TipoAcumulo;
                            CalcularHoraExtra(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
                        }

                        if (tipoDia == TipoDiaAcumulo.Feriado) //Cálculo hora extra Feriado
                        {
                            PercentualHoraExtra horarioPHExtra = horariosPHExtra[8]; //Pega os percentuais Referente ao Feriado
                            CalcularHoraExtra(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
                        }

                        if (tipoDia == TipoDiaAcumulo.Folga) //Cálculo hora extra Folga
                        {
                            PercentualHoraExtra horarioPHExtra = horariosPHExtra[9]; //Pega os percentuais Referente ao Folga
                            horasExtrasDoDia.TipoAcumulo = horarioPHExtra.TipoAcumulo;
                            CalcularHoraExtra(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
                        }
                    }
                    else if (tipoAcumulo != -1) // Caso for -1 (não foi selecionado tipo acumulo) ignora
                    {
                        throw new Exception("Para esse tipo de cálculo de horas extras apenas o acumulo diário e mensal foi implementado.");
                    }
                }
            }
            return horasExtrasDoPeriodo;
        }

        public static void ValidaHorariosDiferenteDiarioMensal(string connectionString, Modelo.Cw_Usuario usuarioLogado, List<DataTable> PedacosRelatorio)
        {
            if (PedacosRelatorio.Count > 0)
            {
                DataTable todos = new DataTable();
                PedacosRelatorio.ForEach(f => todos.Merge(f));
                if (todos.Rows.Count > 0)
                {
                    string[] selectedColumns = new[] { "idhorario", "tipoacumulo" };

                    DataTable dt = new DataView(todos).ToTable(true, selectedColumns);
                    if (dt.Select("tipoacumulo <> 1 and tipoacumulo <> 2 and tipoacumulo <> 3 and tipoacumulo <> -1").Count() > 0)
                    {
                        dt = dt.Select("tipoacumulo <> 1 and tipoacumulo <> 2 and tipoacumulo <> 3 and tipoacumulo <> -1").CopyToDataTable();
                        if (!dt.HasErrors && dt.Rows.Count > 0)
                        {
                            Hashtable ids = new Hashtable();
                            BLL.Horario bllHorario = new BLL.Horario(connectionString, usuarioLogado);

                            foreach (DataRow row in dt.Rows)
                            {
                                ids.Add(Convert.ToInt32(row["idhorario"]), Convert.ToInt32(row["idhorario"]));
                            }
                            List<Modelo.Horario> horariosNaoImplementados = bllHorario.GetParaIncluirMarcacao(ids, false);
                            if (horariosNaoImplementados.Count() > 0)
                            {
                                string horariosImpedindoRelatorio = String.Join("</br>", horariosNaoImplementados.Select(s => "Codigo: " + s.Codigo + " - Descrição:" + s.Descricao));
                                throw new Exception("O relatório não considera horários com tipo acumulo diferente de Diário e Mensal, horários em desacordo: </br>" + horariosImpedindoRelatorio);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retorna/Crias o objeto que guardará os dados da hora extra do dia.
        /// </summary>
        /// <param name="horasExtrasPorDia">Objeto com as horas extras já calculadas.</param>
        /// <param name="idFuncionario">Id do funcionário da hora extra</param>
        /// <param name="dataMarc">Data da marcação da hora extra</param>
        /// <param name="tipoDiaAcumulo">Tipo do Acumulo (Geral|Sabado|Domingo|Feriado|Folga)</param>
        /// <returns></returns>
        private HorasExtrasPorDia InicializaHorasExtrasPorDia(IList<HorasExtrasPorDia> horasExtrasPorDia, int idFuncionario, DateTime dataMarc, TipoDiaAcumulo tipoDiaAcumulo)
        {
            if (horasExtrasPorDia.Where(x => x.DataMarcacao == dataMarc && x.IdFuncionario == idFuncionario).Count() == 0)
            {
                HorasExtrasPorDia hepd = new HorasExtrasPorDia();
                hepd.IdFuncionario = idFuncionario;
                hepd.DataMarcacao = dataMarc;
                hepd.TipoDiaAcumulo = tipoDiaAcumulo;
                hepd.HorasExtras = new List<Modelo.Proxy.HoraExtra>();
                horasExtrasPorDia.Add(hepd);
            }

            return horasExtrasPorDia.Where(x => x.DataMarcacao == dataMarc && x.IdFuncionario == idFuncionario).FirstOrDefault();
        }

        /// <summary>
        ///     Rotina que chama o calculo de acordo com os percentuais
        /// </summary>
        /// <param name="horasExtrasDoPeriodo">Lista com os dias já calculados</param>
        /// <param name="horaExtraDiurna">Quantidade de hora extra Diurna executada no dia</param>
        /// <param name="horaExtraNoturna">Quantidade de hora extra Noturna executada no dia</param>
        /// <param name="tipoDia">Tipo do dia da hora extra (Geral(Seg|Ter|Qua|Quin|Sex), Sábado, Domingo, Feriado, Folga)</param>
        /// <param name="horasExtrasDoDia">Horas extras do dia</param>
        /// <param name="horarioPHExtra">Objeto com os percentuais para calculo das horas extras</param>
        private void CalcularHoraExtra(IList<HorasExtrasPorDia> horasExtrasDoPeriodo, ref int horaExtraDiurna, ref int horaExtraNoturna, TipoDiaAcumulo tipoDia, HorasExtrasPorDia horasExtrasDoDia, ref PercentualHoraExtra horarioPHExtra)
        {
            if (horarioPHExtra.PercentualExtra > 0)
            {
                AcumulaHoraExtraDiurnaNoturna(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
            }
            if (horarioPHExtra.PercentualExtraSegundo > 0)
            {
                horarioPHExtra.QuantidadeExtraMin = 9999;
                horarioPHExtra.PercentualExtra = horarioPHExtra.PercentualExtraSegundo;
                AcumulaHoraExtraDiurnaNoturna(horasExtrasDoPeriodo, ref horaExtraDiurna, ref horaExtraNoturna, tipoDia, horasExtrasDoDia, ref horarioPHExtra);
            }
        }

        private void AcumulaHoraExtraDiurnaNoturna(IList<HorasExtrasPorDia> horasExtrasDoPeriodo, ref int horaExtraDiurna, ref int horaExtraNoturna, TipoDiaAcumulo tipoDia, HorasExtrasPorDia horasExtrasDoDia, ref PercentualHoraExtra horarioPHExtra)
        {
            AcumulaHoraExtraDiurna(horasExtrasDoPeriodo, horasExtrasDoDia, ref horarioPHExtra, ref horaExtraDiurna, tipoDia);
            AcumulaHoraExtraNoturna(horasExtrasDoPeriodo, horasExtrasDoDia, ref horarioPHExtra, ref horaExtraNoturna, tipoDia);
        }

        /// <summary>
        ///     Realiza o Calcula das horas extras Diurna do dias
        /// </summary>
        /// <param name="horasExtrasDoPeriodo">Lista com os dias já calculados</param>
        /// <param name="horasExtrasDoDia">Horas extras do dia</param>
        /// <param name="horarioPHExtra">Objeto com os percentuais para calculo das horas extras</param>
        /// <param name="horaExtraDiurna">Quantidade de hora extra Noturna executada no dia</param>
        /// <param name="tipoDiaAcumulo">Tipo do dia da hora extra (Geral(Seg|Ter|Qua|Quin|Sex), Sábado, Domingo, Feriado, Folga)</param>
        private void AcumulaHoraExtraDiurna(IList<HorasExtrasPorDia> horasExtrasDoPeriodo, HorasExtrasPorDia horasExtrasDoDia, ref PercentualHoraExtra horarioPHExtra, ref int horaExtraDiurna, TipoDiaAcumulo tipoDiaAcumulo)
        {
            decimal perc = horarioPHExtra.PercentualExtra;
            int limite = horarioPHExtra.QuantidadeExtraMin;
            if (horasExtrasDoDia.TipoAcumulo == 3) // Se acumulo for por mês, retiro do limite a quantidade já utilizada no mes.
            {
                IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == horasExtrasDoDia.IdFuncionario && x.DataMarcacao.Month == horasExtrasDoDia.DataMarcacao.Month && x.TipoAcumulo == horasExtrasDoDia.TipoAcumulo && x.TipoDiaAcumulo == tipoDiaAcumulo).SelectMany(x => x.HorasExtras).ToList();
                limite -= heMS.Where(x => x.Percentual == perc).Sum(x => x.HoraDiurna);
            }

            if (horasExtrasDoDia.TipoAcumulo == 2) // Se acumulo for por semana, retiro do limite a quantidade já utilizada ndo começo da semana até a data.
            {
                IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == horasExtrasDoDia.IdFuncionario && x.DataMarcacao.Month == horasExtrasDoDia.DataMarcacao.Month && x.TipoAcumulo == horasExtrasDoDia.TipoAcumulo && x.TipoDiaAcumulo == tipoDiaAcumulo).SelectMany(x => x.HorasExtras).ToList();
                ///

                DateTime dtIni = new DateTime(horasExtrasDoDia.DataMarcacao.StartOfWeek().Year, horasExtrasDoDia.DataMarcacao.StartOfWeek().Month, horasExtrasDoDia.DataMarcacao.StartOfWeek().Day);
                DateTime dtFim = new DateTime(horasExtrasDoDia.DataMarcacao.Year, horasExtrasDoDia.DataMarcacao.Month, horasExtrasDoDia.DataMarcacao.Day);



                IList<Modelo.Proxy.HoraExtra> heMST = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == horasExtrasDoDia.IdFuncionario && x.DataMarcacao >= dtIni && x.DataMarcacao <= dtFim && x.TipoAcumulo == horasExtrasDoDia.TipoAcumulo && x.TipoDiaAcumulo == tipoDiaAcumulo).SelectMany(x => x.HorasExtras).ToList();
                limite -= heMST.Where(x => x.Percentual == perc).Sum(x => x.HoraDiurna);
            }



            short TipoAcumulo = horarioPHExtra.TipoAcumulo;
            Modelo.Proxy.HoraExtra he = horasExtrasDoDia.HorasExtras.Where(x => x.Percentual == perc).FirstOrDefault();
            if (he == null)
            {
                he = new Modelo.Proxy.HoraExtra();
                horasExtrasDoDia.HorasExtras.Add(he);
            }
            he.Percentual = horarioPHExtra.PercentualExtra;
            if (limite > horaExtraDiurna)
            {
                he.HoraDiurna += horaExtraDiurna;
                limite -= horaExtraDiurna;
                horaExtraDiurna = 0;
            }
            else
            {
                he.HoraDiurna += limite;
                horaExtraDiurna -= limite;
                limite = 0;
            }
            horarioPHExtra.QuantidadeExtraMin = limite;
        }

        /// <summary>
        ///     Realiza o Calcula das horas extras Noturna do dias
        /// </summary>
        /// <param name="horasExtrasDoPeriodo">Lista com os dias já calculados</param>
        /// <param name="horasExtrasDoDia">Horas extras do dia</param>
        /// <param name="horarioPHExtra">Objeto com os percentuais para calculo das horas extras</param>
        /// <param name="horaExtraNoturna">Quantidade de hora extra Noturna executada no dia</param>
        /// <param name="tipoDiaAcumulo">Tipo do dia da hora extra (Geral(Seg|Ter|Qua|Quin|Sex), Sábado, Domingo, Feriado, Folga)</param>
        private void AcumulaHoraExtraNoturna(IList<HorasExtrasPorDia> horasExtrasDoPeriodo, HorasExtrasPorDia horasExtrasDoDia, ref PercentualHoraExtra horarioPHExtra, ref int horaExtraNoturna, TipoDiaAcumulo tipoDiaAcumulo)
        {
            decimal perc = horarioPHExtra.PercentualExtra;
            int limite = horarioPHExtra.QuantidadeExtraMin;
            if (horasExtrasDoDia.TipoAcumulo == 3) // Se acumulo for por mês, retiro do limite a quantidade já utilizada no mes.
            {
                IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == horasExtrasDoDia.IdFuncionario && x.DataMarcacao.Month == horasExtrasDoDia.DataMarcacao.Month && x.TipoAcumulo == horasExtrasDoDia.TipoAcumulo && x.TipoDiaAcumulo == tipoDiaAcumulo).SelectMany(x => x.HorasExtras).ToList();
                limite -= heMS.Where(x => x.Percentual == perc).Sum(x => x.HoraNoturna);
            }

            if (horasExtrasDoDia.TipoAcumulo == 2) // Se acumulo for por semana, retiro do limite a quantidade já utilizada ndo começo da semana até a data.
            {
                IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == horasExtrasDoDia.IdFuncionario && x.DataMarcacao.Month == horasExtrasDoDia.DataMarcacao.Month && x.TipoAcumulo == horasExtrasDoDia.TipoAcumulo && x.TipoDiaAcumulo == tipoDiaAcumulo).SelectMany(x => x.HorasExtras).ToList();
                ///

                DateTime dtIni = new DateTime(horasExtrasDoDia.DataMarcacao.StartOfWeek().Year, horasExtrasDoDia.DataMarcacao.StartOfWeek().Month, horasExtrasDoDia.DataMarcacao.StartOfWeek().Day);
                DateTime dtFim = new DateTime(horasExtrasDoDia.DataMarcacao.Year, horasExtrasDoDia.DataMarcacao.Month, horasExtrasDoDia.DataMarcacao.Day);



                IList<Modelo.Proxy.HoraExtra> heMST = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == horasExtrasDoDia.IdFuncionario && x.DataMarcacao >= dtIni && x.DataMarcacao <= dtFim && x.TipoAcumulo == horasExtrasDoDia.TipoAcumulo && x.TipoDiaAcumulo == tipoDiaAcumulo).SelectMany(x => x.HorasExtras).ToList();
                limite -= heMST.Where(x => x.Percentual == perc).Sum(x => x.HoraNoturna);
            }

            short TipoAcumulo = horarioPHExtra.TipoAcumulo;
            Modelo.Proxy.HoraExtra he = horasExtrasDoDia.HorasExtras.Where(x => x.Percentual == perc).FirstOrDefault();
            if (he == null)
            {
                he = new Modelo.Proxy.HoraExtra();
                horasExtrasDoDia.HorasExtras.Add(he);
            }
            he.Percentual = perc;
            if (limite > horaExtraNoturna)
            {
                he.HoraNoturna += horaExtraNoturna;
                limite -= horaExtraNoturna;
                horaExtraNoturna = 0;
            }
            else
            {
                he.HoraNoturna += limite;
                horaExtraNoturna -= limite;
                limite = 0;
            }
            horarioPHExtra.QuantidadeExtraMin = limite;
        }
    }
}