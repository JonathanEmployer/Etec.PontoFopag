using System;
using System.Data;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo;
using Modelo.Proxy;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class FechamentobhdHE : DAL.FB.DALBase, DAL.IFechamentobhdHE
    {
        private string SELECTFECHAMENTO = "";

        private FechamentobhdHE()
        {

        }

        #region Singleton

        private static volatile FB.FechamentobhdHE _instancia = null;

        public static FB.FechamentobhdHE GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.FechamentobhdHE))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.FechamentobhdHE();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        protected override FbParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters(FbParameter[] parms, ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        public Modelo.FechamentobhdHE LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public void Incluir(List<Modelo.FechamentobhdHE> lista)
        {
            throw new NotImplementedException();
        }

        public void SalvaLista(List<string> pLstStrFechamentoBHDHE)
        {
            throw new NotImplementedException();
        }

        public string MontaStringInsert(Modelo.FechamentobhdHE pObjFechamentoBHDHE)
        {
            throw new NotImplementedException();
        }

        public string MontaStringUpdate(Modelo.FechamentobhdHE pObjFechamentoBHDHE)
        {
            throw new NotImplementedException();
        }

        public List<pxyPessoaMarcacaoParaRateio> GetPessoaMarcacaoParaRateio(int pTipo, string pIdTipo, DateTime dataInicial, DateTime dataFinal)
        {
            throw new NotImplementedException();
        }

        public IList<Modelo.FechamentobhdHE> GetFechamentobhdHEPorIdFechamentoBH(int idFechamentoBH, int identificacao)
        {
            throw new NotImplementedException();
        }

        public IList<Modelo.FechamentobhdHE> GetAllList()
        {
            throw new NotImplementedException();
        }

        public DataTable GetRelatorioFechamentoPercentualHESintetico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            throw new NotImplementedException();
        }

        public DataTable GetRelatorioFechamentoPercentualHEAnalitico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans, SqlConnection con)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }

        #endregion






    }
}
