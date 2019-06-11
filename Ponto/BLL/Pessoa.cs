using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Pessoa : IBLL<Modelo.Pessoa>
    {
        DAL.IPessoa dalPessoa;
        private string ConnectionString;

        public Pessoa() : this(null)
        {
            
        }

        public Pessoa(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Pessoa(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalPessoa = new DAL.SQL.Pessoa(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalPessoa = new DAL.SQL.Pessoa(new DataBase(ConnectionString));
                    break;
            }
            dalPessoa.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalPessoa.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalPessoa.GetAll();
        }

        public Modelo.Pessoa LoadObject(int id)
        {
            return dalPessoa.LoadObject(id);
        }

        public List<Modelo.Pessoa> GetAllList()
        {
            return dalPessoa.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Pessoa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            objeto.FormatarCNPJ_CPF();
            if (objeto.TipoPessoa == 0 && !BLL.cwkFuncoes.ValidarCPF(objeto.CNPJ_CPF))
            {
                ret.Add("CNPJ_CPF", "CPF inválido.");
            }
            else if (objeto.TipoPessoa == 1 && !BLL.cwkFuncoes.ValidarCNPJ(objeto.CNPJ_CPF))
            {
                ret.Add("CNPJ_CPF", "CNPJ inválido.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Pessoa objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalPessoa.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalPessoa.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalPessoa.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }


        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalPessoa.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Retorna uma lista com as pessoas que tenham o código igual ao informado
        /// </summary>
        /// <param name="codigo">Código da pessoa</param>
        /// <returns>List de pessoas</returns>
        public List<Modelo.Pessoa> GetPessoaPorCodigo(int codigo)
        {
            return dalPessoa.GetPessoaPorCodigo(codigo);
        }

        /// <summary>
        /// Retorna a lista de pessoas que contenham o nome passado por parâmetro
        /// </summary>
        /// <param name="nome">Nome a ser pesquisado</param>
        /// <returns>Lista de Pessoas</returns>
        public List<Modelo.Pessoa> GetListPessoaPorNome(string nome)
        {
            return dalPessoa.GetListPessoaPorNome(nome);
        }

        /// <summary>
        /// Retorna o Id da pessoa por IdIntegração
        /// </summary>
        /// <param name="idIntegracao">IdIntegracao a ser pesquisado</param>
        /// <returns>IdPessoa</returns>
        public int? GetIdPoridIntegracao(int idIntegracao)
        {
            return dalPessoa.GetIdPorIdIntegracao(idIntegracao);
        }

        /// <summary>
        /// Retorna uma lista com as pessoas que tenham o CNPJ_CPF igual ao informado
        /// </summary>
        /// <param name="CNPJ_CPF">CNPJ_CPF da pessoa</param>
        /// <returns>List de pessoas</returns>
        public List<Modelo.Pessoa> GetPessoaPorCNPJ_CPF(string CNPJ_CPF)
        {
            return dalPessoa.GetPessoaPorCNPJ_CPF(CNPJ_CPF);
        }
    }
}
