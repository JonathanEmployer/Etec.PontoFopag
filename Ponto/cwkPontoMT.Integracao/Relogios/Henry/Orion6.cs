using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Kernel7x;
using System.Reflection;
using System.IO;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios.Henry
{
    public class Orion6 : Relogio
    {
        Kernel7x.Alternativo relogio = new Kernel7x.Alternativo();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int thread;
        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            string nomeArquivoLog = NumeroSerie;
            log.Debug(nomeArquivoLog + " Iniciando coleta de AFD");
            log.Debug(nomeArquivoLog + " Setando evento de coleta");
            relogio.OnColetaEventos += relogio_OnColetaEventos;

            List<RegistroAFD> retorno = new List<RegistroAFD>();
            try
            {
                log.Debug(nomeArquivoLog + " Verificando se já possuia AFD salvo na pasta, caso positivo apaga");
                string caminhoArquivo;
                caminhoArquivo = Assembly.GetEntryAssembly().Location;
                caminhoArquivo = Path.GetDirectoryName(caminhoArquivo) + "\\AFD" + NumeroSerie + ".txt";
                if (File.Exists(caminhoArquivo))
                {
                    File.Delete(caminhoArquivo);
                }


                log.Debug(nomeArquivoLog + " Setando dados do empregador para coleta");
                SEmpregador Emp = new SEmpregador()
                {
                    IdEmpregador = Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? SIdEmpregador.cieCNPJ : SIdEmpregador.cieCPF,
                    CEI = GetStringSomenteAlfanumerico(Empregador.CEI),
                    Documento = GetStringSomenteAlfanumerico(Empregador.Documento),
                    Local = Empregador.Local,
                    RazaoSocial = Empregador.RazaoSocial
                };

                try
                {
                    log.Debug(nomeArquivoLog + " Conectando no rep");
                    Conecta(IP, Porta, TipoComunicacao, NumeroRelogio);
                }
                catch (Exception e)
                {
                    log.Debug(nomeArquivoLog + " Erro ao Conectar no rep");
                    throw e;
                }

                log.Debug(nomeArquivoLog + " Iniciando escrita do AFD pelo método coletaEventosEX da DLL do rep");
                if (!relogio.get_ColetaEventosEx(thread, caminhoArquivo, dataI, Emp.RazaoSocial, Emp.Local, Emp.Documento, Emp.CEI, Emp.IdEmpregador))
                {
                    log.Debug(nomeArquivoLog + " Erro na coleta do evento coletaEventosEX");
                    string err = relogio.ErrorDescription(relogio.get_ThreadLastError(thread));
                    Desconecta();
                    if (err == "Evento não existe")
                    {
                        return retorno;
                    }
                    else
                    {
                        throw new Exception(err);
                    }
                }

                try
                {
                    log.Debug(nomeArquivoLog + " Aguardando AFD ser escrito pelo coletaEventosEX");
                    int tempoMaximoEspera = 0;
                    while (!File.Exists(caminhoArquivo))
                    {
                        tempoMaximoEspera++;
                        if (tempoMaximoEspera == 7200) //Quando chegar a espera de 2 horas, cancela a coleta.
                        {
                            try
                            {
                                relogio.PararColetaEventos(thread);
                            }
                            catch (Exception)
                            {
                                Desconecta();
                                throw;
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    log.Debug(nomeArquivoLog + " Terminada escrita do AFD, desconectando do REP");
                    Desconecta();
                    log.Debug(nomeArquivoLog + " Lendo o arquivo escrito");
                    string data = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    using (StreamReader objReader = new StreamReader(caminhoArquivo))
                    {
                        while (objReader.Peek() >= 0)
                        {
                            string linha = objReader.ReadLine();
                            log.Debug("AFD_" + NumeroSerie + "_" + data + " Linha "+ linha);
                            Util.IncluiRegistro(linha, dataI, dataF, retorno);
                        }
                    }
                    log.Debug(nomeArquivoLog + " Deletando o arquivo após a leitura");
                    File.Delete(caminhoArquivo);
                }
                catch (Exception e)
                {
                    log.Debug(nomeArquivoLog + " Erro na leitura do arquivo, desconectando");
                    Desconecta();
                    log.Debug(nomeArquivoLog + " Erro na leitura do arquivo, erro = "+Util.GetExceptionMessage(e));
                    throw e;
                }

                //Verifica se o AFD gerado pelo rep esta com o cabeçalho (Num série) errado, caso esteja, corrige
                log.Debug(nomeArquivoLog + " Verificando se o AFD gerado pelo rep esta com o cabeçalho (Num série) errado");
                RegistroAFD cabecalho = retorno.Where(w => w.Campo02 == "1").FirstOrDefault();
                if (cabecalho.Campo07 != NumeroSerie)
                {
                    log.Debug(nomeArquivoLog + " Corrigindo Cabecalho");
                    cabecalho.Campo07 = NumeroSerie;
                    cabecalho.LinhaAFD = cabecalho.LinhaAFD.Replace(cabecalho.Campo07, NumeroSerie);
                }
            }
            catch (Exception e)
            {
                log.Debug(nomeArquivoLog + " Erro na coleta do AFD, erro = "+ Util.GetExceptionMessage(e));
                throw e;
            }
            log.Debug(nomeArquivoLog + " Retornando AFD Coletado");
            return retorno;
        }

        void relogio_OnColetaEventos(int pThreadIndex, bool pResultado, int pQtdeRegColetados, string pPathAFD)
        {
            string nomeArquivoLog = NumeroSerie;
            try
            {
                log.Debug(nomeArquivoLog + " relogio_OnColetaEventos Chamado");
                if (pResultado == true)
                {
                    log.Debug(nomeArquivoLog + " relogio_OnColetaEventos resultado = true");
                    if (!File.Exists(pPathAFD))
                    {
                        log.Debug(nomeArquivoLog + " relogio_OnColetaEventos resultado = true, arquivo não foi criado, criando manualmente");
                        using (StreamWriter sw = File.CreateText(pPathAFD))
                        {
                        }
                    }
                    log.Debug(nomeArquivoLog + " relogio_OnColetaEventos resultado = true, arquivo criado pela DLL do rep");
                }
                else
                {
                    log.Debug(nomeArquivoLog + " relogio_OnColetaEventos resultado = false, arquivo não foi criado, criando manualmente");
                    using (StreamWriter sw = File.CreateText(pPathAFD))
                    {
                    }
                }
                log.Debug(nomeArquivoLog + " relogio_OnColetaEventos Chamado, Desconectando do rep");
                Desconecta();
            }
            catch (Exception e)
            {
                log.Debug(nomeArquivoLog + " Erro, relogio_OnColetaEventos Chamado, Desconectando do rep");
                Desconecta();
                log.Debug(nomeArquivoLog + " Erro, relogio_OnColetaEventos Chamado, erro = "+Util.GetExceptionMessage(e));
                throw e;
            }
        }

        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            return GetAFDNsr(dataI, dataF, 0, 0, true);
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            erros = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                Conecta(IP, Porta, TipoComunicacao, NumeroRelogio);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }

            try
            {
                if (Empregador != null)
                {
                    SEmpregador Emp = new SEmpregador()
                    {
                        IdEmpregador = Empregador.TipoDocumento == Entidades.TipoDocumento.CNPJ ? SIdEmpregador.cieCNPJ : SIdEmpregador.cieCPF,
                        CEI = GetStringSomenteAlfanumerico(Empregador.CEI),
                        Documento = GetStringSomenteAlfanumerico(Empregador.Documento),
                        Local = Empregador.Local,
                        RazaoSocial = Empregador.RazaoSocial
                    };
                    if (!relogio.get_EnviaDadosEmpregador(thread, Emp.RazaoSocial, Emp.Local, Emp.Documento, Emp.CEI, Emp.IdEmpregador))
                    {
                        throw new Exception(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)));
                    }
                }

                if (Empregados != null)
                {
                    if (Empregados.Count > 0)
                    {
                        foreach (var item in Empregados)
                        {
                            if (!relogio.get_EnviaUsuarioEquipamento(thread, item.DsCodigo, GetStringSomenteAlfanumerico(item.Pis), item.Nome, item.Biometria, SOperacaoUsuarioEquipamento.couAdicao))
                            {
                                sb.AppendLine(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)));
                            }
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    erros = sb.ToString();
                    return false;
                }
                erros = String.Empty;
                Desconecta();
                return true;
            }
            catch (Exception e)
            {
                sb.AppendLine(e.Message);
                erros += sb.ToString();
                Desconecta();
                return false;
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            erros = "";
            try
            {
                Conecta(IP, Porta, TipoComunicacao, NumeroRelogio);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }
            try
            {
                DateTime dtAtual = new DateTime();
                try
                {
                    relogio.RecebeDataHora(thread, out dtAtual);
                }
                catch (Exception e)
                {

                    throw new Exception(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)) + Environment.NewLine + e.Message);
                }
                dtAtual = dtAtual.AddSeconds(5);
                try
                {
                    relogio.get_EnviaDataHoraEx(thread, dtAtual, true, inicio, termino);
                }
                catch (Exception e)
                {

                    throw new Exception(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)) + Environment.NewLine + e.Message);
                }
                Desconecta();
                return true;
            }
            catch (Exception e)
            {
                erros += e.Message;
                Desconecta();
                return false;
            }
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            erros = "";
            try
            {
                Conecta(IP, Porta, TipoComunicacao, NumeroRelogio);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }
            try
            {
                if (!relogio.get_EnviaDataHora(thread, horario))
                {
                    throw new Exception(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)));
                }
                Desconecta();
                return true;
            }
            catch (Exception e)
            {
                erros += e.Message;
                Desconecta();
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
            erros = "";
            StringBuilder sb = new StringBuilder();
            try
            {
                Conecta(IP, Porta, TipoComunicacao, NumeroRelogio);
            }
            catch (Exception e)
            {
                erros += e.Message;
                return false;
            }

            try
            {
                if (Empregados != null)
                {
                    if (Empregados.Count > 0)
                    {
                        foreach (var item in Empregados)
                        {
                            if (!relogio.get_EnviaUsuarioEquipamento(thread, item.DsCodigo, GetStringSomenteAlfanumerico(item.Pis), item.Nome, item.Biometria, SOperacaoUsuarioEquipamento.couExclusao))
                            {
                                sb.AppendLine(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)));
                            }
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    erros = sb.ToString();
                    return false;
                }
                erros = String.Empty;
                Desconecta();
                return true;
            }
            catch (Exception e)
            {
                sb.AppendLine(e.Message);
                erros += sb.ToString();
                Desconecta();
                return false;
            }
        }
        #region Metodos Auxiliares Orion 6

        private IList<SUsuarioEquipamento> ListaFuncionariosAlteracao(IList<Entidades.Empregado> empregadosParaAlteracao, SOperacaoUsuarioEquipamento operacao)
        {
            IList<SUsuarioEquipamento> result = new List<SUsuarioEquipamento>();
            if (!relogio.get_RecebeListaUsuarioEquipamento(thread))
            {
                throw new Exception(relogio.ErrorDescription(relogio.get_ThreadLastError(thread)));
            }
            Dictionary<string, SUsuarioEquipamento> funcionariosRelogio = new Dictionary<string, SUsuarioEquipamento>();
            bool status, verificaDigital;
            SUsuarioEquipamento user = new SUsuarioEquipamento();

            relogio.Rec_UsuarioEquipamento(thread, out user.Matriculas, out user.PIS, out user.Nome, out verificaDigital, out user.TipoOperacao, out status);

            while (!String.IsNullOrEmpty(user.PIS))
            {
                if (!funcionariosRelogio.ContainsKey(user.PIS))
                {
                    funcionariosRelogio.Add(GetStringSomenteAlfanumerico(user.PIS), user);
                }
                relogio.Rec_UsuarioEquipamento(thread, out user.Matriculas, out user.PIS, out user.Nome, out verificaDigital, out user.TipoOperacao, out status);
            }

            foreach (var item in empregadosParaAlteracao)
            {
                string pis = GetStringSomenteAlfanumerico(item.Pis);
                SUsuarioEquipamento u = new SUsuarioEquipamento();
                if (funcionariosRelogio.ContainsKey(pis))
                {
                    switch (operacao)
                    {
                        case SOperacaoUsuarioEquipamento.couAdicao:
                            u = funcionariosRelogio[pis];
                            u.PIS = pis;
                            u.Nome = item.Nome;
                            u.TipoOperacao = SOperacaoUsuarioEquipamento.couAlteracao;
                            u.Matriculas = item.DsCodigo;
                            u.VerificaDigital = Convert.ToInt16(item.Biometria);
                            result.Add(u);
                            break;
                        case SOperacaoUsuarioEquipamento.couExclusao:
                            u = funcionariosRelogio[pis];
                            u.TipoOperacao = SOperacaoUsuarioEquipamento.couExclusao;
                            result.Add(u);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (operacao == SOperacaoUsuarioEquipamento.couAdicao)
                    {
                        u.PIS = pis;
                        u.Nome = item.Nome;
                        u.TipoOperacao = SOperacaoUsuarioEquipamento.couAdicao;
                        u.Matriculas = item.DsCodigo;
                        u.VerificaDigital = Convert.ToInt16(item.Biometria);
                        result.Add(u);
                    }
                }
            }
            return result;
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new Exception("Função não disponível para este modelo de relógio.");
        }

        private void Conecta(string ip, string porta, TipoComunicacao tipoComunicacao, string numeroRelogio)
        {
            try
            {
                relogio.RaiseExceptions = true;
                thread = relogio.get_AdicionaCardTcpIp(ip, "", 3000, false, SModoComunicacao.cmcOffline);
                relogio.SetConectado(thread, true);
                if (!relogio.get_RecebeConfiguracao(thread))
                {
                    string err = relogio.ErrorDescription(relogio.get_ThreadLastError(thread));
                    if (!err.ToLower().Contains("sucesso"))
                    {
                        Desconecta();
                        throw new Exception(err);
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception("Houve um erro ao comunicar com o Relógio: " + e.Message);
            }
        }

        private void Desconecta()
        {
            try
            {
                relogio.SetConectado(thread, false);
                relogio.get_RemoveCard(thread);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("Thread solicitada não existe")) // Se o erro for de Thread  inexistente não faz nada, significa que já foi fechada anteriormente.
                {
                    throw e;
                }
                
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

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportacaoHabilitada()
        {
            return false;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string nomeRelogio, out string erros, DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }


        public override bool TesteConexao()
        {
            try
            {
                Conecta(IP, Porta, TipoComunicacao, NumeroRelogio);
                Desconecta();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override int UltimoNSR()
        {
            throw new NotImplementedException();
        }
    }
}
