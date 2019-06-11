using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class TipoVinculo : IBLL<Modelo.TipoVinculo>
    {
        DAL.ITipoVinculo dalTipoVinculo;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public TipoVinculo()
            : this(null)
        {

        }

        public TipoVinculo(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public TipoVinculo(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalTipoVinculo = new DAL.SQL.TipoVinculo(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalTipoVinculo.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalTipoVinculo.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalTipoVinculo.GetAll();
        }

        public List<Modelo.TipoVinculo> GetAllList()
        {
            return dalTipoVinculo.GetAllList();
        }

        public Modelo.TipoVinculo LoadObject(int id)
        {
            return dalTipoVinculo.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.TipoVinculo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.TipoVinculo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        if (dalTipoVinculo.BuscaTipoVinculo(objeto.Descricao))
                        {
                            ret.Add("txtDescricao", "Já existe um cadastro de Tipo Vínculo com esse nome. Verifique!");
                        }
                        else
                        {
                            dalTipoVinculo.Incluir(objeto);
                        }
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalTipoVinculo.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalTipoVinculo.Excluir(objeto);
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
            return dalTipoVinculo.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Método responsável em retornar o id de uma Alocação, passando como parâmetro o nome da Alocação
        /// </summary>
        /// <param name="pNomeDescricao">Nome da Alocação</param>
        /// <returns>ID da Alocação</returns>
        public int? getTipoVinculoNome(string pNomeDescricao)
        {
            return dalTipoVinculo.getTipoVinculoNome(pNomeDescricao);
        }

        public int? GetIdPorCod(int Cod)
        {
            return dalTipoVinculo.GetIdPorCod(Cod);
        }

        public Modelo.TipoVinculo LoadObjectByCodigo(int idTipoVinculo)
        {
            return dalTipoVinculo.LoadObjectByCodigo(idTipoVinculo);
        }
    }
    
}
