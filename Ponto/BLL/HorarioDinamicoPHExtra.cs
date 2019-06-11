using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class HorarioDinamicoPHExtra : IBLL<Modelo.HorarioDinamicoPHExtra>
    {
        DAL.IHorarioDinamicoPHExtra dalHorarioDinamicoPHExtra;
        private string ConnectionString;

        public HorarioDinamicoPHExtra() : this(null)
        {
            
        }

        public HorarioDinamicoPHExtra(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public HorarioDinamicoPHExtra(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalHorarioDinamicoPHExtra = new DAL.SQL.HorarioDinamicoPHExtra(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalHorarioDinamicoPHExtra = new DAL.SQL.HorarioDinamicoPHExtra(new DataBase(ConnectionString));
                    break;
            }
            dalHorarioDinamicoPHExtra.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorarioDinamicoPHExtra.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorarioDinamicoPHExtra.GetAll();
        }

        public Modelo.HorarioDinamicoPHExtra LoadObject(int id)
        {
            return dalHorarioDinamicoPHExtra.LoadObject(id);
        }

        public List<Modelo.HorarioDinamicoPHExtra> GetAllList()
        {
            return dalHorarioDinamicoPHExtra.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.HorarioDinamicoPHExtra objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.HorarioDinamicoPHExtra objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorarioDinamicoPHExtra.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorarioDinamicoPHExtra.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorarioDinamicoPHExtra.Excluir(objeto);
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
            return dalHorarioDinamicoPHExtra.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.HorarioDinamicoPHExtra> LoadObjectByHorarioDinamico(int idHorarioDinamico)
        {
            return dalHorarioDinamicoPHExtra.LoadObjectByHorarioDinamico(idHorarioDinamico);
        }

        public List<Modelo.HorarioDinamicoPHExtra> LoadObjectByHorarioDinamico(List<int> idsHorarioDinamico)
        {
            return dalHorarioDinamicoPHExtra.LoadObjectByHorarioDinamico(idsHorarioDinamico);
        }
    }
}
