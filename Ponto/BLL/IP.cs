using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class IP : IBLL<Modelo.IP>
    {DAL.IIP dalIP;
        private string ConnectionString;

        public IP() : this(null)
        {
            
        }

        public IP(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public IP(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalIP = new DAL.SQL.IP(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalIP = new DAL.SQL.IP(new DataBase(ConnectionString));
                    break;
            }
            dalIP.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalIP.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalIP.GetAll();
        }

        public Modelo.IP LoadObject(int id)
        {
            return dalIP.LoadObject(id);
        }

        public List<Modelo.IP> GetAllList()
        {
            return dalIP.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.IP objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.IP objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalIP.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalIP.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalIP.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }


        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalIP.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.IP> GetAllListPorEmpresa(int IDEmpresa)
        {
            return dalIP.GetAllListPorEmpresa(IDEmpresa);
        }
    }
}
