using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class ParametroWebfopag : IBLL<Modelo.ParametroWebfopag>
    {
        DAL.IParametroWebfopag dalParametroWebfopag;
        private string ConnectionString;

        public ParametroWebfopag() : this(null)
        {
            
        }

        public ParametroWebfopag(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ParametroWebfopag(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalParametroWebfopag = new DAL.SQL.ParametroWebfopag(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalParametroWebfopag = new DAL.SQL.ParametroWebfopag(new DataBase(ConnectionString));
                    break;
            }
            dalParametroWebfopag.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalParametroWebfopag.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalParametroWebfopag.GetAll();
        }

        public Modelo.ParametroWebfopag LoadObject(int id)
        {
            return dalParametroWebfopag.LoadObject(id);
        }

        public List<Modelo.ParametroWebfopag> GetAllList()
        {
            return dalParametroWebfopag.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ParametroWebfopag objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ParametroWebfopag objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalParametroWebfopag.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalParametroWebfopag.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalParametroWebfopag.Excluir(objeto);
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
            return dalParametroWebfopag.getId(pValor, pCampo, pValor2);
        }
    }
}
