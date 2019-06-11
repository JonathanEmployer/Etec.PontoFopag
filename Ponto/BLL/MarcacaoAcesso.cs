using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace BLL
{
    public class MarcacaoAcesso : IBLL<Modelo.MarcacaoAcesso>
    {
        DAL.IMarcacaoAcesso dalMarcacaoAcesso;
        private string ConnectionString;

        public MarcacaoAcesso()
            : this(null)
        {

        }

        public MarcacaoAcesso(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalMarcacaoAcesso = new DAL.SQL.MarcacaoAcesso(new DataBase(ConnectionString));
        }

        public int MaxCodigo()
        {
            return dalMarcacaoAcesso.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalMarcacaoAcesso.GetAll();
        }

        public Modelo.MarcacaoAcesso LoadObject(int id)
        {
            return dalMarcacaoAcesso.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.MarcacaoAcesso objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.MarcacaoAcesso objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalMarcacaoAcesso.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalMarcacaoAcesso.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalMarcacaoAcesso.Excluir(objeto);
                        break;
                    default:
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalMarcacaoAcesso.getId(pValor, pCampo, pValor2);
        }

        public byte VerificaUltimaMarcacao(int pFuncionario)
        {
            return dalMarcacaoAcesso.VerificaUltimaMarcacao(pFuncionario);
        }

        public DataTable GetAcessoDia(DateTime pData)
        {
            return dalMarcacaoAcesso.GetAcessoDia(pData);
        }

        public DataTable GetAcessoPessoa(int pFuncionario)
        {
            return dalMarcacaoAcesso.GetAcessoPessoa(pFuncionario);
        }

        public DataTable GetAcessosAnaliticos(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            return dalMarcacaoAcesso.getAcessosAnaliticos(dataInicial,dataFinal,empresas,departamentos,funcionarios,tipo);
        }

        public DataTable GetAcessosSintaticos(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            return dalMarcacaoAcesso.getAcessosSintaticos(dataInicial, dataFinal, empresas, departamentos, funcionarios, tipo);
        }
    }
}