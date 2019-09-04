using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class CartaoPontoV2
    {
        private string conn;
        private Modelo.Cw_Usuario usuarioLogado;
        public CartaoPontoV2(string conn, Modelo.Cw_Usuario usuarioLogado)
        {
            this.conn = conn;
            this.usuarioLogado = usuarioLogado;
        }

        /// <summary>
        /// Retornar uma lista de objetos para geração de cartão ponto
        /// </summary>
        /// <param name="idsFuncs">Lista com os ids de funcionários</param>
        /// <param name="dtIni">Data inicial para geração</param>
        /// <param name="dtFin">Data final para geração</param>
        /// <param name="objProgressBar">Objeto progress bar</param>
        /// <param name="ordemRelatorio">Ordem do relatório (0 = Ordenado por empresa, depois nome do funcionário; 1 = Ordenado por nome de funcionário)</param>
        /// <returns></returns>
        public IList<pxyCartaoPontoEmployer> BuscaDadosRelatorio(IList<int> idsFuncs, DateTime dtIni, DateTime dtFin, ProgressBar? objProgressBar, int ordemRelatorio, bool quebraAuto)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usuarioLogado);
            BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usuarioLogado);
            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(conn, usuarioLogado);
            BLL.Jornada bllJornada = new BLL.Jornada(conn, usuarioLogado);
            BLL.Afastamento bllAfas = new BLL.Afastamento(conn, usuarioLogado);
            BLL.Justificativa bllJust = new BLL.Justificativa(conn, usuarioLogado);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, usuarioLogado);
            IList<pxyCartaoPontoEmployer> cps = new List<pxyCartaoPontoEmployer>();
            BLL.Contrato bllContrato = new BLL.Contrato(conn, usuarioLogado);

            try
            {
                IList<Modelo.Justificativa> justificativas = bllJust.GetAllList(false);
                if (objProgressBar != null)
                {
                    objProgressBar.GetValueOrDefault().setaMensagem("Carregando dados do(s) " + idsFuncs.Count() + " funcionário(s)");
                }
                IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> funcs = bllFuncionario.GetFuncionariosCabecalhoRel(idsFuncs);

                IList<Horario> horario = new List<Horario>();

                int progress = 0;
                int totalCalc = funcs.Count();
                if (objProgressBar != null)
                {
                    objProgressBar.GetValueOrDefault().setaMinMaxPB(0, totalCalc);
                    objProgressBar.GetValueOrDefault().setaValorPB(progress);
                }

                IList<Modelo.Empresa> emps = bllEmpresa.GetEmpresaByIds(funcs.Select(s => s.IdEmpresa).Distinct().ToList());
                //Verifica quantas quebras pode ter o período e multiplica na quantidade do progress bar
                int qtdQuebraPeriodo = QuebraPeridoPorPeriodoFechamento(dtIni, dtFin, bllContrato, emps, funcs.FirstOrDefault(), quebraAuto).Count();
                if (qtdQuebraPeriodo > 0)
                {
                    totalCalc *= qtdQuebraPeriodo;
                }


                // Ordem do relatório, 0 = Ordenado por empresa, 1 = Ordenado por Funcionário
                if (ordemRelatorio == 0)
                {
                    funcs = funcs.OrderBy(o => o.EmpresaNome).ThenBy(o => o.Nome).ToList();
                }
                else
                {
                    funcs = funcs.OrderBy(o => o.Nome).ToList();
                }

                foreach (Modelo.Proxy.PxyFuncionarioCabecalhoRel func in funcs)
                {
                    Dictionary<DateTime, DateTime> periodos = QuebraPeridoPorPeriodoFechamento(dtIni, dtFin, bllContrato, emps, func, quebraAuto);

                    foreach (KeyValuePair<DateTime, DateTime> periodo in periodos.OrderBy(i => i.Key))
                    {
                        progress++;
                        DateTime dataIniPer = periodo.Key;
                        DateTime dataFinPer = periodo.Value;

                        if (objProgressBar != null)
                        {
                            objProgressBar.GetValueOrDefault().setaValorPB(progress);
                        }

                        pxyCartaoPontoEmployer cp = new pxyCartaoPontoEmployer();
                        cp.QuebraAutHTML = quebraAuto;
                        cp.pxyFuncionarioCabecalhoRel = func;
                        List<Modelo.Marcacao> marcs = bllMarcacao.GetCartaoPontoV2(new List<int> { func.IdFunc }, dataIniPer, dataFinPer);
                        TimeSpan ts = dataFinPer - dataIniPer;
                        if (quebraAuto == false)
                        {   //Numero linhas de paginas + 8 linha de marc por cada dia. 
                            cp.NumeroLinha = ts.Days * 8;
                        }
                        if (ts.TotalDays + 1 > marcs.Count())
                        {
                            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                            objFuncionario = bllFuncionario.LoadObject(func.IdFunc);

                            bllMarcacao.AtualizaData(dataIniPer, dataFinPer, objFuncionario);
                            marcs = bllMarcacao.GetCartaoPontoV2(new List<int> { func.IdFunc }, dataIniPer, dataFinPer);
                        }

                        IList<Modelo.Jornada> jornadas = new List<Modelo.Jornada>();
                        IList<Modelo.Proxy.pxyAbonosPorMarcacao> abonos = bllAfas.GetAbonosPorMarcacoes(new List<int> { func.IdFunc }, dataIniPer, dataFinPer);

                        DadosCartao(dataIniPer, dataFinPer, cp);


                        foreach (Modelo.Marcacao marc in marcs)
                        {
                            if (objProgressBar != null)
                            {
                                objProgressBar.GetValueOrDefault().setaMensagem("(" + progress.ToString() + "/" + totalCalc.ToString() + ") " + "Calculando dados do funcionário: " + func.Nome);
                            }
                            PxyCPEMarcacao cpeMarc = new PxyCPEMarcacao();
                            IList<Modelo.BilhetesImp> bils = marc.BilhetesMarcacao.Where(w => w.Mar_data.GetValueOrDefault() == Convert.ToDateTime(marc.Data) && w.DsCodigo == marc.Dscodigo).ToList();

                            CalculoHorasNoturnas(marc, cpeMarc);

                            DadosMarcacao(marc, cpeMarc, bils);

                            cpeMarc.pxyCPEJornadaRealizada = JornadaRealizada(bils);

                            IList<PxyCPETratamentos> trats = Tratamentos(cpeMarc, bils, abonos.Where(x => x.IdMarcacao == marc.Id).ToList(), justificativas, marc);

                            MotivosIndices(cp, trats);

                            cpeMarc.pxyCPETratamentos = trats;

                            JornadasFunc(bllJornada, cp, jornadas, marc, cpeMarc);

                            cp.Marcacao.Add(cpeMarc);
                        }


                        Totalizadores(conn, usuarioLogado, dataIniPer, dataFinPer, func, cp);

                        SetaPadroesParaRel(cp);

                        cps.Add(cp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cps;
        }

        private Dictionary<DateTime, DateTime> QuebraPeridoPorPeriodoFechamento(DateTime dtIni, DateTime dtFin, BLL.Contrato bllContrato, IList<Modelo.Empresa> emps, Modelo.Proxy.PxyFuncionarioCabecalhoRel func, bool quebraAuto)
        {
            Dictionary<DateTime, DateTime> periodos = new Dictionary<DateTime, DateTime>();
            
            if ((dtFin - dtIni).TotalDays > 31 && quebraAuto == true)
            {
                int inicioFechamento, fimfechamento;
                DateTime inicioPeriodo, fimPeriodo;
                GetInicioFimFechamentoPonto(bllContrato, emps, func, out inicioFechamento, out fimfechamento);
                inicioPeriodo = dtIni;
                while (true)
                {
                    if (fimfechamento == 0)
                    {
                        fimPeriodo = cwkFuncoes.LastDayDate(inicioPeriodo);
                    }
                    else
                    {
                        fimPeriodo = Convert.ToDateTime(fimfechamento + "/" + inicioPeriodo.Month + "/" + inicioPeriodo.Year);
                    }
                    if (inicioPeriodo >= fimPeriodo)
                    {
                        fimPeriodo = fimPeriodo.AddMonths(1);
                    }
                    if (fimPeriodo >= dtFin)
                    {
                        periodos.Add(inicioPeriodo, dtFin);
                        break;
                    }
                    else
                    {
                        periodos.Add(inicioPeriodo, fimPeriodo);
                        inicioPeriodo = fimPeriodo.AddDays(1);
                    }
                }
            }
            else 
            {
                periodos.Add(dtIni, dtFin);
            }
            return periodos;
        }

        private void GetInicioFimFechamentoPonto(BLL.Contrato bllContrato, IList<Modelo.Empresa> emps, Modelo.Proxy.PxyFuncionarioCabecalhoRel func, out int inicioFechamento, out int fimfechamento)
        {
            inicioFechamento = 0;
            fimfechamento = 0;
            bool encontrou = false;

            if (usuarioLogado.UtilizaControleContratos)
            {
                Modelo.Contrato contrato = new Modelo.Contrato();
                contrato = bllContrato.ContratosPorFuncionario(func.IdFunc).FirstOrDefault();
                if (usuarioLogado.UtilizaControleContratos && contrato != null && contrato.Id > 0)
                {
                    inicioFechamento = contrato.DiaFechamentoInicial;
                    fimfechamento = contrato.DiaFechamentoFinal;
                    encontrou = true;
                }
            }

            if (!encontrou || (inicioFechamento == 0 && fimfechamento == 0))
            {
                Modelo.Empresa emp = emps.Where(x => x.Id == func.IdEmpresa).FirstOrDefault();
                if (emp != null && emp.Id > 0)
                {
                    inicioFechamento = emp.DiaFechamentoInicial;
                    fimfechamento = emp.DiaFechamentoFinal;
                    encontrou = true;
                }
            }

            if (!encontrou || (inicioFechamento == 0 && fimfechamento == 0))
            {
                Parametros bllParam = new Parametros(conn, usuarioLogado);
                Modelo.Parametros param = bllParam.LoadPrimeiro();
                if (param != null && param.Id > 0)
                {
                    inicioFechamento = param.DiaFechamentoInicial;
                    fimfechamento = param.DiaFechamentoFinal;
                }
            }
        }

        private static void SetaPadroesParaRel(pxyCartaoPontoEmployer cp)
        {
            if (cp.Jornadas.Count() == 0)
            {
                cp.Jornadas.Add(new Modelo.Jornada());
            }

            if (cp.MotivosTratamentos.Count() == 0)
            {
                cp.MotivosTratamentos.Add(new PxyCPEMotivosTratamento());
            }
        }

        private static void Totalizadores(string conn, Modelo.Cw_Usuario usr, DateTime dtIni, DateTime dtFin, Modelo.Proxy.PxyFuncionarioCabecalhoRel func, pxyCartaoPontoEmployer cp)
        {
            Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(dtIni, dtFin);
            var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(func.IdEmpresa, func.IdDepartamento, func.IdFunc, func.IdFuncao, dtIni, dtFin, conn, usr);
            totalizadorHoras.TotalizeHorasEBancoHoras(objTotalHoras);

            cp.Totalizador = new PxyCPETotais();
            cp.Totalizador.TrabalhadasDiurnas = objTotalHoras.horasTrabDiurna;
            cp.Totalizador.TrabalhadasNoturnas = objTotalHoras.horasTrabNoturna;
            cp.Totalizador.ExtrasDiurnas = objTotalHoras.horasExtraDiurna;
            cp.Totalizador.ExtrasNoturnas = objTotalHoras.horasExtraNoturna;
            cp.Totalizador.FaltasAtrasos = Modelo.cwkFuncoes.ConvertMinutosHora2(3, objTotalHoras.horasFaltaDiurnaMin + objTotalHoras.horasFaltaNoturnaMin);
            cp.Totalizador.TrabalhadasNoturnas = objTotalHoras.horasTrabNoturna;
            cp.Totalizador.SaldoAnteriorBH = objTotalHoras.sinalSaldoAnteriorBH + " " + objTotalHoras.saldoAnteriorBH;
            cp.Totalizador.SaldoAtualBH = objTotalHoras.sinalSaldoBHAtual + " " + objTotalHoras.saldoBHAtual;
            cp.Totalizador.TotalTrabalhada = Modelo.cwkFuncoes.ConvertMinutosHora2(3, cp.Marcacao.Sum(s => Modelo.cwkFuncoes.ConvertHorasMinuto(s.TotalHorasTrabalhadas)));
            cp.Totalizador.TotalTrabalhadaComHorasNoturnas = Modelo.cwkFuncoes.ConvertMinutosHora2(3, cp.Marcacao.Sum(s => Modelo.cwkFuncoes.ConvertHorasMinuto(s.TotalHorasTrabalhadasComHorasNoturnas)));
            cp.Totalizador.HorasAdNoturna = objTotalHoras.horasAdNoturno;
        }

        private static void CalculoHorasNoturnas(Modelo.Marcacao marc, PxyCPEMarcacao cpeMarc)
        {
            int[] Entradas = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] Saidas = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            marc.GetEntradasSaidasValidas(ref Entradas, ref Saidas);

            int HoraDiurna = 0;
            int HoraNoturna = 0;

            BLL.CalculoHoras.QtdHorasDiurnaNoturna(Entradas, Saidas, Modelo.cwkFuncoes.ConvertHorasMinuto(marc.InicioAdNoturno), Modelo.cwkFuncoes.ConvertHorasMinuto(marc.FimAdNoturno), ref HoraDiurna, ref HoraNoturna);
            cpeMarc.TotalHorasDiurnas = Modelo.cwkFuncoes.ConvertMinutosHora(HoraDiurna);
            cpeMarc.TotalHorasNoturnas = Modelo.cwkFuncoes.ConvertMinutosHora(HoraNoturna);
        }

        private static void DadosCartao(DateTime dtIni, DateTime dtFin, pxyCartaoPontoEmployer cp)
        {
            cp.Periodo = dtIni.ToShortDateString() + " a " + dtFin.ToShortDateString();
            cp.Marcacao = new List<PxyCPEMarcacao>();
            cp.Jornadas = new List<Modelo.Jornada>();
            cp.MotivosTratamentos = new List<Modelo.Proxy.PxyCPEMotivosTratamento>();
        }

        private static void DadosMarcacao(Modelo.Marcacao marc, PxyCPEMarcacao cpeMarc, IList<Modelo.BilhetesImp> bils)
        {
            cpeMarc.Data = marc.Data.ToShortDateString();
            cpeMarc.DataFormat = marc.Data.ToString("dd/MM/yy");
            cpeMarc.Dia = marc.Dia;
            cpeMarc.MarcDesconsiderada = String.Join(" - ", bils.Where(x => x.Ocorrencia == 'D').Select(s => s.Mar_hora));
            cpeMarc.Ocorrencias = marc.Ocorrencia;
            cpeMarc.HorasDiurnas = marc.Horastrabalhadas;
            cpeMarc.HorasNoturnas = marc.Horastrabalhadasnoturnas;
            cpeMarc.HorasExtrasDiurnas = marc.Horasextrasdiurna;
            cpeMarc.HorasExtrasNoturnas = marc.Horasextranoturna;
            cpeMarc.FaltasAtrasos = Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horasfaltas) + Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horasfaltanoturna));
            cpeMarc.FaltasDiurna = marc.Horasfaltas;
            cpeMarc.FaltasNoturna = marc.Horasfaltanoturna;
            cpeMarc.InItinereHrsDentroJornada = marc.InItinereHrsDentroJornada;
            cpeMarc.InItinerePercDentroJornada = marc.InItinerePercDentroJornada;
            cpeMarc.InItinereHrsForaJornada = marc.InItinereHrsForaJornada;
            cpeMarc.InItinerePercForaJornada = marc.InItinerePercForaJornada;
            cpeMarc.Interjornada = marc.Interjornada;
            cpeMarc.horaExtraInterjornada = marc.horaExtraInterjornada;

            cpeMarc.BancoHorasCredito = marc.Bancohorascre;
            cpeMarc.BancoHorasDebito = marc.Bancohorasdeb;

            int BHDeb = Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Bancohorasdeb);
            int BHCre = Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Bancohorascre);
            int SaldoBH = BHCre - BHDeb;
            if (SaldoBH < 0)
                cpeMarc.BancoHoras = "-" + Modelo.cwkFuncoes.ConvertMinutosHora2(3, Math.Abs(SaldoBH));
            else if (SaldoBH > 0) cpeMarc.BancoHoras = "+" + Modelo.cwkFuncoes.ConvertMinutosHora2(3, SaldoBH);
            else cpeMarc.BancoHoras = "---:--";



            cpeMarc.ConversaoHoraNoturna = marc.ConversaoHoraNoturna;
            cpeMarc.ReducaoHoraNoturna = marc.ReducaoHoraNoturna;
            if (cpeMarc.TotalHorasNoturnas != "--:--")
            {
                int horasNoturnas = Modelo.cwkFuncoes.ConvertHorasMinuto(cpeMarc.TotalHorasNoturnas);
                cpeMarc.HoraNoturnaMin = BLL.CalculoHoras.HoraNoturna(horasNoturnas, marc.ReducaoHoraNoturna);
                cpeMarc.TotalHorasNoturnasComReducao = Modelo.cwkFuncoes.ConvertMinutosHora(cpeMarc.HoraNoturnaMin);
                cpeMarc.HoraNoturnaMin -= horasNoturnas;
            }
            else
            {
                cpeMarc.HoraNoturnaMin = 0;
                cpeMarc.TotalHorasNoturnasComReducao = "--:--";
            }

            if (marc.FolgaCompensado != "Folga")
            {
                cpeMarc.TotalHorasTrabalhadas = Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horastrabalhadas) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horastrabalhadasnoturnas) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horasextrasdiurna) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horasextranoturna) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Bancohorascre) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.InItinereHrsDentroJornada) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.InItinereHrsForaJornada));
            }
            else
            {
                cpeMarc.TotalHorasTrabalhadas = Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horasextrasdiurna) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Horasextranoturna) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.Bancohorascre) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.InItinereHrsDentroJornada) +
                                                                                 Modelo.cwkFuncoes.ConvertHorasMinuto(marc.InItinereHrsForaJornada));
            }

            if (cpeMarc.ConversaoHoraNoturna == 1 && marc.FolgaCompensado != "Folga")
            {
                cpeMarc.TotalHorasTrabalhadasComHorasNoturnas = cpeMarc.TotalHorasTrabalhadas;
                cpeMarc.TotalHorasTrabalhadas = Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(cpeMarc.TotalHorasTrabalhadas) - cpeMarc.HoraNoturnaMin);

            }
            else
            {
                cpeMarc.TotalHorasTrabalhadasComHorasNoturnas = Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(cpeMarc.TotalHorasTrabalhadas) + cpeMarc.HoraNoturnaMin);
            }

            if (marc.CredInclusaoBanco != "---:--" && marc.CredInclusaoBanco != null && marc.DebInclusaoBanco != "---:--" && marc.DebInclusaoBanco != null)
            {
                marc.Legenda = "BH";
            }
        }

        private static void MotivosIndices(pxyCartaoPontoEmployer cp, IList<PxyCPETratamentos> trats)
        {
            foreach (PxyCPETratamentos trat in trats.Where(x => !String.IsNullOrEmpty(x.Motivo)))
            {
                PxyCPEMotivosTratamento mt = cp.MotivosTratamentos.Where(x => x.Motivo == trat.Motivo).FirstOrDefault();
                if (mt == null)
                {
                    mt = new PxyCPEMotivosTratamento();
                    mt.Motivo = trat.Motivo;
                    int indice = 1;
                    if (cp.MotivosTratamentos.Count() > 0)
                    {
                        indice = cp.MotivosTratamentos.Max(s => s.Indice) + 1;
                    }
                    mt.Indice = indice;
                    cp.MotivosTratamentos.Add(mt);
                }
                trat.IndiceMotivo = mt.Indice;
            }
        }

        private static void JornadasFunc(BLL.Jornada bllJornada, pxyCartaoPontoEmployer cp, IList<Modelo.Jornada> jornadas, Modelo.Marcacao marc, PxyCPEMarcacao cpeMarc)
        {
            cpeMarc.CodJornada = marc.FolgaCompensado;
            cpeMarc.JornadaEmLinha = marc.FolgaCompensado;
            if (String.IsNullOrEmpty(cpeMarc.CodJornada))
            {
                Modelo.Jornada j = cp.Jornadas.Where(x => x.Id == marc.IdJornada).FirstOrDefault();
                if (j == null || j.Id < 0)
                {
                    j = jornadas.Where(x => x.Id == marc.IdJornada).FirstOrDefault();
                    if (j == null || j.Id < 0)
                    {
                        j = bllJornada.LoadObject(marc.IdJornada);
                        jornadas.Add(j);
                        cp.Jornadas.Add(j);
                    }
                    else
                    {
                        cp.Jornadas.Add(j);
                    }
                }
                if (j != null && j.Id > 0)
                {
                    cpeMarc.CodJornada = j.Codigo.ToString();
                    if (j.Entrada_1 != "--:--")
                    {
                        cpeMarc.JornadaEmLinha = j.Entrada_1 + "-" + j.Saida_1;
                    }
                    if (j.Entrada_2 != "--:--")
                    {
                        cpeMarc.JornadaEmLinha += "-" + j.Entrada_2 + "-" + j.Saida_2;
                    }
                    if (j.Entrada_3 != "--:--")
                    {
                        cpeMarc.JornadaEmLinha += "-" + j.Entrada_3 + "-" + j.Saida_3;
                    }
                    if (j.Entrada_4 != "--:--")
                    {
                        cpeMarc.JornadaEmLinha += "-" + j.Entrada_4 + "-" + j.Saida_4;
                    }
                }
            }
        }

        private IList<PxyCPETratamentos> Tratamentos(PxyCPEMarcacao cpeMarc, IList<Modelo.BilhetesImp> bils, IList<Modelo.Proxy.pxyAbonosPorMarcacao> abonos, IList<Modelo.Justificativa> justificativas, Modelo.Marcacao marc)
        {
            IList<PxyCPETratamentos> trs = new List<PxyCPETratamentos>();
            foreach (Modelo.BilhetesImp bi in bils.Where(x => x.Ocorrencia == 'I' || x.Ocorrencia == 'P'))
            {
                string motivo = "";
                PxyCPETratamentos tr = new PxyCPETratamentos();
                tr.Horario = bi.Mar_hora;
                tr.Ocorrencia = bi.Ocorrencia.ToString();
                if (bi.Ocorrencia == 'I')
                {
                    Modelo.Justificativa just = justificativas.Where(x => x.Id == bi.Idjustificativa).FirstOrDefault();
                    if (just == null)
                    {
                        just = new Modelo.Justificativa();
                    }
                    motivo = just.Descricao;
                }
                else if (bi.Ocorrencia == 'P')
                {
                    motivo = "PRÉ-ASSINALADA";
                }
                tr.Motivo = motivo.ToUpper();
                trs.Add(tr);
            }

            foreach (Modelo.Proxy.pxyAbonosPorMarcacao abono in abonos)
            {
                PxyCPETratamentos tr = new PxyCPETratamentos();
                tr.Horario = abono.AbonoTotal;
                tr.Ocorrencia = "A";
                tr.Motivo = abono.DescricaoOcorrencia.ToUpper();
                trs.Add(tr);
            }

            if (Modelo.cwkFuncoes.ConvertHorasMinuto(cpeMarc.TotalHorasNoturnas) > 0)
            {
                PxyCPETratamentos tr = new PxyCPETratamentos();
                tr.Horario = cpeMarc.TotalHorasNoturnasComReducao;
                tr.Ocorrencia = "AN";
                tr.Motivo = "ADICIONAL NOTURNO";
                trs.Add(tr);
            }

            if (trs.Count() == 0)
            {
                trs.Add(new PxyCPETratamentos());
            }

            if (marc.CredInclusaoBanco != "---:--" && marc.CredInclusaoBanco != null)
            {
                PxyCPETratamentos tr = new PxyCPETratamentos();
                tr.Ocorrencia = "BH";
                if (marc.Justificativa != null)
                {
                    tr.Motivo = marc.Justificativa;
                }
                else
                {
                    tr.Motivo = "Lançamento Crédito/Débito Banco Horas";
                }
                tr.Horario = marc.CredInclusaoBanco;
                trs.Add(tr);
            }
            else if (marc.DebInclusaoBanco != "---:--" && marc.DebInclusaoBanco != null)
            {
                PxyCPETratamentos tr = new PxyCPETratamentos();
                tr.Ocorrencia = "BH";
                if (marc.Justificativa != null)
                {
                    tr.Motivo = marc.Justificativa;
                }
                else
                {
                    tr.Motivo = "Lançamento Crédito/Débito Banco Horas";
                }
                tr.Horario = "-" + marc.DebInclusaoBanco;
                trs.Add(tr);
            }

            return trs;
        }

        private IList<PxyCPEJornadaRealizada> JornadaRealizada(IList<Modelo.BilhetesImp> bils)
        {
            IList<PxyCPEJornadaRealizada> jrs = new List<PxyCPEJornadaRealizada>();
            int qtdBilhetes = bils.Where(x => x.Ocorrencia != 'D').Count();
            int qtdLinha = 1;
            if (qtdBilhetes > 4)
            {
                qtdLinha = (int)(Math.Ceiling((double)qtdBilhetes / (double)4));
            }
            int posBil = 0;
            for (int i = 0; i < qtdLinha; i++)
            {
                PxyCPEJornadaRealizada jr = new PxyCPEJornadaRealizada();
                posBil++;
                Modelo.BilhetesImp bi = bils.Where(x => x.Ent_sai == "E" && x.Posicao == posBil && x.Ocorrencia != 'D').FirstOrDefault();
                if (bi != null)
                    jr.Entrada1 = bi.Mar_hora;

                bi = bils.Where(x => x.Ent_sai == "S" && x.Posicao == posBil && x.Ocorrencia != 'D').FirstOrDefault();
                if (bi != null)
                    jr.Saida1 = bi.Mar_hora;

                posBil++;
                bi = bils.Where(x => x.Ent_sai == "E" && x.Posicao == posBil && x.Ocorrencia != 'D').FirstOrDefault();
                if (bi != null)
                    jr.Entrada2 = bi.Mar_hora;

                bi = bils.Where(x => x.Ent_sai == "S" && x.Posicao == posBil && x.Ocorrencia != 'D').FirstOrDefault();
                if (bi != null)
                {
                    jr.Saida2 = bi.Mar_hora;
                }
                jrs.Add(jr);
            }
            return jrs;
        }
    }
}
