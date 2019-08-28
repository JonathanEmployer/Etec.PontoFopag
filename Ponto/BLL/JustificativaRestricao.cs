using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class JustificativaRestricao : IBLL<Modelo.JustificativaRestricao>
    {
        DAL.IJustificativaRestricao dalJustificativaRestricao;
        private string ConnectionString;

        public JustificativaRestricao() : this(null)
        {
            
        }

        public JustificativaRestricao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public JustificativaRestricao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalJustificativaRestricao = new DAL.SQL.JustificativaRestricao(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalJustificativaRestricao = new DAL.SQL.JustificativaRestricao(new DataBase(ConnectionString));
                    break;
            }
            dalJustificativaRestricao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJustificativaRestricao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJustificativaRestricao.GetAll();
        }

        public Modelo.JustificativaRestricao LoadObject(int id)
        {
            return dalJustificativaRestricao.LoadObject(id);
        }

        public List<Modelo.JustificativaRestricao> GetAllList()
        {
            return dalJustificativaRestricao.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.JustificativaRestricao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.JustificativaRestricao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalJustificativaRestricao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalJustificativaRestricao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJustificativaRestricao.Excluir(objeto);
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
            return dalJustificativaRestricao.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Método responsável por carregas as restrições dos horários
        /// </summary>
        /// <param name="idsHorario">lista de ids dos horários a terem as restrições carregadas</param>
        /// <returns></returns>
        public List<Modelo.JustificativaRestricao> GetAllListByJustificativas(List<int> idsJustificativa)
        {
            return dalJustificativaRestricao.GetAllListByJustificativas(idsJustificativa);
        }
    }
}
