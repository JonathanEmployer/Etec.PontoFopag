using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class AlertasRepAcompanhamento : IBLL<Modelo.AlertasRepAcompanhamento>
    {
        DAL.IAlertasRepAcompanhamento dalAlertasRepAcompanhamento;
        private string ConnectionString;

        public AlertasRepAcompanhamento() : this(null)
        {
            
        }

        public AlertasRepAcompanhamento(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public AlertasRepAcompanhamento(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalAlertasRepAcompanhamento = new DAL.SQL.AlertasRepAcompanhamento(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAlertasRepAcompanhamento = new DAL.SQL.AlertasRepAcompanhamento(new DataBase(ConnectionString));
                    break;
            }
            dalAlertasRepAcompanhamento.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAlertasRepAcompanhamento.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAlertasRepAcompanhamento.GetAll();
        }

        public Modelo.AlertasRepAcompanhamento LoadObject(int id)
        {
            return dalAlertasRepAcompanhamento.LoadObject(id);
        }

        public List<Modelo.AlertasRepAcompanhamento> GetAllList()
        {
            return dalAlertasRepAcompanhamento.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.AlertasRepAcompanhamento objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.AlertasRepAcompanhamento objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalAlertasRepAcompanhamento.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalAlertasRepAcompanhamento.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAlertasRepAcompanhamento.Excluir(objeto);
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
            return dalAlertasRepAcompanhamento.getId(pValor, pCampo, pValor2);
        }

        public void IncluirLoteIdsRep(SqlTransaction trans, int idAlerta, List<int> idsFuncs)
        {
            dalAlertasRepAcompanhamento.IncluirLoteIdsRep(trans, idAlerta, idsFuncs);
        }

        public void ExcluirLoteIdsRep(SqlTransaction trans, int idAlerta, List<int> idsFuncs)
        {
            dalAlertasRepAcompanhamento.ExcluirLoteIdsRep(trans, idAlerta, idsFuncs);
        }

        public List<Modelo.AlertasRepAcompanhamento> GetAllPorAlerta(Int32 idAlerta)
        {
            return dalAlertasRepAcompanhamento.GetAllPorAlerta(idAlerta);
        }
    }
}
