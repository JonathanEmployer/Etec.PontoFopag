using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public partial class Marcacao : IBLL<Modelo.Marcacao>
    {
        public Modelo.IncrementaProgressBar incrementaPB;
        public Modelo.SetaMinMaxProgressBar setaMinMaxPB;
        public Modelo.SetaMensagem setaMensagem;
        public Modelo.SetaValorProgressBar setaValorPB;

        #region Data

        public List<DateTime> GetDataMarcacoesPeriodo(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            return dalMarcacao.GetDataMarcacoesPeriodo(pIdFuncionario, pDataI, pDataF);
        }

        public DateTime? GetUltimaDataFuncionario(int pIdFuncionario)
        {
            return dalMarcacao.GetUltimaDataFuncionario(pIdFuncionario);
        }

        public DateTime? GetUltimaDataFuncao(int pIdFuncao)
        {
            return dalMarcacao.GetUltimaDataFuncao(pIdFuncao);
        }

        public DateTime? GetUltimaDataDepartamento(int pIdDepartamento)
        {
            return dalMarcacao.GetUltimaDataDepartamento(pIdDepartamento);
        }

        public void SetaIdCompensadoNulo(int pIdCompensacao)
        {
            dalMarcacao.SetaIdCompensadoNulo(pIdCompensacao);
        }

        public int QuantidadeCompensada(int pIdCompensacao)
        {
            return dalMarcacao.QuantidadeCompensada(pIdCompensacao);
        }

        public DateTime? GetUltimaDataEmpresa(int pIdEmpresa)
        {
            return dalMarcacao.GetUltimaDataEmpresa(pIdEmpresa);
        }

        public Modelo.Marcacao GetPorData(Modelo.Funcionario pFuncionario, DateTime pData)
        {
            this.AtualizaData(pData, pData, pFuncionario);
            return dalMarcacao.GetPorData(pFuncionario, pData);
        }

        #endregion

        #region Banco de Horas

        public void CalculaBancoHorasAtualizaData(Modelo.Marcacao objMarcacao, int pIdFuncionario, int pIdFuncao, int pIdDepartamento, int pIdEmpresa, short pNaoEntrarBanco, Modelo.Horario objHorario, List<Modelo.FechamentoBHD> pFechamentoBHDLista, List<Modelo.InclusaoBanco> pInclusaoBancoLista, List<Modelo.BancoHoras> pBancoHorasLista)
        {
            Modelo.BancoHoras objBancoHoras = null;
            BLL.BancoHoras bllBancoHoras = new BancoHoras(ConnectionString, UsuarioLogado);
            //Chama o método que localiza o banco horas
            objBancoHoras = bllBancoHoras.PossuiRegistro(objMarcacao.Data, pIdEmpresa, pIdDepartamento, pIdFuncionario, pIdFuncao, pBancoHorasLista);

            objMarcacao.Bancohorascre = "---:--";
            objMarcacao.Bancohorasdeb = "---:--";

            if ((objBancoHoras == null) || (objMarcacao.Naoentrarbanco == 1) || (pNaoEntrarBanco == 1))
            {
                return;
            }

            int CreditoBH = 0;
            int DebitoBH = 0;
            if (objBancoHoras.FaltaDebito == 1 && objMarcacao.Legenda != "F")
            {
                DebitoBH = Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasfaltas) + Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasfaltanoturna);
                objMarcacao.Horasfaltas = "--:--";
                objMarcacao.Horasfaltanoturna = "--:--";
            }
            else
            {
                DebitoBH = 0;
            }

            InclusaoBancoHoras(objMarcacao, pIdFuncionario, pIdFuncao, pIdDepartamento, pIdEmpresa, pFechamentoBHDLista, pInclusaoBancoLista, ref CreditoBH, ref DebitoBH);
        }

        private void CalculaBancoHoras(Modelo.Marcacao objMarcacao, DateTime pDataAdmissao, DateTime? pDataDemissao, int pIdFuncionario, string pDsCodigo, int pIdHorario, int pIdFuncao, int pIdDepartamento, int pIdEmpresa, short pNaoEntrarBanco, Modelo.Horario objHorario, List<Modelo.FechamentoBHD> pFechamentoBHDLista, List<Modelo.Feriado> pFeriadoLista, List<Modelo.BancoHoras> pBancoHorasLista, List<Modelo.InclusaoBanco> pInclusaoBancoLista)
        {
            Modelo.BancoHoras objBancoHoras = null;
            BLL.BancoHoras bllBancoHoras = new BancoHoras(ConnectionString, UsuarioLogado);
            BLL.Feriado bllFeriado = new Feriado(ConnectionString, UsuarioLogado);
            //Chama o método que localiza o banco horas
            objBancoHoras = bllBancoHoras.PossuiRegistro(objMarcacao.Data, pIdEmpresa, pIdDepartamento, pIdFuncionario, pIdFuncao, pBancoHorasLista);

            objMarcacao.Bancohorascre = "---:--";
            objMarcacao.Bancohorasdeb = "---:--";

            if ((objBancoHoras == null) || (objMarcacao.Naoentrarbanco == 1) || (pNaoEntrarBanco == 1))
            {
                return;
            }

            int CreditoBH = 0;
            int DebitoBH = 0;
            if ((objMarcacao.Entrada_1 != "--:--" && objMarcacao.Saida_1 != "--:--") || (objMarcacao.Horasfaltas != "--:--") || (objMarcacao.Horasfaltanoturna != "--:--"))
            {
                int dia = 0;
                int fdia = 0;
                bool folga = false;

                CreditoBH = Modelo.cwkFuncoes.ConvertHorasMinuto(BLL.CalculoHoras.OperacaoHoras('+', objMarcacao.Horasextrasdiurna, objMarcacao.Horasextranoturna));
                if (objBancoHoras.FaltaDebito == 1 && objMarcacao.Legenda != "F")
                {
                    DebitoBH = Modelo.cwkFuncoes.ConvertHorasMinuto(BLL.CalculoHoras.OperacaoHoras('+', objMarcacao.Horasfaltas, objMarcacao.Horasfaltanoturna));
                }
                else
                {
                    DebitoBH = 0;
                }

                dia = Modelo.cwkFuncoes.Dia(objMarcacao.Data);

                if (bllFeriado.PossuiRegistro(objMarcacao.Data, pIdEmpresa, pIdDepartamento, pFeriadoLista))
                {
                    fdia = 8;
                }
                else
                {
                    fdia = 0;
                }

                List<Modelo.HorarioDetalhe> listaHDetalhe = new List<Modelo.HorarioDetalhe>();
                if (objHorario.TipoHorario == 1)
                {
                    listaHDetalhe = objHorario.HorariosDetalhe.ToList();
                }
                else if (objHorario.HorariosFlexiveis.Exists(h => h.Data == objMarcacao.Data))
                {
                    listaHDetalhe.Add(objHorario.HorariosFlexiveis.Where(h => h.Data == objMarcacao.Data).First());
                }

                foreach (Modelo.HorarioDetalhe hd in listaHDetalhe)
                {
                    if (hd.Dia != dia)
                    {
                        continue;
                    }
                    if (objHorario.TipoHorario == 1)
                    {
                        folga = hd.bCarregar == 1 ? false : true;
                        break;
                    }
                    else
                    {
                        folga = Convert.ToBoolean(hd.Flagfolga);
                        break;
                    }
                }

                if (objBancoHoras.getDias()[9] && (folga || objMarcacao.Folga == 1) &&  (objMarcacao.Afastamento != null && objMarcacao.Afastamento.bSuspensao) && BLL.CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadas, objMarcacao.Horastrabalhadasnoturnas) == "--:--" && CreditoBH != 0)
                {
                    if (objBancoHoras.getPercentuais()[9] >= objBancoHoras.getPercentuais()[dia] || objBancoHoras.getDias()[dia])
                    {
                        dia = 9;
                    }
                }

                if (objBancoHoras.getPercentuais()[fdia] >= objBancoHoras.getPercentuais()[dia] && fdia > 0)
                {
                    dia = fdia;
                }

                if (!objBancoHoras.getDias()[dia])
                {
                    return;
                }

                int hora = 0;
                int limite = 0;
                string limite_str = "--:--";

                if (objBancoHoras.getDias()[dia] && CreditoBH != 0 && objBancoHoras.getPercentuais()[dia] != 0 && objBancoHoras.Bancoprimeiro == 1)
                {
                    hora = CreditoBH + (CreditoBH * objBancoHoras.getPercentuais()[dia] / 100);
                    CreditoBH = hora;
                }

                if (CreditoBH > Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.getLimiteHoras()[dia]) && objBancoHoras.getDias()[dia] && objBancoHoras.Bancoprimeiro == 1)
                {
                    limite = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.getLimiteHoras()[dia]);
                    limite_str = objBancoHoras.getLimiteHoras()[dia];
                }
                else if (CreditoBH > Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.getLimiteHoraExtra()[dia]) && objBancoHoras.getDias()[dia] && objBancoHoras.ExtraPrimeiro == 1)
                {
                    limite_str = BLL.CalculoHoras.OperacaoHoras('-', BLL.CalculoHoras.OperacaoHoras('+', objMarcacao.Horasextrasdiurna, objMarcacao.Horasextranoturna), objBancoHoras.getLimiteHoraExtra()[dia]);
                    limite = Modelo.cwkFuncoes.ConvertHorasMinuto(limite_str);
                }
                else if (objBancoHoras.getDias()[dia] && objBancoHoras.ExtraPrimeiro == 1)
                {
                    CreditoBH = 0;
                }
                else
                {
                    limite = 0;
                }

                if (limite > 0)
                {
                    CreditoBH = limite;
                    if (objBancoHoras.getDias()[dia] && CreditoBH != 0 && objBancoHoras.getPercentuais()[dia] != 0 && objBancoHoras.ExtraPrimeiro == 1)
                    {
                        hora = CreditoBH + (CreditoBH * objBancoHoras.getPercentuais()[dia] / 100);
                        CreditoBH = hora;
                    }
                    if (objMarcacao.Horasextranoturna != "--:--" && objMarcacao.Horasextrasdiurna == "--:--")
                    {
                        objMarcacao.Horasextranoturna = BLL.CalculoHoras.OperacaoHoras('-', objMarcacao.Horasextranoturna, limite_str);
                    }
                    else if (objMarcacao.Horasextranoturna == "--:--" && objMarcacao.Horasextrasdiurna != "--:--")
                    {
                        objMarcacao.Horasextrasdiurna = BLL.CalculoHoras.OperacaoHoras('-', objMarcacao.Horasextrasdiurna, limite_str);
                    }
                    else
                    {
                        if (objBancoHoras.ExtraPrimeiro == 1)
                        {
                            objMarcacao.Horasextrasdiurna = BLL.CalculoHoras.OperacaoHoras('-', BLL.CalculoHoras.OperacaoHoras('+', objMarcacao.Horasextrasdiurna, objMarcacao.Horasextranoturna), limite_str);
                            objMarcacao.Horasextranoturna = "--:--";
                        }
                        else
                        {

                            int somaHoras = Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasextrasdiurna) + Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasextranoturna);
                            if (somaHoras >= limite)
                            {
                                CreditoBH = limite;
                                // Se não tem horas extras noturnas tudo vai para diurno
                                if (objMarcacao.Horasextranoturna == "--:--")
                                {
                                    objMarcacao.Horasextrasdiurna = BLL.CalculoHoras.OperacaoHoras('-', objMarcacao.Horasextrasdiurna, limite_str);
                                }
                                // Tem hora extra noturna e não tem hora extra diurna
                                else if (objMarcacao.Horasextrasdiurna == "--:--")
                                {
                                    objMarcacao.Horasextranoturna = BLL.CalculoHoras.OperacaoHoras('-', objMarcacao.Horasextranoturna, BLL.CalculoHoras.OperacaoHoras('-', limite_str, objMarcacao.Horasextrasdiurna));
                                }
                                // As horas são divididas entre extra noturna e diurna
                                else
                                {
                                    //Sobra extras diurnas e as noturnas continuam iguais
                                    if (Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasextrasdiurna) >= limite)
                                    {
                                        objMarcacao.Horasextrasdiurna = BLL.CalculoHoras.OperacaoHoras('-', objMarcacao.Horasextrasdiurna, limite_str);
                                    }
                                    //Tira todas as horas do diurno e ainda tem que tirar algumas do noturno
                                    else
                                    {
                                        //Calcula as horas que faltam para completar o limite do banco
                                        int faltaParaCompletar = limite - Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasextrasdiurna);
                                        //O resto vai para hora extra
                                        objMarcacao.Horasextranoturna = BLL.CalculoHoras.OperacaoHoras('-', objMarcacao.Horasextranoturna, Modelo.cwkFuncoes.ConvertMinutosBatida(faltaParaCompletar));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (objBancoHoras.ExtraPrimeiro == 0)
                    {
                        objMarcacao.Horasextrasdiurna = "--:--";
                        objMarcacao.Horasextranoturna = "--:--";
                    }
                }

                if (objBancoHoras.FaltaDebito == 1)
                {
                    objMarcacao.Horasfaltas = "--:--";
                    objMarcacao.Horasfaltanoturna = "--:--";
                }

                InclusaoBancoHoras(objMarcacao, pIdFuncionario, pIdFuncao, pIdDepartamento, pIdEmpresa, pFechamentoBHDLista, pInclusaoBancoLista, ref CreditoBH, ref DebitoBH);
            }
            else
            {
                objMarcacao.Bancohorascre = "---:--";
                objMarcacao.Bancohorasdeb = "---:--";
                InclusaoBancoHoras(objMarcacao, pIdFuncionario, pIdFuncao, pIdDepartamento, pIdEmpresa, pFechamentoBHDLista, pInclusaoBancoLista, ref CreditoBH, ref DebitoBH);
            }
        }

        public void InclusaoBancoHoras(Modelo.Marcacao objMarcacao, int pIdFuncionario, int pIdFuncao, int pIdDepartamento, int pIdEmpresa, List<Modelo.FechamentoBHD> pFechamentoBHDLista, List<Modelo.InclusaoBanco> pInclusaoBancoLista, ref int CreditoBH, ref int DebitoBH)
        {
            #region InclusãoBancoHoras
            BLL.InclusaoBanco bllInclusaoBanco = new InclusaoBanco(ConnectionString, UsuarioLogado);
            int credito = 0;
            int debito = 0;
            bllInclusaoBanco.getSaldo(objMarcacao.Data, pIdEmpresa, pIdDepartamento, pIdFuncionario, pIdFuncao, out credito, out debito, pInclusaoBancoLista);

            CreditoBH = CreditoBH + credito;
            DebitoBH = DebitoBH + debito;

            bool teminc = false;
            if (credito > 0 || debito > 0)
            {
                teminc = true;
            }
            #endregion

            //Verifica se neste dia possui fechamento
            Modelo.FechamentoBHD objFechamentoBHD = null;
            if (pFechamentoBHDLista.Count > 0)
            {
                if (pFechamentoBHDLista.Exists(x => x.DataFechamento == objMarcacao.Data))
                {
                    foreach (Modelo.FechamentoBHD fbhd in pFechamentoBHDLista)
                    {
                        if (fbhd.DataFechamento == objMarcacao.Data)
                        {
                            objFechamentoBHD = fbhd;
                        }
                    }
                }
            }


            if (objFechamentoBHD != null)
            {
                if (objMarcacao.Ocorrencia != "Marcações Incorretas")
                {
                    objMarcacao.Ocorrencia = objFechamentoBHD.Saldo + " [" + (objFechamentoBHD.Tiposaldo == 0 ? "C" : "D") + "] H. Pagas";
                    if (!String.IsNullOrEmpty(objFechamentoBHD.MotivoFechamento))
                    {
                        objMarcacao.Ocorrencia += " - " + objFechamentoBHD.MotivoFechamento;
                    }
                }

                objMarcacao.Bancohorascre = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH) != "00:00" ? Modelo.cwkFuncoes.ConvertMinutosHora(3, CreditoBH) : "---:--";
                objMarcacao.Bancohorasdeb = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH) != "00:00" ? Modelo.cwkFuncoes.ConvertMinutosHora(3, DebitoBH) : "---:--"; ;
            }
            else
            {
                if (CreditoBH > 0 || DebitoBH > 0)
                {
                    if (objMarcacao.Ocorrencia != "Marcações Incorretas")
                    {
                        if (CreditoBH == DebitoBH)
                        {
                            objMarcacao.Ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH - DebitoBH) + " - Crédito no BH";
                        }
                        else if (CreditoBH > DebitoBH && CreditoBH > 0)
                        {
                            objMarcacao.Ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH - DebitoBH) + " - Crédito no BH";
                        }
                        else if (DebitoBH > 0)
                        {
                            if (!(objMarcacao.Legenda != "A" && objMarcacao.Semcalculo == 1))
                            {
                                objMarcacao.Ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH - CreditoBH) + " - Débito no BH";
                            }
                            else
                            {
                                objMarcacao.Ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH - CreditoBH) + " - " + objMarcacao.Ocorrencia;
                            }
                        }
                    }

                    objMarcacao.Bancohorascre = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH) != "000:00" ? Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH) : "---:--";
                    objMarcacao.Bancohorasdeb = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH) != "000:00" ? Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH) : "---:--"; ;
                }
            }

            if (teminc == true && objMarcacao.Legenda != "A")
            {
                objMarcacao.Legenda = "I";
            }
        }

        /// <summary>
        /// Método responsável por atualizar no banco de dados as informações referentes a um fechamento de bh.
        /// </summary>




        public string MontaUpdateFechamento(int pIdFuncionario, int pIdFechamentoBH, DateTime pDataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.MontaUpdateFechamento(pIdFuncionario, pIdFechamentoBH, pDataInicial, pDataFinal);
        }

        public void ClearFechamentoBH(int pIdFechamentoBH)
        {
            dalMarcacao.ClearFechamentoBH(pIdFechamentoBH);
        }

        public DateTime? GetDataDSRAnterior(int pIdFuncionario, DateTime pData)
        {
            return dalMarcacao.GetDataDSRAnterior(pIdFuncionario, pData);
        }

        public DateTime? GetDataDSRProximo(int pIdFuncionario, DateTime pData)
        {
            return dalMarcacao.GetDataDSRProximo(pIdFuncionario, pData);
        }

        #endregion

        #region Relatórios

        private void PreencheJornadaPrimeiroRegData(int idfuncionario, ref DateTime data, List<Modelo.JornadaAlternativa> jornadasAlternativas, ref Modelo.JornadaAlternativa objJornadaAlternativa, string[] batidas, ref string entrada_1, ref string entrada_2, ref string entrada_3, ref string entrada_4, ref string saida_1, ref string saida_2, ref string saida_3, ref string saida_4, DataRow row)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);
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
                    }
                }
            }

            PreencheVetorBatidasMarcacao(batidas, row);
        }

        private static void PreencheVetorBatidasMarcacao(string[] batidas, DataRow row)
        {
            int cursor = 0;            
            //Limpa o vetor de batidas
            LimpaVetoraBatidas(batidas);
            //Preenche o vetor apenas com as batidas validas
            for (int i = 1; i <= 8; i++)
            {
                if (Convert.ToChar(row["tratent_" + i.ToString()]) != 'D')
                {
                    batidas[cursor++] = Convert.ToString(row["entrada_" + i.ToString()]);
                }

                if (Convert.ToChar(row["tratsai_" + i.ToString()]) != 'D')
                {
                    batidas[cursor++] = Convert.ToString(row["saida_" + i.ToString()]);
                }

                if (cursor >= 15)
                {
                    break;
                }
            }
        }

        private static void LimpaVetoraBatidas(string[] batidas)
        {
            for (int i = 0; i < batidas.Length; i++)
            {
                batidas[i] = "";
            }
        }

        private static void InsereLinhaJornada(int indiceBatidas, DataTable ret, string horatratamento, string[] batidas, ref string entrada_1, ref string entrada_2, string entrada_3, string entrada_4, ref string saida_1, ref string saida_2, string saida_3, string saida_4, bool naoPossuiHorario, DataRow row, bool folga, ref object[] values2)
        {
            bool bJornada = (entrada_3 != "--:--" && !String.IsNullOrEmpty(entrada_3));
            bool bBatidas = (batidas[indiceBatidas] != "--:--" && !String.IsNullOrEmpty(batidas[indiceBatidas]));
            if (bJornada || bBatidas)
            {
                if (bJornada)
                {
                    entrada_1 = entrada_3;
                    entrada_2 = entrada_4;
                    saida_1 = saida_3;
                    saida_2 = saida_4;
                }
                else
                {
                    entrada_1 = "";
                    entrada_2 = "";
                    saida_1 = "";
                    saida_2 = "";
                }

                values2 = new object[]
                    {
                        row["id"],
                        row["idhorario"],
                        row["legenda"],
                        String.Format("{0:00}", ((DateTime)row["data"]).Day) + "/" + ((DateTime)row["data"]).Month.ToString(),
                        Modelo.cwkFuncoes.DiaSemana(Convert.ToDateTime(row["data"]), Modelo.cwkFuncoes.TipoDiaSemana.Reduzido).Trim(new char[] {'.'}),
                        row["dscodigo"],
                        row["funcionario"],
                        row["matricula"],
                        ((DateTime)row["dataadmissao"]).ToShortDateString(),
                        row["pis"],
                        row["funcao"],
                        row["departamento"],
                        row["empresa"],
                        row["codigoempresa"],
                        row["cnpj_cpf"],
                        row["endereco"],
                        row["cidade"],
                        row["estado"],
                        naoPossuiHorario ? "0000" :row["codigohorario"],
                        "",
                        "",
                        row["indicador"],
                        row["jr_ent_1"],
                        row["jr_sai_1"],
                        row["jr_ent_2"],
                        row["jr_sai_2"],
                        row["jr_ent_3"],
                        row["jr_sai_3"],
                        row["jr_ent_4"],
                        row["jr_sai_4"],
                        row["jr_ent_5"],
                        row["jr_sai_5"],
                        row["jr_ent_6"],
                        row["jr_sai_6"],
                        row["jr_ent_7"],
                        row["jr_sai_7"],
                        row["jr_ent_8"],
                        row["jr_sai_8"],
                        folga ? "" : entrada_1 == "--:--" ? "" : entrada_1,
                        folga ? "" : entrada_2 == "--:--" ? "" : entrada_2,
                        folga ? "" : saida_1 == "--:--" ? "" : saida_1,
                        folga ? "" : saida_2 == "--:--" ? "" : saida_2,
                        horatratamento,
                        batidas[indiceBatidas] == "--:--" ? "" : batidas[indiceBatidas++],
                        batidas[indiceBatidas] == "--:--" ? "" : batidas[indiceBatidas++],
                        batidas[indiceBatidas] == "--:--" ? "" : batidas[indiceBatidas++],
                        batidas[indiceBatidas] == "--:--" ? "" : batidas[indiceBatidas++],
                        batidas[indiceBatidas] == "--:--" ? "" : batidas[indiceBatidas++],
                        batidas[indiceBatidas] == "--:--" ? "" : batidas[indiceBatidas++],
                    };
                ret.Rows.Add(values2);
            }
        }

        #endregion
    }
}
