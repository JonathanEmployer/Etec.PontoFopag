using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class LoteCalculoFuncionario : IBLL<Modelo.LoteCalculoFuncionario>
    {
        DAL.ILoteCalculoFuncionario dalLoteCalculoFuncionario;
        private string ConnectionString;

        public LoteCalculoFuncionario() : this(null)
        {
            
        }

        public LoteCalculoFuncionario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LoteCalculoFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalLoteCalculoFuncionario = new DAL.SQL.LoteCalculoFuncionario(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLoteCalculoFuncionario = new DAL.SQL.LoteCalculoFuncionario(new DataBase(ConnectionString));
                    break;
            }
            dalLoteCalculoFuncionario.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLoteCalculoFuncionario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLoteCalculoFuncionario.GetAll();
        }

        public Modelo.LoteCalculoFuncionario LoadObject(int id)
        {
            return dalLoteCalculoFuncionario.LoadObject(id);
        }

        public List<Modelo.LoteCalculoFuncionario> GetAllList()
        {
            return dalLoteCalculoFuncionario.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LoteCalculoFuncionario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LoteCalculoFuncionario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLoteCalculoFuncionario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLoteCalculoFuncionario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLoteCalculoFuncionario.Excluir(objeto);
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
            return dalLoteCalculoFuncionario.getId(pValor, pCampo, pValor2);
        }
    }
}
