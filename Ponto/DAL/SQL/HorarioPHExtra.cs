using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class HorarioPHExtra : DAL.SQL.DALBase, DAL.IHorarioPHExtra
    {
        public string SELECTPHOR { get; set; }
        public string SELECTPEHE { get; set; }

        public HorarioPHExtra(DataBase database)
        {
            db = database;
            TABELA = "horariophextra";

            SELECTPID = @"   SELECT * FROM horariophextra WHERE id = @id";

            SELECTALL = @"   SELECT * FROM horariophextra";

            SELECTPHOR = @"   SELECT * FROM horariophextra WHERE idhorario = @idhorario";

            SELECTPEHE = @" SELECT    empresa.id AS idempresa
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

            INSERT = @"  INSERT INTO horariophextra
							( codigo,  idhorario,  aplicacao,  percentualextra,  quantidadeextra,  marcapercentualextra,  incdata,  inchora,  incusuario,  considerapercextrasemana,  tipoacumulo,  percentualextrasegundo,  percentualExtraNoturna,  quantidadeExtraNoturna,  percentualextrasegundoNoturna)
							VALUES
							(@codigo, @idhorario, @aplicacao, @percentualextra, @quantidadeextra, @marcapercentualextra, @incdata, @inchora, @incusuario, @considerapercextrasemana, @tipoacumulo, @percentualextrasegundo, @percentualExtraNoturna, @quantidadeExtraNoturna, @percentualextrasegundoNoturna) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE horariophextra SET codigo = @codigo
							, idhorario = @idhorario
							, aplicacao = @aplicacao
							, percentualextra = @percentualextra
							, quantidadeextra = @quantidadeextra
							, marcapercentualextra = @marcapercentualextra
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , considerapercextrasemana = @considerapercextrasemana
                            , tipoacumulo = @tipoacumulo
                            , percentualextrasegundo = @percentualextrasegundo
                            , percentualExtraNoturna = @percentualExtraNoturna
                            , quantidadeExtraNoturna = @quantidadeExtraNoturna
                            , percentualextrasegundoNoturna = @percentualextrasegundoNoturna
						WHERE id = @id";

            DELETE = @"  DELETE FROM horariophextra WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM horariophextra";

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
            ((Modelo.HorarioPHExtra)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioPHExtra)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            //((Modelo.HorarioPHExtra)obj).Aplicacao = Convert.ToInt16(dr["aplicacao"]);
            ((Modelo.HorarioPHExtra)obj).Percentualextra = Convert.ToDecimal(dr["percentualextra"]);
            ((Modelo.HorarioPHExtra)obj).Quantidadeextra = Convert.ToString(dr["quantidadeextra"]);
            ((Modelo.HorarioPHExtra)obj).Marcapercentualextra = Convert.ToInt16(dr["marcapercentualextra"]);
            ((Modelo.HorarioPHExtra)obj).Considerapercextrasemana = Convert.ToInt16(dr["considerapercextrasemana"]);
            ((Modelo.HorarioPHExtra)obj).TipoAcumulo = (dr["tipoacumulo"] is DBNull ? Convert.ToInt16(-1) : Convert.ToInt16(dr["tipoacumulo"]));
            ((Modelo.HorarioPHExtra)obj).PercentualExtraSegundo = (dr["percentualextrasegundo"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["percentualextrasegundo"]));
            if (!(dr["PercentualExtraNoturna"] is DBNull))
            {
                ((Modelo.HorarioPHExtra)obj).PercentualExtraNoturna = Convert.ToDecimal(dr["PercentualExtraNoturna"]);
            }
            ((Modelo.HorarioPHExtra)obj).QuantidadeExtraNoturna = Convert.ToString(dr["QuantidadeExtraNoturna"]);
            if (!(dr["percentualextrasegundoNoturna"] is DBNull))
            {
                ((Modelo.HorarioPHExtra)obj).PercentualExtraSegundoNoturna = Convert.ToInt16(dr["percentualextrasegundoNoturna"]);
            }
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idhorario", SqlDbType.Int),
				new SqlParameter ("@aplicacao", SqlDbType.TinyInt),
				new SqlParameter ("@percentualextra", SqlDbType.Decimal),
				new SqlParameter ("@quantidadeextra", SqlDbType.VarChar),
				new SqlParameter ("@marcapercentualextra", SqlDbType.TinyInt),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@considerapercextrasemana", SqlDbType.TinyInt),
                new SqlParameter ("@tipoacumulo", SqlDbType.SmallInt),
                new SqlParameter ("@percentualextrasegundo", SqlDbType.SmallInt),
                new SqlParameter ("@percentualExtraNoturna", SqlDbType.Decimal),
                new SqlParameter ("@quantidadeExtraNoturna", SqlDbType.VarChar),
                new SqlParameter ("@percentualextrasegundoNoturna", SqlDbType.SmallInt)
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
            parms[1].Value = ((Modelo.HorarioPHExtra)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioPHExtra)obj).Idhorario;
            //parms[3].Value = ((Modelo.HorarioPHExtra)obj).Aplicacao;
            parms[4].Value = ((Modelo.HorarioPHExtra)obj).Percentualextra;
            parms[5].Value = ((Modelo.HorarioPHExtra)obj).Quantidadeextra;
            parms[6].Value = ((Modelo.HorarioPHExtra)obj).Marcapercentualextra;
            parms[7].Value = ((Modelo.HorarioPHExtra)obj).Incdata;
            parms[8].Value = ((Modelo.HorarioPHExtra)obj).Inchora;
            parms[9].Value = ((Modelo.HorarioPHExtra)obj).Incusuario;
            parms[10].Value = ((Modelo.HorarioPHExtra)obj).Altdata;
            parms[11].Value = ((Modelo.HorarioPHExtra)obj).Althora;
            parms[12].Value = ((Modelo.HorarioPHExtra)obj).Altusuario;
            parms[13].Value = ((Modelo.HorarioPHExtra)obj).Considerapercextrasemana;
            parms[14].Value = ((Modelo.HorarioPHExtra)obj).TipoAcumulo;
            parms[15].Value = ((Modelo.HorarioPHExtra)obj).PercentualExtraSegundo;
            parms[16].Value = ((Modelo.HorarioPHExtra)obj).PercentualExtraNoturna;
            parms[17].Value = ((Modelo.HorarioPHExtra)obj).QuantidadeExtraNoturna;
            parms[18].Value = ((Modelo.HorarioPHExtra)obj).PercentualExtraSegundoNoturna;
        }

        public Modelo.HorarioPHExtra LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
            try
            {

                SetInstance(dr, objHorarioPHExtra);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorarioPHExtra;
        }

        protected SqlDataReader LoadDataReaderPorHorario(int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int, 4) };
            parms[0].Value = idHorario;

            return db.ExecuteReader(CommandType.Text, SELECTPHOR, parms);
        }

        protected SqlDataReader LoadDataReaderPorHorario(SqlTransaction trans, int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int, 4) };
            parms[0].Value = idHorario;

            return TransactDbOps.ExecuteReader(trans, CommandType.Text, SELECTPHOR, parms);
        }

        public List<Modelo.HorarioPHExtra> LoadPorHorario(int idHorario)
        {
            SqlDataReader dr = LoadDataReaderPorHorario(idHorario);

            List<Modelo.HorarioPHExtra> lista = new List<Modelo.HorarioPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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

        public List<Modelo.HorarioPHExtra> LoadPorHorario(SqlTransaction trans, int idHorario)
        {
            SqlDataReader dr = LoadDataReaderPorHorario(trans, idHorario);

            List<Modelo.HorarioPHExtra> lista = new List<Modelo.HorarioPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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

        public DataTable GetPercentualHoraExtraDepartamento(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@dataInicial", SqlDbType.DateTime),
                new SqlParameter("@dataFinal", SqlDbType.DateTime)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;

            DataTable dt = new DataTable();
            string aux = SELECTPEHE;

            aux += @"
                WHERE ISNULL(funcionario.excluido, 0) = 0 
                AND (ISNULL(funcionario.funcionarioativo, 0) = 1
                    OR
                    funcionario.id in 
                (
                select marcacao_view.idfuncionario from marcacao_view 
                where 
                (marcacao_view.horasextrasdiurna != '--:--'
                or  marcacao_view.horasextranoturna != '--:--'
                or  marcacao_view.horastrabalhadas != '--:--'
                or  marcacao_view.horastrabalhadasnoturnas != '--:--')
                and marcacao_view.data between @dataInicial and @dataFinal
                group by marcacao_view.idfuncionario)) ";

            switch (tipo)
            {
                case 0: aux += " AND funcionario.idempresa IN " + empresas; break;
                case 1: aux += " AND funcionario.iddepartamento IN " + departamentos; break;
                case 2: aux += " AND funcionario.id IN " + funcionarios; break;
            }

            aux += " ORDER BY LOWER(empresa.nome), LOWER(departamento.descricao), LOWER(funcionario.nome)";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms); 
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPercentualHoraExtra(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTPEHE;

            aux += " WHERE ISNULL(funcionario.excluido, 0) = 0  AND ISNULL(funcionario.funcionarioativo, 0) = 1";

            switch (tipo)
            {
                case 0: aux += " AND funcionario.idempresa IN " + empresas; break;
                case 1: aux += " AND funcionario.iddepartamento IN " + departamentos; break;
                case 2: aux += " AND funcionario.id IN " + funcionarios; break;
            }

            aux += " ORDER BY LOWER(empresa.nome), LOWER(funcionario.nome)";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.HorarioPHExtra> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM horariophextra", parms);
            List<Modelo.HorarioPHExtra> lista = new List<Modelo.HorarioPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioPHExtra objHorarioPHExtra = new Modelo.HorarioPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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
