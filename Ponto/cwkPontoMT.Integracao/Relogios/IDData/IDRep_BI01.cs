using cwkPontoMT.Integracao.Auxiliares.IDData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.IDData
{
    public class IDRep_BI01 : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            List<RegistroAFD> result = new List<RegistroAFD>();
            CIDSysR30 rep = new CIDSysR30();
            try
            {
                rep = new CIDSysR30();
                var conexao = new REPConnection(IP, Convert.ToInt32(Porta));
                try
                {
                    conexao.Reconnect();
                }
                catch (Exception)
                {
                    conexao.Connect();
                }
                List<string> res = new List<string>();
                if (nsrInicio == 0 && nsrFim == 0)
                {
                    res = ColetarEventos(ref rep, ref conexao, dataI);
                }
                else
                {
                    res = ColetarEventosPorNsr(ref rep, ref conexao, nsrInicio, nsrFim);
                    
                }
                foreach (string item in res)
                {
                    Util.IncluiRegistroSemData(item, result);
                }
                result = result.OrderBy(o => o.Nsr).ToList();
                conexao.Disconnect();
                rep.Unload();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
            
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            erros = String.Empty;
            CController rep = new CController();
            try
            {
                if (rep.ConnectServer(IP, Convert.ToInt32(Porta)))
                {
                    rep.LoadDLL();

                    rep.SetDST(inicio, termino);
                    rep.TrataRetorno();
                    rep.DisconnectServer();
                    rep.UnloadDLL();
                }

            }
            catch (Exception ex)
            {
                erros += ex.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            erros = String.Empty;
            CController rep = new CController();
            try
            {
                if (rep.ConnectServer(IP, Convert.ToInt32(Porta)))
                {
                    rep.LoadDLL();

                    byte byDay = (byte)horario.Day;
                    byte byMonth = (byte)horario.Month;
                    ushort usYear = (ushort)horario.Year;
                    byte byHour = (byte)horario.Hour;
                    byte byMinute = (byte)horario.Minute;
                    byte bySecond = (byte)horario.Second;

                    rep.SetDateTime(byDay, byMonth, usYear, byHour, byMinute, bySecond);
                    rep.TrataRetorno();
                    rep.DisconnectServer();
                    rep.UnloadDLL();
                }

            }
            catch (Exception ex)
            {
                erros += ex.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            erros = String.Empty;
            StringBuilder sb = new StringBuilder();
            CController rep = new CController();
            try
            {
                if (rep.ConnectServer(IP, Convert.ToInt32(Porta)))
                {
                    rep.LoadDLL();

                    if (Empregador != null)
                    {
                        byte byIdentifyType = Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? Convert.ToByte(1) : Convert.ToByte(0);
                        string strCNPJ_CPF = GetStringSomenteAlfanumerico(Empregador.Documento);
                        ulong ulCEI = Convert.ToUInt64(String.IsNullOrEmpty(Empregador.CEI) ? "0" : GetStringSomenteAlfanumerico(Empregador.CEI));
                        string strEmployerName = Empregador.RazaoSocial;
                        string strEmployerAddress = Empregador.Local;
                        try
                        {
                            rep.SetEmployer(byIdentifyType, strCNPJ_CPF, ulCEI, strEmployerName, strEmployerAddress);
                            rep.TrataRetorno();
                        }
                        catch (Exception exError)
                        {
                            sb.AppendLine(exError.Message);
                        }
                    }
                    if (Empregados != null)
                    {
                        foreach (var item in Empregados)
                        {
                            string strPIS = item.Pis;
                            string strUserName = item.Nome;
                            uint uiKeyCode = Convert.ToUInt32(item.DsCodigo);
                            string strBarCode = item.DsCodigo;
                            byte byFacilityCode = 0;
                            uint uiProxCode = uiKeyCode;
                            byte byUserType = 0;
                            string strPassword = item.Senha;
                            System.IO.MemoryStream msPhoto = null;
                            ushort usSizeSample = 0;
                            byte byQuantitySamples = 0;
                            byte[] rgbyBiometrics = new byte[1];
                            byte byAccessType = 0;

                            try
                            {
                                rep.AddUser(strPIS, strUserName, uiKeyCode, strBarCode, byFacilityCode, uiProxCode, byUserType, byAccessType, strPassword, msPhoto, usSizeSample, byQuantitySamples, rgbyBiometrics);
                                rep.TrataRetorno();
                            }
                            catch (Exception exError)
                            {
                                if (exError.Message.ToLower().Contains("pis já cadastrado"))
                                {
                                    try
                                    {
                                        rep.SetConnectionState(enuConnectionState.Connected);
                                        rep.ChangeUserData(strPIS, strPIS, strUserName, uiKeyCode, strBarCode, byFacilityCode,
                                            Convert.ToUInt16(uiProxCode), byUserType, strPassword, msPhoto, usSizeSample, byQuantitySamples, rgbyBiometrics);
                                        rep.TrataRetorno();
                                    }
                                    catch (Exception e)
                                    {
                                        sb.AppendLine(e.Message);
                                    }
                                }
                                else
                                {
                                    sb.AppendLine(exError.Message);
                                }
                                
                            }
                        }
                    }
                    rep.DisconnectServer();
                    rep.UnloadDLL();
                }

            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.Message);
            }
            erros = sb.ToString();
            return String.IsNullOrEmpty(erros);
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
            erros = String.Empty;
            CController rep = new CController();
            try
            {
                if (rep.ConnectServer(IP, Convert.ToInt32(Porta)))
                {
                    rep.LoadDLL();

                    if (Empregados != null)
                    {
                        foreach (var item in Empregados)
                        {
                            try
                            {
                                rep.DeleteUser(item.Pis);
                                rep.TrataRetorno();
                            }
                            catch (Exception exError)
                            {
                                erros += exError.Message;
                            }
                        }
                    }
                    rep.UnloadDLL();
                    rep.DisconnectServer();
                }

            }
            catch (Exception ex)
            {
                erros += ex.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            CController rep = new CController();
            erros = String.Empty;
            return String.IsNullOrEmpty(erros);
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            CController rep = new CController();
            erros = String.Empty;
            return String.IsNullOrEmpty(erros);
        }

        public override bool ExportacaoHabilitada()
        {
            return false;
        }

        private string GetStringSomenteAlfanumerico(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            string r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                              char.IsWhiteSpace(c)))
                                                              .ToArray());
            return r;
        }

        private List<string> ColetarEventos(ref CIDSysR30 rep, ref REPConnection conexao, DateTime dataLimite)
        {
            uint totalNSR = RequestTotalNSR(ref rep, ref conexao);
            List<string> res = new List<string>();
            string header = "0000000001";

            if (Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ)
            {
                header += "1";
            }
            else
            {
                header += "2";
            }
            header += GetStringSomenteAlfanumerico(Empregador.Documento).PadLeft(14, '0');
            header += String.IsNullOrEmpty(GetStringSomenteAlfanumerico(Empregador.CEI)) ? "            " : GetStringSomenteAlfanumerico(Empregador.CEI).PadRight(12, ' ');
            header += Empregador.RazaoSocial.PadRight(150, ' ');
            header += NumeroSerie.PadLeft(17, '0');
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("HHmm");

            res.Add(header);
            while (totalNSR > 0)
            {
                string registro = RequestEventByNSRDataLimite(ref totalNSR, ref rep, ref conexao, dataLimite);
                if (!String.IsNullOrEmpty(registro))
                {
                    res.Add(registro);
                }
                if (registro == null)
                {
                    break;
                }
                totalNSR--;
            }
            return res;
        }

        private List<string> ColetarEventosPorNsr(ref CIDSysR30 rep, ref REPConnection conexao, int nsrInicial, int nsrFinal)
        {
            uint totalNSR = RequestTotalNSR(ref rep, ref conexao);
            List<string> res = new List<string>();
            string header = "0000000001";

            if (Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ)
            {
                header += "1";
            }
            else
            {
                header += "2";
            }
            header += GetStringSomenteAlfanumerico(Empregador.Documento).PadLeft(14, '0');
            header += String.IsNullOrEmpty(GetStringSomenteAlfanumerico(Empregador.CEI)) ? "            " : GetStringSomenteAlfanumerico(Empregador.CEI).PadRight(12, ' ');
            header += Empregador.RazaoSocial.PadRight(150, ' ');
            header += NumeroSerie.PadLeft(17, '0');
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("HHmm");

            res.Add(header);
            while (totalNSR >= nsrInicial)
            {
                string registro = RequestEventByNSR(ref totalNSR, ref rep, ref conexao);
                if (!String.IsNullOrEmpty(registro))
                {
                    res.Add(registro);
                }
                if (registro == null)
                {
                    break;
                }
                totalNSR--;
            }
            return res;
        }

        /// <summary>
        /// requisita a quantidade total de eventos disponíveis 
        /// </summary>
        /// <returns>quantidade total de eventos</returns>
        private uint RequestTotalNSR(ref CIDSysR30 rep, ref REPConnection conexao)
        {
            byte[] buffer = rep.RequestTotalNSR();
            byte[] responseBuffer = conexao.ProcessMessage(buffer);
            int availResult = rep.PacketAvail(responseBuffer);

            //se 0 sucesso, senão erro
            uint totalNSR = 0;
            if (availResult == 0)
            {
                //obtem quntidade total de eventos
                rep.GetTotalNSR(ref totalNSR);
            }

            return totalNSR;
        }

        /// <summary>
        /// requisita o evento correspondente a um NSR específico
        /// </summary>
        /// <param name="nsr">NSR a ser coletado</param>
        private string RequestEventByNSRDataLimite(ref uint nsr, ref CIDSysR30 rep, ref REPConnection conexao, DateTime dataLimite)
        {
            byte[] buffer = rep.RequestEventByNSR(nsr);
            byte[] responseBuffer = conexao.ProcessMessage(buffer);
            int availResult = rep.PacketAvail(responseBuffer);
            string text = "";
            switch (availResult)
            {
                case 3:
                {
                    uint regNSR = 0;
                    byte type = 0;
                    byte day = 0;
                    byte month = 0;
                    ushort year = 0;
                    byte hour = 0;
                    byte min = 0;
                    string pis = "";
                    

                    //obtém dados do evento tipo 3
                    rep.GetLogType3(ref regNSR, ref type, ref day,
                        ref month, ref year, ref hour, ref min, ref pis);

                    DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day), Convert.ToInt32(hour), Convert.ToInt32(min), 0);
                    if (dt < dataLimite)
                    {
                        text = null;
                    }
                    else
                    {
                        text += regNSR.ToString().PadLeft(9, '0');
                        text += type.ToString();
                        text += day.ToString().PadLeft(2, '0');
                        text += month.ToString().PadLeft(2, '0');
                        text += year.ToString();
                        text += hour.ToString().PadLeft(2, '0');
                        text += min.ToString().PadLeft(2, '0');
                        text += pis.PadLeft(12, '0');
                    }
                    break;
                }
                default:
                {
                    //se availResult < 0, houve erro no processamento do comando
                    if (availResult < 0)
                    {
                        //mostra mensagem de erro
                        throw new Exception(GetErrorMessage(availResult));
                    }
                    break;
                }
            }
            return text;
        }

        private string RequestEventByNSR(ref uint nsr, ref CIDSysR30 rep, ref REPConnection conexao)
        {
            byte[] buffer = rep.RequestEventByNSR(nsr);
            byte[] responseBuffer = conexao.ProcessMessage(buffer);
            int availResult = rep.PacketAvail(responseBuffer);
            string text = "";
            switch (availResult)
            {
                case 3:
                    {
                        uint regNSR = 0;
                        byte type = 0;
                        byte day = 0;
                        byte month = 0;
                        ushort year = 0;
                        byte hour = 0;
                        byte min = 0;
                        string pis = "";


                        //obtém dados do evento tipo 3
                        rep.GetLogType3(ref regNSR, ref type, ref day,
                            ref month, ref year, ref hour, ref min, ref pis);

                        text += regNSR.ToString().PadLeft(9, '0');
                        text += type.ToString();
                        text += day.ToString().PadLeft(2, '0');
                        text += month.ToString().PadLeft(2, '0');
                        text += year.ToString();
                        text += hour.ToString().PadLeft(2, '0');
                        text += min.ToString().PadLeft(2, '0');
                        text += pis.PadLeft(12, '0');
                        break;
                    }
                default:
                    {
                        //se availResult < 0, houve erro no processamento do comando
                        if (availResult < 0)
                        {
                            //mostra mensagem de erro
                            throw new Exception(GetErrorMessage(availResult));
                        }
                        break;
                    }
            }
            return text;
        }

        private string GetErrorMessage(int availResult)
        {
            string errorMessage = "";

            if (availResult < 0)
            {
                return errorMessage;
            }

            switch (availResult)
            {
                case -1:
                    {
                        errorMessage = "Cartão de Proximidade já cadastrado para outro usuário.";
                        break;
                    }
                case -2:
                    {
                        errorMessage = "Cartão de Código de Barras já cadastrado para outro usuário.";
                        break;
                    }
                case -3:
                    {
                        errorMessage = "PIS já cadastrado para outro usuário.";
                        break;
                    }
                case -4:
                    {
                        errorMessage = "Código individual já cadastrado para outro usuário.";
                        break;
                    }
                case -5:
                    {
                        errorMessage = "Erro na memória MRP.";
                        break;
                    }
                case -6:
                    {
                        errorMessage = "Erro na memória MT.";
                        break;
                    }
                case -7:
                    {
                        errorMessage = "Erro na memória RAM.";
                        break;
                    }
                case -8:
                    {
                        errorMessage = "Dados enviados inválidos.";
                        break;
                    }
                case -9:
                    {
                        errorMessage = "ID REP não possui trabalhadores cadastrados.";
                        break;
                    }
                case -10:
                    {
                        errorMessage = "Trabalhador não cadastrado.";
                        break;
                    }
                case -11:
                    {
                        errorMessage = "ID REP não possui o cadastro do empregador.";
                        break;
                    }
                case -12:
                    {
                        errorMessage = "Dados do empregador inválidos: CPF / CNPJ.";
                        break;
                    }
                case -13:
                    {
                        errorMessage = "Dados do empregador inválidos: Nome / Razão Social.";
                        break;
                    }
                case -14:
                    {
                        errorMessage = "Dados do empregador inválidos: Endereço.";
                        break;
                    }
                case -15:
                    {
                        errorMessage = "Data e/ou hora inválida(s).";
                        break;
                    }
                case -16:
                    {
                        errorMessage = "Erro no módulo biométrico: ERROR.";
                        break;
                    }
                case -17:
                    {
                        errorMessage = "Erro no módulo biométrico: TIMEOUT.";
                        break;
                    }
                case -18:
                    {
                        errorMessage = "Dados de comunicação inválidos: Endereço IP.";
                        break;
                    }
                case -19:
                    {
                        errorMessage = "Dados de comunicação inválidos: Máscara de sub-rede.";
                        break;
                    }
                case -20:
                    {
                        errorMessage = "Dados de comunicação inválidos: IP Gateway.";
                        break;
                    }
                case -21:
                    {
                        errorMessage = "Não existem eventos.";
                        break;
                    }
                case -22:
                    {
                        errorMessage = "Erro no módulo biométrico: CHEIO.";
                        break;
                    }
                case -23:
                    {
                        errorMessage = "Erro na leitura do módulo biométrico: ERROR.";
                        break;
                    }
                case -24:
                    {
                        errorMessage = "Erro na leitura do módulo biométrico: TIMEOUT.";
                        break;
                    }
                case -25:
                    {
                        errorMessage = "Erro de checksum da área de dados.";
                        break;
                    }
                case -26:
                    {
                        errorMessage = "Dados do empregador inválidos: CEI.";
                        break;
                    }
                case -27:
                    {
                        errorMessage = "Equipamento bloqueado.";
                        break;
                    }
                case -100:
                    {
                        errorMessage = "Erro no checksum do cabeçalho do pacote (Verificação na DLL).";
                        break;
                    }
                case -101:
                    {
                        errorMessage = "Erro no checksum dos dados do pacote (Verificação na DLL).";
                        break;
                    }
                case -102:
                    {
                        errorMessage = "Comando inválido (Verificação na DLL).";
                        break;
                    }
                case -103:
                    {
                        errorMessage = "Erro pacote inválido (Verificação na DLL).";
                        break;
                    }
                case -104:
                    {
                        errorMessage = "Erro no tamanho do pacote: pacote vazio (Verificação na DLL).";
                        break;
                    }
                case -105:
                    {
                        errorMessage = "Erro no tamanho dos dados (Verificação na DLL).";
                        break;
                    }
                default:
                    {
                        errorMessage = "Erro desconhecido.";
                        break;
                    }
            }

            return errorMessage;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string nomeRelogio, out string erros, DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }



        public override bool TesteConexao()
        {
            throw new NotImplementedException();
        }

        public override int UltimoNSR()
        {
            throw new NotImplementedException();
        }
    }
}
