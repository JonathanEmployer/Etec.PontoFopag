using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections;
using Modelo;
using Modelo.Proxy;
using DAL.SQL;

namespace BLL
{
    public struct PercentualHoraExtra
    {
        public decimal PercentualExtra { get; set; }
        public string QuantidadeExtra { get; set; }
        public int QuantidadeExtraMin { get; set; }
        public short TipoAcumulo { get; set; }
        public decimal PercentualExtraSegundo { get; set; }
        public decimal? PercentualExtraNoturna { get; set; }
        public string QuantidadeExtraNoturna { get; set; }
        public int QuantidadeExtraNoturnaMin { get; set; }
        public decimal? PercentualExtraSegundoNoturna { get; set; }
        public bool SeparaExtraNoturnaPercentual { get; set; }
    }

    public class TotalizadorHorasFuncionario
    {
        private readonly BLL.Marcacao bllMarcacao;
        private readonly BLL.FechamentoBHD bllFechamentoBHD;
        private readonly BLL.FechamentoBH bllFechamentoBH;
        private readonly BLL.Funcionario bllFuncionario;
        private readonly BLL.BancoHoras bllBancoHoras;
        private readonly BLL.HorarioDetalhe bllHorarioDetalhe;
        private readonly BLL.Horario bllHorario;
        private readonly BLL.HorarioPHExtra bllHorarioPHExtra;
        private readonly BLL.JornadaAlternativa bllJornadaAlternativa;
        private readonly int idEmpresa;
        private readonly int idDepartamento;
        private readonly int idFuncionario;
        private readonly int idFuncao;
        private readonly DateTime dataI;
        private readonly DateTime dataF;
        private readonly List<Modelo.Afastamento> afastamentosSelecionados = new List<Modelo.Afastamento>();
        private readonly List<Modelo.Afastamento> afastamentosAbsenteismo = new List<Modelo.Afastamento>();
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private List<Modelo.JornadaAlternativa> jornadasAlternativasList;
        private int totalExtraDiurna = 0, totalExtraNoturna = 0, totalFaltaDiurna = 0, totalFaltaNoturna = 0, totalExtraNoturnaBH = 0, totalInterjornadaExtra = 0;
        private int totalTrabDiurna = 0, totalTrabNoturna = 0, totalDDSR = 0, qtdDDSR = 0, totalAtrasoDiurno = 0, totalAtrasoNoturno = 0, totalAdNoturno = 0, qtdAdNot = 0;
        private double percAdicNoturno;
        private int totalFaltas = 0, horasfaltasD = 0, horasfaltasN = 0, totalDiasAbonados = 0, totalDiasDSR = 0, totalMinutosAbono = 0, totalMinutosDSR = 0, totalHorasInItinerePercDentroJornada = 0, totalHorasInItinerePercForaJornada = 0;
        private int[,] listTotalHorasGeral = new int[10, 3];
        private int[,] listTotalHoras = new int[10, 3];

        public bool CalcularAtraso { get; set; }
        public DataTable Marcacoes { get; private set; }

        public TotalizadorHorasFuncionario(int pIdEmpresa, int pIdDepartamento, int pIdFuncionario, int pIdFuncao
                                        , DateTime pDataI, DateTime pDataF, List<Modelo.JornadaAlternativa> pJornadasAlternativasList
                                        , DataTable marcacoes, List<Modelo.Afastamento> pAfastamentosSelecionados, List<Modelo.Afastamento> pAfastamentosAbsenteismo
                                        , string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            if (usuarioLogado == null)
            {
                UsuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
            }
            else
            {
                UsuarioLogado = usuarioLogado;
            }

            bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            bllFechamentoBHD = new FechamentoBHD(ConnectionString, UsuarioLogado);
            bllFechamentoBH = new FechamentoBH(ConnectionString, UsuarioLogado);
            bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
            bllBancoHoras = new BancoHoras(ConnectionString, UsuarioLogado);
            bllHorarioDetalhe = new HorarioDetalhe(ConnectionString, UsuarioLogado);
            bllHorario = new Horario(ConnectionString, UsuarioLogado);
            bllHorarioPHExtra = new HorarioPHExtra(ConnectionString, UsuarioLogado);
            bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);

            CalcularAtraso = true;

            idEmpresa = pIdEmpresa;
            idDepartamento = pIdDepartamento;
            idFuncionario = pIdFuncionario;
            idFuncao = pIdFuncao;
            dataI = pDataI;
            dataF = pDataF;

            jornadasAlternativasList = pJornadasAlternativasList;
            if (pAfastamentosSelecionados != null)
                afastamentosSelecionados.AddRange(pAfastamentosSelecionados);
            if (pAfastamentosAbsenteismo != null)
                afastamentosAbsenteismo.AddRange(pAfastamentosAbsenteismo);

            if (marcacoes == null)
                Marcacoes = bllMarcacao.GetParaTotalizaHoras(pIdFuncionario, pDataI, pDataF, false);
            else
                Marcacoes = marcacoes;

        }

        public TotalizadorHorasFuncionario(Modelo.Funcionario objFuncionario, DateTime pDataI, DateTime pDataF, string connString, Modelo.Cw_Usuario usuarioLogado)
            : this(objFuncionario.Idempresa, objFuncionario.Iddepartamento, objFuncionario.Id, objFuncionario.Idfuncao, pDataI, pDataF, connString, usuarioLogado)
        {
        }

        public TotalizadorHorasFuncionario(int idEmpresa, int idDepartamento, int idFuncionario, int idFuncao, DateTime pDataI, DateTime pDataF, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            if (usuarioLogado == null)
            {
                usuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
            }
            UsuarioLogado = usuarioLogado;
            bllMarcacao = new Marcacao(ConnectionString, usuarioLogado);
            bllFechamentoBHD = new FechamentoBHD(ConnectionString, usuarioLogado);
            bllFechamentoBH = new FechamentoBH(ConnectionString, usuarioLogado);
            bllFuncionario = new Funcionario(ConnectionString, usuarioLogado);
            bllBancoHoras = new BancoHoras(ConnectionString, usuarioLogado);
            bllHorarioDetalhe = new HorarioDetalhe(ConnectionString, usuarioLogado);
            bllHorario = new Horario(ConnectionString, usuarioLogado);
            bllHorarioPHExtra = new HorarioPHExtra(ConnectionString, usuarioLogado);
            bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, usuarioLogado);

            CalcularAtraso = true;


            this.idEmpresa = idEmpresa;
            this.idDepartamento = idDepartamento;
            this.idFuncionario = idFuncionario;
            this.idFuncao = idFuncao;

            dataI = pDataI;
            dataF = pDataF;
            jornadasAlternativasList = bllJornadaAlternativa.GetPeriodoFuncionarios(pDataI, pDataF, new List<int>() { idFuncionario });
            Marcacoes = bllMarcacao.GetParaTotalizaHoras(idFuncionario, pDataI, pDataF, true);
        }

        public void TotalizeHorasEBancoHoras(Modelo.TotalHoras objTotalHoras)
        {
            TotalizeHoras(objTotalHoras);
            TotalizeBancoHoras(objTotalHoras);
        }

        public void CalculaFaltaeAtraso(Modelo.TotalHoras objTotalHoras, DataRow marc)
        {
            Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
            CarregaHorarioDetalhe(objHorarioDetalhe, marc);
            CalculaFaltaeAtraso(objHorarioDetalhe, marc);
            AtribuaTotais(objTotalHoras);
        }

        private void TotalizeBancoHoras(Modelo.TotalHoras objTotalHoras)
        {
            List<int> idsFuncs = Marcacoes.AsEnumerable().Select(r => Convert.ToInt32(r["idfuncionario"])).Distinct().ToList();
            if (idsFuncs.Count() <= 0)
            {
                idsFuncs = bllFuncionario.GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(idFuncao, idDepartamento, 0, idEmpresa);
                idsFuncs.Add(idFuncionario);
            }

            List<Modelo.BancoHoras> bancoHorasList = bllBancoHoras.GetAllListFuncs(false, idsFuncs);
            List<Modelo.FechamentoBH> fechamentoBHList = bllFechamentoBH.GetAllListFuncs(idsFuncs, false);
            List<Modelo.FechamentoBHD> fechamentoBHDList = bllFechamentoBHD.getPorListaFuncionario(idsFuncs);

            BLL.CalculoMarcacoes.TotalizadorBancoHoras totalizadorBancoHoras =
            new BLL.CalculoMarcacoes.TotalizadorBancoHoras(idEmpresa, idDepartamento, idFuncionario, idFuncao, dataI, dataF, bancoHorasList, fechamentoBHList,
                                                           fechamentoBHDList, Marcacoes, true, ConnectionString, UsuarioLogado);

            totalizadorBancoHoras.PreenchaBancoHoras(objTotalHoras);
        }

        public void TotalizeHoras(Modelo.TotalHoras objTotalHoras)
        {
            try
            {
                InicializeTotalizadores();

                PercentualHoraExtra[] HorariosPHExtra = new PercentualHoraExtra[10];
                Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                List<(TipoDiaAcumulo, Dictionary<decimal, AcumuloPercentual>)> acumulosTotais = new List<(TipoDiaAcumulo, Dictionary<decimal, AcumuloPercentual>)>();
                Dictionary<TipoDiaAcumulo, Turno> acumulosParciais = new Dictionary<TipoDiaAcumulo, Turno>();
                int idHorarioAnterior = 0;
                Modelo.Horario horario = new Modelo.Horario();
                objTotalHoras.totalInItinere = new List<pxyInItinerePorPercentual>();
                int DiaIniPeriodoFechamento;
                int DiaFimPeriodoFechamento;

                List<Modelo.Empresa> empresas = new List<Modelo.Empresa>();
                if (objTotalHoras.Empresa == null)
                {
                    BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, UsuarioLogado);
                    empresas = bllEmpresa.GetAllList();
                }
                else
                {
                    empresas.Add(objTotalHoras.Empresa);
                }
                int idFunc = 0, idEmp = 0;
                if (Marcacoes.Rows.Count > 0)
                {
                    Int32.TryParse(Marcacoes.Rows[0]["IdFuncionario"].ToString(), out idFunc);
                    Int32.TryParse(Marcacoes.Rows[0]["IdEmpresa"].ToString(), out idEmp);
                }

                if (objTotalHoras.funcionario == null || objTotalHoras.funcionario.diaInicioFechamento == null)
                {
                    CartaoPonto cp = new CartaoPonto(ConnectionString, UsuarioLogado);
                    cp.GetInicioFimFechamentoPonto(empresas, idFunc, out DiaIniPeriodoFechamento, out DiaFimPeriodoFechamento, idEmp);
                }
                else
                {
                    DiaIniPeriodoFechamento = objTotalHoras.funcionario.diaInicioFechamento.GetValueOrDefault();
                    DiaFimPeriodoFechamento = objTotalHoras.funcionario.diaFimfechamento.GetValueOrDefault();
                }

                foreach (DataRow marc in Marcacoes.Rows)
                {
                    DateTime data = PegaData(marc);
                    bool trocaMes = false;

                    DiaIniPeriodoFechamento = DiaIniPeriodoFechamento == 0 ? 1 : DiaIniPeriodoFechamento;
                    if (DiaIniPeriodoFechamento == data.Day)
                    {
                        trocaMes = true;
                    }
                    int dia = Modelo.cwkFuncoes.Dia(data);
                    int idhorario = PegaIdHorario(marc);

                    AtribuaPercentuaisExtra(HorariosPHExtra, marc);

                    if (CarregaHorarioDetalhe(objHorarioDetalhe, marc))
                    {
                        CalculaFaltaeAtraso(objHorarioDetalhe, marc);
                        #region Calcula Horas Extras, Horas Trabalhadas e Horas Faltas
                        int horaExtraNoturna = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horasextranoturna"]);
                        int horaExtraDiurna = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horasextrasdiurna"]);
                        int totalExtra = horaExtraDiurna + horaExtraNoturna;

                        totalTrabDiurna += Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horastrabalhadas"]);
                        totalTrabNoturna += Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horastrabalhadasnoturnas"]);

                        if (marc["AdicionalNoturno"] is DBNull || marc["AdicionalNoturno"].ToString() == "--:--")
                        {
                            totalAdNoturno += Modelo.cwkFuncoes.ConvertHorasMinuto("0");
                        }
                        else
                        {
                            totalAdNoturno += Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["AdicionalNoturno"]);

                            if (!(marc["PercAdicNoturno"] is DBNull))
                            {
                                double percAdDoub;
                                if (Double.TryParse(marc["PercAdicNoturno"].ToString(), out percAdDoub))
                                {
                                    percAdicNoturno = percAdDoub;
                                }
                                else
                                {
                                    percAdicNoturno = 0;
                                }
                            }
                            qtdAdNot++;
                        }
                        int ddsr = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["valordsr"]);

                        if (ddsr > 0)
                        {
                            totalDDSR += ddsr;

                            if (idHorarioAnterior != idhorario)
                            {
                                Modelo.Horario hor = null;
                                if (objTotalHoras.funcionario != null && objTotalHoras.funcionario.HorariosFuncionario != null && objTotalHoras.funcionario.HorariosFuncionario.Count() > 0)
                                {
                                    hor = objTotalHoras.funcionario.HorariosFuncionario.Where(w => w.Id == idhorario).FirstOrDefault();
                                }
                                if (hor == null)
                                {
                                    hor = bllHorario.LoadObject(idhorario);
                                }
                                horario = hor;
                                idHorarioAnterior = idhorario;
                            }

                            if (ddsr < Modelo.cwkFuncoes.ConvertHorasMinuto((string)horario.Qtdhorasdsr))
                            {
                                qtdDDSR++;
                            }
                            else
                            {
                                if (horario.LimitesDDsrProporcionais.Count > 0)
                                {
                                    if (ddsr > horario.LimitesDDsrProporcionais.Max(m => Modelo.cwkFuncoes.ConvertHorasMinuto(m.QtdHorasDsr)))
                                    {
                                        qtdDDSR += 2; //soma mais um na contagem quando houver feriado
                                    }
                                    else
                                    {
                                        qtdDDSR++;
                                    }
                                }
                                else if (Modelo.cwkFuncoes.ConvertHorasMinuto((string)horario.Qtdhorasdsr) > 0)
                                {
                                    qtdDDSR += (ddsr / Modelo.cwkFuncoes.ConvertHorasMinuto((string)horario.Qtdhorasdsr));
                                }
                            }
                        }
                        totalExtraDiurna += horaExtraDiurna;
                        totalExtraNoturna += horaExtraNoturna;
                        totalInterjornadaExtra += Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horaExtraInterjornada"]);
                        totalExtraNoturnaBH += Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["exphorasextranoturna"]);

                        PercentualHorasExtras.TotalizarPercentuaisDia(marc, HorariosPHExtra, objHorarioDetalhe.Flagfolga, trocaMes, dia, data, dataF, horaExtraNoturna, horaExtraDiurna, acumulosTotais, acumulosParciais);

                        TotalizeHorasAfastamento(objTotalHoras, objHorarioDetalhe, data);

                        #endregion
                    }

                    Modelo.Proxy.pxyInItinerePorPercentual initinere = new pxyInItinerePorPercentual();
                    initinere.MinutosDentroJornada = Modelo.cwkFuncoes.ConvertHorasMinuto(marc["InItinereHrsDentroJornada"] is DBNull ? "--:--" : (string)marc["InItinereHrsDentroJornada"]);
                    initinere.MinutosForaJornada = Modelo.cwkFuncoes.ConvertHorasMinuto(marc["InItinereHrsForaJornada"] is DBNull ? "--:--" : (string)marc["InItinereHrsForaJornada"]);
                    initinere.PercentualDentroJornada = marc["InItinerePercDentroJornada"] is DBNull ? 0 : Convert.ToDecimal(marc["InItinerePercDentroJornada"]);
                    initinere.PercentualForaJornada = marc["InItinerePercForaJornada"] is DBNull ? 0 : Convert.ToDecimal(marc["InItinerePercForaJornada"]);
                    objTotalHoras.totalInItinere.Add(initinere);
                }
                foreach ((TipoDiaAcumulo, Dictionary<decimal, AcumuloPercentual>) acumulo in acumulosTotais)
                {
                    PercentualHorasExtras.TotalizarPercentuaisExtra(objTotalHoras, acumulo);
                }
                AtribuaTotais(objTotalHoras);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void TotalizaHorasExtrasClassificadas(Modelo.TotalHoras objTotalHoras)
        {
            try
            {
                BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new ClassificacaoHorasExtras(ConnectionString, UsuarioLogado);
                DataTable dtClass = bllClassificacaoHorasExtras.GetHorasExtrasClassificadasCalculo(Marcacoes.AsEnumerable().Select(s => s.Field<int>("id")).ToList());

                if (dtClass != null && dtClass.Rows.Count > 0)
                {
                    DataTable marcParaCalc = Marcacoes.Copy();
                    //marcParaCalc.AsEnumerable().ToList().ForEach(f => { f.SetField("horasextrasdiurna", "00:00"); f.SetField("horasextranoturna", "00:00"); });
                    for (int i = 0; i < marcParaCalc.Rows.Count; i++)
                    {
                        DataRow rowMarc = marcParaCalc.Rows[i];
                        DataRow rowClass = dtClass.AsEnumerable().Where(w => w.Field<int>("IdMarcacao") == (int)rowMarc["id"]).FirstOrDefault();
                        if (rowClass != null)
                        {
                            rowMarc["horasextrasdiurna"] = Modelo.cwkFuncoes.ConvertMinutosHora(((int)rowClass["ClassificadasDiurnaMin"]));
                            rowMarc["horasextranoturna"] = Modelo.cwkFuncoes.ConvertMinutosHora(((int)rowClass["ClassificadasNoturnaMin"]));
                        }
                    }

                    BLL.HoraExtra HE = new BLL.HoraExtra(marcParaCalc);
                    objTotalHoras.HorasExtrasDoPeriodo = HE.CalcularHoraExtraDiaria();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private static DateTime PegaData(DataRow marc)
        {
            DateTime data;
            if ((marc["data"] is DBNull) ||
                (marc["data"] == null))
            {
                data = new DateTime();
            }
            else
            {
                DateTime.TryParse(Convert.ToString(marc["data"]), out data);
            }
            return data;
        }

        private static int PegaIdHorario(DataRow marc)
        {
            int idhorario;
            if ((marc["idhorario"] is DBNull) ||
                (marc["idhorario"] == null))
            {
                idhorario = 0;
            }
            else
            {
                Int32.TryParse(Convert.ToString(marc["idhorario"]), out idhorario);
            }
            return idhorario;
        }

        public bool CarregaHorarioDetalhe(Modelo.HorarioDetalhe objHorarioDetalhe, DataRow marc)
        {
            bool retorno = true;
            if (!(marc["idJornadaSubstituir"] is DBNull))
            {
                int[] entradas = new int[4] { marc["entrada_1Substituido"].ToString().ConvertHorasMinuto(), marc["entrada_2Substituido"].ToString().ConvertHorasMinuto(), marc["entrada_3Substituido"].ToString().ConvertHorasMinuto(), marc["entrada_4Substituido"].ToString().ConvertHorasMinuto() };
                int[] saidas = new int[4] { marc["saida_1Substituido"].ToString().ConvertHorasMinuto(), marc["saida_2Substituido"].ToString().ConvertHorasMinuto(), marc["saida_3Substituido"].ToString().ConvertHorasMinuto(), marc["saida_4Substituido"].ToString().ConvertHorasMinuto() };
                int pHoraNoturnaI = marc["inicioadnoturno"].ToString().ConvertHorasMinuto();
                int pHoraNoturnaF = marc["fimadnoturno"].ToString().ConvertHorasMinuto();
                int pHoraD = 0;
                int pHoraN = 0;
                int adicionalNoturnoTolerancia = marc["toleranciaAdicionalNoturno"].ToString().ConvertHorasMinuto();

                BLL.CalculoHoras.QtdHorasDiurnaNoturna(entradas, saidas, pHoraNoturnaI, pHoraNoturnaF, adicionalNoturnoTolerancia, ref pHoraD, ref pHoraN);
                if (marc["marcacargahorariamista"].ToString() == "1")
                {
                    objHorarioDetalhe.Cargahorariamista = Modelo.cwkFuncoes.ConvertMinutosHora(pHoraD + pHoraN);
                    objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
                    objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";
                    objHorarioDetalhe.Flagfolga = marc["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolganormal"]);
                }
                else
                {
                    objHorarioDetalhe.Cargahorariamista = "--:--";
                    objHorarioDetalhe.Totaltrabalhadadiurna = Modelo.cwkFuncoes.ConvertMinutosHora(pHoraD);
                    objHorarioDetalhe.Totaltrabalhadanoturna = Modelo.cwkFuncoes.ConvertMinutosHora(pHoraN);
                    objHorarioDetalhe.Flagfolga = marc["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolganormal"]);
                }
            }
            else if (marc["legenda"].ToString() == "J")
            {
                Modelo.JornadaAlternativa objJornadaAlternativa = bllJornadaAlternativa.PossuiRegistro(Convert.ToDateTime(marc["data"]), Convert.ToInt32(marc["idempresa"]),
                    Convert.ToInt32(marc["iddepartamento"]), Convert.ToInt32(marc["idfuncionario"]), Convert.ToInt32(marc["idfuncao"]));
                if (objJornadaAlternativa != null)
                {
                    if (Convert.ToBoolean(objJornadaAlternativa.CargaMista))
                    {
                        objHorarioDetalhe.Cargahorariamista = objJornadaAlternativa.TotalMista;
                        objHorarioDetalhe.Totaltrabalhadadiurna = "--:--";
                        objHorarioDetalhe.Totaltrabalhadanoturna = "--:--";
                        objHorarioDetalhe.Flagfolga = marc["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolganormal"]);
                    }
                    else
                    {
                        objHorarioDetalhe.Cargahorariamista = "--:--";
                        objHorarioDetalhe.Totaltrabalhadadiurna = objJornadaAlternativa.TotalTrabalhadaDiurna;
                        objHorarioDetalhe.Totaltrabalhadanoturna = objJornadaAlternativa.TotalTrabalhadaNoturna;
                        objHorarioDetalhe.Flagfolga = marc["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolganormal"]);
                    }
                }
            }
            else
            {
                #region Carrega o Horário Detalhe

                if ((marc["tipohorario"] is DBNull ? 0 : Convert.ToInt32(marc["tipohorario"])) == 1)
                {
                    objHorarioDetalhe.Totaltrabalhadadiurna = marc["chdiurnanormal"] is DBNull ? "--:--" : marc["chdiurnanormal"].ToString();
                    objHorarioDetalhe.Totaltrabalhadanoturna = marc["chnoturnanormal"] is DBNull ? "--:--" : marc["chnoturnanormal"].ToString();
                    objHorarioDetalhe.Flagfolga = marc["flagfolganormal"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolganormal"]);
                    if (!(marc["cargamistanormal"] is DBNull))
                    {
                        objHorarioDetalhe.Cargahorariamista = marc["cargamistanormal"].ToString();
                        objHorarioDetalhe.CargahorariamistaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objHorarioDetalhe.Cargahorariamista);
                    }
                }
                else
                {
                    if (marc["chdiurnaflexivel"] is DBNull)
                    {
                        retorno = false;
                    }
                    else
                    {
                        objHorarioDetalhe.Totaltrabalhadadiurna = marc["chdiurnaflexivel"] is DBNull ? "--:--" : marc["chdiurnaflexivel"].ToString();
                        objHorarioDetalhe.Totaltrabalhadanoturna = marc["chnoturnaflexivel"] is DBNull ? "--:--" : marc["chnoturnaflexivel"].ToString();
                        objHorarioDetalhe.Flagfolga = marc["flagfolgaflexivel"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["flagfolgaflexivel"]);
                        if (!(marc["cargamistaflexivel"] is DBNull))
                        {
                            objHorarioDetalhe.Cargahorariamista = marc["cargamistaflexivel"].ToString();
                            objHorarioDetalhe.CargahorariamistaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objHorarioDetalhe.Cargahorariamista);
                        }
                    }
                }
                #endregion
            }
            if (retorno)
            {
                objHorarioDetalhe.TotaltrabalhadadiurnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objHorarioDetalhe.Totaltrabalhadadiurna);
                objHorarioDetalhe.TotaltrabalhadanoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objHorarioDetalhe.Totaltrabalhadanoturna);
            }
            return retorno;
        }

        public void CalculaFaltaeAtraso(Modelo.HorarioDetalhe objHorarioDetalhe, DataRow marc)
        {
            horasfaltasD = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horasfaltas"]);
            horasfaltasN = Modelo.cwkFuncoes.ConvertHorasMinuto((string)marc["horasfaltanoturna"]);

            if (!(String.IsNullOrEmpty(objHorarioDetalhe.Cargahorariamista) || objHorarioDetalhe.Cargahorariamista == "--:--"))
            {
                var cargaMistaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objHorarioDetalhe.Cargahorariamista);
                if (cargaMistaMin == (horasfaltasD + horasfaltasN))
                {
                    totalFaltas++;
                }

                if ((horasfaltasD + horasfaltasN) < cargaMistaMin)
                {
                    totalAtrasoDiurno += horasfaltasD + horasfaltasN;
                }
                totalFaltaDiurna += horasfaltasD + horasfaltasN;
            }
            else
            {
                if (((horasfaltasD == objHorarioDetalhe.TotaltrabalhadadiurnaMin
                && horasfaltasN == objHorarioDetalhe.TotaltrabalhadanoturnaMin)
                && ((objHorarioDetalhe.Totaltrabalhadanoturna != "--:--" && !String.IsNullOrEmpty(objHorarioDetalhe.Totaltrabalhadanoturna))
                || (objHorarioDetalhe.Totaltrabalhadadiurna != "--:--" && !String.IsNullOrEmpty(objHorarioDetalhe.Totaltrabalhadadiurna)))))
                {
                    totalFaltas++;
                }

                if ((horasfaltasD + horasfaltasN) < (objHorarioDetalhe.TotaltrabalhadadiurnaMin + objHorarioDetalhe.TotaltrabalhadanoturnaMin))
                {
                    totalAtrasoDiurno += horasfaltasD;
                    totalAtrasoNoturno += horasfaltasN;
                }

                totalFaltaDiurna += horasfaltasD;
                totalFaltaNoturna += horasfaltasN;
            }

        }

        private void InicializeTotalizadores()
        {
            for (int i = 0; i < 10; i++)
            {
                listTotalHorasGeral[i, 0] = 0;
                listTotalHorasGeral[i, 1] = 0;
                listTotalHorasGeral[i, 2] = 0;

                listTotalHoras[i, 0] = 0;
                listTotalHoras[i, 1] = 0;
                listTotalHoras[i, 2] = 0;
            }
            totalExtraDiurna = 0; totalExtraNoturna = 0; totalFaltaDiurna = 0; totalFaltaNoturna = 0; totalExtraNoturnaBH = 0;
            totalTrabDiurna = 0; totalTrabNoturna = 0; totalDDSR = 0; qtdDDSR = 0; totalAtrasoDiurno = 0; totalAtrasoNoturno = 0;
            totalFaltas = 0; horasfaltasD = 0; horasfaltasN = 0; totalHorasInItinerePercDentroJornada = 0; totalHorasInItinerePercForaJornada = 0; totalAdNoturno = 0;
        }

        private void TotalizeHorasAfastamento(Modelo.TotalHoras objTotalHoras, Modelo.HorarioDetalhe objHorarioDetalhe, DateTime data)
        {
            foreach (var item in objTotalHoras.EventosAfastamentos)
            {
                var afastamentosData = new List<Modelo.Afastamento>();
                if (item.Evento.HorasAbonadas == 1)
                {
                    afastamentosData.AddRange(afastamentosAbsenteismo.Where(a => AfastamentoEhDessaMarcacao(data, a)));
                }
                afastamentosData.AddRange(afastamentosSelecionados.Where(a => AfastamentoEhDessaMarcacao(data, a)
                                                                        && item.Evento.GetIdsOcorrencias().Contains(a.IdOcorrencia)
                                                                        && afastamentosData.Where(ad => ad.Id == a.Id).Count() == 0));

                foreach (var afastamento in afastamentosData)
                {
                    if (afastamento.Parcial == 1)
                    {
                        int abonoDiurno = afastamento.Horai.ConvertHorasMinuto();
                        int abonoNoturno = afastamento.Horaf.ConvertHorasMinuto();
                        int abonoD;
                        int abonoN;

                        if (objHorarioDetalhe.CargahorariamistaMin > 0)
                        {
                            abonoD = abonoDiurno;
                            abonoN = abonoNoturno;
                        }
                        else
                        {
                            abonoD = Math.Min(abonoDiurno, objHorarioDetalhe.TotaltrabalhadadiurnaMin);
                            abonoN = Math.Min(abonoNoturno, objHorarioDetalhe.TotaltrabalhadanoturnaMin);
                        }

                        item.TotalHoras += abonoD;
                        item.TotalHoras += abonoN;
                    }
                    else if (afastamento.Abonado == 1)
                    {
                        item.TotalHoras += objHorarioDetalhe.TotaltrabalhadadiurnaMin + objHorarioDetalhe.TotaltrabalhadanoturnaMin + objHorarioDetalhe.CargahorariamistaMin;
                    }
                }
            }
        }

        private bool AfastamentoEhDessaMarcacao(DateTime data, Modelo.Afastamento a)
        {
            return data >= a.Datai && data <= (a.Dataf == null ? DateTime.MaxValue : a.Dataf)
                   && (
                       (a.Tipo == 0 && a.IdFuncionario == idFuncionario) ||
                       (a.Tipo == 1 && a.IdDepartamento == idDepartamento) ||
                       (a.Tipo == 2 && a.IdEmpresa == idEmpresa)
                   );
        }

        private void AtribuaTotais(Modelo.TotalHoras objTotalHoras)
        {
            objTotalHoras.horasExtraDiurna = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalExtraDiurna);
            objTotalHoras.horasExtraDiurnaMin = totalExtraDiurna;
            objTotalHoras.horasExtraNoturna = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalExtraNoturna);
            objTotalHoras.horasExtraNoturnaMin = totalExtraNoturna;
            objTotalHoras.horasFaltaDiurna = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalFaltaDiurna);
            objTotalHoras.horasFaltaDiurnaMin = totalFaltaDiurna;
            objTotalHoras.horasFaltaNoturna = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalFaltaNoturna);
            objTotalHoras.horasFaltaNoturnaMin = totalFaltaNoturna;
            objTotalHoras.horasTrabDiurna = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalTrabDiurna);
            objTotalHoras.horasTrabDiurnaMin = totalTrabDiurna;
            objTotalHoras.horasTrabNoturna = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalTrabNoturna);
            objTotalHoras.horasTrabNoturnaMin = totalTrabNoturna;
            objTotalHoras.horasAdNoturno = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalAdNoturno);
            objTotalHoras.horasAdNoturnoMin = totalAdNoturno;
            objTotalHoras.PercAdicNoturno = percAdicNoturno;
            objTotalHoras.horasDDSR = Modelo.cwkFuncoes.ConvertMinutosHora2(3, totalDDSR);
            objTotalHoras.horasDDSRMin = totalDDSR;
            objTotalHoras.qtdDDSR = qtdDDSR;
            totalDiasAbonados = objTotalHoras.totalAbonoDias;
            totalMinutosAbono = objTotalHoras.totalAbonoMinutos;
            totalDiasDSR = objTotalHoras.totalDSRDias;
            totalMinutosDSR = objTotalHoras.totalDSRMinutos;
            objTotalHoras.FaltasDias = totalFaltas;
            objTotalHoras.FaltasCompletasDiurnasMin = totalFaltaDiurna - totalAtrasoDiurno;
            objTotalHoras.FaltasCompletasNoturnasMin = totalFaltaNoturna - totalAtrasoNoturno;
            if (CalcularAtraso)
            {
                objTotalHoras.atrasoDMin = totalAtrasoDiurno;
                objTotalHoras.atrasoNMin = totalAtrasoNoturno;
            }

            objTotalHoras.horasextranoturnaBHMin = totalExtraNoturnaBH;
            objTotalHoras.horasExtraInterjornada = Modelo.cwkFuncoes.ConvertMinutosHora(3, totalInterjornadaExtra);
            objTotalHoras.horasExtraInterjornadaMin = totalInterjornadaExtra;
            objTotalHoras.qtdAdNot = qtdAdNot;
        }

        private void TotalizePercentuaisExtra(ref DateTime pDataF, int[,] listTotalHorasGeral, int[,] listTotalHoras, PercentualHoraExtra[] HorariosPHExtra, Modelo.HorarioDetalhe objHorarioDetalhe, DataRow marc, ref DateTime data, ref int indice, ref int diaFinalSemana, bool trocaMes, int dia, int idhorario, int horaExtraNoturna, int horaExtraDiurna, int totalExtra)
        {
            int consideraSabadoSemana = Convert.ToInt32(marc["considerasabadosemana"]);
            int consideraDomingoSemana = Convert.ToInt32(marc["consideradomingosemana"]);
            int tipoAcumulo = Convert.ToInt32(marc["tipoacumulo"]);

            if ((string)marc["legenda"] == "F")
            {
                indice = 8;
            }
            else if (objHorarioDetalhe.Flagfolga == 1 || (marc["folga"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(marc["folga"])) == 1)//Folga
            {
                indice = 9;
            }
            else if ((string)marc["dia"] == "Sáb." && consideraSabadoSemana == 0)
            {
                indice = 6;
            }
            else if ((string)marc["dia"] == "Dom." && consideraDomingoSemana == 0)
            {
                indice = 7;
            }
            else
            {
                indice = 0;
            }

            if (consideraDomingoSemana == 1)
            {
                diaFinalSemana = 7;
            }
            else
            {
                if (consideraSabadoSemana == 1)
                {
                    diaFinalSemana = 6;
                }
                else
                {
                    diaFinalSemana = 5;
                }
            }

            int[] HED = new int[10];
            int[] HEN = new int[10];

            if (idhorario != 0)
            {
                int tipo = 0;

                if (tipoAcumulo == 3 || tipoAcumulo == 2)
                {
                    listTotalHoras[indice, 0] += horaExtraDiurna;
                    listTotalHoras[indice, 1] += horaExtraNoturna;
                    listTotalHoras[indice, 2] += totalExtra;
                    tipo = tipoAcumulo;
                }
                else
                {
                    listTotalHoras[indice, 0] = horaExtraDiurna;
                    listTotalHoras[indice, 1] = horaExtraNoturna;
                    listTotalHoras[indice, 2] = totalExtra;
                    tipo = 1;
                }

                int indiceLimite = 0;

                if ((tipo == 2 && dia == diaFinalSemana) || (tipo == 3 && trocaMes) || (tipo == 1) || (data == pDataF))
                {
                    for (int i = 0; i < HED.Length; i++)
                    {
                        HED[i] = 0;
                        HEN[i] = 0;
                    }

                    for (int a = 0; a < 10; a++)
                    {
                        if (a == 0)
                        {
                            indice = 0;
                            indiceLimite = (diaFinalSemana == 7 ? 7 : (diaFinalSemana == 6 ? 6 : 5));
                        }
                        else if (a == 6 && consideraSabadoSemana == 0)
                        {
                            indice = 6;
                            indiceLimite = 6;
                        }
                        else if (a == 7 && consideraDomingoSemana == 0)
                        {
                            indice = 7;
                            indiceLimite = 7;
                        }
                        else if (a > 7)
                        {
                            indice = a;
                            indiceLimite = a;
                        }
                        else
                        {
                            continue;
                        }


                        for (int k = indice; k <= indiceLimite; k++) //50 / 60 / 70 / 80 / 90 / 100
                        {
                            if (HorariosPHExtra[k].PercentualExtra > 0)
                            {
                                int quantidadeExtra = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[k].QuantidadeExtra);

                                //Diurna
                                if (listTotalHoras[indice, 0] + HED[k] <= quantidadeExtra)
                                {
                                    HED[k] += listTotalHoras[indice, 0];
                                    quantidadeExtra -= listTotalHoras[indice, 0];
                                    listTotalHoras[indice, 2] -= listTotalHoras[indice, 0];
                                    listTotalHoras[indice, 0] = 0;
                                }
                                else
                                {
                                    HED[k] = quantidadeExtra;
                                    listTotalHoras[indice, 0] -= quantidadeExtra;
                                    listTotalHoras[indice, 2] -= quantidadeExtra;
                                    quantidadeExtra = 0;
                                }

                                //Noturna
                                if (listTotalHoras[indice, 1] + HEN[k] <= quantidadeExtra)
                                {
                                    HEN[k] += listTotalHoras[indice, 1];
                                    listTotalHoras[indice, 2] -= listTotalHoras[indice, 1];
                                    listTotalHoras[indice, 1] = 0;
                                }
                                else
                                {
                                    HEN[k] = quantidadeExtra;
                                    listTotalHoras[indice, 1] -= quantidadeExtra;
                                    listTotalHoras[indice, 2] -= quantidadeExtra;
                                }
                            }

                        }
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        listTotalHoras[i, 0] = 0;
                        listTotalHoras[i, 1] = 0;
                        listTotalHoras[i, 2] = 0;
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                listTotalHorasGeral[i, 0] += HED[i];
                listTotalHorasGeral[i, 1] += HEN[i];
                listTotalHorasGeral[i, 2] += HED[i] + HEN[i];

                HED[i] = 0;
                HEN[i] = 0;
            }
        }

        public static void AtribuaPercentuaisExtra(PercentualHoraExtra[] HorariosPHExtra, DataRow marc)
        {
            for (int i = 0; i < HorariosPHExtra.Length; i++)
            {
                HorariosPHExtra[i].SeparaExtraNoturnaPercentual = marc["SeparaExtraNoturnaPercentual"] is DBNull ? false : Convert.ToBoolean(marc["SeparaExtraNoturnaPercentual"]);
            }

            HorariosPHExtra[0].PercentualExtra = Convert.ToDecimal(marc["percentualextra50"]);
            HorariosPHExtra[0].QuantidadeExtra = Convert.ToString(marc["quantidadeextra50"]);
            HorariosPHExtra[0].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[0].QuantidadeExtra);
            HorariosPHExtra[0].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro1"]);
            HorariosPHExtra[0].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo1"]);
            if (!(marc["percentualExtraNoturna50"] is DBNull))
                HorariosPHExtra[0].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturna50"]);
            HorariosPHExtra[0].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturna50"]);
            HorariosPHExtra[0].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[0].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna1"] is DBNull))
                HorariosPHExtra[0].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna1"]);

            HorariosPHExtra[1].PercentualExtra = Convert.ToDecimal(marc["percentualextra60"]);
            HorariosPHExtra[1].QuantidadeExtra = Convert.ToString(marc["quantidadeextra60"]);
            HorariosPHExtra[1].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[1].QuantidadeExtra);
            HorariosPHExtra[1].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro2"]);
            HorariosPHExtra[1].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo2"]);
            if (!(marc["percentualExtraNoturna60"] is DBNull))
                HorariosPHExtra[1].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturna60"]);
            HorariosPHExtra[1].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturna60"]);
            HorariosPHExtra[1].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[1].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna2"] is DBNull))
                HorariosPHExtra[1].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna2"]);

            HorariosPHExtra[2].PercentualExtra = Convert.ToDecimal(marc["percentualextra70"]);
            HorariosPHExtra[2].QuantidadeExtra = Convert.ToString(marc["quantidadeextra70"]);
            HorariosPHExtra[2].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[2].QuantidadeExtra);
            HorariosPHExtra[2].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro3"]);
            HorariosPHExtra[2].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo3"]);
            if (!(marc["percentualExtraNoturna70"] is DBNull))
                HorariosPHExtra[2].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturna70"]);
            HorariosPHExtra[2].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturna70"]);
            HorariosPHExtra[2].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[2].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna3"] is DBNull))
                HorariosPHExtra[2].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna3"]);

            HorariosPHExtra[3].PercentualExtra = Convert.ToDecimal(marc["percentualextra80"]);
            HorariosPHExtra[3].QuantidadeExtra = Convert.ToString(marc["quantidadeextra80"]);
            HorariosPHExtra[3].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[3].QuantidadeExtra);
            HorariosPHExtra[3].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro4"]);
            HorariosPHExtra[3].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo4"]);
            if (!(marc["percentualExtraNoturna80"] is DBNull))
                HorariosPHExtra[3].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturna80"]);
            HorariosPHExtra[3].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturna80"]);
            HorariosPHExtra[3].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[3].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna4"] is DBNull))
                HorariosPHExtra[3].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna4"]);

            HorariosPHExtra[4].PercentualExtra = Convert.ToDecimal(marc["percentualextra90"]);
            HorariosPHExtra[4].QuantidadeExtra = Convert.ToString(marc["quantidadeextra90"]);
            HorariosPHExtra[4].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[4].QuantidadeExtra);
            HorariosPHExtra[4].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro5"]);
            HorariosPHExtra[4].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo5"]);
            if (!(marc["percentualExtraNoturna90"] is DBNull))
                HorariosPHExtra[4].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturna90"]);
            HorariosPHExtra[4].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturna90"]);
            HorariosPHExtra[4].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[4].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna5"] is DBNull))
                HorariosPHExtra[4].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna5"]);

            HorariosPHExtra[5].PercentualExtra = Convert.ToDecimal(marc["percentualextra100"]);
            HorariosPHExtra[5].QuantidadeExtra = Convert.ToString(marc["quantidadeextra100"]);
            HorariosPHExtra[5].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[5].QuantidadeExtra);
            HorariosPHExtra[5].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro6"]);
            HorariosPHExtra[5].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo6"]);
            if (!(marc["percentualExtraNoturna100"] is DBNull))
                HorariosPHExtra[5].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturna100"]);
            HorariosPHExtra[5].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturna100"]);
            HorariosPHExtra[5].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[5].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna6"] is DBNull))
                HorariosPHExtra[5].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna6"]);

            HorariosPHExtra[6].PercentualExtra = Convert.ToDecimal(marc["percentualextrasab"]);
            HorariosPHExtra[6].QuantidadeExtra = Convert.ToString(marc["quantidadeextrasab"]);
            HorariosPHExtra[6].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[6].QuantidadeExtra);
            HorariosPHExtra[6].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro7"]);
            HorariosPHExtra[6].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo7"]);
            if (!(marc["percentualExtraNoturnasab"] is DBNull))
                HorariosPHExtra[6].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturnasab"]);
            HorariosPHExtra[6].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturnasab"]);
            HorariosPHExtra[6].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[6].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna7"] is DBNull))
                HorariosPHExtra[6].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna7"]);

            HorariosPHExtra[7].PercentualExtra = Convert.ToDecimal(marc["percentualextradom"]);
            HorariosPHExtra[7].QuantidadeExtra = Convert.ToString(marc["quantidadeextradom"]);
            HorariosPHExtra[7].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[7].QuantidadeExtra);
            HorariosPHExtra[7].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro8"]);
            HorariosPHExtra[7].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo8"]);
            if (!(marc["percentualExtraNoturnadom"] is DBNull))
                HorariosPHExtra[7].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturnadom"]);
            HorariosPHExtra[7].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturnadom"]);
            HorariosPHExtra[7].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[7].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna8"] is DBNull))
                HorariosPHExtra[7].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna8"]);

            HorariosPHExtra[8].PercentualExtra = Convert.ToDecimal(marc["percentualextrafer"]);
            HorariosPHExtra[8].QuantidadeExtra = Convert.ToString(marc["quantidadeextrafer"]);
            HorariosPHExtra[8].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[8].QuantidadeExtra);
            HorariosPHExtra[8].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro9"]);
            HorariosPHExtra[8].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo9"]);
            if (!(marc["percentualExtraNoturnafer"] is DBNull))
                HorariosPHExtra[8].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturnafer"]);
            HorariosPHExtra[8].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturnafer"]);
            HorariosPHExtra[8].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[8].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna9"] is DBNull))
                HorariosPHExtra[8].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna9"]);

            HorariosPHExtra[9].PercentualExtra = Convert.ToDecimal(marc["percentualextrafol"]);
            HorariosPHExtra[9].QuantidadeExtra = Convert.ToString(marc["quantidadeextrafol"]);
            HorariosPHExtra[9].QuantidadeExtraMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[9].QuantidadeExtra);
            HorariosPHExtra[9].PercentualExtraSegundo = Convert.ToDecimal(marc["percextraprimeiro10"]);
            HorariosPHExtra[9].TipoAcumulo = Convert.ToInt16(marc["tipoacumulo10"]);
            if (!(marc["percentualExtraNoturnafol"] is DBNull))
                HorariosPHExtra[9].PercentualExtraNoturna = Convert.ToDecimal(marc["percentualExtraNoturnafol"]);
            HorariosPHExtra[9].QuantidadeExtraNoturna = Convert.ToString(marc["quantidadeExtraNoturnafol"]);
            HorariosPHExtra[9].QuantidadeExtraNoturnaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(HorariosPHExtra[9].QuantidadeExtraNoturna);
            if (!(marc["percextraprimeiroNoturna10"] is DBNull))
                HorariosPHExtra[9].PercentualExtraSegundoNoturna = Convert.ToDecimal(marc["percextraprimeiroNoturna10"]);
        }

        public void TotalizeGruposInItinere(Modelo.TotalHoras objTotalHoras)
        {

            var percentuais = (from percentual in objTotalHoras.totalInItinere select percentual.PercentualDentroJornada)
                                .Union(from percentual in objTotalHoras.totalInItinere select percentual.PercentualForaJornada)
                                .Distinct().OrderBy(x => x).ToList();
            objTotalHoras.GruposPercentual = new List<GrupoHorasPorPercentual>();
            foreach (var percentual in percentuais)
            {
                var grupo = objTotalHoras.GruposPercentual.Find(g => g.Percentual == percentual);
                if (grupo == null)
                {
                    grupo = new GrupoHorasPorPercentual();
                    grupo.Percentual = percentual;
                    grupo.TempoTotal = new TimeSpan();
                    objTotalHoras.GruposPercentual.Add(grupo);
                }
                var totalDentro = (from tempoDentro in objTotalHoras.totalInItinere
                                   where tempoDentro.PercentualDentroJornada == percentual
                                   select tempoDentro.MinutosDentroJornada).Sum();
                var totalFora = (from tempoFora in objTotalHoras.totalInItinere
                                 where tempoFora.PercentualForaJornada == percentual
                                 select tempoFora.MinutosForaJornada).Sum();
                grupo.TempoTotal = grupo.TempoTotal
                                   .Add(TimeSpan.FromMinutes(totalDentro))
                                   .Add(TimeSpan.FromMinutes(totalFora));
            }
            objTotalHoras.GruposPercentual = objTotalHoras.GruposPercentual.Where(grupo => grupo.TempoTotal.TotalMilliseconds > 0).ToList();
        }
    }
}
