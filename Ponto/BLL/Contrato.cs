using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Contrato : IBLL<Modelo.Contrato>
    {
        DAL.IContrato dalContrato;
        private string ConnectionString;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public Contrato() : this(null)
        {

        }

        public Contrato(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Contrato(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalContrato = new DAL.SQL.Contrato(new DataBase(ConnectionString));
            dalContrato.UsuarioLogado = usuarioLogado;
        }

        public DataTable GetAll()
        {
            return dalContrato.GetAll();
        }

        public Modelo.Contrato LoadObject(int id)
        {
            return dalContrato.LoadObject(id);
        }

        public List<Modelo.Contrato> GetAllList()
        {
            return dalContrato.GetAllList();
        }
        public Modelo.Contrato LoadPorCodigo(int codigo)
        {
            return dalContrato.LoadPorCodigo(codigo);
        }
        public List<Modelo.Contrato> GetAllListPorEmpresa(int idEmpresa)
        {
            return dalContrato.GetAllListPorEmpresa(idEmpresa);
        }


        public Dictionary<string, string> ValidaObjeto(Modelo.Contrato objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.DiaFechamentoInicial < 0)
            {
                ret.Add("DiaFechamentoInicial", "O dia inicial deve ser maior que zero(0).");
            }
            else if (objeto.DiaFechamentoInicial > 31)
            {
                ret.Add("DiaFechamentoInicial", "O dia não pode ser maior do que trinta(30).");
            }
            else if (objeto.DiaFechamentoInicial == 0 && objeto.DiaFechamentoFinal != 0)
            {
                ret.Add("DiaFechamentoInicial", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            if (objeto.DiaFechamentoFinal < 0)
            {
                ret.Add("DiaFechamentoFinal", "O dia final deve ser maior que zero(0).");
            }
            else if (objeto.DiaFechamentoFinal > 31)
            {
                ret.Add("DiaFechamentoFinal", "O dia não pode ser maior do que trinta(30).");
            }
            else if (objeto.DiaFechamentoInicial != 0 && objeto.DiaFechamentoFinal == 0)
            {
                ret.Add("DiaFechamentoFinal", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            if (objeto.IdEmpresa == 0)
            {
                ret.Add("IdEmpresa", "Empresa não informada ou não encontrada.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Contrato objeto)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (pAcao == Modelo.Acao.Alterar || pAcao == Modelo.Acao.Incluir)
            {
                res = ValidaObjeto(objeto);
            }
            if (res.Count > 0)
            {
                return res;
            }
            try
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalContrato.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalContrato.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        try
                        {
                            dalContrato.Excluir(objeto);
                        }
                        catch (Exception ex)
                        {
                            string erro = String.Empty;
                            if (ex.Message.ToLower().Contains("fk_contratofuncionario_contrato"))
                            {
                                throw new Exception("Não foi possível excluir o contrato pois este contrato possui funcionários vinculados. Verifique.", ex);
                            }
                            if (ex.Message.ToLower().Contains("fk_contratousuario_contrato"))
                            {
                                throw new Exception("Não foi possível excluir o contrato pois este contrato possui usuários vinculados. Verifique.", ex);
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalContrato.getId(pValor, pCampo, pValor2);
        }

        public int MaxCodigo()
        {
            return dalContrato.MaxCodigo();
        }

        public List<Modelo.Contrato> GetAllPorUsuario(int idCw_Usuario)
        {
            return dalContrato.GetAllPorUsuario(idCw_Usuario);
        }

        public int? GetIdPorIdIntegracao(int idIntegracao)
        {
            return dalContrato.GetIdPorIdIntegracao(idIntegracao);
        }

        /// <summary>
        /// Retorna o período de fechamento do ponto por contrato
        /// </summary>
        /// <param name="idContrato">Id do contrato</param>
        /// <returns>Período de Fechamento do contrato</returns>
        public Modelo.PeriodoFechamento PeriodoFechamento(int idContrato)
        {
            return dalContrato.PeriodoFechamento(idContrato);
        }

        /// <summary>
        /// Retorna o período de fechamento do ponto por contrato
        /// </summary>
        /// <param name="codigoContrato">Código do contrato</param>
        /// <returns>Período de Fechamento do contrato</returns>
        public Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoContrato)
        {
            return dalContrato.PeriodoFechamentoPorCodigo(codigoContrato);
        }

        /// <summary>
        /// Retorna os contratos vinculados a um funcionário
        /// </summary>
        /// <param name="idFuncionario">id do funcionário</param>
        /// <returns>Lista de contratos vinculados a um idfuncionario</returns>
        public List<Modelo.Contrato> ContratosPorFuncionario(int idFuncionario)
        {
            return dalContrato.ContratosPorFuncionario(idFuncionario);
        }        
        public bool ValidaContratoCodigo(int codcontrato, int idempresa)
        {
            return dalContrato.ValidaContratoCodigo(codcontrato,idempresa);
        }


        public int GetIdByConsulta(string consulta)
        {
            int codigo = -1;
            try { codigo = Int32.Parse(consulta.Split('|').FirstOrDefault()); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                return getId(codigo, null, null);
            }
            return 0;
        }

    }
}
