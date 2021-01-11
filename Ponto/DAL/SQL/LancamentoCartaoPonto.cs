using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class LancamentoCartaoPonto : DAL.SQL.DALBase, DAL.ILancamentoCartaoPonto
    {
        private DAL.IBilheteSimp dalBilheteSimp;
        public LancamentoCartaoPonto(DataBase database)
        {
            db = database;
            TABELA = "";

            SELECTPID = @"   ";

            SELECTALL = @"    ";

            INSERT = @"  ";

            UPDATE = @"  ";

            DELETE = @"   ";

            MAXCOD = @"   ";
            dalBilheteSimp = new DAL.SQL.BilhetesImp(db);
            dalBilheteSimp.UsuarioLogado = UsuarioLogado;

        }

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
                obj = new Modelo.LancamentoCartaoPonto();
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
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
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
        }

        public Modelo.LancamentoCartaoPonto LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoCartaoPonto obj = new Modelo.LancamentoCartaoPonto();
            try
            {

                SetInstance(dr, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.LancamentoCartaoPonto> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.LancamentoCartaoPonto> lista = new List<Modelo.LancamentoCartaoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LancamentoCartaoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LancamentoCartaoPonto>>(dr);
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

        public void IncluirRegistros(List<Modelo.BilhetesImp> bilhetes, List<Modelo.LancamentoCartaoPontoRegistros> registros)
        {
            DataTable dtRegs = new DataTable();
            dtRegs.Columns.Add("IdFuncionario", typeof(int));
            dtRegs.Columns.Add("Data", typeof(DateTime));
            dtRegs.Columns.Add("E1", typeof(string));
            dtRegs.Columns.Add("E2", typeof(string));
            dtRegs.Columns.Add("E3", typeof(string));
            dtRegs.Columns.Add("E4", typeof(string));
            dtRegs.Columns.Add("E5", typeof(string));
            dtRegs.Columns.Add("E6", typeof(string));
            dtRegs.Columns.Add("E7", typeof(string));
            dtRegs.Columns.Add("E8", typeof(string));

            dtRegs.Columns.Add("S1", typeof(string));
            dtRegs.Columns.Add("S2", typeof(string));
            dtRegs.Columns.Add("S3", typeof(string));
            dtRegs.Columns.Add("S4", typeof(string));
            dtRegs.Columns.Add("S5", typeof(string));
            dtRegs.Columns.Add("S6", typeof(string));
            dtRegs.Columns.Add("S7", typeof(string));
            dtRegs.Columns.Add("S8", typeof(string));

            dtRegs.Columns.Add("RelE1", typeof(string));
            dtRegs.Columns.Add("RelE2", typeof(string));
            dtRegs.Columns.Add("RelE3", typeof(string));
            dtRegs.Columns.Add("RelE4", typeof(string));
            dtRegs.Columns.Add("RelE5", typeof(string));
            dtRegs.Columns.Add("RelE6", typeof(string));
            dtRegs.Columns.Add("RelE7", typeof(string));
            dtRegs.Columns.Add("RelE8", typeof(string));

            dtRegs.Columns.Add("RelS1", typeof(string));
            dtRegs.Columns.Add("RelS2", typeof(string));
            dtRegs.Columns.Add("RelS3", typeof(string));
            dtRegs.Columns.Add("RelS4", typeof(string));
            dtRegs.Columns.Add("RelS5", typeof(string));
            dtRegs.Columns.Add("RelS6", typeof(string));
            dtRegs.Columns.Add("RelS7", typeof(string));
            dtRegs.Columns.Add("RelS8", typeof(string));

            dtRegs.Columns.Add("Usuario", typeof(string));

            foreach (var reg in registros.Where(w => bilhetes.Select(s => s.Data).Distinct().Contains(w.Data)))
            {
                dtRegs.Rows.Add(reg.IdFuncionario, reg.Data,
                                reg.E1, reg.E2, reg.E3, reg.E4, reg.E5, reg.E6, reg.E7, reg.E8,
                                reg.S1, reg.S2, reg.S3, reg.S4, reg.S5, reg.S6, reg.S7, reg.S8,
                                !string.IsNullOrEmpty(reg.E1) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E2) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E3) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E4) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E5) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E6) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E7) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.E8) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S1) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S2) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S3) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S4) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S5) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S6) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S7) ? "MA" : "",
                                !string.IsNullOrEmpty(reg.S8) ? "MA" : "",
                                UsuarioLogado.Login) ;
            }

            string comando = @" UPDATE m
                                   SET m.entrada_1 = mu.e1,
	                                   m.entrada_2 = mu.e2,
	                                   m.entrada_3 = mu.e3,
	                                   m.entrada_4 = mu.e4,
	                                   m.entrada_5 = mu.e5,
	                                   m.entrada_6 = mu.e6,
	                                   m.entrada_7 = mu.e7,
	                                   m.entrada_8 = mu.e8,
	                                   m.saida_1 = mu.s1,
	                                   m.saida_2 = mu.s2,
	                                   m.saida_3 = mu.s3,
	                                   m.saida_4 = mu.s4,
	                                   m.saida_5 = mu.s5,
	                                   m.saida_6 = mu.s6,
	                                   m.saida_7 = mu.s7,
	                                   m.saida_8 = mu.s8,
                                       m.ent_num_relogio_1 = mu.RelE1,
                                       m.ent_num_relogio_2 = mu.RelE2,
                                       m.ent_num_relogio_3 = mu.RelE3,
                                       m.ent_num_relogio_4 = mu.RelE4,
                                       m.ent_num_relogio_5 = mu.RelE5,
                                       m.ent_num_relogio_6 = mu.RelE6,
                                       m.ent_num_relogio_7 = mu.RelE7,
                                       m.ent_num_relogio_8 = mu.RelE8,
                                       m.sai_num_relogio_1 = mu.RelS1,
                                       m.sai_num_relogio_2 = mu.RelS2,
                                       m.sai_num_relogio_3 = mu.RelS3,
                                       m.sai_num_relogio_4 = mu.RelS4,
                                       m.sai_num_relogio_5 = mu.RelS5,
                                       m.sai_num_relogio_6 = mu.RelS6,
                                       m.sai_num_relogio_7 = mu.RelS7,
                                       m.sai_num_relogio_8 = mu.RelS8,
	                                   m.altdata = convert(date,GETDATE()),
	                                   m.althora = GETDATE(),
	                                   m.altusuario = mu.Usuario
                                  FROM marcacao m
                                 INNER JOIN #marcacaoUpdate mu ON m.idfuncionario = mu.idfuncionario and m.data = mu.data";

            bilhetes.ForEach(f => f.Chave = f.ToMD5());
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("", conn, trans))
                    {
                        EnviarBulkCopy(dtRegs, conn, trans, command, comando, "marcacaoUpdate");
                        dalBilheteSimp.UsuarioLogado = UsuarioLogado;
                        dalBilheteSimp.InserirRegistros<Modelo.BilhetesImp>(bilhetes, trans);
                        trans.Commit();
                    }
                }
            } 
        }
    }
}
