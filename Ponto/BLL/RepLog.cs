using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class RepLog : IBLL<Modelo.RepLog>
    {
        DAL.IRepLog dalRepLog;
        private string ConnectionString;

        public RepLog() : this(null)
        {
            
        }

        public RepLog(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public RepLog(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalRepLog = new DAL.SQL.RepLog(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalRepLog = new DAL.SQL.RepLog(new DataBase(ConnectionString));
                    break;
            }
            dalRepLog.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalRepLog.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalRepLog.GetAll();
        }

        public Modelo.RepLog LoadObject(int id)
        {
            return dalRepLog.LoadObject(id);
        }

        public List<Modelo.RepLog> GetAllList()
        {
            return dalRepLog.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.RepLog objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                objeto.Codigo = MaxCodigo();
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.RepLog objeto)
        {
            objeto.NaoValidaCodigo = true;
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalRepLog.Incluir(objeto);
                        dalRepLog.DeletaLogAntigo();
                        break;
                    case Modelo.Acao.Alterar:
                        dalRepLog.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalRepLog.Excluir(objeto);
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
            return dalRepLog.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.RepLog> GetAllListByRep(int idRep)
        {
            return dalRepLog.GetAllListByRep(idRep);
        }
        public List<Modelo.Proxy.PxyRepLogAFD> GetRepLogAFD(string lote)
        {
            return dalRepLog.GetRepLogAFD(lote);
        }

        public List<Modelo.Proxy.PxyResumoRepLogImportacao> GetRepLogAFDResumo(string relogio)
        {
            DataTable dt = dalRepLog.GetRepLogAFDResumo(relogio);
            List<Modelo.Proxy.PxyResumoRepLogImportacao> ret = dt.DataTableMapToList<Modelo.Proxy.PxyResumoRepLogImportacao>();
            return ret;
        }
    }
}
