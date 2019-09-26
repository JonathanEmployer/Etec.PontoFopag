using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modelo.Proxy.Relatorios;

namespace DAL.SQL
{
    public class Afastamento : DAL.SQL.DALBase, DAL.IAfastamento
    {
        public Afastamento(DataBase database)
        {
            db = database;
            TABELA = "afastamento";

            SELECTPID = @"
                SELECT  afastamento.*, 
	                case when afastamento.tipo = 0 then 'Individual' when afastamento.tipo = 1 then 'Departamento' when afastamento.tipo = 2 then 'Empresa' when afastamento.tipo = 3 then 'Contrato' else '' end as TipoAfastamentoStr,
	                case
			                when tipo = 3 then (select 'Contrato - ' + emp.nome + ' - ' + ct.codigocontrato from contrato ct left join empresa emp on emp.id = ct.idempresa where ct.id = afastamento.idcontrato)
			                when tipo = 2 then (SELECT empresa.nome FROM empresa WHERE empresa.id = afastamento.idempresa) 
			                when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = afastamento.iddepartamento) 
			                when tipo = 0 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = afastamento.idfuncionario) end AS Nome,
	                (select descricao from ocorrencia where ocorrencia.id = afastamento.idocorrencia) as ocorrencia,
	                coalesce(emp.id, dep.idempresa, fun.idempresa, 0) as idempresafiltro
	                ,coalesce(CONVERT(varchar, ct.codigo) + ' | ' + convert(varchar, e.codigo) + ' | ' + e.nome, '') as NomeContrato
	                FROM afastamento
                left join empresa emp on afastamento.idempresa = emp.id
                left join departamento dep on afastamento.iddepartamento = dep.id
                left join contrato ct on ct.id = afastamento.idcontrato
                left join funcionario fun on afastamento.idfuncionario = fun.id
                left join empresa e on ct.idempresa = e.id
                WHERE afastamento.id = @id";

            INSERT = @" INSERT INTO afastamento
							(codigo, descricao, idocorrencia, tipo, abonado, datai, dataf, idfuncionario, idempresa, iddepartamento, horai, horaf, parcial, semcalculo, incdata, inchora, incusuario, idcontrato, bSuspensao, IdIntegracao, Observacao, SemAbono)
							VALUES
							(@codigo, @descricao, @idocorrencia, @tipo, @abonado, @datai, @dataf, @idfuncionario, @idempresa, @iddepartamento, @horai, @horaf, @parcial, @semcalculo, @incdata, @inchora, @incusuario, @idcontrato, @bSuspensao, @IdIntegracao, @Observacao, @SemAbono)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE afastamento SET codigo = @codigo
							, descricao = @descricao
							, idocorrencia = @idocorrencia
							, tipo = @tipo
							, abonado = @abonado
							, datai = @datai
							, dataf = @dataf
							, idfuncionario = @idfuncionario
							, idempresa = @idempresa
							, iddepartamento = @iddepartamento
							, horai = @horai
							, horaf = @horaf
							, parcial = @parcial
							, semcalculo = @semcalculo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idcontrato = @idcontrato
                            , bSuspensao = @bSuspensao
                            , IdIntegracao = @IdIntegracao
                            , Observacao = @Observacao
                            , semabono = @SemAbono
						WHERE id = @id";

            DELETE = @"  DELETE FROM afastamento WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM afastamento";

            SELECTALLLIST = @"  SELECT afastamento.*, 
	                                   CASE WHEN afastamento.tipo = 0 THEN 'Individual' 
			                                WHEN afastamento.tipo = 1 THEN 'Departamento' 
			                                WHEN afastamento.tipo = 2 THEN 'Empresa' 
			                                WHEN afastamento.tipo = 3 THEN 'Contrato' 
			                                ELSE '' 
	                                   END AS TipoAfastamentoStr,
	                                   CASE WHEN tipo = 3 THEN 'Contrato - ' + EmpContrato.nome + ' - ' + ct.codigocontrato
			                                WHEN tipo = 2 THEN emp.nome
			                                WHEN tipo = 1 THEN dep.descricao 
			                                WHEN tipo = 0 THEN fun.nome 
	                                   END AS Nome,
	                                   ocorrencia.descricao AS ocorrencia,
	                                   COALESCE(emp.id, dep.idempresa, fun.idempresa, 0) AS idempresafiltro,
	                                   COALESCE(CONVERT(VARCHAR, ct.codigo) + ' | ' + convert(VARCHAR, EmpContrato.codigo) + ' | ' + EmpContrato.nome, '') AS NomeContrato
                                  FROM afastamento
                                  LEFT JOIN empresa emp ON afastamento.idempresa = emp.id
                                  LEFT JOIN departamento dep ON afastamento.iddepartamento = dep.id
                                  LEFT JOIN funcionario fun ON afastamento.idfuncionario = fun.id
                                  LEFT JOIN dbo.ocorrencia ON ocorrencia.id = afastamento.idocorrencia
                                 OUTER APPLY (SELECT TOP(1) * 
				                                FROM contrato ct 
			                                   WHERE ct.id = afastamento.idcontrato 
			                                   ORDER BY ct.inchora) ct
                                  LEFT JOIN empresa EmpContrato on ct.idempresa = EmpContrato.id ";
        }
        public string SELECTRELF
        {
            get
            {
                return @" SELECT     afastamento.id
                                    , ocorrencia.descricao AS ocorrencia
                                    , funcionario.nome + ' - ' + departamento.descricao + ' - ' + empresa.nome AS nome
                                    , afastamento.datai
                                    , afastamento.dataf
                                    , afastamento.abonado
                                    , coalesce(CONVERT(varchar, ct.codigo) + ' | ' + convert(varchar, empresa.codigo) + ' | ' + empresa.nome, '') as NomeContrato
                                    , funcionario.nome as nome2
                                    , departamento.descricao 
                                    , empresa.nome AS empresa
                                    , empresa.id idempresa
                            FROM    afastamento 
                            INNER JOIN funcionario ON funcionario.id = afastamento.idfuncionario
                            INNER JOIN empresa ON empresa.id = funcionario.idempresa
                            INNER JOIN departamento ON departamento.id = funcionario.iddepartamento                             
                            INNER JOIN ocorrencia ON ocorrencia.id = afastamento.idocorrencia
                            left join contrato ct on ct.id = afastamento.idcontrato
                            WHERE afastamento.Id > 0 AND afastamento.tipo = 0 "
                            + GetWhereSelectAll();
            }
        }

        public string SELECTRELD
        {
            get
            {
                return @" SELECT     afastamento.id
                                    , ocorrencia.descricao AS ocorrencia
                                    , departamento.descricao + ' - ' + empresa.nome AS nome
                                    , afastamento.datai
                                    , afastamento.dataf
                                    , afastamento.abonado
                                    , coalesce(CONVERT(varchar, ct.codigo) + ' | ' + convert(varchar, empresa.codigo) + ' | ' + empresa.nome, '') as NomeContrato
                                    , departamento.descricao 
                                    , empresa.nome AS empresa
                                    , empresa.id idempresa
                            FROM    afastamento 
                            INNER JOIN departamento ON departamento.id = afastamento.iddepartamento
                            INNER JOIN empresa ON empresa.id = departamento.idempresa
                            INNER JOIN ocorrencia ON ocorrencia.id = afastamento.idocorrencia
                            left join contrato ct on ct.id = afastamento.idcontrato
                            WHERE afastamento.Id > 0 AND afastamento.tipo = 1"
                            + GetWhereSelectAll();
            }
        }

        public string SELECTRELE
        {
            get
            {
                return @" SELECT     afastamento.id
                                    , ocorrencia.descricao AS ocorrencia
                                    , empresa.nome AS nome
                                    , afastamento.datai
                                    , afastamento.dataf
                                    , afastamento.abonado
                                    , coalesce(CONVERT(varchar, ct.codigo) + ' | ' + convert(varchar, empresa.codigo) + ' | ' + empresa.nome, '') as NomeContrato
                                    , empresa.id idempresa
                            FROM    afastamento                             
                            INNER JOIN empresa ON empresa.id = afastamento.idempresa
                            INNER JOIN ocorrencia ON ocorrencia.id = afastamento.idocorrencia
                            left join contrato ct on ct.id = afastamento.idcontrato
                            WHERE afastamento.id > 0 AND afastamento.tipo = 2"
                             + GetWhereSelectAll();
            }
        }

        public string SELECTMAR
        {
            get
            {
                return @"   SELECT afastamento.*
                            , coalesce(CONVERT(varchar, ct.codigo) + ' | ' + convert(varchar, empresa.codigo) + ' | ' + empresa.nome, '') as NomeContrato
                            FROM afastamento
                            INNER JOIN funcionario on funcionario.id = afastamento.idfuncionario
                            INNER JOIN empresa on empresa.id = funcionario.idempresa
                            left join contrato ct on ct.id = afastamento.idcontrato
                             WHERE afastamento.datai = @data 
                             AND isnull(afastamento.dataf, '9999-12-31') = @data 
                             AND afastamento.idfuncionario = @idfuncionario
                             AND afastamento.tipo = 0";
            }
        }

        public string VERIFICA
        {
            get
            {
                return @"   SELECT ISNULL(COUNT(id), 0) AS qt
                            FROM afastamento
                            WHERE ((@datainicial >= datai AND @datainicial <= isnull(dataf, '9999-12-31'))
                            OR (@datafinal >= datai AND @datafinal <= isnull(dataf, '9999-12-31'))
                            OR (@datainicial <= datai AND @datafinal >= isnull(dataf, '9999-12-31')))
                            AND tipo = @tipo
                            AND id <> @id
                            AND ((idfuncionario = @identificacao AND tipo = 0)
                            OR (idempresa = @identificacao AND tipo = 2)
                            OR (iddepartamento = @identificacao AND tipo = 1))";
            }
        }

        protected override string SELECTALL
        {
            get
            {
                return @"   SELECT   afastamento.id
                                    , afastamento.codigo
                                    , o.descricao as ocorrencia
                                    , afastamento.datai
                                    , afastamento.dataf
                                    , case when afastamento.tipo = 0 then 'Individual' when afastamento.tipo = 1 then 'Departamento' when afastamento.tipo = 2 then 'Empresa' when afastamento.tipo = 3 then 'Contrato' else '' end as tipo
                                    , case when tipo = 3 then (select 'Contrato - ' + emp.nome + ' - ' + ct.codigocontrato from contrato ct left join empresa emp on emp.id = ct.idempresa where ct.id = afastamento.idcontrato)
                                           when tipo = 2 then (SELECT empresa.nome FROM empresa WHERE empresa.id = afastamento.idempresa) 
                                           when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = afastamento.iddepartamento) 
                                           when tipo = 0 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = afastamento.idfuncionario) end AS nome
                             FROM afastamento 
                             INNER JOIN ocorrencia o ON o.id = afastamento.idocorrencia
                             LEFT JOIN funcionario ON funcionario.id = afastamento.idfuncionario
                             LEFT JOIN contrato ct ON ct.id = afastamento.idcontrato
                             LEFT JOIN departamento ON departamento.id = (case when afastamento.tipo = 0 then funcionario.iddepartamento else afastamento.iddepartamento end)
                             INNER JOIN empresa ON empresa.id = (case when afastamento.tipo = 0 then funcionario.idempresa when tipo = 1 then departamento.idempresa when tipo = 3 then ct.idempresa else afastamento.idempresa end)
                             WHERE 1 = 1 "
                            + GetWhereSelectAll();
            }
            set
            {
                base.SELECTALL = value;
            }
        }


        #region Metodos
        public override void Incluir(Modelo.ModeloBase obj)
        {
            if (obj.Codigo < MaxCodigo())
                obj.Codigo = MaxCodigo();

            base.Incluir(obj);
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiCampos(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Afastamento();
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

        private void AtribuiCampos(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Afastamento)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Afastamento)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Afastamento)obj).IdOcorrencia = Convert.ToInt32(dr["idocorrencia"]);
            ((Modelo.Afastamento)obj).Tipo = Convert.ToInt32(dr["tipo"]);
            ((Modelo.Afastamento)obj).Abonado = Convert.ToInt16(dr["abonado"]);
            ((Modelo.Afastamento)obj).Datai = Convert.ToDateTime(dr["datai"]);
            if (!(dr["dataf"] is DBNull))
            {
                ((Modelo.Afastamento)obj).Dataf = Convert.ToDateTime(dr["dataf"]);
            }
            ((Modelo.Afastamento)obj).IdFuncionario = (dr["idfuncionario"] is DBNull ? 0 : Convert.ToInt32(dr["idfuncionario"]));
            ((Modelo.Afastamento)obj).IdEmpresa = (dr["idempresa"] is DBNull ? 0 : Convert.ToInt32(dr["idempresa"]));
            ((Modelo.Afastamento)obj).IdDepartamento = (dr["iddepartamento"] is DBNull ? 0 : Convert.ToInt32(dr["iddepartamento"]));
            ((Modelo.Afastamento)obj).Horai = Convert.ToString(dr["horai"]);
            ((Modelo.Afastamento)obj).Horaf = Convert.ToString(dr["horaf"]);
            ((Modelo.Afastamento)obj).Parcial = Convert.ToInt16(dr["parcial"]);
            ((Modelo.Afastamento)obj).SemCalculo = Convert.ToInt16(dr["semcalculo"]);
            var colunas = Enumerable.Range(0, dr.FieldCount).Select(i => dr.GetName(i)).ToArray();
            if (colunas.Contains("codigoOcorrencia"))
            {
                ((Modelo.Afastamento)obj).CodigoOcorrencia = Convert.ToInt16(dr["codigoOcorrencia"]);
                ((Modelo.Afastamento)obj).ocorrencia = Convert.ToString(dr["descricaoOcorrencia"]);
            }
            if (!(dr["idcontrato"] is DBNull))
            {
                ((Modelo.Afastamento)obj).IdContrato = Convert.ToInt32(dr["idcontrato"]);
            }
            ((Modelo.Afastamento)obj).NomeContrato = Convert.ToString(dr["NomeContrato"]);
            ((Modelo.Afastamento)obj).bSuspensao = Convert.ToBoolean(dr["bSuspensao"]);
            ((Modelo.Afastamento)obj).IdIntegracao = Convert.ToString(dr["idIntegracao"]);
            object valIdLancamentoLoteFuncionario = dr["idLancamentoLoteFuncionario"];
            Int32? idLancamentoLoteFuncionarioint = (valIdLancamentoLoteFuncionario == null || valIdLancamentoLoteFuncionario is DBNull) ? (Int32?)null : (Int32?)valIdLancamentoLoteFuncionario;
            ((Modelo.Afastamento)obj).IdLancamentoLoteFuncionario = idLancamentoLoteFuncionarioint;
            ((Modelo.Afastamento)obj).Observacao = Convert.ToString(dr["Observacao"]);
            ((Modelo.Afastamento)obj).SemAbono = Convert.ToBoolean(dr["SemAbono"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@descricao", SqlDbType.VarChar),
                new SqlParameter ("@idocorrencia", SqlDbType.Int),
                new SqlParameter ("@tipo", SqlDbType.Int),
                new SqlParameter ("@abonado", SqlDbType.TinyInt),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime),
                new SqlParameter ("@idfuncionario", SqlDbType.Int),
                new SqlParameter ("@idempresa", SqlDbType.Int),
                new SqlParameter ("@iddepartamento", SqlDbType.Int),
                new SqlParameter ("@horai", SqlDbType.VarChar),
                new SqlParameter ("@horaf", SqlDbType.VarChar),
                new SqlParameter ("@parcial", SqlDbType.TinyInt),
                new SqlParameter ("@semcalculo", SqlDbType.TinyInt),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idcontrato", SqlDbType.Int),
                new SqlParameter ("@NomeContrato", SqlDbType.VarChar),
                new SqlParameter ("@bSuspensao", SqlDbType.Bit),
                new SqlParameter ("@IdIntegracao", SqlDbType.VarChar),
                new SqlParameter ("@idLancamentoLoteFuncionario", SqlDbType.Int),
                new SqlParameter ("@Observacao", SqlDbType.VarChar),
                new SqlParameter ("@SemAbono", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.Afastamento)obj).Codigo;
            parms[2].Value = ((Modelo.Afastamento)obj).Descricao;
            parms[3].Value = ((Modelo.Afastamento)obj).IdOcorrencia;
            parms[4].Value = ((Modelo.Afastamento)obj).Tipo;
            parms[5].Value = ((Modelo.Afastamento)obj).Abonado;
            parms[6].Value = ((Modelo.Afastamento)obj).Datai;
            parms[7].Value = ((Modelo.Afastamento)obj).Dataf;
            if (((Modelo.Afastamento)obj).IdFuncionario > 0)
            {
                parms[8].Value = ((Modelo.Afastamento)obj).IdFuncionario;
            }
            if (((Modelo.Afastamento)obj).IdEmpresa > 0)
            {
                parms[9].Value = ((Modelo.Afastamento)obj).IdEmpresa;
            }
            if (((Modelo.Afastamento)obj).IdDepartamento > 0)
            {
                parms[10].Value = ((Modelo.Afastamento)obj).IdDepartamento;
            }
            parms[11].Value = ((Modelo.Afastamento)obj).Horai;
            parms[12].Value = ((Modelo.Afastamento)obj).Horaf;
            parms[13].Value = ((Modelo.Afastamento)obj).Parcial;
            parms[14].Value = ((Modelo.Afastamento)obj).SemCalculo;
            parms[15].Value = ((Modelo.Afastamento)obj).Incdata;
            parms[16].Value = ((Modelo.Afastamento)obj).Inchora;
            parms[17].Value = ((Modelo.Afastamento)obj).Incusuario;
            parms[18].Value = ((Modelo.Afastamento)obj).Altdata;
            parms[19].Value = ((Modelo.Afastamento)obj).Althora;
            parms[20].Value = ((Modelo.Afastamento)obj).Altusuario;
            parms[21].Value = ((Modelo.Afastamento)obj).IdContrato;
            parms[22].Value = ((Modelo.Afastamento)obj).NomeContrato;
            parms[23].Value = ((Modelo.Afastamento)obj).bSuspensao;
            parms[24].Value = ((Modelo.Afastamento)obj).IdIntegracao;
            parms[25].Value = ((Modelo.Afastamento)obj).IdLancamentoLoteFuncionario;
            parms[26].Value = ((Modelo.Afastamento)obj).Observacao;
            parms[27].Value = ((Modelo.Afastamento)obj).SemAbono;
        }

        public Modelo.Afastamento LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            try
            {
                SetInstance(dr, objAfastamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objAfastamento;
        }

        public DataTable GetPorAfastamentoRel(DateTime pDataInicial, DateTime pDataFinal, string pEmpresas, string pDepartamentos, string pFuncionarios, int pTipo)
        {
            SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime)
                };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            DataTable dt = new DataTable();

            string aux = "";

            switch (pTipo)
            {
                case 0: aux = @SELECTRELE + " AND afastamento.idempresa  IN " + pEmpresas; break;
                case 1: aux = @SELECTRELD + " AND afastamento.iddepartamento IN " + pDepartamentos; break;
                case 2: aux = @SELECTRELF + " AND afastamento.idfuncionario IN " + pFuncionarios; break;
                default: break;
            }

            aux += " AND afastamento.datai >= @datai AND isnull(afastamento.dataf, '9999-12-31') <= @dataf";

            switch (pTipo)
            {
                case 0: aux += " ORDER BY empresa.nome, datai "; break;
                case 1: aux += " ORDER BY departamento.descricao, datai "; break;
                case 2: aux += " ORDER BY funcionario.nome, datai "; break;
                default: break;
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetAfastamentoPorOcorrenciaRel(string pEmpresas, string pDepartamentos, string pFuncionarios, int pTipo, int pIdOcorrencia)
        {
            SqlParameter[] parms = new SqlParameter[1]
                {
                    new SqlParameter("@idocorrencia", SqlDbType.Int)
                };

            parms[0].Value = pIdOcorrencia;

            DataTable dt = new DataTable();

            string aux = "";

            switch (pTipo)
            {
                case 0: aux = @SELECTRELE + " AND afastamento.idempresa  IN " + pEmpresas; break;
                case 1: aux = @SELECTRELD + " AND afastamento.iddepartamento IN " + pDepartamentos; break;
                case 2: aux = @SELECTRELF + " AND afastamento.idfuncionario IN " + pFuncionarios; break;
                default: break;
            }

            if (pIdOcorrencia > 0)
                aux += " AND afastamento.idocorrencia = @idocorrencia ";
            switch (pTipo)
            {
                case 0: aux += " ORDER BY LOWER(empresa.nome), ocorrencia, datai"; break;
                case 1: aux += " ORDER BY LOWER(departamento.descricao), ocorrencia, datai"; break;
                case 2: aux += " ORDER BY LOWER(funcionario.nome),ocorrencia, datai"; break;
                default: break;
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        private SqlDataReader getPeriodoFuncionario(int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int, 4)
            };
            parms[0].Value = pIdFuncionario;

            string aux = SELECTALLLIST + @" WHERE tipo = 0 and idfuncionario = @idfuncionario ORDER BY id";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        private SqlDataReader getPeriodoDepartemento(int pIdEmpresa, int pIdDepartamento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@idempresa", SqlDbType.Int, 4)
                , new SqlParameter("@iddepartamento", SqlDbType.Int, 4)
            };
            parms[0].Value = pIdEmpresa;
            parms[1].Value = pIdDepartamento;

            string aux = SELECTALLLIST + @" WHERE tipo = 1 and idempresa = @idempresa and iddepartamento = @iddepartamento ORDER BY id";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        private SqlDataReader getPeriodoEmpresa(int pIdEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@idempresa", SqlDbType.Int, 4)
            };
            parms[0].Value = pIdEmpresa;

            string aux = SELECTALLLIST + @" WHERE tipo = 2 and idempresa = @idempresa ORDER BY id";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        public List<Modelo.Afastamento> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datai", SqlDbType.DateTime)
                ,new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string aux = SELECTALLLIST + " WHERE ((@datai >= datai AND @datai <= isnull(dataf, '9999-12-31')) " +
                              " OR (@dataf >= datai AND @dataf <= isnull(dataf, '9999-12-31')) " +
                              " OR (@datai <= datai AND @dataf >= isnull(dataf, '9999-12-31'))) " +
                              " ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Afastamento> lista = new List<Modelo.Afastamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Afastamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Afastamento>>(dr);
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

        public List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(int idFuncionario, DateTime pDataInicial, DateTime? pDataFinal, bool apenasFerias)
        {
            return GetAfastamentoFuncionarioPeriodo(new List<int> { idFuncionario}, pDataInicial, pDataFinal, apenasFerias);
        }

        public List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(List<int> idsFuncionarios, DateTime pDataInicial, DateTime? pDataFinal, bool apenasFerias)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dtInicioPeriodo", SqlDbType.DateTime)
                ,new SqlParameter("@dtFimPeriodo", SqlDbType.DateTime)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string _listIdsFuncionarios = string.Join(",",idsFuncionarios);
            string sql = SELECTALLLIST;

            sql += string.Format(@"where afastamento.id in (
                        select
	                        a.id
                        from funcionario fun
	                        left join contratofuncionario cf on cf.idfuncionario  = fun.id
	                        inner join afastamento a on a.idfuncionario = fun.id or a.iddepartamento = fun.iddepartamento or a.idempresa = fun.idempresa or a.idcontrato = cf.id
	                        inner join ocorrencia oc on oc.id = a.idocorrencia
	                        left join dbo.departamento dep on dep.id = fun.iddepartamento
	                        left join dbo.empresa emp on emp.id = fun.idempresa
                        where 
	                        fun.id in ({0})
	                        and (
			                        (a.datai between @dtInicioPeriodo and @dtFimPeriodo) or
			                        (isnull(a.dataf, '9999-12-31') between @dtInicioPeriodo and @dtFimPeriodo) or
			                        (@dtInicioPeriodo between a.datai and isnull(a.dataf, '9999-12-31')) or
			                        (@dtFimPeriodo between a.datai and isnull(a.dataf, '9999-12-31')) )
                )", _listIdsFuncionarios);

            if (apenasFerias)
            {
                sql += " and ocorrencia.OcorrenciaFerias = 1 ";
            }



            List<Modelo.Afastamento> ret = new List<Modelo.Afastamento>();

            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms))
            {
                while (dr.Read())
                {
                    Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
                    AtribuiCampos(dr, objAfastamento);
                    ret.Add(objAfastamento);
                }
            }
            return ret;
        }

        public List<Modelo.Afastamento> GetParaExportacaoFolha(DateTime dataI, DateTime dataF, string idsOcorrencias, bool considerarAbsenteismo, List<int> IdsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datai", SqlDbType.DateTime)
                ,new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = dataI;
            parms[1].Value = dataF;

            string where = null;
            if (!String.IsNullOrEmpty(idsOcorrencias))
                where = "ocorrencia.id in (" + idsOcorrencias + ")";
            if (considerarAbsenteismo)
            {
                where = (where != null) ? where + " OR " : String.Empty;
                where += "ocorrencia.absenteismo = 1";
            }

            string aux = @"
                SELECT  afastamento.*, 
	                case when afastamento.tipo = 0 then 'Individual' when afastamento.tipo = 1 then 'Departamento' when afastamento.tipo = 2 then 'Empresa' when afastamento.tipo = 3 then 'Contrato' else '' end as TipoAfastamentoStr,
	                case
			                when tipo = 3 then (select 'Contrato - ' + emp.nome + ' - ' + ct.codigocontrato from contrato ct left join empresa emp on emp.id = ct.idempresa where ct.id = afastamento.idcontrato)
			                when tipo = 2 then (SELECT empresa.nome FROM empresa WHERE empresa.id = afastamento.idempresa) 
			                when tipo = 1 then (SELECT departamento.descricao FROM departamento WHERE departamento.id = afastamento.iddepartamento) 
			                when tipo = 0 then (SELECT funcionario.nome FROM funcionario WHERE funcionario.id = afastamento.idfuncionario) end AS Nome,
	                (select descricao from ocorrencia where ocorrencia.id = afastamento.idocorrencia) as ocorrencia,
	                coalesce(emp.id, dep.idempresa, fun.idempresa, 0) as idempresafiltro
	                ,coalesce(CONVERT(varchar, ct.codigo) + ' | ' + convert(varchar, e.codigo) + ' | ' + e.nome, '') as NomeContrato
                    ,ocorrencia.codigo codigoOcorrencia, ocorrencia.descricao descricaoOcorrencia 
	                FROM afastamento
                left join empresa emp on afastamento.idempresa = emp.id
                left join departamento dep on afastamento.iddepartamento = dep.id
                left join contrato ct on ct.id = afastamento.idcontrato
                left join funcionario fun on afastamento.idfuncionario = fun.id
                left join empresa e on ct.idempresa = e.id
                inner join ocorrencia ON ocorrencia.Id = afastamento.Idocorrencia
                 WHERE ((@datai >= datai AND @datai <= isnull(dataf, '9999-12-31')) 
                 OR (@dataf >= datai AND @dataf <= isnull(dataf, '9999-12-31')) 
                 OR (@datai <= datai AND @dataf >= isnull(dataf, '9999-12-31'))) 
                ";
            if (where != null)
                aux += " AND (" + where + " )";
            if (IdsFuncs.Count() > 0)
            {
                aux += " AND fun.id IN (" + String.Join(",", IdsFuncs) +")";
            }
            aux += " ORDER BY id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Afastamento> ret = new List<Modelo.Afastamento>();

            Modelo.Afastamento objAfastamento = null;
            while (dr.Read())
            {
                objAfastamento = new Modelo.Afastamento();
                AtribuiCampos(dr, objAfastamento);
                ret.Add(objAfastamento);
            }
            dr.Close();
            dr.Dispose();

            return ret;
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ";
            }
            return "";
        }

        private bool VerificaPeriodo(DateTime pData, SqlDataReader dr)
        {
            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            while (dr.Read())
            {
                AtribuiCampos(dr, objAfastamento);

                if (pData >= objAfastamento.Datai && pData <= (objAfastamento.Dataf == null ? DateTime.MaxValue : objAfastamento.Dataf))
                {
                    return true;
                }
            }
            dr.Close();
            dr.Dispose();
            return false;
        }

        public bool PossuiRegistro(DateTime pData, int pIdEmpresa, int pIdDepartamento, int pIdFuncionario)
        {
            SqlDataReader dr;

            //Verifica e possui registro por Funcionario
            dr = this.getPeriodoFuncionario(pIdFuncionario);
            if (VerificaPeriodo(pData, dr))
            {
                return true;
            }

            //Verifica e possui registro por Departamento
            dr = this.getPeriodoDepartemento(pIdEmpresa, pIdDepartamento);
            if (VerificaPeriodo(pData, dr))
            {
                return true;
            }

            //Verifica e possui registro por Empresa
            dr = this.getPeriodoEmpresa(pIdEmpresa);
            if (VerificaPeriodo(pData, dr))
            {
                return true;
            }

            return false;
        }

        public int VerificaExiste(int pId, DateTime? pDataInicial, DateTime? pDataFinal, int pTipo, int pIdentificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),
                new SqlParameter("@tipo", SqlDbType.Int),
                new SqlParameter("@identificacao", SqlDbType.Int),
                new SqlParameter("@id", SqlDbType.Int)
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;
            parms[2].Value = pTipo;
            parms[3].Value = pIdentificacao;
            parms[4].Value = pId;

            int qt = (int)db.ExecuteScalar(CommandType.Text, VERIFICA, parms);

            return qt;
        }

        public Modelo.Afastamento LoadParaManutencao(DateTime pData, int pIdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@data", SqlDbType.DateTime),
                new SqlParameter("@idfuncionario", SqlDbType.Int)
            };

            parms[0].Value = pData;
            parms[1].Value = pIdFuncionario;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTMAR, parms);
            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            try
            {
                SetInstance(dr, objAfastamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objAfastamento;
        }

        public void Incluir(List<Modelo.Afastamento> afastamentos)
        {
            SqlParameter[] parms = GetParameters();
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd;
                        foreach (Modelo.Afastamento afast in afastamentos)
                        {
                            SetDadosInc(afast);
                            SetParameters(parms, afast);
                            cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
                            afast.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw (ex);
                    }
                }
            }
        }

        private string PermissaoUsuarioContrato(Modelo.Cw_Usuario usuarioLogado)
        {
            string sql = SELECTALLLIST + PermissaoUsuarioFuncionario(UsuarioLogado, SELECTALLLIST, "coalesce(emp.id, dep.idempresa, fun.idempresa, 0)", "fun.id", "Where");
            if (usuarioLogado != null)
            {
                if (usuarioLogado.UtilizaControleContratos)
                {
                    sql += @" or ct.id in (select cont.id from contratousuario cu
                                           join contrato cont on cu.idcontrato = cont.id
                                           where cu.idcwusuario = " + usuarioLogado.Id + ")";
                }
            }
            return sql;
        }

        public List<Modelo.Afastamento> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string sql = PermissaoUsuarioContrato(UsuarioLogado);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Afastamento> lista = new List<Modelo.Afastamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Afastamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Afastamento>>(dr);
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

        public IList<Modelo.Proxy.pxyAbonosPorMarcacao> GetAbonosPorMarcacoes(IList<int> idFuncionarios, DateTime dataIni, DateTime dataFin)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@idFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@dataIni", SqlDbType.DateTime),
                    new SqlParameter("@dataFin", SqlDbType.DateTime)
            };

            parms[0].Value = String.Join(",", idFuncionarios.Select(s => s.ToString()));
            parms[1].Value = dataIni;
            parms[2].Value = dataFin;

            string sql = @" SELECT t.*,
	                                dbo.FN_CONVMIN(dbo.FN_CONVHORA(t.AbonoDiurno) + dbo.FN_CONVHORA(t.AbonoNoturno)) AbonoTotal
                              FROM (
                            SELECT
		                            a.id
		                            ,a.codigo
		                            ,a.descricao
		                            ,a.idocorrencia
		                            ,a.tipo
		                            ,a.datai
		                            ,a.dataf
		                            ,a.idfuncionario
		                            ,a.idempresa
		                            ,a.iddepartamento
		                            ,a.abonado
		                            ,a.parcial
		                            ,a.semcalculo
		                            ,a.bSuspensao
		                            ,CASE WHEN a.abonado = 1 and a.parcial = 0 THEN m.horastrabalhadas
			                              WHEN a.abonado = 1 and a.parcial = 1 THEN a.horai
			                            ELSE NULL
		                            END AbonoDiurno
		                            ,CASE WHEN a.abonado = 1 and a.parcial = 0 THEN m.horastrabalhadasnoturnas
			                              WHEN a.abonado = 1 and a.parcial = 1 THEN a.horaf
			                            ELSE NULL
		                            END AbonoNoturno
		                            , m.id idMarcacao
		                            , m.data DataMarcacao
		                            , o.descricao descricaoOcorrencia
                                    , a.Observacao
	                            FROM afastamento a
	                            INNER JOIN marcacao_view m ON m.data BETWEEN a.datai AND isnull(a.dataf, '9999-12-31')
	                            LEFT JOIN funcionario f ON f.id = m.idfuncionario
	                            LEFT JOIN ocorrencia o ON o.id = a.idocorrencia
	                            WHERE ((a.tipo = 0 AND a.idfuncionario = f.id) OR 
		                               (a.tipo = 1 AND a.iddepartamento = f.iddepartamento) OR 
		                               (a.tipo = 2 AND a.idempresa = f.idempresa))
	                              AND m.data BETWEEN @dataIni AND @dataFin
	                              AND f.id in (select * FROM F_clausulain(@idFuncionarios))
	                              ) t";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            IList<Modelo.Proxy.pxyAbonosPorMarcacao> lista = new List<Modelo.Proxy.pxyAbonosPorMarcacao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyAbonosPorMarcacao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyAbonosPorMarcacao>>(dr);
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

        public int? GetIdPorIdIntegracao(string idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = @"select * 
                              from (
	                            select a.id, a.idintegracao, ISNULL(isnull(f.idempresa, d.idempresa), a.idempresa) idEmpresa
	                              from afastamento a
	                              left join funcionario f on f.id = a.idfuncionario
	                              left join departamento d on d.id = a.iddepartamento
		                            ) t where idIntegracao = '" + idIntegracao + "'";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "t.idEmpresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));
            return Id;
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoAfastamento(int idAfastamento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = idAfastamento;

            string sql = @"select top(3) fp.id, fp.codigo, fp.dataFechamento, fp.descricao, fp.observacao
                              from afastamento a
                             inner join fechamentoPonto fp on fp.datafechamento >= a.datai
                             inner join fechamentoPontoFuncionario fpf on fp.id = fpf.idfechamentoponto
                             inner join funcionario f on fpf.idfuncionario = f.id
                              left join Contrato co on co.id = a.idcontrato
                              left join ContratoFuncionario cf on co.id = cf.idcontrato
                             where ((a.idfuncionario = f.id) or 
                                    (a.iddepartamento = f.iddepartamento and a.idempresa = f.idempresa) or
	                                (a.idempresa = f.idempresa and a.iddepartamento is null) or
		                            (cf.idfuncionario = f.id and a.idContrato is not null))
                               and a.id = @id
                             Group by fp.id, fp.codigo, fp.dataFechamento, fp.descricao, fp.observacao
                             order by fp.dataFechamento desc";

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

        public int? GetIdAfastamentoPorIdMarcacao(int IdMarcacao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = @"SELECT a.id FROM afastamento a
                            INNER JOIN marcacao m ON a.idfuncionario = m.idfuncionario AND a.datai = m.data AND isnull(a.dataf, '9999-12-31') = m.data
                            WHERE m.id = " + IdMarcacao;
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));
            return Id;
        }

        #region Sql Relatorio
        public DataTable GetParaRelatorioAbono(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento, string pIdsOcorrenciasSelecionados)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@modoOrdenacao", SqlDbType.Int),
                    new SqlParameter("@idsOcorrencias", SqlDbType.VarChar)
            };

            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = pModoOrdenacao;
            parms[3].Value = pIdsOcorrenciasSelecionados;

            string SQL = " select * "
                          + ", dbo.FN_CONVMIN(d.abonoParcialMin) abonoParcial "
                          + ", dbo.FN_CONVMIN(d.abonoTotalMin) abonoTotal "
                     + " from ( "
                              + " select f.nome funcionario "
                            + ", f.dscodigo "
                            + ", a.idocorrencia "
                            + ", o.descricao ocorrencia "
                            + ", d.descricao departamento "
                            + ", e.nome empresa "
                            + ", case when ISNULL(e.cnpj, '') <> '' then e.cnpj else e.cpf end AS cnpj_cpf "
                            + ", a.datai inicioabono "
                            + ", a.dataf fimabono "
                            + ", m.data dtmarcacao "
                            + ", m.dia "
                            + ", a.abonado "
                            + ", a.parcial "
                            + ", case when a.parcial = 0 then dbo.FN_CONVHORA(m.horastrabalhadas) + dbo.FN_CONVHORA(m.horastrabalhadasnoturnas) else '0' end abonototalmin "
                            + ", case when a.parcial = 1 then dbo.FN_CONVHORA(a.horai) + dbo.FN_CONVHORA(a.horaf) else '0' end abonoparcialmin "
                          + " from afastamento a "
                          + " inner join marcacao_view m on m.data between a.datai and a.dataf "
                          + " left join funcionario f on f.id = m.idfuncionario "
                          + " left join departamento d on f.iddepartamento = d.id "
                          + " left join empresa e on e.id = f.idempresa "
                          + " left join ocorrencia o on o.id = a.idocorrencia "
                          + " where ((a.tipo = 0 and a.idfuncionario = f.id) or "
                          + " (a.tipo = 1 and a.iddepartamento = f.iddepartamento) or "
                          + " (a.tipo = 2 and a.idempresa = f.idempresa)) and a.abonado = 1 "
                          + " and a.datai >= @datai AND m.data <= @dataf "
                          + " and o.id in (select * from F_RetornaTabelaLista (@idsOcorrencias, ',')) ";

            switch (pTipo)
            {
                //Empresa
                case 0:
                    SQL += "AND f.idempresa IN " + pIdentificacao;
                    break;
                //Departamento
                case 1:
                    SQL += "AND f.iddepartamento IN " + pIdentificacao;
                    break;
                //Individual
                case 2:
                    SQL += "AND f.id IN " + pIdentificacao;
                    break;
            }

            SQL += ") D ";

            String orderBy = "ORDER BY ";
            if (pAgrupaDepartamento == 1)
                orderBy += "d.departamento, ";

            orderBy += "d.funcionario, d.dtMarcacao";
            SQL += orderBy;

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<PxyRelAfastamento> GetRelatorioAfastamentoFolha(List<int> idsFuncs, DateTime pDataI, DateTime pDataF, Int16 absenteismo, bool considerarAbonado, bool considerarParcial, bool considerarSemCalculo, bool considerarSuspensao, bool considerarSemAbono)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@dtInicioPeriodo", SqlDbType.DateTime),
                    new SqlParameter("@dtFimPeriodo", SqlDbType.DateTime),
                    new SqlParameter("@tiposAbono", SqlDbType.VarChar),
                    new SqlParameter("@Absenteismo", SqlDbType.SmallInt)
            };

            List<int> tiposAbono = new List<int>();
            if (considerarParcial)
            {
                tiposAbono.Add(2);
            }
            else if (considerarAbonado)
            {
                tiposAbono.Add(1);
            }
            else if (considerarSemCalculo)
            {
                tiposAbono.Add(3);
            }
            else if (considerarSuspensao)
            {
                tiposAbono.Add(4);
            }
            else if (considerarSemAbono)
            {
                tiposAbono.Add(5);
            }

            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = String.Join(",", tiposAbono);
            parms[3].Value = absenteismo;
            string _listIdsFuncionarios = String.Join(",", idsFuncs);

            string SQL = string.Format(@" select p.[data] AfastamentoData, t.*
                                            from FN_DatasPeriodo(@dtInicioPeriodo, @dtFimPeriodo) p
                                            inner join (
                                            select *
                                            from (
                                                select *
                                                from (
                                                        select
                                                            fun.id FuncionarioID,
                                                            fun.dscodigo FuncionarioCodigo,
                                                            fun.nome FuncionarioNome,
                                                            fun.CPF FuncionarioCpf,
                                                            fun.matricula FuncionarioMatricula,
                                                            a.id AfastamentoId,
                                                            a.codigo AfastamentoCodigo,
                                                            a.descricao AfastamentoDescricao,
                                                            a.abonado AfastamentoAbonado,
                                                            a.parcial AfastamentoParcial,
                                                            a.semcalculo AfastamentoSemCalculo,
                                                            a.bSuspensao AfastamentoSuspensao,
                                                            a.SemAbono AfastamentoSemAbono,
                                                            a.datai AfastamentoDataInicio,
                                                            a.dataf AfastamentoDataFim,
                                                            a.horai AfastamentoAbonoParcialDiurno,
                                                            dbo.FN_CONVHORA(a.horai) AfastamentoAbonoParcialDiurnoMin,
                                                            a.horaf AfastamentoAbonoParcialNoturno,
                                                            dbo.FN_CONVHORA(a.horaf) AfastamentoAbonoParcialNoturnoMin,
                                                            a.idocorrencia OcorrenciaID,
                                                            a.idIntegracao AfastamentoIdIntegrador,
                                                            a.Observacao AfastamentoObservacao,
                                                            o.descricao OcorrenciaDescricao,
                                                            o.absenteismo OcorrenciaAbsenteismo,
                                                            case WHEN a.parcial = 1 THEN 2
                                                                    WHEN a.abonado = 1 THEN 1
                                                                    WHEN a.SemCalculo = 1 THEN 3
                                                                    WHEN a.bSuspensao = 1 THEN 4
                                                                    WHEN a.SemAbono = 1 THEN 5
                                                                    else 0
                                                            END TipoAbono
                                                        from funcionario fun
                                                        inner join afastamento a on a.idfuncionario = fun.id or a.iddepartamento = fun.iddepartamento or a.idempresa = fun.idempresa
                                                        INNER JOIN ocorrencia o on a.idocorrencia = o.id
                                                        where (@Absenteismo = 2 or o.absenteismo = @Absenteismo)
                                                            and a.id in (
                                                                select
	                                                                a.id
                                                                from funcionario fun
	                                                                left join contratofuncionario cf on cf.idfuncionario  = fun.id
	                                                                inner join afastamento a on a.idfuncionario = fun.id or a.iddepartamento = fun.iddepartamento or a.idempresa = fun.idempresa or a.idcontrato = cf.id
	                                                                inner join ocorrencia oc on oc.id = a.idocorrencia
	                                                                left join dbo.departamento dep on dep.id = fun.iddepartamento
	                                                                left join dbo.empresa emp on emp.id = fun.idempresa
                                                                where 
	                                                                fun.id in ({0})
	                                                                and (
			                                                                (a.datai between @dtInicioPeriodo and @dtFimPeriodo) or
			                                                                (isnull(a.dataf, '9999-12-31') between @dtInicioPeriodo and @dtFimPeriodo) or
			                                                                (@dtInicioPeriodo between a.datai and isnull(a.dataf, '9999-12-31')) or
			                                                                (@dtFimPeriodo between a.datai and isnull(a.dataf, '9999-12-31')) )
                                                        )
                                                        ) t WHERE t.TipoAbono in (select * from dbo.F_ClausulaIn(@tiposAbono))
                                            ) x
                                            ) t on p.[data] BETWEEN t.AfastamentoDataInicio and t.AfastamentoDataFim
                                            order by FuncionarioID, p.[data] ", _listIdsFuncionarios);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);
            List<PxyRelAfastamento> lista = new List<PxyRelAfastamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, PxyRelAfastamento>();
                lista = AutoMapper.Mapper.Map<List<PxyRelAfastamento>>(dr);
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
