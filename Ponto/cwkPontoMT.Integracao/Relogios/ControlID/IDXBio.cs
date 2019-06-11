using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controlid;
using System.IO;
using cwkPontoMT.Integracao.Entidades;

namespace cwkPontoMT.Integracao.Relogios.ControlID
{
    public class IDXBio : Relogio
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            log.Debug("Iniciando Coleta de Registros, parametros: dataI = " + dataI + ", dataF = " + dataF + ", nsrInicio = " + nsrInicio + ", nsrFim = " + nsrFim);
            List<RegistroAFD> result = new List<RegistroAFD>();
            try
            {
                DateTime? dtInicio = dataI;
                DateTime? dtFinal = dataF;
                string header = "0000000001";
                log.Debug("Criando cabeçalho");
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
                log.Debug("Conectando ao equipamento");
                RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);
                bool sucessoComunicacao = false;
                if (nsrInicio == 0 && nsrFim == 0)
                {
                    log.Debug("Buscando desde o primeiro NSR");
                    sucessoComunicacao = rep.BuscarAFD(1);
                }
                else
                {
                    dtInicio = null;
                    dtFinal = null;
                    log.Debug("Buscando a partir do NSR " + nsrInicio);
                    sucessoComunicacao = rep.BuscarAFD(nsrInicio);
                }
                if (sucessoComunicacao)
                {
                    string linha;
                    log.Debug("Iniciando Leitura do AFD");
                    StringBuilder sb = new StringBuilder();
                    while (rep.LerAFD(out linha))
                    {
                        log.Debug("Linha = " + linha);
                        sb.Append(linha);
                    }
                    log.Debug("Desconecantado do equipamento");
                    rep.Desconectar();

                    string retorno = header + "\r\n" + sb.ToString();
                    IList<string> strings = retorno.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
                    List<RegistroAFD> regs = new List<RegistroAFD>();
                    log.Debug("Gerando registros para retorno");
                    foreach (string item in strings)
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            try
                            {
                                Util.IncluiRegistro(item, dtInicio, dtFinal, regs);
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }
                        }
                    }

                    return regs;
                }
                else
                {
                    return new List<RegistroAFD>();
                }
            }
            catch (Exception e)
            {
                log.Error("Erro ao buscar registros, erro: " + e.Message + ", stacktrace = " + e.StackTrace);
                throw e;
            }
        }
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            try
            {
                RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);
                bool gravou = false;
                bool sucessoComunicacao = false;
                sucessoComunicacao = rep.GravarConfigHVerao(inicio.Year, inicio.Month, inicio.Day, termino.Year, termino.Month, termino.Day, out gravou);
                erros = "";
                rep.Desconectar();
                return sucessoComunicacao && gravou;
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            try
            {
                RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);
                bool sucessoComunicacao = false;
                sucessoComunicacao = rep.GravarDataHora(horario);
                erros = "";
                rep.Desconectar();
                return sucessoComunicacao;
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            try
            {
                Dictionary<string, object> dadosRep = new Dictionary<string, object>();
                string errData = "";
                dadosRep = RecebeInformacoesRep(out errData);
                int x = dadosRep.Count();

                RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);

                bool sucessoEmpresa = false;
                bool gravouEmpresa = false;
                erros = "";
                if (Empregador != null)
                {
                    string documento = GetStringSomenteAlfanumerico(Empregador.Documento);
                    int tipodoc = Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? 1 : 2;
                    string cei = String.IsNullOrEmpty(Empregador.CEI) ? "0" : Empregador.CEI;
                    string razaosocial = Empregador.RazaoSocial;
                    string local = Empregador.Local;
                    sucessoEmpresa = rep.GravarEmpregador(documento, tipodoc, cei, razaosocial, local, out gravouEmpresa);
                    if (!(sucessoEmpresa && gravouEmpresa))
                    {
                        erros += "Erro ao atualizar os dados de Empregador\r\n";
                    }
                }

                Dictionary<string, string> errosEmpregados = new Dictionary<string, string>();
                foreach (var item in Empregados)
                {
                    bool sucessoEmpregado = false;
                    bool gravouEmpregado = false;
                    try
                    {
                        int RFIDint32 = Convert.ToInt32(item.RFID);

                        sucessoEmpregado = rep.GravarUsuario(Convert.ToInt64(item.Pis), item.Nome, Convert.ToInt32(item.DsCodigo), item.Senha, "", RFIDint32, 0, out gravouEmpregado);

                        if (!(sucessoEmpregado && gravouEmpregado))
                        {
                            if (!errosEmpregados.ContainsKey(item.Pis))
                            {
                                string lastLog,mensagem;
                                rep.GetLastLog(out lastLog);
                                var stringTratada = lastLog.Split(':').LastOrDefault();

                                if (!string.IsNullOrEmpty(stringTratada))
                                    mensagem = stringTratada;
                                else
                                    mensagem = "Erro ao comunicar/enviar para o Rep";
                                
                                errosEmpregados.Add(item.Pis, mensagem + "\r\n");
                            }
                            else
                                errosEmpregados[item.Pis] += "Erro ao comunicar/enviar para o Rep\r\n";
                        }
                    }
                    catch (Exception e)
                    {
                        string mensagem;
                        if (e.Message.Contains("Valor era muito grande ou muito pequeno para Int32"))
                            mensagem = "O número do cartão RFID não é compatível com a integração do equipamento(Integração aceita apenas número até 2.147.483.647)\r\n";
                        else
                            mensagem = e.Message;

                        if (!errosEmpregados.ContainsKey(item.Pis))
                            errosEmpregados.Add(item.Pis, mensagem + "\r\n");
                        else
                            errosEmpregados[item.Pis] += mensagem + "\r\n";
                    }
                }
                if (errosEmpregados.Count > 0)
                {
                    foreach (string key in errosEmpregados.Keys)
                    {
                        erros += ("Pis: " + key + " -- " + errosEmpregados[key]);
                    }
                }

                rep.Desconectar();
                if (Empregador != null)
                {
                    return (sucessoEmpresa && gravouEmpresa && String.IsNullOrEmpty(erros));
                }
                else
                {
                    return (String.IsNullOrEmpty(erros));
                }

            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            erros = "";
            try
            {
                RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);

                string numSerie = "";
                uint tamBobina = 0;
                uint restanteBobina = 0;
                uint uptime = 0;
                uint cortes = 0;
                uint papelAcumulado = 0;
                uint nsrAtual = 0;

                if (rep.LerInfo(out numSerie, out tamBobina, out restanteBobina,
                    out uptime, out cortes, out papelAcumulado, out nsrAtual))
                {
                    result.Add("Número de Série: ", numSerie);
                    result.Add("Tamanho da bobina: ", tamBobina);
                    result.Add("Tamanho restante da bobina: ", restanteBobina);
                    result.Add("Tempo de funcionamento do relógio: ", uptime);
                    result.Add("Quantidade de cortes (guilhotina): ", cortes);
                    result.Add("Metragem de papel impresso: ", papelAcumulado);
                    result.Add("NSR Atual: ", nsrAtual);
                }
                else
                {
                    result.Add("Não foi possível ler informações do relógio.", 0);
                }
                rep.Desconectar();
            }
            catch (Exception e)
            {
                result.Add("Não foi possível ler informações do relógio.", e.Message);
                erros += e.Message;
            }
            return result;
        }

        public override bool ExclusaoHabilitada()
        {
            return true;
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            IList<Entidades.Empregado> funcs = new List<Entidades.Empregado>();
            erros = "";
            foreach (var item in Empregados)
            {
                if (VerificaExistenciaFuncionarioRelogio(item, out erros))
                {
                    funcs.Add(item);
                }
                else
                {
                    erros += "Funcionario não cadastrado: " + item.Nome + ".";
                }
            }

            if (funcs.Count == 0)
            {
                erros = "Não há funcionarios cadastrados no relógio escolhido, dentre os funcionarios selecionados.";
                return false;
            }
            else
            {
                try
                {
                    string err = "";
                    RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);
                    foreach (var item in funcs)
                    {
                        bool gravou = false;
                        bool sucessoComunicacao = false;
                        sucessoComunicacao = rep.RemoverUsuario(Convert.ToInt64(item.Pis), out gravou);
                        if (!(gravou && sucessoComunicacao))
                        {
                            err += "Erro ao excluir o funcionario: " + item.Nome;
                        }
                    }
                    rep.Desconectar();
                    return String.IsNullOrEmpty(err);
                }
                catch (Exception e)
                {
                    erros += e.Message;
                    return false;
                }
            }
        }

        #region Métodos Auxiliares Control iD

        private RepCid Connect(string ip, int porta, uint passcode)
        {
            RepCid _rep = new RepCid();
            RepCid.ErrosRep result;
            result = _rep.Conectar(ip, porta, passcode);

            switch (result)
            {
                case RepCid.ErrosRep.ErroAutenticacao:
                    throw new Exception("Houve um erro ao comunicar com o REP. (" + "Erro de Autenticação" + ")");
                    break;
                case RepCid.ErrosRep.ErroConexao:
                    throw new Exception("Houve um erro ao comunicar com o REP. (" + "Erro de Conexão" + ")");
                    break;
                case RepCid.ErrosRep.ErroNaoOcioso:
                    throw new Exception("Houve um erro ao comunicar com o REP. (" + "Equipamento não está ocioso" + ")");
                    break;
                case RepCid.ErrosRep.ErroOutro:
                    throw new Exception("Houve um erro ao comunicar com o REP. (" + "Erro desconhecido (outros)" + ")");
                    break;
                case RepCid.ErrosRep.OK:
                    return _rep;
                    break;
                default:
                    throw new Exception("Houve um erro ao comunicar com o REP. (" + "Erro desconhecido" + ")");
            }
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

        #endregion

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            try
            {
                RepCid rep = Connect(IP, Convert.ToInt32(Porta), 0);

                string nome, senha, barras = "";
                int codigo, rfid, privilegios = 0;

                bool retorno = rep.LerDadosUsuario(
                    Convert.ToInt64(funcionario.Pis),
                    out nome, out codigo, out senha,
                    out barras, out rfid, out privilegios
                    );

                rep.Desconectar();
                erros = "";
                return retorno;
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            erros = String.Empty;
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

                if (Empregados != null)
                {

                    Stream user_s = new FileStream(d.FullName + "\\usuarios.dat", FileMode.Create);
                    BinaryWriter user_bw = new BinaryWriter(user_s);

                    IList<CidUsuario> lstUsuariosExportar = new List<CidUsuario>();
                    foreach (var item in Empregados)
                    {
                        CidUsuario emp = new CidUsuario();
                        emp.Barras = item.DsCodigo;
                        emp.Codigo = Convert.ToUInt32(item.DsCodigo);
                        emp.Nome = item.Nome;
                        emp.Digitais = null;
                        emp.PIS = Convert.ToUInt64(item.Pis);
                        emp.Priv = 0;
                        emp.Senha = item.Senha;
                        emp.RFID = Convert.ToUInt32(item.RFID);
                        lstUsuariosExportar.Add(emp);
                    }

                    RepCidUsbUserManagement.GravarDados(lstUsuariosExportar, user_bw, null);
                    user_bw.Close();

                }
                return true;
            }
            catch (Exception e)
            {
                erros = e.Message;
            }
            return false;
        }

        public override bool ExportacaoHabilitada()
        {
            return true;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, DirectoryInfo pasta)
        {
            erros = String.Empty;
            try
            {
                string nomeRelogio = "";
                Dictionary<string, string> retorno = new Dictionary<string, string>();
                DirectoryInfo d;

                if (Empregados != null)
                {
                    String nomeGuid = Guid.NewGuid() + ".dat";
                    Stream user_s = new FileStream(pasta.FullName + "\\" + nomeGuid, FileMode.Create);
                    BinaryWriter user_bw = new BinaryWriter(user_s);
                    UInt32 codigo;
                    UInt64 pis;
                    IList<CidUsuario> lstUsuariosExportar = new List<CidUsuario>();
                    foreach (var item in Empregados)
                    {
                        CidUsuario emp = new CidUsuario();
                        emp.Barras = item.DsCodigo;
                        UInt32.TryParse(item.DsCodigo, out codigo);
                        emp.Codigo = codigo;
                        emp.Nome = item.Nome;
                        emp.Digitais = null;
                        UInt64.TryParse(item.Pis, out pis);
                        emp.PIS = pis;
                        emp.Priv = 0;
                        emp.Senha = item.Senha;
                        lstUsuariosExportar.Add(emp);
                    }

                    RepCidUsbUserManagement.GravarDados(lstUsuariosExportar, user_bw, null);
                    user_bw.Close();
                    retorno.Add(nomeGuid, "usuarios");
                }
                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override bool TesteConexao()
        {
            throw new NotImplementedException();
        }

        public override int UltimoNSR()
        {
            throw new NotImplementedException();
        }

        public override List<Biometria> GetBiometria(out string erros)
        {
            throw new NotImplementedException();
        }
    }
}
