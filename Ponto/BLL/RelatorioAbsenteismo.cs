using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Modelo;

namespace BLL
{
    public class RelatorioAbsenteismo
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public DataTable Funcionarios { get; set; }
        public bool Faltas { get; set; }
        public bool Atrasos { get; set; }
        public bool Abonadas { get; set; }
        public bool DebitoBH { get; set; }

        private List<Modelo.JornadaAlternativa> jornadasList;
        private List<Modelo.Afastamento> afastamentos;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public RelatorioAbsenteismo(DateTime dataInicial, DateTime dataFinal, DataTable funcionarios, bool faltas, bool atrasos, bool abonadas, bool debitoBH, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            Funcionarios = funcionarios;
            Faltas = faltas;
            Atrasos = atrasos;
            Abonadas = abonadas;
            DebitoBH = debitoBH;
        }

        public IEnumerable<AbsenteismoLinha> Gerar()
        {
            JornadaAlternativa bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);
            Afastamento bllAfastamento = new Afastamento(ConnectionString, UsuarioLogado);
            jornadasList = bllJornadaAlternativa.GetPeriodo(DataInicial, DataFinal);
            if (Abonadas)
                // possui o afastamento de todos os funcionários no período
                afastamentos = bllAfastamento.GetParaExportacaoFolha(DataInicial, DataFinal, null, true, new List<int>());
            return Calcular();
        }

        private IEnumerable<AbsenteismoLinha> Calcular()
        {
            foreach (DataRow func in Funcionarios.Rows)
            {
                int idFuncionario = Convert.ToInt32(func["idFuncionario"]);
                Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                DataTable marcacoes = bllMarcacao.GetParaRelatorioAbstinencia(idFuncionario, DataInicial, DataFinal);
                if (afastamentos != null)
                {
                    var afastamentoAgrupado = afastamentos.Where(x => x.IdFuncionario == idFuncionario)
                                                     .GroupBy(g => g.CodigoOcorrencia)
                                                     .Select(s => new { codigoOcorrencia = s.Key, ocorrencia = s.Max(m => m.ocorrencia) });
                    foreach (var afastamento in afastamentoAgrupado)
                    {

                        var linha = CriarLinhaRelatorio(func, afastamento.codigoOcorrencia, afastamento.ocorrencia);
                        CalcularAbonoFuncionario(linha, marcacoes);
                        if (linha.QuantidadeHoras > 0)
                            yield return linha;
                    }

                }

                var linhaAtraso = CriarLinhaRelatorio(func, 0, "Atraso");
                CalcularAbonoFuncionario(linhaAtraso, marcacoes);
                if (linhaAtraso.QuantidadeHoras > 0)
                    yield return linhaAtraso;

                var linhaFalta = CriarLinhaRelatorio(func, 0, "Falta");
                CalcularAbonoFuncionario(linhaFalta, marcacoes);
                if (linhaFalta.QuantidadeHoras > 0)
                    yield return linhaFalta;

                var linhaDebBH = CriarLinhaRelatorio(func, 0, "Débito Banco Horas");
                CalcularAbonoFuncionario(linhaDebBH, marcacoes);
                if (linhaDebBH.QuantidadeHoras > 0)
                    yield return linhaDebBH;
            }
        }

        private void CalcularAbonoFuncionario(AbsenteismoLinha linha, DataTable marcacoes)
        {
            JornadaAlternativa bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);

            foreach (DataRow marc in marcacoes.Rows)
            {
                DateTime data = Convert.ToDateTime(marc["data"]);
                int horasfaltasD = ((string)marc["horasfaltas"]).ConvertHorasMinuto();
                int horasfaltasN = ((string)marc["horasfaltanoturna"]).ConvertHorasMinuto();
                int horasDiurnasJornada, horasNoturnasJornada, cargahorariamista;
                int horasBHDeb = ((string)marc["bancohorasdeb"]).ConvertHorasMinuto();
                Modelo.JornadaAlternativa ja = bllJornadaAlternativa.PossuiRegistro(jornadasList, data, linha.IdFuncionario, linha.IdFuncao, linha.IdDepartamento, linha.IdEmpresa);

                if (ja != null)
                {
                    horasDiurnasJornada = ja.TotalTrabalhadaDiurna.ConvertHorasMinuto();
                    horasNoturnasJornada = ja.TotalTrabalhadaNoturna.ConvertHorasMinuto();
                    cargahorariamista = ja.TotalMista.ConvertHorasMinuto();
                }
                else
                {
                    var horarioDetalhe = GetHorarioDetalhe(marc);
                    if (horarioDetalhe == null)
                        continue;
                    horasDiurnasJornada = horarioDetalhe.Totaltrabalhadadiurna.ConvertHorasMinuto();
                    horasNoturnasJornada = horarioDetalhe.Totaltrabalhadanoturna.ConvertHorasMinuto();
                    cargahorariamista = horarioDetalhe.Cargahorariamista.ConvertHorasMinuto();
                }

                CalcularFaltaEAtraso(linha, horasfaltasD, horasfaltasN, horasDiurnasJornada, horasNoturnasJornada);
                CalcularAbonadas(linha, data, horasDiurnasJornada, horasNoturnasJornada, cargahorariamista);
                CalcularDebitoBancoHoras(linha, horasBHDeb);
            }
        }

        private void CalcularDebitoBancoHoras(AbsenteismoLinha linha, int horasBHDeb)
        {
            if (DebitoBH)
            {
                if (linha.ocorrencia.Contains("Débito Banco Horas") && horasBHDeb > 0)
                {
                    linha.QuantidadeHoras += horasBHDeb;
                }
            }
        }

        private void CalcularAbonadas(AbsenteismoLinha linha, DateTime data, int horasDiurnasJornada, int horasNoturnasJornada, int cargahorariamista)
        {
            if (Abonadas)
            {
                var afastamentosData = GetAfastamentosDataOcorrencia(linha, data);
                foreach (var afastamento in afastamentosData)
                {
                    if (afastamento.Parcial == 1)
                    {
                        int abonoDiurno = afastamento.Horai.ConvertHorasMinuto();
                        int abonoNoturno = afastamento.Horaf.ConvertHorasMinuto();

                        if (cargahorariamista == 0)
                        {
                            linha.QuantidadeHoras += Math.Min(abonoDiurno, horasDiurnasJornada);
                            linha.QuantidadeHoras += Math.Min(abonoNoturno, horasNoturnasJornada);
                        }
                        else
                            linha.QuantidadeHoras += Math.Min(abonoDiurno + abonoNoturno, cargahorariamista);

                    }
                    else if (afastamento.Abonado == 1)
                        linha.QuantidadeHoras += horasDiurnasJornada + horasNoturnasJornada + cargahorariamista;
                }
            }
        }

        private void CalcularFaltaEAtraso(AbsenteismoLinha linha, int horasfaltasD, int horasfaltasN, int horasDiurnasJornada, int horasNoturnasJornada)
        {
            if (horasfaltasD < horasDiurnasJornada)
            {
                if ((Atrasos) && (linha.ocorrencia == "Atraso"))
                    linha.QuantidadeHoras += horasfaltasD;
            }
            else if ((Faltas) && (linha.ocorrencia == "Falta"))
            {
                linha.QuantidadeHoras += horasfaltasD;
            }

            if (horasfaltasN < horasNoturnasJornada)
            {
                if ((Atrasos) && (linha.ocorrencia == "Atraso"))
                    linha.QuantidadeHoras += horasfaltasN;
            }
            else if ((Faltas) && (linha.ocorrencia == "Falta"))
            {
                linha.QuantidadeHoras += horasfaltasN;
            }
        }

        private IEnumerable<Modelo.Afastamento> GetAfastamentosDataOcorrencia(AbsenteismoLinha linha, DateTime data)
        {
            return afastamentos.Where(a => data >= a.Datai && data <= a.Dataf && a.CodigoOcorrencia == linha.codigoOcorrencia
                                                                           && (
                                                                           (a.Tipo == 0 && a.IdFuncionario == linha.IdFuncionario) ||
                                                                           (a.Tipo == 1 && a.IdDepartamento == linha.IdDepartamento) ||
                                                                           (a.Tipo == 2 && a.IdEmpresa == linha.IdEmpresa)
                                                                           ));
        }

        private Modelo.HorarioDetalhe GetHorarioDetalhe(DataRow marc)
        {
            var horarioDetalhe = new Modelo.HorarioDetalhe();

            if ((marc["tipohorario"] is DBNull ? 0 : Convert.ToInt32(marc["tipohorario"])) == 1)
            {
                horarioDetalhe.Totaltrabalhadadiurna = marc["chdiurnanormal"] is DBNull ? "--:--" : marc["chdiurnanormal"].ToString();
                horarioDetalhe.Totaltrabalhadanoturna = marc["chnoturnanormal"] is DBNull ? "--:--" : marc["chnoturnanormal"].ToString();
                horarioDetalhe.Flagfolga = marc["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolganormal"]);
                if (!(marc["cargamistanormal"] is DBNull))
                {
                    horarioDetalhe.Cargahorariamista = marc["cargamistanormal"].ToString();
                }
            }
            else
            {
                if (marc["chdiurnaflexivel"] is DBNull)
                {
                    return null;
                }
                else
                {
                    horarioDetalhe.Totaltrabalhadadiurna = marc["chdiurnaflexivel"] is DBNull ? "--:--" : marc["chdiurnaflexivel"].ToString();
                    horarioDetalhe.Totaltrabalhadanoturna = marc["chnoturnaflexivel"] is DBNull ? "--:--" : marc["chnoturnaflexivel"].ToString();
                    horarioDetalhe.Flagfolga = marc["flagfolgaflexivel"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolgaflexivel"]);
                    if (!(marc["cargamistaflexivel"] is DBNull))
                    {
                        horarioDetalhe.Cargahorariamista = marc["cargamistaflexivel"].ToString();
                    }
                }
            }
            return horarioDetalhe;
        }

        private AbsenteismoLinha CriarLinhaRelatorio(DataRow func, int codigoOcorrencia, string ocorrencia)
        {
            var linha = new AbsenteismoLinha();
            linha.Empresa = func["empresa"].ToString();
            linha.Departamento = func["departamento"].ToString();
            linha.DSCodigo = func["dscodigo"].ToString();
            linha.Funcionario = func["funcionario"].ToString();
            linha.IdEmpresa = Convert.ToInt32(func["idempresa"]);
            linha.IdDepartamento = Convert.ToInt32(func["iddepartamento"]);
            linha.IdFuncao = Convert.ToInt32(func["idfuncao"]);
            linha.IdFuncionario = Convert.ToInt32(func["idfuncionario"]);
            linha.codigoOcorrencia = codigoOcorrencia;
            linha.ocorrencia = ocorrencia;
            return linha;
        }
    }

    public class AbsenteismoLinha
    {
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        public string DSCodigo { get; set; }
        public string Funcionario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdDepartamento { get; set; }
        public int IdFuncao { get; set; }
        public int IdFuncionario { get; set; }
        public int QuantidadeHoras { get; set; }
        public int idFuncao { get; set; }
        public int codigoOcorrencia { get; set; }
        public string ocorrencia { get; set; }

    }
}
