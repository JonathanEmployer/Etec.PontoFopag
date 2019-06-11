using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using cwkPontoMT.Integracao;

namespace BLL.IntegracaoRelogio
{
    public abstract class ComunicacaoRelogio
    {
        public readonly string ARQUIVO_LOG = Modelo.cwkGlobal.DirApp + "\\LogComunicacaoRelogio.txt";
        protected Modelo.REP objRep;
        protected Relogio relogio;
        protected string tituloLog;
        protected string ConnectionString;
        protected BLL.REP bllRep;
        protected Modelo.Cw_Usuario UsuarioLogado;
        protected DateTime horaComando;
        
        protected ComunicacaoRelogio(int pIdRep, string connString, Modelo.Cw_Usuario usuarioLogado)
        : this (pIdRep, connString, usuarioLogado, DateTime.Now)
        { 
        }

        protected ComunicacaoRelogio(int pIdRep, string connString, Modelo.Cw_Usuario usuarioLogado, DateTime horaComando)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
            bllRep = new BLL.REP(ConnectionString, usuarioLogado);
            objRep = bllRep.LoadObject(pIdRep);

            if (objRep.EquipamentoHomologado.EquipamentoHomologadoInmetro)
            {
                if (String.IsNullOrEmpty(UsuarioLogado.Cpf) || String.IsNullOrEmpty(UsuarioLogado.LoginRep) || String.IsNullOrEmpty(UsuarioLogado.SenhaRep))
                {
                    if ((String.IsNullOrEmpty(objRep.CpfRep) || String.IsNullOrEmpty(objRep.LoginRep) || String.IsNullOrEmpty(objRep.SenhaRep)))
                    {
                        throw new Exception("Para comunicação com REPs Homologados pelo INMETRO é obrigatório informar o Login, Senha e CPF para comunicação com o Equipamento. "
                                        + "Verifique o cadastro deste usuário (" + UsuarioLogado.Nome + ") e realize o preenchimento dos campos \"Login REP\", \"Senha REP\" e \"CPF\"."); 
                    }
                }
            }
            this.horaComando = horaComando;
        }

        public bool Enviar(out bool pExibirLog)
        {
            return Enviar(out pExibirLog, "");
        }

        public bool Enviar(out bool pExibirLog, string conn)
        {
            try
            {
                SetRelogio();
                relogio.Conn = conn;
                SetDadosEnvio();
                EfetuarEnvio();
                pExibirLog = false;
                return true;
            }
            catch (Exception ex)
            {
                GravarLogComunicacaoRelogio(ex.Message, tituloLog);
                pExibirLog = true;
                return false;
            }
        }

        public Dictionary<string, string> ExportarWeb(ref string caminho, DirectoryInfo pasta)
        {
            try
            {
                SetRelogio();
                SetDadosEnvio();
                Dictionary<string, string> retorno = EfetuarEnvio(ref caminho, pasta);
                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void SetRelogio()
        {
            relogio = RelogioFactory.GetRelogio((TipoRelogio)objRep.Relogio);

            switch (objRep.Relogio)
            {
                case (short)TipoRelogio.InnerRep:
                    relogio.SetDadosTopData(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio,
                        objRep.Local, objRep.QtdDigitos.ToString());
                    break;
                case (short)TipoRelogio.InnerRepBarras2i:
                    relogio.SetDadosTopData(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio,
                        objRep.Local, objRep.QtdDigitos.ToString());
                    break;
                case (short)TipoRelogio.Orion6:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.RepTrilobit:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.Henry:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.ControlID:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.ZPM_R130:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (short)TipoRelogio.REP_BI_01:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (short)TipoRelogio.Kurumin_REP_II_Max:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (short)TipoRelogio.DIXI_IDNOX:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.Dimep_PrintPoint:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.Henry_Hexa:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);

                    Int64 senhaConvertida = 0;
                    if (!Int64.TryParse(UsuarioLogado.SenhaRep, out senhaConvertida))
                    {
                        throw new Exception("A senha de comunicação deve ser composta somente por números para o equipamento Henry HEXA C.");
                    }
                    break;
                case (short)TipoRelogio.MDREPMiniPrint:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.MDREPPrintPoint:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                case (short)TipoRelogio.InnerRepPlus:
                    {
                        relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                            (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                        relogio.SetNumeroSerie(objRep.NumSerie);
                    }
                    break;
                case (short)TipoRelogio.MDREPPrintPointIII:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                        relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (short)TipoRelogio.Telematica:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (short)TipoRelogio.IDClass:
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(UsuarioLogado.Cpf) && !string.IsNullOrEmpty(UsuarioLogado.LoginRep))
            {
                relogio.UsuarioREP = UsuarioLogado.LoginRep;
                relogio.SenhaUsuarioREP = UsuarioLogado.SenhaRep;
                relogio.Cpf = UsuarioLogado.Cpf;
            }
            else
            {
                relogio.UsuarioREP = objRep.LoginRep;
                relogio.SenhaUsuarioREP = objRep.SenhaRep;
                relogio.Cpf = objRep.CpfRep;
            }
            
            relogio.dataComando = horaComando;
            if (string.IsNullOrEmpty(objRep.IdTimeZoneInfo))
            {
                relogio.timeZoneInfoRep = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            else
            {
                relogio.timeZoneInfoRep = TimeZoneInfo.FindSystemTimeZoneById(objRep.IdTimeZoneInfo);
            }

        }

        protected void GravarLogComunicacaoRelogio(string erros, string descricaoEnvio)
        {
            StreamWriter file = new StreamWriter(ARQUIVO_LOG, true);
            file.WriteLine("------------------------------------------------------");
            file.WriteLine("Log de " + descricaoEnvio + " - " + DateTime.Now.ToShortTimeString() + " - " + DateTime.Now.ToShortDateString());
            file.WriteLine(String.Empty);
            file.WriteLine(erros);
            file.Close();
        }

        protected abstract void SetDadosEnvio();

        protected abstract void EfetuarEnvio();

        protected abstract Dictionary<string, string> EfetuarEnvio(ref string caminho, DirectoryInfo pasta);

    }
}
