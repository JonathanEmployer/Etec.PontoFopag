using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Classificacao : IBLL<Modelo.Classificacao>
    {
        DAL.IClassificacao dalClassificacao;
        private string ConnectionString;

        public Classificacao() : this(null)
        {
            
        }

        public Classificacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Classificacao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalClassificacao = new DAL.SQL.Classificacao(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalClassificacao = new DAL.SQL.Classificacao(new DataBase(ConnectionString));
                    break;
            }
            dalClassificacao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalClassificacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalClassificacao.GetAll();
        }

        public Modelo.Classificacao LoadObject(int id)
        {
            return dalClassificacao.LoadObject(id);
        }

        public List<Modelo.Classificacao> GetAllList()
        {
            return dalClassificacao.GetAllList();
        }

        public List<Modelo.Classificacao> GetAllPorExibePaineldoRH()
        {
            return dalClassificacao.GetAllPorExibePaineldoRH();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Classificacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Classificacao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalClassificacao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalClassificacao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalClassificacao.Excluir(objeto);
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
            return dalClassificacao.getId(pValor, pCampo, pValor2);
        }

        public int? GetIdPorCod(int cod)
        {
            return dalClassificacao.GetIdPorCod(cod);
        }
    }
}
