using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using AutoMapper;

namespace DAL.SQL
{
    public class MudCodigoFunc : DAL.SQL.DALBase, DAL.IMudCodigoFunc
    {

        public MudCodigoFunc(DataBase database)
        {
            db = database;
            TABELA = "mudcodigofunc";

            SELECTPID = @"   SELECT m.*, f.dscodigo + ' | ' + f.nome AS NomeFuncionario 
                             FROM mudcodigofunc m
                             INNER JOIN funcionario f ON f.id = m.idfuncionario
                             WHERE m.id = @id";

            SELECTALL = @"   SELECT m.id
		                            , m.codigo
		                            , m.datainicial as data
		                            , f.nome AS NomeFuncionario  
		                            , m.dscodigoantigo
		                            , m.dscodigonovo
                            FROM mudcodigofunc m
                            INNER JOIN funcionario f ON f.id = m.idfuncionario";

            INSERT = @"  INSERT INTO mudcodigofunc
							(codigo, datainicial, idfuncionario, dscodigoantigo, dscodigonovo, tipohorario, idhorarionormal, iddepartamento, idempresa, incdata, inchora, incusuario)
							VALUES
							(@codigo, @datainicial, @idfuncionario, @dscodigoantigo, @dscodigonovo, @tipohorario, @idhorarionormal, @iddepartamento, @idempresa, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE mudcodigofunc SET codigo = @codigo
							, datainicial = @datainicial
                            , idfuncionario = @idfuncionario
							, dscodigoantigo = @dscodigoantigo
							, dscodigonovo = @dscodigonovo
							, tipohorario = @tipohorario
							, idhorarionormal = @idhorarionormal
							, iddepartamento = @iddepartamento
							, idempresa = @idempresa
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM mudcodigofunc WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM mudcodigofunc";

            SELECTALLLIST = @" SELECT m.*, f.dscodigo + ' | ' + f.nome AS NomeFuncionario 
                               FROM mudcodigofunc m
                               INNER JOIN funcionario f ON f.id = m.idfuncionario";
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
                obj = new Modelo.MudCodigoFunc();
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
            ((Modelo.MudCodigoFunc)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.MudCodigoFunc)obj).Datainicial = Convert.ToDateTime(dr["datainicial"]);
            ((Modelo.MudCodigoFunc)obj).IdFuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.MudCodigoFunc)obj).DSCodigoAntigo = Convert.ToString(dr["dscodigoantigo"]);
            ((Modelo.MudCodigoFunc)obj).DSCodigoNovo = Convert.ToString(dr["dscodigonovo"]);
            ((Modelo.MudCodigoFunc)obj).Tipohorario = Convert.ToInt16(dr["tipohorario"]);
            ((Modelo.MudCodigoFunc)obj).Idhorarionormal = Convert.ToInt32(dr["idhorarionormal"]);
            ((Modelo.MudCodigoFunc)obj).Iddepartamento = Convert.ToInt32(dr["iddepartamento"]);
            ((Modelo.MudCodigoFunc)obj).Idempresa = Convert.ToInt32(dr["idempresa"]);
            //renato - arrumar aqui
            ((Modelo.MudCodigoFunc)obj).NomeFuncionario = Convert.ToString(dr["NomeFuncionario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@datainicial", SqlDbType.DateTime),
                new SqlParameter ("@idfuncionario", SqlDbType.Int),
				new SqlParameter ("@dscodigoantigo", SqlDbType.VarChar),
				new SqlParameter ("@dscodigonovo", SqlDbType.VarChar),
				new SqlParameter ("@tipohorario", SqlDbType.TinyInt),
				new SqlParameter ("@idhorarionormal", SqlDbType.Int),
				new SqlParameter ("@iddepartamento", SqlDbType.Int),
				new SqlParameter ("@idempresa", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.MudCodigoFunc)obj).Codigo;
            parms[2].Value = ((Modelo.MudCodigoFunc)obj).Datainicial;
            parms[3].Value = ((Modelo.MudCodigoFunc)obj).IdFuncionario;
            parms[4].Value = ((Modelo.MudCodigoFunc)obj).DSCodigoAntigo;
            parms[5].Value = ((Modelo.MudCodigoFunc)obj).DSCodigoNovo;
            parms[6].Value = ((Modelo.MudCodigoFunc)obj).Tipohorario;
            parms[7].Value = ((Modelo.MudCodigoFunc)obj).Idhorarionormal;
            parms[8].Value = ((Modelo.MudCodigoFunc)obj).Iddepartamento;
            parms[9].Value = ((Modelo.MudCodigoFunc)obj).Idempresa;
            parms[10].Value = ((Modelo.MudCodigoFunc)obj).Incdata;
            parms[11].Value = ((Modelo.MudCodigoFunc)obj).Inchora;
            parms[12].Value = ((Modelo.MudCodigoFunc)obj).Incusuario;
            parms[13].Value = ((Modelo.MudCodigoFunc)obj).Altdata;
            parms[14].Value = ((Modelo.MudCodigoFunc)obj).Althora;
            parms[15].Value = ((Modelo.MudCodigoFunc)obj).Altusuario;
        }

        public Modelo.MudCodigoFunc LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.MudCodigoFunc objMudCodigoFunc = new Modelo.MudCodigoFunc();
            try
            {

                SetInstance(dr, objMudCodigoFunc);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMudCodigoFunc;
        }


        public bool VerificaMarcacao(int pId, DateTime pData)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[]{
                    new SqlParameter("@id", SqlDbType.Int, 4),
                    };

            parms[0].Value = pId;

            aux = @"SELECT MAX (bilhetesimp.data) AS Data FROM bilhetesimp " +
                " WHERE bilhetesimp.func IN (Select funcionario.dscodigo FROM funcionario WHERE funcionario.id = @id)" +
                " AND bilhetesimp.importado = '1'";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            bool ret = false;
            if (dr.HasRows)
            {
                dr.Read();
                if (dr["Data"] is System.DBNull || pData > Convert.ToDateTime(dr["Data"]))
                    ret = true;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public List<Modelo.MudCodigoFunc> GetMudancasPeriodo(DateTime datai, DateTime dataf)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };
                parms[0].Value = datai;
                parms[1].Value = dataf;

                List<Modelo.MudCodigoFunc> ret = new List<Modelo.MudCodigoFunc>();
                string select = SELECTALLLIST + " WHERE datainicial >= @datai AND datainicial <= @dataf ";
                string cmd = select + PermissaoUsuarioFuncionario(UsuarioLogado, select, "f.idempresa", "f.id", null);

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
                Modelo.MudCodigoFunc objMudCodigoFunc;
                while (dr.Read())
                {
                    objMudCodigoFunc = new Modelo.MudCodigoFunc();
                    AuxSetInstance(dr, objMudCodigoFunc);
                    ret.Add(objMudCodigoFunc);
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();

                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("Houve um problema ao retornar as mudanças de código dos funcionários no período " + datai.ToShortDateString() + "à " + dataf.ToShortDateString(), e);
            }
        }

        public List<Modelo.MudCodigoFunc> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = SELECTALLLIST + PermissaoUsuarioFuncionario(UsuarioLogado, SELECTALLLIST, "f.idempresa", "f.id", null);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.MudCodigoFunc> lista = new List<Modelo.MudCodigoFunc>();
            try
            {
                while (dr.Read())
                {
                    Modelo.MudCodigoFunc objMudCodigoFunc = new Modelo.MudCodigoFunc();
                    AuxSetInstance(dr, objMudCodigoFunc);
                    lista.Add(objMudCodigoFunc);
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

        #endregion
    }
}
