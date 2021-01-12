using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Sdk_Inner_Rep;
using System.IO;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao.Relogios.TopData
{
    public class InnerRepPlus : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            try
            {
                List<RegistroAFD> registros = new List<RegistroAFD>();
                List<RegistroAFD> retorno = new List<RegistroAFD>();
                Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
                string erros = String.Empty;
                if (IniciaComunicacao(out erros, out innerRep))
                {

                    if ((nsrInicio == 0 && nsrFim == 0))
                    {
                        int nsr = UltimoNSR();
                        registros = GetRegistrosOrdemDecrescente(dataI, dataF,ref innerRep, nsr);
                    }
                    else
                    {
                        registros = GetRegistrosOrdemDecrescentePorNsr(ref innerRep, nsrInicio, nsrFim);
                    }
                    registros = registros.OrderBy(o => o.Nsr).ToList();

                    try
                    {
                        Util.IncluiRegistro(innerRep.LerCabecalhoRegistrosColetados(), dataI, dataF, retorno);
                    }
                    catch (Exception)
                    {
                        GeraCabecalhoManual(dataI, dataF, retorno);
                    }
                    retorno.AddRange(registros);
                    Util.IncluiRegistro(innerRep.LerTrailerRegistrosColetados(), dataI, dataF, retorno);
                    innerRep.FinalizarLeitura();
                    return retorno;
                }
                else
                {
                    throw new Exception(erros);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
        }

        private List<RegistroAFD> GetRegistrosOrdemDecrescente(DateTime dataI, DateTime dataF,ref Sdk_Inner_Rep.InnerRepPlusSDK innerRep, int nsrInicial)
        {
            int nsr = nsrInicial;
            int resultadoColeta = 0;
            bool recebeuUltimoBilhete = false;

            List<RegistroAFD> result = new List<RegistroAFD>();

            try
            {
                while (!recebeuUltimoBilhete)
                {
                    resultadoColeta = ColetaNSR(nsr, innerRep);

                    /* Como essa coleta Funciona em ordem decrescente e por data fico lendo enquando existir registro e o mesno não chegou na data Inicial */
                    if (resultadoColeta == (int)ResultadoColeta.ULTIMO_REGISTRO_LIDO ||
                        (resultadoColeta == (int)ResultadoColeta.REGISTRO_LIDO))
                    {
                        string dadosRegistro = innerRep.LerRegistroLinha();
                        if (dadosRegistro != "" && dadosRegistro != null)
                        {
                            DateTime? dtRegistro = Util.IncluiRegistro(dadosRegistro, dataI, dataF, result);
                            if (dtRegistro.HasValue)
                            {
                                if (dtRegistro.Value < dataI)
                                {
                                    recebeuUltimoBilhete = true;
                                }
                            }
                        }
                    }
                    else
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

        private List<RegistroAFD> GetRegistrosOrdemDecrescentePorNsr(ref InnerRepPlusSDK innerRep, int nsrInicial, int nsrFinal)
        {
            int resultadoColeta = 0;
            bool recebeuUltimoBilhete = false;

            List<RegistroAFD> result = new List<RegistroAFD>();

            if (nsrInicial > nsrFinal)
            {
                return result;
            }

            try
            {
                while (!recebeuUltimoBilhete)
                {
                    resultadoColeta = ColetaNSR(nsrInicial, innerRep);

                    if (resultadoColeta == (int)ResultadoColeta.ULTIMO_REGISTRO_LIDO ||
                        (resultadoColeta == (int)ResultadoColeta.REGISTRO_LIDO))
                    {
                        string dadosRegistro = innerRep.LerRegistroLinha();
                        if (dadosRegistro != "" && dadosRegistro != null)
                        {
                            Util.IncluiRegistro(dadosRegistro, null, null, result);
                        }
                    }

                    if (resultadoColeta != (int)ResultadoColeta.REGISTRO_LIDO)
                    {
                        recebeuUltimoBilhete = true;
                    }
                    nsrInicial++;
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
            Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
            if (IniciaComunicacao(out erros, out innerRep))
            {
                if (Empregador != null)
                {
                    if (!EnviarEmpregador(ref erros, innerRep))
                    {
                        return false;
                    } 
                }
                
                if (Empregados.Count() > 0)
                {
                    if (!EnviarEmpregados(biometrico, ref erros, innerRep))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool EnviarEmpregados(bool biometrico, ref string erros, Sdk_Inner_Rep.InnerRepPlusSDK innerRep)
        {
            StringBuilder log = new StringBuilder();
            int ret = DefinirEmpregados(biometrico, innerRep, ref log);
            ret = innerRep.EnviarListaIndividualEmpregados();
            bool retorno = TrataRetornoEnvioEmpresaFuncionario(ref erros, ret);

            if (log.Length > 0)
            {
                if (erros.Length > 0)
                {
                    erros += ". ";
                }
                erros += log.ToString();
                retorno = false;
            }
            return retorno;
        }

        private int DefinirEmpregados(bool biometrico, Sdk_Inner_Rep.InnerRepPlusSDK innerRep, ref StringBuilder log)
        {
            string nomeExibicao
            , senhaFuncionario;
            bool podeMarcarPeloTeclado = false;

            innerRep.InicializarListaEmpregados();
            int ret = 0;
            foreach (Entidades.Empregado item in Empregados)
            {
                if (item.DsCodigo.Length > 16)
                {
                    log.AppendLine("Funcionário " + item.Nome + ": O código (" + item.DsCodigo + ") ultrapassa o limite de 16 caracteres.");
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

                if (item.Pis.Length > 11)
                    item.Pis = item.Pis.Substring(1, 11);

                //Essa flag tem q ser true quando o relógio não é biométrico ou quando o funcionário tem senha
                podeMarcarPeloTeclado = !biometrico || item.Senha.Length == 4;
                senhaFuncionario = item.Senha.Length == 4 ? item.Senha : "1234";
                ret = innerRep.IncluirEmpregadosLista(item.Nome, item.Pis, item.DsCodigo, item.DsCodigo, item.DsCodigo, nomeExibicao, senhaFuncionario, false);
                if (ret > 0)
                {
                    MensagemErroFuncionario(log, ret, item);
                }
            }
            return ret;
        }

        private bool EnviarEmpregador(ref string erros, Sdk_Inner_Rep.InnerRepPlusSDK innerRep)
        {


            bool retorno = DefinirEmpregador(ref erros, innerRep);

            if (!retorno)
            {
                return retorno;
            }

            int ret = innerRep.EnviarEmpregador();
            if (!TrataRetornoEnviaConfiguracoes(ret, ref erros))
            {
                return false;
            }
            return true;
        }

        private bool DefinirEmpregador(ref string erros, Sdk_Inner_Rep.InnerRepPlusSDK innerRep)
        {
            bool retorno = true;

            if (Empregador.RazaoSocial.Length > 150)
                Empregador.RazaoSocial = Empregador.RazaoSocial.Substring(0, 150);
            Empregador.Documento = Empregador.Documento.Replace(".", "").Replace("/", "").Replace("-", "");
            int ret = innerRep.DefinirLocal(Local.Replace("(", "").Replace(")", ""));
            TrataRetornoEnvioLocal(ret);
            ret = innerRep.DefinirEmpregador(Empregador.RazaoSocial, Empregador.Documento, Empregador.CEI, (int)Empregador.TipoDocumento);
            if (ret > 0)
            {
                MensagemErroEmpresa(ref erros, ret);
                retorno = false;
            }
            return retorno;
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            erros = "Erro ao configurar o horário de verão do relógio. ";
            Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
            if (IniciaComunicacao(out erros, out innerRep))
            {
                int ret = innerRep.DefinirHorarioVerao(inicio.Day, inicio.Month, inicio.Year, inicio.Hour, inicio.Minute,
                                                    termino.Day, termino.Month, termino.Year, termino.Hour, termino.Minute);
                if (ret > 0)
                {
                    erros += "Verifique se os parâmetros de comunicação estão corretos.";
                    return false;
                }

                ret = innerRep.EnviarHorarioVerao();
                return TrataRetornoEnviaConfiguracoes(ret, ref erros);
            }
            return false;
        }

        private bool IniciaComunicacao(out string erros, out Sdk_Inner_Rep.InnerRepPlusSDK innerRep)
        {
            if (String.IsNullOrEmpty(Cpf))
            {
                throw new Exception("CPF não informado, Verifique o cadastro do usuário");
            }
            innerRep = new Sdk_Inner_Rep.InnerRepPlusSDK();

            int ret = innerRep.DefinirParametrosComunicacao(Cpf, IP, "**AUTENTICACAO**", 1);
            return TrataRetornoComunicacao(ret, out erros);
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            erros = "Erro ao configurar o horário do relógio. ";
            Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
            if (IniciaComunicacao(out erros, out innerRep))
            {
                int ret = innerRep.EnviarRelogio();
                return TrataRetornoEnviaConfiguracoes(ret, ref erros);
            }
            return false;
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExclusaoHabilitada()
        {
            return true;
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            erros = "";
            List<string> lErros = new List<string>();
            bool retorno = true;
            try
            {
                if (Empregados != null && Empregados.Count > 0)
                {
                    Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
                    if (IniciaComunicacao(out erros, out innerRep))
                    {
                        innerRep.InicializarListaEmpregados();
                        int ret = 0;
                        foreach (var item in Empregados)
                        {
                            ret = innerRep.IncluirPisEmpregadoListaExclusao(item.Pis);
                            switch (ret)
                            {
                                case 1: lErros.Add("Erro ao incluir o Funcionário: "+item.DsCodigo+" - "+item.Nome+" na lista de exclusão.");
                                    retorno = false;
                                    break;
                                case 4: lErros.Add("Aguardando resposta do REP para o Funcionário: " + item.DsCodigo + " - " + item.Nome + " para inclusão na lista de exclusão.");
                                    retorno = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ret = innerRep.EnviarListaExclusaoEmpregados();
                        int quantidadeExclusoes = innerRep.LerQuantidade();
                        int contador = 0;
                        int resultadoLeitura;

                        while (contador < quantidadeExclusoes)
                        {
                            resultadoLeitura = innerRep.LerResultado(contador);

                            /* Testa o retorno do método LeDadosEmpregados */
                            switch (resultadoLeitura)
                            {
                                case (int)Sdk_Inner_Rep.InnerRepPlusSDK.ResultadoLeitura.SUCESSO:
                                    string resposta = "";
                                    switch (innerRep.LerStatus())
                                    {
                                        case (int)Sdk_Inner_Rep.InnerRepPlusSDK.ResultadoExclusaoEmpregados.EXCLUIDO_SUCESSO:
                                            resposta = "Usuário excluído";
                                            break;
                                        case (int)Sdk_Inner_Rep.InnerRepPlusSDK.ResultadoExclusaoEmpregados.USUARIO_NAO_EXISTE:
                                            resposta = "Usuário não existe";
                                            break;
                                        case (int)Sdk_Inner_Rep.InnerRepPlusSDK.ResultadoExclusaoEmpregados.NAO_REALIZOU_OPERACAO:
                                            resposta = "Não realizou a operação";
                                            break;
                                        case (int)Sdk_Inner_Rep.InnerRepPlusSDK.ResultadoExclusaoEmpregados.CPF_NAO_CADASTRADO:
                                            resposta = "Cpf não cadastrado";
                                            break;
                                    }

                                    string[] row = new string[] {innerRep.LerPis(),
                                                     resposta};

                                    //adiciona a linha no Grid pela Thread
                                    break;
                            }

                            contador++;

                        }
                        bool retornoEnvio = TrataRetornoEnviaConfiguracoes(ret, ref erros);
                        if (!retorno || !retornoEnvio)
                        {
                            if (lErros.Count > 0)
	                        {
		                         if (erros.Length > 0)
                                     erros += Environment.NewLine + String.Join(Environment.NewLine, lErros);
                                 else
                                     erros = String.Join(Environment.NewLine, lErros);
	                        }
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new Exception("Função não disponível para este modelo de relógio.");
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            erros = String.Empty;
            StringBuilder log = new StringBuilder();
            Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
            bool retorno = false;

             if (retorno = IniciaComunicacao(out erros, out innerRep))
	        {
		        try
                {
                    DirectoryInfo d;
                    if (!Directory.Exists(caminho))
                    {
                        d = Directory.CreateDirectory(caminho);
                    }
                    else
                    {
                        d = new DirectoryInfo(caminho);
                    }

                    if (Empregador != null)
                    {
                        retorno = DefinirEmpregador(ref erros, innerRep);
                        int resultadoGeracao = innerRep.GerarArquivoRB1(NumeroSerie, caminho, 1);

                        if (resultadoGeracao == 1)
                        {
                            erros += "Erro na geração do arquivo";
                        }
                    }

                    if (Empregados != null && Empregados.Count > 0)
                    {
                        int ret = DefinirEmpregados(Biometrico, innerRep, ref log);

                        if (log.Length > 0)
                        {
                            if (erros.Length > 0)
                            {
                                erros += ". ";
                            }
                            erros += log.ToString();
                        }

                        int resultadoGeracao = innerRep.GerarArquivoRB1(NumeroSerie, caminho, 2);

                        if (resultadoGeracao == 1)
                        {
                            erros += "Erro na geração do arquivo";
                        }
                    }

                    if (erros.Length > 0)
                    {
                        retorno = false;
                    }

                    return retorno;
                }
                catch (Exception e)
                {
                    erros = e.Message;
                }
                 return false;
	        }
            else
	        {
                 throw new Exception(erros);
	        }
            
        }

        private int ExportaEmpregados(out string erros, ref Sdk_Inner_Rep.InnerRepPlusSDK innerRep, string caminho)
        {
            StringBuilder log = new StringBuilder();
            erros = String.Empty;

            int retorno = DefinirEmpregados(Biometrico, innerRep, ref log);

            if (log.Length > 0)
            {
                if (erros.Length > 0)
                {
                    erros += ". ";
                }
                erros += log.ToString();
            }

            int resultadoGeracao = innerRep.GerarArquivoRB1(NumeroSerie, caminho, 2);

            return resultadoGeracao;
        }

        private int ExportaEmpregador(out string erros, ref Sdk_Inner_Rep.InnerRepPlusSDK innerRep, string caminho)
        {
            erros = String.Empty;
            bool retorno = false;
            
            retorno = DefinirEmpregador(ref erros, innerRep);
            int resultadoGeracao = innerRep.GerarArquivoRB1(NumeroSerie, caminho, 1);

            return resultadoGeracao;
        }

        public override bool ExportacaoHabilitada()
        {
            return true;
        }

        private int ColetaNSR(int NSR, Sdk_Inner_Rep.InnerRepPlusSDK innerRep)
        {
            /* Solicita um NSR específico para o REP */
            int ret = innerRep.SolicitarRegistroNsr(NSR);
            if (ret == 1)
            {
                throw new Exception("Parâmetros inválidos.");
            }

            /* Verifica o status da coleta enquanto o status leitura estiver em Andamento */
            int statusColeta = innerRep.LerStatusColeta();
            int tentativas = 0;
            while (statusColeta < 2 && tentativas < 1000)
            {
                Thread.Sleep(10);
                statusColeta = innerRep.LerStatusColeta();
                tentativas++;
            }

            int resultadoColeta = 3;

            /* Status da coleta recebido, verifica se recebeu o registro com sucesso */
            if ((statusColeta == (int)Sdk_Inner_Rep.InnerRepPlusSDK.StatusLeitura.FINALIZADA_COM_ULTIMO_REGISTRO) ||
                (statusColeta == (int)Sdk_Inner_Rep.InnerRepPlusSDK.StatusLeitura.FINALIZADA_COM_REGISTRO))
            {
                /* Recupera o número de Série do REP */
                string numeroSerie = innerRep.LerNumSerieRep();
                /* Recebe o resultado da coleta */
                resultadoColeta = innerRep.LerResultadoColeta();
                if (resultadoColeta == 4)
                {
                    // Se deu erro ao ler o ResultadoColeta espero 1 segundo e tento novamente
                    Thread.Sleep(1000);
                    resultadoColeta = innerRep.LerResultadoColeta();
                    if (resultadoColeta == 4)
                        throw new Exception("O comando interno do SDK para receber o resultado da leitura retornou erro.");
                }
            }
            
            return resultadoColeta;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, System.IO.DirectoryInfo pasta)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            erros = String.Empty;
            StringBuilder log = new StringBuilder();
            Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
            bool retorno = false;

            try
            {
                if (retorno = IniciaComunicacao(out erros, out innerRep))
                {
                    if (Empregador != null)
                    {
                        string nomeGuid = Guid.NewGuid() + ".rb1";
                        int expEmpregador = ExportaEmpregador(out erros, ref innerRep, caminho);
                        ret.Add("BASE_IMPORTA_EMPRESA_REP_" + NumeroSerie + ".rb1", "Empregador");

                        if (expEmpregador == 1)
                        {
                            erros += "Erro na geração do arquivo";
                        }
                            
                    }

                     if (Empregados != null)
                     {
                         string nomeGuid = Guid.NewGuid() + ".rb1";
                         int expEmpregados = ExportaEmpregados(out erros, ref innerRep, caminho);
                         ret.Add("BASE_IMPORTA_EMPREGADOS_REP_" + NumeroSerie + ".rb1", "Empregados");

                         if (expEmpregados == 1)
                         {
                             erros += "Erro na geração do arquivo";
                         }
                     }
                    
                }
            }
            catch (Exception e)
            {
                
                throw e;
            }

            return ret; 
        }

        public override bool TesteConexao()
        {
            InnerRepPlusSDK innerRep;
            string erros;
            return IniciaComunicacao(out erros,out innerRep);
            
        }

        #region Tratamentos de Erros
        private static bool TrataRetornoComunicacao(int ret, out string erros)
        {
            if (ret == (int)Sdk_Inner_Rep.InnerRepPlusSDK.ParametrosComunicacao.ERRO_CPF_RESPONSAVEL)
            {
                erros = "Informe o CPF!";
                return false;
            }
            switch (ret)
            {
                case 0: erros = String.Empty;
                    return true;
                case 1: erros = "Erro na configuração do IP.";
                    return false;
                case 2: erros = "Erro na configuração da Chave de Comunicação.";
                    return false;
                default: erros = "Erro ao efetuar a comunicação.";
                    return false;
            }
        }

        private static bool TrataRetornoEnvioEmpresaFuncionario(ref string erros, int ret)
        {
            switch (ret)
            {
                case 0: erros = String.Empty;
                    return true;
                case 1: erros = "Ocorreu um erro no envio dos dados de funcionários.";
                    return false;
                case 4: erros = "Aguardando resposta do REP.";
                    return false;
                default: return true;
            }
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
                    log.AppendLine("Funcionário " + item.Nome + ": Erro no cadastro do Cartão.");
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
                case 12:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro de caracteres inválido no nome.");
                    break;
                case 13:
                    log.AppendLine("Funcionário " + item.Nome + ": Erro de caracteres inválido nome exibição.");
                    break;
            }
        }

        private void MensagemErroEmpresa(ref string erros, int ret)
        {
            switch (ret)
            {
                case 1:
                    erros = "Erro no cadastro da razão social.";
                    break;
                case 2:
                    erros = "Erro no cadastro do CNPJ/CPF.";
                    break;
                case 3:
                    erros = "Erro no cadastro do CEI.";
                    break;
                case 4:
                    erros = "Erro no tipo de Identificador de empregador.";
                    break;
                case 5:
                    erros = "- Erro de conversão.";
                    break;
            }
        }

        private static bool TrataRetornoEnviaConfiguracoes(int ret, ref string erroRetornoConfiguracao)
        {
            erroRetornoConfiguracao += " Retorno Rep: ";
            switch (ret)
            {
                //Atualiza o label lblResultadoConfiguracoes pela Thread
                case Sdk_Inner_Rep.InnerRepSdk.ResultadoComunicacao.REPOUSO:
                    erroRetornoConfiguracao += "Repouso";
                    return false;
                case Sdk_Inner_Rep.InnerRepSdk.ResultadoComunicacao.AGUARDANDO:
                    erroRetornoConfiguracao += "Aguardando";
                    return false;
                case Sdk_Inner_Rep.InnerRepSdk.ResultadoComunicacao.SUCESSO:
                    erroRetornoConfiguracao = String.Empty;
                    return true;
                case Sdk_Inner_Rep.InnerRepSdk.ResultadoComunicacao.TIMEOUT:
                    erroRetornoConfiguracao += "Timeout";
                    return false;
                case Sdk_Inner_Rep.InnerRepSdk.ResultadoComunicacao.ERRO:
                    erroRetornoConfiguracao += "Erro";
                    return false;
                default:
                    erroRetornoConfiguracao = "Desconhecido";
                    return false;
            }
        }
        private static void TrataRetornoEnvioLocal(int ret)
        {
            switch (ret)
            {
                case 0:
                    break;
                case 7:
                    throw new Exception("Caracteres inválidos no local.");
                default:
                    throw new Exception("Erro ao enviar o local do Rep.");
            }
        }
        #endregion

        public enum ResultadoColeta
        {
            REGISTRO_LIDO = 1,
            ULTIMO_REGISTRO_LIDO = 2,
            REGISTRO_INEXISTENTE = 3,
            ERRO = 4
        }

        public override int UltimoNSR()
        {
            try
            {
                int NSRCorrente = 1;
                Sdk_Inner_Rep.InnerRepPlusSDK innerRep;
                string erros = String.Empty;
                if (IniciaComunicacao(out erros, out innerRep))
                {
                    int intervalo = 10000;
                    int ultimoNSRColetado = 0;
                    bool encontrouUltimo = false;
                    while (!encontrouUltimo)
                    {
                        int resultadoColeta = ColetaNSR(NSRCorrente, innerRep);
                        if (NSRCorrente == 1 && resultadoColeta == (int)Sdk_Inner_Rep.InnerRepPlusSDK.StatusLeitura.FINALIZADA_SEM_REGISTRO)
                        {
                            encontrouUltimo = true;
                        }
                        else
                        {
                            if (NSRCorrente == 1)
                            {
                                NSRCorrente = 0;
                            }
                            switch (resultadoColeta)
                            {
                                case (int)ResultadoColeta.ULTIMO_REGISTRO_LIDO:
                                    encontrouUltimo = true;
                                    break;
                                case (int)ResultadoColeta.REGISTRO_LIDO:
                                    ultimoNSRColetado = NSRCorrente;
                                    NSRCorrente += intervalo;
                                    break;
                                case (int)ResultadoColeta.REGISTRO_INEXISTENTE:
                                    NSRCorrente = ultimoNSRColetado;
                                    if (intervalo > 10)
                                    {
                                        intervalo /= 10;
                                    }
                                    else
                                    { intervalo = 1; }
                                    NSRCorrente += intervalo;
                                    break;
                                case (int)ResultadoColeta.ERRO:
                                    throw new Exception("O comando interno do SDK para receber o resultado da leitura retornou erro.");
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(erros);
                }
                return NSRCorrente;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override List<Biometria> GetBiometria(out string erros)
        {
            throw new NotImplementedException();
        }
    }
}
