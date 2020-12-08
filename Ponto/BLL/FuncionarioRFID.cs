using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class FuncionarioRFID : IBLL<Modelo.FuncionarioRFID>
    {
        DAL.IFuncionarioRFID dalFuncionarioRFID;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public FuncionarioRFID() : this(null) { }

        public FuncionarioRFID(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public FuncionarioRFID(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalFuncionarioRFID = new DAL.SQL.FuncionarioRFID(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalFuncionarioRFID.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFuncionarioRFID.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFuncionarioRFID.GetAll();
        }

        public List<Modelo.FuncionarioRFID> GetAllList()
        {
            return dalFuncionarioRFID.GetAllList();
        }

        public List<Modelo.FuncionarioRFID> GetAllListByFuncionario(int idFuncionario, bool apenasAtivos)
        {
            return dalFuncionarioRFID.GetAllListByFuncionario(idFuncionario, apenasAtivos);
        }


        public Modelo.FuncionarioRFID LoadObject(int id)
        {
            return dalFuncionarioRFID.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FuncionarioRFID objeto)
        {
            //throw new Exception("não implementado");
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.IdFuncionario == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (objeto.RFID == 0 && string.IsNullOrEmpty(objeto.MIFARE))
            {
                ret.Add("txtRFID", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FuncionarioRFID objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = pAcao == Modelo.Acao.Alterar && objeto.Ativo == false ? new Dictionary<string, string>() : ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFuncionarioRFID.Incluir(objeto);
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalFuncionarioRFID.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFuncionarioRFID.Excluir(objeto);
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
            return dalFuncionarioRFID.getId(pValor, pCampo, pValor2);
        }

        public int? GetIdPorCod(int Cod)
        {
            return dalFuncionarioRFID.GetIdPorCod(Cod);
        }

        public List<Modelo.FuncionarioRFID> GetAllListByFuncionario(List<int> idsFuncs, bool apenasAtivos)
        {
            return dalFuncionarioRFID.GetAllListByFuncionario(idsFuncs, apenasAtivos);
        }
    }
}
