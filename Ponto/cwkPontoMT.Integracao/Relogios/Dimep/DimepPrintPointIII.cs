using cwkPontoMT.Integracao.Entidades;
using cwkPontoMT.Integracao.Relogios.Dimep.UtilsDimep;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Dimep
{
    class DimepPrintPointIII : Relogio
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override List<RegistroAFD> GetAFD(DateTime dataI, DateTime dataF)
        {
            List<RegistroAFD> regs = new List<RegistroAFD>();
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, DateTime.Now, Conn);
            string comando = "Importar Bilhetes";
            try
            {

                DateTime? dtInicio = dataI;
                DateTime? dtFinal = dataF;
                Util.CriarCabecalho(regs, dtInicio, dtFinal, Empregador, NumeroSerie);
                IList<COLETA> coletas = new List<COLETA>();
                
                RELOGIOS rel = dimepUtils.getRelogio();
                using (var db = new SERVCOM_NETEntities(Conn))
                {
                    coletas = db.COLETA.Where(w => w.IDRELOGIO == rel.IDRELOGIO && w.DATAHORA >= dataI && w.DATAHORA <= dataF).ToList();
                }
                    foreach (COLETA bil in coletas)
                    {
                        RegistroAFD reg = new RegistroAFD();
                        reg.Campo01 = bil.NSR;
                        reg.Campo02 = "3";
                        reg.Campo04 = bil.DATAHORA.GetValueOrDefault().ToString("ddMMyyyy");
                        reg.Campo05 = bil.DATAHORA.GetValueOrDefault().ToString("HHmm");
                        reg.Campo06 = bil.PIS.ToString();
                        reg.Nsr = Convert.ToInt32(bil.NSR);
                        reg.DataHoraRegistro = bil.DATAHORA.GetValueOrDefault();
                        regs.Add(reg);
                    }
                

                // Se tem apenas cabeçalho
                if (regs.Count() == 1)
                {
                    regs = new List<RegistroAFD>();
                }

                return regs;
            }
            catch (Exception e)
            {
                log.Error("Erro ao coletar registros, erro: " + e.Message + ", stacktrace = " + e.StackTrace);
                throw e;
            }
        }

        public override List<RegistroAFD> GetAFDNsr(DateTime dataI, DateTime dataF, int nsrInicio, int nsrFim, bool ordemDecrescente)
        {
            List<RegistroAFD> regs = new List<RegistroAFD>();
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, DateTime.Now, Conn);
            string comando = "Importar Bilhetes";
            try
            {
                DateTime? dtInicio = dataI;
                DateTime? dtFinal = dataF;
                Util.CriarCabecalho(regs, dtInicio, dtFinal, Empregador, NumeroSerie);
                IList<COLETA> coletas = new List<COLETA>();

                RELOGIOS rel = dimepUtils.getRelogio();
                using (var db = new SERVCOM_NETEntities(Conn))
                {   
                    coletas = db.COLETA.SqlQuery("SELECT * FROM dbo.COLETA where IDRELOGIO = " + rel.IDRELOGIO + " AND CONVERT(INT, NSR) BETWEEN " + nsrInicio + " AND " + nsrFim + " ").ToList();
                }
                    foreach (COLETA bil in coletas)
                    {
                        RegistroAFD reg = new RegistroAFD();
                        reg.Campo01 = bil.NSR;
                        reg.Campo02 = "3";
                        reg.Campo04 = bil.DATAHORA.GetValueOrDefault().ToString("ddMMyyyy");
                        reg.Campo05 = bil.DATAHORA.GetValueOrDefault().ToString("HHmm");
                        reg.Campo06 = bil.PIS.ToString();
                        reg.Nsr = Convert.ToInt32(bil.NSR);
                        reg.DataHoraRegistro = bil.DATAHORA.GetValueOrDefault();
                        regs.Add(reg);
                    }
                

                // Se tem apenas cabeçalho
                if (regs.Count() == 1)
                {
                    regs = new List<RegistroAFD>();
                }

                return regs;
            }
            catch (Exception e)
            {
                log.Error("Erro ao coletar registros, erro: " + e.Message + ", stacktrace = " + e.StackTrace);
                throw e;
            }
        }

        public override bool ConfigurarHorarioVerao(DateTime inicio, DateTime termino, out string erros)
        {
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, dataComando, Conn);
            string comando = "Configurar Horário de Verão";
            try
            {
                bool retorno = false;
                if (!dimepUtils.ValidaLogServCom(comando, out retorno, out erros, cwkPontoMT.Integracao.Relogios.Dimep.UtilsDimep.DimepUtils.ComandoEnviado.HorarioVerao))
                {
                    RELOGIOS rel = dimepUtils.getRelogio();
                    using (var db = new SERVCOM_NETEntities(Conn))
                    {
                        rel.RELOGIOENTRAHORARIODEVERAO = true;
                        rel.INICIOHORARIODEVERAO = inicio;
                        rel.FIMHORARIODEVERAO = termino;
                        db.Entry(rel).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    EXECUCAOCOMANDO execComando = new EXECUCAOCOMANDO();
                    execComando.IDTIPOCOMANDO = 2;
                    execComando.HORARIOPARAEXECUCAO = DateTime.Now;
                    execComando.TIPOEXECUTOR = 3;
                    retorno = dimepUtils.ExecutaComando(out erros, execComando);
                    if (retorno)
                    {
                        erros = "Adicionado na fila de execução do ServComNet";
                        retorno = false;
                    }
                }
                if (erros == "Adicionado na fila de execução do ServComNet")
                {
                    log.Debug("Configurar Horário de verão; "+ erros+ "; Comando deverá ser confirmado na próxima interação");
                }
                else if (retorno)
                {
                    log.Debug("Configurar Horário de verão; Comando enviado com sucesso; comando confirmado pelo ServComNet");
                }
                else
                {
                    log.Debug("Configurar Horário de verão; " + erros);
                   
                }

                return retorno;
            }
            catch (Exception e)
            {
                log.Error("Erro ao Configurar Horário de verão, erro: " + e.Message + ", stacktrace = " + e.StackTrace);
                throw e;
            }
        }

        public override bool ConfigurarHorarioRelogio(DateTime horario, out string erros)
        {
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, dataComando, Conn);
            string comando = "Configurar Data e Hora";
            try
            {
                bool retorno = false;
                if (!dimepUtils.ValidaLogServCom(comando, out retorno, out erros, cwkPontoMT.Integracao.Relogios.Dimep.UtilsDimep.DimepUtils.ComandoEnviado.DataHora))
                {
                    RELOGIOS rel = dimepUtils.getRelogio();
                    using (var db = new SERVCOM_NETEntities(Conn))
                    {
                        rel.FUSOHORARIO = Util.DiffFusoMinutos(timeZoneInfoRep);
                        db.Entry(rel).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    EXECUCAOCOMANDO execComando = new EXECUCAOCOMANDO();
                    execComando.IDTIPOCOMANDO = 1;
                    execComando.HORARIOPARAEXECUCAO = DateTime.Now;
                    execComando.TIPOEXECUTOR = 3;
                    retorno = dimepUtils.ExecutaComando(out erros, execComando);
                    if (retorno)
                    {
                        erros = "Adicionado na fila de execução do ServComNet";
                        retorno = false;
                    }
                }
                if (erros == "Adicionado na fila de execução do ServComNet")
                {
                    log.Debug("Configurar Horário; " + erros + "; Comando deverá ser confirmado na próxima interação");
                }
                else if (retorno)
                {
                    log.Debug("Configurar Horário; Comando confirmado pelo ServComNet");
                }
                else
                {
                    log.Debug("Configurar Horário; " + erros);
                }
                return retorno;
            }
            catch (Exception e)
            {
                log.Error("Erro ao Configurar Horário de verão, erro: " + e.Message + ", stacktrace = " + e.StackTrace);
                throw e;
            }
        }

        public override bool EnviarEmpregadorEEmpregados(bool biometrico, out string erros)
        {
            erros = string.Empty;
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, DateTime.Now, Conn);
            RELOGIOS rel = dimepUtils.getRelogio();
            string comando = "";
            try
            {
                if (Empregados != null)
                {
                    if (Empregados.Count > 0)
                    {
                        comando = "Inclusão de Funcionários";
                        log.Debug("Enviar Empregados; " + "Iniciando Inclusão de " + Empregados.Count() + " Empregado(s)");
                        foreach (var item in this.Empregados)
                        {
                            try
                            {
                                IncluiAlteraFuncionario(rel, comando, item);
                            }
                            catch (Exception exception)
                            {
                                throw exception;
                            }
                        }
                    }
                }

                if (Empregador != null)
                {
                    comando = "Inclusão de Empresa";
                    log.Debug("Enviar Empregador; Inclusão de empresa não realizado, Relógio não tem suporte a essa opção.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Enviar Empregador/Empregados, erro: " + ex.Message + ", stacktrace = " + ex.StackTrace);
                erros += ex.Message;
            }
            return String.IsNullOrEmpty(erros);
        }

        private void IncluiAlteraFuncionario(RELOGIOS rel, string comando, Entidades.Empregado item)
        {
            try
            {
                using (var db = new SERVCOM_NETEntities(Conn))
                {
                    try 
	                {
                        log.Debug("Enviar Empregados; Incluindo Funcionário: " + item.Nome + " Matrícula: " + item.Matricula);
                        db.UP_INCLUIPESSOA(item.Matricula, item.Senha, item.Pis, item.Nome);
	                }
	                catch (Exception e)
	                {
                        string exception = Util.GetExceptionMessage(e);
		                if (exception.Contains("EXISTE OUTRA PESSOA ASSOCIADA COM ESSA MATRÍCULA."))
                        {
                            decimal pisdecimal = Convert.ToDecimal(item.Pis);
                            if (db.PESSOA.Where(w => w.MATRICULA.Contains(item.Matricula) && w.PIS == pisdecimal).Count() > 0)
                            {
                                log.Debug("Enviar Empregados; Alterando Funcionário: " + item.Nome + " Matrícula: " + item.Matricula);
                                db.UP_ALTERAPESSOA(item.Matricula, item.Senha, item.Nome);
                            }
                            else
                            {
                                throw new Exception(exception);
                            }
                        }
                        else
                        {
                            throw new Exception(exception);
                        }
	                }
                    
                    try
                    {
                        log.Debug("Enviar Empregados; Associando o Funcionário: " + item.Nome + " Matrícula: " + item.Matricula + " ao rep de Id = " + rel.IDRELOGIO);
                        db.UP_ASSOCIAPESSOARELOGIO(item.Matricula, rel.IDRELOGIO);
                    }
                    catch (Exception e)
                    {
                        string exception = Util.GetExceptionMessage(e);
                        if (!exception.Contains("O RELÓGIO INFORMADO JÁ ESTÁ ASSOCIADO A ESSA PESSOA."))
                        {
                            throw new Exception(exception);
                        }
                    }

                    try
                    {
                        log.Debug("Enviar Empregados; Incluir cartão do Funcionário: " + item.Nome + " Matrícula: " + item.Matricula + " Id cartão " + item.DsCodigo);
                        string cracha = CampoCracha == 0 ? item.DsCodigo : Util.SoNumeros(item.Matricula).ToString();
                        db.UP_INCLUICARTAO(item.Matricula, cracha, 0);
                    }
                    catch (Exception e)
                    {
                        string exception = Util.GetExceptionMessage(e);
                        if (!exception.Contains("O CARTÃO INFORMADO JÁ ESTÁ ASSOCIADO A ESSA PESSOA."))
                        {
                            throw new Exception(exception);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void RemoveFuncionario(RELOGIOS rel, string comando, Entidades.Empregado item)
        {
            try
            {
                using (var db = new SERVCOM_NETEntities(Conn))
                {
                    try
                    {
                        log.Debug("Remove Empregados; Desassocia o Funcionário: " + item.Nome + " Matrícula: " + item.Matricula + " do rep de Id = " + rel.IDRELOGIO);
                        db.UP_DESASSOCIAPESSOARELOGIO(item.Matricula, rel.IDRELOGIO);
                    }
                    catch (Exception e)
                    {
                        string erro = Util.GetExceptionMessage(e);
                        if (!erro.Contains("NÃO EXISTE PESSOA CADASTRADA COM A MATRÍCULA INFORMADA") && !erro.Contains("O RELÓGIO INFORMADO NÃO ESTÁ ASSOCIADO A ESSA PESSOA"))
                        {
                            throw;
                        }
                    }
                    
                    try
                    {
                        log.Debug("Remove Empregados; Excluindo cartão do Funcionário: " + item.Nome + " Matrícula: " + item.Matricula + " Id cartão " + item.DsCodigo);
                        db.UP_EXCLUICARTAO(item.Matricula, item.DsCodigo);
                    }
                    catch (Exception e)
                    {
                        string erro = Util.GetExceptionMessage(e);
                        if (!erro.Contains("NÃO EXISTE PESSOA CADASTRADA COM A MATRÍCULA INFORMADA") && !erro.Contains("O CARTÃO INFORMADO NÃO EXISTE"))
                        {
                            throw;
                        }
                    }
                    
                    try
                    {
                        log.Debug("Remove Empregados; Excluindo Funcionário: " + item.Nome + " Matrícula: " + item.Matricula);
                        db.UP_EXCLUIPESSOA(item.Matricula);
                    }
                    catch (Exception e)
                    {
                        string erro = Util.GetExceptionMessage(e);
                        if (!erro.Contains("NÃO EXISTE PESSOA CADASTRADA COM A MATRÍCULA INFORMADA"))
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override Dictionary<string, object> RecebeInformacoesRep(out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool RemoveFuncionariosRep(out string erros)
        {
            erros = string.Empty;
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, DateTime.Now, Conn);
            RELOGIOS rel = dimepUtils.getRelogio();
            string comando = "Remove Funcionário";
            try
            {
                if (Empregados != null)
                {
                    if (Empregados.Count > 0)
                    {
                        log.Debug("Remove Empregados; Iniciando exclusão de "+Empregados.Count()+" Empregado(s)");
                        foreach (var item in this.Empregados)
                        {
                            try
                            {
                                RemoveFuncionario(rel, comando, item);
                            }
                            catch (Exception exception)
                            {
                                throw exception;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string erro = Util.GetExceptionMessage(ex);
                log.Error("Remove Empregados, erro: " + ex.Message + ", stacktrace = " + ex.StackTrace);
                erros += erro;
            }
            return String.IsNullOrEmpty(erros);
        }

        public override bool VerificaExistenciaFuncionarioRelogio(Entidades.Empregado funcionario, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportaEmpregadorFuncionarios(string caminho, out string erros)
        {
            throw new NotImplementedException();
        }

        public override bool ExportacaoHabilitada()
        {
            return false;
        }

        public override Dictionary<string, string> ExportaEmpregadorFuncionariosWeb(ref string caminho, out string erros, System.IO.DirectoryInfo pasta)
        {
            throw new NotImplementedException();
        }

        public override bool ExclusaoHabilitada()
        {
            return true;
        }

        public override bool TesteConexao()
        {
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, DateTime.Now, Conn);
            RELOGIOS rel = dimepUtils.getRelogio();
            using (var db = new SERVCOM_NETEntities(Conn))
            {
                DateTime ultimaComunicacao = db.LOG.Where(w => w.IDRELOGIO == rel.IDRELOGIO).Max(m => m.DATAHORA).GetValueOrDefault();
                if ((DateTime.Now - ultimaComunicacao).TotalMinutes <= 5 )
                {
                    return true;
                }
                return false;
            }
        }

        public override int UltimoNSR()
        {
            DimepUtils dimepUtils = new DimepUtils(NumeroSerie, dataComando, Conn);
            RELOGIOS rel = dimepUtils.getRelogio();
            return Convert.ToInt32(rel.ULTIMONSR);
        }

        public override List<Biometria> GetBiometria(out string erros)
        {
            throw new NotImplementedException();
        }
    }
}
