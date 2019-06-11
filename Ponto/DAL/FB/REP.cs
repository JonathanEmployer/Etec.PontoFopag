using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo.Proxy;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class REP : DAL.FB.DALBase, DAL.IREP
    {
        public string SELECTPNR { get; set; }

        private REP()
        {
            GEN = "GEN_rep_id";

            TABELA = "rep";

            SELECTPID = "   SELECT * FROM \"rep\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT \"id\", \"numserie\", \"local\", \"codigo\", \"numrelogio\" FROM \"rep\"";

            SELECTPNR = "   SELECT * FROM \"rep\" WHERE \"rep\".\"numrelogio\" = @numrelogio";

            INSERT = "  INSERT INTO \"rep\" "
                        + " (\"codigo\", \"numserie\", \"local\", \"incdata\", \"inchora\", \"incusuario\", \"numrelogio\") "
                        + " VALUES "
                        + " (@codigo, @numserie, @local, @incdata, @inchora, @incusuario, @numrelogio) ";						

            UPDATE = " UPDATE \"rep\" SET "
							+"  \"codigo\" = @codigo "
							+", \"numserie\" = @numserie "
                            +", \"local\" = @local "
						    +", \"altdata\" = @altdata "
						    +", \"althora\" = @althora "
						    +", \"altusuario\" = @altusuario "
                            + ", \"numrelogio\" = @numrelogio "
                        + " WHERE \"id\" = @id ";

            DELETE = "  DELETE FROM \"rep\" WHERE \"id\" = @id";

            MAXCOD = " SELECT MAX(\"codigo\") AS \"codigo\" FROM \"rep\"";

        }

        #region Singleton

        private static volatile FB.REP _instancia = null;

        public static FB.REP GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.REP))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.REP();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AuxSetInstance(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.REP();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.REP)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.REP)obj).NumSerie = Convert.ToString(dr["numserie"]);
            ((Modelo.REP)obj).Local = Convert.ToString(dr["local"]);
            ((Modelo.REP)obj).NumRelogio = Convert.ToString(dr["numrelogio"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@numserie", FbDbType.VarChar),
                new FbParameter ("@local", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.TimeStamp),
				new FbParameter ("@inchora", FbDbType.TimeStamp),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.TimeStamp),
				new FbParameter ("@althora", FbDbType.TimeStamp),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@numrelogio", FbDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.REP)obj).Codigo;
            parms[2].Value = ((Modelo.REP)obj).NumSerie;
            parms[3].Value = ((Modelo.REP)obj).Local;
            parms[4].Value = ((Modelo.REP)obj).Incdata;
            parms[5].Value = ((Modelo.REP)obj).Inchora;
            parms[6].Value = ((Modelo.REP)obj).Incusuario;
            parms[7].Value = ((Modelo.REP)obj).Altdata;
            parms[8].Value = ((Modelo.REP)obj).Althora;
            parms[9].Value = ((Modelo.REP)obj).Altusuario;
            parms[10].Value = ((Modelo.REP)obj).NumRelogio;
        }

        public Modelo.REP LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.REP objREP = new Modelo.REP();
            try
            {

                SetInstance(dr, objREP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objREP;
        }

        private FbDataReader LoadDataReaderPorNumRelogio(string numRelogio)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@numrelogio", FbDbType.VarChar) };
            parms[0].Value = numRelogio;

            return FB.DataBase.ExecuteReader(CommandType.Text, SELECTPNR, parms);
        }

        public Modelo.REP LoadObjectPorNumRelogio(string numRelogio)
        {
            FbDataReader dr = LoadDataReaderPorNumRelogio(numRelogio);

            Modelo.REP objREP = new Modelo.REP();
            try
            {
                SetInstance(dr, objREP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objREP;
        }

        public string GetNumInner(string pNumSerie)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@numeroserie", FbDbType.VarChar, 20)
               
            };
            parms[0].Value = pNumSerie;


            string aux =  "SELECT \"numrelogio\" FROM \"rep\" WHERE \"numserie\" = @numeroserie";

            return Convert.ToString(FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms));

        }

        public bool GetCPFCNPJ(string pCPFCNPJ, string pTipo)
        {
            if (pCPFCNPJ.Trim().Length != 14)
                    return false;

            int aux1;
            string aux;
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@cpfcnpj", FbDbType.VarChar, 20)
               
            };
            if (pTipo == "1")
            {
                parms[0].Value = (pCPFCNPJ.Substring(0, 2) + "." + pCPFCNPJ.Substring(2, 3) + "." + pCPFCNPJ.Substring(5, 3) + "/" + pCPFCNPJ.Substring(8, 4) + "-" + pCPFCNPJ.Substring(12, 2));
                aux = "SELECT COUNT(*) FROM \"empresa\" WHERE \"cnpj\" = @cpfcnpj";
            }
            else
            {
                parms[0].Value = (pCPFCNPJ.Substring(3, 3) + "." + pCPFCNPJ.Substring(6, 3) + "." + pCPFCNPJ.Substring(9, 3) + "-" + pCPFCNPJ.Substring(12, 2));
                aux = "SELECT COUNT(*) FROM \"empresa\" WHERE \"cpf\" = @cpfcnpj";
            }

            aux1 = (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            if (aux1 >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Modelo.REP> GetAllList()
        {
            List<Modelo.REP> lista = new List<Modelo.REP>();

            FbParameter[] parms = new FbParameter[] { };

            string aux = "SELECT * FROM \"rep\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.REP objREP = new Modelo.REP();
                    AuxSetInstance(dr, objREP);
                    lista.Add(objREP);
                }
            }
            dr.Close();

            return lista;
        }

        #endregion


        public List<Modelo.Proxy.pxyRep> PegaPxysRep()
        {
            throw new NotImplementedException();
        }

        public Modelo.REP LoadObjectByCodigo(int codigo)
        {
            throw new NotImplementedException();
        }


        public void SetUltimoNSR(int idrep, int ultimoNsr)
        {
            throw new NotImplementedException();
        }


        public void SetUltimoNSRComDataIntegracao(int idrep, int ultimoNsr)
        {
            throw new NotImplementedException();
        }


        public Modelo.REP LoadObjectByNumSerie(string NumSerie)
        {
            throw new NotImplementedException();
        }

        public void SetUltimaImportacao(string numeroRelogio, long ultimoNsr, DateTime dataUltimoBilhete)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.REP> VerificarIpEntreRep(string ip, int id)
        {
            throw new NotImplementedException();
        }

        public List<RepSituacao> VerificarSituacaoReps(int TempoSemComunicacao)
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
    }
}
