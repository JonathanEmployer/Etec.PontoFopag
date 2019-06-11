using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class OcorrenciaEmpresa : IBLL<Modelo.OcorrenciaEmpresa>
    {
        DAL.IOcorrenciaEmpresa dalOcorrenciaEmpresa;
        DAL.IOcorrencia dalOcorrencia;
        private string ConnectionString;

        public OcorrenciaEmpresa() : this(null)
        {
            
        }

        public OcorrenciaEmpresa(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public OcorrenciaEmpresa(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            switch (Modelo.cwkGlobal.BD)
            {
                case 2:
                    dalOcorrenciaEmpresa = DAL.FB.OcorrenciaEmpresa.GetInstancia;
                    dalOcorrencia = DAL.FB.Ocorrencia.GetInstancia;
                    break;
                default:
                    dalOcorrenciaEmpresa = new DAL.SQL.OcorrenciaEmpresa(new DataBase(ConnectionString));
                    dalOcorrencia = new DAL.SQL.Ocorrencia(new DataBase(ConnectionString));
                    break;
            }

            dalOcorrencia.UsuarioLogado = usuarioLogado;
            dalOcorrenciaEmpresa.UsuarioLogado = usuarioLogado;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.OcorrenciaEmpresa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.OcorrenciaEmpresa objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalOcorrenciaEmpresa.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalOcorrenciaEmpresa.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalOcorrenciaEmpresa.Excluir(objeto);
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
            return dalOcorrenciaEmpresa.getId(pValor, pCampo, pValor2);
        }

        public Modelo.OcorrenciaEmpresa LoadObjectByCodigo(int pCodigo)
        {
            return dalOcorrenciaEmpresa.LoadObjectByCodigo(pCodigo);
        }

        public DataTable GetAll()
        {
            return dalOcorrenciaEmpresa.GetAll();
        }

        public Modelo.OcorrenciaEmpresa LoadObject(int id)
        {
            return dalOcorrenciaEmpresa.LoadObject(id);
        }

        public List<Modelo.OcorrenciaEmpresa> GetAllList()
        {
            return dalOcorrenciaEmpresa.GetAllList();
        }

        public List<Modelo.OcorrenciaEmpresa> GetAllExibePainel(int idEmpresa)
        {
            List<Modelo.OcorrenciaEmpresa> lstOcorrenciasEmpresa = dalOcorrenciaEmpresa.GetAllPorExibePainelRHPorEmpresa(idEmpresa);
            return lstOcorrenciasEmpresa;
        }

        public void DeleteAllByIdEmpresa(int idEmpresa)
        {
            dalOcorrenciaEmpresa.DeleteAllByIdEmpresa(idEmpresa);
        }

        public void IncluirOcorrenciasEmpresa(List<Modelo.OcorrenciaEmpresa> ocorrenciasEmpresa)
        {
            dalOcorrenciaEmpresa.IncluirOcorrenciasEmpresa(ocorrenciasEmpresa);
        }
    }
}
