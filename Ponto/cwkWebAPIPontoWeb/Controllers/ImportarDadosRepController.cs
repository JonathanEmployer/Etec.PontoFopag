using cwkPontoMT.Integracao.Entidades;
using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using CentralCliente;
using cwkWebAPIPontoWeb.Utils;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Controlador para envio de dados para o Rep.
    /// </summary>
    public class ImportarDadosRepController : ApiController
    {
        /// <summary>
        /// Retorna uma lista de Objetos do tipo ImportacaoDadosRep com os dados a serem importados pelos relógios.
        /// </summary>
        /// <param name="idRelogio">String com os id's dos relógios</param>
        /// <returns> Retorna uma lista de Objetos do Tipo ImportacaoDadosRep com a lista de Empregados e Empresas a serem importados no relógio</returns>
        [HttpGet]
        [TratamentoDeErro]
        public List<ImportacaoDadosRep> RetornaFuncExpRep([FromUri]List<String> idRelogio)
        {
            List<ImportacaoDadosRep> LImportacaoDadosRep = new List<ImportacaoDadosRep>();
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                string connectionStr = CriptoString.Decrypt(usu.connectionString);
                IList<Modelo.EnvioDadosRep> LDadosRep = new List<Modelo.EnvioDadosRep>();
                CarregaEnvioDadosRep(idRelogio, connectionStr, ref LDadosRep);
                CarregaReps(connectionStr, LDadosRep,ref LImportacaoDadosRep);
                foreach (ImportacaoDadosRep importacaoDadosRep in LImportacaoDadosRep)
                {
                    if (importacaoDadosRep.EnvioDadosRep != null && importacaoDadosRep.EnvioDadosRep.Id > 0)
                    {
                        IList<Modelo.EnvioDadosRepDet> dadosRepDetalhe = new List<Modelo.EnvioDadosRepDet>();
                        dadosRepDetalhe = CarregaEnvioDadosRepDet(connectionStr, importacaoDadosRep.EnvioDadosRep.Id, dadosRepDetalhe);
                        if (dadosRepDetalhe != null && dadosRepDetalhe.Count > 0)
                        {
                            CarregaFuncionarios(connectionStr, importacaoDadosRep, dadosRepDetalhe);
                            CarregaEmpresas(connectionStr, importacaoDadosRep, dadosRepDetalhe);
                        }
                    }
                }
                return LImportacaoDadosRep;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (usu.ID == 0)
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                throw ex;
            }
        }

        /// <summary>
        /// Deleta uma lista de registros ImportacaoDadosRep de acordo com os ids passados por parâmetro.
        /// </summary>
        /// <param name="idsDadosImportacao">Passar lista de id's dos registros a serem excluídos</param>
        /// <returns> Retorna um dicionário de chave e valor, 0 = Erro ao excluir, chave 1 = Ok - Registro Excluido, chave 2 = nenhum registro encontrado para exclusão, </returns>
        [HttpDelete]
        [TratamentoDeErro]
        public Dictionary<int,string> DeletaExpFuncRep([FromUri]List<int> idsDadosImportacao)
        {
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                string connectionStr = CriptoString.Decrypt(usu.connectionString);
                using (var conn = new SqlConnection(connectionStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Parameters.AddWithValue("@idsEnvioDadosRep", string.Join(",", idsDadosImportacao));
                        conn.Open();
                        cmd.CommandText = @"delete from EnvioDadosRepDet where IDEnvioDadosRep in (select * from [dbo].[F_RetornaTabelaLista] (@idsEnvioDadosRep,','));";
                        int i = cmd.ExecuteNonQuery();
                        Dictionary<int, string> retorno = new Dictionary<int, string>();
                        if (i < 0)
                        {
                            return new Dictionary<int, string>() { { 0, "Erro ao excluir registro filho!" } };
                        }
                    }
                }
                using (var conn = new SqlConnection(connectionStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Parameters.AddWithValue("@idsEnvioDadosRep", string.Join(",", idsDadosImportacao));
                        conn.Open();
                        cmd.CommandText = @"delete from EnvioDadosRep where ID in (select * from [dbo].[F_RetornaTabelaLista] (@idsEnvioDadosRep,','));";
                        int i = cmd.ExecuteNonQuery();
                        Dictionary<int, string> retorno = new Dictionary<int, string>();
                        if (i < 0)
                        {
                            return new Dictionary<int, string>() { { 0, "Erro ao excluir registro!" } };
                        }
                        else if (i == 0) { return new Dictionary<int, string>() { { 2, "Nenhum Registro Encontrado para Exclusão." } }; }
                    }
                }
                return new Dictionary<int, string>() { { 1, "Registro excluído com sucesso!" } };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (usu.ID == 0)
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                return new Dictionary<int, string>() { { 0, "Erro ao excluir registro: "+ex.Message } };
            }
        }

        #region Métodos para buscar dados no banco
        private static void CarregaReps(string connectionStr, IList<Modelo.EnvioDadosRep> LdadosRep, ref List<ImportacaoDadosRep> LImportacaoDadosRep)
        {
            string idsRelogios = String.Join(",", LdadosRep.Select(x => x.idRelogioSelecionado));
            List<RepViewModel> LRep = new List<RepViewModel>();
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idsRelogios", string.Join(",", idsRelogios));
                    conn.Open();
                    cmd.CommandText = @"select 
	                            r.id as Id
	                            , r.codigo as Codigo
	                            , r.tipocomunicacao as TipoComunicacao
	                            , r.numserie as NumSerie
	                            , r.numrelogio as NumRelogio
	                            , r.relogio as NumModeloRelogio
	                            , coalesce(r.senha, '') as SenhaComunicacao
	                            , coalesce(r.ip, '') as Ip
	                            , coalesce(r.porta, '') as Porta
	                            , r.qtdDigitos as QtdDigitosCartao
	                            , r.biometrico as UtilizaBiometria
                                , coalesce(eh.nomeFabricante, '') as NomeFabricante
                                , coalesce(eh.nomeModelo, '') as NomeModelo
	                            , coalesce(e.nome, '') as NomeEmpregador
	                            , coalesce(r.local, '') as EnderecoEmpregador
	                            , coalesce(e.cei, '') as CEI
	                            , coalesce(e.cnpj, e.cpf, '') as CpfCnpjEmpregador
                                , tb.Descricao as TipoBiometria
                            from rep r 
                            left join equipamentohomologado eh on r.idequipamentohomologado = eh.id
                            left join empresa e on r.idempresa = e.id
                            left join EquipamentoTipoBiometria et on r.IdEquipamentoTipoBiometria = et.Id
							left join TipoBiometria tb on et.IdTipoBiometria = tb.Id
                           where r.id in (select * from [dbo].[F_RetornaTabelaLista] (@idsRelogios,','));";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<RepViewModel> dados = new List<RepViewModel>();
                            dados = MetodosAuxiliares.DataReaderMapToList<RepViewModel>(reader);
                            LRep = dados;
                        }
                    }
                }
            }
            //Para cada registro a ser importado (tabela EnvioDadosRep) cria um objeto ImportacaoDadosRep que será enviado pelo webservice. Esse objeto possui o id da tabela ImportacaoDadosRep, o rep, os funcionários e empresas a serem importados.
            foreach (Modelo.EnvioDadosRep ldr in LdadosRep)
            {
                RepViewModel repAdd = LRep.Where(x => x.Id == ldr.idRelogioSelecionado).FirstOrDefault();
                if (repAdd != null)
                {
                    ImportacaoDadosRep idr = new ImportacaoDadosRep();
                    idr.Rep = repAdd;
                    idr.EnvioDadosRep = new Modelo.EnvioDadosRep();
                    idr.EnvioDadosRep.Id = ldr.Id;
                    idr.EnvioDadosRep.Codigo = ldr.Codigo;
                    idr.EnvioDadosRep.idRelogioSelecionado = ldr.idRelogioSelecionado;
                    idr.EnvioDadosRep.bOperacao = (ldr.bOperacao);
                    idr.EnvioDadosRep.Altdata = ldr.Altdata;
                    idr.EnvioDadosRep.Althora = ldr.Althora;
                    idr.EnvioDadosRep.Altusuario = ldr.Altusuario;
                    idr.EnvioDadosRep.Incdata = ldr.Incdata;
                    idr.EnvioDadosRep.Inchora = ldr.Inchora;
                    idr.EnvioDadosRep.Incusuario = ldr.Incusuario;
                    idr.EnvioDadosRep.TipoComunicacao = ldr.TipoComunicacao;
                    LImportacaoDadosRep.Add(idr);
                }
                
            }
        }
        private static void CarregaFuncionarios(string connectionStr, ImportacaoDadosRep importacaoDadosRep, IList<Modelo.EnvioDadosRepDet> dadosRepDetalhe)
        {
            string idsFuncionarios = String.Join(",", dadosRepDetalhe.Select(x => x.idFuncionario)); 
            List<Empregado> LFunc = new List<Empregado>();
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idsFuncionarios", string.Join(",", idsFuncionarios));
                    cmd.Parameters.AddWithValue("@idRep", importacaoDadosRep.Rep.Id);
                    conn.Open();
                    cmd.CommandText = @"select  1 Biometria,
		                                        DsCodigo,
	                                            Nome,
	                                            Pis,
	                                            master.dbo.FN_ClSeguranca_Decrypt(func.senha) Senha,
                                                Matricula,
                                                (select top(1) RFID from FuncionarioRFID where IdFuncionario = func.id and Ativo = 1 and RFID is not null) RFID,
		                                        (select top(1) MIFARE from FuncionarioRFID where IdFuncionario = func.id and Ativo = 1 and MIFARE is not null) MIFARE,
		                                        b.valorBiometria,
                                                func.id
                                        from funcionario func
                                        left join biometria b on b.idfuncionario = func.id and b.IdRep = @idRep
                                        where func.id in (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','));";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Empregado> dados = new List<Empregado>();
                            dados = MetodosAuxiliares.DataReaderMapToList<Empregado>(reader);
                            LFunc = dados;
                        }
                    }
                }
            }
            importacaoDadosRep.Empregados = LFunc;
        }

        private static void CarregaEmpresas(string connectionStr, ImportacaoDadosRep importacaoDadosRep, IList<Modelo.EnvioDadosRepDet> dadosRepDetalhe)
        {
            string idsEmpresas = String.Join(",", dadosRepDetalhe.Select(x => x.idEmpresa));
            List<cwkPontoMT.Integracao.Entidades.Empresa> LEmp = new List<cwkPontoMT.Integracao.Entidades.Empresa>();
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idsEmpresas", string.Join(",", idsEmpresas));
                    conn.Open();
                    cmd.CommandText = @"select *,
		                                        case when len(Documento) >= 13 then
			                                        1
		                                        else 2 end TipoDocumento
                                            from (
	                                        select id,
                                                    isnull(CEI,'') CEI,
		                                            replace(replace(replace(CNPJ,'.',''),'/',''),'-','') Documento,
		                                            Endereco Local,
		                                            Nome RazaoSocial
	                                            from Empresa
		                                        ) emp
                                         where Emp.id in (select * from [dbo].[F_RetornaTabelaLista] (@idsEmpresas,','));";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<cwkPontoMT.Integracao.Entidades.Empresa> dados = new List<cwkPontoMT.Integracao.Entidades.Empresa>();
                            dados = MetodosAuxiliares.DataReaderMapToList<cwkPontoMT.Integracao.Entidades.Empresa>(reader);
                            LEmp = dados;
                        }
                    }
                }
            }
            importacaoDadosRep.Empresas = LEmp;
        }

        private static IList<Modelo.EnvioDadosRepDet> CarregaEnvioDadosRepDet(string connectionStr, int IdDadosRep, IList<Modelo.EnvioDadosRepDet> dadosRepDetalhe)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idExporta", IdDadosRep);
                    conn.Open();
                    cmd.CommandText = @"select edrd.* 
                                                  from EnvioDadosRepDet edrd
                                                 where edrd.IDEnvioDadosRep = @idExporta;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Modelo.EnvioDadosRepDet> dados = new List<Modelo.EnvioDadosRepDet>();
                            dados = MetodosAuxiliares.DataReaderMapToList<Modelo.EnvioDadosRepDet>(reader);
                            dadosRepDetalhe = dados;
                        }
                    }
                }
            }
            return dadosRepDetalhe;
        }

        private static void CarregaEnvioDadosRep(List<String> idsRelogios, string connectionStr, ref IList<Modelo.EnvioDadosRep> dadosRep)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idsRelogios", string.Join(",", idsRelogios));
                    conn.Open();
                    cmd.CommandText = @"select edr.*, edr.idRep idRelogioSelecionado
                                        from EnvioDadosRep edr
                                        where edr.IDRep in (select * from [dbo].[F_RetornaTabelaLista] (@idsRelogios,','));";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Modelo.EnvioDadosRep> dados = new List<Modelo.EnvioDadosRep>();
                            dados = MetodosAuxiliares.DataReaderMapToList<Modelo.EnvioDadosRep>(reader);
                            dadosRep = dados;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
