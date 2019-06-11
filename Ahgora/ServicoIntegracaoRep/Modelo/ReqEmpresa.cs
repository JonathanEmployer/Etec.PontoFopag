using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    public class CfgEmpresa
    {
        public string cmd = "cfg_empresa";
        public Empresa Empresa { get; set; }
    }

    public class ReqEmpresa : ReqResp
    {
        public string NumeroFabricacao { get; set; }
        public string ControleAcesso { get; set; }
        public string CtrlPatrimonio { get; set; }
        public int NSR { get; set; }
        public Empresa Empresa { get; set; }
        public string Portaria1510 { get; set; }
    }

    public class ServidorAplicacao
    {
        public string Host { get; set; }
        public string Porta { get; set; }
        public string Senha { get; set; }
        public string ComCriptografia { get; set; }
        public string ProxyHost { get; set; }
        public string ProxyPorta { get; set; }
        public string ProxyUsuario { get; set; }
        public string ProxySenha { get; set; }
    }

    public class REP
    {
        public string Mascara { get; set; }
        public string Gateway { get; set; }
        public string DNS { get; set; }
        public string DHCP { get; set; }
        public string IP { get; set; }
        public string IP_atual { get; set; }
        public string Proxy { get; set; }
    }

    public class Empresa
    {
        public string HorarioVeraoAutomatico { get; set; }
        public int TamanhoCodigo { get; set; }
        public ServidorAplicacao ServidorAplicacao { get; set; }
        public string minMinuCnt { get; set; }
        public int TamanhoSenha { get; set; }
        public string Identificador { get; set; }
        public string StringBarCode { get; set; }
        public REP REP { get; set; }
        public string BioThreshold { get; set; }
        public string RazaoSocial { get; set; }
        public string MenuFuncPIS { get; set; }
        public string HorarioVerao { get; set; }
        public string Local { get; set; }
        public string CEI { get; set; }
        public string SenhaMenu { get; set; }
        public string DataFimHV { get; set; }
        public string DataInicioHV { get; set; }
        public string StringRFID { get; set; }
        public int template_min_minutia_count { get; set; }
        public string UsbDadosCSV { get; set; }
        public string CNPJouCPF { get; set; }
        public string Tipo { get; set; }
        public string OtimizacaoCabecalho { get; set; }
        public string ValidacaoComBio { get; set; }
        public string TotalCutting { get; set; }
        public string ComUsbDados { get; set; }
    }
}
