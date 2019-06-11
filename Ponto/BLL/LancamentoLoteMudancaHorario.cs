using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class LancamentoLoteMudancaHorario : IBLL<Modelo.LancamentoLoteMudancaHorario>
    {
        DAL.ILancamentoLoteMudancaHorario dalLancamentoLoteMudancaHorario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoLoteMudancaHorario()
            : this(null)
        {

        }

        public LancamentoLoteMudancaHorario(string connString)
            : this(connString, null)
        {

        }

        public LancamentoLoteMudancaHorario(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalLancamentoLoteMudancaHorario = new DAL.SQL.LancamentoLoteMudancaHorario(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoLoteMudancaHorario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoLoteMudancaHorario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoLoteMudancaHorario.GetAll();
        }

        public List<Modelo.LancamentoLoteMudancaHorario> GetAllList()
        {
            return dalLancamentoLoteMudancaHorario.GetAllList();
        }

        public Modelo.LancamentoLoteMudancaHorario LoadObject(int id)
        {
            return dalLancamentoLoteMudancaHorario.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoLoteMudancaHorario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoLoteMudancaHorario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoLoteMudancaHorario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoLoteMudancaHorario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLancamentoLoteMudancaHorario.Excluir(objeto);
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
            return dalLancamentoLoteMudancaHorario.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna o LancamentoLoteMudancaHorario de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteMudancaHorario</returns>
        public Modelo.LancamentoLoteMudancaHorario LoadByIdLote(int idLote)
        {
            return dalLancamentoLoteMudancaHorario.LoadByIdLote(idLote);
        }
    }
}
