using System;
using DAL.SQL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BLL
{
    public class EmpresaLogo : IBLL<Modelo.EmpresaLogo>
    {
        DAL.IEmpresaLogo dalEmpresaLogo;


        private string ConnectionString;

        public EmpresaLogo()
            : this(null)
        {

        }

        public EmpresaLogo(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EmpresaLogo(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalEmpresaLogo = new DAL.SQL.EmpresaLogo(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalEmpresaLogo = new DAL.SQL.EmpresaLogo(new DataBase(ConnectionString));
                    break;
            }
            dalEmpresaLogo.UsuarioLogado = usuarioLogado;
        }
        public int MaxCodigo()
        {
            return dalEmpresaLogo.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEmpresaLogo.GetAll();
        }

        public Modelo.EmpresaLogo LoadObject(int id)
        {
            return dalEmpresaLogo.LoadObject(id);
        }
        public Dictionary<string, string> ValidaObjeto(Modelo.EmpresaLogo objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EmpresaLogo objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEmpresaLogo.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalEmpresaLogo.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEmpresaLogo.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalEmpresaLogo.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.EmpresaLogo> GetAllListPorEmpresa(int IDEmpresa)
        {
            return dalEmpresaLogo.GetAllListPorEmpresa(IDEmpresa);
        }

    }
}
