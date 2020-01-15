using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class TransferenciaBilhetesDetalhes : IBLL<Modelo.TransferenciaBilhetesDetalhes>
    {
        DAL.ITransferenciaBilhetesDetalhes dalTransferenciaBilhetesDetalhes;
        private string ConnectionString;

        public TransferenciaBilhetesDetalhes() : this(null)
        {
            
        }

        public TransferenciaBilhetesDetalhes(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public TransferenciaBilhetesDetalhes(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalTransferenciaBilhetesDetalhes = new DAL.SQL.TransferenciaBilhetesDetalhes(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalTransferenciaBilhetesDetalhes = new DAL.SQL.TransferenciaBilhetesDetalhes(new DataBase(ConnectionString));
                    break;
            }
            dalTransferenciaBilhetesDetalhes.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalTransferenciaBilhetesDetalhes.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalTransferenciaBilhetesDetalhes.GetAll();
        }

        public Modelo.TransferenciaBilhetesDetalhes LoadObject(int id)
        {
            return dalTransferenciaBilhetesDetalhes.LoadObject(id);
        }

        public List<Modelo.TransferenciaBilhetesDetalhes> GetAllList()
        {
            return dalTransferenciaBilhetesDetalhes.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.TransferenciaBilhetesDetalhes objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigat�rio.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.TransferenciaBilhetesDetalhes objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalTransferenciaBilhetesDetalhes.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalTransferenciaBilhetesDetalhes.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalTransferenciaBilhetesDetalhes.Excluir(objeto);
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
            return dalTransferenciaBilhetesDetalhes.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.TransferenciaBilhetesDetalhes> GetAllListByTransferenciaBilhetes(int idTransferenciaBilhetes)
        {
            return GetAllListByTransferenciaBilhetes(idTransferenciaBilhetes);
        }
    }
}
