using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RelatorioOcorrenciaPontoWeb
    {
        private DateTime _DataInicial;
        private DateTime _DataFinal;
        private int _Tipo;
        private string _IdTipo;
        private int _IdOcorrencia;
        private int _ModoOrdenacao;
        private int _AgrupaDepartamento;
        private bool[] _ListaOcorrencias;

        private BLL.Marcacao bllMarcacao;
        private BLL.Afastamento bllAfastamento;
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.Justificativa bllJustificativa;
        private BLL.Funcionario bllFuncionario;
        private BLL.JornadaAlternativa bllJornadaAlternativa;

        string ocorrencia, empresa, cnpj_cpf, departamento, funcionario
        , dscodigo, marcacoes, matricula, cpf, competencia, observacao, idDocumentoWorkflow;
        int idMarcacao;

        DateTime data;
        string dia;

        string bancohorasdeb, ocorrenciaMarcacao;
        int idFuncionario, idDepartamento, idEmpresa;

        int tipoHorario = 0;
        int idHorarioDetalhe = 0;
        int toleranciaFalta = 0;
        string[] entradas_jornada = new string[4];
        string[] saidas_jornada = new string[4];
        string[] entradas = new string[8];
        string[] saidas = new string[8];
        bool carregouDados = false;
        private List<Modelo.Ocorrencia> OcorrenciasSelecionadas;
        private List<Modelo.Justificativa> JustificativasSelecionadas;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        string horasextradiurna, horasextranoturna, descricao;
        public RelatorioOcorrenciaPontoWeb(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIdTipo, int pIdOcorrencia, int pModoOrdenacao, bool pAgrupaDepartamento, IList<bool> pegaOcorrencias,
                                            string listaDeOcorrencias, string listaDeJustificativas, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            OcorrenciasSelecionadas = new List<Modelo.Ocorrencia>();
            JustificativasSelecionadas = new List<Modelo.Justificativa>();
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;

            _DataInicial = pDataInicial;
            _DataFinal = pDataFinal;
            _Tipo = pTipo;
            _IdTipo = pIdTipo;
            _IdOcorrencia = pIdOcorrencia;
            _ModoOrdenacao = pModoOrdenacao;

            #region parâmetros do checkbox

            #endregion

            _ListaOcorrencias = pegaOcorrencias.ToArray();

            if (pAgrupaDepartamento)
            {
                _AgrupaDepartamento = 1;
            }
            else
            {
                _AgrupaDepartamento = 0;
            }


            bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            bllAfastamento = new Afastamento(ConnectionString, UsuarioLogado);
            bllOcorrencia = new Ocorrencia(ConnectionString, UsuarioLogado);
            bllJustificativa = new Justificativa(ConnectionString, UsuarioLogado);

            if (!string.IsNullOrEmpty(listaDeOcorrencias))
            {
                OcorrenciasSelecionadas = bllOcorrencia.GetAllListPorIds(listaDeOcorrencias.Split(',').Select(Int32.Parse).ToList());
            }

            if (!string.IsNullOrEmpty(listaDeJustificativas))
            {
                JustificativasSelecionadas = bllJustificativa.GetAllListPorIds(listaDeJustificativas.Split(',').Select(Int32.Parse).ToList());
            }

            bllFuncionario = new BLL.Funcionario(ConnectionString, UsuarioLogado);
            bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);
        }

        public DataTable GeraRelatorio()
        {
            DataTable ret = new DataTable();

            DataColumn[] colunas = new DataColumn[]
            {
                new DataColumn("id"),
                new DataColumn("empresa"),
                new DataColumn("cnpj_cpf"),
                new DataColumn("departamento"),
                new DataColumn("dscodigo"),
                new DataColumn("funcionario"),
                new DataColumn("data") { DataType = typeof(DateTime) },
                new DataColumn("dia"),
                new DataColumn("ocorrencia"),
                new DataColumn("marcacoes"),
                new DataColumn("bancohorasdeb"),
                new DataColumn("horasextradiurna"),
                new DataColumn("horasextranoturna"),
                new DataColumn("horastrabalhadas"),
                new DataColumn("horastrabalhadasnoturnas"),
                new DataColumn("Matricula"),
                new DataColumn("CPF"),
                new DataColumn("Observacao"),
                new DataColumn("Competencia"),
                new DataColumn("IdDocumentoWorkflow"),
                new DataColumn("nomeRelatorio")
            };

            ret.Columns.AddRange(colunas);

            //Carrega as informações
            DataTable listaMarcacao = bllMarcacao.GetParaRelatorioOcorrencia(_Tipo, _IdTipo, _DataInicial, _DataFinal, _ModoOrdenacao, _AgrupaDepartamento);

            List<Modelo.Afastamento> listaAfastamentos = null;
            List<Modelo.Ocorrencia> listaOcorrencias = null;
            Modelo.Ocorrencia objOcorrencia = null;
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            List<Modelo.JornadaAlternativa> listaJornadaAlternativa = null;
            List<Modelo.Justificativa> ListaJustificativas = bllJustificativa.GetAllList(false);

            foreach (DataRow marc in listaMarcacao.Rows)
            {
                carregouDados = false;
                ocorrenciaMarcacao = marc["ocorrencia"].ToString().Trim();
                data = Convert.ToDateTime(marc["data"]);
                dia = marc["dia"].ToString().Trim();
                if (_ListaOcorrencias[0]) //Entrada Atrasada
                {
                    EntradaAtrasada(ret, marc);
                }

                if (_ListaOcorrencias[1]) //Saida Antecipada
                {
                    SaidaAntecipada(ret, marc);
                }

                if (_ListaOcorrencias[2] || _ListaOcorrencias[7]) //Falta ou Atraso
                {
                    if (listaJornadaAlternativa == null)
                    {
                        listaJornadaAlternativa = bllJornadaAlternativa.GetPeriodo(_DataInicial, _DataFinal);
                    }
                    if (objFuncionario.Id != Convert.ToInt32(marc["idfuncionario"]))
                    {
                        objFuncionario = bllFuncionario.LoadObject(Convert.ToInt32(marc["idfuncionario"]));
                    }

                    var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(objFuncionario.Idempresa, objFuncionario.Iddepartamento, objFuncionario.Id, objFuncionario.Idfuncao, _DataInicial, _DataFinal, listaJornadaAlternativa, listaMarcacao, null, null, ConnectionString, UsuarioLogado);
                    Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(_DataInicial, _DataFinal);
                    totalizadorHoras.CalculaFaltaeAtraso(objTotalHoras, marc);
                    if (_ListaOcorrencias[2])
                    {
                        InsereOcorrenciaFalta(ret, entradas, saidas, marc, objTotalHoras);
                    }
                    if (_ListaOcorrencias[7])
                    {
                        InsereOcorrenciaAtraso(ret, entradas, saidas, marc, objTotalHoras);
                    }
                }

                if (_ListaOcorrencias[3])
                {
                    bancohorasdeb = marc["bancohorasdeb"].ToString();
                    int bancohorasdebInt = Modelo.cwkFuncoes.ConvertHorasMinuto(bancohorasdeb);
                    if (bancohorasdebInt > 0) //Debito Banco de Horas
                    {
                        ocorrencia = "Débito Banco de Horas";
                        InsereOcorrenciaGeral(ret, entradas, saidas, marc);
                    }
                }

                if (_ListaOcorrencias[4] && marc["legenda"].ToString().Trim() == "A") //Ocorrencia
                {
                    if (listaAfastamentos == null)
                    {
                        listaAfastamentos = bllAfastamento.GetPeriodo(_DataInicial, _DataFinal);
                    }
                    if (OcorrenciasSelecionadas != null)
                    {
                        if (OcorrenciasSelecionadas.Count > 0)
                        {
                            listaAfastamentos = listaAfastamentos.Where(w => OcorrenciasSelecionadas.Select(s => s.Id).Contains(w.IdOcorrencia)).ToList();
                        }
                    }

                    SetaVariaveisFuncionario(marc);
                    if (_IdOcorrencia > 0) //Uma Ocorrência
                    {
                        Modelo.Afastamento afastamentosInserir = listaAfastamentos.Where(a => (a.IdOcorrencia == _IdOcorrencia)
                            && ((a.Tipo == 0 && a.IdFuncionario == idFuncionario)
                            || (a.Tipo == 1 && a.IdDepartamento == idDepartamento)
                            || (a.Tipo == 2 && a.IdEmpresa == idEmpresa))
                            && data >= a.Datai && data <= (a.Dataf == null ? DateTime.MaxValue : a.Dataf)).FirstOrDefault();
                        if (afastamentosInserir != null)
                        {
                            listaOcorrencias = AtribuiOcorrencias(listaOcorrencias);

                            if (objOcorrencia == null)
                            {
                                objOcorrencia = listaOcorrencias.Where(o => o.Id == _IdOcorrencia).First();
                            }
                            else if (objOcorrencia.Id != _IdOcorrencia)
                            {
                                objOcorrencia = listaOcorrencias.Where(o => o.Id == _IdOcorrencia).First();
                            }
                            ocorrencia = objOcorrencia.Descricao;
                            InsereOcorrenciaAfastamento(ret, entradas, saidas, marc, afastamentosInserir);
                        }
                    }
                    else //Todas Ocorrências
                    {
                        if (OcorrenciasSelecionadas != null)
                        {
                            if (OcorrenciasSelecionadas.Count == 0)
                            {
                                listaOcorrencias = bllOcorrencia.GetAllList(false);
                            }
                            else
                            {
                                listaOcorrencias = OcorrenciasSelecionadas;
                            }
                        }
                        List<Modelo.Afastamento> afastamentosInserir = listaAfastamentos.Where(a =>
                            ((a.Tipo == 0 && a.IdFuncionario == idFuncionario)
                            || (a.Tipo == 1 && a.IdDepartamento == idDepartamento)
                            || (a.Tipo == 2 && a.IdEmpresa == idEmpresa))
                            && data >= a.Datai && data <= (a.Dataf == null ? DateTime.MaxValue : a.Dataf)).ToList();

                        foreach (Modelo.Afastamento afas in afastamentosInserir)
                        {
                            objOcorrencia = listaOcorrencias.Where(o => o.Id == afas.IdOcorrencia).First();
                            ocorrencia = objOcorrencia.Descricao;
                            InsereOcorrenciaAfastamento(ret, entradas, saidas, marc, afas);
                        }
                    }
                }

                if (_ListaOcorrencias[5] && ocorrenciaMarcacao == "Marcações Incorretas") //Marcacoes Incorretas
                {
                    ocorrencia = "Marcações Incorretas";
                    InsereOcorrenciaGeral(ret, entradas, saidas, marc);
                }

                if (_ListaOcorrencias[6] && ((marc["hr_extra_diurna"].ToString().Trim() != "--:--") || marc["hr_extra_noturna"].ToString().Trim() != "--:--"))
                {
                    ocorrencia = "Horas Extras";
                    InsereOcorrenciaGeral(ret, entradas, saidas, marc);

                }
                if (_ListaOcorrencias[8]) //Manutenções Manuais
                {
                    foreach (var jsel in JustificativasSelecionadas)
                    {
                        CarregaDados(marc);

                        var hasJust = false;
                        for (var x = 1; x <= 8; x++)
                        {
                            int idJust;
                            if (Int32.TryParse(marc["justif_ent_" + x].ToString(), out idJust) && jsel.Id == idJust)
                            {
                                hasJust = true;
                            }
                            else
                                entradas[x - 1] = "--:--";

                            if (Int32.TryParse(marc["justif_sai_" + x].ToString(), out idJust) && jsel.Id == idJust)
                            {
                                hasJust = true;
                            }
                            else
                                saidas[x - 1] = "--:--";
                        }
                        if (hasJust)
                        {
                            var justificativa = ListaJustificativas.Find(ju => ju.Id == jsel.Id);
                            ocorrencia = justificativa.Descricao;
                            InsereOcorrenciaGeral(ret, entradas, saidas, marc);
                        }
                    }

                    //List<Int32> justificativas = new List<int>();

                    //for (var x = 1; x <= 8; x++)
                    //{
                    //    int idJust;
                    //    if (Int32.TryParse(marc["justif_ent_" + x].ToString(), out idJust) && JustificativasSelecionadas.Any(j => j.Id == idJust))
                    //    {
                    //        justificativas.Add(idJust);
                    //    }
                    //    else
                    //        entradas[x - 1] = "--:--";



                    //    if (Int32.TryParse(marc["justif_sai_" + x].ToString(), out idJust) && JustificativasSelecionadas.Any(j => j.Id == idJust))
                    //    {
                    //        justificativas.Add(idJust);
                    //    }
                    //    else
                    //        saidas[x - 1] = "--:--";
                    //}

                    //foreach (var justificativa in ListaJustificativas.Where(ju => justificativas.Any(jid => jid == ju.Id)))
                    //{
                    //    ocorrencia = justificativa.Descricao;
                    //    InsereOcorrenciaGeral(ret, entradas, saidas, marc);
                    //}

                }
            }

            return ret;
        }

        private void SetaVariaveisFuncionario(DataRow marc)
        {
            idFuncionario = Convert.ToInt32(marc["idfuncionario"]);
            idDepartamento = Convert.ToInt32(marc["iddepartamento"]);
            idEmpresa = Convert.ToInt32(marc["idempresa"]);
        }

        private void SaidaAntecipada(DataTable pRet, DataRow pMarc)
        {
            if (!carregouDados)
            {
                if (!CarregaDados(pMarc))
                    return;
            }

            int tamanho = Math.Min(saidas.Length, saidas_jornada.Length);
            for (int i = 0; i < tamanho; i++)
            {
                if (saidas[i] == "--:--" || saidas_jornada[i] == "--:--")
                {
                    continue;
                }
                if (Modelo.cwkFuncoes.ConvertHorasMinuto(saidas_jornada[i]) > Modelo.cwkFuncoes.ConvertHorasMinuto(saidas[i]) + toleranciaFalta)
                {
                    ocorrencia = "Saída Antecipada";
                    InsereOcorrenciaGeral(pRet, entradas, saidas, pMarc);
                    break;
                }
            }
        }

        private void EntradaAtrasada(DataTable pRet, DataRow pMarc)
        {
            if (!CarregaDados(pMarc))
                return;

            int tamanho = Math.Min(entradas.Length, entradas_jornada.Length);
            for (int i = 0; i < tamanho; i++)
            {
                if (entradas[i] == "--:--" || entradas_jornada[i] == "--:--")
                {
                    continue;
                }
                if (Modelo.cwkFuncoes.ConvertHorasMinuto(entradas_jornada[i]) + toleranciaFalta < Modelo.cwkFuncoes.ConvertHorasMinuto(entradas[i]))
                {
                    ocorrencia = "Entrada Atrasada";
                    InsereOcorrenciaGeral(pRet, entradas, saidas, pMarc);
                    break;
                }
            }
        }

        private bool CarregaDados(DataRow pMarc)
        {
            carregouDados = true;
            bool ret = true;
            if (!SetaVariaveisHorarioDetalhe(pMarc))
                ret = false;

            SetaEntradasSaidas(pMarc);

            return ret;
        }

        private void SetaEntradasSaidas(DataRow marc)
        {
            entradas[0] = Convert.ToString(marc["entrada_1"]);
            entradas[1] = Convert.ToString(marc["entrada_2"]);
            entradas[2] = Convert.ToString(marc["entrada_3"]);
            entradas[3] = Convert.ToString(marc["entrada_4"]);
            entradas[4] = Convert.ToString(marc["entrada_5"]);
            entradas[5] = Convert.ToString(marc["entrada_6"]);
            entradas[6] = Convert.ToString(marc["entrada_7"]);
            entradas[7] = Convert.ToString(marc["entrada_8"]);

            saidas[0] = Convert.ToString(marc["saida_1"]);
            saidas[1] = Convert.ToString(marc["saida_2"]);
            saidas[2] = Convert.ToString(marc["saida_3"]);
            saidas[3] = Convert.ToString(marc["saida_4"]);
            saidas[4] = Convert.ToString(marc["saida_5"]);
            saidas[5] = Convert.ToString(marc["saida_6"]);
            saidas[6] = Convert.ToString(marc["saida_7"]);
            saidas[7] = Convert.ToString(marc["saida_8"]);
        }

        /// <summary>
        /// Seta as variáveis do horário detalhe
        /// </summary>
        /// <param name="tipoHorario"></param>
        /// <param name="idHorarioDetalhe"></param>
        /// <param name="entradas_jornada"></param>
        /// <param name="saidas_jornada"></param>
        /// <param name="marc"></param>
        /// <returns>caso não exista horário detalhe returna false, caso contrário retorna true</returns>
        private bool SetaVariaveisHorarioDetalhe(DataRow marc)
        {
            tipoHorario = Convert.ToInt32(marc["tipohorario"]);
            if (tipoHorario == 1) //Normal
            {
                idHorarioDetalhe = marc["idhdnormal"] is DBNull ? 0 : Convert.ToInt32(marc["idhdnormal"]);

                entradas_jornada[0] = Convert.ToString(marc["entrada_1normal"]);
                entradas_jornada[1] = Convert.ToString(marc["entrada_2normal"]);
                entradas_jornada[2] = Convert.ToString(marc["entrada_3normal"]);
                entradas_jornada[3] = Convert.ToString(marc["entrada_4normal"]);
                saidas_jornada[0] = Convert.ToString(marc["saida_1normal"]);
                saidas_jornada[1] = Convert.ToString(marc["saida_2normal"]);
                saidas_jornada[2] = Convert.ToString(marc["saida_3normal"]);
                saidas_jornada[3] = Convert.ToString(marc["saida_4normal"]);
            }
            else //Flexível
            {
                idHorarioDetalhe = marc["idhdflexivel"] is DBNull ? 0 : Convert.ToInt32(marc["idhdflexivel"]);

                //Não possui horario detalhe
                if (idHorarioDetalhe == 0)
                {
                    entradas_jornada[0] = "--:--";
                    entradas_jornada[1] = "--:--";
                    entradas_jornada[2] = "--:--";
                    entradas_jornada[3] = "--:--";
                    return false;
                }

                entradas_jornada[0] = Convert.ToString(marc["entrada_1flexivel"]);
                entradas_jornada[1] = Convert.ToString(marc["entrada_2flexivel"]);
                entradas_jornada[2] = Convert.ToString(marc["entrada_3flexivel"]);
                entradas_jornada[3] = Convert.ToString(marc["entrada_4flexivel"]);
                saidas_jornada[0] = Convert.ToString(marc["saida_1flexivel"]);
                saidas_jornada[1] = Convert.ToString(marc["saida_2flexivel"]);
                saidas_jornada[2] = Convert.ToString(marc["saida_3flexivel"]);
                saidas_jornada[3] = Convert.ToString(marc["saida_4flexivel"]);
            }

            toleranciaFalta = Convert.ToInt32(marc["thorafalta"]);

            return true;
        }

        private List<Modelo.Ocorrencia> AtribuiOcorrencias(List<Modelo.Ocorrencia> listaOcorrencias)
        {
            if (listaOcorrencias == null)
            {
                listaOcorrencias = bllOcorrencia.GetAllList(false);
            }
            return listaOcorrencias;
        }

        private void SetaVariaveisRelatorio(string[] pEntradas, string[] pSaidas, DataRow pMarc)
        {
            if (!carregouDados)
            {
                CarregaDados(pMarc);
            }

            idMarcacao = Convert.ToInt32(pMarc["id"]);
            empresa = pMarc["empresa"].ToString();
            cnpj_cpf = pMarc["cnpj_cpf"].ToString();
            departamento = pMarc["departamento"].ToString();
            funcionario = pMarc["funcionario"].ToString();
            dscodigo = pMarc["dscodigo"].ToString();
            bancohorasdeb = pMarc["bancohorasdeb"].ToString();

            matricula = pMarc["Matricula"].ToString();
            cpf = pMarc["CPF"].ToString();
            competencia = pMarc["Competencia"].ToString();
            observacao = pMarc["Observacao"].ToString();
            idDocumentoWorkflow = pMarc["IdDocumentoWorkflow"].ToString();

            marcacoes = (pEntradas[0] != "--:--" ? pEntradas[0] : "") +
                            (pSaidas[0] != "--:--" ? (pEntradas[0] != "--:--" ? " - " : "") + pSaidas[0] : "") +
                            (pEntradas[1] != "--:--" ? (pSaidas[0] != "--:--" ? " - " : "") + pEntradas[1] : "") +
                            (pSaidas[1] != "--:--" ? (pEntradas[1] != "--:--" ? " - " : "") + pSaidas[1] : "") +
                            (pEntradas[2] != "--:--" ? (pSaidas[1] != "--:--" ? " - " : "") + pEntradas[2] : "") +
                            (pSaidas[2] != "--:--" ? (pEntradas[2] != "--:--" ? " - " : "") + pSaidas[2] : "") +
                            (pEntradas[3] != "--:--" ? (pSaidas[2] != "--:--" ? " - " : "") + pEntradas[3] : "") +
                            (pSaidas[3] != "--:--" ? (pEntradas[3] != "--:--" ? " - " : "") + pSaidas[3] : "") +
                            (pEntradas[4] != "--:--" ? (pSaidas[3] != "--:--" ? " - " : "") + pEntradas[4] : "") +
                            (pSaidas[4] != "--:--" ? (pEntradas[4] != "--:--" ? " - " : "") + pSaidas[4] : "") +
                            (pEntradas[5] != "--:--" ? (pSaidas[4] != "--:--" ? " - " : "") + pEntradas[5] : "") +
                            (pSaidas[5] != "--:--" ? (pEntradas[5] != "--:--" ? " - " : "") + pSaidas[5] : "") +
                            (pEntradas[6] != "--:--" ? (pSaidas[5] != "--:--" ? " - " : "") + pEntradas[6] : "") +
                            (pSaidas[6] != "--:--" ? (pEntradas[6] != "--:--" ? " - " : "") + pSaidas[6] : "") +
                            (pEntradas[7] != "--:--" ? (pSaidas[6] != "--:--" ? " - " : "") + pEntradas[7] : "") +
                            (pSaidas[7] != "--:--" ? (pEntradas[7] != "--:--" ? " - " : "") + pSaidas[7] : "");
        }

        private void SetaVariaveisPorOcorrencia(DataRow pMarc, Modelo.Afastamento afastamento)
        {
            switch (ocorrencia)
            {
                case "Falta":
                    horasextradiurna = (pMarc["horasfaltas"].ToString() != "--:--" ? pMarc["horasfaltas"].ToString() : "");
                    horasextranoturna = (pMarc["horasfaltanoturna"].ToString() != "--:--" ? pMarc["horasfaltanoturna"].ToString() : "");
                    break;
                case "Horas Extras":
                    horasextradiurna = (pMarc["hr_extra_diurna"].ToString() != "--:--" ? pMarc["hr_extra_diurna"].ToString() : "");
                    horasextranoturna = (pMarc["hr_extra_noturna"].ToString() != "--:--" ? pMarc["hr_extra_noturna"].ToString() : "");
                    break;
                default:
                    if (afastamento != null && afastamento.Abonado == 1)
                    {
                        if ((afastamento.Horai != "--:--" || afastamento.Horaf != "--:--"))
                        {
                            horasextradiurna = afastamento.Horai != "--:--" ? afastamento.Horai : "";
                            horasextranoturna = afastamento.Horaf != "--:--" ? afastamento.Horaf : "";
                        }
                        else
                        {
                            horasextradiurna = (pMarc["horastrabalhadas"].ToString() != "--:--" ? pMarc["horastrabalhadas"].ToString() : "");
                            horasextranoturna = (pMarc["horastrabalhadasnoturnas"].ToString() != "--:--" ? pMarc["horastrabalhadasnoturnas"].ToString() : "");
                        }
                    }
                    else
                    {
                        horasextradiurna = "";
                        horasextranoturna = "";
                    }
                    break;
            }
        }
        private void InsereOcorrenciaGeral(DataTable pRet, string[] pEntradas, string[] pSaidas, DataRow pMarc)
        {
            SetaVariaveisRelatorio(pEntradas, pSaidas, pMarc);
            SetaVariaveisPorOcorrencia(pMarc, null);
            InsereOcorrenciaLista(pRet);
        }

        private void InsereOcorrenciaAfastamento(DataTable pRet, string[] pEntradas, string[] pSaidas, DataRow pMarc, Modelo.Afastamento afastamento)
        {
            SetaVariaveisRelatorio(pEntradas, pSaidas, pMarc);
            SetaVariaveisPorOcorrencia(pMarc, afastamento);
            InsereOcorrenciaLista(pRet);
        }

        private void InsereOcorrenciaFalta(DataTable pRet, string[] pEntradas, string[] pSaidas, DataRow pMarc, Modelo.TotalHoras totalHoras)
        {
            if (totalHoras.FaltasDias > 0 && (totalHoras.horasFaltaDiurnaMin > 0 || totalHoras.horasFaltaNoturnaMin > 0))
            {
                SetaVariaveisRelatorio(pEntradas, pSaidas, pMarc);
                ocorrencia = "Falta";
                horasextradiurna = totalHoras.horasFaltaDiurna;
                horasextranoturna = totalHoras.horasFaltaNoturna;
                InsereOcorrenciaLista(pRet);
            }
        }

        private void InsereOcorrenciaAtraso(DataTable pRet, string[] pEntradas, string[] pSaidas, DataRow pMarc, Modelo.TotalHoras totalHoras)
        {
            if (totalHoras.atrasoDMin > 0 || totalHoras.atrasoNMin > 0)
            {
                SetaVariaveisRelatorio(pEntradas, pSaidas, pMarc);
                ocorrencia = "Atraso";
                horasextradiurna = Modelo.cwkFuncoes.ConvertMinutosHora(totalHoras.atrasoDMin);
                horasextranoturna = Modelo.cwkFuncoes.ConvertMinutosHora(totalHoras.atrasoNMin);
                InsereOcorrenciaLista(pRet);
            }
        }

        private void InsereOcorrenciaLista(DataTable pRet)
        {
            if (horasextradiurna == "---:--" || horasextradiurna == "--:--" || horasextradiurna == "00:00")
            {
                horasextradiurna = "";
            }

            if (horasextranoturna == "---:--" || horasextranoturna == "--:--" || horasextranoturna == "00:00")
            {
                horasextranoturna = "";
            }

            object[] values = new object[]
                        {
                            idMarcacao,
                            empresa,
                            cnpj_cpf,
                            departamento,
                            dscodigo,
                            funcionario,
                            data.ToShortDateString(),
                            dia,
                            ocorrencia,
                            marcacoes,
                            bancohorasdeb != "---:--" ? bancohorasdeb : "",
                            horasextradiurna,
                            horasextranoturna,
                            null, // horastrabalhadas
                            null, // horastrabalhadasnoturnas
                            matricula,
                            cpf,
                            observacao,
                            competencia,
                            idDocumentoWorkflow,
                        };
            pRet.Rows.Add(values);
        }
    }
}
