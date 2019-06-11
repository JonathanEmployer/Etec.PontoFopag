using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LancamentoLoteInclusaoBanco : DAL.SQL.DALBase, DAL.ILancamentoLoteInclusaoBanco
    {

        public LancamentoLoteInclusaoBanco(DataBase database)
        {
            db = database;
            TABELA = "LancamentoLoteInclusaoBanco";

            SELECTPID = @"   SELECT * FROM LancamentoLoteInclusaoBanco WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM LancamentoLoteInclusaoBanco";

            INSERT = @"  INSERT INTO LancamentoLoteInclusaoBanco
							(codigo, incdata, inchora, incusuario, idLancamentoLote, tipoCreditoDebito, credito, debito)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idLancamentoLote, @tipoCreditoDebito, @credito, @debito)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LancamentoLoteInclusaoBanco SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idLancamentoLote = @idLancamentoLote, 
                                tipoCreditoDebito = @tipoCreditoDebito, 
                                credito = @credito, 
                                debito = @debito                            
						WHERE id = @id";

            DELETE = @"  DELETE FROM LancamentoLoteInclusaoBanco WHERE id = @id ";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LancamentoLoteInclusaoBanco";

        }

        #region Metodos
        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.ExcluirAux(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteMudancaHorario>();
                    obj = Mapper.Map<Modelo.LancamentoLoteMudancaHorario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteMudancaHorario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteMudancaHorario();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.LancamentoLoteInclusaoBanco obj)
        {
            try
            {

                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteInclusaoBanco>();
                    obj = Mapper.Map<Modelo.LancamentoLoteInclusaoBanco>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteInclusaoBanco();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteInclusaoBanco();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.LancamentoLoteInclusaoBanco> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoLoteInclusaoBanco>>();
                    obj = Mapper.Map<List<Modelo.LancamentoLoteInclusaoBanco>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.LancamentoLoteInclusaoBanco>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.LancamentoLoteInclusaoBanco>();
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
                new SqlParameter ("@idLancamentoLote", SqlDbType.Int),
				new SqlParameter ("@tipoCreditoDebito", SqlDbType.Int),
                new SqlParameter ("@credito", SqlDbType.VarChar),
                new SqlParameter ("@debito", SqlDbType.VarChar)                
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
            parms[1].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Codigo;
            parms[2].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Incdata;
            parms[3].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Inchora;
            parms[4].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Incusuario;
            parms[5].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Altdata;
            parms[6].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Althora;
            parms[7].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Altusuario;
            parms[8].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).IdLancamentoLote;
            parms[9].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Tipocreditodebito;
            parms[10].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Credito;
            parms[11].Value = ((Modelo.LancamentoLoteInclusaoBanco)obj).Debito;
        }

        public Modelo.LancamentoLoteInclusaoBanco LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoLoteInclusaoBanco obj = new Modelo.LancamentoLoteInclusaoBanco();
            try
            {
                SetaObjeto(dr, ref obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.LancamentoLoteInclusaoBanco> GetAllList()
        {
            List<Modelo.LancamentoLoteInclusaoBanco> lista = new List<Modelo.LancamentoLoteInclusaoBanco>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            SetaListaObjeto(dr,ref lista);
            return lista;
        }

        /// <summary>
        /// Retorna o LancamentoLoteInclusaoBanco de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteInclusaoBanco</returns>
        public Modelo.LancamentoLoteInclusaoBanco LoadByIdLote(int idLote)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idLote", SqlDbType.Int)
            };

            parms[0].Value = idLote;

            string sql = @" select llIB.*
                               from LancamentoLoteInclusaoBanco llIB
                              where idlancamentolote = @idLote";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteInclusaoBanco>();
                    return AutoMapper.Mapper.Map<Modelo.LancamentoLoteInclusaoBanco>(dr);
                }
                else
                {
                    return new Modelo.LancamentoLoteInclusaoBanco();
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

        public void ExcluirLancamentoLoteInclusaoBanco(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" delete from InclusaoBanco where idLancamentoLoteFuncionario in (select * from f_clausulain (@idlancamentolotefuncionario)) ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void IncluirLancamentoLoteInclusaoBanco(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" insert into InclusaoBanco (codigo, data, tipo, identificacao, tipocreditodebito, credito, debito, fechado, idusuario, incdata, inchora, incusuario, altdata, althora, altusuario, idLancamentoLoteFuncionario)
                            select isnull(codUltMud.codigo,0) + row_number() over(order by llf.id) Codigo,
								   ll.datalancamento data,
								   2,
	                               llf.idfuncionario,
								   llib.tipoCreditoDebito,
								   llib.credito,
								   llib.debito,
								   0,
								   0,
	                               llf.incdata,
	                               llf.inchora,
	                               llf.incusuario,
	                               llf.altdata,
	                               llf.althora,
	                               llf.altusuario,
	                               llf.id idLancamentoLoteFuncionario
                              from LancamentoLote ll
                             inner join lancamentoloteInclusaoBanco llib on ll.id = llib.idlancamentolote
                             inner join LancamentoLoteFuncionario llf on ll.id = llf.idlancamentolote and llf.id in ( select * from f_clausulain(@idlancamentolotefuncionario))
                             inner join funcionario f on llf.idfuncionario = f.id
                             cross join (select max(codigo) codigo from InclusaoBanco) codUltMud
                             where not exists (select * from InclusaoBanco where idLancamentoLoteFuncionario = llf.id) and llf.efetivado = 1 ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void UpdateLancamentoLoteInclusaoBanco(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" update InclusaoBanco
                               set data = ll.datalancamento,
									tipocreditodebito = llib.tipoCreditoDebito, 
									credito = llib.credito, 
									debito = llib.debito
                              from InclusaoBanco
                             inner join LancamentoLoteFuncionario llf on mudancahorario.idlancamentolotefuncionario = llf.id and llf.id in ( select * from f_clausulain(@idlancamentolotefuncionario)) and efetivado = 1
                             inner join LancamentoLote ll on ll.id = llf.idlancamentolote
                             inner join funcionario f on llf.idfuncionario = f.id
                             inner join lancamentoloteInclusaoBanco llib on ll.id = llib.idlancamentolote ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
        #endregion
    }
}
