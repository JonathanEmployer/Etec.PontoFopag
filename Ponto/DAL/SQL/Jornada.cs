
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace DAL.SQL
{
    public class Jornada : DAL.SQL.DALBase, DAL.IJornada
    {

        public Jornada(DataBase database)
        {
            db = database;
            TABELA = "jornada";

            SELECTPID = "SELECT * FROM jornada WHERE id = @id";

            SELECTALL = "SELECT id " +
                              ", (jornada.entrada_1 " +
                              "+ ' - ' + jornada.saida_1 " +
                              "+ case when entrada_2 <> '--:--' then ' - ' + entrada_2 else '' end " +
                              "+ case when saida_2 <> '--:--' then ' - ' + saida_2 else '' end " +
                              "+ case when entrada_3 <> '--:--' then ' - ' + entrada_3 else '' end " +
                              "+ case when saida_3 <> '--:--' then ' - ' + saida_3 else '' end " +
                              "+ case when entrada_4 <> '--:--' then ' - ' + entrada_4 else '' end " +
                              "+ case when saida_4 <> '--:--' then ' - ' + saida_4 else '' end " +
                              ") AS horarios" +
                              ", jornada.codigo " +
                              "FROM jornada";

            INSERT = @"  INSERT INTO jornada
                        (codigo, descricao, entrada_1, entrada_2, entrada_3, entrada_4, saida_1, saida_2, saida_3, saida_4, incdata, inchora, incusuario)
                        VALUES
                        (@codigo, (@entrada_1 + ' - ' + @saida_1 
                                   + case when @entrada_2 <> '--:--' then ' - ' + @entrada_2 else '' end 
                                   + case when @saida_2 <> '--:--' then ' - ' + @saida_2 else '' end 
                                   + case when @entrada_3 <> '--:--' then ' - ' + @entrada_3 else '' end 
                                   + case when @saida_3 <> '--:--' then ' - ' + @saida_3 else '' end 
                                   + case when @entrada_4 <> '--:--' then ' - ' + @entrada_4 else '' end 
                                   + case when @saida_4 <> '--:--' then ' - ' + @saida_4 else '' end ), @entrada_1, @entrada_2, @entrada_3, @entrada_4, @saida_1, @saida_2, @saida_3, @saida_4, @incdata, @inchora, @incusuario) 
                        SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE jornada SET codigo = @codigo 
                                        , descricao = (@entrada_1 + ' - ' + @saida_1 
                                                        + case when @entrada_2<> '--:--' then ' - ' + @entrada_2 else '' end
                                                        + case when @saida_2<> '--:--' then ' - ' + @saida_2 else '' end
                                                        + case when @entrada_3<> '--:--' then ' - ' + @entrada_3 else '' end
                                                        + case when @saida_3<> '--:--' then ' - ' + @saida_3 else '' end
                                                        + case when @entrada_4<> '--:--' then ' - ' + @entrada_4 else '' end
                                                        + case when @saida_4<> '--:--' then ' - ' + @saida_4 else '' end ) 
                                        , entrada_1 = @entrada_1 
                                        , entrada_2 = @entrada_2 
                                        , entrada_3 = @entrada_3 
                                        , entrada_4 = @entrada_4 
                                        , saida_1 = @saida_1 
                                        , saida_2 = @saida_2 
                                        , saida_3 = @saida_3 
                                        , saida_4 = @saida_4 
                                        , altdata = @altdata 
                                        , althora = @althora 
                                        , altusuario = @altusuario 
                                    WHERE id = @id";
            
            DELETE = "DELETE FROM jornada WHERE id = @id";

            MAXCOD = "SELECT MAX(codigo) AS codigo FROM jornada";
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
                obj = new Modelo.Jornada();
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
            ((Modelo.Jornada)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Jornada)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Jornada)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.Jornada)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.Jornada)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.Jornada)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.Jornada)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.Jornada)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.Jornada)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.Jornada)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
                new SqlParameter ("@entrada_1", SqlDbType.VarChar),
                new SqlParameter ("@entrada_2", SqlDbType.VarChar),
                new SqlParameter ("@entrada_3", SqlDbType.VarChar),
                new SqlParameter ("@entrada_4", SqlDbType.VarChar),
                new SqlParameter ("@saida_1", SqlDbType.VarChar),
                new SqlParameter ("@saida_2", SqlDbType.VarChar),
                new SqlParameter ("@saida_3", SqlDbType.VarChar),
                new SqlParameter ("@saida_4", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.Date),
				new SqlParameter ("@inchora", SqlDbType.Date),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.Date),
				new SqlParameter ("@althora", SqlDbType.Date),
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
            parms[1].Value = ((Modelo.Jornada)obj).Codigo;
            parms[2].Value = ((Modelo.Jornada)obj).Descricao;
            parms[3].Value = ((Modelo.Jornada)obj).Entrada_1;
            parms[4].Value = ((Modelo.Jornada)obj).Entrada_2;
            parms[5].Value = ((Modelo.Jornada)obj).Entrada_3;
            parms[6].Value = ((Modelo.Jornada)obj).Entrada_4;
            parms[7].Value = ((Modelo.Jornada)obj).Saida_1;
            parms[8].Value = ((Modelo.Jornada)obj).Saida_2;
            parms[9].Value = ((Modelo.Jornada)obj).Saida_3;
            parms[10].Value = ((Modelo.Jornada)obj).Saida_4;
            parms[11].Value = ((Modelo.Jornada)obj).Incdata;
            parms[12].Value = ((Modelo.Jornada)obj).Inchora;
            parms[13].Value = ((Modelo.Jornada)obj).Incusuario;
            parms[14].Value = ((Modelo.Jornada)obj).Altdata;
            parms[15].Value = ((Modelo.Jornada)obj).Althora;
            parms[16].Value = ((Modelo.Jornada)obj).Altusuario;
            
        }

        public Modelo.Jornada LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Jornada objJornada = new Modelo.Jornada();
            try
            {
                SetInstance(dr, objJornada);

                objJornada.Entrada_1Ant = objJornada.Entrada_1;
                objJornada.Saida_1Ant = objJornada.Saida_1;
                objJornada.Entrada_2Ant = objJornada.Entrada_2;
                objJornada.Saida_2Ant = objJornada.Saida_2;
                objJornada.Entrada_3Ant = objJornada.Entrada_3;
                objJornada.Saida_3Ant = objJornada.Saida_3;
                objJornada.Entrada_4Ant = objJornada.Entrada_4;
                objJornada.Saida_4Ant = objJornada.Saida_4;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornada;
        }

        public Modelo.Jornada LoadObjectCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@codigo", SqlDbType.Int, 4) };
            parms[0].Value = codigo;
            string sql = @"SELECT top(1) *,
	                               (jornada.entrada_1 + ' - ' + jornada.saida_1 
                                   + case when entrada_2 <> '--:--' then ' - ' + entrada_2 else '' end 
                                   + case when saida_2 <> '--:--' then ' - ' + saida_2 else '' end 
                                   + case when entrada_3 <> '--:--' then ' - ' + entrada_3 else '' end 
                                   + case when saida_3 <> '--:--' then ' - ' + saida_3 else '' end 
                                   + case when entrada_4 <> '--:--' then ' - ' + entrada_4 else '' end 
                                   + case when saida_4 <> '--:--' then ' - ' + saida_4 else '' end ) AS horarios
                              FROM jornada
                             WHERE codigo = @codigo";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            Modelo.Jornada objJornada = new Modelo.Jornada();
            try
            {
                SetInstance(dr, objJornada);

                objJornada.Entrada_1Ant = objJornada.Entrada_1;
                objJornada.Saida_1Ant = objJornada.Saida_1;
                objJornada.Entrada_2Ant = objJornada.Entrada_2;
                objJornada.Saida_2Ant = objJornada.Saida_2;
                objJornada.Entrada_3Ant = objJornada.Entrada_3;
                objJornada.Saida_3Ant = objJornada.Saida_3;
                objJornada.Entrada_4Ant = objJornada.Entrada_4;
                objJornada.Saida_4Ant = objJornada.Saida_4;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornada;
        }

        public List<Modelo.Jornada> GetAllList()
        {
            return GetAllList(new List<int>());
        }

        public List<Modelo.Jornada> GetAllList(List<int> idsJornadas)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idsJornadas",DbType.String)
            };

            parms[0].Value = String.Join(", ", idsJornadas);

            string cmd = @"SELECT jornada.*,
                                  (jornada.entrada_1 +
                                   ' - ' + jornada.saida_1 +
                                   case when entrada_2 <> '--:--' then ' - ' + entrada_2 else '' end + 
                                   case when saida_2 <> '--:--' then ' - ' + saida_2 else '' end +
                                   case when entrada_3 <> '--:--' then ' - ' + entrada_3 else '' end +
                                   case when saida_3 <> '--:--' then ' - ' + saida_3 else '' end +
                                   case when entrada_4 <> '--:--' then ' - ' + entrada_4 else '' end +
                                   case when saida_4 <> '--:--' then ' - ' + saida_4 else '' end
                                  ) AS horarios
                             FROM jornada ";
            if (idsJornadas.Count > 0)
            {
                cmd += " where jornada.id in (select * from dbo.F_ClausulaIn(@idsJornadas))";
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.Jornada> lista = new List<Modelo.Jornada>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Jornada objJornada = new Modelo.Jornada();
                    AuxSetInstance(dr, objJornada);
                    lista.Add(objJornada);
                }
            }
            catch (Exception ex)
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
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

        public bool JornadaExiste(Modelo.Jornada objJornada)
        {
            bool ret = false;
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@entrada_1", SqlDbType.VarChar),
                new SqlParameter("@saida_1", SqlDbType.VarChar),
                new SqlParameter("@entrada_2", SqlDbType.VarChar),
                new SqlParameter("@saida_2", SqlDbType.VarChar),
                new SqlParameter("@entrada_3", SqlDbType.VarChar),
                new SqlParameter("@saida_3", SqlDbType.VarChar),
                new SqlParameter("@entrada_4", SqlDbType.VarChar),
                new SqlParameter("@saida_4", SqlDbType.VarChar),
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = objJornada.Entrada_1;
            parms[1].Value = objJornada.Saida_1;
            parms[2].Value = objJornada.Entrada_2;
            parms[3].Value = objJornada.Saida_2;
            parms[4].Value = objJornada.Entrada_3;
            parms[5].Value = objJornada.Saida_3;
            parms[6].Value = objJornada.Entrada_4;
            parms[7].Value = objJornada.Saida_4;
            parms[8].Value = objJornada.Id;

            StringBuilder str = new StringBuilder();
            str.AppendLine("SELECT id FROM jornada");
            str.AppendLine("WHERE");
            str.AppendLine("entrada_1 = @entrada_1");
            str.AppendLine("AND saida_1 = @saida_1");
            str.AppendLine("AND entrada_2 = @entrada_2");
            str.AppendLine("AND saida_2 = @saida_2");
            str.AppendLine("AND entrada_3 = @entrada_3");
            str.AppendLine("AND saida_3 = @saida_3");
            str.AppendLine("AND entrada_4 = @entrada_4");
            str.AppendLine("AND saida_4 = @saida_4");
            str.AppendLine("AND id <> @id");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, str.ToString(), parms);

            if (dr.Read())
            {
                ret = true;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoJornada(int id)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string sql = @"select top(3) f.id, f.codigo, f.dataFechamento, f.descricao, f.observacao
                              from jornada j
                             inner join horariodetalhe dh on j.id = dh.idjornada
                             inner join horario h on h.id = dh.idhorario
                             inner join marcacao m on m.idhorario = h.id and m.idFechamentoPonto is not null
                             inner join FechamentoPonto f on m.idFechamentoPonto = f.id
                             where j.id = @id
                            Group by f.id, f.codigo, f.dataFechamento, f.descricao, f.observacao
                            order by f.dataFechamento desc";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.FechamentoPonto> lista = new List<Modelo.FechamentoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.FechamentoPonto>>(dr);
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

        public List<Modelo.Jornada> getTodosHorariosDaEmpresa(int pIdEmpresa)
        {
            //Mudar para FB
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idempresa", SqlDbType.Int) };
            parms[0].Value = pIdEmpresa;

            string comando = "SELECT DISTINCT jornada.* " +
                             ", (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idjornada = jornada.id) AS datainicial " +
                             ", (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idjornada = jornada.id) AS datafinal " +
                             "FROM jornada " +
                             "INNER JOIN horariodetalhe ON horariodetalhe.idjornada = jornada.id " +
                             "INNER JOIN funcionario ON funcionario.idhorario = horariodetalhe.idhorario " +
                             "WHERE funcionario.idempresa = @idempresa";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Jornada> listaJornada = new List<Modelo.Jornada>();
            while (dr.Read())
            {
                Modelo.Jornada objJornada = new Modelo.Jornada();
                AuxSetInstance(dr, objJornada);
                listaJornada.Add(objJornada);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaJornada;
        }

        public List<Modelo.Proxy.PxyIdPeriodo> GetFuncionariosRecalculo(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string sql = @"select m.idfuncionario Id, min(m.data) InicioPeriodo, max(m.data) FimPeriodo
                              from jornada j
                             inner join horariodetalhe hd on j.id = hd.idjornada
                             inner join horario h on h.id = hd.idhorario
                             inner join marcacao m with(nolock) on m.idhorario = h.id
                             inner join funcionario f on m.idfuncionario = f.id
                             where m.idfechamentobh is null
                               and m.idFechamentoPonto is null
                               and f.funcionarioativo = 1
                               and f.excluido = 0
                               and j.id = @id
                             group by m.idfuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyIdPeriodo> lista = new List<Modelo.Proxy.PxyIdPeriodo>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyIdPeriodo>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyIdPeriodo>>(dr);
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