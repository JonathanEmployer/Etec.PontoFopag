using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ParametrosInternos : IBLL<Modelo.ParametrosInternos>
    {
        DAL.IParametrosInternos dalParametrosInternos;
        private string ConnectionString;

        public ParametrosInternos() : this(null)
        {

        }

        public ParametrosInternos(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }
        public ParametrosInternos(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            dalParametrosInternos = new DAL.SQL.ParametrosInternos(new DataBase(ConnectionString));

            if (usuarioLogado != null)
            {
                dalParametrosInternos.UsuarioLogado = usuarioLogado;
            }
        }
        public DataTable GetAll()
        {
            return dalParametrosInternos.GetAll();
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalParametrosInternos.getId(pValor, pCampo, pValor2);
        }

        public Modelo.ParametrosInternos LoadObject(int id)
        {
            return dalParametrosInternos.LoadObject(id);
        }

        public Dictionary<string, string> Salvar(Acao pAcao, Modelo.ParametrosInternos objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalParametrosInternos.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalParametrosInternos.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalParametrosInternos.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ParametrosInternos objeto)
        {
            return new Dictionary<string, string>();
        }

        public Modelo.ParametrosInternos LoadFirtObject()
        {
            return dalParametrosInternos.LoadFirtObject();
        }
    }
}
