using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Auxiliares
{
    public static class REPZPM
    {
        #region Externs/variáveis DLL ZPM

        #region Funções de Inicialização
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_IniciaDriver(System.String NumFabricacao);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_EncerraDriver(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_Localiza(System.String EnderecoIP, int Porta, int Timeout); /*R300*/
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoLocaliza(int Handle_Localizacao, int IndiceLinha, System.String Modelo, System.String NumFabricacao, System.String VersaoREP, System.String EnderecoIP, System.String Porta, System.String MACAddress, System.String Criptografia);  /*R300*/
        [DllImport("dllrep.dll")]
        public static extern void DLLREP_Versao(StringBuilder Versao);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_DefineModoIP(int Handle, System.String EnderecoIP, int Porta);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_DefineModoIPCriptografado(int Handle, System.String EnderecoIP, int Porta); /*R300*/
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_DefineModoArquivo(int Handle, System.String Unidade);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_LeModo(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_DefineTimeout(int Handle, Int32 Timeout);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_LeTimeout(int Handle);
        [DllImport("dllrep.dll")]
        public static extern void DLLREP_ConfiguraCodigoBarra(int Tamanho);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_LeCodigoBarra(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_IniciaLog(int Handle, System.String nomeArquivo);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_EncerraLog(int Handle);
        #endregion

        #region Funções de Eventos
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_IniciaLeituraEventos(int Handle, int Porta, int EnviarComandosNoEvento); /*R300*/
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_EncerraLeituraEventos(int Handle); /*R300*/
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaEvento(int Handle); /*R300*/
        #endregion

        #region Funções de Atualização
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_Empregador(int Handle, int Operacao, System.String Tipo, System.String Identificacao, System.String CEI, System.String RazaoSocial, System.String LocalTrabalho);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_Funcionario_Prepara(int Handle, int Operacao, System.String PIS, System.String Matricula, System.String NomeFuncionario, System.String TemplateBiometrico, System.String PIS_Teclado, System.String CodigoTeclado, System.String CodigoBarra, System.String CodigoMifare, System.String CodigoTAG);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_Funcionario_Envia(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_AtualizaDataHora(int Handle, System.String DataHora);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_AjustaHorarioVerao(int Handle, System.String DataInicio, System.String DataFim);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_ReinicializaSenhas(int Handle);
        #endregion

        #region Funções de Leitura
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaEmpregador(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaFuncionario(int Handle, System.String PIS);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaTodosFuncionarios(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaFuncionario_Prepara(int Handle, System.String PIS);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaPonto(int Handle, System.String DataInicio, System.String DataFim);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaLeituraMRP(int Handle, System.String DataInicio, System.String DataFim);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaLeituraMRP_NSR(int Handle, System.String NSRInicio, System.String NSRFim);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaStatus(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaStatusCompleto(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_BuscaHorarioVerao(int Handle);
        #endregion

        #region Funções de Transmissão de Comando
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_LimpaComandos(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_VerificaRetornoPenDrive(int Handle, int ID_Comando);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_ObtemCodigoErro(int Handle, int ID_Linha);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_ObtemMensagemErro(int Handle, StringBuilder sMensagemErro, int ID_Linha);
        #endregion

        #region Funções de Obtenção de Retornos
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_TotalRetornos(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoEmpregador(int Handle, StringBuilder Tipo, StringBuilder Identificacao, StringBuilder CEI, StringBuilder RazaoSocial, StringBuilder LocalTrabalho);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoFuncionario(int Handle, int IndiceLinha, StringBuilder PIS, StringBuilder Matricula, StringBuilder NomeFuncionario, StringBuilder TemplateBiometrico, StringBuilder PIS_Teclado, StringBuilder CodigoTeclado, StringBuilder CodigoBarras, StringBuilder CodigoMifare, StringBuilder CodigoTAG);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoPonto(int Handle, int Indicelinha, StringBuilder PIS, StringBuilder DataHora, StringBuilder NSR);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoLeituraMRP(int Handle, int IndiceLinha, StringBuilder RegistroAFD);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoStatus(int Handle, StringBuilder NumFabricacao, StringBuilder UltimaMarcacaoPIS, StringBuilder UltimaMarcacaodataHora, StringBuilder StatusPapel, StringBuilder DataHora, StringBuilder MemoriaTotalMRP, StringBuilder MemoriaUsoMRP);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoStatusCompleto(int Handle, StringBuilder NumFabricacao, StringBuilder UltimaMarcacaoPIS, StringBuilder UltimaMarcacaoDataHora, StringBuilder StatusPapel, StringBuilder DataHora, StringBuilder MemoriaTotalMRP, StringBuilder MemoriaUsoMRP, StringBuilder REPVersao, StringBuilder Cutter, StringBuilder MTVersao, StringBuilder EmpCad, StringBuilder EmpCadMax, StringBuilder NSRAtual, StringBuilder NSRMax, StringBuilder BioTipo, StringBuilder BioVersao, StringBuilder BioSeg, StringBuilder BioCad, StringBuilder BioMax, StringBuilder MRPVersao, StringBuilder RedeDHCP, StringBuilder RedeIP, StringBuilder RedeGateway, StringBuilder RedeMask, StringBuilder RedePort, StringBuilder RedeMAC, StringBuilder HorarioVerao, StringBuilder HorarioVeraoInicio, StringBuilder HorarioVeraoFim);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoHorarioVerao(int Handle, StringBuilder DataInicio, StringBuilder DataFim);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoReinicializaSenhas(int Handle);
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_VerificaRetornoComandoEvento(int Handle, int IDComando); /*R300*/
        [DllImport("dllrep.dll")]
        public static extern int DLLREP_RetornoUltimoEvento(int Handle, StringBuilder DataHora, StringBuilder NumFabricacao, StringBuilder Modelo, StringBuilder Imp1Ativada, StringBuilder Imp1SemPapel, StringBuilder Imp1CabecaQuente, StringBuilder Imp1Tampa, StringBuilder Imp1Nivel, StringBuilder Imp1TicketsRestantes, StringBuilder Imp1PoucoPapel, StringBuilder Imp2Ativada, StringBuilder Imp2SemPapel, StringBuilder Imp2CabecaQuente, StringBuilder Imp2Tampa, StringBuilder Imp2Nivel, StringBuilder Imp2TicketsRestantes, StringBuilder Imp2NivelPoucoPapel, StringBuilder BatBaixa, StringBuilder BatNivel, StringBuilder EnergiaFalta, StringBuilder EnergiaRetorno, StringBuilder PendriveFisco, StringBuilder PendriveComCmdUsuario, StringBuilder PendriveSemCmdUsuario, StringBuilder Intrusao, StringBuilder BatTampaAberta, StringBuilder ExtraindoRIM, StringBuilder MenuAdmin, StringBuilder MenuSup); /*R300*/
        #endregion

        public static int Handle;
        public static int Retorno;
        public static int Modo; /*Define se IP=0 ou Arquivo=1*/
        public static int ID_Comando;

        #endregion

        #region Funções de tratamento de retorno do REP

        /// <summary>
        /// Trata o código de retorno da DLL. O método lança um exceção caso um erro seja encontrado
        /// </summary>
        /// <param name="Ret">Código de retorno</param>
        public static void Trata_Retorno_DLL(Int32 Ret)
        {
            switch (Ret)
            {
                case -1:
                    throw new Exception("Erro DLL - Handle inválido!");

                case -2:
                    throw new Exception("Erro DLL - Erro de comunicação!");

                case -4:
                    throw new Exception("Erro DLL - Erro de timeout!");

                case -5:
                    throw new Exception("Erro DLL - Erro de protocolo!");

                case -6:
                    throw new Exception("Erro DLL - REP retornou mensagem de Erro. Utilize Função DLLREP_ObtemCodigoErro ou DLLREP_ObtemMensagemErro, para maiores detalhes");

                case -7:
                    throw new Exception("Erro DLL - Erro valor inválido / ausente");

                case -10:
                    throw new Exception("Erro DLL - Erro de gravação!");

                case -11:
                    throw new Exception("Erro DLL - Erro no buffer de envio. Utilize o comando LimpaComandos para liberar o buffer!");

                case -21:
                    throw new Exception("Erro DLL - Formato de data inválido!");
                case -22:
                    throw new Exception("Erro DLL - Tamanho do Codigo de Barras Invalido");
            }
        }

        /// <summary>
        /// Trata o código de retorno do Rep. O método lança uma exceção caso um erro seja encontrado
        /// </summary>
        /// <param name="Ret">Código de retorno</param>
        public static void Trata_Retorno_REP(Int32 Ret)
        {
            switch (Ret)
            {
                case -1:
                    throw new Exception("Erro REP - Erro da função. Resultado não disponível");
                case 1:
                    throw new Exception("Erro REP - Registro mal formado.");

                case 2:
                    throw new Exception("Erro REP - Valor inválido.");

                case 3:
                    throw new Exception("Erro REP - Falha durante a operação do comando.");

                case 4:
                    throw new Exception("Erro REP - Comando inválido");

                case 5:
                    throw new Exception("Erro REP - Registro não encontrado");

                case 6:
                    throw new Exception("Erro REP - Troca de comandos via pendrive desativada");

                case 7:
                    throw new Exception("Erro REP - Arquivo de comando excede tamanho permitido.");

                case 8:
                    throw new Exception("Erro REP - Espaço da memória de trabalho esgotado");

                case 9:
                    throw new Exception("Erro DLL - Espaço da MRP esgotado!");
                default:
                    break;
            }
        }

        /// <summary>
        /// Verifica se o driver está iniciado (Handle válido).
        /// O método lança uma exceção caso o driver não esteja iniciado (Handle inválido)
        /// </summary>
        private static void TrataHandle()
        {
            if (Handle <= 0)
            {
                throw new Exception("Driver não iniciado");
            }
        }
        #endregion

        #region Funções de Comunicação do REP ZPM
        
        public static void IniciaDriver(string NumeroRep, string ip, Int32 porta)
        {
            try
            {
                Handle = DLLREP_IniciaDriver(NumeroRep);
                TrataHandle();

                // Delay necessário pois foi verificado em debug do programa de exemplo
                // que a execução direta causava alguns erros intermitentes.
                // (SEH Exception/AccessViolation Exception)
                Thread.Sleep(2000);

                LimpaComandosRep();

                Int32 ret = 0;
                ret = DLLREP_DefineModoIP(Handle, ip, porta);

                Trata_Retorno_DLL(ret);
                Modo = 0;
            }
            catch (Exception e)
            {
                throw new Exception("Erro na inicialização do Driver! \r\n" + e.Message);
            }
        }

        public static void LimpaComandosRep()
        {
            TrataHandle();
            try
            {
                Int32 ret = 0;
                ret = DLLREP_LimpaComandos(REPZPM.Handle);
                Trata_Retorno_DLL(ret);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void EncerraDriver()
        {
            TrataHandle();
            try
            {
                Retorno = DLLREP_EncerraDriver(Handle);
                Trata_Retorno_DLL(Retorno);
            }
            catch (Exception e)
            {
                throw new Exception("Erro no encerramento do Driver. Favor verificar \r\n" + e.Message);
            }
        }

        public static Tuple<DateTime?, DateTime?> RetornaHorarioVeraoExistente()
        {
            TrataHandle();
            StringBuilder DataInicio = new StringBuilder(10);
            StringBuilder DataFim = new StringBuilder(10);
            LimpaComandosRep();
            Retorno = DLLREP_BuscaHorarioVerao(Handle);
            Trata_Retorno_DLL(Retorno);

            Retorno = DLLREP_RetornoHorarioVerao(Handle, DataInicio, DataFim);
            Trata_Retorno_DLL(Retorno);

            if ((Convert.ToString(DataInicio) == "0") && (Convert.ToString(DataFim) == "0"))
            {
                return new Tuple<DateTime?, DateTime?>(null, null);
            }
            else
            {
                List<string> dtIniList = DataInicio.ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> dtFimList = DataFim.ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (dtIniList.Count < 3 || dtFimList.Count < 3)
                {
                    throw new Exception("String de data inválida");
                }
                DateTime dtini = new DateTime(Convert.ToInt32(dtIniList[2]), Convert.ToInt32(dtIniList[1]), Convert.ToInt32(dtIniList[0]));
                DateTime dtfim = new DateTime(Convert.ToInt32(dtFimList[2]), Convert.ToInt32(dtFimList[1]), Convert.ToInt32(dtFimList[0]));
                return new Tuple<DateTime?, DateTime?>(dtini, dtfim);
            }
        }

        public static void AtualizaHorarioVerao(DateTime inicio, DateTime fim)
        {
            TrataHandle();
            LimpaComandosRep();
            ID_Comando = DLLREP_AjustaHorarioVerao(Handle, inicio.ToShortDateString(), fim.ToShortDateString());

            Trata_Retorno_DLL(ID_Comando);

            int Total_Retornos = DLLREP_TotalRetornos(Handle);
            int conta = 1;

            Dictionary<int, Exception> erros = new Dictionary<int, Exception>();
            while (conta <= Total_Retornos)
            {
                /*Obtém o código de erro do REP*/
                Trata_Retorno_REP(DLLREP_ObtemCodigoErro(Handle, conta));
                conta++;
            }
        }

        public static void AtualizaHorarioRelogio(DateTime? dataAtual)
        {
            DateTime dt = dataAtual.HasValue ? dataAtual.GetValueOrDefault() : DateTime.Now.AddSeconds(5);
            TrataHandle();
            LimpaComandosRep();
            string dtfmt = dt.ToString("dd/MM/yyyy HH:mm:ss");
            ID_Comando = DLLREP_AtualizaDataHora(Handle, dtfmt);

            Trata_Retorno_DLL(ID_Comando);

            int Total_Retornos = DLLREP_TotalRetornos(Handle);
            int conta = 1;

            Dictionary<int, Exception> erros = new Dictionary<int, Exception>();
            while (conta <= Total_Retornos)
            {
                /*Obtém o código de erro do REP*/
                Trata_Retorno_REP(DLLREP_ObtemCodigoErro(Handle, conta));
                conta++;
            }
        }

        public static void CadastraEmpregador(TipoPessoa ps, string CNPJCPF, string CEI, string NomeOuRazSoc, string LocalTrabalho)
        {
            TrataHandle();
            LimpaComandosRep();
            string cei = CEI;
            string pessoa = "";
            switch (ps)
            {
                case TipoPessoa.Fisica:
                    pessoa = "F";
                    break;
                case TipoPessoa.Juridica:
                    pessoa = "J";
                    break;
                default:
                    break;
            }

            if (String.IsNullOrEmpty(CNPJCPF))
            {
                throw new Exception("CPF/CNPJ não pode ser vazio ou nulo");
            }
            if (String.IsNullOrEmpty(NomeOuRazSoc))
            {
                throw new Exception("Nome/Razão Social do Empregador não pode ser vazio ou nulo");
            }
            if (String.IsNullOrEmpty(LocalTrabalho))
            {
                throw new Exception("Local de Trabalho não pode ser vazio ou nulo");
            }
            if (String.IsNullOrEmpty(cei))
            {
                cei = string.Empty;
            }

            try
            {
                ID_Comando = DLLREP_Empregador(Handle, (int)Operacao.Incluir, pessoa, CNPJCPF, cei, NomeOuRazSoc, LocalTrabalho);
                int Total_Retornos = DLLREP_TotalRetornos(Handle);
                Trata_Retorno_REP(DLLREP_ObtemCodigoErro(Handle, Total_Retornos));
            }
            catch (Exception e)
            {
                string msg = e.Message;
                LimpaComandosRep();
                if (string.IsNullOrEmpty(cei))
                {
                    cei = "#";
                }
                ID_Comando = DLLREP_Empregador(Handle, (int)Operacao.Alterar, pessoa, CNPJCPF, cei, NomeOuRazSoc, LocalTrabalho);
                int Total_Retornos = DLLREP_TotalRetornos(Handle);
                Trata_Retorno_REP(DLLREP_ObtemCodigoErro(Handle, Total_Retornos));
                msg = msg + "!";
            }
        }

        /// <summary>
        /// Realiza o envio de um funcionário para o Rep
        /// </summary>
        /// <param name="op">Operação à ser realizada com o funcionário: Inclusão, Alteração, Exclusão. Obrigatório</param>
        /// <param name="PIS">Código do PIS do Funcionário sem caracteres especiais (pontuação, etc.). Obrigatório</param>
        /// <param name="Matricula">Código de Matrícula do Funcionário sem caracteres especiais (pontuação, etc.). Obrigatório na Inclusão/Alteração</param>
        /// <param name="Nome">Nome do Funcionário sem caracteres especiais (pontuação, etc.). Obrigatório na Inclusão/Alteração</param>
        /// <param name="TemplatesBiometria">Lista de strings contendo os templates de Biometria do funcionário. Limite de 1 Template para Linha R200, e de 10 Templates para demais modelos. Opcional</param>
        /// <param name="HabilitarTeclado">Permite que o funcionário utilize o teclado</param>
        /// <param name="CodigoTeclado">Código de Identificação do funcionário. Obrigatório se HabilitarTeclado for igual a true</param>
        /// <param name="CodigoBarras">Código de Barras do funcionário. Enviar de acordo com o configurado no REP. Máximo 20 caracteres. Opcional</param>
        /// <param name="CodigoMifare">Código de Proximidade do funcionário. Opcional</param>
        /// <param name="CodigoTag">Código Tag do funcionário. Opcional</param>
        public static void EnviaFuncionario(Operacao op, string PIS, string Matricula, string Nome, IList<string> TemplatesBiometria, bool HabilitarTeclado, string CodigoTeclado, string CodigoBarras, string CodigoMifare, string CodigoTag)
        {
            TrataHandle();
            LimpaComandosRep();
            StringBuilder sbBio = new StringBuilder();
            string HabilitaTeclado = "N";

            if (HabilitarTeclado)
            {
                HabilitaTeclado = "S";
            }
            if (TemplatesBiometria != null)
            {
                int i = 0;
                while (i < TemplatesBiometria.Count && i < 10)
                {
                    sbBio.Append(TemplatesBiometria[i] + ";");
                    i++;
                }
            }

            #region Validações

            if (String.IsNullOrEmpty(PIS))
            {
                throw new Exception("Campo PIS não pode ser vazio");
            }
            if (op == Operacao.Alterar || op == Operacao.Incluir)
            {
                if (String.IsNullOrEmpty(Nome))
                {
                    throw new Exception("Campo Nome não pode ser vazio");
                }
                if (String.IsNullOrEmpty(Matricula))
                {
                    throw new Exception("Campo Matrícula não pode ser vazio");
                }
                if ((String.IsNullOrEmpty(CodigoTeclado) || CodigoTeclado == "#") && HabilitarTeclado)
                {
                    throw new Exception("Campo Codigo de Teclado não pode ser vazio ou excluido"
                        + " se o teclado estiver Habilitado para o funcionário");
                }
                if (!String.IsNullOrEmpty(CodigoBarras) && CodigoBarras.Length > 20)
                {
                    throw new Exception("Quantidade de dígitos do código de barras excede ao máximo suportado pelo REP");
                }
            }

            if (op == Operacao.Alterar) //CodigoMifare CodigoTag
            {
                if (String.IsNullOrEmpty(CodigoBarras))
                {
                    CodigoBarras = "#";
                }
                if (String.IsNullOrEmpty(CodigoMifare))
                {
                    CodigoMifare = "#";
                }
                if (String.IsNullOrEmpty(CodigoTag))
                {
                    CodigoTag = "#";
                }
            }
            #endregion

            Retorno = DLLREP_Funcionario_Prepara(Handle, (int)op, PIS,
                Matricula, Nome, sbBio.ToString(), HabilitaTeclado, CodigoTeclado,
                CodigoBarras, CodigoMifare, CodigoTag);
            if (Retorno != 1)
            {
                throw new Exception("Erro ao preparar funcionário para envio.");
            }
            ID_Comando = DLLREP_Funcionario_Envia(Handle);
            Trata_Retorno_DLL(ID_Comando);
            try
            {
                Retorno = DLLREP_ObtemCodigoErro(Handle, 1);
                Trata_Retorno_REP(Retorno);
            }
            catch (Exception e)
            {
                if (op == Operacao.Incluir)
                {
                    EnviaFuncionario(Operacao.Alterar, PIS, Matricula, Nome,
                        TemplatesBiometria, HabilitarTeclado, CodigoTeclado,
                        CodigoBarras, CodigoMifare, CodigoTag);
                }
                else if (op == Operacao.Excluir && e.Message.Contains("Registro não encontrado"))
                {
                    return;
                }
                else
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Recebe registros AFD do REP contidos no período especificado
        /// </summary>
        /// <param name="Inicio">Início do período de coleta</param>
        /// <param name="Fim">Fim do período de coleta</param>
        /// <param name="RepUsaDataHoraExtendida">Parâmetro que indica se o REP usa formato extendido na consulta ou não (true para formato DD/MM/YYYY HH:MM:SS - Somente p/ a família R300)</param>
        /// <returns></returns>
        public static IList<string> RecebeAFDPorPeriodo(DateTime Inicio, DateTime Fim, bool RepUsaDataHoraExtendida)
        {
            TrataHandle();
            LimpaComandosRep();
            int TotalRegistros;
            int i = 1;
            StringBuilder RegistroAFD = new StringBuilder(300);
            IList<string> result = new List<string>();

            string dtInicio = String.Empty;
            string dtFim = String.Empty;

            if (RepUsaDataHoraExtendida)
            {
                dtInicio = Inicio.ToString("dd/MM/yyyy HH:mm:ss");
                dtFim = Fim.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else
            {
                dtInicio = Inicio.ToShortDateString();
                dtFim = Fim.ToShortDateString();
            }

            ID_Comando = DLLREP_BuscaLeituraMRP(Handle, dtInicio, dtFim);
            Trata_Retorno_DLL(ID_Comando);

            TotalRegistros = DLLREP_TotalRetornos(Handle);

            /*Verifica se retornaram registros*/
            if (TotalRegistros > 0)
            {
                for (i = 1; i <= TotalRegistros; i++)
                {
                    Retorno = DLLREP_RetornoLeituraMRP(Handle, i, RegistroAFD);

                    /*Sucesso na execução do comando*/
                    if (Retorno == 1)
                    {
                        result.Add(RegistroAFD.ToString());
                    }
                    else
                    {
                        /*Trata o retorno de erro do REP*/
                        Trata_Retorno_REP(Retorno);
                    }
                }
            }
            return result;
        }

        public static IList<string> RecebeAFDPorIntervaloNsr(int Inicio, int Fim)
        {
            TrataHandle();
            LimpaComandosRep();
            int TotalRegistros;
            int i = 1;
            StringBuilder RegistroAFD = new StringBuilder(300);
            IList<string> result = new List<string>();

            string dtInicio = String.Empty;
            string dtFim = String.Empty;

            

            ID_Comando = DLLREP_BuscaLeituraMRP_NSR(Handle, Inicio.ToString(), Fim.ToString());
            Trata_Retorno_DLL(ID_Comando);

            TotalRegistros = DLLREP_TotalRetornos(Handle);

            /*Verifica se retornaram registros*/
            if (TotalRegistros > 0)
            {
                for (i = 1; i <= TotalRegistros; i++)
                {
                    Retorno = DLLREP_RetornoLeituraMRP(Handle, i, RegistroAFD);

                    /*Sucesso na execução do comando*/
                    if (Retorno == 1)
                    {
                        result.Add(RegistroAFD.ToString());
                    }
                    else
                    {
                        /*Trata o retorno de erro do REP*/
                        Trata_Retorno_REP(Retorno);
                    }
                }
            }
            return result;
        }
        #endregion

    }

    public enum Operacao
    {
        Excluir = 0,
        Incluir = 1,
        Alterar = 2
    }

    public enum TipoPessoa
    {
        Fisica,
        Juridica
    }
}
