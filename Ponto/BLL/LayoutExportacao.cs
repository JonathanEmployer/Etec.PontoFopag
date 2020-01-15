using System;
using System.Collections.Generic;
using System.Data;
using DAL.SQL;

namespace BLL
{
    public class LayoutExportacao : IBLL<Modelo.LayoutExportacao>
    {
        DAL.ILayoutExportacao dalLayoutExportacao;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public LayoutExportacao() : this(null)
        {
            
        }

        public LayoutExportacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LayoutExportacao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalLayoutExportacao = new DAL.SQL.LayoutExportacao(new DataBase(ConnectionString), usuarioLogado);
            UsuarioLogado = usuarioLogado;
            dalLayoutExportacao.UsuarioLogado = UsuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLayoutExportacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLayoutExportacao.GetAll();
        }

        public List<Modelo.LayoutExportacao> GetAllList()
        {
            return dalLayoutExportacao.GetAllList();
        }

        public Modelo.LayoutExportacao LoadObject(int id)
        {
            return dalLayoutExportacao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LayoutExportacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
 
            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }
            else
            {
                objeto.Descricao = objeto.Descricao.TrimEnd();
                if (objeto.Descricao.Length == 0)
                {
                    ret.Add("txtDescricao", "Campo obrigatório.");
                }
                else if (objeto.Descricao.Length > 50)
                {
                    ret.Add("txtDescricao", "A descrição não pode ter mais de 50 caracteres.");
                }
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LayoutExportacao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLayoutExportacao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLayoutExportacao.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLayoutExportacao.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalLayoutExportacao.getId(pValor, pCampo, pValor2);
        }
    }
   
}
