using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class OcorrenciaEmpresa : DAL.FB.DALBase, DAL.IOcorrenciaEmpresa
    {
        #region Singleton

        private static volatile FB.OcorrenciaEmpresa _instancia = null;

        public static FB.OcorrenciaEmpresa GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.OcorrenciaEmpresa))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.OcorrenciaEmpresa();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        public Modelo.OcorrenciaEmpresa LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.OcorrenciaEmpresa> GetAllList()
        {
            throw new NotImplementedException();
        }

        public void DeleteAllByIdEmpresa(int idEmpresa)
        {
            throw new NotImplementedException();
        }

        public void IncluirOcorrenciasEmpresa(List<Modelo.OcorrenciaEmpresa> ocorrenciasEmpresa)
        {
            throw new NotImplementedException();
        }

        public Modelo.OcorrenciaEmpresa LoadObjectByCodigo(int pCodigo)
        {
            throw new NotImplementedException();
        }

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.OcorrenciaEmpresa> GetAllPorExibePainelRHPorEmpresa(int idEmpresa)
        {
            throw new NotImplementedException();
        }

        protected override FbParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
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
    }
}
