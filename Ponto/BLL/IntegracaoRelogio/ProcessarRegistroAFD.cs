using cwkPontoMT.Integracao;
using cwkPontoMT.Integracao.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;

namespace BLL.IntegracaoRelogio
{
    public class ProcessarRegistroAFD
    {
        private Modelo.REP objRep;
        private Modelo.Cw_Usuario usuarioLogado;
        private string conexao;
        private BLL.REP bllRep;
        public ProcessarRegistroAFD(Modelo.REP rep, string conexao, Modelo.Cw_Usuario usuarioLogado)
        {
            this.bllRep = new BLL.REP(conexao, usuarioLogado);
            this.objRep = rep;
            this.conexao = conexao;
            this.usuarioLogado = usuarioLogado;
        }

        public ResultadoImportacao ProcessarImportacao(List<int> idsFuncsImportar, List<RegistroAFD> registros)
        {
            ResultadoImportacao resultado = new ResultadoImportacao();
            resultado.InicioProcessamento = DateTime.Now;
            if (registros.Count() > 0)
            {
                AFD bllAFD = new BLL.AFD(conexao, usuarioLogado);
                List<Modelo.AFD> linhasAFD = PreparaESalvaAFD(registros, bllAFD, objRep.NumRelogio);
                try
                {
                    #region Valida Dados
                    string numeroRelogio = ValidaLinhasAFD(idsFuncsImportar, registros);
                    VerificaRegistrosEmProcessamento(registros, numeroRelogio);
                    VerificaRegistrosJaIncluidos(registros, numeroRelogio);
                    #endregion
                    AtualizaRetornoAFD(registros, linhasAFD, bllAFD);
                    ProcessarRegistrosPonto(linhasAFD);
                }
                catch (Exception e)
                {
                    linhasAFD.ForEach(f => { f.Observacao = "Erro: "+e.Message; f.Situacao = Modelo.EnumSituacaoAFD.ErroDesconhecido; });
                    bllAFD.AtualizarRegistros(linhasAFD);
                    throw;
                }
            }
            GeraResumoRetorno(ref resultado, registros);
            return resultado;
        }

        private void ProcessarRegistrosPonto(List<Modelo.AFD> linhasAFD)
        {
            List<Modelo.AFD> linhasAFDProcessar = linhasAFD.Where(w => w.Situacao == Modelo.EnumSituacaoAFD.RegistroProcessado).ToList();
            if (linhasAFDProcessar.Count > 0)
            {
                RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(conexao, usuarioLogado);
                int ultimoCodigo = bllRegistroPonto.MaxCodigo();
                List<Modelo.RegistroPonto> registrosProcessar = new List<Modelo.RegistroPonto>();
                foreach (Modelo.AFD linha in linhasAFDProcessar)
                {
                    Modelo.RegistroPonto reg = new Modelo.RegistroPonto();
                    reg.Acao = 0;
                    reg.Batida = DateTime.ParseExact(linha.Campo04 + " " + linha.Campo05, "ddMMyyyy HHmm", CultureInfo.InvariantCulture);
                    reg.Codigo = ultimoCodigo++;
                    reg.OrigemRegistro = linha.Relogio;
                    reg.Situacao = "I";
                    reg.IdFuncionario = linha.IdFuncionario;
                    reg.IpInterno = linha.IpDnsRep;
                    reg.Chave = linha.Identificador;
                    reg.Lote = linha.Lote.ToString();
                    reg.NSR = linha.Nsr;
                    registrosProcessar.Add(reg);
                }

                bllRegistroPonto.InserirRegistros(registrosProcessar);
            }
            string numRelogio = linhasAFD.FirstOrDefault().Relogio;

            Modelo.AFD ultimoReg = new Modelo.AFD();
            // Se entre os registros algum deu erro que precisa reprocessar o lote, adiciona como ultimo registro o primeiro do lote, para que o rep envie os dados novamente.
            if (linhasAFD.Where(w => w.Situacao == Modelo.EnumSituacaoAFD.ErroDesconhecido || w.Situacao == Modelo.EnumSituacaoAFD.EmpresaNaoEncontrada || w.Situacao == Modelo.EnumSituacaoAFD.RepNaoCadastrado).Count() > 0)
            {
                ultimoReg = linhasAFD.Where(w => w.Nsr > 0).OrderBy(o => o.Nsr).FirstOrDefault();
            }
            else // Se não tem erro, siginifca que não foi processado por não haver necessidade, portanto adiciona o último do lote
            {
                ultimoReg = linhasAFD.Where(w => w.Nsr > 0).Where(w => w.Nsr != 999999999).OrderByDescending(o => o.Nsr).FirstOrDefault();
            }
            if (ultimoReg.DataHora == null)
            {
                try
                {
                    ultimoReg.PreencheDataHora();
                }
                catch (Exception)
                {

                    ultimoReg = linhasAFD.Where(w => w.Nsr > 0).Where(w => w.Campo02 != "3").OrderByDescending(o => o.Nsr).FirstOrDefault();
                }
            }
            bllRep.SetUltimaImportacao(numRelogio, ultimoReg.Nsr, ultimoReg.DataHora.GetValueOrDefault());

            Modelo.RepLog repLog = new Modelo.RepLog() { IdRep = objRep.Id, Comando = "Processamento AFD", DescricaoExecucao = "Registro adicionado na fila para processamento, para maiores detalhes <a href=\"/RepLog/ResultadoImportacao?chave=" + linhasAFD.FirstOrDefault().Lote + "\"  target=\"_blank\"> Clique Aqui </a>", Executor = "ServidorPontofopag", Status = 2 };
            BLL.RepLog bllRepLog = new BLL.RepLog(conexao, usuarioLogado);
            bllRepLog.Salvar(Modelo.Acao.Incluir, repLog);
        }

        #region Tratar e Importar Dados do AFD
        private void AtualizaRetornoAFD(List<RegistroAFD> registros, List<Modelo.AFD> linhasAFD, AFD bllAFD)
        {
            foreach (Modelo.AFD afd in linhasAFD)
            {
                RegistroAFD reg = registros.Where(w => w.Identificador == afd.Identificador).FirstOrDefault();
                if (reg != null)
                {
                    afd.IdFuncionario = reg.IdFuncionario;
                    afd.Situacao = (Modelo.EnumSituacaoAFD)(int)reg.StatusColeta;
                    afd.Observacao = afd.DescSituacaoRegistro();
                    if (reg.DataHoraRegistro != DateTime.MinValue)
                    {
                        afd.DataHora =  reg.DataHoraRegistro;
                    }
                }
            }

            bllAFD.AtualizarRegistros(linhasAFD);
        }

        private List<Modelo.AFD> PreparaESalvaAFD(List<RegistroAFD> registros, AFD bllAFD, string numeroRelogio)
        {
            Guid lote = Guid.NewGuid();
            registros.ForEach(f => { f.Identificador = Guid.NewGuid(); f.Lote = lote;  f.LinhaAFD = String.IsNullOrEmpty(f.LinhaAFD) ? (f.Campo01 +
                f.Campo02 +
                f.Campo03 +
                f.Campo04 +
                f.Campo05 +
                f.Campo06 +
                f.Campo07 +
                f.Campo08 +
                f.Campo09 +
                f.Campo10 +
                f.Campo11 +
                f.Campo12) : f.LinhaAFD; });

            List<Modelo.AFD> linhasAFD = new List<Modelo.AFD>();
            Modelo.EnumOrgaoFiscalizador orgaoFiscalizador = Modelo.EnumOrgaoFiscalizador.MTE;
            if (registros.Where(w => w.Campo02 == "3").Count() > 0)
            {
                RegistroAFD reg = registros.Where(w => w.Campo02 == "3").FirstOrDefault();
                if (reg.LinhaAFD != null && reg.LinhaAFD.Length > 236)
                {
                    orgaoFiscalizador = Modelo.EnumOrgaoFiscalizador.Inmetro;
                }   
            }

            registros.ForEach(reg => linhasAFD.Add(new Modelo.AFD { LinhaAFD = reg.LinhaAFD, EnumOrgaoFiscalizador = orgaoFiscalizador, Identificador = reg.Identificador, Lote = reg.Lote, Situacao = 0, Nsr = reg.Nsr, IpDnsRep = objRep.IP, Campo01 = reg.Campo01, Campo02 = reg.Campo02, Campo03 = reg.Campo03, Campo04 = reg.Campo04, Campo05 = reg.Campo05, Campo06 = reg.Campo06, Campo07 = reg.Campo07, Campo08 = reg.Campo08, Campo09 = reg.Campo09, Campo10 = reg.Campo10, Campo11 = reg.Campo11, Campo12 = reg.Campo12, Relogio = numeroRelogio }));
            bllAFD.InserirRegistros(linhasAFD);

            linhasAFD = bllAFD.GetAllListByLote(lote.ToString(), true);
            return linhasAFD;
        }

        private string ValidaLinhasAFD(List<int> idsFuncsImportar, List<RegistroAFD> registros)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conexao, usuarioLogado);
            List<string> lPis = registros.Where(w => w.Campo02 == "3").Select(s => s.Campo06).Distinct().ToList();
            //Busca todos os funcionários existentes no banco.
            List<Modelo.Funcionario> funcsBilhetes = bllFuncionario.GetAllListPorPis(lPis);
            //Busca todos os funcionários ativos, que não estão excluídos e que o usuário possua permissão
            List<Modelo.Funcionario> funcsPermitidos = bllFuncionario.GetAllListComUltimosFechamentos(false, funcsBilhetes.Select(s => s.Id).ToList());
            //Dicionario para agilizar a validação.
            IList<ValidaColeta> ValidaColeta = new List<ValidaColeta>();
            string numeroRelogio = null;
            //int codigoBilhete = bllBilheteSimp.MaxCodigo();

            RegistroAFD cabecalho = registros.Where(reg => reg.Campo02 == "1").FirstOrDefault();
            bool validar = true;
            if (cabecalho != null)
            {
                //Busca o número do rep
                numeroRelogio = bllRep.GetNumInner(cabecalho.Campo07);
                if (numeroRelogio == null || numeroRelogio == "")
                {
                    registros.ForEach(reg => reg.StatusColeta = SituacaoRegistro.RepNaoCadastrado);
                    validar = false;
                }
                if (!bllRep.GetCPFCNPJ(cabecalho.Campo04, cabecalho.Campo03))
                {
                    registros.ForEach(reg => reg.StatusColeta = SituacaoRegistro.EmpresaNaoEncontrada);
                    validar = false;
                }
            }

            if (validar)
            {
                foreach (RegistroAFD reg in registros)
                {
                    if (reg.Campo01 != "999999999")
                    {
                        switch (reg.Campo02)
                        {
                            case "1":
                                reg.StatusColeta = SituacaoRegistro.RegistroNaoUtilizadoPeloSistema;
                                break;
                            case "2":
                                reg.StatusColeta = SituacaoRegistro.RegistroNaoUtilizadoPeloSistema;
                                break;
                            case "3":
                                reg.PreencheDataHoraRegistro();
                                //Busca o DSCodigo do funcionário pelo PIS e valida a situacao.
                                ValidaColeta coletaValidada = ValidaColeta.Where(p => p.PIS == reg.Campo06).FirstOrDefault();
                                if (coletaValidada == null || String.IsNullOrEmpty(coletaValidada.PIS))
                                {
                                    ValidaSituacaoPIS(idsFuncsImportar, funcsBilhetes, funcsPermitidos, ref ValidaColeta, reg);
                                    coletaValidada = ValidaColeta.Where(p => p.PIS == reg.Campo06).FirstOrDefault();
                                }

                                if (coletaValidada.SituacaoRegistro == null)
                                {
                                    if (reg.DataHoraRegistro.Date <= coletaValidada.UltimoFechamentoPonto || reg.DataHoraRegistro.Date <= coletaValidada.UltimoFechamentoBancoHoras)
                                    {
                                        reg.StatusColeta = SituacaoRegistro.PontoFechado;
                                    }
                                    else
                                    {
                                        //IncluirBilhete(listaBilhetes, numeroRelogio, ref codigoBilhete, reg, dataBil, coletaValidada.dsCodigoFuncionario, coletaValidada.IdFuncionario, coletaValidada.PISCadFuncionario);
                                        reg.StatusColeta = SituacaoRegistro.RegistroProcessado;
                                    }
                                }
                                else
                                {
                                    reg.StatusColeta = coletaValidada.SituacaoRegistro.GetValueOrDefault();
                                }

                                reg.IdFuncionario = coletaValidada.IdFuncionario;
                                break;
                            case "4":
                                reg.StatusColeta = SituacaoRegistro.RegistroNaoUtilizadoPeloSistema;
                                break;
                            case "5":
                                reg.StatusColeta = SituacaoRegistro.RegistroNaoUtilizadoPeloSistema;
                                break;
                            default:
                                reg.StatusColeta = SituacaoRegistro.RegistroNaoUtilizadoPeloSistema;
                                break;
                        }
                    }
                }
            }

            return numeroRelogio;
        }

        /// <summary>
        /// Verifica duplicidade entre registros já processador e alocados em bilhetes
        /// </summary>
        /// <param name="registros">Registros a serem incluidos</param>
        /// <param name="numeroRelogio">Numero do relógio a serem verificadas as batidas</param>
        private void VerificaRegistrosJaIncluidos(List<RegistroAFD> registros, string numeroRelogio)
        {
            List<RegistroAFD> registrosValidos = registros.Where(w => w.StatusColeta == SituacaoRegistro.RegistroProcessado).ToList();
            if (registrosValidos.Count() > 0)
            {
                BLL.BilhetesImp bllBilheteSimp = new BLL.BilhetesImp(conexao, usuarioLogado);
                Hashtable bilhetesExistentesPorPis = bllBilheteSimp.GetHashPorPISPeriodo(null, registrosValidos.Min(m => m.DataHoraRegistro).Date, registrosValidos.Max(m => m.DataHoraRegistro).Date, registrosValidos.Select(s => Convert.ToDecimal(cwkPontoMT.Integracao.Util.GetStringSomenteAlfanumerico(s.Campo06)).ToString()).ToList());
                string keyPis = String.Empty;
                if ( bilhetesExistentesPorPis.Count > 0)
                {
                    foreach (RegistroAFD registro in registrosValidos)
                    {
                        keyPis = registro.DataHoraRegistro.ToShortDateString() + registro.DataHoraRegistro.ToString("HH:mm") + Convert.ToDecimal(cwkPontoMT.Integracao.Util.GetStringSomenteAlfanumerico(registro.Campo06)) + numeroRelogio;

                        if (bilhetesExistentesPorPis.ContainsKey(keyPis))
                        {
                            registro.StatusColeta = SituacaoRegistro.RegistroProcessadoAnteriormente;
                        }
                    }
                } 
            }
        }

        /// <summary>
        /// Busca os registros que já estão na fila para serem processados incluidos por outro processo
        /// </summary>
        /// <param name="registros">Registros a serem incluidos</param>
        /// <param name="numeroRelogio">Numero do relógio a serem verificadas as batidas</param>
        private void VerificaRegistrosEmProcessamento(List<RegistroAFD> registros, string numeroRelogio)
        {
            #region Verifica duplicidade entre registros em processamento
            List<RegistroAFD> registrosValidos = registros.Where(w => w.StatusColeta == SituacaoRegistro.RegistroProcessado).ToList();
            if (registrosValidos.Count > 0)
            {
                BLL.RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(conexao, usuarioLogado);
                Hashtable RegistrosExistentesPorPis = bllRegistroPonto.GetHashPorPISPeriodo(registrosValidos.Min(m => m.DataHoraRegistro).Date, registrosValidos.Max(m => m.DataHoraRegistro).Date, registrosValidos.Select(s => Convert.ToDecimal(cwkPontoMT.Integracao.Util.GetStringSomenteAlfanumerico(s.Campo06)).ToString()).ToList());
                string keyPis = String.Empty;
                if (RegistrosExistentesPorPis.Count > 0)
                {
                    foreach (RegistroAFD registro in registrosValidos)
                    {
                        keyPis = registro.DataHoraRegistro.ToShortDateString() + registro.DataHoraRegistro.ToString("HH:mm") + Convert.ToDecimal(cwkPontoMT.Integracao.Util.GetStringSomenteAlfanumerico(registro.Campo06)) + numeroRelogio;

                        if (RegistrosExistentesPorPis.ContainsKey(keyPis))
                        {
                            registro.StatusColeta = SituacaoRegistro.RegistroAguardandoProcessamento;
                        }
                    }
                } 
            }
            #endregion
        }
        #endregion

        private void ValidaSituacaoPIS(List<int> idsFuncsImportar, List<Modelo.Funcionario> funcsBilhetes, List<Modelo.Funcionario> funcsPermitidos, ref IList<ValidaColeta> PisValidado, RegistroAFD reg)
        {
            Modelo.Funcionario func = funcsBilhetes.Where(w => Convert.ToDecimal(cwkPontoMT.Integracao.Util.GetStringSomenteAlfanumerico(w.Pis)) == Convert.ToDecimal(cwkPontoMT.Integracao.Util.GetStringSomenteAlfanumerico(reg.Campo06))).OrderByDescending(x => x.Funcionarioativo).FirstOrDefault();
            Modelo.Funcionario funcPermitido = new Modelo.Funcionario();
            ValidaColeta PisInfoNovo = new ValidaColeta();
            PisInfoNovo.PIS = reg.Campo06;

            if (func != null && func.Id > 0)
            {
                PisInfoNovo.IdFuncionario = func.Id;
                PisInfoNovo.PISCadFuncionario = func.Pis;
                PisInfoNovo.dsCodigoFuncionario = func.Dscodigo;
                funcPermitido = funcsPermitidos.Where(f => f.Id == func.Id).FirstOrDefault();
                if (funcPermitido != null)
                {
                    PisInfoNovo.UltimoFechamentoPonto = funcPermitido.DataUltimoFechamento.GetValueOrDefault();
                    PisInfoNovo.UltimoFechamentoBancoHoras = funcPermitido.DataUltimoFechamentoBH.GetValueOrDefault();
                }

            }

            if (func == null || func.Id == 0)
            {
                PisInfoNovo.SituacaoRegistro = SituacaoRegistro.FuncNaoEncontrado;
            }
            else if (idsFuncsImportar.Count() > 0 && idsFuncsImportar.Where(x => x == func.Id).Count() == 0)
            {
                PisInfoNovo.SituacaoRegistro = SituacaoRegistro.FuncNaoSelecionadoParaImportacao;
            }
            else if (func.Excluido == 1)
            {
                PisInfoNovo.SituacaoRegistro = SituacaoRegistro.FuncExcluido;
            }
            else if (!func.bFuncionarioativo)
            {
                PisInfoNovo.SituacaoRegistro = SituacaoRegistro.FuncInativo;
            }
            else if (funcPermitido == null || funcPermitido.Id == 0)
            {
                PisInfoNovo.SituacaoRegistro = SituacaoRegistro.UsuarioSemPermissao;
            }
            PisValidado.Add(PisInfoNovo);
        }

        private void GeraResumoRetorno(ref ResultadoImportacao resultado, List<RegistroAFD> registros)
        {
            Resumo resumo = new Resumo();
            resumo.RegistroSalvo = registros.Where(w => w.StatusColeta == SituacaoRegistro.RegistroProcessado).Count();
            resumo.FuncDemitido = registros.Where(w => w.StatusColeta == SituacaoRegistro.FuncDemitido).Count();
            resumo.FuncExcluido = registros.Where(w => w.StatusColeta == SituacaoRegistro.FuncExcluido).Count();
            resumo.FuncInativo = registros.Where(w => w.StatusColeta == SituacaoRegistro.FuncInativo).Count();
            resumo.FuncNaoEncontrado = registros.Where(w => w.StatusColeta == SituacaoRegistro.FuncNaoEncontrado).Count();
            resumo.FuncNaoSelecionadoParaImportacao = registros.Where(w => w.StatusColeta == SituacaoRegistro.FuncNaoSelecionadoParaImportacao).Count();
            resumo.RegistroNaoUtilizadoPeloSistema = registros.Where(w => w.StatusColeta == SituacaoRegistro.RegistroNaoUtilizadoPeloSistema).Count();
            resumo.RegistroProcessado = registros.Where(w => w.StatusColeta == SituacaoRegistro.RegistroProcessado).Count();
            resumo.UsuarioSemPermissao = registros.Where(w => w.StatusColeta == SituacaoRegistro.UsuarioSemPermissao).Count();
            resumo.PontoFechado = registros.Where(w => w.StatusColeta == SituacaoRegistro.PontoFechado).Count();

            resultado.FimProcessamento = DateTime.Now;
            resultado.Resumo = resumo;
            resultado.RegistrosAFD = registros;
        }
    }
}
