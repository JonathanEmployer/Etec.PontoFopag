using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class LancamentoLoteInclusaoBanco : IBLL<Modelo.LancamentoLoteInclusaoBanco>
    {
        DAL.ILancamentoLoteInclusaoBanco dalLancamentoLoteInclusaoBanco;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoLoteInclusaoBanco()
            : this(null)
        {

        }

        public LancamentoLoteInclusaoBanco(string connString)
            : this(connString, null)
        {

        }

        public LancamentoLoteInclusaoBanco(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalLancamentoLoteInclusaoBanco = new DAL.SQL.LancamentoLoteInclusaoBanco(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoLoteInclusaoBanco.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoLoteInclusaoBanco.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoLoteInclusaoBanco.GetAll();
        }

        public List<Modelo.LancamentoLoteInclusaoBanco> GetAllList()
        {
            return dalLancamentoLoteInclusaoBanco.GetAllList();
        }

        public Modelo.LancamentoLoteInclusaoBanco LoadObject(int id)
        {
            return dalLancamentoLoteInclusaoBanco.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoLoteInclusaoBanco objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoLoteInclusaoBanco objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoLoteInclusaoBanco.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoLoteInclusaoBanco.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLancamentoLoteInclusaoBanco.Excluir(objeto);
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
            return dalLancamentoLoteInclusaoBanco.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna o LancamentoLoteInclusaoBanco de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteInclusaoBanco</returns>
        public Modelo.LancamentoLoteInclusaoBanco LoadByIdLote(int idLote)
        {
            return dalLancamentoLoteInclusaoBanco.LoadByIdLote(idLote);
        }
    }
}
