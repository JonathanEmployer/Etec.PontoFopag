using System;
using System.Collections.Generic;
using System.Data;
using DAL.SQL;
using System.Collections;
using System.Data.SqlClient;

namespace BLL
{
    public partial class BilhetesImp : IBLL<Modelo.BilhetesImp>
    {
        private DAL.IBilheteSimp dalBilheteSimp;
        private DAL.IProvisorio dalProvisorio;
        private DAL.IEmpresa dalEmp;
        private DAL.IFuncionario dalFuncionario;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar progressBarVazia = new Modelo.ProgressBar();

        public BilhetesImp() : this(null)
        {
            
        }

        public BilhetesImp(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public BilhetesImp(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;
             else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            DataBase db = new DataBase(ConnectionString);
            dalBilheteSimp = new DAL.SQL.BilhetesImp(db);
            dalProvisorio = new DAL.SQL.Provisorio(db);
            dalEmp = new DAL.SQL.Empresa(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);

            dalEmp.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;
            dalBilheteSimp.UsuarioLogado = usuarioLogado;
            dalProvisorio.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
            progressBarVazia.incrementaPB = this.IncrementaProgressBarVazio;
            progressBarVazia.setaMensagem = this.SetaMensagemVazio;
            progressBarVazia.setaMinMaxPB = this.SetaMinMaxProgressBarVazio;
            progressBarVazia.setaValorPB = this.SetaValorProgressBarVazio;
        }

        public BilhetesImp(string connString, bool webApi)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            if (webApi)
            {
                DataBase db = new DataBase(ConnectionString);
                dalBilheteSimp = new DAL.SQL.BilhetesImp(db);
                dalProvisorio = new DAL.SQL.Provisorio(db);
                dalEmp = new DAL.SQL.Empresa(db);
                dalFuncionario = new DAL.SQL.Funcionario(db);
            }
        }

        public int MaxCodigo()
        {
            return dalBilheteSimp.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalBilheteSimp.GetAll();
        }

        public Modelo.BilhetesImp LoadObject(int id)
        {
            return dalBilheteSimp.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.BilhetesImp objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.BilhetesImp objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalBilheteSimp.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalBilheteSimp.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalBilheteSimp.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int Salvar(Modelo.Acao pAcao, List<Modelo.BilhetesImp> lista)
        {
            switch (pAcao)
            {
                case Modelo.Acao.Incluir:
                    return dalBilheteSimp.IncluirbilhetesEmLote(lista);
                case Modelo.Acao.Alterar:
                    return dalBilheteSimp.Alterar(lista);
                case Modelo.Acao.Excluir:
                    return dalBilheteSimp.Excluir(lista);
            }
            return 0;
        }

        public int Salvar(Modelo.Acao pAcao, List<Modelo.BilhetesImp> lista, string login, string conectionStr)
        {
            List<string> ret = new List<string>();
            return Salvar(pAcao, lista,  login,  conectionStr, out ret);
        }

        public int Salvar(Modelo.Acao pAcao, List<Modelo.BilhetesImp> lista, string login, string conectionStr, out List<string> dsCodigoFuncsProcessados)
        {
            dsCodigoFuncsProcessados = new List<string>();
            switch (pAcao)
            {
                case Modelo.Acao.Incluir:
                    return dalBilheteSimp.IncluirbilhetesEmLoteWebApi(lista, login, conectionStr, out dsCodigoFuncsProcessados);
                case Modelo.Acao.Alterar:
                    return dalBilheteSimp.Alterar(lista, login);
            }
            return 0;
        }

        public Dictionary<string, string> SalvarNaLista(Modelo.BilhetesImp objeto, List<Modelo.BilhetesImp> lista)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                if (objeto.Acao == Modelo.Acao.Incluir)
                {
                    lista.Add(objeto);
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].Ent_sai == objeto.Ent_sai && lista[i].Posicao == objeto.Posicao)
                        {
                            if (objeto.Id == 0 && objeto.Acao == Modelo.Acao.Excluir)
                            {
                                lista.RemoveAt(i);
                            }
                            else
                            {
                                lista[i] = objeto;
                                if (objeto.Id == 0)
                                {
                                    lista[i].Acao = Modelo.Acao.Incluir;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalBilheteSimp.getId(pValor, pCampo, pValor2);
        }

        public Int64 GetUltimoNSRRep(string pRelogio)
        {
            return dalBilheteSimp.GetUltimoNSRRep(pRelogio);
        }

        public List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros> FuncsDoisRegistrosRegistribuirBilhetes(bool importado, List<string> lPis, DateTime datai, DateTime dataf)
        {
            return dalBilheteSimp.FuncsDoisRegistrosRegistribuirBilhetes(importado, lPis, datai, dataf);
        }

        public List<Modelo.BilhetesImp> GetBilhetesFuncPis(List<string> lPIS, DateTime pDataI, DateTime pDataF)
        {
            return dalBilheteSimp.GetBilhetesFuncPis(lPIS, pDataI, pDataF);
        }

        public Modelo.BilhetesImp UltimoBilhetePorRep(string pRelogio)
        {
            return dalBilheteSimp.UltimoBilhetePorRep(pRelogio);
        }

        public Hashtable GetHashPorPISPeriodo(SqlTransaction trans, DateTime pDataI, DateTime pDataF, List<string> lPis)
        {
            return dalBilheteSimp.GetHashPorPISPeriodo(trans, pDataI, pDataF, lPis);
        }

        public void IncrementaProgressBarVazio(int incremento)
        {
        }

        public void SetaValorProgressBarVazio(int valor)
        {
        }

        public void SetaMinMaxProgressBarVazio(int min, int max)
        {
        }

        public void SetaMensagemVazio(string mensagem)
        {
        }
    }
}
