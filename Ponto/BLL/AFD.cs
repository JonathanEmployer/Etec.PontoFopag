using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class AFD : IBLL<Modelo.AFD>
    {
        DAL.IAFD dalAFD;
        private string ConnectionString;

        public AFD() : this(null)
        {
            
        }

        public AFD(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public AFD(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalAFD = new DAL.SQL.AFD(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAFD = new DAL.SQL.AFD(new DataBase(ConnectionString));
                    break;
            }
            dalAFD.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAFD.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAFD.GetAll();
        }

        public Modelo.AFD LoadObject(int id)
        {
            return dalAFD.LoadObject(id);
        }

        public List<Modelo.AFD> GetAllList()
        {
            return dalAFD.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.AFD objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.AFD objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalAFD.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalAFD.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalAFD.Excluir(objeto);
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
            return dalAFD.getId(pValor, pCampo, pValor2);
        }


        public void InserirRegistros(List<Modelo.AFD> registros)
        {
            if (registros.Count > 0)
            {
                dalAFD.InserirRegistros(registros);
            }
        }

        public void AtualizarRegistros(List<Modelo.AFD> registros)
        {
            if (registros.Count > 0)
            {
                dalAFD.AtualizarRegistros(registros);
            }
        }

        public List<Modelo.AFD> GetAllListByLote(string lote, bool nolock)
        {
            DataTable dt = dalAFD.GetAllListByLote(lote, nolock);
            List<Modelo.AFD> ret = dt.DataTableMapToList<Modelo.AFD>();
            return ret;
        }

        public Modelo.AFD GetUltimoRegistroByOrigem(string origemRegistro)
        {
            Modelo.AFD ultimoAFD = dalAFD.GetUltimoRegistroByOrigem(origemRegistro);
            if (ultimoAFD != null && ultimoAFD.Nsr > 0 && ultimoAFD.DataHora == null)
                ultimoAFD.PreencheDataHora();

            return ultimoAFD;
        }
    }
}
