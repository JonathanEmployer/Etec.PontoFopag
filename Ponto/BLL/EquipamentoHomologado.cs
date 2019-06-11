using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class EquipamentoHomologado : IBLL<Modelo.EquipamentoHomologado>
    {
        DAL.IEquipamentoHomologado dalEquipamentoHomologado;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public EquipamentoHomologado() : this(null)
        {
            
        }

        public EquipamentoHomologado(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EquipamentoHomologado(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalEquipamentoHomologado = new DAL.SQL.EquipamentoHomologado(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalEquipamentoHomologado.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEquipamentoHomologado.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEquipamentoHomologado.GetAll();
        }

        public List<Modelo.EquipamentoHomologado> GetAllList()
        {
            return dalEquipamentoHomologado.GetAllList();
        }

        public Modelo.EquipamentoHomologado LoadObject(int id)
        {
            return dalEquipamentoHomologado.LoadObject(id);
        }

        public Modelo.EquipamentoHomologado LoadByCodigoModelo(string numSerie)
        {
            return dalEquipamentoHomologado.LoadByCodigoModelo(numSerie);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EquipamentoHomologado objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();         
            
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EquipamentoHomologado objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEquipamentoHomologado.Incluir(objeto);
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalEquipamentoHomologado.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEquipamentoHomologado.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalEquipamentoHomologado.getId(pValor, pCampo, pValor2);
        }

    }
}
