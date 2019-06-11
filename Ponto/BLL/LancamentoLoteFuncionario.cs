using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class LancamentoLoteFuncionario : IBLL<Modelo.LancamentoLoteFuncionario>
    {
        DAL.ILancamentoLoteFuncionario dalLancamentoLoteFuncionario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoLoteFuncionario()
            : this(null)
        {

        }

        public LancamentoLoteFuncionario(string connString)
            : this(connString, null)
        {

        }

        public LancamentoLoteFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLancamentoLoteFuncionario = new DAL.SQL.LancamentoLoteFuncionario(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoLoteFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoLoteFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoLoteFuncionario.GetAll();
        }

        public List<Modelo.LancamentoLoteFuncionario> GetAllList()
        {
            return dalLancamentoLoteFuncionario.GetAllList();
        }

        public Modelo.LancamentoLoteFuncionario LoadObject(int id)
        {
            return dalLancamentoLoteFuncionario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoLoteFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoLoteFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoLoteFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoLoteFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLancamentoLoteFuncionario.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalLancamentoLoteFuncionario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.LancamentoLoteFuncionario> GetListWhere(string condicao)
        {
            return dalLancamentoLoteFuncionario.GetListWhere(condicao);
        }

        /// <summary>
        /// Exclui as folgas lançadas por lote de acordo com os funcionários, período e tipo lançamento passados por parâmetro
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idsFuncionarios"> Lista de Ids de Funcionários</param>
        /// <param name="dataInicial">Data Início</param>
        /// <param name="dataFinal">Data Fim</param>
        /// <param name="tpLancamento">Tipo do Lançamento</param>
        public void ExcluirFuncionariosDataTipo(SqlTransaction trans, List<int> idsFuncionarios, DateTime dataInicial, DateTime dataFinal, TipoLancamento tpLancamento)
        {
            dalLancamentoLoteFuncionario.ExcluirFuncionariosDataTipo(trans, idsFuncionarios, dataInicial, dataFinal, tpLancamento);
            DAL.ILancamentoLote dalLancamentoLote = new DAL.SQL.LancamentoLote(new DataBase(ConnectionString));
            dalLancamentoLote.ExcluiLoteSemFilho(trans);
        }
    }
}
