using cwkPontoMT.Integracao;
using Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public abstract class ComunicacaoRelogio
    {
        protected Relogio relogio;
        protected string tituloLog;
        protected DateTime horaComando;
        protected Modelo.Proxy.PxyConfigComunicadorServico config = new Modelo.Proxy.PxyConfigComunicadorServico();
        protected ModeloAux.RepViewModel rep = new ModeloAux.RepViewModel();

        protected ComunicacaoRelogio(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config)
            : this(rep, config, DateTime.Now)
        {
        }

        protected ComunicacaoRelogio(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config, DateTime horaComando)
        {
            this.config = config;
            this.rep = rep;
            if (rep.EquipamentoHomologadoInmetro)
            {
                if (String.IsNullOrEmpty(rep.CpfUsuarioRep) || String.IsNullOrEmpty(rep.LoginRep) || String.IsNullOrEmpty(rep.SenhaRep))
                {
                    throw new Exception("Para comunicação com REPs Homologados pelo INMETRO é obrigatório informar o Login, Senha e CPF para comunicação com o Equipamento. "
                        + "Verifique o cadastro deste usuário (" + config.Usuario + ") e realize o preenchimento dos campos \"Login REP\", \"Senha REP\" e \"CPF\".");
                }
            }
            this.horaComando = horaComando;
        }

        public bool Enviar()
        {
            try
            {
                SetRelogio();
                SetDadosEnvio();
                EfetuarEnvio();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Receber(ComunicacaoApi comApi)
        {
            try
            {
                SetRelogio();
                SetDadosReceber();
                EfetuarRecebimento(comApi);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<string, string> ExportarWeb(ref string caminho, DirectoryInfo pasta)
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
            relogio = RelogioFactory.GetRelogio((TipoRelogio)rep.NumModeloRelogio);

            if (rep.TipoIP == 1)
            {
                if (string.IsNullOrEmpty(rep.Ip))
                {
                    throw new Exception("Não foi informado o DNS, verifique as configuraçãos no cadastro do Rep.");
                }
                IPHostEntry hostEntry;
                hostEntry = Dns.GetHostEntry(rep.Ip);

                if (hostEntry.AddressList.Length > 0)
                {
                    rep.Ip = hostEntry.AddressList[0].ToString();
                }
            }

            switch (rep.NumModeloRelogio)
            {
                case (short)TipoRelogio.InnerRep:
                    relogio.SetDadosTopData(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio,
                        rep.EnderecoEmpregador, rep.QtdDigitosCartao.ToString());
                    break;
                case (short)TipoRelogio.InnerRepBarras2i:
                    relogio.SetDadosTopData(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio,
                        rep.EnderecoEmpregador, rep.QtdDigitosCartao.ToString());
                    break;
                case (short)TipoRelogio.Orion6:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.RepTrilobit:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.Henry:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.ControlID:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.ZPM_R130:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.REP_BI_01:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.Kurumin_REP_II_Max:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.DIXI_IDNOX:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.Dimep_PrintPoint:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.Henry_Hexa:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);

                    Int64 senhaConvertida = 0;
                    if (!Int64.TryParse(CriptoString.Decrypt(rep.SenhaRep), out senhaConvertida))
                    {
                        throw new Exception("A senha de comunicação deve ser composta somente por números para o equipamento Henry HEXA C.");
                    }
                    break;
                case (short)TipoRelogio.MDREPMiniPrint:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.MDREPPrintPoint:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                case (short)TipoRelogio.InnerRepPlus:
                    {
                        relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                            (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                        relogio.SetBiometrico(rep.UtilizaBiometria);
                    }
                    break;
                case (short)TipoRelogio.MDREPPrintPointIII:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    relogio.SetConn(config.ConexaoServCom);
                    break;
                case (short)TipoRelogio.Telematica:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    relogio.SetTipoBio(rep.TipoBiometria);
                    relogio.SetLocalArquivo(config.LocalArquivoTelematica);
                    relogio.SetConn(config.ConexaoTelematica);
                    break;
                case (short)TipoRelogio.IDClass:
                    relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao,
                        (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);
                    break;
                default:
                    break;
            }
            relogio.UsuarioREP = CriptoString.Decrypt(rep.LoginRep);
            relogio.SenhaUsuarioREP = CriptoString.Decrypt(rep.SenhaRep);
            relogio.Cpf = CriptoString.Decrypt(rep.CpfUsuarioRep);
            relogio.dataComando = horaComando;
            relogio.SetNumeroSerie(rep.NumSerie);
            relogio.CampoCracha = rep.CampoCracha;
            if (string.IsNullOrEmpty(rep.IdTimeZoneInfo))
            {
                relogio.timeZoneInfoRep = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            else
            {
                relogio.timeZoneInfoRep = TimeZoneInfo.FindSystemTimeZoneById(rep.IdTimeZoneInfo);
            }
            relogio.TipoBiometria = rep.TipoBiometria;
        }

        protected abstract void SetDadosEnvio();

        protected abstract void SetDadosReceber();

        protected abstract void EfetuarEnvio();

        protected abstract void EfetuarRecebimento(ComunicacaoApi comApi);

        protected abstract Dictionary<string, string> EfetuarEnvio(ref string caminho, DirectoryInfo pasta);

    }
}
