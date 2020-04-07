using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class EnvioConfiguracoesDataHora
    {
        DAL.IEnvioConfiguracoesDataHora dalEnvioConfiguracoesDataHora;
        private string ConnectionString;
        public EnvioConfiguracoesDataHora() : this(null)
        {

        }

        public EnvioConfiguracoesDataHora(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public EnvioConfiguracoesDataHora(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalEnvioConfiguracoesDataHora = new DAL.SQL.EnvioConfiguracoesDataHora(new DataBase(ConnectionString));
            dalEnvioConfiguracoesDataHora.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalEnvioConfiguracoesDataHora.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalEnvioConfiguracoesDataHora.GetAll();
        }

        public Modelo.EnvioConfiguracoesDataHora LoadObject(int id)
        {
            return dalEnvioConfiguracoesDataHora.LoadObject(id);
        }

        public Modelo.EnvioConfiguracoesDataHora LoadObjectByID(int id)
        {
            return dalEnvioConfiguracoesDataHora.LoadObjectByID(id);
        }

        public IList<Modelo.EnvioConfiguracoesDataHora> LoadListByIDRep(int idRep)
        {
            return dalEnvioConfiguracoesDataHora.LoadListByIDRep(idRep);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.EnvioConfiguracoesDataHora objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.EnvioConfiguracoesDataHora objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalEnvioConfiguracoesDataHora.Incluir(objeto);
                        REP bllRep = new REP(ConnectionString);
                        Modelo.REP rep = bllRep.LoadObject(objeto.idRelogio);
                        ServicoPontoCom.EnviaMensagemServicoPontoCom(rep.NumSerie, Modelo.Enumeradores.PontoComFuncoes.EnviarConfiguracoesDataHora);
                        break;
                    case Modelo.Acao.Excluir:
                        dalEnvioConfiguracoesDataHora.Excluir(objeto);
                        break;
                    default:
                        break;
                }
            }
            return erros;
        }
    }
}
