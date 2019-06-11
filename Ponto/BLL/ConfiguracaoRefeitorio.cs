using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL.SQL;

namespace BLL
{
    public class ConfiguracaoRefeitorio : IBLL<Modelo.ConfiguracaoRefeitorio>
    {
        DAL.SQL.ConfiguracaoRefeitorio dalConfiguracaoRefeitorio;
        private string ConnectionString;

        public ConfiguracaoRefeitorio()
            : this(null)
        {

        }

        public ConfiguracaoRefeitorio(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalConfiguracaoRefeitorio = new DAL.SQL.ConfiguracaoRefeitorio(new DataBase(ConnectionString));
            dalConfiguracaoRefeitorio.UsuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
        }

        public DataTable GetAll()
        {
            return dalConfiguracaoRefeitorio.GetAll();
        }

        public Modelo.ConfiguracaoRefeitorio LoadObject(int id)
        {
            return dalConfiguracaoRefeitorio.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ConfiguracaoRefeitorio objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.TipoConexao == -1)
            {
                ret.Add("rgTipoConexao", "Campo obrigatório.");
            }
            else
            {
                if (objeto.TipoConexao == 0)
                {
                    if (objeto.Porta == -1)
                    {
                        ret.Add("rgPorta", "Campo obrigatório.");
                    }
                }

                if (objeto.TipoConexao == 2)
                {
                    if (objeto.PortaTCP == 0)
                    {
                        ret.Add("txtPortaTCP", "Campo obrigatório.");
                    }
                }
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ConfiguracaoRefeitorio objeto)
        {
            Modelo.ConfiguracaoRefeitorio objConfiguracaoRefeitorio = LoadObject(1);

            objeto.Id = objConfiguracaoRefeitorio.Id;

            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
                if (objeto.Id == 1)
                    dalConfiguracaoRefeitorio.Alterar(objeto);
                else
                    dalConfiguracaoRefeitorio.Incluir(objeto);

            return erros;
        }

        public int CountRegistros()
        {
            return dalConfiguracaoRefeitorio.CountCampo("configuracaorefeitorio", "id", 1, 1);
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalConfiguracaoRefeitorio.getId(pValor, pCampo, pValor2);
        }
    }
}