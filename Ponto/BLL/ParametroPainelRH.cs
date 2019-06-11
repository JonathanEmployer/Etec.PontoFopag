using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class ParametroPainelRH : IBLL<Modelo.ParametroPainelRH>
    {
        DAL.IParametroPainelRH dalParametroPainelRH;
        private string ConnectionString;

        public ParametroPainelRH() : this(null)
        {
            
        }

        public ParametroPainelRH(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ParametroPainelRH(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalParametroPainelRH = new DAL.SQL.ParametroPainelRH(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalParametroPainelRH = new DAL.SQL.ParametroPainelRH(new DataBase(ConnectionString));
                    break;
            }
            dalParametroPainelRH.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalParametroPainelRH.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalParametroPainelRH.GetAll();
        }

        public Modelo.ParametroPainelRH LoadObject(int id)
        {
            return dalParametroPainelRH.LoadObject(id);
        }

        public List<Modelo.ParametroPainelRH> GetAllList()
        {
            return dalParametroPainelRH.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ParametroPainelRH objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ParametroPainelRH objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalParametroPainelRH.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalParametroPainelRH.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalParametroPainelRH.Excluir(objeto);
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
            return dalParametroPainelRH.getId(pValor, pCampo, pValor2);
        }
    }
}
