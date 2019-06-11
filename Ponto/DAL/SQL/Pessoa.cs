using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class Pessoa : DAL.SQL.DALBase, DAL.IPessoa
    {

        public Pessoa(DataBase database)
        {
            db = database;
            TABELA = "Pessoa";

            SELECTPID = @"   SELECT * FROM Pessoa WHERE id = @id";

            SELECTALL = @"   SELECT   Pessoa.*
                             FROM Pessoa";

            INSERT = @"  INSERT INTO Pessoa
							(codigo, incdata, inchora, incusuario, TipoPessoa, RazaoSocial, Fantasia , CNPJ_CPF , Insc_RG , Email, IdIntegracao)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @TipoPessoa, @RazaoSocial, @Fantasia , @CNPJ_CPF , @Insc_RG , @Email, @IdIntegracao) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE Pessoa SET
							  codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , TipoPessoa = @TipoPessoa
                            , RazaoSocial = @RazaoSocial
                            , Fantasia = @Fantasia
                            , CNPJ_CPF = @CNPJ_CPF 
                            , Insc_RG = @Insc_RG 
                            , Email = @Email
                            , IdIntegracao = @IdIntegracao
						WHERE id = @id";

            DELETE = @"  DELETE FROM Pessoa WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM Pessoa";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
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
                obj = new Modelo.Pessoa();
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Pessoa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Pessoa)obj).TipoPessoa = Convert.ToInt16(dr["TipoPessoa"]);
            ((Modelo.Pessoa)obj).RazaoSocial = Convert.ToString(dr["RazaoSocial"]);
            ((Modelo.Pessoa)obj).Fantasia = Convert.ToString(dr["Fantasia"]);
            ((Modelo.Pessoa)obj).CNPJ_CPF = Convert.ToString(dr["CNPJ_CPF"]);
            ((Modelo.Pessoa)obj).Insc_RG = Convert.ToString(dr["Insc_RG"]);
            ((Modelo.Pessoa)obj).Email = Convert.ToString(dr["Email"]);
            object val = dr["IdIntegracao"];
            Int32? idint = (val == null || val is DBNull) ? (Int32?)null : (Int32?)val;
            ((Modelo.Pessoa)obj).IdIntegracao = idint;
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@TipoPessoa", SqlDbType.Int),
                new SqlParameter ("@RazaoSocial", SqlDbType.VarChar),
                new SqlParameter ("@Fantasia", SqlDbType.VarChar),
                new SqlParameter ("@CNPJ_CPF", SqlDbType.VarChar),
                new SqlParameter ("@Insc_RG", SqlDbType.VarChar),
                new SqlParameter ("@Email", SqlDbType.VarChar),
                new SqlParameter ("@IdIntegracao", SqlDbType.Int)
			};
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.Pessoa)obj).Codigo;
            parms[2].Value = ((Modelo.Pessoa)obj).Incdata;
            parms[3].Value = ((Modelo.Pessoa)obj).Inchora;
            parms[4].Value = ((Modelo.Pessoa)obj).Incusuario;
            parms[5].Value = ((Modelo.Pessoa)obj).Altdata;
            parms[6].Value = ((Modelo.Pessoa)obj).Althora;
            parms[7].Value = ((Modelo.Pessoa)obj).Altusuario;
            parms[8].Value = ((Modelo.Pessoa)obj).TipoPessoa;
            parms[9].Value = ((Modelo.Pessoa)obj).RazaoSocial;
            parms[10].Value = ((Modelo.Pessoa)obj).Fantasia;
            parms[11].Value = ((Modelo.Pessoa)obj).CNPJ_CPF;
            parms[12].Value = ((Modelo.Pessoa)obj).Insc_RG;
            parms[13].Value = ((Modelo.Pessoa)obj).Email;
            parms[14].Value = ((Modelo.Pessoa)obj).IdIntegracao;
        }

        public Modelo.Pessoa LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Pessoa objPessoa = new Modelo.Pessoa();
            try
            {

                SetInstance(dr, objPessoa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objPessoa;
        }

        public List<Modelo.Pessoa> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM Pessoa", parms);

            List<Modelo.Pessoa> lista = new List<Modelo.Pessoa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Pessoa objPessoa = new Modelo.Pessoa();
                    AuxSetInstance(dr, objPessoa);
                    lista.Add(objPessoa);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Pessoa> GetAllListPorIds(List<int> ids)
        {
            List<Modelo.Pessoa> result = new List<Modelo.Pessoa>();

            try
            {
                var parameters = new string[ids.Count];
                List<SqlParameter> parmList = new List<SqlParameter>();
                for (int i = 0; i < ids.Count; i++)
                {
                    parameters[i] = string.Format("@Id{0}", i);
                    parmList.Add(new SqlParameter(parameters[i], ids[i]));
                }

                string sql = string.Format("SELECT * from Pessoa WHERE Id IN ({0})", string.Join(", ", parameters));

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parmList.ToArray());

                try
                {
                    while (dr.Read())
                    {
                        Modelo.Pessoa objPessoa = new Modelo.Pessoa();
                        AuxSetInstance(dr, objPessoa);
                        result.Add(objPessoa);
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (!dr.IsClosed)
                    {
                        dr.Close();
                    }
                    dr.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /// <summary>
        /// Retorna uma lista com as pessoas que tenham o código igual ao informado
        /// </summary>
        /// <param name="codigo">Código da pessoa</param>
        /// <returns>List de pessoas</returns>
        public List<Modelo.Pessoa> GetPessoaPorCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string sql = @"select * from pessoa where codigo = @codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Pessoa> lista = new List<Modelo.Pessoa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Pessoa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Pessoa>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Retorna a lista de pessoas que contenham o nome passado por parâmetro
        /// </summary>
        /// <param name="nome">Nome a ser pesquisado</param>
        /// <returns>Lista de Pessoas</returns>
        public List<Modelo.Pessoa> GetListPessoaPorNome(string nome)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@nome", SqlDbType.VarChar)
            };
            parms[0].Value = nome;

            string sql = @"select * from pessoa where razaosocial like '%'+@nome+'%' or @nome is null";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Pessoa> lista = new List<Modelo.Pessoa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Pessoa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Pessoa>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public int? GetIdPorIdIntegracao(int idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "select top 1 id from pessoa where idIntegracao = " + idIntegracao;
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));

            return Id;
        }

        /// <summary>
        /// Retorna uma lista com as pessoas que tenham o CNPJ_CPF igual ao informado
        /// </summary>
        /// <param name="CNPJ_CPF">CNPJ_CPF da pessoa</param>
        /// <returns>List de pessoas</returns>
        public List<Modelo.Pessoa> GetPessoaPorCNPJ_CPF(string CNPJ_CPF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@CNPJ_CPF", SqlDbType.VarChar)
            };
            parms[0].Value = CNPJ_CPF;

            string sql = @"select * from pessoa where CNPJ_CPF = @CNPJ_CPF";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Pessoa> lista = new List<Modelo.Pessoa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Pessoa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Pessoa>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }
        #endregion
    }
}
