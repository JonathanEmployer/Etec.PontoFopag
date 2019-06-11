using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class EnvioDadosRepDet
    {
        DAL.IEnvioDadosRepDet dalEnvioEmpresaFuncionariosRepDet;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public EnvioDadosRepDet() : this(null)
        {

        }

        public EnvioDadosRepDet(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EnvioDadosRepDet(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
            dalEnvioEmpresaFuncionariosRepDet = new DAL.SQL.EnvioDadosRepDet(new DataBase(ConnectionString));
            dalEnvioEmpresaFuncionariosRepDet.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEnvioEmpresaFuncionariosRepDet.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEnvioEmpresaFuncionariosRepDet.GetAll();
        }

        public Modelo.EnvioDadosRepDet LoadObject(int id)
        {
            return dalEnvioEmpresaFuncionariosRepDet.LoadObject(id);
        }

        public List<Modelo.EnvioDadosRepDet> getListByRep(int idRep)
        {
            return dalEnvioEmpresaFuncionariosRepDet.getListByRep(idRep);
        }

        public List<Modelo.EnvioDadosRepDet> getByIdEnvioDadosRep(int idEnvioDadosRep)
        {
            return dalEnvioEmpresaFuncionariosRepDet.getByIdEnvioDadosRep(idEnvioDadosRep);
        }


        public Dictionary<string, string> ValidaObjeto(Modelo.EnvioDadosRepDet objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Valor tem que ser diferente de zero (0).");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EnvioDadosRepDet objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEnvioEmpresaFuncionariosRepDet.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEnvioEmpresaFuncionariosRepDet.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEnvioEmpresaFuncionariosRepDet.Excluir(objeto);
                        break;
                    default:
                        break;
                }
            }
            return erros;
        }

    }
}
