using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RelatorioEspelho
    {
        private DAL.IRelatorioEspelho dalRelatorioEspelho;
        
        private string ConnectionString;
        private Char[] tiposTratamento = new Char[] { 'D', 'I', 'P' };
        private Modelo.Cw_Usuario UsuarioLogado;

        public RelatorioEspelho() : this(null)
        {
            
        }

        public RelatorioEspelho(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public RelatorioEspelho(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalRelatorioEspelho = new DAL.SQL.RelatorioEspelho();
                    break;
                case 2:
                    dalRelatorioEspelho = new DAL.FB.RelatorioEspelho();
                    break;
            }
            UsuarioLogado = usuarioLogado;
        }

        public DataTable GetEspelhoPontoRel(DateTime dataInicial, DateTime dataFinal, string ids, int tipo, Modelo.ProgressBar pb, List<string> jornadas)
        {
            pb.setaMensagem("Gerando relatório...");
            BilhetesImp bllBilhetesImp = new BilhetesImp(ConnectionString, UsuarioLogado);
            JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
            
            //Contém as informações necessárias para montar o relatório
            DataTable marcacoes = new DataTable();
            marcacoes = dalRelatorioEspelho.GetMarcacoesEspelho(dataInicial, dataFinal, ids, tipo, new DAL.SQL.DataBase(ConnectionString));
            #region Teste
            //List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            //Modelo.BilhetesImp teste = new Modelo.BilhetesImp();
            //teste.Codigo = 1;
            //teste.Data = DateTime.Now;
            //teste.Mar_data = DateTime.Now;
            //teste.DsCodigo = "1";
            //teste.Ent_sai = "E";
            //teste.Func = "1";
            //teste.Hora = "22:00";
            //teste.Mar_hora = "22:00";
            //teste.Motivo = "JOSE";
            //teste.Ocorrencia = 'I';
            //teste.Ordem = "010";
            //teste.Posicao = 1;
            //teste.Relogio = "02";
            //bilhetes.Add(teste);

            //teste = new Modelo.BilhetesImp();
            //teste.Codigo = 1;
            //teste.Data = DateTime.Now.AddDays(1);
            //teste.Mar_data = DateTime.Now;
            //teste.DsCodigo = "1";
            //teste.Ent_sai = "S";
            //teste.Func = "1";
            //teste.Hora = "03:00";
            //teste.Mar_hora = "03:00";
            //teste.Motivo = "JOSE";
            //teste.Ocorrencia = 'I';
            //teste.Ordem = "010";
            //teste.Posicao = 1;
            //teste.Relogio = "02";
            //bilhetes.Add(teste);
            #endregion

            List<Modelo.BilhetesImp> bilhetes = bllBilhetesImp.GetBilhetesEspelho(dataInicial, dataFinal, ids, tipo);

            DataTable ret = new DataTable();
            SetaColunasDataTable(ret);

            #region Variáveis

            int id, idfuncionario, sequencia, indiceBatida, idjornada;
            DateTime dataMarcacao, outraData;
            string dscodigo, diaStr, outroDiaStr, marcacoesRegistradas;
            string entrada_1 = "--:--", entrada_2 = "--:--", entrada_3 = "--:--", entrada_4 = "--:--";
            string saida_1 = "--:--", saida_2 = "--:--", saida_3 = "--:--", saida_4 = "--:--";

            string[] batidas = new string[16];

            bool folga, naoPossuiHorario;

            Modelo.JornadaAlternativa objJornadaAlternativa = null;
            List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetPeriodo(dataInicial, dataFinal);

            List<Tratamento> tratamentos;
            #endregion

            pb.setaMinMaxPB(0, marcacoes.Rows.Count);
            pb.setaValorPB(0);
            string chave = String.Empty;
            foreach (DataRow row in marcacoes.Rows)
            {
                pb.setaMensagem("Funcionário(a): " + row["funcionario"].ToString());
                sequencia = 1;
                SetaVariaveis(row, out id, out idfuncionario, out dataMarcacao, out dscodigo, out diaStr, out folga);

                PreencheJornada(idfuncionario, dataMarcacao, jornadasAlternativas, row, ref objJornadaAlternativa, ref entrada_1, ref entrada_2
                    , ref entrada_3, ref entrada_4, ref saida_1, ref saida_2, ref saida_3, ref saida_4, out idjornada);

                //Preenche a lista com as jornadas utilizadas no relatório
                if (idjornada != 0)
                {
                    chave = idfuncionario + "-" + idjornada;
                    if (!jornadas.Contains(chave))
                        jornadas.Add(chave);
                }

                naoPossuiHorario = ((entrada_1 == "--:--" && entrada_2 == "--:--"
                                && saida_1 == "--:--" && saida_2 == "--:--") || folga);

                var bil = bilhetes.Where(b => b.DsCodigo == dscodigo && b.Mar_data.Value.Date == dataMarcacao.Date);
                tratamentos = new List<Tratamento>();
                indiceBatida = 0;
                //Marcação não possui batidas
                if (bil.Count() == 0)
                {
                    LimpaVetoraBatidas(batidas);
                    //Insere Marcação Vazia
                    AdicionaRegistro(ret, id, dscodigo, diaStr, entrada_1, entrada_2, saida_1, saida_2, "", batidas, "", "", "", diaStr, folga, naoPossuiHorario, row, ref indiceBatida, false, sequencia++, idjornada, false, idfuncionario);
                    //Se tiver horário com mais de 4 batidas, insere registro com o restante das batidas
                    //if (entrada_3 != "--:--" && !String.IsNullOrEmpty(entrada_3) && !folga)
                    //{
                    //    AdicionaRegistro(ret, id, dscodigo, diaStr, entrada_3, entrada_4, saida_3, saida_4, "", batidas, "", "", "", diaStr, folga, naoPossuiHorario, row, ref indiceBatida, false, sequencia++, 0, false, idfuncionario);
                    //}
                }
                //Marcação possui batidas
                else
                {
                    List<Modelo.BilhetesImp> bilhetesOutroDia = bil.Where(b => b.Data.Date != dataMarcacao.Date).ToList();
                    List<Modelo.BilhetesImp> bilhetesDiaMarcacao = bil.Where(b => b.Data.Date == dataMarcacao.Date).ToList();
                    //Possui batidas do dia pra noite
                    if (bilhetesOutroDia.Count() > 0)
                    {
                        outraData = bilhetesOutroDia.First().Data;
                        outroDiaStr = String.Format("{0:00}", (outraData.Day));
                        if (outraData.Date < dataMarcacao.Date)
                        {
                            //Insere da data anterior
                            marcacoesRegistradas = PreencheVetorBatidasMarcacao(batidas, bilhetesOutroDia, tratamentos, indiceBatida);
                            AuxRelatorio(ret, id, dscodigo, outroDiaStr, diaStr, marcacoesRegistradas, entrada_1, entrada_2, entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4, batidas, folga, naoPossuiHorario, tratamentos, row, ref sequencia, idjornada, ref indiceBatida, idfuncionario);

                            //Insere da data atual
                            tratamentos.Clear();
                            marcacoesRegistradas = PreencheVetorBatidasMarcacao(batidas, bilhetesDiaMarcacao, tratamentos, indiceBatida);
                            AuxRelatorio(ret, id, dscodigo, diaStr, diaStr, marcacoesRegistradas, "", "", "", "", "", "", "", "", batidas, folga, naoPossuiHorario, tratamentos, row, ref sequencia, 0, ref indiceBatida, idfuncionario);
                        }
                        else
                        {
                            //Insere da data atual
                            marcacoesRegistradas = PreencheVetorBatidasMarcacao(batidas, bilhetesDiaMarcacao, tratamentos, indiceBatida);
                            AuxRelatorio(ret, id, dscodigo, diaStr, diaStr, marcacoesRegistradas, entrada_1, entrada_2, entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4, batidas, folga, naoPossuiHorario, tratamentos, row, ref sequencia, idjornada, ref indiceBatida, idfuncionario);

                            //Insere da próxima data
                            tratamentos.Clear();
                            marcacoesRegistradas = PreencheVetorBatidasMarcacao(batidas, bilhetesOutroDia, tratamentos, indiceBatida);
                            AuxRelatorio(ret, id, dscodigo, outroDiaStr, diaStr, marcacoesRegistradas, "", "", "", "", "", "", "", "", batidas, folga, naoPossuiHorario, tratamentos, row, ref sequencia, 0, ref indiceBatida, idfuncionario);
                        }
                    }
                    else
                    {
                        marcacoesRegistradas = PreencheVetorBatidasMarcacao(batidas, bilhetesDiaMarcacao, tratamentos, indiceBatida);
                        AuxRelatorio(ret, id, dscodigo, diaStr, diaStr, marcacoesRegistradas, entrada_1, entrada_2, entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4, batidas, folga, naoPossuiHorario, tratamentos, row, ref sequencia, idjornada, ref indiceBatida, idfuncionario);
                    }
                }
                pb.incrementaPB(1);
            }

            return ret;
        }

        private void AuxRelatorio(DataTable ret, int id, string dscodigo, string dataStr, string dataAgrupamento, string marcacoesRegistradas, string entrada_1, string entrada_2, string entrada_3, string entrada_4, string saida_1, string saida_2, string saida_3, string saida_4, string[] batidas, bool folga, bool naoPossuiHorario, List<Tratamento> tratamentos, DataRow row, ref int sequencia, int idjornada, ref int indiceBatida, int idfuncionario)
        {
            bool comecaNaSaida = !(indiceBatida % 2 == 0);
            //Não possui tratamentos
            if (tratamentos.Count == 0)
            {
                AdicionaRegistro(ret, id, dscodigo, dataStr, entrada_1, entrada_2, saida_1, saida_2, marcacoesRegistradas, batidas, "", "", "", dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, true, sequencia++, idjornada, comecaNaSaida, idfuncionario);
                if (indiceBatida <= 15)
                {
                    if (batidas[indiceBatida] != "--:--")
                    {
                        comecaNaSaida = !(indiceBatida % 2 == 0);
                        AdicionaRegistro(ret, id, dscodigo, dataStr, entrada_3, entrada_4, saida_3, saida_4, "", batidas, "", "", "", dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, true, sequencia++, 0, comecaNaSaida, idfuncionario);
                    }
                    if (batidas[indiceBatida] != "--:--")
                    {
                        comecaNaSaida = !(indiceBatida % 2 == 0);
                        AdicionaRegistro(ret, id, dscodigo, dataStr, "", "", "", "", "", batidas, "", "", "", dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, true, sequencia++, 0, comecaNaSaida, idfuncionario);
                    }
                }
            }
            //Possui tratamentos
            else
            {
                int contador = 1;
                foreach (Tratamento t in tratamentos)
                {
                    if (contador == 1)
                    {
                        AdicionaRegistro(ret, id, dscodigo, dataStr, entrada_1, entrada_2, saida_1, saida_2, marcacoesRegistradas, batidas, t.Horario, t.Ocorrencia, t.Motivo, dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, true, sequencia++, idjornada, comecaNaSaida, idfuncionario);
                    }
                    else if (contador == 2 && (batidas[indiceBatida] != "--:--"))
                    {
                        comecaNaSaida = !(indiceBatida % 2 == 0);
                        AdicionaRegistro(ret, id, dscodigo, dataStr, entrada_3, entrada_4, saida_3, saida_4, "", batidas, t.Horario, t.Ocorrencia, t.Motivo, dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, true, sequencia++, 0, comecaNaSaida, idfuncionario);
                    }
                    else if (batidas[indiceBatida] != "--:--")
                    {
                        comecaNaSaida = !(indiceBatida % 2 == 0);
                        AdicionaRegistro(ret, id, dscodigo, dataStr, "", "", "", "", "", batidas, t.Horario, t.Ocorrencia, t.Motivo, dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, true, sequencia++, 0, comecaNaSaida, idfuncionario);
                    }
                    else
                    {
                        AdicionaRegistro(ret, id, dscodigo, dataStr, "", "", "", "", "", batidas, t.Horario, t.Ocorrencia, t.Motivo, dataAgrupamento, folga, naoPossuiHorario, row, ref indiceBatida, false, sequencia++, 0, false, idfuncionario);
                    }
                    contador++;
                }
            }
        }

        private int AdicionaRegistro(DataTable ret, int id, string dscodigo, string dataStr, string entrada_1, string entrada_2, string saida_1, string saida_2, string registradas, string[] batidas, string horaTratamento, string ocorrencia, string motivo, string dataAgrupamento, bool folga, bool naoPossuiHorario, DataRow row, ref int indiceBatida, bool imprimeBatidas, int sequencia, int idjornada, bool comecaNaSaida, int idfuncionario)
        {
            int aux = indiceBatida;
            bool imp1 = !comecaNaSaida && imprimeBatidas && indiceBatida++ < 16;
            bool imp2 = imprimeBatidas && indiceBatida++ < 16;
            bool imp3 = imprimeBatidas && indiceBatida++ < 16;
            bool imp4 = imprimeBatidas && indiceBatida++ < 16;
            bool imp5 = imprimeBatidas && indiceBatida++ < 16;
            bool imp6 = imprimeBatidas && indiceBatida++ < 16;
            indiceBatida = aux;

            object[] reg = new object[]
                    {
                        id,
                        dscodigo,
                        row["funcionario"],
                        ((DateTime)row["dataadmissao"]).ToShortDateString(),
                        row["pis"],
                        row["empresa"],
                        row["codigoempresa"],
                        row["cnpj_cpf"],
                        row["endereco"],
                        row["cidade"],
                        row["estado"],
                        dataStr,
                        folga ? "" : entrada_1 == "--:--" ? "" : entrada_1,
                        folga ? "" : entrada_2 == "--:--" ? "" : entrada_2,
                        folga ? "" : saida_1 == "--:--" ? "" : saida_1,
                        folga ? "" : saida_2 == "--:--" ? "" : saida_2,
                        registradas,
                        naoPossuiHorario ? "00000" : String.Format("{0:00000}",idjornada),
                        imp1 ? batidas[indiceBatida] == "--:--" ? "" : batidas[indiceBatida++] : "",
                        imp2 ? batidas[indiceBatida] == "--:--" ? "" : batidas[indiceBatida++] : "",
                        imp3 ? batidas[indiceBatida] == "--:--" ? "" : batidas[indiceBatida++] : "",
                        imp4 ? batidas[indiceBatida] == "--:--" ? "" : batidas[indiceBatida++] : "",
                        imp5 ? batidas[indiceBatida] == "--:--" ? "" : batidas[indiceBatida++] : "",
                        imp6 ? batidas[indiceBatida] == "--:--" ? "" : batidas[indiceBatida++] : "",
                        horaTratamento,
                        ocorrencia,
                        motivo,
                        dataAgrupamento,
                        sequencia,
                        idfuncionario
                    };
            ret.Rows.Add(reg);
            return indiceBatida - aux;
        }

        private void SetaVariaveis(DataRow row, out int id, out int idfuncionario, out DateTime dataMarcacao, out string dscodigo, out string diaStr, out bool folga)
        {
            id = Convert.ToInt32(row["id"]);
            dscodigo = row["dscodigo"].ToString();
            dataMarcacao = Convert.ToDateTime(row["data"].ToString());
            diaStr = String.Format("{0:00}", dataMarcacao.Day);
            folga = Convert.ToBoolean(row["folga"]);
            idfuncionario = Convert.ToInt32(row["idfuncionario"]);
        }

        private void PreencheJornada(int idfuncionario, DateTime data, List<Modelo.JornadaAlternativa> jornadasAlternativas, DataRow row, ref Modelo.JornadaAlternativa objJornadaAlternativa, ref string entrada_1, ref string entrada_2, ref string entrada_3, ref string entrada_4, ref string saida_1, ref string saida_2, ref string saida_3, ref string saida_4, out int idjornada)
        {
            JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
            objJornadaAlternativa = bllJornadaAlternativa.PossuiRegistro(jornadasAlternativas, data, idfuncionario
                                                                         , Convert.ToInt32(row["idfuncao"].ToString())
                                                                         , Convert.ToInt32(row["iddepartamento"].ToString())
                                                                         , Convert.ToInt32(row["idempresa"].ToString()));
            if (objJornadaAlternativa != null)
            {
                entrada_1 = objJornadaAlternativa.Entrada_1;
                entrada_2 = objJornadaAlternativa.Entrada_2;
                entrada_3 = objJornadaAlternativa.Entrada_3;
                entrada_4 = objJornadaAlternativa.Entrada_4;
                saida_1 = objJornadaAlternativa.Saida_1;
                saida_2 = objJornadaAlternativa.Saida_2;
                saida_3 = objJornadaAlternativa.Saida_3;
                saida_4 = objJornadaAlternativa.Saida_4;
                idjornada = objJornadaAlternativa.Idjornada;
            }
            else
            {
                if (Convert.ToInt32(row["tipohorario"]) == 1)
                {
                    entrada_1 = row["entrada_1normal"].ToString();
                    entrada_2 = row["entrada_2normal"].ToString();
                    entrada_3 = row["entrada_3normal"].ToString();
                    entrada_4 = row["entrada_4normal"].ToString();
                    saida_1 = row["saida_1normal"].ToString();
                    saida_2 = row["saida_2normal"].ToString();
                    saida_3 = row["saida_3normal"].ToString();
                    saida_4 = row["saida_4normal"].ToString();
                    idjornada = row["idjornada_normal"] is DBNull ? 0 : Convert.ToInt32(row["idjornada_normal"]);
                }
                else
                {
                    if (row["entrada_1flexivel"] is DBNull)
                    {
                        entrada_1 = "--:--";
                        entrada_2 = "--:--";
                        entrada_3 = "--:--";
                        entrada_4 = "--:--";
                        saida_1 = "--:--";
                        saida_2 = "--:--";
                        saida_3 = "--:--";
                        saida_4 = "--:--";
                        idjornada = 0;
                    }
                    else
                    {
                        entrada_1 = row["entrada_1flexivel"].ToString();
                        entrada_2 = row["entrada_2flexivel"].ToString();
                        entrada_3 = row["entrada_3flexivel"].ToString();
                        entrada_4 = row["entrada_4flexivel"].ToString();
                        saida_1 = row["saida_1flexivel"].ToString();
                        saida_2 = row["saida_2flexivel"].ToString();
                        saida_3 = row["saida_3flexivel"].ToString();
                        saida_4 = row["saida_4flexivel"].ToString();
                        idjornada = row["idjornada_flexivel"] is DBNull ? 0 : Convert.ToInt32(row["idjornada_flexivel"]);
                    }
                }
            }
        }

        private void SetaColunasDataTable(DataTable ret)
        {
            DataColumn[] colunasHora = new DataColumn[]
            {
                new DataColumn("id"),
                new DataColumn("dscodigo"),
                new DataColumn("funcionario"),
                new DataColumn("dataadmissao"),
                new DataColumn("pis"),
                new DataColumn("empresa"),
                new DataColumn("codigoempresa"),
                new DataColumn("cnpj_cpf"),
                new DataColumn("endereco"),
                new DataColumn("cidade"),
                new DataColumn("estado"),
                new DataColumn("dia"),
                new DataColumn("hentrada_1"),
                new DataColumn("hentrada_2"),
                new DataColumn("hsaida_1"),
                new DataColumn("hsaida_2"),
                new DataColumn("registradas"),
                new DataColumn("codigohorario"),
                new DataColumn("batida1"),
                new DataColumn("batida2"),
                new DataColumn("batida3"),
                new DataColumn("batida4"),
                new DataColumn("batida5"),
                new DataColumn("batida6"),
                new DataColumn("horatratamento"),
                new DataColumn("ocorrencia"),
                new DataColumn("motivo"),
                new DataColumn("dia_agrupamento"),
                new DataColumn("sequencia"),
                new DataColumn("idfuncionario"),
            };

            ret.Columns.AddRange(colunasHora);
        }

        private void LimpaVetoraBatidas(string[] batidas)
        {
            for (int i = 0; i < batidas.Length; i++)
            {
                batidas[i] = "--:--";
            }
        }

        private string PreencheVetorBatidasMarcacao(string[] batidas, List<Modelo.BilhetesImp> bilhetes, List<Tratamento> tratamentos, int cursor)
        {
            StringBuilder registradas = new StringBuilder();
            int qtdRegistradas = 0;
            //Limpa o vetor de batidas
            LimpaVetoraBatidas(batidas);
            //Preenche o vetor apenas com as batidas validas
            Tratamento trat;
            foreach (Modelo.BilhetesImp item in
                        (from b in bilhetes
                         orderby b.Posicao, b.Ent_sai
                         select b)
                     )
            {
                if (item.Ocorrencia != 'I' && item.Ocorrencia != 'P')
                {
                    if (qtdRegistradas > 0)
                        registradas.Append(" ");
                    registradas.Append(item.Hora);
                    qtdRegistradas++;
                }
                //Preenche o vetor apenas com as batidas válidas
                if (item.Ocorrencia != 'D' && cursor <= 15)
                    batidas[cursor++] = item.Hora;

                if (tiposTratamento.Contains(item.Ocorrencia))
                {
                    trat = new Tratamento();
                    trat.Horario = item.Hora;
                    trat.Motivo = item.Motivo;
                    trat.Ocorrencia = item.Ocorrencia != new char() ? item.Ocorrencia.ToString() : "";
                    tratamentos.Add(trat);
                }
            }
            //Retorna uma string com as marcações registradas no relogio
            return registradas.ToString();
        }

        public DataTable GetJornadasEspelho(List<string> jornadas, int tipo)
        {
            return dalRelatorioEspelho.GetJornadasEspelho(jornadas, tipo, new DAL.SQL.DataBase(ConnectionString));
        }
    }

    public struct Tratamento
    {
        public string Horario { get; set; }
        public string Ocorrencia { get; set; }
        public string Motivo { get; set; }
    }
}
