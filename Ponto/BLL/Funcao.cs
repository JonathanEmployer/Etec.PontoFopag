using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Funcao : IBLL<Modelo.Funcao>
    {
        DAL.IFuncao dalFuncao;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Funcao() : this(null)
        {
            
        }

        public Funcao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Funcao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalFuncao = new DAL.SQL.Funcao(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalFuncao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFuncao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFuncao.GetAll();
        }

        public List<Modelo.Funcao> GetAllList()
        {
            return dalFuncao.GetAllList();
        }

        public Modelo.Funcao LoadObject(int id)
        {
            return dalFuncao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Funcao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigat�rio.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigat�rio.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Funcao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        if (dalFuncao.BuscaFuncao(objeto.Descricao))
                        {
                            ret.Add("txtDescricao", "J� existe um cadastro de fun��o com esse nome. Verifique!");
                        }
                        else
                        {
                            dalFuncao.Incluir(objeto);
                        }
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalFuncao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFuncao.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// M�todo respons�vel em retornar o id da tabela. O campo padr�o para busca � o campo c�digo, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso n�o desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo C�digo</param>
        /// <param name="pCampo">Nome do segundo campo que ser� utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalFuncao.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// M�todo respons�vel em retornar o id de uma fun��o, passando como par�metro o nome da fun��o
        /// </summary>
        /// <param name="pNomeDescricao">Nome da Fun��o</param>
        /// <returns>ID da Fun��o</returns>
        public int? getFuncaoNome(string pNomeDescricao)
        {
            return dalFuncao.getFuncaoNome(pNomeDescricao);
        }

        public int? GetIdPorCod(int Cod) 
        {
            return dalFuncao.GetIdPorCod(Cod);
        }

        public Modelo.Funcao LoadObjectByCodigo(int idFuncao)
        {
            return dalFuncao.LoadObjectByCodigo(idFuncao);
        }

        public int? GetIdPorIdIntegracao(int? idIntegracao)
        {
            return dalFuncao.GetIdPorIdIntegracao(idIntegracao);
        }
    }
}
