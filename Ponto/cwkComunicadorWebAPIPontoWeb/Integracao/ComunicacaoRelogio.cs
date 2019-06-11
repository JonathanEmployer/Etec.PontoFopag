using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using cwkPontoMT.Integracao;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using System.Reflection;

namespace cwkComunicadorWebAPIPontoWeb.Integracao
{
    public abstract class ComunicacaoRelogio
    {
        protected RepViewModel objRep;
        protected Relogio relogio;
        protected string tituloLog;

        protected ComunicacaoRelogio(RepViewModel rep)
        {
            objRep = rep;

            if (rep.EquipamentoHomologadoInmetro)
            {
                if (String.IsNullOrEmpty(rep.CpfRepDec) || String.IsNullOrEmpty(rep.LoginRepDec) || String.IsNullOrEmpty(rep.SenhaRepDec))
                {
                    throw new Exception("Para comunicação com REPs Homologados pelo INMETRO é obrigatório informar o Login, Senha e CPF para comunicação com o Equipamento. "
                        + "Verifique o cadastro do seu usuário no sistema Pontofopag e realize o preenchimento dos campos \"Login REP\", \"Senha REP\" e \"CPF\".");
                }
            }
            SetRelogio();
        }

        public bool Enviar(out bool pExibirLog, out string log)
        {
            try
            {
                SetRelogio();
                SetDadosEnvio();
                EfetuarEnvio();
                pExibirLog = false;
                log = String.Empty;
                return true;
            }
            catch (Exception ex)
            {
                GravarLogComunicacaoRelogio(ex.Message, tituloLog);
                pExibirLog = true;
                log = ex.Message;
                return false;
            }
        }

        protected void SetRelogio()
        {
            relogio = RelogioFactory.GetRelogio((TipoRelogio)objRep.NumModeloRelogio);

            switch (objRep.NumModeloRelogio)
            {
                case (int)TipoRelogio.InnerRep:
                    relogio.SetDadosTopData(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao, 
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, 
                        objRep.EnderecoEmpregador, objRep.QtdDigitosCartao.ToString());
                    break;
                case (int)TipoRelogio.InnerRepBarras2i:
                    relogio.SetDadosTopData(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio,
                        objRep.EnderecoEmpregador, objRep.QtdDigitosCartao.ToString());
                    break;
                case (int)TipoRelogio.Orion6:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.RepTrilobit:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.Henry:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.ControlID:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.ZPM_R130:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (int)TipoRelogio.REP_BI_01:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (int)TipoRelogio.Kurumin_REP_II_Max:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    relogio.SetNumeroSerie(objRep.NumSerie);
                    break;
                case (int)TipoRelogio.DIXI_IDNOX:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.Dimep_PrintPoint:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.Henry_Hexa:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);

                    Int64 senhaConvertida = 0;
                    if (!Int64.TryParse(objRep.SenhaRepDec, out senhaConvertida))
                    {
                        throw new Exception("A senha de comunicação deve ser composta somente por números para o equipamento Henry HEXA C.");
                    }
                    break;
                case (int)TipoRelogio.InnerRepPlus:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                case (int)TipoRelogio.Telematica:
                    relogio.SetDados(objRep.Ip, objRep.Porta, objRep.SenhaComunicacao,
                        (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.EnderecoEmpregador);
                    break;
                default:
                    break;
            }
            relogio.UsuarioREP = objRep.LoginRepDec;
            relogio.SenhaUsuarioREP = objRep.SenhaRepDec;
            relogio.Cpf = objRep.CpfRepDec;
        }

        protected void GravarLogComunicacaoRelogio(string erros, string descricaoEnvio)
        {
            string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!Directory.Exists(Path.Combine(dirApp, "Logs")))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(dirApp, "Logs"));
                }
                catch (Exception e)
                {
                    return;
                }
            }
            dirApp = Path.Combine(dirApp, "Logs");
            string filePath = "Log" + descricaoEnvio.Replace(' ', '_') + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt";
            StreamWriter file = new StreamWriter(filePath, true);
            file.WriteLine("------------------------------------------------------");
            file.WriteLine("Log de " + descricaoEnvio + " - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString());
            file.WriteLine(String.Empty);
            file.WriteLine(erros);
            file.Flush();
            file.Close();
            file.Dispose();
        }

        protected abstract void SetDadosEnvio();

        protected abstract void EfetuarEnvio();

    }
}
