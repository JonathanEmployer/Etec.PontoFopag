using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class EquipamentoTipoBiometria : IBLL<Modelo.EquipamentoTipoBiometria>
    {
        DAL.IEquipamentoTipoBiometria dalEquipamentoTipoBiometria;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public EquipamentoTipoBiometria() : this(null)
        {
            
        }

        public EquipamentoTipoBiometria(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EquipamentoTipoBiometria(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalEquipamentoTipoBiometria = new DAL.SQL.EquipamentoTipoBiometria(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalEquipamentoTipoBiometria.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEquipamentoTipoBiometria.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEquipamentoTipoBiometria.GetAll();
        }

        public List<Modelo.EquipamentoTipoBiometria> GetAllList()
        {
            return dalEquipamentoTipoBiometria.GetAllList();
        }

        public List<Modelo.Utils.ItensCombo> GetAllList(int IdEquipamentoTipoBiometria)
        {
            return dalEquipamentoTipoBiometria.GetAllList(IdEquipamentoTipoBiometria);
        }

        public Modelo.EquipamentoTipoBiometria LoadObject(int id)
        {
            return dalEquipamentoTipoBiometria.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EquipamentoTipoBiometria objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();         
            
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EquipamentoTipoBiometria objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEquipamentoTipoBiometria.Incluir(objeto);
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalEquipamentoTipoBiometria.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEquipamentoTipoBiometria.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalEquipamentoTipoBiometria.getId(pValor, pCampo, pValor2);
        }

    }
}
