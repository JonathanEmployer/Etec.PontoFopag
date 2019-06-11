using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao
{
    public abstract class Relogio
    {
        public void SetDados(string ip, string porta, string senha, TipoComunicacao tipoComunicacao, string numeroRelogio, string local)
        {
            IP = ip;
            Porta = porta;
            Senha = senha;
            TipoComunicacao = tipoComunicacao;
            NumeroRelogio = numeroRelogio;
            Local = local;
        }
        
        public void SetDadosTopData(string ip, string porta, string senha, TipoComunicacao tipoComunicacao, string numeroRelogio, string local, string QntDigitos)
        {
            IP = ip;
            Porta = porta;
            Senha = senha;
            TipoComunicacao = tipoComunicacao;
            NumeroRelogio = numeroRelogio;
            Local = local;
            this.QntDigitos = QntDigitos;
        }

        public void SetEmpregados(List<Empregado> empregados)
        {
            Empregados = empregados;
        }
        public void SetEmpresa(Entidades.Empresa empregador)
        {
            Empregador = empregador;
        }

        public void SetNumeroSerie(string NumSerie)
        {
            NumeroSerie = NumSerie;
        }

        public void SetBiometrico(bool biometrico)
        {
            Biometrico = biometrico;
        }

        public void SetTimeZoneInfoRep(TimeZoneInfo timeZoneInfoRep)
        {
            this.timeZoneInfoRep = timeZoneInfoRep;
        }

        public void SetConn(string conn)
        {
            this.Conn = conn;
        }

        public void SetTipoBio(string bio)
        {
            this.TipoBiometria = bio;
        }

        public void SetCampoCracha(Int16 campoCracha)
        {
            this.CampoCracha = campoCracha;
        }

        public void SetLocalArquivo(string localArquivoTelematica)
        {
            this.LocalArquivo = localArquivoTelematica;
        }

        public string IP { get; set; }
        public string Porta { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string UsuarioREP { get; set; }
        public string SenhaUsuarioREP { get; set; }
        public TipoComunicacao TipoComunicacao { get; set; }
        public string NumeroRelogio { get; set; }
        public string NumeroSerie { get; set; }
        public bool Biometrico { get; set; }
        public string Local { get; set; }
        public List<Entidades.Empregado> Empregados { get; set; }
        public Entidades.Empresa Empregador { get; set; }
        public string QntDigitos { get; set; }
        public TimeZoneInfo timeZoneInfoRep = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        public DateTime dataComando = DateTime.Now;
        public string Conn { get; set; }
        public string TipoBiometria { get; set; }
        public Int16 CampoCracha { get; set; }

        public abstract List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF);
        public abstract List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente);
        public abstract bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros);
        public abstract bool ConfigurarHorarioRelogio(DateTime horario, out string erros);
        public abstract bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros);
        public abstract Dictionary<string, object> RecebeInformacoesRep(out string erros);
        public abstract bool RemoveFuncionariosRep(out string erros);
        public abstract bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros);
        public abstract bool ExportaEmpregadorFuncionarios(string caminho, out string erros);
        public abstract bool ExportacaoHabilitada();
        public abstract Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, DirectoryInfo pasta);
        public abstract bool ExclusaoHabilitada();
        /// <summary>
        /// Verifica se o sistema esta conseguindo conexao com o equipamento
        /// </summary>
        /// <returns>Retorna True quando equipamento "Respondendo" e False quando não conseguir conexão</returns>
        public abstract bool TesteConexao();
        /// <summary>
        /// Método que busca qual o último NSR do Rep.
        /// </summary>
        /// <returns>Último NSR do Rep</returns>
        public abstract int UltimoNSR();

        public string LocalArquivo { get; set; }


        public void GeraCabecalhoManual(DateTime dataI, DateTime dataF, List<RegistroAFD> retorno)
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
            header += Util.GetStringSomenteAlfanumerico(Empregador.Documento).PadLeft(14, '0');
            header += String.IsNullOrEmpty(Util.GetStringSomenteAlfanumerico(Empregador.CEI)) ? "            " : Util.GetStringSomenteAlfanumerico(Empregador.CEI).PadRight(12, ' ');
            header += Empregador.RazaoSocial.PadRight(150, ' ');
            header += NumeroSerie.PadLeft(17, '0');
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("HHmm");

            Util.IncluiRegistro(header, dataI, dataF, retorno);
        }
    }
}
