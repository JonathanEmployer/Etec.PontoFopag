//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PontoWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class cworkpontoEntities : DbContext
    {
        public cworkpontoEntities()
            : base("name=cworkpontoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<afastamento> afastamento { get; set; }
        public virtual DbSet<Alertas> Alertas { get; set; }
        public virtual DbSet<AlertasFuncionario> AlertasFuncionario { get; set; }
        public virtual DbSet<alias_tabela> alias_tabela { get; set; }
        public virtual DbSet<backup> backup { get; set; }
        public virtual DbSet<bancohoras> bancohoras { get; set; }
        public virtual DbSet<bilhetesimp> bilhetesimp { get; set; }
        public virtual DbSet<compensacao> compensacao { get; set; }
        public virtual DbSet<configuracaorefeitorio> configuracaorefeitorio { get; set; }
        public virtual DbSet<cw_acesso> cw_acesso { get; set; }
        public virtual DbSet<cw_acessocampo> cw_acessocampo { get; set; }
        public virtual DbSet<cw_grupo> cw_grupo { get; set; }
        public virtual DbSet<cwkvsnsys> cwkvsnsys { get; set; }
        public virtual DbSet<departamento> departamento { get; set; }
        public virtual DbSet<diascompensacao> diascompensacao { get; set; }
        public virtual DbSet<diasjornadaalternativa> diasjornadaalternativa { get; set; }
        public virtual DbSet<empresa> empresa { get; set; }
        public virtual DbSet<empresacwusuario> empresacwusuario { get; set; }
        public virtual DbSet<equipamento> equipamento { get; set; }
        public virtual DbSet<equipamentohomologado> equipamentohomologado { get; set; }
        public virtual DbSet<eventos> eventos { get; set; }
        public virtual DbSet<exportacaocampos> exportacaocampos { get; set; }
        public virtual DbSet<fechamentobh> fechamentobh { get; set; }
        public virtual DbSet<fechamentobhd> fechamentobhd { get; set; }
        public virtual DbSet<fechamentobhdpercentual> fechamentobhdpercentual { get; set; }
        public virtual DbSet<feriado> feriado { get; set; }
        public virtual DbSet<funcao> funcao { get; set; }
        public virtual DbSet<funcionario> funcionario { get; set; }
        public virtual DbSet<funcionariohistorico> funcionariohistorico { get; set; }
        public virtual DbSet<Geral> Geral { get; set; }
        public virtual DbSet<horario> horario { get; set; }
        public virtual DbSet<horariodetalhe> horariodetalhe { get; set; }
        public virtual DbSet<horariophextra> horariophextra { get; set; }
        public virtual DbSet<Importacaoautomatica> Importacaoautomatica { get; set; }
        public virtual DbSet<importalayouttexto> importalayouttexto { get; set; }
        public virtual DbSet<inclusaobanco> inclusaobanco { get; set; }
        public virtual DbSet<jornada> jornada { get; set; }
        public virtual DbSet<jornadaalternativa> jornadaalternativa { get; set; }
        public virtual DbSet<justificativa> justificativa { get; set; }
        public virtual DbSet<layoutexportacao> layoutexportacao { get; set; }
        public virtual DbSet<layoutimportacaofuncionario> layoutimportacaofuncionario { get; set; }
        public virtual DbSet<marcacaoacesso> marcacaoacesso { get; set; }
        public virtual DbSet<mudancahorario> mudancahorario { get; set; }
        public virtual DbSet<mudcodigofunc> mudcodigofunc { get; set; }
        public virtual DbSet<ocorrencia> ocorrencia { get; set; }
        public virtual DbSet<parametros> parametros { get; set; }
        public virtual DbSet<provisorio> provisorio { get; set; }
        public virtual DbSet<rep> rep { get; set; }
        public virtual DbSet<Revendas> Revendas { get; set; }
        public virtual DbSet<tipobilhetes> tipobilhetes { get; set; }
        public virtual DbSet<jornadaalternativa_view> jornadaalternativa_view { get; set; }
        public virtual DbSet<marcacao_view> marcacao_view { get; set; }
        public virtual DbSet<marcacao> marcacao { get; set; }
        public virtual DbSet<cw_usuario> cw_usuario { get; set; }
    
        [DbFunction("cworkpontoEntities", "F_BancoHoras")]
        public virtual IQueryable<F_BancoHoras_Result> F_BancoHoras(Nullable<System.DateTime> data, Nullable<int> idFuncionario)
        {
            var dataParameter = data.HasValue ?
                new ObjectParameter("data", data) :
                new ObjectParameter("data", typeof(System.DateTime));
    
            var idFuncionarioParameter = idFuncionario.HasValue ?
                new ObjectParameter("idFuncionario", idFuncionario) :
                new ObjectParameter("idFuncionario", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_BancoHoras_Result>("[cworkpontoEntities].[F_BancoHoras](@data, @idFuncionario)", dataParameter, idFuncionarioParameter);
        }
    
        [DbFunction("cworkpontoEntities", "F_BHPerc")]
        public virtual IQueryable<F_BHPerc_Result> F_BHPerc(Nullable<System.DateTime> p_dataInicio, Nullable<System.DateTime> p_dataFim, Nullable<int> p_idFuncionario, Nullable<int> p_considerarUltimoFechamento)
        {
            var p_dataInicioParameter = p_dataInicio.HasValue ?
                new ObjectParameter("p_dataInicio", p_dataInicio) :
                new ObjectParameter("p_dataInicio", typeof(System.DateTime));
    
            var p_dataFimParameter = p_dataFim.HasValue ?
                new ObjectParameter("p_dataFim", p_dataFim) :
                new ObjectParameter("p_dataFim", typeof(System.DateTime));
    
            var p_idFuncionarioParameter = p_idFuncionario.HasValue ?
                new ObjectParameter("p_idFuncionario", p_idFuncionario) :
                new ObjectParameter("p_idFuncionario", typeof(int));
    
            var p_considerarUltimoFechamentoParameter = p_considerarUltimoFechamento.HasValue ?
                new ObjectParameter("p_considerarUltimoFechamento", p_considerarUltimoFechamento) :
                new ObjectParameter("p_considerarUltimoFechamento", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_BHPerc_Result>("[cworkpontoEntities].[F_BHPerc](@p_dataInicio, @p_dataFim, @p_idFuncionario, @p_considerarUltimoFechamento)", p_dataInicioParameter, p_dataFimParameter, p_idFuncionarioParameter, p_considerarUltimoFechamentoParameter);
        }
    
        [DbFunction("cworkpontoEntities", "FnGethorariophextra")]
        public virtual IQueryable<FnGethorariophextra_Result> FnGethorariophextra()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGethorariophextra_Result>("[cworkpontoEntities].[FnGethorariophextra]()");
        }
    
        [DbFunction("cworkpontoEntities", "FnGetTratamentoBilhetes")]
        public virtual IQueryable<FnGetTratamentoBilhetes_Result> FnGetTratamentoBilhetes(Nullable<System.DateTime> dataInicial, Nullable<System.DateTime> dataFinal)
        {
            var dataInicialParameter = dataInicial.HasValue ?
                new ObjectParameter("DataInicial", dataInicial) :
                new ObjectParameter("DataInicial", typeof(System.DateTime));
    
            var dataFinalParameter = dataFinal.HasValue ?
                new ObjectParameter("DataFinal", dataFinal) :
                new ObjectParameter("DataFinal", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FnGetTratamentoBilhetes_Result>("[cworkpontoEntities].[FnGetTratamentoBilhetes](@DataInicial, @DataFinal)", dataInicialParameter, dataFinalParameter);
        }
    
        public virtual int importa_marcacao()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("importa_marcacao");
        }
    
        public virtual int insert_marcacao()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insert_marcacao");
        }
    
        public virtual int p_enviaAlertaEntradaAtrasada(Nullable<System.DateTime> ultimaExecucao, Nullable<System.DateTime> dataExecucao, Nullable<System.DateTime> ultimoAviso, Nullable<int> idAlerta, string email, string key)
        {
            var ultimaExecucaoParameter = ultimaExecucao.HasValue ?
                new ObjectParameter("ultimaExecucao", ultimaExecucao) :
                new ObjectParameter("ultimaExecucao", typeof(System.DateTime));
    
            var dataExecucaoParameter = dataExecucao.HasValue ?
                new ObjectParameter("dataExecucao", dataExecucao) :
                new ObjectParameter("dataExecucao", typeof(System.DateTime));
    
            var ultimoAvisoParameter = ultimoAviso.HasValue ?
                new ObjectParameter("UltimoAviso", ultimoAviso) :
                new ObjectParameter("UltimoAviso", typeof(System.DateTime));
    
            var idAlertaParameter = idAlerta.HasValue ?
                new ObjectParameter("idAlerta", idAlerta) :
                new ObjectParameter("idAlerta", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_enviaAlertaEntradaAtrasada", ultimaExecucaoParameter, dataExecucaoParameter, ultimoAvisoParameter, idAlertaParameter, emailParameter, keyParameter);
        }
    
        public virtual int p_enviaAlertas(Nullable<System.DateTime> ultimaExecucao, Nullable<System.DateTime> dataExecucao, string key)
        {
            var ultimaExecucaoParameter = ultimaExecucao.HasValue ?
                new ObjectParameter("ultimaExecucao", ultimaExecucao) :
                new ObjectParameter("ultimaExecucao", typeof(System.DateTime));
    
            var dataExecucaoParameter = dataExecucao.HasValue ?
                new ObjectParameter("dataExecucao", dataExecucao) :
                new ObjectParameter("dataExecucao", typeof(System.DateTime));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_enviaAlertas", ultimaExecucaoParameter, dataExecucaoParameter, keyParameter);
        }
    
        public virtual int p_enviaAlertaSaidaAntecipada(Nullable<System.DateTime> ultimaExecucao, Nullable<System.DateTime> dataExecucao, Nullable<System.DateTime> ultimoAviso, Nullable<int> idAlerta, string email, string key)
        {
            var ultimaExecucaoParameter = ultimaExecucao.HasValue ?
                new ObjectParameter("ultimaExecucao", ultimaExecucao) :
                new ObjectParameter("ultimaExecucao", typeof(System.DateTime));
    
            var dataExecucaoParameter = dataExecucao.HasValue ?
                new ObjectParameter("dataExecucao", dataExecucao) :
                new ObjectParameter("dataExecucao", typeof(System.DateTime));
    
            var ultimoAvisoParameter = ultimoAviso.HasValue ?
                new ObjectParameter("UltimoAviso", ultimoAviso) :
                new ObjectParameter("UltimoAviso", typeof(System.DateTime));
    
            var idAlertaParameter = idAlerta.HasValue ?
                new ObjectParameter("idAlerta", idAlerta) :
                new ObjectParameter("idAlerta", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_enviaAlertaSaidaAntecipada", ultimaExecucaoParameter, dataExecucaoParameter, ultimoAvisoParameter, idAlertaParameter, emailParameter, keyParameter);
        }
    
        public virtual int p_enviaAlertasBancoHoras(Nullable<System.DateTime> ultimaExecucao, Nullable<System.DateTime> dataExecucao, Nullable<System.DateTime> ultimoAviso, Nullable<int> idAlerta, string email, string key)
        {
            var ultimaExecucaoParameter = ultimaExecucao.HasValue ?
                new ObjectParameter("ultimaExecucao", ultimaExecucao) :
                new ObjectParameter("ultimaExecucao", typeof(System.DateTime));
    
            var dataExecucaoParameter = dataExecucao.HasValue ?
                new ObjectParameter("dataExecucao", dataExecucao) :
                new ObjectParameter("dataExecucao", typeof(System.DateTime));
    
            var ultimoAvisoParameter = ultimoAviso.HasValue ?
                new ObjectParameter("UltimoAviso", ultimoAviso) :
                new ObjectParameter("UltimoAviso", typeof(System.DateTime));
    
            var idAlertaParameter = idAlerta.HasValue ?
                new ObjectParameter("idAlerta", idAlerta) :
                new ObjectParameter("idAlerta", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_enviaAlertasBancoHoras", ultimaExecucaoParameter, dataExecucaoParameter, ultimoAvisoParameter, idAlertaParameter, emailParameter, keyParameter);
        }
    
        public virtual int p_enviaAlertasFalta(Nullable<System.DateTime> ultimaExecucao, Nullable<System.DateTime> dataExecucao, Nullable<System.DateTime> ultimoAviso, Nullable<int> idAlerta, string email, string key)
        {
            var ultimaExecucaoParameter = ultimaExecucao.HasValue ?
                new ObjectParameter("ultimaExecucao", ultimaExecucao) :
                new ObjectParameter("ultimaExecucao", typeof(System.DateTime));
    
            var dataExecucaoParameter = dataExecucao.HasValue ?
                new ObjectParameter("dataExecucao", dataExecucao) :
                new ObjectParameter("dataExecucao", typeof(System.DateTime));
    
            var ultimoAvisoParameter = ultimoAviso.HasValue ?
                new ObjectParameter("UltimoAviso", ultimoAviso) :
                new ObjectParameter("UltimoAviso", typeof(System.DateTime));
    
            var idAlertaParameter = idAlerta.HasValue ?
                new ObjectParameter("idAlerta", idAlerta) :
                new ObjectParameter("idAlerta", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_enviaAlertasFalta", ultimaExecucaoParameter, dataExecucaoParameter, ultimoAvisoParameter, idAlertaParameter, emailParameter, keyParameter);
        }
    
        public virtual int p_enviaAlertaSMarcacoesIncorretas(Nullable<System.DateTime> ultimaExecucao, Nullable<System.DateTime> dataExecucao, Nullable<System.DateTime> ultimoAviso, Nullable<int> idAlerta, string email, string key)
        {
            var ultimaExecucaoParameter = ultimaExecucao.HasValue ?
                new ObjectParameter("ultimaExecucao", ultimaExecucao) :
                new ObjectParameter("ultimaExecucao", typeof(System.DateTime));
    
            var dataExecucaoParameter = dataExecucao.HasValue ?
                new ObjectParameter("dataExecucao", dataExecucao) :
                new ObjectParameter("dataExecucao", typeof(System.DateTime));
    
            var ultimoAvisoParameter = ultimoAviso.HasValue ?
                new ObjectParameter("UltimoAviso", ultimoAviso) :
                new ObjectParameter("UltimoAviso", typeof(System.DateTime));
    
            var idAlertaParameter = idAlerta.HasValue ?
                new ObjectParameter("idAlerta", idAlerta) :
                new ObjectParameter("idAlerta", typeof(int));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("p_enviaAlertaSMarcacoesIncorretas", ultimaExecucaoParameter, dataExecucaoParameter, ultimoAvisoParameter, idAlertaParameter, emailParameter, keyParameter);
        }
    
        public virtual int sql_marcacao(string select, string key)
        {
            var selectParameter = select != null ?
                new ObjectParameter("Select", select) :
                new ObjectParameter("Select", typeof(string));
    
            var keyParameter = key != null ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sql_marcacao", selectParameter, keyParameter);
        }
    
        public virtual int update_bilhete()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("update_bilhete");
        }
    
        public virtual int update_marcacao()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("update_marcacao");
        }
    }
}
