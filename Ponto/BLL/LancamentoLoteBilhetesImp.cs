using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class LancamentoLoteBilhetesImp : IBLL<Modelo.LancamentoLoteBilhetesImp>
    {
        DAL.ILancamentoLoteBilhetesImp dalLancamentoLoteBilhetesImp;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoLoteBilhetesImp()
            : this(null)
        {

        }

        public LancamentoLoteBilhetesImp(string connString)
            : this(connString, null)
        {

        }

        public LancamentoLoteBilhetesImp(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalLancamentoLoteBilhetesImp = new DAL.SQL.LancamentoLoteBilhetesImp(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoLoteBilhetesImp.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoLoteBilhetesImp.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoLoteBilhetesImp.GetAll();
        }

        public List<Modelo.LancamentoLoteBilhetesImp> GetAllList()
        {
            return dalLancamentoLoteBilhetesImp.GetAllList();
        }

        public Modelo.LancamentoLoteBilhetesImp LoadObject(int id)
        {
            return dalLancamentoLoteBilhetesImp.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoLoteBilhetesImp objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoLoteBilhetesImp objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoLoteBilhetesImp.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoLoteBilhetesImp.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLancamentoLoteBilhetesImp.Excluir(objeto);
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
            return dalLancamentoLoteBilhetesImp.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna o LancamentoLoteBilhetesImp de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteBilhetesImp</returns>
        public Modelo.LancamentoLoteBilhetesImp LoadByIdLote(int idLote)
        {
            return dalLancamentoLoteBilhetesImp.LoadByIdLote(idLote);
        }
    }
}
