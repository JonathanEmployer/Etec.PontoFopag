using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class HorarioDinamicoRestricao : IBLL<Modelo.HorarioDinamicoRestricao>
    {
        DAL.IHorarioDinamicoRestricao dalHorarioDinamicoRestricao;
        private string ConnectionString;

        public HorarioDinamicoRestricao() : this(null)
        {
            
        }

        public HorarioDinamicoRestricao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public HorarioDinamicoRestricao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalHorarioDinamicoRestricao = new DAL.SQL.HorarioDinamicoRestricao(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalHorarioDinamicoRestricao = new DAL.SQL.HorarioDinamicoRestricao(new DataBase(ConnectionString));
                    break;
            }
            dalHorarioDinamicoRestricao.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioDinamicoRestricao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorarioDinamicoRestricao.GetAll();
        }

        public Modelo.HorarioDinamicoRestricao LoadObject(int id)
        {
            return dalHorarioDinamicoRestricao.LoadObject(id);
        }

        public List<Modelo.HorarioDinamicoRestricao> GetAllList()
        {
            return dalHorarioDinamicoRestricao.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDinamicoRestricao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigat�rio.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioDinamicoRestricao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDinamicoRestricao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDinamicoRestricao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDinamicoRestricao.Excluir(objeto);
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
            return dalHorarioDinamicoRestricao.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// M�todo respons�vel por carregas as restri��es dos hor�rios
        /// </summary>
        /// <param name="idsHorario">lista de ids dos hor�rios a terem as restri��es carregadas</param>
        /// <returns></returns>
        public List<Modelo.HorarioDinamicoRestricao> GetAllListByHorarios(List<int> idsHorario)
        {
            return dalHorarioDinamicoRestricao.GetAllListByHorarios(idsHorario);
        }
    }
}
