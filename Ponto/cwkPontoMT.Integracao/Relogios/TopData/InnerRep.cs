using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Sdk_Inner_Rep;
using System.IO;
using cwkPontoMT.Integracao.Entidades;
using java.text;

namespace cwkPontoMT.Integracao.Relogios.TopData
{
    public class InnerRep : Relogio
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            Sdk_Inner_Rep.InnerRepSdk innerRep = new Sdk_Inner_Rep.InnerRepSdk();
            try
            {
                //Diminuido 1 numero pois o novo comunicador já pede o próximo, e quando não existe na topdata da problema
                nsrInicio--;
                List<RegistroAFD> registros = new List<RegistroAFD>();
                List<RegistroAFD> retorno = new List<RegistroAFD>();
                /* Cria uma instância da classe InnerRepSdk */
                Logar("Iniciando SDK");
                
                /* Define os parâmetros de comunicação com o REP */
                Logar("Definindo parametros de comunicação");
                innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");

                if ((nsrInicio == 0 && nsrFim == 0))
                {
                    Logar("Iniciando importação por período, parâmetros recebidos: Data Inicial = " + dataI.ToString("dd/MM/yyyy HH:mm") + " Data Final = " + dataF.ToString("dd/MM/yyyy HH:mm") + " NSR Inicial = " + nsrInicio.ToString() + " NSR Final = " + nsrFim.ToString());
                    int nsr = GetUltimoNsrRep(innerRep, nsrInicio);
                    registros = GetRegistrosOrdemDecrescente(dataI, dataF, innerRep, nsr);
                }
                else
                {
                    Logar("Iniciando importação por NSR, parâmetros recebidos: Data Inicial = " + dataI.ToString("dd/MM/yyyy HH:mm") + " Data Final = " + dataF.ToString("dd/MM/yyyy HH:mm") + " NSR Inicial = " + nsrInicio.ToString() + " NSR Final = " + nsrFim.ToString());
                    if (nsrFim == int.MaxValue || nsrFim == 0)
                    {
                        nsrFim = GetUltimoNsrRep(innerRep, nsrInicio);
                    }
                    registros = GetRegistrosPorNsr(innerRep, nsrInicio, nsrFim);
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

        private void Logar(string mensagem)
        {
            log.Info($"{NumeroSerie} - {mensagem}");
        }

        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
        }

        private List<RegistroAFD> GetRegistrosOrdemDecrescente(DateTime dataI, DateTime dataF, Sdk_Inner_Rep.InnerRepSdk innerRep, int nsrInicial)
        {
            int ret;
            int nsr = nsrInicial;
            int statusColeta = 0;
            int resultadoColeta = 0;
            string dadosRegistro;
            int numeroRegistrosLidos = 0;
            bool recebeuUltimoBilhete = false;

            List<RegistroAFD> result = new List<RegistroAFD>();
            Logar("Iniciando Importação dos registros em ordem decrescente");
            try
            {
                while (!recebeuUltimoBilhete)
                {
                    Logar("Solicitando NSR " + nsr.ToString());
                    ret = innerRep.SolicitaRegistroDoNsr(nsr);
                    if (ret == 0)
                    {
                        /* Solicita o Status da Leitura.. */
                        statusColeta = innerRep.RecebeStatusLeitura();

                        int tentativas = 0;
                        /* Enquanto Status da leitura estiver em Andamento (1) verifica o status novamente.. */
                        while (statusColeta < 2 && tentativas <= 1000)
                        {
                            Logar("Erro na Solicitação, Status da coleta: "+statusColeta.ToString()+", tentativa: " + tentativas.ToString());
                            Thread.Sleep(20);
                            statusColeta = innerRep.RecebeStatusLeitura();
                            tentativas++;
                        }

                        Logar("Resultado da leitura recebida = " + statusColeta.ToString());
                        /* Status da coleta recebido, verifica se recebeu o registro com sucesso */
                        if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                            (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_REGISTRO))
                        {
                            /* Recebe o resultado da leitura */
                            resultadoColeta = innerRep.RecebeResultadoLeitura();

                            /* Faz a leitura do registro */
                            dadosRegistro = innerRep.LeLinhaDoRegistro();
                            Logar("Dados recebidos = " + dadosRegistro);

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

                            if (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO)
                                recebeuUltimoBilhete = true;
                            else
                                nsr--;
                        }

                        /* Se o status da coleta foi finalizado com ultimo registro, ou se a coleta não possui registros, sai do laço.. */
                        if (nsr < 1 || recebeuUltimoBilhete || 
                           (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_SEM_REGISTRO) ||
                           (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO))
                        {
                            Logar("Terminou Coleta, ultimo NSR Recebido = " + nsr.ToString() + "Status da Coleta = " + statusColeta.ToString());
                            recebeuUltimoBilhete = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private List<RegistroAFD> GetRegistrosPorNsr(InnerRepSdk innerRep, int nsrInicial, int nsrFinal)
        {
            int ret;
            int nsr = nsrInicial;
            int statusColeta = 0;
            int resultadoColeta = 0;
            string dadosRegistro;
            int numeroRegistrosLidos = 0;
            bool recebeuUltimoBilhete = false;
            Logar("Iniciando Importação dos registros por nsr, NSR Inicial: "+nsrInicial.ToString()+"NSR Final: "+nsrFinal.ToString());
            List<RegistroAFD> result = new List<RegistroAFD>();
            if (nsrInicial == nsrFinal)
            {
                return result;
            }

            try
            {
                while (!recebeuUltimoBilhete)
                {
                    Logar("Solicitando NSR " + nsr.ToString());
                    ret = innerRep.SolicitaRegistroDoNsr(nsr);
                    if (ret == 0)
                    {
                        /* Solicita o Status da Leitura.. */
                        statusColeta = innerRep.RecebeStatusLeitura();

                        int tentativas = 0;
                        /* Enquanto Status da leitura estiver em Andamento (1) verifica o status novamente.. */
                        while (statusColeta < 2 && tentativas <= 1000)
                        {
                            Logar("Erro na Solicitação, Status da coleta: " + statusColeta.ToString() + ", tentativa: " + tentativas.ToString());
                            Thread.Sleep(50);
                            statusColeta = innerRep.RecebeStatusLeitura();
                            tentativas++;
                        }

                        Logar("Resultado da leitura recebida = " + statusColeta.ToString());
                        /* Status da coleta recebido, verifica se recebeu o registro com sucesso */
                        if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                            (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_REGISTRO))
                        {
                            /* Recebe o resultado da leitura */
                            resultadoColeta = innerRep.RecebeResultadoLeitura();
                            
                            /* Faz a leitura do registro */
                            dadosRegistro = innerRep.LeLinhaDoRegistro();
                            Logar("Dados recebidos = " + dadosRegistro);

                            Util.IncluiRegistro(dadosRegistro, null, null, result);
                            if (result.Where(w => w.Nsr > 0 && w.Nsr < 999999999).Min(m => m.Nsr) < nsrInicial)
                            {
                                recebeuUltimoBilhete = true;
                            }
                            else
                            {
                                numeroRegistrosLidos++;
                            }

                            nsr++;
                        }

                        /* Se o status da coleta foi finalizado com ultimo registro, ou se a coleta não possui registros, sai do laço.. */
                        if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_SEM_REGISTRO) ||
                            (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                            recebeuUltimoBilhete)
                        {
                            Logar("Terminou Coleta, ultimo NSR Recebido = " + nsr.ToString() + "Status da Coleta = " + statusColeta.ToString());
                            recebeuUltimoBilhete = true;
                        }
                    }
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
            var logRet = new StringBuilder();
            Sdk_Inner_Rep.InnerRepInterface innerRep = new Sdk_Inner_Rep.InnerRepSdk();
            var ret = innerRep.DefineParametrosComunicacao(IP, "**AUTENTICACAO**");

            Logar($"innerRep.ConfiguraInnerRep: Local = {Local}; senhaComunic = {Senha}; senhaRelogio = {Senha}; senhaBio = {Senha}; Qtd de dígitos = {QntDigitos}");
            if (string.IsNullOrEmpty(Senha))
            {
                Logar($"Relógio não tem senha cadastrada, para enviar funcionários é necessário que essa senha esteja cadastrada");
            }
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
                    Logar("Número de dígitos incompatível, valores aceitos (3,5,6,14 ou 16)");
                    throw new Exception("Número de dígitos incompatível, valores aceitos (3,5,6,14 ou 16)");
            }

            if (Empregador == null)
            {
                throw new Exception("Para enviar empregados para o modelo de equipamento Inner Rep é necessário enviar os dados da empresa também, remova o envio pendente e refaça a operação enviando os dados da empresa juntos com os funcionários.");
            }

            Logar("Enviando Empresa");
            if (Empregador.RazaoSocial.Length > 150)
                Empregador.RazaoSocial = Empregador.RazaoSocial.Substring(0, 150);
            Empregador.Documento = Empregador.Documento.Replace(".", "").Replace("/", "").Replace("-", "");

            ret = innerRep.ConfiguraEmpregador(Empregador.RazaoSocial, Empregador.Documento, Empregador.CEI, (int)Empregador.TipoDocumento);
            if (ret > 0)
            {
                Logar("Erro enviar empregador, codigo = " + ret);
                MensagemErroEmpresa(logRet, ret);
            } 

            innerRep.LimpaListaEmpregados();

            string nomeExibicao
                , senhaFuncionario;
            bool podeMarcarPeloTeclado = false;
            Logar("Iniciando envio dos empregados");
            foreach (Entidades.Empregado item in Empregados)
            {
                if (item.DsCodigo.Length > 16)
                {
                    Logar("Funcionário " + item.Nome + ": O código (" + item.DsCodigo + ") ultrapassa o limite de 20 caracteres.");
                    logRet.AppendLine("Funcionário " + item.Nome + ": O código (" + item.DsCodigo + ") ultrapassa o limite de 20 caracteres.");
                    continue;
                }

                item.Senha = item.Senha ?? "";
                if (item.Senha.Length > 4)
                {
                    Logar("Funcionário " + item.Nome + ": A senha do funcionário ultrapassa o limite de 4 digitos.");
                    logRet.AppendLine("Funcionário " + item.Nome + ": A senha do funcionário ultrapassa o limite de 4 digitos.");
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
                Logar($" Enviando funcionário nome = {item.Nome}; pis = {item.Pis}; cartao = {item.DsCodigo}; nome exibicao = {nomeExibicao}; pode marcar teclado = {podeMarcarPeloTeclado}; senha = {senhaFuncionario}; verifica bio = false");
                ret = innerRep.IncluiEmpregadoLista(item.Nome, item.Pis, item.DsCodigo, nomeExibicao, podeMarcarPeloTeclado, senhaFuncionario, false);
                Logar("Codigo retorno = " + ret);
                if (ret > 0)
                {
                    MensagemErroFuncionario(logRet, ret, item);
                }
            }

            int _ret = innerRep.EnviaConfiguracoes();
            Logar("Retorno config = "+_ret);

            if (_ret > 0)
            {
                Logar($"Se codigo erro {_ret } maior que zero exibe erro");
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

            if (logRet.Length > 0)
            {
                erros = logRet.ToString();
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

        private int GetUltimoNsrRep(InnerRepSdk innerRep, int nsrInicial)
        {
            Logar("Buscando Último NSR Rep.");
            int iniciarLeituraParaUltimoNsrDo = nsrInicial <= 3 ? 1 : (nsrInicial - 2);
            int statusColeta;
            try
            {
                Logar("Solicitando NSR a partir " + iniciarLeituraParaUltimoNsrDo);
                innerRep.SolicitaRegistroDoNsr(iniciarLeituraParaUltimoNsrDo);
                statusColeta = innerRep.RecebeStatusLeitura();
            }
            catch (Exception)
            {
                Logar("Erro ao tentar a partir de " + nsrInicial + ", solicitando a partir NSR 1.");
                innerRep.SolicitaRegistroDoNsr(1);
                statusColeta = innerRep.RecebeStatusLeitura();
            }


            int tentativas = 0;
            /* Enquanto Status da leitura estiver em Andamento (1) verifica o status novamente.. */
            while (statusColeta < 2 && tentativas <= 1000)
            {
                Logar("Esperando retorno do rep para último NSR, Status da coleta: " + statusColeta.ToString() + ", tentativa: " + tentativas.ToString());
                Thread.Sleep(500);
                statusColeta = innerRep.RecebeStatusLeitura();
                tentativas++;
            }
            if (tentativas>= 1000)
            {
                Logar("Numero de tentativas esgotadas, continuando sem pegar o ultimo NSR");
            }

            if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                (statusColeta == (int)Sdk_Inner_Rep.InnerRepSdk.StatusLeitura.FINALIZADA_COM_REGISTRO))
            {
                string menorData = innerRep.LeMenorDataEntreOsRegistroLidos();
                DateTime dtIni;
                DateTime.TryParse(menorData, out dtIni);
                string maiorData = innerRep.LeMaiorDataEntreOsRegistroLidos();
                int res = Convert.ToInt32(innerRep.LeNumeroDoUltimoNsrDoInnerRep());
                Logar("Ultimo NSR Encontrado = "+res.ToString());
                return res;
            }
            else
            {
                return 0;
            }
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
