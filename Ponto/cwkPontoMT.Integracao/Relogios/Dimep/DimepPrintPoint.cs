using org.cesar.dmplight.watchComm.api;
using org.cesar.dmplight.watchComm.business;
using org.cesar.dmplight.watchComm.impl;
using org.cesar.dmplight.watchComm.impl.printpoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Dimep
{
    public class DimepPrintPoint : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            List<RegistroAFD> result = new List<RegistroAFD>();
            try
            {
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

                Util.IncluiRegistro(header, dataI, dataF, result);
                InstanciaWatchComm();
                this._watchComm.OpenConnection();
                MRPRecord[] recordArray = new MRPRecord[] { };
                try
                {
                    if (nsrInicio > 0)
                    {
                        this._watchComm.RepositioningMRPRecordsPointer(nsrInicio.ToString());
                        
                    }
                    else
                    {
                        this._watchComm.RepositioningMRPRecordsPointer(dataI);
                    }
                    recordArray = this._watchComm.InquiryMRPRecords(false, false, true, false);
                }
                catch (Exception e)
                {
                    this._watchComm.RepositioningMRPRecordsPointer("1");
                    recordArray = this._watchComm.InquiryMRPRecords(false, false, true, false);
                }

                int count = 0;
                bool forcarPorData = false;
                while (recordArray != null)
                {
                    foreach (MRPRecord record in recordArray)
                    {
                        if (record is MRPRecord_RegistrationMarkingPoint)
                        {
                            string reg = string.Empty;
                            string nsrStr = ((MRPRecord_RegistrationMarkingPoint)record).NSR.Substring(1);
                            //Se o primeiro NSR não for o solicitado ou o proximo, força por data
                            if (count == 0 && nsrInicio > 0 && (Convert.ToInt32(nsrStr) -1 > nsrInicio))
                            {
                                forcarPorData = true;
                                break;
                            }
                            reg += nsrStr;
                            reg += "3";
                            reg += ((MRPRecord_RegistrationMarkingPoint)record).DateTimeMarkingPoint.ToString("ddMMyyyyHHmm");
                            reg += ((MRPRecord_RegistrationMarkingPoint)record).Pis;
                            Util.IncluiRegistro(reg, dataI, dataF, result);
                            count++;
                        }
                    }
                    if (forcarPorData)
                    {
                        break;
                    }
                    recordArray = this._watchComm.ConfirmationReceiptMRPRecords();
                }
                _watchComm.CloseConnection();
                if (forcarPorData)
                {
                    nsrInicio = 0;
                    return GetAFDNsr(dataI, dataF, nsrInicio, nsrFim, ordemDecrescente);
                }
            }
            catch (Exception exception)
            {
                _watchComm.CloseConnection();
                // Apenas deixo dar excessão caso não tenha conseguido coletar nada, se conseguiu coletar algo mando parcialmente para o server.
                if (result == null || result.Count == 0)
                {
                    throw new Exception(exception.Message, exception);
                }
            }

            return result;
        } 

        protected WatchComm _watchComm;
        protected TcpClient _watchConnection;
        protected WatchListener _watchListener;
        protected WatchListener _watchListener2;

        protected string CaminhoEXE()
        {
            return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        }

        protected virtual void InstanciaWatchComm()
        {
            string accessKey = "";
            TCPComm comm = new TCPComm(IP, 0xbb8);
            comm.SetTimeOut(0x3a98);
            _watchComm = new WatchComm(WatchProtocolType.PrintPoint, comm, 1, accessKey, WatchConnectionType.ConnectedMode, "01.00.0000");
        }

        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF,0,0,false);
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            InstanciaWatchComm();
            erros = String.Empty;
            try
            {
                this._watchComm.AddParcialConfiguration(ParcialConfigurationType.DST, inicio, termino);
                _watchComm.OpenConnection();
                _watchComm.SendParcialSettings();
                _watchComm.CloseConnection();
            }
            catch (Exception e)
            {
                erros += e.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            InstanciaWatchComm();
            erros = String.Empty;
            try
            {
                _watchComm.OpenConnection();
                _watchComm.SetDateTime(horario);
                _watchComm.CloseConnection();
            }
            catch (Exception e)
            {
                erros += e.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            erros = string.Empty;
            InstanciaWatchComm();
            try
            {
                if (Empregados != null)
                {
                    if (Empregados.Count > 0)
                    {
                        foreach (var item in this.Empregados)
                        {
                            try
                            {
                                _watchComm.AddEmployee(item.Pis, item.Nome, item.Senha);
                            }
                            catch (Exception exception)
                            {
                                erros += exception.Message;
                                return false;
                            }
                        }
                        try
                        {
                            _watchComm.OpenConnection();
                            _watchComm.IncludeEmployeesList(true, false);
                            _watchComm.CloseConnection();
                        }
                        catch (Exception exception2)
                        {
                            erros += exception2.Message;
                        }

                        foreach (var item in this.Empregados)
                        {
                            try
                            {
                                _watchComm.AddCredential(item.DsCodigo, item.Pis, Convert.ToByte(0));
                            }
                            catch (Exception exception)
                            {
                                erros += exception.Message;
                                return false;
                            }
                        }
                        try
                        {
                            _watchComm.OpenConnection();
                            _watchComm.IncludeCredentialList(false);
                            _watchComm.CloseConnection();
                        }
                        catch (Exception exception2)
                        {
                            erros += exception2.Message;
                        }
                    }
                }

                if (Empregador != null)
                {
                    try
                    {
                        _watchComm.OpenConnection();
                        _watchComm.ChangeEmployer(
                            Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? EmployeerType.CNPJ : EmployeerType.CPF,
                            GetStringSomenteAlfanumerico(Empregador.Documento), GetStringSomenteAlfanumerico(Empregador.CEI), Empregador.RazaoSocial, Empregador.Local);
                        _watchComm.CloseConnection();
                    }
                    catch (Exception ex)
                    {
                        erros += ex.Message;
                    } 
                }
            }
            catch (Exception ex)
            {
                erros += ex.Message;
            }
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
            this.InstanciaWatchComm();
            try
            {
                if (Empregados != null)
                {
                    if (Empregados.Count > 0)
                    {
                        foreach (var item in this.Empregados)
                        {
                            try
                            {
                                _watchComm.AddCredential(item.DsCodigo, item.Pis, Convert.ToByte(0));
                            }
                            catch (Exception exception)
                            {
                                erros += exception.Message;
                                return false;
                            }
                        }
                        try
                        {
                            _watchComm.OpenConnection();
                            _watchComm.ExcludeCredentialList();
                            _watchComm.CloseConnection();
                        }
                        catch (Exception exception2)
                        {
                            erros += exception2.Message;
                        }
                        foreach (var item in this.Empregados)
                        {
                            try
                            {
                                _watchComm.AddEmployee(item.Pis, item.Nome, item.Senha);
                            }
                            catch (Exception exception)
                            {
                                erros += exception.Message;
                                return false;
                            }
                        }
                        try
                        {
                            _watchComm.OpenConnection();
                            _watchComm.ExcludeEmployeesList();
                            _watchComm.CloseConnection();
                        }
                        catch (Exception exception2)
                        {
                            erros += exception2.Message;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                erros += e.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            throw new NotImplementedException();
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
                                                              char.IsWhiteSpace(c) ||
                                                              c == '+' ||
                                                              c == ',' ||
                                                              c == ']'))
                                                              .ToArray());
            return r;
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
