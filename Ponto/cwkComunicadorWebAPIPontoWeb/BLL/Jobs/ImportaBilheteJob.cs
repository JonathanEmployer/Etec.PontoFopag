using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Job;
using Quartz.Impl;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using cwkPontoMT.Integracao;
using cwkPontoMT.Integracao.Entidades;
using System.IO;
using System.Reflection;
using cwkComunicadorWebAPIPontoWeb.Utils;
using System.Threading;
using Microsoft;
using System.Net;

namespace cwkComunicadorWebAPIPontoWeb.BLL.Jobs
{
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class ImportaBilheteJob : IInterruptableJob
    {
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;
        private Progress<ReportaErro> progresso;
        public ImportaBilheteJob()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
        }
        public async void Execute(IJobExecutionContext context)
        {
            bool mostraMensagem = false;
            JobDataMap map = context.JobDetail.JobDataMap;
            if (map.ContainsKey("Progress"))
            {
                progresso = (Progress<ReportaErro>)map.Get("Progress");
            }
            RepViewModel rep = (RepViewModel)map.Get("Rep");
            DateTime iniImportacao;
            if (!GetRepProcessando(rep, out iniImportacao))
            {
                try
                {
                    SetRepProcessando(rep, true, progresso);
                    if (rep.NumModeloRelogio == 2) // Para o Orion 6 valida apenas o ping, para evitar de ficar utilizando a DLL Kernel e evitar problemas. (Portanto para o orion 6 deve sempre estar permitindo o ping.)
                    {
                        ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Verificando conexão com o Rep Orion 6" + " Job: " + context.JobDetail.Description }, progresso);
                        if (!(await CwkUtils.EnderecoDisponivel(rep.Ip)))
                        {
                            throw new Exception("Não foi possível conectar-se ao rep.");
                        }
                    }
                    string nomeRep;
                    ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Iniciando rotina de comunicação com o REP" + " Job: " + context.JobDetail.Description }, progresso);
                    Relogio relogio = GetRelogioRep(rep, out nomeRep);
                    if (await BLL.Workers.EnviaBilheteWorker.LancadorArquivosExportacao(CwkUtils.FileLogStringUtil("AFDsImportados"), progresso, rep.Id, context.JobDetail.Description))
                    {

                        if (map.ContainsKey("Url") && map.ContainsKey("UserData"))
                        {
                            string url = map.Get("Url").ToString();
                            TokenResponseViewModel userData = (TokenResponseViewModel)map.Get("UserData");

                            if (!String.IsNullOrEmpty(url) && userData != null)
                            {
                                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Verificando se existe dados de empresa, funcionário(s) e data/hora a serem enviados ao rep," + " Job: " + context.JobDetail.Description }, progresso);
                                ImportacaoDadosRepBLL impBll = new ImportacaoDadosRepBLL();
                                await impBll.EnviarEmpresaFuncionarios(new List<RepViewModel>() { rep }, userData.AccessToken, url, ct, progresso);

                                ImportacaoConfigDataHoraBLL icdhBLL = new ImportacaoConfigDataHoraBLL();
                                await icdhBLL.EnviarConfigDataHora(new List<RepViewModel>() { rep }, userData.AccessToken, ct, progresso, false);
                            }
                            else
                            {
                                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Erro, Mensagem = "(Job) Não foi possível recuperar o usuário logado no aplicativo" + " Job: " + context.JobDetail.Description }, progresso);
                            }

                            ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Verificando endereço do rep, ip = " + rep.Ip + " Job: " + context.JobDetail.Description }, progresso);
                            if (!await CwkUtils.EnderecoDisponivel(rep.Ip))
                            {
                                try
                                {
                                    if (rep.NumeroTentativasComunicacao < 3 || rep.DataUltimaTentativa <= DateTime.Now.AddHours(-1))
                                    {
                                        RepBLL bllrep = new RepBLL();
                                        ListaRepsViewModel reps = await bllrep.GetAllRepsAsync(userData.Username, userData.AccessToken, url, ct, progresso);

                                        foreach (var repOn in reps.Reps)
                                        {
                                            if (repOn.Id == rep.Id)
                                            {
                                                repOn.DataUltimaTentativa = DateTime.Now;
                                                if (repOn.NumeroTentativasComunicacao >= 3)
                                                {
                                                    repOn.NumeroTentativasComunicacao = 0;
                                                }
                                                else
                                                {
                                                    repOn.NumeroTentativasComunicacao++;
                                                }
                                            }
                                        }

                                        RepViewModel repAtualizado = reps.Reps.FirstOrDefault(f => f.Id == rep.Id);
                                        if (repAtualizado != null)
                                        {
                                            rep = repAtualizado;
                                        }

                                        while (!await bllrep.SetXmlRegisterData(reps))
                                        {
                                            Thread.Sleep(500);
                                        }

                                    }
                                }
                                catch (Exception w)
                                {
                                    throw w;
                                }
                            }
                            try
                            {   //Verifica conexão com o rep
                                if (rep.NumModeloRelogio != 2) // Para o Orion 6 valida apenas o ping, para evitar de ficar utilizando a DLL Kernel e evitar problemas. (Portanto para o orion 6 deve sempre estar permitindo o ping.)
                                {
                                    ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Verificando conexão com o Rep" + " Job: " + context.JobDetail.Description }, progresso);
                                    if (rep.NumModeloRelogio == 1 || rep.NumModeloRelogio == 10 || rep.NumModeloRelogio == 4 || rep.NumModeloRelogio == 21)
                                    {
                                        bool online = relogio.TesteConexao();
                                        if (!online)
                                        {
                                            throw new Exception("Não foi possível conectar-se ao rep.");
                                        }
                                    }
                                    else
                                    {
                                        var res2 = relogio.GetAFD(DateTime.Now.Date, DateTime.Now);
                                        int x = res2.Count;
                                        x++;
                                    }
                                }
                                else
                                {
                                    if (!(await CwkUtils.EnderecoDisponivel(rep.Ip)))
                                    {
                                        throw new Exception("Não foi possível conectar-se ao rep.");
                                    }
                                }
                            }
                            catch (Exception ez)
                            {
                                throw ez;
                            }
                        }

                        if (map.ContainsKey("MostraMensagem"))
                        {
                            mostraMensagem = Convert.ToBoolean(map["MostraMensagem"]);
                        }

                        ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Definindo dados para coleta: " + context.JobDetail.Description }, progresso);
                        List<RegistroAFD> registros = new List<RegistroAFD>();

                        if (rep.UltimoNsr > 0)
                        {
                            ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Iniciando coleta: NSR Inicial: " + rep.UltimoNsr + " Job: " + context.JobDetail.Description }, progresso);
                            registros = relogio.GetAFDNsr(rep.DataUltimoNsr, DateTime.Now, rep.UltimoNsr, int.MaxValue, true);
                        }
                        else
                            // Se ainda não existe um nsr inicial e foi especificado uma data para iniciar a coleta, busca os registros pela data.
                            if (rep.UltimoNsr == 0 && rep.DataInicioImportacao != null)
                            {
                                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Iniciando coleta: Data Inicial: " + rep.DataInicioImportacao + " Job: " + context.JobDetail.Description }, progresso);
                                registros = relogio.GetAFDNsr(rep.DataInicioImportacao, DateTime.Now, 0, 0, true);
                            }
                            else // Se não tem NSR inicial e também não foi especificada uma data, busca todos os registros do ultimo mês apenas
                            {
                                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Iniciando coleta: NSR Inicial ou Data Inicial não foram informados, iniciando de: " + DateTime.Now.AddMonths(-1) + " Job: " + context.JobDetail.Description }, progresso);
                                registros = relogio.GetAFDNsr(DateTime.Now.AddMonths(-1), DateTime.Now, 0, 0, true);
                            }

                        //Só envia se existir linhas com nsr maior que o último se contar o cabeçalho e o Trailer
                        int qtdRegistros = registros.Where(w => w.Nsr > rep.UltimoNsr && w.Campo01 != "000000000" && w.Campo01 != "999999999" && w.Campo02 != "6").Count();
                        if (qtdRegistros > 0)
                        {
                            //Mantém apenas o Cabeçalho e os Registros com NSR maior que ultimo coletado
                            registros = registros.Where(w => (w.Nsr > rep.UltimoNsr && w.Campo02 != "6") || w.Campo01 == "000000000").ToList();
                            registros = registros.OrderBy(o => o.Nsr).ToList();
                            //registros.Add(regHeader);
                            rep.UltimoNsr = registros.Where(w => w.Nsr > 0 && w.Campo01 != "999999999").Max(m => m.Nsr);
                            DateTime dtUltimoNSR = DateTime.Now;
                            RegistroAFD reg = registros.Where(w => w.Nsr == rep.UltimoNsr).FirstOrDefault();
                            if (reg != null)
                            {
                                string strDtUltimoNSR = String.Empty;
                                if (reg.Campo02 == "2")
                                {
                                    strDtUltimoNSR = reg.Campo03.Substring(0, 2) + "/" + reg.Campo03.Substring(2, 2) + "/" + reg.Campo03.Substring(4, 4);
                                    strDtUltimoNSR += " " + reg.Campo04.Substring(0, 2) + ":" + reg.Campo04.Substring(2, 2);
                                }
                                else
                                {
                                    strDtUltimoNSR = reg.Campo04.Substring(0, 2) + "/" + reg.Campo04.Substring(2, 2) + "/" + reg.Campo04.Substring(4, 4);
                                    strDtUltimoNSR += " " + reg.Campo05.Substring(0, 2) + ":" + reg.Campo05.Substring(2, 2);
                                }
                                if (DateTime.TryParse(strDtUltimoNSR, out dtUltimoNSR))
                                {
                                    rep.DataUltimoNsr = dtUltimoNSR;
                                }
                            }
                            ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Registros coletados: " + qtdRegistros + " Último NSR coletado: " + rep.UltimoNsr + ", data e hora do NSR: " + rep.DataUltimoNsr + " Job: " + context.JobDetail.Description }, progresso);
                            string pathLogImport = CwkUtils.FileLogStringUtil("AFDsImportados");
                            if (String.IsNullOrEmpty(pathLogImport))
                            {
                                string erro = "Não foi possível criar diretório de Importação de bilhetes";
                                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Erro, Mensagem = "(Job) Erro: " + erro + " Job: " + context.JobDetail.Description }, progresso);
                                throw new Exception(erro);
                            }
                            string nomeAfd = "AFD_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + "_" + nomeRep + "_IDREP_" + rep.Id.ToString() + ".txt";
                            pathLogImport = Path.Combine(pathLogImport, nomeAfd);
                            try
                            {
                                using (StreamWriter file = new StreamWriter(pathLogImport, true))
                                {
                                    foreach (var item in registros)
                                    {
                                        file.WriteLine(item.LinhaAFD);
                                    }
                                }
                                context.JobDetail.JobDataMap["Rep"] = rep;
                            }
                            catch (Exception e)
                            {

                                throw e;
                            }
                        }
                        else
                        {
                            ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Não foram encontrados novos registros. Último NSR coletado: " + rep.UltimoNsr + " Job: " + context.JobDetail.Description }, progresso);
                        }
                        if (mostraMensagem)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Dados Sincronizados com sucesso!", "Pontofopag Comunicador",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception e)
                {
                    ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Erro, Mensagem = "(Job) Erro: " + e.Message + " Job: " + context.JobDetail.Description }, progresso);
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "Log_Importação_de_bilhetes" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions(context.JobDetail.Description, e, filePath);
                    if (mostraMensagem)
                    {
                        System.Windows.Forms.MessageBox.Show(
                            "Houve um erro ao realizar a comunicação. Verifique informações no arquivo de log disponível em "
                            + filePath, "Pontofopag Comunicador", System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    SetRepProcessando(rep, false, progresso);
                }
            }
            else
            {
                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Aviso, Mensagem = "Existe um outro processo em andamento iniciado em " + iniImportacao.ToString("dd/MM/yyyy HH:mm") + " para o rep " + rep.DescRelogio + " Número de Série: " + rep.NumSerie }, progresso);
            }
        }

        private static void SetRepProcessando(RepViewModel rep, bool processando, IProgress<ReportaErro> progresso)
        {
            try
            {
                if (VariaveisGlobais.LRepProcessando == null)
                {
                    VariaveisGlobais.LRepProcessando = new List<RepProcessando>();
                }
                RepProcessando rp = VariaveisGlobais.LRepProcessando.Where(w => w.RepVM.NumSerie == rep.NumSerie).FirstOrDefault();
                if (rp == null)
                {
                    rp = new RepProcessando();
                    rp.RepVM = rep;
                    rp.Processando = processando;
                    SetDataProcessamentoRep(processando, rp, progresso);
                    VariaveisGlobais.LRepProcessando.Add(rp);
                }
                else
                {
                    SetDataProcessamentoRep(processando, rp, progresso);
                    rp.Processando = processando;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void SetDataProcessamentoRep(bool processando, RepProcessando rp, IProgress<ReportaErro> progresso)
        {
            ReportaErro reportErro = new ReportaErro();
            if (processando)
            {
                rp.InicioImportacao = DateTime.Now;
                reportErro = new ReportaErro() { TipoMsg = TipoMensagem.Aviso, Mensagem = "Iniciando Coleta de Bilhetes em " + rp.InicioImportacao.ToString("dd/MM/yyyy HH:mm") + " para o Rep " + rp.RepVM.DescRelogio + " Número de Série: " + rp.RepVM.NumSerie };
            }
            else
            {
                rp.FimImportacao = DateTime.Now;
                reportErro = new ReportaErro() { TipoMsg = TipoMensagem.Aviso, Mensagem = "Finalizando Coleta de Bilhetes em " + rp.FimImportacao.ToString("dd/MM/yyyy HH:mm") + " que foi iniciada em " + rp.InicioImportacao.ToString("dd/MM/yyyy HH:mm") + " para o Rep " + rp.RepVM.DescRelogio + " Número de Série: " + rp.RepVM.NumSerie };
            }
            if (progresso != null)
            {
                progresso.Report(reportErro);
            }
        }
        private static bool GetRepProcessando(RepViewModel rep, out DateTime dataInicioImportacao)
        {
            if (VariaveisGlobais.LRepProcessando != null)
            {
                RepProcessando rp = VariaveisGlobais.LRepProcessando.Where(w => w.RepVM.NumSerie == rep.NumSerie).FirstOrDefault();
                if (rp != null)
                {
                    dataInicioImportacao = rp.InicioImportacao;
                    // Se o equipamento ficar mais de 2 horas como "processando" libero para um novo processamento setando como false.
                    if ((DateTime.Now - dataInicioImportacao).TotalHours > 2)
                    {
                        rp.Processando = false;
                    }
                    return rp.Processando;
                }
            }
            dataInicioImportacao = DateTime.Now;
            return false;
        }


        private static Relogio GetRelogioRep(RepViewModel rep, out string nomeRep)
        {
            Relogio relogio = RelogioFactory.GetRelogio((TipoRelogio)rep.NumModeloRelogio);


            if (rep.TipoIP == 1)
            {
                IPHostEntry hostEntry;
                hostEntry = Dns.GetHostEntry(rep.Ip);

                if (hostEntry.AddressList.Length > 0)
                {
                    rep.Ip = hostEntry.AddressList[0].ToString();
                }
            }

            relogio.SetDados(rep.Ip, rep.Porta, rep.SenhaComunicacao, (TipoComunicacao)rep.TipoComunicacao, rep.NumRelogio, rep.EnderecoEmpregador);

            Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
            empregador.RazaoSocial = rep.NomeEmpregador;

            empregador.Documento = new string(rep.CpfCnpjEmpregador.ToCharArray().Where((c => char.IsLetterOrDigit(c))).ToArray());
            nomeRep = new string(rep.NomeModelo.ToCharArray().Where((c => char.IsLetterOrDigit(c))).ToArray());
            empregador.TipoDocumento = empregador.Documento.Length > 11 ? TipoDocumento.CNPJ : TipoDocumento.CPF;
            empregador.CEI = rep.CEI;
            empregador.Local = rep.EnderecoEmpregador;
            relogio.SetEmpresa(empregador);
            relogio.SetNumeroSerie(rep.NumSerie);
            relogio.Senha = rep.SenhaComunicacao;
            if (rep.EquipamentoHomologadoInmetro)
            {
                relogio.Cpf = rep.CpfRepDec;
                relogio.UsuarioREP = rep.LoginRepDec;
                relogio.SenhaUsuarioREP = rep.SenhaRepDec;
            }
            return relogio;
        }

        public void Interrupt()
        {
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        private void ReportaProgresso(ReportaErro info, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(info);
            }
        }
    }
}
