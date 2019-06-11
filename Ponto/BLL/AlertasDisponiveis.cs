using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class AlertasDisponiveis : IBLL<Modelo.AlertasDisponiveis>
    {
        DAL.IAlertasDisponiveis dalAlertasDisponiveis;
        private string ConnectionString;

        public AlertasDisponiveis() : this(null)
        {
            
        }

        public AlertasDisponiveis(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public AlertasDisponiveis(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalAlertasDisponiveis = new DAL.SQL.AlertasDisponiveis(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAlertasDisponiveis = new DAL.SQL.AlertasDisponiveis(new DataBase(ConnectionString));
                    break;
            }
            dalAlertasDisponiveis.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAlertasDisponiveis.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAlertasDisponiveis.GetAll();
        }

        public Modelo.AlertasDisponiveis LoadObject(int id)
        {
            return dalAlertasDisponiveis.LoadObject(id);
        }

        public List<Modelo.AlertasDisponiveis> GetAllList()
        {
            return dalAlertasDisponiveis.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.AlertasDisponiveis objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.AlertasDisponiveis objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalAlertasDisponiveis.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalAlertasDisponiveis.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAlertasDisponiveis.Excluir(objeto);
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
            return dalAlertasDisponiveis.getId(pValor, pCampo, pValor2);
        }
    }
}
