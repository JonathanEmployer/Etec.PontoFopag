using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Alocacao : IBLL<Modelo.Alocacao>
    {
        DAL.IAlocacao dalAlocacao;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Alocacao()
            : this(null)
        {

        }

        public Alocacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Alocacao(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalAlocacao = new DAL.SQL.Alocacao(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalAlocacao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAlocacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAlocacao.GetAll();
        }

        public List<Modelo.Alocacao> GetAllList()
        {
            return dalAlocacao.GetAllList();
        }

        public Modelo.Alocacao LoadObject(int id)
        {
            return dalAlocacao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Alocacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Alocacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        if (dalAlocacao.BuscaAlocacao(objeto.Descricao))
                        {
                            ret.Add("txtDescricao", "Já existe um cadastro de Alocação com esse nome. Verifique!");
                        }
                        else
                        {
                            dalAlocacao.Incluir(objeto);
                        }
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalAlocacao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAlocacao.Excluir(objeto);
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
            return dalAlocacao.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Método responsável em retornar o id de uma Alocação, passando como parâmetro o nome da Alocação
        /// </summary>
        /// <param name="pNomeDescricao">Nome da Alocação</param>
        /// <returns>ID da Alocação</returns>
        public int? getAlocacaoNome(string pNomeDescricao)
        {
            return dalAlocacao.getAlocacaoNome(pNomeDescricao);
        }

        public int? GetIdPorCod(int Cod)
        {
            return dalAlocacao.GetIdPorCod(Cod);
        }

        public Modelo.Alocacao LoadObjectByCodigo(int idAlocacao)
        {
            return dalAlocacao.LoadObjectByCodigo(idAlocacao);
        }
    }
}
