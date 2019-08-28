using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Ocorrencia : IBLL<Modelo.Ocorrencia>
    {
        DAL.IOcorrencia dalOcorrencia;
        private string ConnectionString;

        public Ocorrencia() : this(null)
        {
            
        }

        public Ocorrencia(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Ocorrencia(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalOcorrencia = new DAL.SQL.Ocorrencia(new DataBase(ConnectionString));
            dalOcorrencia.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalOcorrencia.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalOcorrencia.GetAll();
        }

        public DataTable GetAllComOpcaoTodos()
        {
            DataTable dt = dalOcorrencia.GetAll();
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, "Todas as Ocorrências", 0, false };
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        public Modelo.Ocorrencia LoadObject(int id)
        {
            return dalOcorrencia.LoadObject(id);
        }

        public Hashtable GetHashIdDescricao()
        {
            return dalOcorrencia.GetHashIdDescricao();
        }

        public List<Modelo.Ocorrencia> GetAllList(bool validaPermissaoUser)
        {
            return dalOcorrencia.GetAllList(validaPermissaoUser);
        }

        public List<Modelo.Ocorrencia> GetAllListConsultaEvento(bool validaPermissaoUser)
        {
            return dalOcorrencia.GetAllListConsultaEvento(validaPermissaoUser);
        }

        public List<Modelo.Ocorrencia> GetAllPorExibePainelRHPorEmpresa(int idEmpresa)
        {
            return dalOcorrencia.GetAllPorExibePainelRHPorEmpresa(idEmpresa);
        }

        public List<Modelo.Proxy.pxyOcorrenciaEvento> GetAllOcorrenciaEventoList()
        {
            return dalOcorrencia.GetAllOcorrenciaEventoList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Ocorrencia objeto)
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

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Ocorrencia objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalOcorrencia.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalOcorrencia.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalOcorrencia.Excluir(objeto);
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
            return dalOcorrencia.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Método responsável em retornar o id da ocorrencia, passando como parâmetro o nome da ocorrencia
        /// </summary>
        /// <param name="pNomeDescricao">Descrição da Ocorrencia</param>
        /// <returns>ID da Ocorrencia</returns>
        public int? getOcorrenciaNome(string pDescricao)
        {
            return dalOcorrencia.getOcorrenciaNome(pDescricao);
        }

        public Modelo.Ocorrencia LoadObjectByCodigo(int pCodigo, bool validaPermissaoUser)
        {
            return dalOcorrencia.LoadObjectByCodigo(pCodigo, validaPermissaoUser);
        }

        public List<Modelo.Ocorrencia> GetAllListPorIds(List<int> ids)
        {
            return dalOcorrencia.GetAllListPorIds(ids);
        }

        public List<Modelo.Ocorrencia> GetAllPorExibePaineldoRH()
        {
            return dalOcorrencia.GetAllPorExibePaineldoRH();
        }


        public int? GetIdPorIdIntegracao(int idIntegracao)
        {
            return dalOcorrencia.GetIdPorIdIntegracao(idIntegracao);
        }
    }
}
