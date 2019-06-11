using cwkWebAPIPontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using CentralCliente;
using cwkWebAPIPontoWeb.Utils;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RepController : ApiController
    {
        /// <summary>
        /// Retorna todos os relógios cadastrados no banco ao qual o usuário possui acesso.
        /// </summary>
        public IList<RepViewModel> Get(string usuario)
        {
            if (String.IsNullOrEmpty(usuario))
            {
               usuario = MetodosAuxiliares.UsuarioPontoWeb().Login;
            }
            return BuscaRepsVmPorLoginUsuario(usuario);
        }

        public static IList<RepViewModel> BuscaRepsVmPorLoginUsuario(string usuario)
        {
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            IList<RepViewModel> retorno = new List<RepViewModel>();
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                string connectionStr = CriptoString.Decrypt(usu.connectionString);
                using (var conn = new SqlConnection(connectionStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = @"
                            select 
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
	                            , coalesce(r.local, e.endereco, '') as EnderecoEmpregador
	                            , coalesce(e.cei, '') as CEI
	                            , coalesce(e.cnpj, e.cpf, '') as CpfCnpjEmpregador
	                            , coalesce(eh.EquipamentoHomologadoInmetro, 0) as EquipamentoHomologadoInmetro
	                            , coalesce(ISNULL(r.CpfRep, cwu.Cpf), '') as CpfUsuarioRep
	                            , coalesce(ISNULL(r.LoginRep, cwu.LoginRep), '') as LoginRep
	                            , coalesce(ISNULL(r.SenhaRep, cwu.SenhaRep), '') as SenhaRep
                                , coalesce(r.TipoIP, '') as TipoIP
                                , isnull(r.DataInicioImportacao,getdate()) DataInicioImportacao
                                , r.TempoRequisicao
                                , r.ImportacaoAtivada
                                , r.UltimoNSR
								, eh.ServicoComunicador
								,ISNULL((SELECT DISTINCT 1 FROM dbo.envioconfiguracoesdatahora ecdh WHERE ecdh.bEnviaDataHoraServidor = 1 AND ecdh.idRelogio = r.id),0) EnviarDataHora
								,ISNULL((SELECT DISTINCT 1 FROM dbo.envioconfiguracoesdatahora ecdh WHERE ecdh.bEnviaHorarioVerao = 1 AND ecdh.idRelogio = r.id),0) EnviarHorarioVerao
								, ISNULL((SELECT DISTINCT 1 FROM dbo.EnvioDadosRep edr INNER JOIN dbo.EnvioDadosRepDet edrd ON edr.ID = edrd.IDEnvioDadosRep
									WHERE edrd.IDEmpresa IS NOT NULL AND edr.IDRep = r.id),0) EnviarEmpresa
								, ISNULL((SELECT DISTINCT 1 FROM dbo.EnvioDadosRep edr INNER JOIN dbo.EnvioDadosRepDet edrd ON edr.ID = edrd.IDEnvioDadosRep
									WHERE edrd.IDFuncionario IS NOT NULL AND edr.IDRep = r.id),0) EnviarFuncionario
								, r.UltimaIntegracao
                                , r.IdTimeZoneInfo
                                , r.CpfRep
                                , r.LoginRep
                                , r.SenhaRep
                                , r.CampoCracha
                                , r.IdEquipamentoTipoBiometria
                            from rep r
                            left join equipamentohomologado eh on r.idequipamentohomologado = eh.id
                            left join empresa e on r.idempresa = e.id
                            cross join cw_usuario cwu
                            where cwu.idUsuarioCentralCliente = " + usu.ID.ToString();
                        using (var reader = cmd.ExecuteReader())
                        {
                            List<RepViewModel> dados = new List<RepViewModel>();
                            dados = MetodosAuxiliares.DataReaderMapToList<RepViewModel>(reader);
                            retorno = dados;
                        }
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (usu.ID == 0)
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                throw ex;
            }
        }

        public static IList<Modelo.REP> BuscaRepsPorLoginUsuario(string usuario)
        {
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;

            string connectionStr = CriptoString.Decrypt(usu.connectionString);
            if (usu.ID == 0)
            {
                throw new Exception("Usuário não encontrado");
            }

            return BLLAPI.Rep.GetReps(connectionStr);
        }
    }
}
