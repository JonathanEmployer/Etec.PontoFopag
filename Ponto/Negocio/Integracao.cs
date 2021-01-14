using cwkPontoMT.Integracao;
using ModeloAux;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio
{
    public class Integracao
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessarRep(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            try
            {
                config = Configuracao.GetConfiguracao();
                IList<ModeloAux.RepViewModel> reps = Negocio.Rep.GetRepConfig(config);
                rep = reps.Where(w => w.Id == rep.Id).FirstOrDefault();
                Int64 intervaloUltimaIntegracao = Convert.ToInt64((DateTime.Now - rep.UltimaIntegracao.GetValueOrDefault()).TotalSeconds);
                string nomeLog = rep.NumSerie;
                log.Info(nomeLog + " Iniciando nova integração do rep num série: " + rep.NumSerie + " modelo: " + rep.NomeModelo + " intervalo entre a ultima integracao em segundos = " + intervaloUltimaIntegracao + " intervalo de espera do rep = " + rep.TempoRequisicao);

                if (!rep.UltimaIntegracao.HasValue || intervaloUltimaIntegracao >= rep.TempoRequisicao)
                {
                    try
                    {
                        Log.EnviarLogApi(rep, "Interação Rep", "Nova integração iniciada", "", Modelo.Enumeradores.SituacaoLog.Informacao);
                        log.Info(nomeLog + " Nova integração iniciada");
                        EnviarComandosRep(rep, config);
                    }
                    catch (Exception e)
                    {
                        string detalhe = e.InnerException == null ? " " : " detalhes = " + e.InnerException.Message.ToString();
                        log.Error(nomeLog + " Erro na integração, erro = " + e.Message + detalhe);
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                Log.EnviarLogApi(rep, "Interação Rep", "Erro: " + e.GetFullMessage(), "Detalhe: " + e.StackTrace.ToString(), Modelo.Enumeradores.SituacaoLog.Erro);
                throw e;
            }
            finally
            {
                log.Info("Finalizando integração do rep num série: " + rep.NumSerie + " modelo: " + rep.NomeModelo);
            }
        }

        public void EnviarComandosRep(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            try
            {
                Rep.SetarRepProcessando(rep, true);
                //Desenvolver lógica para setar o rep como processando nesse momento
                if ((rep.NumModeloRelogio == 22) && (String.IsNullOrEmpty(config.InstanciaServCom) || String.IsNullOrEmpty(config.DataBaseServCom) || String.IsNullOrEmpty(config.UsuarioServCom) || String.IsNullOrEmpty(config.SenhaServCom)))
                {
                    throw new Exception("Para esse relógio é necessário configurar os parâmetros do ServComNet");
                }

                if ((rep.NumModeloRelogio == 23) && (String.IsNullOrEmpty(config.InstanciaTelematica) || String.IsNullOrEmpty(config.DataBaseTelematica) || String.IsNullOrEmpty(config.UsuarioTelematica) || String.IsNullOrEmpty(config.SenhaTelematica) || String.IsNullOrEmpty(config.LocalArquivoTelematica)))
                {
                    throw new Exception("Para esse relógio é necessário configurar os parâmetros do ConexRep");
                }

                //Enviar Data/Hora e Horário de Verão
                EnvioDataHoraEHorarioVerao(rep, config);

                //Enviar Empresa e Funcionário
                EnviarEmpresaEFuncionario(rep, config);

                //Coletar
                ColetaAFD coleta = new ColetaAFD(rep, config, DateTime.Now);
                List<RegistroAFD> registros = coleta.ImportarAFDRep();
                log.Info(rep.NumSerie + " Total de registros coletados: " + registros.Count);
                if (registros.Where(w => w.Campo01 != "000000000" && w.Campo01 != "999999999").Count() > 0)
                {
                    log.Info(rep.NumSerie + " Enviando AFD para o Servidor");
                    ResultadoImportacao result = coleta.EnviarAfdServidor(registros);
                    log.Info(rep.NumSerie + " Enviado AFD para o servidor");
                }
                log.Info(rep.NumSerie + " Setando o Processar como False");
                Rep.SetarRepProcessando(rep, false);
            }
            catch (Exception e)
            {
                Rep.SetarRepProcessando(rep, false);
                log.Error(rep.NumSerie + " Erro ao enviar comando, erro: " + e.Message);
                throw (e);
            }
        }

        private void EnvioDataHoraEHorarioVerao(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            if (rep.EnviarDataHora || rep.EnviarHorarioVerao)
            {
                ComunicacaoApi comApi = new ComunicacaoApi(config.TokenAccess);
                List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora> envConf = comApi.GetConfiguracoesDataHoraAsync(new List<int> { rep.Id }).Result;
                try
                {
                    if (rep.EnviarDataHora)
                    {
                        Modelo.Proxy.PxyEnvioConfiguracoesDataHora envDataHora = envConf.Where(w => w.bEnviaDataHoraServidor == true).OrderBy(o => o.Inchora).FirstOrDefault();
                        bool retorno = true;
                        if (envDataHora != null && envDataHora.bEnviaDataHoraServidor)
                        {
                            Log.EnviarLogApi(rep, "Envio de Data/Hora", "Enviando Data e hora = " + DateTime.Now, "", Modelo.Enumeradores.SituacaoLog.Informacao);
                            ConfiguracaoHorario ch = new ConfiguracaoHorario(rep, config, envDataHora.Inchora.GetValueOrDefault());
                            ch.EnviarDataHoraComputador = true;
                            ch.SetDataHoraAtual(DateTime.Now);
                            retorno = ch.Enviar();
                            if (retorno)
                            {
                                Dictionary<int, string> retDel = comApi.DeletaDadosConfigDataHora(new List<int> { envDataHora.Id }).Result;
                                Log.EnviarLogApi(rep, "Envio de Data/Hora", "Comando enviado com sucesso", "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Adicionado na fila"))
                    {
                        Log.EnviarLogApi(rep, "Envio de Data/Hora", ex.Message, "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                    }
                    else
                    {
                        Log.EnviarLogApi(rep, "Envio de Data/Hora", "Erro ao enviar comando, Erro: " + ex.Message, " Detalhes: " + ex.StackTrace, Modelo.Enumeradores.SituacaoLog.Erro);
                    }
                }

                try
                {
                    if (rep.EnviarHorarioVerao)
                    {
                        Modelo.Proxy.PxyEnvioConfiguracoesDataHora envHorarioVerao = envConf.Where(w => w.bEnviaHorarioVerao == true).OrderBy(o => o.Inchora).FirstOrDefault();
                        bool retorno = true;
                        if (envHorarioVerao != null && envHorarioVerao.bEnviaHorarioVerao)
                        {
                            Log.EnviarLogApi(rep, "Envio de Horário de Verão", "Envio de Horário de Verão, data inicio =  " + envHorarioVerao.dtInicioHorarioVerao.GetValueOrDefault().ToString("dd/MM/yyyy") + ", data fim = " + envHorarioVerao.dtFimHorarioVerao.GetValueOrDefault().ToString("dd/MM/yyyy"), "", Modelo.Enumeradores.SituacaoLog.Informacao);
                            ConfiguracaoHorario ch = new ConfiguracaoHorario(rep, config, envHorarioVerao.Inchora.GetValueOrDefault());
                            ch.SetHorarioVerao(envHorarioVerao.dtInicioHorarioVerao, envHorarioVerao.dtFimHorarioVerao);
                            retorno = ch.Enviar();
                        }
                        if (retorno)
                        {
                            Dictionary<int, string> retDel = comApi.DeletaDadosConfigDataHora(new List<int> { envHorarioVerao.Id }).Result;
                            Log.EnviarLogApi(rep, "Envio de Horário de Verão", "Comando enviado com sucesso", "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.EnviarLogApi(rep, "Envio de Horário de Verão", "Erro ao enviar comando, Erro: " + ex.Message, " Detalhes: " + ex.StackTrace, Modelo.Enumeradores.SituacaoLog.Erro);
                }
            }
        }

        private void EnviarEmpresaEFuncionario(ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            if (rep.EnviarEmpresa || rep.EnviarFuncionario)
            {
                ComunicacaoApi comApi = new ComunicacaoApi(config.TokenAccess);
                List<ImportacaoDadosRep> envConf = comApi.GetEmpFuncExpRepAsync(new List<int> { rep.Id }).Result;
                bool retorno = true;
                bool EmpregadorFuncionario = false;

                try
                {
                    foreach (ImportacaoDadosRep enviar in envConf)
                    {
                        rep.TipoBiometria = enviar.Rep.TipoBiometria;
                        EmpregadorFuncionario = false;
                        if (enviar.EnvioDadosRep.TipoComunicacao != "R")
                        {
                            if (enviar.EnvioDadosRep.TipoComunicacao != "E")
                            {
                                EmpregadorFuncionario = true;
                                Log.EnviarLogApi(rep, "Envio de Empresa/Funcionários", "Enviando/Excluindo dados de Empresa/Funcionários", "", Modelo.Enumeradores.SituacaoLog.Informacao);
                            }
                            else
                                Log.EnviarLogApi(rep, "Envio de Biometrias", "Enviando dados de Biometria", "", Modelo.Enumeradores.SituacaoLog.Informacao);

                            EnvioEmpresaEFuncionarios envEmpFunc = new EnvioEmpresaEFuncionarios(rep, config, enviar.EnvioDadosRep.Inchora.GetValueOrDefault(), enviar);
                            retorno = envEmpFunc.Enviar();
                        }
                        if (enviar.EnvioDadosRep.TipoComunicacao == "R")
                        {
                            Log.EnviarLogApi(rep, "Recebimento de Biometrias", "Recebendo dados de Biometria", "", Modelo.Enumeradores.SituacaoLog.Informacao);
                            EnvioEmpresaEFuncionarios envEmpFunc = new EnvioEmpresaEFuncionarios(rep, config, enviar.EnvioDadosRep.Inchora.GetValueOrDefault(), enviar);
                            retorno = envEmpFunc.Receber(comApi);
                        }
                        if (retorno)
                        {
                            Dictionary<int, string> retDel = comApi.DeletaEmpFuncExpRep(new List<int> { enviar.EnvioDadosRep.Id }).Result;
                        }
                    }
                    if (EmpregadorFuncionario)
                        Log.EnviarLogApi(rep, "Envio de Empresa/Funcionários", "Comando enviado com sucesso", "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                    else
                        Log.EnviarLogApi(rep, "Envio/Recebimento de Biometria", "Comando enviado com sucesso", "", Modelo.Enumeradores.SituacaoLog.Sucesso);
                }
                catch (Exception ex)
                {
                    if (EmpregadorFuncionario)
                        Log.EnviarLogApi(rep, "Envio de Empresa/Funcionários", "Erro ao enviar comando, Erro: " + ex.Message, " Detalhes: " + ex.StackTrace, Modelo.Enumeradores.SituacaoLog.Erro);
                    else
                        Log.EnviarLogApi(rep, "Envio/Recebimento de Biometria", "Erro ao enviar comando, Erro: " + ex.Message, " Detalhes: " + ex.StackTrace, Modelo.Enumeradores.SituacaoLog.Erro);
                }
            }
        }
    }
}
