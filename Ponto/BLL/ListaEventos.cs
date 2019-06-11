using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BLL
{
    public class ListaEventos : IBLL<Modelo.ListaEventos>
    {
        DAL.IListaEventos dalListaEventos;
        private string ConnectionString;

        public ListaEventos() : this(null)
        {
            
        }

        public ListaEventos(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ListaEventos(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalListaEventos = new DAL.SQL.ListaEventos(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalListaEventos = new DAL.SQL.ListaEventos(new DataBase(ConnectionString));
                    break;
            }
            dalListaEventos.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalListaEventos.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalListaEventos.GetAll();
        }

        public Modelo.ListaEventos LoadObject(int id)
        {
            return dalListaEventos.LoadObject(id);
        }

        public List<Modelo.ListaEventos> GetAllList()
        {
            return dalListaEventos.GetAllList();
        }

        #region validacoes
        public Dictionary<string, string> ValidaObjeto(Modelo.ListaEventos objeto, Modelo.Acao acao)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (acao != Modelo.Acao.Excluir)
            {
                if (objeto.Cod_Lista_Eventos == 0)
                {
                    ret.Add("Cod_Lista_Eventos", "Campo obrigatório.");
                }

                objeto.IdEventosSelecionados = String.IsNullOrEmpty(objeto.IdEventosSelecionados) ? "" : objeto.IdEventosSelecionados;
                objeto.IdEventosSelecionados_Ant = String.IsNullOrEmpty(objeto.IdEventosSelecionados_Ant) ? "" : objeto.IdEventosSelecionados_Ant;

            }
            return ret;
        }
        #endregion

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ListaEventos objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto, pAcao);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalListaEventos.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalListaEventos.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalListaEventos.Excluir(objeto);
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
            return dalListaEventos.getId(pValor, pCampo, pValor2);
        }


        public Dictionary<string, string> ValidaObjeto(Modelo.ListaEventos objeto)
        {
            throw new NotImplementedException();
        }
    }
}
