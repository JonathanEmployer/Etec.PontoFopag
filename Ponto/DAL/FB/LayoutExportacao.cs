using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class LayoutExportacao : DAL.FB.DALBase, DAL.ILayoutExportacao
    {

        private LayoutExportacao()
        {
            GEN = "GEN_layoutexportacao_id";

            TABELA = "layoutexportacao";

            SELECTPID = "SELECT * FROM \"layoutexportacao\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"layoutexportacao\".\"id\" " +
                              ", \"layoutexportacao\".\"descricao\" " +
                              ", \"layoutexportacao\".\"codigo\" " +
                              "FROM \"layoutexportacao\"";

            INSERT = "  INSERT INTO \"layoutexportacao\"" +
                        "(\"codigo\", \"descricao\", \"incdata\", \"inchora\", \"incusuario\")" +
                        "VALUES" +
                        "(@codigo, @descricao, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"layoutexportacao\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";
            
            DELETE = "DELETE FROM \"layoutexportacao\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"layoutexportacao\"";
        }

        #region Singleton

        private static volatile FB.LayoutExportacao _instancia = null;

        public static FB.LayoutExportacao GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.LayoutExportacao))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.LayoutExportacao();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
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

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.LayoutExportacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.LayoutExportacao)obj).Descricao = Convert.ToString(dr["descricao"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
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
            FbDataReader dr = LoadDataReader(id);

            Modelo.LayoutExportacao objLayoutExportacao = new Modelo.LayoutExportacao();
            try
            {
                SetInstance(dr, objLayoutExportacao);

                DAL.FB.ExportacaoCampos dalExportacaoCampos = DAL.FB.ExportacaoCampos.GetInstancia;
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
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"layoutexportacao\"", parms);

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
            }
            return lista;
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = MaxCodigo(trans);
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            SalvarCampos(trans, (Modelo.LayoutExportacao)obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizando em outro registro. Verifique.");
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            SalvarCampos(trans, (Modelo.LayoutExportacao)obj);

            cmd.Parameters.Clear();
        }

        private void SalvarCampos(FbTransaction trans, Modelo.LayoutExportacao obj)
        {
            DAL.FB.ExportacaoCampos dalExportacaoCampos = DAL.FB.ExportacaoCampos.GetInstancia;
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

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}