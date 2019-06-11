using cwkPontoMT.Integracao.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios.Henry
{
    public class PrismaSuperFacil : Relogio
    {
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            DateTime? dtInicio = dataI;
            DateTime? dtFinal = dataF;
            string header = "0000000001";
            List<RegistroAFD> regs = new List<RegistroAFD>();
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

            string retorno = String.Empty;
            if (nsrInicio <= 0)
            {
                retorno = RecebeMarcacoes(dataI, dataF);
            }
            else
            {
                dtInicio = null;
                dtFinal = null;
                if (nsrFim == int.MaxValue)
                {
                    try
                    {
                        int nsrFimRep = UltimoNSR();
                        nsrFim = nsrFimRep;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                if (nsrInicio >= nsrFim)
                {
                    return regs;
                }
                retorno = RecebeMarcacoesNsr(nsrInicio, nsrFim);
            }

            retorno = header + "\r\n" + retorno;
            IList<string> strings = retorno.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();

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
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
        }

        public override List<Biometria> GetBiometria(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            try
            {
                DateTime dtRelogio = RecebeHoraRelogio();
                string res = EnviaDtHora_IniFimHorarioVerao(dtRelogio, inicio, termino);
                string info = "";
                string oper = "";
                erros = trataRetorno(res, out info, out oper);

                IList<string> err = erros.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (err.Count > 0)
                {
                    foreach (var item in err)
                    {
                        erros += item + "\r\n";
                    }
                }
                else
                {
                    erros = "";
                }

                return String.IsNullOrEmpty(erros);
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
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
            IList<Entidades.Empregado> funcs = new List<Entidades.Empregado>();
            erros = "";
            try
            {
                foreach (var item in Empregados)
                {
                    string info = "";
                    string oper = "";
                    erros += "\r\n";
                    try
                    {
                        string res = EnviaFuncionario(Operacao.Exclusao, item.Pis, item.Nome, true, item.DsCodigo, String.Empty);
                        erros += trataRetorno(res, out info, out oper);
                        if (info.Contains("22"))
                        {
                            erros += item.Nome + " - Usuário não cadastrado no REP." + Environment.NewLine;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                IList<string> err = erros.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (err.Count > 0)
                {
                    erros = "";
                    foreach (var item in err)
                    {
                        erros += item + "\r\n";
                    }
                }
                else
                {
                    erros = "";
                }

                return String.IsNullOrEmpty(erros);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }
            //}
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            try
            {
                DateTime? inicioHorarioVerao = null;
                DateTime? fimHorarioVerao = null;

                RecebeIniFimHorarioVerao(out inicioHorarioVerao, out fimHorarioVerao);

                string res = EnviaDtHora_IniFimHorarioVerao(horario, inicioHorarioVerao, fimHorarioVerao);
                string info = "";
                string oper = "";
                erros = trataRetorno(res, out info, out oper);

                IList<string> err = erros.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (err.Count > 0)
                {
                    foreach (var item in err)
                    {
                        erros += item + "\r\n";
                    }
                }
                else
                {
                    erros = "";
                }

                return String.IsNullOrEmpty(erros);
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


                string info = "";
                string oper = "";
                erros = "";

                if (Empregador != null)
                {
                    try
                    {
                        string resEmpresa = EnviaEmpresa(Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? TipoEmpregador.Jurídica : TipoEmpregador.Física,
                                                                     Empregador.Documento, Empregador.CEI, Empregador.RazaoSocial, Empregador.Local);

                        erros += trataRetorno(resEmpresa, out info, out oper);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }



                foreach (var item in Empregados)
                {
                    info = "";
                    oper = "";
                    erros += "\r\n";
                    try
                    {
                        string res = EnviaFuncionario(Operacao.Inclusao, item.Pis, item.Nome, biometrico, item.DsCodigo, String.Empty);
                        erros += trataRetorno(res, out info, out oper);



                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }

                Empregados.GroupBy(x => x.DsCodigo).ToList().ForEach(x =>
                {
                    var empregado = Empregados.Where(y => y.DsCodigo == x.Key).ToList();
                    EnviaFuncionarioBiometria(x.Key, empregado.Count(), empregado.Select(b => Encoding.UTF8.GetString(b.valorBiometria)).ToList());
                });

                IList<string> err = erros.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (err.Count > 0)
                {
                    foreach (var item in err)
                    {
                        erros += item + "\r\n";
                    }
                }
                else
                {
                    erros = "";
                }

                return String.IsNullOrEmpty(erros);
            }
            catch (Exception e)
            {
                erros = "";
                erros += e.Message;
                throw e;
            }
        }

        #region Métodos Auxiliares Henry

        private DateTime RecebeHoraRelogio()
        {
            try
            {
                string dados = "01+RH+00";
                string res = EnviaDadosRep(dados);
                string info = "";
                string oper = "";
                string erros = trataRetorno(res, out info, out oper);
                List<string> dthoraTotal = info.Split(' ').ToList();
                List<string> data = dthoraTotal[0].Split('/').ToList();
                List<string> hora = dthoraTotal[1].Split(':').ToList();

                DateTime d = new DateTime(2000 + Convert.ToInt32(data[2]), Convert.ToInt32(data[1]), Convert.ToInt32(data[0]),
                    Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), Convert.ToInt32(hora[2]));

                return d;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void RecebeIniFimHorarioVerao(out DateTime? inicio, out DateTime? fim)
        {
            string dados = "01+RH+00";
            string res = EnviaDadosRep(dados);
            string info = "";
            string oper = "";
            string resultado = trataRetorno(res, out info, out oper);
            List<string> dthoraTotal = resultado.Split(' ').ToList();
            List<string> dataInicio = dthoraTotal[0].Split('/').ToList();
            List<string> dataFinal = dthoraTotal[1].Split('/').ToList();

            try
            {
                inicio = new DateTime(2000 + Convert.ToInt32(dataInicio[2]), Convert.ToInt32(dataInicio[1]), Convert.ToInt32(dataInicio[0]));
                fim = new DateTime(2000 + Convert.ToInt32(dataFinal[2]), Convert.ToInt32(dataFinal[1]), Convert.ToInt32(dataFinal[0]));
            }
            catch (Exception)
            {
                inicio = null;
                fim = null;
            }

        }

        private string EnviaDadosRep(string dados)
        {
            try
            {
                string retorno = "";
                using (TcpClient tcpClient = new TcpClient())
                {
                    char[] data;

                    string accentedStr = dados;
                    byte[] tempBytes;
                    tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
                    string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);


                    data = asciiStr.ToCharArray();
                    String str = "";
                    str = textFormat(data);//formatando texto (cabeçalho, checksum e Byte final)
                    byte[] ba = Encoding.UTF8.GetBytes(str);


                    tcpClient.Connect(IP, Convert.ToInt32(Porta));
                    tcpClient.Client.Send(ba);
                    byte[] bb = new byte[96000];
                    int k = tcpClient.Client.Receive(bb);
                    retorno = Encoding.Default.GetString(bb.Take(k).ToArray());
                }
                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string EnviaDtHora_IniFimHorarioVerao(DateTime DataHoraAtual, DateTime? InicioHorarioVerao, DateTime? FimHorarioVerao)
        {
            if (InicioHorarioVerao.HasValue ^ FimHorarioVerao.HasValue)
            {
                throw new Exception("Obrigatório informar Inicio e Fim do horário de Verão.");
            }
            string iniHorVerao = InicioHorarioVerao.HasValue ? InicioHorarioVerao.Value.ToString("dd/MM/yy") : "00/00/00";
            string fimHorVerao = FimHorarioVerao.HasValue ? FimHorarioVerao.Value.ToString("dd/MM/yy") : "00/00/00";
            string result = "01+EH+00+" + DataHoraAtual.ToString("dd/MM/yy HH:mm:ss") + "]" + iniHorVerao + "]" + fimHorVerao;

            try
            {
                return EnviaDadosRep(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string EnviaEmpresa(TipoEmpregador tipoEmp, string CnpjCpf, string Cei, string RazaoSocial, string Local)
        {
            string result = "01+EE+00+" + (int)tipoEmp + "]" + GetStringSomenteAlfanumerico(CnpjCpf) + "]" + Cei + "]" + RazaoSocial + "]" + Local;

            try
            {
                return EnviaDadosRep(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string EnviaFuncionario(Operacao op, string pis, string nome, bool usaBiometria, string referencia1, string referencia2)
        {
            string oper = "";
            switch (op)
            {
                case Operacao.Inclusao:
                    oper = "I";
                    break;
                case Operacao.Alteracao:
                    oper = "A";
                    break;
                case Operacao.Exclusao:
                    oper = "E";
                    break;
                default:
                    break;
            }

            int qtdRefs = 0;
            string referencia = "";
            if (String.IsNullOrEmpty(referencia1) && String.IsNullOrEmpty(referencia2))
            {
                throw new Exception("pelo menos uma referência é obrigatória.");
            }
            if (!String.IsNullOrEmpty(referencia1) && !String.IsNullOrEmpty(referencia2))
            {
                qtdRefs = 2;
                referencia = referencia1 + "}" + referencia2;
            }
            else if (!String.IsNullOrEmpty(referencia1) ^ !String.IsNullOrEmpty(referencia2))
            {
                qtdRefs = 1;
                referencia = String.IsNullOrEmpty(referencia1) ? referencia2 : referencia1;
            }
            string result = "01+EU+00+1+" + oper + "[" + pis + "[" + nome + "[" + Convert.ToInt16(usaBiometria).ToString() + "[" + qtdRefs + "[" + referencia + "]";

            try
            {
                return EnviaDadosRep(result);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        private string EnviaFuncionarioBiometria(string Referencia, int QtdeBio, List<string> ValorBiometria)
        {
            string result = "01+ED+00+D]" + Referencia + "}" + QtdeBio + "}";
            for (int i = 0; i < QtdeBio; i++)
            {
                result += i + "{" + ValorBiometria[i];
            }

            try
            {
                return EnviaDadosRep(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private List<string> RecebeBiometria(int CodFuncionario)
        {
            var resultado = new List<string>();
            string info = "";
            string oper = "";

            string dadosEnvio = "01+RD+00+D]" + CodFuncionario;

            try
            {
                var result = trataRetorno(EnviaDadosRep(dadosEnvio), out info, out oper);
                resultado = result.Substring(result.IndexOf('{') + 1).Split('{').ToList();
            }
            catch (Exception)
            {
            }

            return resultado;
        }
        private string RecebeMarcacoes(DateTime? inicio, DateTime? fim)
        {
            Int32 nsrInicial = 0;   // pegar registros de alteração/envio de empresa que são grandes
            Int32 nsrFinal = 0;
            string resultado = "";
            string info = "";
            string oper = "";

            if (inicio.HasValue && fim.HasValue)
            {
                string dadosInicio = "01+RR+00+D]1]" + inicio.GetValueOrDefault().Date.AddSeconds(1).ToString("dd/MM/yyyy HH:mm:ss") + "]";
                string dadosFinal = "01+RR+00+D]1]" + fim.GetValueOrDefault().Date.AddDays(1).AddSeconds(1).ToString("dd/MM/yyyy HH:mm:ss") + "]";

                try
                {
                    nsrInicial = Convert.ToInt32(trataRetorno(EnviaDadosRep(dadosInicio), out info, out oper).Substring(0, 9));
                }
                catch (Exception)
                {
                    //caso de problemas de parâmetros incorretos pega todos os bilhetes do REP 
                    if (nsrInicial == 0)
                    {
                        nsrInicial = 1;
                    }
                }
                try
                {
                    nsrFinal = Convert.ToInt32(trataRetorno(EnviaDadosRep(dadosFinal), out info, out oper).Substring(0, 9));
                }
                catch (Exception)
                {
                    if (nsrFinal == 0)
                    {
                        // geralmente o problema acontece no nsrFinal, quando se quer pegar bilhetes do mesmo dia (hoje).
                        // Deste modo temos o nsrInicial OK, mas não temos nsrFinal. Logo, setamos o nsrFinal como o MaxValue
                        // da Int32, à fim de que tenhamos todos os bilhetes possíveis, partindo do nsrInicial
                        nsrFinal = Int32.MaxValue;
                    }
                }
            }
            else
            {
                nsrInicial = 1;
                nsrFinal = Int32.MaxValue;
            }
            resultado = RecebeMarcacoesNsr(nsrInicial, nsrFinal);
            return resultado;
        }

        private string RecebeMarcacoesNsr(int nsrInicio, int nsrFim)
        {
            int step = 3;           // Step fixo em 3 para não dar problemas de buffer ao 
            Int32 nsrInicial = 0;   // pegar registros de alteração/envio de empresa que são grandes
            Int32 nsrFinal = 0;
            string resultado = "";
            string info = "";
            string oper = "";
            string dados = "";

            try
            {
                nsrInicial = nsrInicio > 0 ? nsrInicio : 1;

            }
            catch (Exception)
            {
                //caso de problemas de parâmetros incorretos pega todos os bilhetes do REP 
                if (nsrInicial == 0)
                {
                    nsrInicial = 1;
                }
            }
            try
            {
                if (nsrFim <= 0 || nsrFim == int.MaxValue)
                { nsrFinal = UltimoNSR(); }
                else
                { nsrFinal = nsrFim; }
            }
            catch (Exception)
            {
                if (nsrFinal == 0)
                {
                    // geralmente o problema acontece no nsrFinal, quando se quer pegar bilhetes do mesmo dia (hoje).
                    // Deste modo temos o nsrInicial OK, mas não temos nsrFinal. Logo, setamos o nsrFinal como o MaxValue
                    // da Int32, à fim de que tenhamos todos os bilhetes possíveis, partindo do nsrInicial
                    nsrFinal = Int32.MaxValue;
                }
            }
            do
            {
                try
                {
                    // Diferença entre o Ultimo NSR a ser coletado e o Ultimo Coletado (nsrInicial - 1)
                    int difNsr = (nsrFinal - (nsrInicial - 1));
                    if (difNsr < 3)
                    {
                        // Se a diferença entre o inicial e o Final for menor que o step, seta o step com a diferença, pra não trazer mais NSR que o solicitado ou que existe no equipamento (para evitar problemas ne memória no equipamento quando pede mais nsr que existe.)
                        step = difNsr;
                    }
                    try
                    {
                        EnviaSolicitacaoNSR(step, nsrInicial, ref resultado, ref info, ref oper, ref dados);
                    }
                    catch (Exception ex)
                    {
                        // Se o Rep apresentar erro de SD espera um segundo e tenta novamente.
                        if (ex.Message.Contains("SD Card"))
                        {
                            Thread.Sleep(1000);
                            EnviaSolicitacaoNSR(step, nsrInicial, ref resultado, ref info, ref oper, ref dados);
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                    // Adiciona o quantidade coletada ao NSR Inicial para colocar o NSR Inicial no Próximo NSR a ser Coletado
                    nsrInicial += step;
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrEmpty(resultado))
                    {
                        throw ex;
                    }
                    break;
                }
                //continua enquando estiver retornando dados (info) e o nsrInicial - 1 (ultimo nsr coletado) for menor que o NsrFinal solicitado
            } while (Convert.ToInt32(info) >= step && nsrFinal > (nsrInicial - 1));
            return resultado;
        }

        private void EnviaSolicitacaoNSR(int step, Int32 nsrInicial, ref string resultado, ref string info, ref string oper, ref string dados)
        {
            dados = "01+RR+00+N]" + step + "]" + nsrInicial.ToString();
            string res = EnviaDadosRep(dados);
            resultado += trataRetorno(res, out info, out oper);
        }

        public enum TipoEmpregador
        {
            Jurídica = 1,
            Física = 2
        }

        public enum Operacao
        {
            Inclusao,
            Alteracao,
            Exclusao
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

        private string textFormat(char[] data)
        {
            //tradução do código do exemplo Java do manual da Henry
            string aux = "", aux2 = "", str = "";
            char BYTE_INIT, BYTE_END, BYTE_CKSUM;
            char[] BYTE_TAM = new char[2] { '0', '0' };
            BYTE_INIT = (char)Int16.Parse("2");//conf. bit inicial
            BYTE_END = (char)Int16.Parse("3");//conf. bit final
            BYTE_TAM[0] = (char)data.Length;//conf. tamanho dos dados
            BYTE_TAM[1] = (char)Int16.Parse("0");
            aux2 += BYTE_INIT; //Inserindo byte inicial
            aux2 += BYTE_TAM[0]; //Inserindo byte do tamanho
            aux2 += BYTE_TAM[1];
            foreach (char chr in data)
            {
                str += chr;
            }
            aux = aux2 + str; // concatenando com a informação
            BYTE_CKSUM = aux[1];//Calculo do Checksum
            for (int a = 2; a < aux.Length; a++)
            {
                BYTE_CKSUM = (char)(BYTE_CKSUM ^ aux[a]);
            }
            aux += BYTE_CKSUM; //Inserindo Checksum
            aux += BYTE_END; //Inserindo byte Final
            return aux;
        }

        private string trataRetorno(string entrada, out string info, out string operacao)
        {
            Dictionary<int, string> erros = PopularErros();

            string temp = entrada.Substring(3);
            temp = temp.Substring(0, temp.Length - 2);
            //temp = GetStringSomenteAlfanumerico(temp);
            if (!temp.EndsWith("\r\n"))
            {
                temp += "\r\n";
            }
            IList<string> ret = temp.Split(new char[] { ']' }).ToList();
            string header = ret[0];
            IList<string> ret2 = header.Split(new char[] { '+' }).ToList();

            info = ret2.Count > 3 ? ret2[3] : "0";
            operacao = ret2[1];

            if (ret2.Count >= 3)
            {
                try
                {
                    int codigoRetorno = Convert.ToInt32(ret2[2]);
                    if (codigoRetorno > 0 && !(operacao == "RR" && codigoRetorno == 103))
                    {
                        throw new Exception("Erro: " + erros[codigoRetorno] + ". Código: " + codigoRetorno);
                    }
                    if (operacao == "RH")
                    {
                        return ret[1] + " " + ret[2];
                    }
                    if (operacao == "EU" && ret2.Count == 5)
                    {
                        info = ret2[4];
                    }
                    if (ret.Count > 1)
                    {
                        return ret[1];
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return "";
        }

        private Dictionary<int, string> PopularErros()
        {
            Dictionary<int, string> erros = new Dictionary<int, string>();

            erros.Add(1, "Não há dados");
            erros.Add(2, "Reservado");
            erros.Add(3, "Reservado");
            erros.Add(4, "Reservado");
            erros.Add(5, "Reservado");
            erros.Add(6, "Reservado");
            erros.Add(7, "Reservado");
            erros.Add(8, "Reservado");
            erros.Add(9, "Reservado");
            erros.Add(10, "Comando desconhecido");
            erros.Add(11, "Tamanho do pacote é inválido");
            erros.Add(12, "Parâmetros informados são inválidos");
            erros.Add(13, "Erro de checksum");
            erros.Add(14, "Tamanho dos parâmetros são inválidos");
            erros.Add(15, "Número da mensagem é inválido");
            erros.Add(16, "Start Byte é inválido");
            erros.Add(17, "Erro para receber pacote");
            erros.Add(18, "Reservado");
            erros.Add(19, "Reservado");
            erros.Add(20, "Não há empregador cadastrado");
            erros.Add(21, "Não há usuários cadastrados");
            erros.Add(22, "Usuário não cadastrado");
            erros.Add(23, "Usuário já cadastrado");
            erros.Add(24, "Limite de cadastro de usuários atingido");
            erros.Add(25, "Equipamento não possui biometria");
            erros.Add(26, "Index biométrico não encontrado");
            erros.Add(27, "Limite de cadastro de digitais atingido");
            erros.Add(28, "Equipamento não possui eventos");
            erros.Add(29, "Erro na manipulação de biometrias");
            erros.Add(30, "Documento do empregador é inválido");
            erros.Add(31, "Tipo do documento do empregador é inválido");
            erros.Add(32, "Ip é inválido");
            erros.Add(33, "Tipo de operação do usuário é inválida");
            erros.Add(34, "PIS do empregado é inválido");
            erros.Add(35, "Cei do empregador é inválido");
            erros.Add(36, "Referencia do empregado é inválido");
            erros.Add(37, "Nome do empregado é inválido");
            erros.Add(38, "Reservado");
            erros.Add(39, "Reservado");
            erros.Add(40, "MRP está cheia");
            erros.Add(41, "Erro ao gravar dados na MRP");
            erros.Add(42, "Erro ao ler dados da MRP");
            erros.Add(43, "Erro ao gravar dados na MT");
            erros.Add(44, "Erro ao ler dados da MT");
            erros.Add(45, "Reservado");
            erros.Add(46, "Reservado");
            erros.Add(47, "Reservado");
            erros.Add(48, "Reservado");
            erros.Add(49, "Reservado");
            erros.Add(50, "Erro desconhecido");
            erros.Add(51, "Reservado");
            erros.Add(52, "Reservado");
            erros.Add(53, "Reservado");
            erros.Add(54, "Reservado");
            erros.Add(55, "Reservado");
            erros.Add(56, "Reservado");
            erros.Add(57, "Reservado");
            erros.Add(58, "Reservado");
            erros.Add(59, "Reservado");
            erros.Add(60, "Reservado");
            erros.Add(61, "Matrícula já existe");
            erros.Add(62, "PIS já existe");
            erros.Add(63, "Opção inválida");
            erros.Add(64, "Matrícula não existe");
            erros.Add(65, "PIS não existe");
            erros.Add(66, "Reservado");
            erros.Add(67, "Reservado");
            erros.Add(68, "Reservado");
            erros.Add(69, "Reservado");
            erros.Add(70, "Reservado");
            erros.Add(71, "Reservado");
            erros.Add(72, "Reservado");
            erros.Add(73, "Reservado");
            erros.Add(74, "Reservado");
            erros.Add(75, "Reservado");
            erros.Add(76, "Reservado");
            erros.Add(77, "Reservado");
            erros.Add(78, "Reservado");
            erros.Add(79, "Reservado");
            erros.Add(80, "Reservado");
            erros.Add(81, "Reservado");
            erros.Add(82, "Reservado");
            erros.Add(83, "Reservado");
            erros.Add(84, "Reservado");
            erros.Add(85, "Reservado");
            erros.Add(86, "Reservado");
            erros.Add(87, "Reservado");
            erros.Add(88, "Reservado");
            erros.Add(89, "Reservado");
            erros.Add(90, "Reservado");
            erros.Add(91, "Reservado");
            erros.Add(92, "Reservado");
            erros.Add(93, "Reservado");
            erros.Add(94, "Reservado");
            erros.Add(95, "Reservado");
            erros.Add(96, "Reservado");
            erros.Add(97, "Reservado");
            erros.Add(98, "Reservado");
            erros.Add(99, "Reservado");
            erros.Add(100, "Comando desconhecido");
            erros.Add(101, "Erro de checksum");
            erros.Add(102, "Start Byte é inválido");
            erros.Add(103, "Parâmetros informados são inválidos");
            erros.Add(104, "Tamanho dos parâmetros é inválido");
            erros.Add(105, "Comando informado é inválido");
            erros.Add(106, "Data hora do comando é inválida");
            erros.Add(107, "Evento é inválido");
            erros.Add(108, "Tamanho do evento é inválido");
            erros.Add(109, "Código de inserção é inválido");
            erros.Add(110, "Parâmetros excedem o limite");
            erros.Add(111, "NSR inválido");
            erros.Add(112, "Erro ao inicializar o SD Card");
            erros.Add(113, "Erro ao detectar o SD Card");
            erros.Add(114, "Erro ao montar SD Card");
            erros.Add(115, "Não há espaço na MRP");
            erros.Add(116, "Erro ao obter informações sobre o SD Card");
            erros.Add(117, "Erro ao abrir arquivo MRP");
            erros.Add(118, "Erro ao gravar dados na MRP");
            erros.Add(119, "Erro ao ler dados da MRP");
            erros.Add(120, "Erro ao setar posição");
            erros.Add(121, "Erro ao sincronizar");

            return erros;
        }

        #endregion

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new Exception("Função não disponível para este modelo de relógio.");
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, DirectoryInfo pasta)
        {
            erros = String.Empty;
            Dictionary<string, string> retorno = new Dictionary<string, string>();
            try
            {
                if (Empregador != null)
                {
                    string nomeGuid = Guid.NewGuid() + ".txt";
                    string expEmpregador = ExportaEmpregador(Empregador);
                    File.WriteAllText(Path.Combine(pasta.FullName, nomeGuid), expEmpregador);
                    retorno.Add(nomeGuid, "Empregador");
                }

                if (Empregados != null)
                {
                    string nomeGuid = Guid.NewGuid() + ".txt";
                    string expEmpregados = ExportaEmpregados(Empregados);
                    File.WriteAllText(Path.Combine(pasta.FullName, nomeGuid), expEmpregados);
                    retorno.Add(nomeGuid, "Empregados");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return retorno;
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

                if (Empregador != null)
                {
                    string expEmpregador = ExportaEmpregador(Empregador);
                    File.WriteAllText(Path.Combine(d.FullName, "rep_empregador.txt"), expEmpregador);
                }

                if (Empregados != null)
                {
                    string expEmpregados = ExportaEmpregados(Empregados);
                    File.WriteAllText(Path.Combine(d.FullName, "rep_colaborador.txt"), expEmpregados);
                }
                return true;
            }
            catch (Exception e)
            {
                erros = e.Message;
            }
            return false;
        }

        private string ExportaEmpregados(List<Entidades.Empregado> empregados)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in empregados)
            {
                sb.AppendLine("1+1+I[0" + item.Pis + "[" + item.Nome + "[" + Convert.ToInt16(item.Biometria).ToString() + "[1[" + item.DsCodigo);
            }
            return sb.ToString();
        }

        private string ExportaEmpregador(Entidades.Empresa empregador)
        {
            return "2+" + (int)empregador.TipoDocumento + "]" + GetStringSomenteAlfanumerico(empregador.Documento) + "]"
                + (String.IsNullOrEmpty(empregador.CEI) ? "            " : empregador.CEI) + "]" + empregador.RazaoSocial + "]" + empregador.Local;
        }

        public override bool ExportacaoHabilitada()
        {
            return true;
        }

        public override bool TesteConexao()
        {
            string dadosFinal = "01+RQ+00+R";
            try
            {
                string info, oper = String.Empty;
                int ret = Convert.ToInt32(trataRetorno(EnviaDadosRep(dadosFinal), out info, out oper).Replace("\r\n", ""));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override int UltimoNSR()
        {
            string dadosFinal = "01+RQ+00+R";
            try
            {
                string info, oper = String.Empty;
                return Convert.ToInt32(trataRetorno(EnviaDadosRep(dadosFinal), out info, out oper).Replace("\r\n", ""));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
