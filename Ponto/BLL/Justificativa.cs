using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Justificativa : IBLL<Modelo.Justificativa>
    {
        DAL.IJustificativa dalJustificativa;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Justificativa() : this(null)
        {
            
        }

        public Justificativa(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Justificativa(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalJustificativa = new DAL.SQL.Justificativa(new DataBase(ConnectionString));
            dalJustificativa.UsuarioLogado = usuarioLogado;
        }
        

        public int MaxCodigo()
        {
            return dalJustificativa.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJustificativa.GetAll();
        }

        public Modelo.Justificativa LoadObject(int id)
        {
            return dalJustificativa.LoadObject(id);
        }

        public Modelo.Justificativa LoadObjectByCodigo(int pCodigo)
        {
            return dalJustificativa.LoadObjectByCodigo(pCodigo);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Justificativa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao.TrimEnd()))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Justificativa objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        if (dalJustificativa.BuscaJustificativa(objeto.Descricao))
                        {
                            ret.Add("txtDescricao", "Já existe uma justificativa com esse nome. Verifique!");
                        }
                        else
                        {
                            dalJustificativa.Incluir(objeto);
                        }
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalJustificativa.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJustificativa.Excluir(objeto);
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
            return dalJustificativa.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.Justificativa> GetAllList(bool validaPermissaoUser)
        {
            return dalJustificativa.GetAllList(validaPermissaoUser);
        }

        public List<Modelo.Justificativa> GetAllListConsultaEvento(bool validaPermissaoUser)
        {
            return dalJustificativa.GetAllListConsultaEvento(validaPermissaoUser);
        }

        public List<Modelo.Justificativa> GetAllPorExibePaineldoRH()
        {
            return dalJustificativa.GetAllPorExibePaineldoRH();
        }

        public int? GetIdPorCod(int Cod, bool validaPermissaoUser)
        {
            return dalJustificativa.GetIdPorCod(Cod, validaPermissaoUser);
        }

        public int GetIdPorIdIntegracao(int IdIntegracao)
        {
            return dalJustificativa.GetIdPorIdIntegracao(IdIntegracao);
        }

        public Modelo.Justificativa LoadObjectByDescricao(string descricao)
        {
            return dalJustificativa.LoadObjectByDescricao(descricao);
        }

        public Modelo.Justificativa LoadObjectParaColetor()
        {
            return dalJustificativa.LoadObjectParaColetor();
        }

        public List<Modelo.Justificativa> GetAllListPorIds(List<int> ids)
        {
            return dalJustificativa.GetAllListPorIds(ids);
        }
    }
}
