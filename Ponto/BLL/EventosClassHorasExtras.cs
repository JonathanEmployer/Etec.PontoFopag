using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class EventosClassHorasExtras : IBLL<Modelo.EventosClassHorasExtras>
    {
        DAL.IEventosClassHorasExtras dalEventosClassHorasExtras;
        private string ConnectionString;

        public EventosClassHorasExtras() : this(null)
        {
            
        }

        public EventosClassHorasExtras(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EventosClassHorasExtras(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalEventosClassHorasExtras = new DAL.SQL.EventosClassHorasExtras(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalEventosClassHorasExtras = new DAL.SQL.EventosClassHorasExtras(new DataBase(ConnectionString));
                    break;
            }
            dalEventosClassHorasExtras.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEventosClassHorasExtras.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEventosClassHorasExtras.GetAll();
        }

        public Modelo.EventosClassHorasExtras LoadObject(int id)
        {
            return dalEventosClassHorasExtras.LoadObject(id);
        }

        public List<Modelo.EventosClassHorasExtras> GetAllList()
        {
            return dalEventosClassHorasExtras.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EventosClassHorasExtras objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EventosClassHorasExtras objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEventosClassHorasExtras.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEventosClassHorasExtras.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEventosClassHorasExtras.Excluir(objeto);
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
            return dalEventosClassHorasExtras.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna todas as classificações ligadas a um evento
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <returns>Lista com as Classificações</returns>
        public IList<Modelo.EventosClassHorasExtras> GetListPorEvento(int idEvento)
        {
            return dalEventosClassHorasExtras.GetListPorEvento(idEvento);
        }

        /// <summary>
        /// Retorna todos os ids separados por virgulas das classificações ligados ao evento
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <returns>string com os ids separados por virgulas das classificações</returns>
        public string GetIdsClassificacaoPorEvento(int idEvento)
        {
            return dalEventosClassHorasExtras.GetIdsClassificacaoPorEvento(idEvento);
        }
    }
}
