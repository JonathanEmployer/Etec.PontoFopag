using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class FechamentoBHDPercentual : IBLL<Modelo.FechamentoBHDPercentual>
    {
        DAL.IFechamentoBHDPercentual dalFechamentoBHPercentual;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public FechamentoBHDPercentual() : this(null)
        {
            
        }

        public FechamentoBHDPercentual(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public FechamentoBHDPercentual(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalFechamentoBHPercentual = new DAL.SQL.FechamentoBHDPercentual(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalFechamentoBHPercentual.UsuarioLogado = usuarioLogado;
        }


        public DataTable GetAll()
        {
            return dalFechamentoBHPercentual.GetAll();
        }

        public List<int> GetIds()
        {
            return dalFechamentoBHPercentual.GetIds();
        }

        /// <summary>
        /// Retorna uma tabela hash onde o código é a chave e o id é o valor
        /// </summary>
        /// <returns>Tabela Hash(Código, Id)</returns>
        public Hashtable GetHashCodigoId()
        {
            return dalFechamentoBHPercentual.GetHashCodigoId();
        }

        public Modelo.FechamentoBHDPercentual LoadObject(int id)
        {
            return dalFechamentoBHPercentual.LoadObject(id);
        }

        //public void PreencheObjetoInclusao()
        //{
        //                pObjFechamentoBHPercentual.Incusuario
        //    comando.Append(" , altdata = '" + dt.Month + dt.Day + dt.Year + "'");
        //    comando.Append(" , althora = '" + dt.Month + dt.Day + dt.Year + " " + dt.ToLongTimeString() + "'");
        //    comando.Append(" , altusuario = '" + Modelo.cwkGlobal.objUsuarioLogado.Login + "'");
        //}

        //public void PreencheObjetoAlteracao()
        //{
        //                pObjFechamentoBHPercentual.Incusuario
        //    comando.Append(" , altdata = '" + dt.Month + dt.Day + dt.Year + "'");
        //    comando.Append(" , althora = '" + dt.Month + dt.Day + dt.Year + " " + dt.ToLongTimeString() + "'");
        //    comando.Append(" , altusuario = '" + Modelo.cwkGlobal.objUsuarioLogado.Login + "'");
        //}

        public IList<Modelo.FechamentoBHDPercentual> GetFechamentoBHPercentualPorIdFechamentoBHD(int idFechamentoBHD)
        {
            return dalFechamentoBHPercentual.GetFechamentoBHPercentualPorIdFechamentoBHD(idFechamentoBHD);
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentoBHDPercentual objeto)
        {
            try
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        PreencheObjetos(ref objeto, DateTime.Now.Date, DateTime.Now, dalFechamentoBHPercentual.UsuarioLogado.Login,
                            pAcao);
                        dalFechamentoBHPercentual.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        PreencheObjetos(ref objeto, DateTime.Now.Date, DateTime.Now, dalFechamentoBHPercentual.UsuarioLogado.Login,
                            pAcao);
                        dalFechamentoBHPercentual.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFechamentoBHPercentual.Excluir(objeto);
                        break;
                }
            }
            catch (Exception e)
            {
                
                throw e;
            }
            return new Dictionary<string, string>();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentoBHDPercentual objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalFechamentoBHPercentual.getId(pValor, pCampo, pValor2);
        }

        private Dictionary<string, string> PreencheObjetos(ref Modelo.FechamentoBHDPercentual pObjFechamentoBHPercentual, 
            DateTime pData, DateTime pHora, string pUsuario, Modelo.Acao acao)
        {
            if (acao == Modelo.Acao.Incluir)
            {
                pObjFechamentoBHPercentual.Incdata = pData;
                pObjFechamentoBHPercentual.Incusuario = pUsuario;
                pObjFechamentoBHPercentual.Inchora = pHora;
            }
            else if (acao == Modelo.Acao.Alterar)
            {
                pObjFechamentoBHPercentual.Altdata = pData;
                pObjFechamentoBHPercentual.Altusuario = pUsuario;
                pObjFechamentoBHPercentual.Althora = pHora;
            }
            return new Dictionary<string, string>(); 
        }

        public List<Modelo.FechamentoBHDPercentual> GetAllList()
        {
            return dalFechamentoBHPercentual.GetAllList();
        }

        public DataTable GetBancoHorasPercentual(DateTime? pdataInicial, DateTime pdataFinal, int pidFuncionario, int pconsiderarUltimoFechamento)
        {
            return dalFechamentoBHPercentual.GetBancoHorasPercentual(pdataInicial, pdataFinal, pidFuncionario, pconsiderarUltimoFechamento);
        }
    }
}
