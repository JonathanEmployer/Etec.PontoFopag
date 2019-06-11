using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class HorarioAItinere : DAL.SQL.DALBase, DAL.IHorarioAItinere
    {
        public string SELECTAIHOR { get; set; }
        public string SELECTAIEHE { get; set; }

        public HorarioAItinere(DataBase database)
        {
            db = database;
            TABELA = "HorarioInItinere";

            SELECTPID = @"   SELECT * FROM HorarioInItinere WHERE id = @id";

            SELECTALL = @"   SELECT * FROM HorarioInItinere";

            SELECTAIHOR = @"   SELECT * FROM HorarioInItinere WHERE idhorario = @idhorario";

            SELECTAIEHE = @" SELECT    empresa.id AS idempresa
                                    , empresa.nome AS empresa
                                    , empresa.cnpj
                                    , empresa.endereco
                                    , empresa.cep
                                    , empresa.cidade
                                    , empresa.estado
                                    , departamento.id AS iddepartamento
                                    , departamento.descricao AS departamento
                                    , funcionario.id AS idfuncionario
                                    , funcionario.nome AS funcionario
                                    , CAST(funcionario.dscodigo AS BIGINT) AS dscodigo     
                                    , funcionario.idfuncao                               
                            FROM funcionario
                            INNER JOIN empresa ON empresa.id = funcionario.idempresa
                            INNER JOIN departamento ON departamento.id = funcionario.iddepartamento";

            INSERT = @"  INSERT INTO HorarioInItinere
							(codigo, idhorario, Dia, MarcaDiaBool, PercentualDentroJornada, PercentualDentroFora, incdata, inchora, incusuario, altdata, althora, altusuario)
							VALUES
							(@codigo, @idhorario, @Dia, @MarcaDiaBool, @PercentualDentroJornada, @PercentualDentroFora, @incdata, @inchora, @incusuario, @altdata, @althora, @altusuario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE HorarioInItinere SET codigo = @codigo
                            , idhorario = @idhorario
							, Dia = @Dia
							, MarcaDiaBool = @MarcaDiaBool
							, PercentualDentroJornada = @PercentualDentroJornada
							, PercentualDentroFora = @PercentualDentroFora
							, incdata = @incdata
							, inchora = @inchora
							, incusuario = @incusuario
                            , altdata = @altdata
                            , althora = @althora
                            , altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM HorarioInItinere WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM HorarioInItinere";

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
                obj = new Modelo.HorarioPHExtra();
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
            ((Modelo.HorarioAItinere)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioAItinere)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.HorarioAItinere)obj).Dia = Convert.ToInt16(dr["Dia"]);
            ((Modelo.HorarioAItinere)obj).MarcaDiaBool = Convert.ToBoolean(dr["MarcaDiaBool"]);
            ((Modelo.HorarioAItinere)obj).PercentualDentroJornada = Convert.ToDecimal(dr["PercentualDentroJornada"]);
            ((Modelo.HorarioAItinere)obj).PercentualDentroFora = Convert.ToDecimal(dr["PercentualDentroFora"]);

        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@idhorario", SqlDbType.Int),
				new SqlParameter ("@Dia", SqlDbType.Int),
				new SqlParameter ("@MarcaDiaBool", SqlDbType.Bit),
				new SqlParameter ("@PercentualDentroJornada", SqlDbType.Decimal),
				new SqlParameter ("@PercentualDentroFora", SqlDbType.Decimal),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.HorarioAItinere)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioAItinere)obj).Idhorario;
            parms[3].Value = ((Modelo.HorarioAItinere)obj).Dia;
            parms[4].Value = ((Modelo.HorarioAItinere)obj).MarcaDiaBool;
            parms[5].Value = ((Modelo.HorarioAItinere)obj).PercentualDentroJornada;
            parms[6].Value = ((Modelo.HorarioAItinere)obj).PercentualDentroFora;
            parms[7].Value = ((Modelo.HorarioAItinere)obj).Incdata;
            parms[8].Value = ((Modelo.HorarioAItinere)obj).Inchora;
            parms[9].Value = ((Modelo.HorarioAItinere)obj).Incusuario;
            parms[10].Value = ((Modelo.HorarioAItinere)obj).Altdata;
            parms[11].Value = ((Modelo.HorarioAItinere)obj).Althora;
            parms[12].Value = ((Modelo.HorarioAItinere)obj).Altusuario;
        }

        public Modelo.HorarioAItinere LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioAItinere objHorarioAItinere = new Modelo.HorarioAItinere();
            try
            {

                SetInstance(dr, objHorarioAItinere);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorarioAItinere;
        }

        protected SqlDataReader LoadDataReaderPorHorario(int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int, 4) };
            parms[0].Value = idHorario;

            return db.ExecuteReader(CommandType.Text, SELECTAIHOR, parms);
        }

        protected SqlDataReader LoadDataReaderPorHorario(SqlTransaction trans, int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int, 4) };
            parms[0].Value = idHorario;

            return TransactDbOps.ExecuteReader(trans, CommandType.Text, SELECTAIHOR, parms);
        }

        public List<Modelo.HorarioAItinere> LoadPorHorario(int idHorario)
        {
            SqlDataReader dr = LoadDataReaderPorHorario(idHorario);

            List<Modelo.HorarioAItinere> lista = new List<Modelo.HorarioAItinere>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioAItinere objHorarioAItinere = new Modelo.HorarioAItinere();
                    AuxSetInstance(dr, objHorarioAItinere);
                    lista.Add(objHorarioAItinere);
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }


        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }



        public List<Modelo.HorarioAItinere> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM HorarioInItinere", parms);
            List<Modelo.HorarioAItinere> lista = new List<Modelo.HorarioAItinere>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioAItinere objHorarioAItinere = new Modelo.HorarioAItinere();
                    AuxSetInstance(dr, objHorarioAItinere);
                    lista.Add(objHorarioAItinere);
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