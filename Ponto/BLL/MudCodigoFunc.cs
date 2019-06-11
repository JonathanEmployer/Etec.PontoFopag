using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class MudCodigoFunc : IBLL<Modelo.MudCodigoFunc>
    {
        DAL.IMudCodigoFunc dalMudCodigoFunc;
        private Modelo.Cw_Usuario UsuarioLogado;
        private string ConnectionString;
        public MudCodigoFunc() : this(null)
        {
        }

        public MudCodigoFunc(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {
        }

        public MudCodigoFunc(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalMudCodigoFunc = new DAL.SQL.MudCodigoFunc(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalMudCodigoFunc = DAL.FB.MudCodigoFunc.GetInstancia;
                    break;
            }
            dalMudCodigoFunc.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalMudCodigoFunc.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalMudCodigoFunc.GetAll();
        }

        public Modelo.MudCodigoFunc LoadObject(int id)
        {
            return dalMudCodigoFunc.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.MudCodigoFunc objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.MudCodigoFunc objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalMudCodigoFunc.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalMudCodigoFunc.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalMudCodigoFunc.Excluir(objeto);
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
            return dalMudCodigoFunc.getId(pValor, pCampo, pValor2);
        }

        public bool VerificaMarcacao(int pId, DateTime pData) 
        {
            return dalMudCodigoFunc.VerificaMarcacao(pId, pData);
        }

        public List<Modelo.MudCodigoFunc> GetMudancasPeriodo(DateTime datai, DateTime dataf)
        {
            return dalMudCodigoFunc.GetMudancasPeriodo(datai, dataf);
        }

        public List<Modelo.MudCodigoFunc> GetAllList()
        {
            return dalMudCodigoFunc.GetAllList();
        }
    }
}
