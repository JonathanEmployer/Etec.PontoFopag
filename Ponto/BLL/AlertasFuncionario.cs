using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class AlertasFuncionario : IBLL<Modelo.AlertasFuncionario>
    {
        DAL.IAlertasFuncionario dalAlertasFuncionario;
        private string ConnectionString;

        public AlertasFuncionario() : this(null)
        {
            
        }

        public AlertasFuncionario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public AlertasFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalAlertasFuncionario = new DAL.SQL.AlertasFuncionario(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAlertasFuncionario = new DAL.SQL.AlertasFuncionario(new DataBase(ConnectionString));
                    break;
            }
            dalAlertasFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAlertasFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAlertasFuncionario.GetAll();
        }

        public Modelo.AlertasFuncionario LoadObject(int id)
        {
            return dalAlertasFuncionario.LoadObject(id);
        }

        public List<Modelo.AlertasFuncionario> GetAllList()
        {
            return dalAlertasFuncionario.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.AlertasFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.AlertasFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalAlertasFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalAlertasFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAlertasFuncionario.Excluir(objeto);
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
            return dalAlertasFuncionario.getId(pValor, pCampo, pValor2);
        }

        public void IncluirLoteIdsFuncionario(SqlTransaction trans, int idAlerta, List<int> idsFuncs)
        {
            dalAlertasFuncionario.IncluirLoteIdsFuncionario(trans, idAlerta, idsFuncs);
        }

        public void ExcluirLoteIdsFuncionario(SqlTransaction trans, int idAlerta, List<int> idsFuncs)
        {
            dalAlertasFuncionario.ExcluirLoteIdsFuncionario(trans, idAlerta, idsFuncs);
        }

        public List<Modelo.AlertasFuncionario> GetAllPorAlerta(Int32 idAlerta)
        {
            return dalAlertasFuncionario.GetAllPorAlerta(idAlerta);
        }
    }
}
