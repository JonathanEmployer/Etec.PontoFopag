using br.com.cwork.cwkhexacomm;
using cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies;
using cwkPontoMT.Integracao.Entidades;
using org.bouncycastle.util.encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace cwkPontoMT.Integracao.Relogios.Henry
{
    public class Hexa : Relogio
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            log.Debug(NumeroSerie + " Iniciando o método GetAFDNsr");
            DateTime? dtInicio = dataI;
            DateTime? dtFinal = dataF;
            string header = "0000000001";
            List<RegistroAFD> regs = new List<RegistroAFD>();
            log.Debug(NumeroSerie + " Obtendo cabeçalho do arquivo");
            if (Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ)
            {
                header += "1";
            }
            else
            {
                header += "2";
            }
            header += GetStringSomenteAlfanumericoESimbolosPermitidos(Empregador.Documento).PadLeft(14, '0');
            header += String.IsNullOrEmpty(GetStringSomenteAlfanumericoESimbolosPermitidos(Empregador.CEI)) ? "            " : GetStringSomenteAlfanumericoESimbolosPermitidos(Empregador.CEI).PadRight(12, ' ');
            header += Empregador.RazaoSocial.PadRight(150, ' ');
            header += NumeroSerie.PadLeft(17, '0');
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("ddMMyyyy");
            header += DateTime.Now.ToString("HHmm");

            log.Debug(NumeroSerie + " Cabeçalho:");
            log.Debug(NumeroSerie + " " + header);
            string retorno = String.Empty;
            if (nsrInicio <= 0)
            {
                log.Debug(NumeroSerie + " Iniciando coleta através da data de inicio de importação" + " - " + dataI);
                retorno = RecebeMarcacoes(dataI, dataF);
            }
            else
            {
                log.Debug(NumeroSerie + " Iniciando coleta através do NSR" + " - " + nsrInicio);
                dtInicio = null;
                dtFinal = null;
                //if (nsrFim == int.MaxValue)
                //{
                //    try
                //    {
                //        int nsrFimRep = UltimoNSR();
                //        //nsrFim = nsrFimRep;
                //        log.Debug(NumeroSerie + " Ultimo NSR Indicado pelo rep NSR " + " - " + nsrFimRep);
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                //Removida a lógica de consideração do ultimo nsr do rep, pois o mesmo estava retornando o ultimo NSR errado.
                //if (nsrInicio >= nsrFim)
                //{
                //    return regs;
                //}
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

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            try
            {
                erros = String.Empty;
                var errosDic = PopularErros();
                HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
                rep.Conectar();
                int count = 0;
                string aes = GetChaveAES();
                while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
                {
                    rep.Autenticar(aes);
                    if (rep.Autenticado() == java.lang.Boolean.FALSE)
                    {
                        log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                        Thread.Sleep(1000);
                    }

                    count++;
                    if (count >= 10)
                    {
                        log.Debug(NumeroSerie + "Falha ao se autenticar com o REP.");
                        throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                    }
                }

                DateTime dtRelogio = RecebeHoraRelogio(rep, aes);
                var res = EnviaDtHora_IniFimHorarioVerao(rep, aes, dtRelogio, inicio, termino);

                if (res.Info > 0)
                {
                    erros = "Erro: " + errosDic[res.Info] + ". Código: " + res.Info;
                }

                rep.Logout(aes);
                rep.Desconectar();
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
            erros = String.Empty;
            try
            {
                var errosDic = PopularErros();
                HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
                rep.Conectar();
                string aes = GetChaveAES();
                int count = 0;
                while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
                {
                    rep.Autenticar(aes);
                    if (rep.Autenticado() == java.lang.Boolean.FALSE)
                    {
                        log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                        Thread.Sleep(1000);
                    }

                    count++;
                    if (count >= 10)
                    {
                        log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                        throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                    }
                }

                foreach (var item in Empregados)
                {
                    try
                    {
                        var res = EnviaFuncionario(rep, aes, Operacao.Exclusao, item.Pis, item.Nome, true, item.DsCodigo, String.Empty);

                        if (res.Info > 0)
                        {
                            if (res.Info < 12)
                            {
                                while (res.Info < 12 && res.Info > 0)
                                {
                                    Thread.Sleep(1000);
                                    res = EnviaFuncionario(rep, aes, Operacao.Exclusao, item.Pis, item.Nome, true, item.DsCodigo, String.Empty);
                                }
                            }
                            if (res.Info > 0)
                            {
                                if (res.Info.Equals("22"))
                                {
                                    erros += item.Nome + " - Usuário não cadastrado no REP." + Environment.NewLine;
                                }
                                else
                                {
                                    erros += "Erro: " + errosDic[res.Info] + ". Código: " + res.Info + (item.Nome) + Environment.NewLine;
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        erros += "Erro: " + e.Message + Environment.NewLine;
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
                rep.Logout(aes);
                rep.Desconectar();
                return String.IsNullOrEmpty(erros);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
            string aes = GetChaveAES();
            try
            {
                erros = String.Empty;
                var errosDic = PopularErros();
                DateTime? inicioHorarioVerao = null;
                DateTime? fimHorarioVerao = null;

                rep.Conectar();
                rep.Autenticar(aes);
                RecebeIniFimHorarioVerao(rep, aes, out inicioHorarioVerao, out fimHorarioVerao);

                var res = EnviaDtHora_IniFimHorarioVerao(rep, aes, horario, inicioHorarioVerao, fimHorarioVerao);
                if (res.Info > 0)
                {
                    erros = "Erro: " + errosDic[res.Info] + ". Código: " + res.Info;
                }

                return String.IsNullOrEmpty(erros);
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
            finally
            {
                rep.Logout(aes);
                rep.Desconectar();
            }
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
            string aes = GetChaveAES();
            try
            {
                erros = String.Empty;
                var errosDic = PopularErros();
                rep.Conectar();
                int count = 0;
                while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
                {
                    rep.Autenticar(aes);
                    if (rep.Autenticado() == java.lang.Boolean.FALSE)
                    {
                        log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                        Thread.Sleep(1000);
                    }

                    count++;
                    if (count >= 10)
                    {
                        log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                        throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                    }
                }
                erros = "";

                if (Empregador != null)
                {
                    try
                    {
                        var resEmpresa = EnviaEmpresa(rep, aes, Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? TipoEmpregador.Jurídica : TipoEmpregador.Física,
                                                                     Empregador.Documento, Empregador.CEI, Empregador.RazaoSocial, Empregador.Local);

                        if (resEmpresa.Info > 0)
                        {
                            erros += "Erro: " + errosDic[resEmpresa.Info] + ". Código: " + resEmpresa.Info + Environment.NewLine;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }



                foreach (var item in Empregados)
                {
                    erros += "\r\n";
                    try
                    {
                        bool validaBiometria = biometrico;
                        string segundoCodigo = String.Empty;
                        if (item.RFID.GetValueOrDefault() > 0 && ModeloRep == "R2")
                        {
                            validaBiometria = false;
                            segundoCodigo = item.RFID.ToString();
                        }
                        else if (ModeloRep == "R4")
                        {
                            if (item.MIFARE.GetValueOrDefault() > 0)
                            {
                                validaBiometria = false;
                                segundoCodigo = item.MIFARE.ToString();
                            }
                        }

                        var res = EnviaFuncionario(rep, aes, Operacao.Inclusao, item.Pis, item.Nome, validaBiometria, item.DsCodigo, segundoCodigo);

                        if (res.Info > 0)
                        {
                            if (res.Info < 12)
                            {
                                while (res.Info < 12 && res.Info > 0)
                                {
                                    Thread.Sleep(1000);
                                    res = EnviaFuncionario(rep, aes, Operacao.Inclusao, item.Pis, item.Nome, biometrico, item.DsCodigo, String.Empty);
                                }
                            }
                            if (res.Info > 0)
                            {
                                if (res.Info.Equals("22"))
                                {
                                    erros += item.Nome + " - Usuário não cadastrado no REP." + Environment.NewLine;
                                }
                                else
                                {
                                    erros += "Erro: " + errosDic[res.Info] + ". Código: " + res.Info + (item.Nome) + Environment.NewLine;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }

                if (Empregados.Count() > 0)
                {
                    Empregados.GroupBy(x => x.DsCodigo).ToList().ForEach(x =>
                    {
                        var empregado = Empregados.Where(y => y.DsCodigo == x.Key).ToList();
                        if (empregado.Count() > 0)
                        {
                            var qtdeBiometria = empregado.Where(e => e.valorBiometria != null).Count();
                            var biometrias = new List<string>();
                            if (qtdeBiometria > 0)
                                biometrias = empregado.Select(b => Encoding.Default.GetString(b.valorBiometria)).ToList();
                            EnviaFuncionarioBiometria(x.Key, qtdeBiometria, biometrias, rep, aes);
                        }
                    });
                }

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
            finally
            {
                rep.Logout(aes);
                rep.Desconectar();
            }
        }


        public override List<Biometria> GetBiometria(out string erros)
        {
            var listBiometria = new List<Biometria>();

            try
            {

                if (TipoBiometria == "Verde")
                {
                    HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);

                    string aes = GetChaveAES();

                    erros = String.Empty;
                    var errosDic = PopularErros();
                    rep.Conectar();
                    int count = 0;
                    while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
                    {
                        rep.Autenticar(aes);
                        if (rep.Autenticado() == java.lang.Boolean.FALSE)
                        {
                            log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                            Thread.Sleep(1000);
                        }

                        count++;
                        if (count >= 10)
                        {
                            log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                            throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                        }
                    }
                    erros = "";

                    foreach (var empregado in Empregados)
                    {
                        erros += "";
                        try
                        {
                            var result = new BiometricMessage();
                            result = RecebeBiometriaVerde(rep, empregado.DsCodigo, aes);

                            if (result.Info > 0)
                            {
                                erros += "Erro: " + errosDic[result.Info] + ". Código: " + result.Info + Environment.NewLine;
                            }


                            var codigoBiometria = 0;
                            if (result.Dados.Count() > 0)
                            {
                                foreach (var biometria in result.Dados)
                                {
                                    listBiometria.Add(new Biometria()
                                    {
                                        codigo = codigoBiometria++,
                                        valorBiometria = (byte[])biometria,
                                        idfuncionario = empregado.id,
                                        idRep = IdRelogio
                                    });
                                }
                            }
                            else
                            {
                                listBiometria.Add(new Biometria()
                                {
                                    codigo = codigoBiometria++,
                                    valorBiometria = Encoding.UTF8.GetBytes("0"),
                                    idfuncionario = empregado.id,
                                    idRep = IdRelogio
                                });
                            }
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }
                    }
                }
                else
                {

                    var tcpRep = new TcpRep();
                    var repAutenticado = tcpRep.ConnectTcp(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);


                    erros = String.Empty;
                    var errosDic = PopularErros();

                    int count = 0;
                    while (!repAutenticado && count < 10)
                    {
                        repAutenticado = tcpRep.ConnectTcp(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
                        if (repAutenticado)
                        {
                            log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                            Thread.Sleep(1000);
                        }

                        count++;
                        if (count >= 10)
                        {
                            log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                            throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                        }
                    }
                    erros = "";

                    foreach (var empregado in Empregados)
                    {
                        erros += "";
                        try
                        {
                            var result = new BiometricMessage();
                            result = RecebeBiometriaVermelha(tcpRep, empregado.DsCodigo);

                            if (result.Info > 0)
                            {
                                erros += "Erro: " + errosDic[result.Info] + ". Código: " + result.Info + Environment.NewLine;
                            }


                            var codigoBiometria = 0;
                            if (result.Dados.Count() > 0)
                            {
                                foreach (var biometria in result.Dados)
                                {
                                    listBiometria.Add(new Biometria()
                                    {
                                        codigo = codigoBiometria++,
                                        valorBiometria = (byte[])biometria,
                                        idfuncionario = empregado.id,
                                        idRep = IdRelogio
                                    });
                                }
                            }
                            else
                            {
                                listBiometria.Add(new Biometria()
                                {
                                    codigo = codigoBiometria++,
                                    valorBiometria = Encoding.UTF8.GetBytes("0"),
                                    idfuncionario = empregado.id,
                                    idRep = IdRelogio
                                });
                            }
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }
                    }
                }
                return listBiometria;
            }
            catch (Exception e)
            {
                erros = "";
                erros += e.Message;
                throw e;
            }
        }


        #region Métodos Auxiliares Henry

        private IMessage EnviaFuncionarioBiometria(string Referencia, int QtdeBio, List<string> ValorBiometria, HexaRep rep, string aes)
        {
            string result = "";

            if (QtdeBio == 0)
            {
                result = "E]" + Referencia;

                IMessage msg = null;
                int count = 0;

                try
                {
                    while (count < 10)
                    {
                        try
                        {
                            Thread.Sleep(300);
                            string retorno = rep.Comunicar(aes, "01", "ED", "00", result);
                            IParseStrategy strat = HexaParseStrategyFactory.Produce("ED");
                            msg = strat.Parse("ED", retorno);
                            break;
                        }
                        catch (java.lang.IllegalArgumentException iae)
                        {
                            throw iae;
                        }
                        catch (java.lang.IllegalStateException ise)
                        {
                            throw ise;
                        }
                        catch (Exception)
                        {
                            count++;
                            if (count >= 10)
                            {
                                throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                            }
                            continue;
                        }
                    }

                    return msg;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                if (TipoBiometria == "Verde")
                {
                    result = "T]" + Referencia + "}B}B}";
                }
                else
                {
                    result = "D]" + Referencia + "}" + 1 + "}";
                }

                IMessage msg = null;
                int count = 0;

                try
                {

                    for (int i = 0; i < QtdeBio; i++)
                    {
                        var commandBio = result;
                        if (TipoBiometria == "Verde")
                            commandBio += i + "}514{" + ValorBiometria[i];

                        else
                        {
                            commandBio += i + "{" + ValorBiometria[i];
                        }
                        //result += i + "{" + ValorBiometria[i];

                        while (count < 10)
                        {
                            try
                            {
                                Thread.Sleep(300);
                                string retorno = rep.Comunicar(aes, "01", "ED", "00", commandBio);
                                IParseStrategy strat = HexaParseStrategyFactory.Produce("ED");
                                msg = strat.Parse("ED", retorno);
                                break;
                            }
                            catch (java.lang.IllegalArgumentException iae)
                            {
                                throw iae;
                            }
                            catch (java.lang.IllegalStateException ise)
                            {
                                throw ise;
                            }
                            catch (Exception)
                            {
                                count++;
                                if (count >= 10)
                                {
                                    throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                                }
                                continue;
                            }
                        }
                    }

                    return msg;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private BiometricMessage RecebeBiometriaVerde(HexaRep rep, string CodFuncionario, string aes)
        {

            string result = "Q]" + CodFuncionario;
            int QtdBiometria = 1;
            BiometricMessage msg = new BiometricMessage();
            int count = 0;

            while (count < 10)
            {
                try
                {
                    string retorno = rep.Comunicar(aes, "01", "RD", "00", result);
                    IParseStrategy strat = HexaParseStrategyFactory.Produce("RD");
                    msg = strat.ParseBiometric("Q", retorno, TipoBiometria);
                    QtdBiometria = int.Parse(Encoding.UTF8.GetString((byte[])msg.Dados[0]));
                    if (QtdBiometria == 0)
                    {
                        msg.Info = 0;
                        msg.Indice = 0;
                        msg.Comando = "";
                        msg.Dados = new List<object>();
                    }
                    else
                    {
                        msg = new BiometricMessage();
                    }
                    break;
                }
                catch (java.lang.IllegalArgumentException iae)
                {
                    throw iae;
                }
                catch (java.lang.IllegalStateException ise)
                {
                    throw ise;
                }
                catch (Exception)
                {
                    count++;
                    if (count >= 10)
                    {
                        throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                    }
                    continue;
                }
            }
            result = "T]" + CodFuncionario + "}B}N}";

            for (int i = 0; i < QtdBiometria; i++)
            {
                while (count < 10)
                {
                    try
                    {
                        var command = result + i;

                        string retorno = rep.Comunicar(aes, "01", "RD", "00", command);
                        IParseStrategy strat = HexaParseStrategyFactory.Produce("RD");
                        var msgBio = strat.ParseBiometric("RD", retorno, TipoBiometria);

                        msg.Dados.Add(msgBio.Dados[0]);
                    }
                    catch (java.lang.IllegalArgumentException iae)
                    {
                        throw iae;
                    }
                    catch (java.lang.IllegalStateException ise)
                    {
                        throw ise;
                    }
                    catch (Exception)
                    {
                        count++;
                        if (count >= 10)
                        {
                            throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                        }
                        continue;
                    }
                    break;
                }
            }

            return new BiometricMessage() { Info = msg.Info, Indice = msg.Indice, Comando = msg.Comando, Dados = msg.Dados };
        }
        private BiometricMessage RecebeBiometriaVermelha(TcpRep rep, string CodFuncionario)
        {

            string result = "Q]" + CodFuncionario;
            int QtdBiometria = 1;
            BiometricMessage msg = new BiometricMessage();
            int count = 0;

            while (count < 10)
            {
                try
                {
                    QtdBiometria = int.Parse(rep.ReceiveBiometricsAmount("01+RD+00+Q]" + CodFuncionario));
                    if (QtdBiometria == 0)
                    {
                        msg.Info = 0;
                        msg.Indice = 0;
                        msg.Comando = "";
                        msg.Dados = new List<object>();
                    }
                    else
                    {
                        msg = new BiometricMessage();
                    }
                    break;
                }
                catch (java.lang.IllegalArgumentException iae)
                {
                    throw iae;
                }
                catch (java.lang.IllegalStateException ise)
                {
                    throw ise;
                }
                catch (Exception)
                {
                    count++;
                    if (count >= 10)
                    {
                        throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                    }
                    continue;
                }
            }

            result = "D]" + CodFuncionario + "}";

            for (int i = 0; i < QtdBiometria; i++)
            {
                while (count < 10)
                {
                    try
                    {
                        var command = result + i;

                        var bio = rep.ReceiveBiometrics("01+RD+00+" + command, 48);

                        msg.Dados.Add(Encoding.Default.GetBytes(bio));
                    }
                    catch (java.lang.IllegalArgumentException iae)
                    {
                        throw iae;
                    }
                    catch (java.lang.IllegalStateException ise)
                    {
                        throw ise;
                    }
                    catch (Exception)
                    {
                        count++;
                        if (count >= 10)
                        {
                            throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                        }
                        continue;
                    }
                    break;
                }
            }

            return new BiometricMessage() { Info = msg.Info, Indice = msg.Indice, Comando = msg.Comando, Dados = msg.Dados };
        }

        private DateTime RecebeHoraRelogio(HexaRep rep, string aes)
        {
            try
            {
                ReceiveDateTimeMessage msg = null;
                IParseStrategy strat = HexaParseStrategyFactory.Produce("RH");

                while (true)
                {
                    try
                    {
                        string res = rep.Comunicar(aes, "01", "RH", "00", "");
                        msg = (ReceiveDateTimeMessage)strat.Parse("RH", res);
                        break;
                    }
                    catch (java.lang.IllegalStateException)
                    {
                        throw new Exception("Não foi possível conectar com o equipamento");
                    }
                    catch (java.lang.IllegalArgumentException)
                    {
                        throw new Exception("Não foi possível comunicar com o equipamento");
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return msg.DataHora;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void RecebeIniFimHorarioVerao(HexaRep rep, string aes, out DateTime? inicio, out DateTime? fim)
        {
            ReceiveDateTimeMessage msg = null;
            IParseStrategy strat = HexaParseStrategyFactory.Produce("RH");

            while (true)
            {
                try
                {
                    string res = rep.Comunicar(aes, "01", "RH", "00", "");
                    msg = (ReceiveDateTimeMessage)strat.Parse("RH", res);
                    break;
                }
                catch (java.lang.IllegalStateException)
                {
                    throw new Exception("Não foi possível conectar com o equipamento");
                }
                catch (java.lang.IllegalArgumentException)
                {
                    throw new Exception("Não foi possível comunicar com o equipamento");
                }
                catch (Exception)
                {
                    continue;
                }
            }

            try
            {
                inicio = msg.InicioHorarioVerao;
                fim = msg.FimHorarioVerao;
            }
            catch (Exception)
            {
                inicio = null;
                fim = null;
            }

        }

        private IMessage EnviaDtHora_IniFimHorarioVerao(HexaRep rep, string aes, DateTime DataHoraAtual, DateTime? InicioHorarioVerao, DateTime? FimHorarioVerao)
        {
            if (InicioHorarioVerao.HasValue ^ FimHorarioVerao.HasValue)
            {
                throw new Exception("Obrigatório informar Inicio e Fim do horário de Verão.");
            }
            string iniHorVerao = InicioHorarioVerao.HasValue ? InicioHorarioVerao.Value.ToString("dd/MM/yy") : "00/00/00";
            string fimHorVerao = FimHorarioVerao.HasValue ? FimHorarioVerao.Value.ToString("dd/MM/yy") : "00/00/00";
            string result = DataHoraAtual.ToString("dd/MM/yy HH:mm:ss") + "]" + iniHorVerao + "]" + fimHorVerao;

            GeneralMessage msg = null;
            IParseStrategy strat = HexaParseStrategyFactory.Produce("EH");
            int count = 0;

            while (count < 10)
            {
                try
                {
                    string res = rep.Comunicar(aes, "01", "EH", "00", result);
                    msg = (GeneralMessage)strat.Parse("EH", res);
                    break;
                }
                catch (java.lang.IllegalStateException)
                {
                    throw new Exception("Não foi possível conectar com o equipamento");
                }
                catch (java.lang.IllegalArgumentException)
                {
                    throw new Exception("Não foi possível comunicar com o equipamento");
                }
                catch (Exception)
                {
                    count++;
                    if (count >= 10)
                    {
                        throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                    }
                    continue;
                }
            }
            return msg;

        }

        private IMessage EnviaEmpresa(HexaRep rep, string aes, TipoEmpregador tipoEmp, string CnpjCpf, string Cei, string RazaoSocial, string Local)
        {
            string result = (int)tipoEmp + "]" + GetStringSomenteAlfanumericoESimbolosPermitidos(CnpjCpf) + "]" + Cei + "]" + RazaoSocial + "]" + Local;
            IMessage msg = null;
            int count = 0;
            try
            {
                while (count < 10)
                {
                    try
                    {
                        string retorno = rep.Comunicar(aes, "01", "EE", "00", result);
                        IParseStrategy strat = HexaParseStrategyFactory.Produce("EE");
                        msg = strat.Parse("EE", retorno);
                        break;
                    }
                    catch (java.lang.IllegalArgumentException iae)
                    {
                        throw iae;
                    }
                    catch (java.lang.IllegalStateException ise)
                    {
                        throw ise;
                    }
                    catch (Exception)
                    {
                        count++;
                        if (count >= 10)
                        {
                            throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                        }
                        continue;
                    }
                }
                return msg;
            }
            catch (java.lang.IllegalStateException)
            {
                throw new Exception("Não foi possível conectar com o equipamento");
            }
            catch (java.lang.IllegalArgumentException)
            {
                throw new Exception("Não foi possível comunicar com o equipamento");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private IMessage EnviaFuncionario(HexaRep rep, string aes, Operacao op, string pis, string nome, bool usaBiometria, string referencia1, string referencia2)
        {
            string oper = "";
            IMessage msg = null;
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
            string result = "1+" + oper + "[" + pis + "[" + nome + "[" + Convert.ToInt16(usaBiometria).ToString() + "[" + qtdRefs + "[" + referencia + "]";
            int count = 0;
            try
            {
                count = 0;
                while (count < 10)
                {
                    try
                    {
                        string retorno = rep.Comunicar(aes, "01", "EU", "00", result);
                        IParseStrategy strat = HexaParseStrategyFactory.Produce("EU");
                        msg = strat.Parse("EU", retorno);
                        break;
                    }
                    catch (java.lang.IllegalArgumentException iae)
                    {
                        throw iae;
                    }
                    catch (java.lang.IllegalStateException ise)
                    {
                        throw ise;
                    }
                    catch (Exception)
                    {
                        count++;
                        if (count >= 10)
                        {
                            throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Reinicie o REP e tente novamente.");
                        }
                        continue;
                    }
                }
                return msg;
            }
            catch (java.lang.IllegalStateException)
            {
                throw new Exception("Não foi possível conectar com o equipamento");
            }
            catch (java.lang.IllegalArgumentException)
            {
                throw new Exception("Não foi possível comunicar com o equipamento");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string RecebeMarcacoes(DateTime? inicio, DateTime? fim)
        {
            HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
            log.Debug(NumeroSerie + " Obtendo chave do REP Hexa B");
            string aes = GetChaveAES();
            StringBuilder sb = new StringBuilder();

            try
            {
                int count = 0;
                rep.Conectar();
                log.Debug(NumeroSerie + " Iniciando autenticação do REP");
                while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
                {
                    rep.Autenticar(aes);
                    if (rep.Autenticado() == java.lang.Boolean.FALSE)
                    {
                        log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                        Thread.Sleep(1000);
                    }

                    count++;
                    if (count >= 10)
                    {
                        log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                        throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                    }
                }
                log.Debug(NumeroSerie + " Autenticação realizada com sucesso!");
                IMessage msg1 = null;
                IMessage msg2 = null;
                IParseStrategy stratRR = HexaParseStrategyFactory.Produce("RR");
                IParseStrategy stratRQ = HexaParseStrategyFactory.Produce("RQ");

                Int32 nsrInicial = 0;
                Int32 nsrFinal = 0;
                List<Ticket> batidas = new List<Ticket>();
                int step = 7;
                count = 0;

                log.Debug(NumeroSerie + " Iniciando tentativa de comunicação com o REP");
                while (count < 10)
                {
                    try
                    {
                        string strInicio = rep.Comunicar(aes, "01", "RR", "00", "D]1]" + inicio.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") + "]");
                        msg1 = stratRR.Parse("RR", strInicio);
                        nsrInicial = ((TicketMessage)msg1).MinNSR;
                        break;
                    }
                    catch (Exception)
                    {
                        count++;
                        if (count >= 10)
                        {
                            log.Debug(NumeroSerie + " Falha ao se comunicar com o REP, falha ao receber o retorno do equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP e tente novamente.");
                            throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP e tente novamente.");
                        }
                        continue;
                    }
                }
                log.Debug(NumeroSerie + " Tentativa de comunicação realizada com sucesso!");

                count = 0;
                log.Debug(NumeroSerie + " Iniciando obtenção do NsrFinal");
                while (count < 10)
                {
                    try
                    {
                        string strFinal = rep.Comunicar(aes, "01", "RQ", "00", "R");
                        msg2 = stratRQ.Parse("RQ", strFinal);
                        nsrFinal = Convert.ToInt32(((GeneralMessage)msg2).Dados);
                        if (nsrFinal < nsrInicial)
                        {
                            continue;
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        count++;
                        if (count >= 10)
                        {
                            log.Debug(NumeroSerie + " Falha ao obter o NsrFinal");
                            throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP e tente novamente.");
                        }
                        continue;
                    }
                }
                log.Debug(NumeroSerie + " Tentativa de obter o NsrFinal realizada com sucesso. Nsr: " + nsrFinal);

                if (nsrInicial == 0)
                {
                    nsrInicial = 1;
                }
                if (nsrFinal == 0)
                {
                    nsrFinal = Int32.MaxValue;
                }

                string data = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                log.Debug(NumeroSerie + " Iniciando gravação do AFD em um arquivo a parte");
                try
                {
                    do
                    {
                        count = 0;
                        while (count < 10)
                        {
                            try
                            {
                                string strFinal = rep.Comunicar(aes, "01", "RR", "00", "N]" + step.ToString() + "]" + nsrInicial.ToString());
                                msg2 = stratRR.Parse("RR", strFinal);

                                foreach (var item in ((TicketMessage)msg2).Tickets)
                                {
                                    log.Debug("AFD_" + NumeroSerie + "_" + data + "Linha " + item.TicketCompleto);
                                }

                                if (((TicketMessage)msg2).Tickets.Where(t => t.TipoTicket == 3) != null)
                                {
                                    batidas.AddRange(((TicketMessage)msg2).Tickets.Where(t => t.TipoTicket == 3));
                                }
                                if (batidas.Count > 0)
                                {
                                    if (batidas.Max(m => m.DataHora) > fim.GetValueOrDefault())
                                    {
                                        break;
                                    }
                                }

                                break;
                            }
                            catch (Exception)
                            {
                                count++;
                                if (count >= 10)
                                {
                                    log.Debug(NumeroSerie + " Falha ao receber o retorno do equipamento após 10 tentativas.");
                                    throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP e tente novamente.");
                                }
                                Thread.Sleep(500);
                                continue;
                            }
                        }
                        nsrInicial += step;
                    } while (nsrFinal >= (nsrInicial) && msg2.Info != 50);
                }
                catch (Exception e)
                {
                    //Se coletou batidas, mesmo que apresente erro, tenta enviar para o servidor
                    if (!batidas.Any())
                    {
                        throw e;
                    }
                }
                log.Debug(NumeroSerie + " Setando o NSR Inicial. Posição atual: " + nsrInicial);
                log.Debug(NumeroSerie + " Gravação do AFD Realizado com sucesso!");

                log.Debug(NumeroSerie + " Adicionando a(s) batida(s) encontrada(s) no string builder.");
                foreach (var item in batidas)
                {
                    sb.AppendLine(item.ToString());
                }
            }
            catch (java.lang.IllegalStateException)
            {
                throw new Exception("Não foi possível conectar com o equipamento");
            }
            catch (java.lang.IllegalArgumentException)
            {
                throw new Exception("Não foi possível comunicar com o equipamento");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                log.Debug(NumeroSerie + " Desconectando com o REP");
                rep.Logout(aes);
                rep.Desconectar();
            }
            return sb.ToString();
        }

        private string GetChaveAES()
        {
            return Encoding.Default.GetString(Base64.encode(new byte[]{0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38,
                0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38}));
        }

        private string RecebeMarcacoesNsr(int nsrInicio, int nsrFim)
        {
            log.Debug(NumeroSerie + " Iniciando método RecebeMarcacoesNsr");
            HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
            log.Debug(NumeroSerie + " Obtendo chave do REP Hexa B");
            string aes = GetChaveAES();
            int count = 0;
            rep.Conectar();
            log.Debug(NumeroSerie + " Iniciando autenticação do REP");
            while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
            {
                rep.Autenticar(aes);
                if (rep.Autenticado() == java.lang.Boolean.FALSE)
                {
                    log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                    Thread.Sleep(1000);
                }

                count++;
                if (count >= 10)
                {
                    log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                    throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                }
            }
            log.Debug(NumeroSerie + " Tentativa de comunicação realizada com sucesso!");

            StringBuilder sb = new StringBuilder();
            try
            {
                IMessage msg2 = null;
                IParseStrategy stratRR = HexaParseStrategyFactory.Produce("RR");
                IParseStrategy stratRQ = HexaParseStrategyFactory.Produce("RQ");

                List<Ticket> batidas = new List<Ticket>();
                int step = 5;

                if (nsrInicio == 0)
                {
                    nsrInicio = 1;
                }
                if (nsrFim == 0)
                {
                    nsrFim = Int32.MaxValue;
                }

                int nsrIniOriginal = nsrInicio;
                int nsrFimOriginal = nsrFim;

                // Sempre coleto um registro antes do ultimo do sistema, ou seja, sempre será solicitado para o rep 1 registro anterior ao que foi solicitado.
                // isso acontece pois o Hexa Apresenta erro as vezes ao pedir o ultimo bilhete mais o step, ou sempre apresenta erro caso solicite um nsr que ainda não existe.
                // essa validação foi criada para que nunca seja solicitado o último registro do rep.
                if (nsrInicio > 2)
                {
                    nsrInicio -= 1;
                    log.Debug(NumeroSerie + " Voltando 1 NSRs, NSR Inicio = " + nsrInicio + "( isso é feito para que o sistema nunca inicie uma coleta do ultimo NSR do rep, pois isso causa erro no rep as vezes)");
                }

                log.Debug(NumeroSerie + " Iniciando gravação do AFD em um arquivo a parte");
                string data = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                count = 0;
                int infoRetorno = 0;
                do
                {
                    int RetornoComandoIncorreto = 0;
                    int ultimoNSRSolicitado = 0;
                    int ultimoNSRColetado = 0;
                    while (true)
                    {
                        try
                        {
                            log.Debug(NumeroSerie + " Solicitando " + step.ToString() + " registros a partir do = " + nsrInicio.ToString() + ". Comando = \"RR\", info = \"00\", dados = N]" + step.ToString() + "]" + nsrInicio.ToString());
                            string strFinal = rep.Comunicar(aes, "01", "RR", "00", "N]" + step.ToString() + "]" + nsrInicio.ToString());
                            log.Debug(NumeroSerie + " Lendo Retorno Solicitado = " + strFinal);
                            // Se foi retorno de erro para o processo
                            if (infoRetorno == 50)
                                break;

                            msg2 = stratRR.Parse("RR", strFinal);
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            log.Debug(NumeroSerie + " Retorno do Rep = " + js.Serialize(msg2));

                            // Se o NSR Min não corresponder ao nsrInicio (Solicitado) significa que o rep retornou a errada
                            if (((TicketMessage)msg2).MinNSR == nsrInicio)
                            {
                                //foreach (var item in ((TicketMessage)msg2).Tickets)
                                //{
                                //    log.Debug("AFD_" + NumeroSerie + "_" + data, item.TicketCompleto);
                                //}
                                if (((TicketMessage)msg2).Tickets.Where(t => t.TipoTicket == 3) != null)
                                {
                                    batidas.AddRange(((TicketMessage)msg2).Tickets.Where(t => t.TipoTicket == 3));
                                }

                                ultimoNSRSolicitado = (nsrInicio + step - 1);
                                ultimoNSRColetado = ((TicketMessage)msg2).MaxNSR;
                                log.Debug(NumeroSerie + " Comparando última coleta, Ultimo Solicitado = " + ultimoNSRSolicitado + " Ultimo coletado = " + ultimoNSRColetado);
                                if (ultimoNSRSolicitado > ultimoNSRColetado)
                                {
                                    nsrFim = ultimoNSRColetado;
                                }
                                nsrInicio += step;
                                break;
                            }
                            else //Caso o Rep não retornou o que foi solicitado tenta por 3 vez pegar o correto
                            {
                                if (RetornoComandoIncorreto < 3)
                                {
                                    log.Debug(NumeroSerie + " XXXXX Rep retornou dados errados, comando será enviado novamente");
                                    RetornoComandoIncorreto++;
                                    Thread.Sleep(200);
                                }
                                else
                                {
                                    throw new Exception("Relógio não acatou o comando solicitado corretamente");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            count++;
                            // Se tentou coletar mais que 10 vezes aborta o processo
                            if (count >= 10)
                            {
                                log.Debug("***************************************************************");
                                log.Debug(NumeroSerie + $" ***** Testando dados coletados apos 10 tentativas, qtd Batidas {(batidas != null ? batidas.Count : 0)}; ultimoNSRSolicitado = {ultimoNSRColetado}; ultimoNSRSolicitado = {ultimoNSRColetado}");
                                // Envio o que já foi coletado até o momento, pois se a variavel batidas estiver maior que 0, significa que coletou algo. Ou se ultimoNSRSolicitado e ultimoNSRColetado forem iguais significa que ele coletou até o último e depois pediu mais um fora do que existe no rep, o que da problema.
                                if ((batidas != null && batidas.Count > 0) || (ultimoNSRSolicitado > 0 && ultimoNSRSolicitado == ultimoNSRColetado))
                                {
                                    log.Debug(NumeroSerie + " XXX Relógio apresentou erro ao continuar buscando batidas (Ou ou rep parou de responder ou acabaram os bilhetes)");
                                    log.Debug(NumeroSerie + "     Retornando " + batidas.Count() + " que foi possível coletar do rep, NSR Fim = " + batidas.Select(s => s.NSR).Max());
                                    break;
                                }
                                else
                                {
                                    log.Debug(NumeroSerie + " XXXXX Falha ao se comunicar com o REP, falha ao receber o retorno do equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP e tente novamente. Erro: " + ex.Message + " StackTrace = " + ex.StackTrace);
                                    throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas de coleta. Aguarde alguns instantes, ou reinicie o REP e tente novamente.");
                                }
                            }
                            else
                            {
                                log.Debug(NumeroSerie + " XXXXX Falha ao se comunicar com o REP, realizará nova tentativa. Erro: " + ex.Message + " StackTrace = " + ex.StackTrace);
                            }
                            Thread.Sleep(250);
                            continue;
                        }
                    }
                    log.Debug(NumeroSerie + " Avaliando proxima Interacao, passa para proxima se " + nsrFim + "(nsrFim)>= (" + nsrInicio + "(nsrInicio)) && " + msg2.Info + "(msg2.Info) != 50 &&" + infoRetorno + "(infoRetorno) != 50");
                } while (nsrFim >= (nsrInicio) && msg2.Info != 50 && infoRetorno != 50);

                log.Debug(NumeroSerie + " Adicionando a(s) batida(s) encontrada(s) no string builder.");
                foreach (var item in batidas.Where(w => w.NSR >= nsrIniOriginal && w.NSR <= nsrFimOriginal))
                {
                    sb.AppendLine(item.ToString());
                }
                log.Debug(NumeroSerie + " Batidas retornadas = " + sb.ToString());
            }
            catch (java.lang.IllegalStateException)
            {
                throw new Exception("Não foi possível conectar com o equipamento");
            }
            catch (java.lang.IllegalArgumentException)
            {
                throw new Exception("Não foi possível comunicar com o equipamento");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                log.Debug(NumeroSerie + " Desconectando com o REP");
                rep.Logout(aes);
                rep.Desconectar();
            }
            return sb.ToString();
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

        private string GetStringSomenteAlfanumericoESimbolosPermitidos(string s)
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
            return "2+" + (int)empregador.TipoDocumento + "]" + GetStringSomenteAlfanumericoESimbolosPermitidos(empregador.Documento) + "]"
                + (String.IsNullOrEmpty(empregador.CEI) ? "            " : empregador.CEI) + "]" + empregador.RazaoSocial + "]" + empregador.Local;
        }

        public override bool ExportacaoHabilitada()
        {
            return true;
        }

        #endregion

        public override bool TesteConexao()
        {
            throw new NotImplementedException();
        }

        public override int UltimoNSR()
        {
            HexaRep rep = new HexaRep(IP, Convert.ToInt32(Porta), UsuarioREP, SenhaUsuarioREP);
            string aes = GetChaveAES();

            try
            {
                try
                {
                    int count = 0;
                    rep.Conectar();
                    while (rep.Autenticado() == java.lang.Boolean.FALSE && count < 10)
                    {
                        rep.Autenticar(aes);
                        if (rep.Autenticado() == java.lang.Boolean.FALSE)
                        {
                            log.Debug(NumeroSerie + " Erro na comunicação, tentando novamente, ping = " + PingHost(IP));
                            Thread.Sleep(1000);
                        }

                        count++;
                        if (count >= 10)
                        {
                            log.Debug(NumeroSerie + " Falha ao se autenticar com o REP.");
                            throw new Exception("Falha ao realizar a autenticação no equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP para tentar novamente.");
                        }
                    }
                    IMessage msg1 = null;
                    IParseStrategy stratRQ = HexaParseStrategyFactory.Produce("RQ");
                    count = 0;
                    while (count < 10)
                    {
                        try
                        {
                            string strFinal = rep.Comunicar(aes, "01", "RQ", "00", "R");
                            msg1 = stratRQ.Parse("RQ", strFinal);
                            int nsrFinal = Convert.ToInt32(((GeneralMessage)msg1).Dados);
                            return nsrFinal;
                        }
                        catch (Exception)
                        {
                            count++;
                            if (count >= 10)
                            {
                                throw new Exception("Falha ao receber o retorno do equipamento após 10 tentativas. Aguarde alguns instantes, ou reinicie o REP e tente novamente.");
                            }
                            continue;
                        }
                    }
                }
                catch (java.lang.IllegalStateException)
                {
                    throw new Exception("Não foi possível conectar com o equipamento");
                }
                catch (java.lang.IllegalArgumentException)
                {
                    throw new Exception("Não foi possível comunicar com o equipamento");
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    rep.Logout(aes);
                    rep.Desconectar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar ultimo NSR, erro: " + ex.Message);
            }
            return int.MaxValue;
        }

        public static string PingHost(string nameOrAddress)
        {
            bool pingable = false;
            string pingableStr;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException pe)
            {
                return pe.Message;
            }
            return pingableStr = pingable.ToString();
        }
    }
}
