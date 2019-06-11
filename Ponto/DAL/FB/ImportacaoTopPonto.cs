﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DAL.FB
{
    public class ImportacaoTopPonto : IImportacaoTopPonto
    {
        private ImportacaoTopPonto() { }

        #region Singleton

        private static volatile ImportacaoTopPonto _instancia = null;

        public static FB.ImportacaoTopPonto GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.ImportacaoTopPonto))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.ImportacaoTopPonto();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        public Hashtable GetHashEmpresa()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"codigo\" FROM \"empresa\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashDepartamento()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"codigo\" FROM \"departamento\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashFuncao()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"codigo\" FROM \"funcao\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (!lista.ContainsKey(Convert.ToInt32(dr["codigo"])))
                            lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashFuncaoDescricao()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"descricao\" FROM \"funcao\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (!lista.ContainsKey(dr["descricao"].ToString()))
                            lista.Add(dr["descricao"].ToString(), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashHorario()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"codigo\" FROM \"horario\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashFuncionario()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"dscodigo\" FROM \"funcionario\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToString(dr["dscodigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashFuncCodigoDscodigo()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"codigo\", \"dscodigo\" FROM \"funcionario\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToString(dr["dscodigo"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashOcorrenciaDescId()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"descricao\" FROM \"ocorrencia\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToString(dr["descricao"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashOcorrenciaCodigoId()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"id\", \"codigo\" FROM \"ocorrencia\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }

        public Hashtable GetHashFuncCodigoId()
        {
            Hashtable lista = new Hashtable();

            FbParameter[] parms = new FbParameter[] { };
            string cmd = "SELECT \"codigo\", \"id\" FROM \"funcionario\"";
            using (FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
            }

            return lista;
        }
    }
}
