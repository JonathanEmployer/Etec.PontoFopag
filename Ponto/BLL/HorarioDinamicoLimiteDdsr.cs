using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class HorarioDinamicoLimiteDdsr : IBLL<Modelo.HorarioDinamicoLimiteDdsr>
    {
        DAL.IHorarioDinamicoLimiteDdsr dalHorarioDinamicoLimiteDdsr;
        private string ConnectionString;

        public HorarioDinamicoLimiteDdsr() : this(null)
        {
            
        }

        public HorarioDinamicoLimiteDdsr(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public HorarioDinamicoLimiteDdsr(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalHorarioDinamicoLimiteDdsr = new DAL.SQL.HorarioDinamicoLimiteDdsr(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalHorarioDinamicoLimiteDdsr = new DAL.SQL.HorarioDinamicoLimiteDdsr(new DataBase(ConnectionString));
                    break;
            }
            dalHorarioDinamicoLimiteDdsr.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioDinamicoLimiteDdsr.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorarioDinamicoLimiteDdsr.GetAll();
        }

        public Modelo.HorarioDinamicoLimiteDdsr LoadObject(int id)
        {
            return dalHorarioDinamicoLimiteDdsr.LoadObject(id);
        }

        public List<Modelo.HorarioDinamicoLimiteDdsr> GetAllList()
        {
            return dalHorarioDinamicoLimiteDdsr.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDinamicoLimiteDdsr objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioDinamicoLimiteDdsr objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDinamicoLimiteDdsr.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDinamicoLimiteDdsr.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDinamicoLimiteDdsr.Excluir(objeto);
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
            return dalHorarioDinamicoLimiteDdsr.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.HorarioDinamicoLimiteDdsr> LoadObjectByHorarioDinamico(int idHorarioDinamico)
        {
            return dalHorarioDinamicoLimiteDdsr.LoadObjectByHorarioDinamico(idHorarioDinamico);
        }
    }
}
