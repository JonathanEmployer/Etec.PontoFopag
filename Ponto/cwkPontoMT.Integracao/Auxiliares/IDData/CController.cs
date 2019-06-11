using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Auxiliares.IDData
{
    public class CController
    {
        #region Variables

        private enuDDL_IDSysR30_Functions _eCurrentCommand;
        private enuConnectionState _eCurrentConnectionState;
        private CIDSysR30 _objIDSysR30;
        private CSocketClient _objSocketClient;
        private List<string> _lstrEventData;

        private string _strCommand_Error_Message;

        #endregion

        #region Constructor / Destructor

        public CController()
        {
            this._eCurrentCommand = enuDDL_IDSysR30_Functions.None;
            this._eCurrentConnectionState = enuConnectionState.None;

            this._objSocketClient = null;
            this._objIDSysR30 = null;

            this._lstrEventData = new List<string>();

            this._strCommand_Error_Message = "";
        }

        #endregion

        #region Generic Functions

        /// <summary>
        /// Formata a string
        /// </summary>
        /// <param name="p_strValue"></param>
        /// <param name="p_strMask"></param>
        /// <param name="p_strValueFormated"></param>
        private void FormatString(string p_strValue, string p_strMask, ref string p_strValueFormated)
        {
            StringBuilder strbValue = new StringBuilder();
            // remove caracteres nao numericos
            foreach (char c in p_strValue)
            {
                if (Char.IsNumber(c))
                    strbValue.Append(c);
            }

            int idxMask = p_strMask.Length;
            int idxValue = strbValue.Length;

            for (; idxValue > 0 && idxMask > 0; )
            {
                if (p_strMask[--idxMask] == '#')
                {
                    idxValue--;
                }
            }

            StringBuilder strbReturn = new StringBuilder();
            for (; idxMask < p_strMask.Length; idxMask++)
            {
                strbReturn.Append((p_strMask[idxMask] == '#') ? strbValue[idxValue++] : p_strMask[idxMask]);
            }
            p_strValueFormated = strbReturn.ToString();
        }

        #endregion

        #region Connection to equipment

        public enuConnectionState GetConnectionState()
        {
            enuConnectionState eAux = this._eCurrentConnectionState;

            if (eAux == enuConnectionState.DataReceivedError)
            {
                this._eCurrentConnectionState = enuConnectionState.Connected;
            }

            return eAux;
        }

        public void SetConnectionState(enuConnectionState eNewConnectionState)
        {
            this._eCurrentConnectionState = eNewConnectionState;
        }

        public bool ConnectServer(string strIPAddress, int iTCPPort)
        {
            bool bReturn = false;

            try
            {
                this._objSocketClient = new CSocketClient(strIPAddress, iTCPPort);
                this._objSocketClient.OnRead += new CSocketClient.BufferEventHandler(SocketClient_OnRead);
                this._objSocketClient.OnConnect += new CSocketClient.ConnectionDelegate(SocketClient_OnConnect);
                this._objSocketClient.OnDisconnect += new CSocketClient.ConnectionDelegate(SocketClient_OnDisconnect);
                this._objSocketClient.OnError += new CSocketClient.ErrorDelegate(SocketClient_OnError);

                this._eCurrentConnectionState = enuConnectionState.AttemptConnection;

                bReturn = this._objSocketClient.Connect();
            }
            catch (Exception exError)
            {
                throw exError;
            }

            return bReturn;
        }

        public bool DisconnectServer()
        {
            bool bReturn = false;

            try
            {
                if (this._objSocketClient != null)
                {
                    if (this._objSocketClient.Connected)
                    {
                        bReturn = this._objSocketClient.Disconnect();
                    }
                    else
                    {
                        bReturn = true;
                    }
                }
                else
                {
                    bReturn = true;
                }

                this._objSocketClient = null;
                this._eCurrentConnectionState = enuConnectionState.Disconnected;
            }
            catch (Exception exError)
            {
                throw exError;
            }

            return bReturn;
        }

        public bool IsConnected()
        {
            try
            {
                bool bReturn = false;

                if (this._objSocketClient != null)
                {
                    bReturn = this._objSocketClient.Connected;
                }

                return bReturn;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        private bool SendBuffer(byte[] rgbyTXBuffer)
        {
            try
            {
                bool bReturn = false;

                if (this._objSocketClient != null)
                {
                    bReturn = this._objSocketClient.SendBuffer(rgbyTXBuffer);
                }

                return bReturn;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        private void SocketClient_OnConnect()
        {
            this._eCurrentConnectionState = enuConnectionState.Connected;
        }

        private void SocketClient_OnDisconnect()
        {
            this._eCurrentConnectionState = enuConnectionState.Disconnected;
        }

        private void SocketClient_OnRead(byte[] rgbyTXBuffer)
        {
            try
            {
                int iCommand_Error_Code = this._objIDSysR30.PacketAvail(rgbyTXBuffer);

                switch (iCommand_Error_Code)
                {
                    case 0:
                        break;
                    case 2:
                        this.GetLogType2();
                        break;
                    case 3:
                        this.GetLogType3();
                        break;
                    case 4:
                        this.GetLogType4();
                        break;
                    case 5:
                        this.GetLogType5();
                        break;
                    case -1:
                        this._strCommand_Error_Message = "Cartão de Proximidade já cadastrado para outro usuário.";
                        break;
                    case -2:
                        this._strCommand_Error_Message = "Cartão de Código de Barras já cadastrado para outro usuário.";
                        break;
                    case -3:
                        this._strCommand_Error_Message = "PIS já cadastrado para outro usuário.";
                        break;
                    case -4:
                        this._strCommand_Error_Message = "Código individual já cadastrado para outro usuário.";
                        break;
                    case -5:
                        this._strCommand_Error_Message = "Erro na memória MRP.";
                        break;
                    case -6:
                        this._strCommand_Error_Message = "Erro na memória MT.";
                        break;
                    case -7:
                        this._strCommand_Error_Message = "Erro na memória RAM.";
                        break;
                    case -8:
                        this._strCommand_Error_Message = "Dados enviados inválidos.";
                        break;
                    case -9:
                        this._strCommand_Error_Message = "ID REP não possui trabalhadores cadastrados.";
                        break;
                    case -10:
                        this._strCommand_Error_Message = "Trabalhador não cadastrado.";
                        break;
                    case -11:
                        this._strCommand_Error_Message = "ID REP não possui o cadastro do empregador.";
                        break;
                    case -12:
                        this._strCommand_Error_Message = "Dados do empregador inválidos: CPF / CNPJ.";
                        break;
                    case -13:
                        this._strCommand_Error_Message = "Dados do empregador inválidos: Nome / Razão Social.";
                        break;
                    case -14:
                        this._strCommand_Error_Message = "Dados do empregador inválidos: Endereço.";
                        break;
                    case -15:
                        this._strCommand_Error_Message = "Data e/ou hora inválida(s).";
                        break;
                    case -16:
                        this._strCommand_Error_Message = "Erro no módulo biométrico: ERROR.";
                        break;
                    case -17:
                        this._strCommand_Error_Message = "Erro no módulo biométrico: TIMEOUT.";
                        break;
                    case -18:
                        this._strCommand_Error_Message = "Dados de comunicação inválidos: Endereço IP.";
                        break;
                    case -19:
                        this._strCommand_Error_Message = "Dados de comunicação inválidos: Máscara de sub-rede.";
                        break;
                    case -20:
                        this._strCommand_Error_Message = "Dados de comunicação inválidos: IP Gateway.";
                        break;
                    case -21:
                        this._strCommand_Error_Message = "Não existem eventos.";
                        break;
                    case -22:
                        this._strCommand_Error_Message = "Erro no módulo biométrico: CHEIO.";
                        break;
                    case -23:
                        this._strCommand_Error_Message = "Erro na leitura do módulo biométrico: ERROR.";
                        break;
                    case -24:
                        this._strCommand_Error_Message = "Erro na leitura do módulo biométrico: TIMEOUT.";
                        break;
                    case -25:
                        this._strCommand_Error_Message = "Erro de checksum da área de dados.";
                        break;
                    case -26:
                        this._strCommand_Error_Message = "Dados do empregador inválidos: CEI.";
                        break;
                    case -27:
                        this._strCommand_Error_Message = "Equipamento bloqueado.";
                        break;
                    case -100:
                        this._strCommand_Error_Message = "Erro no checksum do cabeçalho do pacote (Verificação na DLL).";
                        break;
                    case -101:
                        this._strCommand_Error_Message = "Erro no checksum dos dados do pacote (Verificação na DLL).";
                        break;
                    case -102:
                        this._strCommand_Error_Message = "Comando inválido (Verificação na DLL).";
                        break;
                    case -103:
                        this._strCommand_Error_Message = "Erro pacote inválido (Verificação na DLL).";
                        break;
                    case -104:
                        this._strCommand_Error_Message = "Erro no tamanho do pacote: pacote vazio (Verificação na DLL).";
                        break;
                    case -105:
                        this._strCommand_Error_Message = "Erro no tamanho dos dados (Verificação na DLL).";
                        break;
                    default:
                        this._strCommand_Error_Message = "Erro desconhecido.";
                        break;
                }

                if (iCommand_Error_Code >= 0)
                {
                    this._eCurrentConnectionState = enuConnectionState.DataReceived;

                    switch (this._eCurrentCommand)
                    {
                        case enuDDL_IDSysR30_Functions.ReadUserData:
                            {
                                this.GetUserData();
                            }
                            break;
                        case enuDDL_IDSysR30_Functions.ReadEmployerData:
                            {
                                this.GetEmployerData();
                            }
                            break;
                        case enuDDL_IDSysR30_Functions.ReadREPCommunication:
                            {
                                this.GetREPCommunication();
                            }
                            break;
                        case enuDDL_IDSysR30_Functions.RequestNFR:
                            {
                                this.GetNFR();
                            }
                            break;
                        case enuDDL_IDSysR30_Functions.RequestTotalNSR:
                            {
                                this.GetTotalNSR();
                            }
                            break;
                        case enuDDL_IDSysR30_Functions.RequestTotalUsers:
                            {
                                this.GetTotalUsers();
                            }
                            break;
                        default:
                            {
                                this.CommandOK();
                            }
                            break;
                    }
                }
                else
                {
                    this._eCurrentConnectionState = enuConnectionState.DataReceivedError;

                    this._lstrEventData.Add(" ");
                    this._lstrEventData.Add("Erro : " + iCommand_Error_Code.ToString() + " - " + _strCommand_Error_Message);
                }
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        private void SocketClient_OnError(string strErroMessage, int iErroCode)
        {
            if (this._eCurrentConnectionState == enuConnectionState.AttemptConnection)
            {
                this._eCurrentConnectionState = enuConnectionState.AttemptConnectionFail;
            }
            else
            {
                this._eCurrentConnectionState = enuConnectionState.ConnectionError;
            }

            this._lstrEventData.Add(" ");
            this._lstrEventData.Add("Erro : " + iErroCode.ToString() + " - " + strErroMessage);
        }

        #endregion

        #region DLL

        public bool LoadDLL()
        {
            try
            {
                this._objIDSysR30 = new CIDSysR30();
                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        #endregion

        #region Communication Functions

        public void SetCommand(enuDDL_IDSysR30_Functions eNewCommand)
        {
            this._eCurrentCommand = eNewCommand;
        }

        public enuDDL_IDSysR30_Functions GetCommand()
        {
            return this._eCurrentCommand;
        }

        public bool AddUser(string strPIS, string strUserName, uint uiKeyCode, string strBarCode, byte byFacilityCode, uint uiProxCode, byte byUserType, byte byAccessType, string strPassword, System.IO.MemoryStream msPhoto, ushort usSizeSample, byte byQuantitySamples, byte[] rgbyBiometrics)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.AddUser(strPIS, strUserName, uiKeyCode, strBarCode, byFacilityCode, uiProxCode, byUserType, byAccessType, strPassword, msPhoto, usSizeSample, byQuantitySamples, rgbyBiometrics);

                if (this.SendBuffer(rgbyBuffer) == false)
                {
                    return false;
                }

                this._eCurrentCommand = enuDDL_IDSysR30_Functions.AddUser;
                this._eCurrentConnectionState = enuConnectionState.SendingData;

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Clear();
                this._lstrEventData.Add("--------------------------------------------------");
                this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                this._lstrEventData.Add("Dados do Usuário Enviados");
                this._lstrEventData.Add("--------------------------------------------------");
                this._lstrEventData.Add("");

                strMask = "###.#####.##-#";
                this.FormatString(strPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS...............................: " + strFormatedValue);

                this._lstrEventData.Add("Nome do Trabalhador...............: " + strUserName);
                this._lstrEventData.Add("Tipo (UserType)...................: " + byUserType.ToString());
                this._lstrEventData.Add("Modo de Marcação (Status).........: " + byUserType.ToString());
                this._lstrEventData.Add("Código (KeyCode)..................: " + uiKeyCode.ToString());
                this._lstrEventData.Add("Senha (Password)..................: " + strPassword);
                this._lstrEventData.Add("Proximidade (ProxCode)............: " + uiProxCode.ToString());
                this._lstrEventData.Add("Barras (BarCode)..................: " + strBarCode);
                this._lstrEventData.Add("Tamanho da Foto (SizePhoto).......: 0");
                this._lstrEventData.Add("Quant. Amostras (QuantitySamples).: " + byQuantitySamples.ToString());
                this._lstrEventData.Add("Tamanho Amostras (SizeSample).....: " + usSizeSample.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool ChangeUserData(string strPIS, string strNewPIS, string strUserName, uint uiKeyCode, string strBarCode, byte byFacilityCode, ushort usProxCode, byte byUserType, string strPassword, System.IO.MemoryStream msPhoto, ushort usSizeSample, byte byQuantitySamples, byte[] rgbyBiometrics)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.ChangeUserData(strPIS, strNewPIS, strUserName, uiKeyCode, strBarCode, byFacilityCode, usProxCode, byUserType, strPassword, msPhoto, usSizeSample, byQuantitySamples, rgbyBiometrics);

                if (this.SendBuffer(rgbyBuffer) == false)
                {
                    return false;
                }

                this._eCurrentCommand = enuDDL_IDSysR30_Functions.ChangeUserData;
                this._eCurrentConnectionState = enuConnectionState.SendingData;

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Clear();
                this._lstrEventData.Add("--------------------------------------------------");
                this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                this._lstrEventData.Add("Dados do Usuário Enviados");
                this._lstrEventData.Add("--------------------------------------------------");
                this._lstrEventData.Add("");

                strMask = "###.#####.##-#";
                this.FormatString(strPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS...............................: " + strFormatedValue);

                this.FormatString(strNewPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("Novo PIS..........................: " + strFormatedValue);

                this._lstrEventData.Add("Nome do Trabalhador...............: " + strUserName);
                this._lstrEventData.Add("Tipo (UserType)...................: " + byUserType.ToString());
                this._lstrEventData.Add("Modo de Marcação (Status).........: " + byUserType.ToString());
                this._lstrEventData.Add("Código (KeyCode)..................: " + uiKeyCode.ToString());
                this._lstrEventData.Add("Senha (Password)..................: " + strPassword);
                this._lstrEventData.Add("Proximidade (ProxCode)............: " + usProxCode.ToString());
                this._lstrEventData.Add("Barras (BarCode)..................: " + strBarCode);
                this._lstrEventData.Add("Tamanho da Foto (SizePhoto).......: 0");
                this._lstrEventData.Add("Quant. Amostras (QuantitySamples).: " + byQuantitySamples.ToString());
                this._lstrEventData.Add("Tamanho Amostras (SizeSample).....: " + usSizeSample.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool DeleteUser(string strPIS)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.DeleteUser(strPIS);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.DeleteUser;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    string strMask = "";
                    string strFormatedValue = "";

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    strMask = "###.#####.##-#";
                    this.FormatString(strPIS, strMask, ref strFormatedValue);
                    this._lstrEventData.Add("PIS : " + strFormatedValue);

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool ReadUserData(string strPIS)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.ReadUserData(strPIS);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.ReadUserData;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    string strMask = "";
                    string strFormatedValue = "";

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    strMask = "###.#####.##-#";
                    this.FormatString(strPIS, strMask, ref strFormatedValue);
                    this._lstrEventData.Add("PIS : " + strFormatedValue);

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool SetDST(DateTime start, DateTime end)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.SetDST(start, end);
                if (this.SendBuffer(rgbyBuffer))
                {

                }
            }
            catch (Exception exError)
            {
                throw exError;
            }
            return false;
        }

        public bool SetEmployer(byte byIdentifyType, string strCNPJ_CPF, ulong ulCEI, string strEmployerName, string strEmployerAddress)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.SetEmployer(byIdentifyType, strCNPJ_CPF, ulCEI, strEmployerName, strEmployerAddress);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.SetEmployer;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    string strMask = "";
                    string strFormatedValue = "";

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("Dados da Empresa Enviados");
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    this._lstrEventData.Add("Identificador..................: " + byIdentifyType.ToString() + " (1 - CNPJ / 2 - CPF )");

                    if (byIdentifyType == 1)
                    {
                        strMask = "##.###.###/####-##";
                        this.FormatString(strCNPJ_CPF, strMask, ref strFormatedValue);
                        this._lstrEventData.Add("CNPJ...........................: " + strFormatedValue);
                    }
                    else
                    {
                        strMask = "###.###.###-##";
                        this.FormatString(strCNPJ_CPF, strMask, ref strFormatedValue);
                        this._lstrEventData.Add("CPF............................: " + strFormatedValue);
                    }

                    this._lstrEventData.Add("CEI............................: " + ulCEI.ToString());
                    this._lstrEventData.Add("Nome da Empresa/Empregador.....: " + strEmployerName.ToString());
                    this._lstrEventData.Add("Endereço da Empresa/Empregador.: " + strEmployerAddress.ToString());

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool ReadEmployerData()
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.ReadEmployerData();

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.ReadEmployerData;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool SetDateTime(byte byDay, byte byMonth, ushort usYear, byte byHour, byte byMinute, byte bySecond)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.SetDateTime(byDay, byMonth, usYear, byHour, byMinute, bySecond);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.SetDateTime;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    this._lstrEventData.Add("Nova Data : " + byDay.ToString("00") + "//" + byMonth.ToString("00") + "//" + usYear.ToString("0000"));
                    this._lstrEventData.Add("Nova Hora : " + byHour.ToString("00") + "//" + byMinute.ToString("00") + "//" + bySecond.ToString("00"));

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool SetREPCommunication(byte byCommunicationType, string strIPEquipment, string strSubnetMask, string strIPGateway, ushort usTCPPort_Comm, ushort usTCPPort_Alarm, byte byBaudrate, byte bySerialAddress, byte byMulticastAddress, byte byBroadcastAddress)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.SetREPCommunication(byCommunicationType, strIPEquipment, strSubnetMask, strIPGateway, usTCPPort_Comm, usTCPPort_Alarm, byBaudrate, bySerialAddress, byMulticastAddress, byBroadcastAddress);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.SetREPCommunication;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    string strIPMode = "IP Fixo";

                    if (strIPEquipment == "0.0.0.0" || strIPEquipment == "")
                    {
                        strIPMode = "DHCP";
                    }

                    this._lstrEventData.Add("Obter Endereço IP....: " + strIPMode);
                    this._lstrEventData.Add("Endereço IP..........: " + strIPEquipment);
                    this._lstrEventData.Add("Máscara de sub-rede..: " + strSubnetMask);
                    this._lstrEventData.Add("Gateway padrão.......: " + strIPGateway);
                    this._lstrEventData.Add("Porta de Comunicação.: " + usTCPPort_Comm.ToString());
                    this._lstrEventData.Add("Baudrate.............: " + byBaudrate.ToString());
                    this._lstrEventData.Add("Endreço Serial.......: " + bySerialAddress.ToString());
                    this._lstrEventData.Add("Endreço Multicast....: " + byMulticastAddress.ToString());
                    this._lstrEventData.Add("Endreço Broadcast....: " + byBroadcastAddress.ToString());

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool ReadREPCommunication()
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.ReadREPCommunication();

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.ReadREPCommunication;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool RequestEventByNSR(uint uiNSR)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.RequestEventByNSR(uiNSR);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.RequestEventByNSR;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");
                    this._lstrEventData.Add("NSR : " + uiNSR.ToString());

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool RequestNFR()
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.RequestNFR();

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.RequestNFR;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool RequestTotalNSR()
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.RequestTotalNSR();

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.RequestTotalNSR;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool RequestTotalUsers()
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.RequestTotalUsers();

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.RequestTotalUsers;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool RequestUserByIndex(uint uiIndex)
        {
            try
            {
                byte[] rgbyBuffer = this._objIDSysR30.RequestUserByIndex(uiIndex);

                if (this.SendBuffer(rgbyBuffer))
                {
                    this._eCurrentCommand = enuDDL_IDSysR30_Functions.ReadUserData;
                    this._eCurrentConnectionState = enuConnectionState.SendingData;

                    this._lstrEventData.Clear();
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("Comando " + this._eCurrentCommand.ToString());
                    this._lstrEventData.Add("--------------------------------------------------");
                    this._lstrEventData.Add("");
                    this._lstrEventData.Add("Índice : " + uiIndex.ToString());

                    return true;
                }

                return false;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetUserData()
        {
            try
            {
                string strPIS = "";
                string strUserName = "";
                uint uiKeyCode = 0;
                string strBarCode = "";
                byte byFacilityCode = 0;
                ushort usProxCode = 0;
                byte byUserType = 0;
                string strPassword = "";
                ushort usSizePhoto = 0;
                System.IO.MemoryStream msPhoto = null;
                ushort usSizeSample = 0;
                byte byQuantitySamples = 0;
                byte[] rgbyBiometric_Sample1 = new byte[404];
                byte[] rgbyBiometric_Sample2 = new byte[404];

                this._objIDSysR30.GetUserData(ref strPIS, ref strUserName, ref uiKeyCode, ref strBarCode, ref byFacilityCode, ref usProxCode, ref byUserType, ref strPassword, ref usSizePhoto, ref msPhoto, ref usSizeSample, ref byQuantitySamples, ref rgbyBiometric_Sample1, ref rgbyBiometric_Sample2);

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Add("Dados do Usuário");
                this._lstrEventData.Add("");

                strMask = "###.#####.##-#";
                this.FormatString(strPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS...............................: " + strFormatedValue);

                this._lstrEventData.Add("Nome do Trabalhador...............: " + strUserName);
                this._lstrEventData.Add("Tipo (UserType)...................: " + byUserType.ToString());
                this._lstrEventData.Add("Modo de Marcação (Status).........: " + byUserType.ToString());
                this._lstrEventData.Add("Código (KeyCode)..................: " + uiKeyCode.ToString());
                this._lstrEventData.Add("Senha (Password)..................: " + strPassword.ToString());
                this._lstrEventData.Add("Tamanho da Foto (SizePhoto).......: " + usSizePhoto.ToString());
                this._lstrEventData.Add("Quant. Amostras (QuantitySamples).: " + byQuantitySamples.ToString());
                this._lstrEventData.Add("Tamanho Amostras (SizeSample).....: " + usSizeSample.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetEmployerData()
        {
            try
            {
                byte byIdentifyType = 0;
                ulong ulCNPJ_CPF = 0;
                ulong ulCEI = 0;
                string strEmployerName = "";
                string strEmployerAddress = "";

                this._objIDSysR30.GetEmployerData(ref byIdentifyType, ref ulCNPJ_CPF, ref ulCEI, ref strEmployerName, ref strEmployerAddress);

                this._lstrEventData.Add("Dados da Empresa/Empregador");
                this._lstrEventData.Add("");

                this._lstrEventData.Add("Identificador..................: " + byIdentifyType.ToString() + " (1 - CNPJ / 2 - CPF )");

                string strMask = "";
                string strFormatedValue = "";

                if (byIdentifyType == 1)
                {
                    strMask = "##.###.###/####-##";
                    this.FormatString(ulCNPJ_CPF.ToString(), strMask, ref strFormatedValue);
                    this._lstrEventData.Add("CNPJ...........................: " + strFormatedValue);
                }
                else
                {
                    strMask = "###.###.###-##";
                    this.FormatString(ulCNPJ_CPF.ToString(), strMask, ref strFormatedValue);
                    this._lstrEventData.Add("CPF............................: " + strFormatedValue);
                }

                this._lstrEventData.Add("CEI............................: " + ulCEI.ToString());
                this._lstrEventData.Add("Nome da Empresa/Empregador.....: " + strEmployerName.ToString());
                this._lstrEventData.Add("Endereço da Empresa/Empregador.: " + strEmployerAddress.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetLogType2()
        {
            try
            {
                uint uiNSR = 0;
                byte byRegType = 0;
                byte byRegDateDay = 0;
                byte byRegDateMonth = 0;
                ushort usRegDateYear = 0;
                byte byRegTimeHour = 0;
                byte byRegTimeMin = 0;
                byte byIdentifyType = 0;
                ulong ulCNPJ_CPF = 0;
                ulong ulCEI = 0;
                string strEmployerName = "";
                string strEmployerAddress = "";

                this._objIDSysR30.GetLogType2(ref uiNSR, ref byRegType, ref byRegDateDay, ref byRegDateMonth, ref usRegDateYear, ref byRegTimeHour, ref byRegTimeMin, ref byIdentifyType, ref ulCNPJ_CPF, ref ulCEI, ref strEmployerName, ref strEmployerAddress);

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Add("Evento Tipo 2 (Inclusão/alteraçao dos dados do Empregador)");
                this._lstrEventData.Add("");
                this._lstrEventData.Add("NSR................................: " + uiNSR.ToString());
                this._lstrEventData.Add("Tipo do Registro...................: " + byRegType.ToString());
                this._lstrEventData.Add("Data do Registro...................: " + byRegDateDay.ToString("00") + "/" + byRegDateMonth.ToString("00") + "/" + usRegDateYear.ToString("0000"));
                this._lstrEventData.Add("Hora do Registro...................: " + byRegTimeHour.ToString("00") + ":" + byRegTimeMin.ToString("00"));
                this._lstrEventData.Add("Identificador......................: " + byIdentifyType.ToString() + " (1 - CNPJ / 2 - CPF )");

                if (byIdentifyType == 1)
                {
                    strMask = "##.###.###/####-##";
                    this.FormatString(ulCNPJ_CPF.ToString(), strMask, ref strFormatedValue);
                    this._lstrEventData.Add("CNPJ...........................: " + strFormatedValue);
                }
                else
                {
                    strMask = "###.###.###-##";
                    this.FormatString(ulCNPJ_CPF.ToString(), strMask, ref strFormatedValue);
                    this._lstrEventData.Add("CPF............................: " + strFormatedValue);
                }

                this._lstrEventData.Add("CEI............................: " + ulCEI.ToString());
                this._lstrEventData.Add("Nome da Empresa/Empregador.....: " + strEmployerName.ToString());
                this._lstrEventData.Add("Endereço da Empresa/Empregador.: " + strEmployerAddress.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetLogType3()
        {
            try
            {
                uint uiNSR = 0;
                byte byRegType = 0;
                byte byRegDateDay = 0;
                byte byRegDateMonth = 0;
                ushort usRegDateYear = 0;
                byte byRegTimeHour = 0;
                byte byRegTimeMin = 0;
                string strPIS = "";

                this._objIDSysR30.GetLogType3(ref uiNSR, ref byRegType, ref byRegDateDay, ref byRegDateMonth, ref usRegDateYear, ref byRegTimeHour, ref byRegTimeMin, ref strPIS);

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Add("Evento Tipo 3 (Marcação do Ponto)");
                this._lstrEventData.Add("");
                this._lstrEventData.Add("NSR................................: " + uiNSR.ToString());
                this._lstrEventData.Add("Tipo do Registro...................: " + byRegType.ToString());
                this._lstrEventData.Add("Data do Registro...................: " + byRegDateDay.ToString("00") + "/" + byRegDateMonth.ToString("00") + "/" + usRegDateYear.ToString("0000"));
                this._lstrEventData.Add("Hora do Registro...................: " + byRegTimeHour.ToString("00") + ":" + byRegTimeMin.ToString("00"));

                strMask = "###.#####.##-#";
                this.FormatString(strPIS.ToString().PadLeft(11, '0'), strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS................................: " + strFormatedValue);

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetLogType4()
        {
            try
            {
                uint uiNSR = 0;
                byte byRegType = 0;
                byte byDayBeforeAdjust = 0;
                byte byMonthBeforeAdjust = 0;
                ushort usYearBeforeAdjust = 0;
                byte byHourBeforeAdjust = 0;
                byte byMinuteBeforeAdjust = 0;
                byte byDayAfterAdjust = 0;
                byte byMonthAfterAdjust = 0;
                ushort usYearAfterAdjust = 0;
                byte byHourAfterAdjust = 0;
                byte byMinuteAfterAdjust = 0;

                this._objIDSysR30.GetLogType4(ref uiNSR, ref byRegType, ref byDayBeforeAdjust, ref byMonthBeforeAdjust, ref usYearBeforeAdjust, ref byHourBeforeAdjust, ref byMinuteBeforeAdjust, ref byDayAfterAdjust, ref byMonthAfterAdjust, ref usYearAfterAdjust, ref byHourAfterAdjust, ref byMinuteAfterAdjust);

                this._lstrEventData.Add("Evento Tipo 4 (Alteraçao de Data e Hora)");
                this._lstrEventData.Add("");
                this._lstrEventData.Add("NSR......................: " + uiNSR.ToString());
                this._lstrEventData.Add("Tipo do Registro.........: " + byRegType.ToString());
                this._lstrEventData.Add("Data Antes da Alteração..: " + byDayBeforeAdjust.ToString("00") + "/" + byMonthBeforeAdjust.ToString("00") + "/" + byHourBeforeAdjust.ToString("0000"));
                this._lstrEventData.Add("Hora Antes da Alteração..: " + byHourBeforeAdjust.ToString("00") + ":" + byMinuteBeforeAdjust.ToString("00"));
                this._lstrEventData.Add("Data Depois da Alteração.: " + byDayAfterAdjust.ToString("00") + "/" + byMonthAfterAdjust.ToString("00") + "/" + usYearAfterAdjust.ToString("0000"));
                this._lstrEventData.Add("Hora Depois da Alteração.: " + byHourAfterAdjust.ToString("00") + ":" + byMinuteAfterAdjust.ToString("00"));

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetLogType5()
        {
            try
            {
                uint uiNSR = 0;
                byte byRegType = 0;
                byte byRegDateDay = 0;
                byte byRegDateMonth = 0;
                ushort usRegDateYear = 0;
                byte byRegTimeHour = 0;
                byte byRegTimeMin = 0;
                byte byOpType = 0;
                string strPIS = "";
                string strUsername = "";

                this._objIDSysR30.GetLogType5(ref uiNSR, ref byRegType, ref byRegDateDay, ref byRegDateMonth, ref usRegDateYear, ref byRegTimeHour, ref byRegTimeMin, ref byOpType, ref strPIS, ref strUsername);

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Add("Evento Tipo 5 (Inclusão/alteraçao/exclusão de Usuário)");
                this._lstrEventData.Add("");
                this._lstrEventData.Add("NSR.................: " + uiNSR.ToString());
                this._lstrEventData.Add("Tipo do Registro....: " + byRegType.ToString());
                this._lstrEventData.Add("Data do Registro....: " + byRegDateDay.ToString("00") + "/" + byRegDateMonth.ToString("00") + "/" + usRegDateYear.ToString("0000"));
                this._lstrEventData.Add("Hora do Registro....: " + byRegTimeHour.ToString("00") + ":" + byRegTimeMin.ToString("00"));
                this._lstrEventData.Add("Operação............: " + (char)byOpType + " ( I - Inclusão / A - Alteração / E - Exclusão )");

                strMask = "###.#####.##-#";
                this.FormatString(strPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS.................: " + strFormatedValue);

                this._lstrEventData.Add("Nome do Trabalhador.: " + strUsername);

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetREPCommunication()
        {
            try
            {
                byte byCommunicationType = 0;
                string strIPEquipment = "";
                string strSubnetMask = "";
                string strIPGateway = "";
                ushort usTCPPort_Comm = 0;
                ushort usTCPPort_Alarm = 0;
                byte byBaudrate = 0;
                byte bySerialAddress = 0;
                byte byMulticastAddress = 0;
                byte byBroadcastAddress = 0;

                this._objIDSysR30.GetREPCommunication(ref byCommunicationType, ref strIPEquipment, ref strSubnetMask, ref strIPGateway, ref usTCPPort_Comm, ref usTCPPort_Alarm, ref byBaudrate, ref bySerialAddress, ref byMulticastAddress, ref byBroadcastAddress);

                this._lstrEventData.Add("Dados da Configuração de Comunicação do Equipamento");
                this._lstrEventData.Add("");

                string strIPMode = "IP Fixo";

                if (strIPEquipment == "0.0.0.0" || strIPEquipment == "")
                {
                    strIPMode = "DHCP";
                }

                this._lstrEventData.Add("Obter Endereço IP....: " + strIPMode);
                this._lstrEventData.Add("Endereço IP..........: " + strIPEquipment);
                this._lstrEventData.Add("Máscara de sub-rede..: " + strSubnetMask);
                this._lstrEventData.Add("Gateway padrão.......: " + strIPGateway);
                this._lstrEventData.Add("Porta de Comunicação.: " + usTCPPort_Comm.ToString());
                this._lstrEventData.Add("Baudrate.............: " + byBaudrate.ToString());
                this._lstrEventData.Add("Endreço Serial.......: " + bySerialAddress.ToString());
                this._lstrEventData.Add("Endreço Multicast....: " + byMulticastAddress.ToString());
                this._lstrEventData.Add("Endreço Broadcast....: " + byBroadcastAddress.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetNFR()
        {
            try
            {
                string strNFR = "";

                this._objIDSysR30.GetNFR(ref strNFR);

                this._lstrEventData.Add("NFR do Equipamento");
                this._lstrEventData.Add("");

                this._lstrEventData.Add("NFR : " + strNFR);

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public int GetTotalNSR()
        {
            try
            {
                uint uiTotalNSR = 0;

                this._objIDSysR30.GetTotalNSR(ref uiTotalNSR);

                this._lstrEventData.Add("Total de Eventos Registrados no Equipamento");
                this._lstrEventData.Add("");

                this._lstrEventData.Add("Total : " + uiTotalNSR.ToString());

                return Convert.ToInt32(uiTotalNSR);
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool GetTotalUsers()
        {
            try
            {
                uint uiTotalUsers = 0;

                this._objIDSysR30.GetTotalUsers(ref uiTotalUsers);

                this._lstrEventData.Add("Total de Usuários Registrados no Equipamento");
                this._lstrEventData.Add("");

                this._lstrEventData.Add("Total : " + uiTotalUsers.ToString());

                return true;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public void CommandOK()
        {
            this._lstrEventData.Add("");
            this._lstrEventData.Add("Resposta do comando " + this.GetCommand().ToString() + " recebida com sucesso.");
            this._lstrEventData.Add("");
        }

        public List<string> GetEventData()
        {
            return this._lstrEventData;
        }

        #endregion

        #region Other Functions

        public bool IsHamsterConnected()
        {
            return this._objIDSysR30.IsHamsterConnected();
        }

        public bool GetTemplates_FIM01(ref byte[] rgbySample404_1, ref byte byFingerPID_1, ref byte bySampleID_1, ref byte byQuality_1, ref byte[] rgbySample404_2, ref byte byFingerPID_2, ref byte bySampleID_2, ref byte byQuality_2, bool bRotateSamples)
        {
            if (this._objIDSysR30.GetTemplates_FIM01(ref rgbySample404_1, ref byFingerPID_1, ref bySampleID_1, ref byQuality_1, ref rgbySample404_2, ref byFingerPID_2, ref bySampleID_2, ref byQuality_2, bRotateSamples) == false)
            {
                return false;
            }

            this._lstrEventData.Clear();
            this._lstrEventData.Add("Função GetTemplates_FIM01");
            this._lstrEventData.Add("");

            this._lstrEventData.Add("Amostra 1 - Template.....: OK");
            this._lstrEventData.Add("Amostra 1 - Dedo.........: " + byFingerPID_1.ToString());
            this._lstrEventData.Add("Amostra 1 - Qual amostra.: " + bySampleID_1.ToString());
            this._lstrEventData.Add("Amostra 1 - Qualidade....: " + bySampleID_1.ToString());
            this._lstrEventData.Add("Amostra 2 - Template.....: OK");
            this._lstrEventData.Add("Amostra 2 - Dedo.........: " + byFingerPID_2.ToString());
            this._lstrEventData.Add("Amostra 2 - Qual amostra.: " + bySampleID_2.ToString());
            this._lstrEventData.Add("Amostra 2 - Qualidade....: " + bySampleID_2.ToString());

            if (bRotateSamples)
            {
                this._lstrEventData.Add("Rotacionar amostras......: Sim");
            }
            else
            {
                this._lstrEventData.Add("Rotacionar amostras......: Não");
            }

            return true;
        }

        public bool GetTemplates_FIM10(ref byte[] rgbySample400_1, ref byte byFingerPID_1, ref byte bySampleID_1, ref byte byQuality_1, ref byte[] rgbySample400_2, ref byte byFingerPID_2, ref byte bySampleID_2, ref byte byQuality_2, bool bRotateSamples)
        {
            if (this._objIDSysR30.GetTemplates_FIM10(ref rgbySample400_1, ref byFingerPID_1, ref bySampleID_1, ref byQuality_1, ref rgbySample400_2, ref byFingerPID_2, ref bySampleID_2, ref byQuality_2, bRotateSamples) == false)
            {
                return false;
            }

            this._lstrEventData.Clear();
            this._lstrEventData.Add("Função GetTemplates_FIM10");
            this._lstrEventData.Add("");

            this._lstrEventData.Add("Amostra 1 - Template.....: OK");
            this._lstrEventData.Add("Amostra 1 - Dedo.........: " + byFingerPID_1.ToString());
            this._lstrEventData.Add("Amostra 1 - Qual amostra.: " + bySampleID_1.ToString());
            this._lstrEventData.Add("Amostra 1 - Qualidade....: " + bySampleID_1.ToString());
            this._lstrEventData.Add("Amostra 2 - Template.....: OK");
            this._lstrEventData.Add("Amostra 2 - Dedo.........: " + byFingerPID_2.ToString());
            this._lstrEventData.Add("Amostra 2 - Qual amostra.: " + bySampleID_2.ToString());
            this._lstrEventData.Add("Amostra 2 - Qualidade....: " + bySampleID_2.ToString());

            if (bRotateSamples)
            {
                this._lstrEventData.Add("Rotacionar amostras......: Sim");
            }
            else
            {
                this._lstrEventData.Add("Rotacionar amostras......: Não");
            }
            return true;
        }

        public bool ConvertTemplate404ToTemplate400(byte[] rgbyTemplate404, ref byte[] rgbyTemplate400)
        {
            return this._objIDSysR30.ConvertTemplate404ToTemplate400(rgbyTemplate404, ref rgbyTemplate400);
        }

        public bool ConvertTemplate400ToTemplate404(byte[] rgbyTemplate400, ref byte[] rgbyTemplate404)
        {
            return this._objIDSysR30.ConvertTemplate400ToTemplate404(rgbyTemplate400, ref rgbyTemplate404);
        }

        public int CreateFile(string strPath, uint uiQuantityUsers, string strEmployerName, string strCNPJ_CPF, string strCEI, string strEmployerAddress, string strNFR)
        {
            try
            {
                int iReturn = this._objIDSysR30.CreateFile(strPath, uiQuantityUsers, strEmployerName, strCNPJ_CPF, strCEI, strEmployerAddress, strNFR);

                this._lstrEventData.Clear();
                this._lstrEventData.Add("CreateFile");
                this._lstrEventData.Add("");

                this._lstrEventData.Add("Arquivo.............: " + strPath);
                this._lstrEventData.Add("Quantidade Usuários.: " + uiQuantityUsers.ToString());
                this._lstrEventData.Add("Nome................: " + strEmployerName);
                this._lstrEventData.Add("CNPJ/CPF............: " + strCNPJ_CPF);
                this._lstrEventData.Add("CEI.................: " + strCEI);
                this._lstrEventData.Add("Endereço............: " + strEmployerAddress);
                this._lstrEventData.Add("NFR.................: " + strNFR);
                this._lstrEventData.Add("");
                this._lstrEventData.Add("Retorno.............: " + iReturn.ToString());

                return iReturn;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public int FileOpen(string strPath, ref uint uiQuantityUsers, ref string strEmployerName, ref string strCNPJ_CPF, ref string strCEI, ref string strEmployerAddress, ref string strNFR)
        {
            try
            {
                int iReturn = this._objIDSysR30.FileOpen(strPath, ref uiQuantityUsers, ref strEmployerName, ref strCNPJ_CPF, ref strCEI, ref strEmployerAddress, ref strNFR);

                this._lstrEventData.Clear();
                this._lstrEventData.Add("FileOpen");
                this._lstrEventData.Add("");

                this._lstrEventData.Add("Arquivo.............: " + strPath);
                this._lstrEventData.Add("Quantidade Usuários.: " + uiQuantityUsers.ToString());
                this._lstrEventData.Add("Nome................: " + strEmployerName);
                this._lstrEventData.Add("CNPJ/CPF............: " + strCNPJ_CPF);
                this._lstrEventData.Add("CEI.................: " + strCEI);
                this._lstrEventData.Add("Endereço............: " + strEmployerAddress);
                this._lstrEventData.Add("NFR.................: " + strNFR);
                this._lstrEventData.Add("");
                this._lstrEventData.Add("Retorno.............: " + iReturn.ToString());

                return iReturn;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public int FileClose()
        {
            try
            {
                int iReturn = this._objIDSysR30.FileClose();

                this._lstrEventData.Clear();
                this._lstrEventData.Add("FileClose");
                this._lstrEventData.Add("");
                this._lstrEventData.Add("Retorno.............: " + iReturn.ToString());

                return iReturn;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public void EncryptBuffer(ref byte[] rgbyBuffer, uint uiSize)
        {
            try
            {
                this._objIDSysR30.EncryptBuffer(ref rgbyBuffer, uiSize);
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public void DecryptBuffer(ref byte[] rgbyBuffer, uint uiSize)
        {
            try
            {
                this._objIDSysR30.DecryptBuffer(ref rgbyBuffer, uiSize);
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public void ExportUbsUser(string strPIS, string strUserName, uint uiKeyCode, string strBarCode, uint uiProxCode, byte byUserType, string strPassword, byte byAcessType, byte[] rgbyBiometrics, uint uiSizeSample)
        {
            try
            {
                this._objIDSysR30.ExportUbsUser(strPIS, strUserName, uiKeyCode, strBarCode, uiProxCode, byUserType, strPassword, byAcessType, rgbyBiometrics, uiSizeSample);

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Clear();
                this._lstrEventData.Add("ExportUbsUser");
                this._lstrEventData.Add("");

                strMask = "###.#####.##-#";
                this.FormatString(strPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS...............................: " + strFormatedValue);

                this._lstrEventData.Add("Nome do Trabalhador...............: " + strUserName);
                this._lstrEventData.Add("Código (KeyCode)..................: " + uiKeyCode.ToString());
                this._lstrEventData.Add("Barras (BarCode)..................: " + strBarCode);
                this._lstrEventData.Add("Proximidade (ProxCode)............: " + uiProxCode.ToString());
                this._lstrEventData.Add("Tipo (UserType)...................: " + byUserType.ToString());
                this._lstrEventData.Add("Senha (Password)..................: " + strPassword);
                this._lstrEventData.Add("Tamanho Amostras (SizeSample).....: " + uiSizeSample.ToString());
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public int ImportUbsUser(ref string strPIS, ref string strUserName, ref uint uiKeyCode, ref string strBarCode, ref uint uiProxCode, ref byte byUserType, ref string strPassword, ref byte byAcessType, ref byte[] rgbyBiometrics, ref uint uiSizeSample)
        {
            try
            {
                int iReturn = this._objIDSysR30.ImportUbsUser(ref strPIS, ref strUserName, ref uiKeyCode, ref strBarCode, ref uiProxCode, ref byUserType, ref strPassword, ref byAcessType, ref rgbyBiometrics, ref uiSizeSample);

                string strMask = "";
                string strFormatedValue = "";

                this._lstrEventData.Clear();
                this._lstrEventData.Add("ImportUbsUser");
                this._lstrEventData.Add("");

                strMask = "###.#####.##-#";
                this.FormatString(strPIS, strMask, ref strFormatedValue);
                this._lstrEventData.Add("PIS...............................: " + strFormatedValue);

                this._lstrEventData.Add("Nome do Trabalhador...............: " + strUserName);
                this._lstrEventData.Add("Código (KeyCode)..................: " + uiKeyCode.ToString());
                this._lstrEventData.Add("Barras (BarCode)..................: " + strBarCode);
                this._lstrEventData.Add("Proximidade (ProxCode)............: " + uiProxCode.ToString());
                this._lstrEventData.Add("Tipo (UserType)...................: " + byUserType.ToString());
                this._lstrEventData.Add("Senha (Password)..................: " + strPassword);
                this._lstrEventData.Add("Tamanho Amostras (SizeSample).....: " + uiSizeSample.ToString());
                this._lstrEventData.Add("");
                this._lstrEventData.Add("Retorno.............: " + iReturn.ToString());

                return iReturn;
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public int FileOpenAFD(string strPath)
        {
            try
            {
                return this._objIDSysR30.FileOpenAFD(strPath);
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public int ImportAFD()
        {
            try
            {
                return this._objIDSysR30.ImportAFD();
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        public bool UnloadDLL()
        {
            try
            {
                return this._objIDSysR30.Unload();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void TrataRetorno()
        {
            try
            {
                Thread.Sleep(1200);
                switch (GetConnectionState())
                {
                    case enuConnectionState.Disconnected:
                    {
                        throw new Exception("Desconectado");
                    }
                    case enuConnectionState.AttemptConnectionFail:
                    {
                        throw new Exception("Falha na tentativa de conexão");
                    }
                    case enuConnectionState.DataReceivedError:
                    {
                        var log = GetEventData();
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in log)
                        {
                            sb.AppendLine(item);
                        }
                        throw new Exception("Comando recebido com erro:\r\n" + sb.ToString());

                    }
                    case enuConnectionState.ConnectionError:
                    {
                        var log = GetEventData();
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in log)
                        {
                            sb.AppendLine(item);
                        }
                        throw new Exception("Erro de Conexão:\r\n" + sb.ToString());
                    }
                }
            }
            catch (Exception exError)
            {
                throw exError;
            }
        }

        #endregion
    }
}
