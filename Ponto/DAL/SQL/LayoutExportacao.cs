using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class LayoutExportacao : DAL.SQL.DALBase, DAL.ILayoutExportacao
    {
        private DAL.SQL.ExportacaoCampos dalExportacaoCampos;

            
        public LayoutExportacao(DataBase database,  Modelo.Cw_Usuario pUsuarioLogado) : this(database)
        {
            dalExportacaoCampos.UsuarioLogado = pUsuarioLogado;
        }

        public LayoutExportacao(DataBase database)
        {
            db = database;
            dalExportacaoCampos = new DAL.SQL.ExportacaoCampos(db);
            TABELA = "layoutexportacao";

            SELECTPID = @"   SELECT * FROM layoutexportacao WHERE id = @id";

            SELECTALL = @"   SELECT   layoutexportacao.id
                                    , layoutexportacao.descricao
                                    , layoutexportacao.codigo
                             FROM layoutexportacao";

            INSERT = @"  INSERT INTO layoutexportacao
							(codigo, descricao, incdata, inchora, incusuario)
							VALUES
							(@codigo, @descricao, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE layoutexportacao SET
							  codigo = @codigo
							, descricao = @descricao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM layoutexportacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM layoutexportacao";
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
                obj = new Modelo.LayoutExportacao();
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
            ((Modelo.LayoutExportacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.LayoutExportacao)obj).Descricao = Convert.ToString(dr["descricao"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.LayoutExportacao)obj).Codigo;
            parms[2].Value = ((Modelo.LayoutExportacao)obj).Descricao;
            parms[3].Value = ((Modelo.LayoutExportacao)obj).Incdata;
            parms[4].Value = ((Modelo.LayoutExportacao)obj).Inchora;
            parms[5].Value = ((Modelo.LayoutExportacao)obj).Incusuario;
            parms[6].Value = ((Modelo.LayoutExportacao)obj).Altdata;
            parms[7].Value = ((Modelo.LayoutExportacao)obj).Althora;
            parms[8].Value = ((Modelo.LayoutExportacao)obj).Altusuario;
        }

        public Modelo.LayoutExportacao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LayoutExportacao objLayoutExportacao = new Modelo.LayoutExportacao();
            try
            {
                SetInstance(dr, objLayoutExportacao);
                objLayoutExportacao.ExportacaoCampos = dalExportacaoCampos.LoadPLayout(objLayoutExportacao.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objLayoutExportacao;
        }

        public List<Modelo.LayoutExportacao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM layoutexportacao", parms);

            List<Modelo.LayoutExportacao> lista = new List<Modelo.LayoutExportacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.LayoutExportacao objLayoutExportacao = new Modelo.LayoutExportacao();
                    AuxSetInstance(dr, objLayoutExportacao);
                    lista.Add(objLayoutExportacao);
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

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = TransactDbOps.MaxCodigo(trans, MAXCOD);
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            SalvarCampos(trans, (Modelo.LayoutExportacao)obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            SalvarCampos(trans, (Modelo.LayoutExportacao)obj);

            cmd.Parameters.Clear();
        }

        private void SalvarCampos(SqlTransaction trans, Modelo.LayoutExportacao obj)
        {
            foreach (Modelo.ExportacaoCampos dja in obj.ExportacaoCampos)
            {
                dja.IdLayoutExportacao = obj.Id;
                switch (dja.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalExportacaoCampos.Incluir(trans, dja);
                        break;
                    case Modelo.Acao.Alterar:
                        dalExportacaoCampos.Alterar(trans, dja);
                        break;
                    case Modelo.Acao.Excluir:
                        dalExportacaoCampos.Excluir(trans, dja);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
