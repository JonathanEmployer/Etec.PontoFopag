using BLL.IntegracaoRelogio;
using CentralCliente;
using cwkPontoMT.Integracao;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Configuration;

namespace BLL_N.IntegracaoRep
{
    public class IntegracaoRepViaCentralCliente
    {
        Modelo.Cw_Usuario usuario = new Modelo.Cw_Usuario();
        public IntegracaoRepViaCentralCliente()
        {
            Modelo.cwkGlobal.objUsuarioLogado = usuario;
            usuario.Login = "ServicoComunicador";
        }

        public void EnviarComandosRep(CentralCliente.Rep repCC)
        {
            try
            {
                Utils.Utils.LogarTxt("IntegracaoRep", "Abrindo central do cliente");
                using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
                {
                    repCC = db.Rep.Find(repCC.Id);
                    repCC.Cliente = db.Cliente.Find(repCC.idCliente);
                    repCC.Cliente.ParametroServComNet = db.ParametroServComNet.Find(repCC.Cliente.IdParametroServComNet);

                    Utils.Utils.LogarTxt("IntegracaoRep", "Alterando rep adicionando data primeira importação, total de requisicoes e processando, para o rep id = " + repCC.IdRep);
                    CentralCliente.Rep alterado = db.Rep.Where(w => w.IdRep == repCC.IdRep).FirstOrDefault();
                    alterado.DataPrimeiraImportacao = alterado.DataPrimeiraImportacao == null ? DateTime.Now : alterado.DataPrimeiraImportacao;
                    alterado.totalDeRequisicoes++;
                    alterado.Processando = true;
                    db.Entry(alterado).State = EntityState.Modified;
                    Utils.Utils.LogarTxt("IntegracaoRep", "Salvando alteração rep adicionando data primeira importação, total de requisicoes e processando, para o rep id = " + repCC.IdRep);
                    db.SaveChanges();
                }

                Utils.Utils.LogarTxt("IntegracaoRep", "Pegando cliente relacionado ao rep id = " + repCC.IdRep);
                CentralCliente.Cliente cli = repCC.Cliente;
                Utils.Utils.LogarTxt("IntegracaoRep", "Pegando conexão do cliente relacionado ao rep id = " + repCC.IdRep);
                string conn = cli.CaminhoBD;
                Utils.Utils.LogarTxt("IntegracaoRep", "Descriptografando conexão do cliente relacionado ao rep id = " + repCC.IdRep);
                conn = BLL.CriptoString.Decrypt(conn);
                BLL.REP bllRep = new BLL.REP(conn, true);
                Utils.Utils.LogarTxt("IntegracaoRep", "Carregando rep do banco do cliente rep id = " + repCC.IdRep);
                Modelo.REP repCliente = bllRep.LoadObject(repCC.IdRep);
                BLL.EnvioConfiguracoesDataHora bllEnvioConfiguracoesDataHora = new BLL.EnvioConfiguracoesDataHora(conn, usuario);
                Utils.Utils.LogarTxt("IntegracaoRep", "Verificando se o rep utiliza servcom ");
                if ((repCliente.Relogio == 22) && (cli.ParametroServComNet == null || String.IsNullOrEmpty(cli.ParametroServComNet.ServidorInstancia)))
                {
                    throw new Exception("Para esse relógio é necessário configurar os parâmetros do ServComNet");
                }
                Utils.Utils.LogarTxt("IntegracaoRep", "Iniciando envio de data e hora e horário de verão rep id = " + repCC.IdRep);
                //Enviar Data/Hora e Horário de Verão
                EnvioDataHoraEHorarioVerao(repCC, conn, bllEnvioConfiguracoesDataHora);
                Utils.Utils.LogarTxt("IntegracaoRep", "Iniciando envio de Empresa e Funcionário rep id = " + repCC.IdRep);
                //Enviar Empresa e Funcionário
                BLL.EnvioDadosRepDet bllEnvioDadosRepDet = new BLL.EnvioDadosRepDet(conn, usuario);
                EnvioEmpresaEFuncionario(repCC, conn, repCliente, bllEnvioDadosRepDet);

                Utils.Utils.LogarTxt("IntegracaoRep", "Iniciando coleta rep id = " + repCC.IdRep);
                //Coletar
                ColetaBilhetes coleta = new ColetaBilhetes(repCliente, conn, usuario);
                coleta.ImportarAFDRep();

                Utils.Utils.LogarTxt("IntegracaoRep", "Atualizando rep id = " + repCC.IdRep + " na central do cliente");
                //Atualzar o Rep na central do cliente
                AtualizarRepCentralCliente(repCC, repCliente, bllEnvioConfiguracoesDataHora, bllEnvioDadosRepDet);
            }
            catch (Exception e)
            {
                using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
                {
                    CentralCliente.Rep alterado = db.Rep.Where(w => w.IdRep == repCC.IdRep).FirstOrDefault();
                    alterado.Processando = false;
                    db.Entry(alterado).State = EntityState.Modified;
                    db.SaveChanges();
                }
                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de Comandos", 1, e.Message, "Comunicador", e.StackTrace.ToString());
                throw e;
            }
        }

        private static void AtualizarRepCentralCliente(CentralCliente.Rep repCC, Modelo.REP repCliente, BLL.EnvioConfiguracoesDataHora bllEnvioConfiguracoesDataHora, BLL.EnvioDadosRepDet bllEnvioDadosRepDet)
        {
            IList<EnvioConfiguracoesDataHora> ev = bllEnvioConfiguracoesDataHora.LoadListByIDRep(repCliente.Id);
            IList<Modelo.EnvioDadosRepDet> envsDetSobra = bllEnvioDadosRepDet.getListByRep(repCliente.Id);
            using (CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities())
            {
                CentralCliente.Rep alterado = db.Rep.Where(w => w.IdRep == repCC.IdRep).FirstOrDefault();
                if (ev != null)
                {
                    alterado.temDataHoraExportar = ev.Where(w => w.bEnviaDataHoraServidor).Count() > 0 ? true : false;
                    alterado.temHorarioVeraoExportar = ev.Where(w => w.bEnviaHorarioVerao).Count() > 0 ? true : false;
                }
                else
                {
                    alterado.temDataHoraExportar = false;
                    alterado.temHorarioVeraoExportar = false;
                }

                if (envsDetSobra != null)
                {
                    alterado.temFuncionarioExportar = envsDetSobra.Where(w => w.idFuncionario != null).Count() > 0 ? true : false;
                    alterado.temEmpresaExportar = envsDetSobra.Where(w => w.idEmpresa != null).Count() > 0 ? true : false;
                }
                else
                {
                    alterado.temFuncionarioExportar = false;
                    alterado.temEmpresaExportar = false;
                }
                alterado.dataUltimaImportacao = DateTime.Now;
                alterado.dataUltimaExportacao = DateTime.Now;
                alterado.Processando = false;
                db.Entry(alterado).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void EnvioDataHoraEHorarioVerao(CentralCliente.Rep repCC, string conn, BLL.EnvioConfiguracoesDataHora bllEnvioConfiguracoesDataHora)
        {
            if (repCC.temDataHoraExportar || repCC.temHorarioVeraoExportar)
            {
                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de Data/Hora e Horario de verão", 2, "Verificando Data e hora ou horario de verão para enviar", "Comunicador", "");
                EnvioConfiguracoesDataHora envConf = bllEnvioConfiguracoesDataHora.LoadListByIDRep(repCC.IdRep).Where(w => w.bEnviaDataHoraServidor == true).OrderBy(o => o.Inchora).FirstOrDefault();
                if (repCC.temDataHoraExportar)
                {
                    bool retorno = true;
                    if (envConf != null && envConf.bEnviaDataHoraServidor)
                    {
                        cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de Data/Hora", 2, "Enviando Data e hora = " + DateTime.Now, "Comunicador", "");
                        ConfiguracaoHorario ch = new ConfiguracaoHorario(repCC.IdRep, conn, usuario, envConf.Inchora.GetValueOrDefault());
                        bool exibirLog = true;
                        ch.EnviarDataHoraComputador = true;
                        ch.SetDataHoraAtual(DateTime.Now);
                        retorno = ch.Enviar(out exibirLog);
                    }
                    if (retorno)
                    {
                        cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de Data/Hora", 2, "Limpando comando de data e hora a enviar", "Comunicador", "");
                        if (envConf != null && envConf.bEnviaDataHoraServidor)
                        {
                            bllEnvioConfiguracoesDataHora.Salvar(Acao.Excluir, envConf);
                        }
                    }
                }

                envConf = bllEnvioConfiguracoesDataHora.LoadListByIDRep(repCC.IdRep).Where(w => w.bEnviaHorarioVerao == true).OrderBy(o => o.Inchora).FirstOrDefault();
                if (repCC.temHorarioVeraoExportar)
                {
                    bool retorno = true;
                    if (envConf != null && envConf.bEnviaHorarioVerao)
                    {
                        cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de Horário de Verão", 2, "Enviando Horario Verão", "Comunicador", "");
                        ConfiguracaoHorario ch = new ConfiguracaoHorario(repCC.IdRep, conn, usuario, envConf.Inchora.GetValueOrDefault());
                        BLL.EnvioConfiguracoesDataHora bllDT = new BLL.EnvioConfiguracoesDataHora(conn, usuario);
                        Modelo.EnvioConfiguracoesDataHora envDT = bllDT.LoadListByIDRep(repCC.IdRep).Where(w => w.bEnviaHorarioVerao == true).OrderBy(o => o.Inchora).FirstOrDefault();
                        bool exibirLog = true;
                        ch.SetHorarioVerao(envDT.dtInicioHorarioVerao, envDT.dtFimHorarioVerao);
                        retorno = ch.Enviar(out exibirLog);
                    }
                    if (retorno)
                    {
                        cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de Horário de Verão", 2, "Limpando comando de horario de verão a enviar", "Comunicador", "");
                        if (envConf != null && envConf.bEnviaHorarioVerao)
                        {
                            bllEnvioConfiguracoesDataHora.Salvar(Acao.Excluir, envConf);
                        }
                    }
                }
            }
        }

        private void EnvioEmpresaEFuncionario(CentralCliente.Rep repCC, string conn, Modelo.REP repCliente, BLL.EnvioDadosRepDet bllEnvioDadosRepDet)
        {
            if (repCC.temEmpresaExportar || repCC.temFuncionarioExportar)
            {
                BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usuario);
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usuario);
                BLL.EnvioDadosRep bllEnvioDadosRep = new BLL.EnvioDadosRep(conn, usuario);
                IList<Modelo.EnvioDadosRepDet> envsDet = bllEnvioDadosRepDet.getListByRep(repCliente.Id);

                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de dados para o Rep", 2, "Preparando o envio de " + envsDet.GroupBy(g => g.idEnvioDadosRep).Count() + " Envios encontrados", "Comunicador", "");
                foreach (IList<Modelo.EnvioDadosRepDet> item in envsDet.GroupBy(g => g.idEnvioDadosRep))
                {
                    Modelo.EnvioDadosRep envDados = bllEnvioDadosRep.LoadObject(item.Max(s => s.idEnvioDadosRep));
                    Modelo.Empresa objEmpresa = new Modelo.Empresa();
                    List<Modelo.Funcionario> listaFuncionarios = new List<Modelo.Funcionario>();

                    if (repCC.temEmpresaExportar && item.Max(s => s.idEmpresa) != null)
                        objEmpresa = bllEmpresa.LoadObject(item.Max(s => s.idEmpresa).GetValueOrDefault());

                    if (repCC.temFuncionarioExportar && item.Select(s => s.idFuncionario != null).Count() > 0)
                    {
                        string idsFuncs = String.Join(",", item.Where(w => w.idFuncionario != null).Select(s => s.idFuncionario));
                        listaFuncionarios = bllFuncionario.GetAllListByIds("(" + idsFuncs + ")");
                    }

                    string msg = "Enviando ";
                    if (objEmpresa.Id > 0)
                    {
                        msg += " empresa codigo = " + objEmpresa.Codigo + " ";
                    }
                    msg += "Dscodigo Funcionários = " + String.Join(", ", listaFuncionarios.Select(s => s.Dscodigo));
                    cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de dados para o Rep", 2, "Enviado dados para o relógio", "Comunicador", msg);

                    ComunicacaoRelogio envioFuncionarios = null;
                    if (envDados.bOperacao == 1)
                    {
                        envioFuncionarios = new BLL.IntegracaoRelogio.ExcluiFuncionariosRelogio(objEmpresa, listaFuncionarios, repCliente.Id, conn, usuario);
                    }
                    else
                    {
                        envioFuncionarios = new BLL.IntegracaoRelogio.EnvioEmpresaEFuncionarios(objEmpresa, listaFuncionarios, repCliente.Id, conn, usuario);
                    }
                    bool exibirLog = false;
                    if (envioFuncionarios.Enviar(out exibirLog))
                    {
                        cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de dados para o Rep", 0, "Dados Enviado com sucesso", "Comunicador", "Empresa codigo = " + objEmpresa.Codigo + " Dscodigo Funcionário = " + String.Join(", ", listaFuncionarios.Select(s => s.Codigo)));

                        foreach (Modelo.EnvioDadosRepDet EnvioDadosRepDetExc in item)
                        {
                            bllEnvioDadosRepDet.Salvar(Modelo.Acao.Excluir, EnvioDadosRepDetExc);
                        }
                        int IdEnvioDadosRep = item.Max(m => m.idEnvioDadosRep);

                        Modelo.EnvioDadosRep edr = bllEnvioDadosRep.LoadObject(IdEnvioDadosRep);
                        bllEnvioDadosRep.Salvar(Modelo.Acao.Excluir, edr);
                        cwkPontoMT.Integracao.Util.GravaLogCentralCliente(repCC.numSerie, "Envio de dados para o Rep", 0, "Removido da fila os dados enviados", "Comunicador", "");
                    }
                }
            }
        }
    }
}
