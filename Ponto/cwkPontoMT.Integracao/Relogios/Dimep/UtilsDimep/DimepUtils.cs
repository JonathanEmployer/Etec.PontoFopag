using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Dimep.UtilsDimep
{
    public class DimepUtils
    {
        string NumeroSerie = "";
        DateTime dataComando = DateTime.Now;
        string connServCom = "";
        public DimepUtils(string NumeroSerie, DateTime dataComando, string conn)
        {
            this.NumeroSerie = NumeroSerie;
            this.dataComando = dataComando;

            connServCom = conn;
        }

        public bool ValidaLogServCom(string comando, out bool retorno, out string erros, ComandoEnviado comandoServCom)
        {
            try
            {
                retorno = false;
                erros = "";
                DateTime dataUltimoLogSucesso = DateTime.MinValue;
                RELOGIOS relogio = getRelogio();
                LOG LogServCom = new LOG(); ;
                using (var db = new SERVCOM_NETEntities(connServCom))
                {
                    LogServCom = db.LOG.Where(w => w.IDRELOGIO == relogio.IDRELOGIO && w.COMANDOENVIADO == (int)comandoServCom).OrderByDescending(o => o.DATAHORA).FirstOrDefault();
                    if (LogServCom != null)
                    {
                        DateTime.TryParse(LogServCom.DATAHORA.ToString(), out dataUltimoLogSucesso);
                    }
                }

                if (dataUltimoLogSucesso > dataComando)
                {
                    if (LogServCom.STATUSEXECUCAO == 0)
                    {
                        erros = "";
                        retorno = true;
                    }
                    else
                    {
                        erros = LogServCom.DESCRICAO;
                        retorno = false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Util.GravaLogCentralCliente(NumeroSerie, comando, 1, "Erro ao validar log no ServComNet, detalhe: "+ e.Message, "Comunicador", e.StackTrace.ToString());
                throw e;
            }
        }

        public enum ComandoEnviado
        {
            ArquivoLimpeza = 48,
            ArquivoMensagem = 49,
            AssociarPessoas = 46,
            Coleta = 0,
            ColetaBackUP = 44,
            ColetaTemplate = 18,
            Configuracao = 1,
            ConfiguracaoParcial = 19,
            DataHora = 4,
            DataHoraHorarioVerao = 43,
            DesassociarPessoas = 47,
            Desconhecido = 23,
            Empregador = 21,
            EnvioCodigoAlternativo = 39,
            EnvioFeriados = 33,
            EnvioJornadas = 38,
            EnvioMensagensFuncao = 34,
            EnvioMensagensSistema = 36,
            EnvioMensagensUsuario = 35,
            EnvioSinaleiros = 24,
            EnvioSupervisor = 14,
            EnvioTurnos = 37,
            ExclusaoCartao = 3,
            ExclusaoCartaoParcial = 62,
            ExclusaoCentroCustos = 27,
            ExclusaoPessoa = 6,
            ExclusaoPessoaParcial = 60,
            ExclusaoTemplate = 16,
            ExclusaoTemplateParcial = 64,
            FormatoCartao = 42,
            FormatoMemoria = 41,
            HorarioVerao = 7,
            InclusaoCartao = 2,
            InclusaoCartaoParcial = 61,
            InclusaoCentroCustos = 26,
            InclusaoPessoa = 5,
            InclusaoPessoaParcial = 50,
            InclusaoTemplate = 15,
            InclusaoTemplateParcial = 63,
            LimpezaCartoesAlternativos = 32,
            LimpezaCentroCustos = 25,
            LimpezaCredenciais = 8,
            LimpezaFeriados = 31,
            LimpezaInvalida = 22,
            LimpezaJornadas = 29,
            LimpezaMensagens = 10,
            LimpezaPessoas = 45,
            LimpezaSinaleiros = 30,
            LimpezaSupervisores = 11,
            LimpezaTemplate = 9,
            LimpezaTemplateBloqueado = 40,
            LimpezaTurnos = 28,
            ReposicionarPonteiro = 20,
            Sincronizacao = 17,
            Status = 12,
            StatusImediato = 13
        }

        public RELOGIOS getRelogio()
        {
            RELOGIOS rel = null;
            EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder(connServCom);
            using (var db = new SERVCOM_NETEntities(connServCom))
            {
                rel = db.RELOGIOS.AsEnumerable().Where(w => w.NUMEROFABRICACAO == Convert.ToDecimal(NumeroSerie)).FirstOrDefault();
                if (rel == null || rel.IDRELOGIO <= 0)
                {
                    throw new Exception("Relógio " + NumeroSerie + " não cadastrado no ServComNet (Sistema Dimep)");
                }

            }
            return rel;
        }

        public bool ExecutaComando(out string erros, EXECUCAOCOMANDO execComando)
        {
            erros = "";
            try
            {
                RELOGIOS rel = getRelogio();
                using (var db = new SERVCOM_NETEntities(connServCom))
                {
                    execComando.IDRELOGIO = rel.IDRELOGIO;
                    db.EXECUCAOCOMANDO.Add(execComando);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                erros = e.Message;
                return false;
            }
        }
    }
}
