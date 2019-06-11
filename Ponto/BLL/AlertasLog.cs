using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class AlertasLog : IBLL<Modelo.AlertasLog>
    {
        DAL.IAlertasLog dalAlertasLog;
        private string ConnectionString;

        public AlertasLog() : this(null)
        {
            
        }

        public AlertasLog(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public AlertasLog(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalAlertasLog = new DAL.SQL.AlertasLog(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAlertasLog = new DAL.SQL.AlertasLog(new DataBase(ConnectionString));
                    break;
            }
            dalAlertasLog.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAlertasLog.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAlertasLog.GetAll();
        }

        public Modelo.AlertasLog LoadObject(int id)
        {
            return dalAlertasLog.LoadObject(id);
        }

        public List<Modelo.AlertasLog> GetAllList()
        {
            return dalAlertasLog.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.AlertasLog objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.AlertasLog objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalAlertasLog.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalAlertasLog.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAlertasLog.Excluir(objeto);
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
            return dalAlertasLog.getId(pValor, pCampo, pValor2);
        }


        public List<Modelo.AlertasLog> GetAllListByAlerta(Int32 idAlerta)
        {
            return dalAlertasLog.GetAllListByAlerta(idAlerta);
        }
    }
}
