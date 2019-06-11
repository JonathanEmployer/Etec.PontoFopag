using cwkPontoMT.Integracao;
using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

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

            //objeto.Empresas = objeto.Empresas == null ? new List<Modelo.Empresa>() : objeto.Empresas;
            //objeto.Funcionarios = objeto.Funcionarios == null ? new List<Modelo.Funcionario>() : objeto.Funcionarios;
            //List<Modelo.Funcionario> funcionariosSelecionados = objeto.Funcionarios.Where(f => f.Selecionado == true).ToList();
            //List<Modelo.Empresa> empresasSelecionadas = objeto.Empresas.Where(e => e.Selecionado == true).ToList();

            //if (objeto.Codigo == 0)
            //{
            //    ret.Add("txtCodigo", "Valor do Código tem que ser diferente de zero (0).");
            //}

            //if (objeto.bEnviarEmpresa &&
            //(objeto.Empresas.Count >= 0) && (empresasSelecionadas.Count() == 0))
            //{
            //    ret.Add("Grid Empresas", "Nenhum Empresa selecionada.");
            //}

            //if (objeto.bEnviarFunc &&
            //    (objeto.Funcionarios.Count >= 0) && (funcionariosSelecionados.Count() == 0))
            //{
            //    ret.Add("Grid Funcionários", "Nenhum Funcionário selecionado.");
            //}

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
