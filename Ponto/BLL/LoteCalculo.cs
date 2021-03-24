using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class LoteCalculo : IBLL<Modelo.LoteCalculo>
    {
        DAL.ILoteCalculo dalLoteCalculo;
        private string ConnectionString;

        public LoteCalculo() : this(null)
        {
            
        }

        public LoteCalculo(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LoteCalculo(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalLoteCalculo = new DAL.SQL.LoteCalculo(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLoteCalculo = new DAL.SQL.LoteCalculo(new DataBase(ConnectionString));
                    break;
            }
            dalLoteCalculo.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLoteCalculo.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLoteCalculo.GetAll();
        }

        public Modelo.LoteCalculo LoadObject(int id)
        {
            return dalLoteCalculo.LoadObject(id);
        }

        public List<Modelo.LoteCalculo> GetAllList()
        {
            return dalLoteCalculo.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LoteCalculo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0 && objeto.NaoValidaCodigo == false)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LoteCalculo objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLoteCalculo.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLoteCalculo.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLoteCalculo.Excluir(objeto);
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
            return dalLoteCalculo.getId(pValor, pCampo, pValor2);
        }
    }
}
