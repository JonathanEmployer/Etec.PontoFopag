using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL.SQL;

namespace BLL
{
    public class LayoutImportacao : IBLL<Modelo.LayoutImportacao>
    {
        DAL.ILayoutImportacao DALImportacao;
        private string ConnectionString;

        public LayoutImportacao()
            : this(null)
        {

        }

        public LayoutImportacao(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            DALImportacao = new DAL.SQL.LayoutImportacao(new DataBase(ConnectionString));
            DALImportacao.UsuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
        }

        public int MaxCodigo()
        {
            return DALImportacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return DALImportacao.GetAll();
        }

        public List<Modelo.LayoutImportacao> GetAllList()
        {
            return DALImportacao.GetAllList();
        }

        public Modelo.LayoutImportacao LoadObject(int id)
        {
            return DALImportacao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LayoutImportacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LayoutImportacao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        DALImportacao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        DALImportacao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        DALImportacao.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return DALImportacao.getId(pValor, pCampo, pValor2);
        }

        public int QtdRegistrosLayout() 
        {
            return DALImportacao.QtdRegistrosLayout();
        }
    
    }
}
