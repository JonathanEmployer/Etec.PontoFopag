using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class CamposSelecionadosRelCartaoPonto : IBLL<Modelo.CamposSelecionadosRelCartaoPonto>
    {
        DAL.ICamposSelecionadosRelCartaoPonto dalCamposSelecionadosRelCartaoPonto;
        private string ConnectionString;

        public CamposSelecionadosRelCartaoPonto() : this(null)
        {
            
        }

        public CamposSelecionadosRelCartaoPonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public CamposSelecionadosRelCartaoPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalCamposSelecionadosRelCartaoPonto = new DAL.SQL.CamposSelecionadosRelCartaoPonto(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalCamposSelecionadosRelCartaoPonto = new DAL.SQL.CamposSelecionadosRelCartaoPonto(new DataBase(ConnectionString));
                    break;
            }
            dalCamposSelecionadosRelCartaoPonto.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalCamposSelecionadosRelCartaoPonto.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCamposSelecionadosRelCartaoPonto.GetAll();
        }

        public Modelo.CamposSelecionadosRelCartaoPonto LoadObject(int id)
        {
            return dalCamposSelecionadosRelCartaoPonto.LoadObject(id);
        }

        public List<Modelo.CamposSelecionadosRelCartaoPonto> GetAllList()
        {
            return dalCamposSelecionadosRelCartaoPonto.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.CamposSelecionadosRelCartaoPonto objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.CamposSelecionadosRelCartaoPonto objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalCamposSelecionadosRelCartaoPonto.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalCamposSelecionadosRelCartaoPonto.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCamposSelecionadosRelCartaoPonto.Excluir(objeto);
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
            return dalCamposSelecionadosRelCartaoPonto.getId(pValor, pCampo, pValor2);
        }
    }
}
