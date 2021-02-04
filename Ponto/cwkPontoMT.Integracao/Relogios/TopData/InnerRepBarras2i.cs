﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Sdk_Inner_Rep;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao.Relogios.TopData
{
    public class InnerRepBarras2i : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            Sdk_Inner_Rep.InnerRepSdk innerRep = new Sdk_Inner_Rep.InnerRepSdk();
            try
            {
                List<RegistroAFD> registros = new List<RegistroAFD>();
                List<RegistroAFD> retorno = new List<RegistroAFD>();
                /* Cria uma instância da classe InnerRepSdk */
                
                /* Define os parâmetros de comunicação com o REP */
                innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");

                if ((nsrInicio == 0 && nsrFim == 0))
                {
                    int nsr = GetUltimoNsrRep(innerRep);
                    registros = GetRegistrosOrdemDecrescente(dataI, dataF, innerRep, nsr);
                }
                else
                {
                    if (nsrFim == int.MaxValue)
                    {
                        nsrFim = GetUltimoNsrRep(innerRep);
                    }
                    registros = GetRegistrosOrdemDecrescentePorNsr(innerRep, nsrInicio, nsrFim);
                }
                registros = registros.OrderBy(o => o.Nsr).ToList();

                try
                {
                    Util.IncluiRegistro(innerRep.CabecalhoCorrespondenteAosRegistrosColetados(), dataI, dataF, retorno);
                }
                catch (Exception)
                {
                    GeraCabecalhoManual(dataI, dataF, retorno);
                }
                retorno.AddRange(registros);
                Util.IncluiRegistro(innerRep.TrailerCorrespondenteAosRegistrosColetados(), dataI, dataF, retorno);
                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                innerRep.FinalizaLeitura();
            }
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
        }

        private static List<RegistroAFD> GetRegistrosOrdemDecrescente(DateTime dataI, DateTime dataF, Sdk_Inner_Rep.InnerRepSdk innerRep, int nsrInicial)
        {
            int ret;
            int nsr = nsrInicial;
            int statusColeta = 0;
            int resultadoColeta = 0;
            string dadosRegistro;
            int numeroRegistrosLidos = 0;
            bool recebeuUltimoBilhete = false;

            List<RegistroAFD> result = new List<RegistroAFD>();

            try
            {
                while (!recebeuUltimoBilhete)
                {
                    ret = innerRep.SolicitaRegistroDoNsr(nsr);

                    /* Solicita o Status da Leitura.. */
                    statusColeta = innerRep.RecebeStatusLeitura();

                    /* Enquanto Status da leitura estiver em Andamento (1) verifica o status novamente.. */
                    while (statusColeta < 2)
                    {
                        Thread.Sleep(100);
                        statusColeta = innerRep.RecebeStatusLeitura();
                    }

                    /* Status da coleta recebido, verifica se recebeu o registro com sucesso */
                    if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                        (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_REGISTRO))
                    {
                        /* Recebe o resultado da leitura */
                        resultadoColeta = innerRep.RecebeResultadoLeitura();

                        /* Faz a leitura do registro */
                        dadosRegistro = innerRep.LeLinhaDoRegistro();

                        DateTime? dtRegistro = Util.IncluiRegistro(dadosRegistro, dataI, dataF, result);
                        if (dtRegistro.HasValue)
                        {
                            if (dtRegistro.Value < dataI)
                            {
                                recebeuUltimoBilhete = true;
                            }
                        }
                        else
                        {
                            numeroRegistrosLidos++;
                        }

                    }

                    /* Se o status da coleta foi finalizado com ultimo registro, ou se a coleta não possui registros, sai do laço.. */
                    if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_SEM_REGISTRO) ||
                        (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO))
                    {
                        recebeuUltimoBilhete = true;
                    }
                    nsr--;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static List<RegistroAFD> GetRegistrosOrdemDecrescentePorNsr(InnerRepSdk innerRep, int nsrInicial, int nsrFinal)
        {
            int ret;
            int nsr = nsrFinal;
            int statusColeta = 0;
            int resultadoColeta = 0;
            string dadosRegistro;
            int numeroRegistrosLidos = 0;
            bool recebeuUltimoBilhete = false;

            List<RegistroAFD> result = new List<RegistroAFD>();
            if (nsrInicial == nsrFinal)
            {
                return result;
            }

            try
            {
                while (!recebeuUltimoBilhete)
                {
                    ret = innerRep.SolicitaRegistroDoNsr(nsr);

                    /* Solicita o Status da Leitura.. */
                    statusColeta = innerRep.RecebeStatusLeitura();

                    /* Enquanto Status da leitura estiver em Andamento (1) verifica o status novamente.. */
                    while (statusColeta < 2)
                    {
                        Thread.Sleep(50);
                        statusColeta = innerRep.RecebeStatusLeitura();
                    }

                    /* Status da coleta recebido, verifica se recebeu o registro com sucesso */
                    if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                        (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_REGISTRO))
                    {
                        /* Recebe o resultado da leitura */
                        resultadoColeta = innerRep.RecebeResultadoLeitura();

                        /* Faz a leitura do registro */
                        dadosRegistro = innerRep.LeLinhaDoRegistro();

                        Util.IncluiRegistro(dadosRegistro, null, null, result);
                        if (result.Where(w => w.Nsr > 0 && w.Nsr < 999999999).Min(m => m.Nsr) < nsrInicial)
                        {
                            recebeuUltimoBilhete = true;
                        }
                        else
                        {
                            numeroRegistrosLidos++;
                        }

                    }

                    /* Se o status da coleta foi finalizado com ultimo registro, ou se a coleta não possui registros, sai do laço.. */
                    if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_SEM_REGISTRO) ||
                        (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO))
                    {
                        recebeuUltimoBilhete = true;
                    }
                    nsr--;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            var log = new StringBuilder();
            Sdk_Inner_Rep.InnerRepInterface innerRep = new Sdk_Inner_Rep.InnerRepSdk();
            var ret = innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");

            switch (QntDigitos)
            {
                case "3":
                    ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 00 00 00 00 00 00 00 00 00 13 14 15");
                    break;
                case "5":
                    ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 00 00 00 00 00 00 00 16 07 08 09 10");
                    break;
                case "6":
                    ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 01 02 03 04 00 00 00 00 00 00 00 00");
                    break;
                case "14":
                    ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 02 03 04 05 00 00 00 00 00 01 00 00");
                    break;
                case "16":
                    ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16");
                    break;
                default:
                    throw new Exception("Número de dígitos incompatível, valores aceitos (3,5,6,14 ou 16)");
            }

            if (Empregador.RazaoSocial.Length > 150)
                Empregador.RazaoSocial = Empregador.RazaoSocial.Substring(0, 150);
            Empregador.Documento = Empregador.Documento.Replace(".", "").Replace("/", "").Replace("-", "");

            ret = innerRep.ConfiguraEmpregador(Empregador.RazaoSocial, Empregador.Documento, Empregador.CEI, (int)Empregador.TipoDocumento);
            if (ret > 0)
            {
                MensagemErroEmpresa(log, ret);
            }

            innerRep.LimpaListaEmpregados();

            string nomeExibicao
                , senhaFuncionario;
            bool podeMarcarPeloTeclado = false;
            foreach (Entidades.Empregado item in Empregados)
            {
                if (item.DsCodigo.Length > 16)
                {
                    log.AppendLine("Funcionário " + item.Nome + ": O código (" + item.DsCodigo + ") ultrapassa o limite de 20 caracteres.");
                    continue;
                }

                item.Senha = item.Senha ?? "";
                if (item.Senha.Length > 4)
                {
                    log.AppendLine("Funcionário " + item.Nome + ": A senha do funcionário ultrapassa o limite de 4 digitos.");
                    continue;
                }

                if (item.Nome.Length > 52)
                    item.Nome = item.Nome.Substring(0, 52);

                if (item.Nome.Length > 16)
                    nomeExibicao = item.Nome.Substring(0, 16);
                else
                    nomeExibicao = item.Nome;

                if (item.Pis.Length == 12)
                    item.Pis = item.Pis.Substring(1, 11);

                //Essa flag tem q ser true quando o relógio não é biométrico ou quando o funcionário tem senha
                podeMarcarPeloTeclado = !biometrico || item.Senha.Length == 4;
                senhaFuncionario = item.Senha.Length == 4 ? item.Senha : "1234";
                ret = innerRep.IncluiEmpregadoLista(item.Nome, item.Pis, item.DsCodigo, nomeExibicao, podeMarcarPeloTeclado, senhaFuncionario, false);
                if (ret > 0)
                {
                    MensagemErroFuncionario(log, ret, item);
                }
            }

            int _ret = innerRep.EnviaConfiguracoes();

            if (_ret > 0)
            {
                switch (_ret)
                {
                    case 1:
                        throw new Exception("Problemas de comunicação com o relógio. Status: ERRO");
                    case 2:
                        throw new Exception("Problemas de comunicação com o relógio. Status: TIMEOUT");
                    case 3:
                        throw new Exception("Problemas de comunicação com o relógio. Status: REPOUSO");
                    case 4:
                        throw new Exception("Problemas de comunicação com o relógio. Status: AGUARDANDO");
                    default:
                        throw new Exception("Problemas de comunicação com o relógio.");
                }
            }

            if (log.Length > 0)
            {
                erros = log.ToString();
                return false;
            }
            erros = String.Empty;
            return true;
        }

        private static void MensagemErroFuncionario(StringBuilder log, int ret, Entidades.Empregado item)
        {
            switch (ret)
            {
                case 1:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro do nome.");
                    break;
                case 2:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro do pis.");
                    break;
                case 3:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro do código.");
                    break;
                case 4:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro do nome de exibição.");
                    break;
                case 5:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro na configuração de entrada via teclado.");
                    break;
                case 6:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro da senha.");
                    break;
                case 7:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro para verificação de biometria.");
                    break;
                case 8:
                    log.AppendLine("Funcionário " + item.Nome + ": PIS repetido na lista.");
                    break;
                case 9:
                    log.AppendLine("Funcionário " + item.Nome + ": Código repetido na lista.");
                    break;
                case 10:
                    log.AppendLine("Funcionário " + item.Nome + ": Limite de empregados atingido.");
                    break;
                case 11:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro de Conversão.");
                    break;
            }
        }

        private void MensagemErroEmpresa(StringBuilder log, int ret)
        {
            switch (ret)
            {
                case 1:
                    log.AppendLine("- Erro no cadastro da razão social.");
                    break;
                case 2:
                    log.AppendLine("- Erro no cadastro do CNPJ/CPF.");
                    break;
                case 3:
                    log.AppendLine("- Erro no cadastro do CEI.");
                    break;
                case 4:
                    log.AppendLine("- Erro no tipo de Identificador de empregador.");
                    break;
                case 5:
                    log.AppendLine("- Erro de conversão.");
                    break;
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            int ret;
            Sdk_Inner_Rep.InnerRepInterface innerRep = new Sdk_Inner_Rep.InnerRepSdk();

            ret = innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");
            if (QntDigitos == "5")
            {
                ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 00 00 00 00 00 00 00 16 07 08 09 10");
            }
            else if (QntDigitos == "16")
            {
                ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16");
            }
            else if (QntDigitos == "14")
            {
                ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 02 03 04 05 00 00 00 00 00 01 00 00");
            }
            else if (QntDigitos == "3")
            {
                ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 00 00 00 00 00 00 00 00 00 00 00 13 14 15");
            }
            else if (QntDigitos == "6")
            {
                ret = innerRep.ConfiguraInnerRep(Local, Senha, Senha, Senha, "00 00 01 02 03 00 00 00 00 00 00 00 00 00 00 00");
            }

            ret = innerRep.ConfiguraHorarioVerao(inicio.Day, inicio.Month, inicio.Year, inicio.Hour, inicio.Minute,
               termino.Day, termino.Month, termino.Year, termino.Hour, termino.Minute);

            ret = innerRep.EnviaConfiguracoesGerais();
            innerRep.FinalizaLeitura();
            if (ret > 0)
            {
                erros = "Erro ao configurar o horário do relógio. Verifique se os parâmetros de comunicação estão corretos. ";
                return false;
            }

            erros = String.Empty;
            return true;

        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            int ret;
            Sdk_Inner_Rep.InnerRepSdk innerRep = new Sdk_Inner_Rep.InnerRepSdk();
            ret = innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");
            ret = innerRep.EnviaRelogio();
            innerRep.FinalizaLeitura();
            if (ret > 0)
            {
                erros = "Erro ao configurar o horário do relógio. Verifique se os parâmetros de comunicação estão corretos.";
                return false;
            }
            erros = String.Empty;
            return true;
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExclusaoHabilitada()
        {
            return false;
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            throw new Exception("Função não disponível para este modelo de relógio.");
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new Exception("Função não disponível para este modelo de relógio.");
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportacaoHabilitada()
        {
            return false;
        }

        private int GetUltimoNsrRep(InnerRepSdk innerRep)
        {
            innerRep.SolicitaRegistroDoNsr(1);
            int statusColeta = innerRep.RecebeStatusLeitura();
            while (statusColeta < 2)
            {
                Thread.Sleep(500);
                statusColeta = innerRep.RecebeStatusLeitura();
            }
            if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_REGISTRO))
            {
                string menorData = innerRep.LeMenorDataEntreOsRegistroLidos();
                DateTime dtIni;
                DateTime.TryParse(menorData, out dtIni);
                string maiorData = innerRep.LeMaiorDataEntreOsRegistroLidos();
                int res = Convert.ToInt32(innerRep.LeNumeroDoUltimoNsrDoInnerRep());
                return res;
            }
            else
            {
                return 0;
            }
        }

        private string GetStringSomenteAlfanumerico(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            string r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                              char.IsWhiteSpace(c) ||
                                                              c == '+' ||
                                                              c == ',' ||
                                                              c == ']'))
                                                              .ToArray());
            return r;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, System.IO.DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }

        public override bool TesteConexao()
        {
            try
            {
                /* Cria uma instância da classe InnerRepSdk */
                Sdk_Inner_Rep.InnerRepSdk innerRep = new Sdk_Inner_Rep.InnerRepSdk();
                /* Define os parâmetros de comunicação com o REP */
                int ret = innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");
                ret = innerRep.SolicitaRelogio();
                if (ret == 1)
                {
                    return false;
                }
                string re = innerRep.LeRelogio();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override int UltimoNSR()
        {
            throw new NotImplementedException();
        }

        public override List<Biometria> GetBiometria(out string erros)
        {
            throw new NotImplementedException();
        }
    }
}
