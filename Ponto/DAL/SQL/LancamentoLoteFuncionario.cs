using AutoMapper;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LancamentoLoteFuncionario : DAL.SQL.DALBase, DAL.ILancamentoLoteFuncionario
    {

        public LancamentoLoteFuncionario(DataBase database)
        {
            db = database;
            TABELA = "LancamentoLoteFuncionario";

            SELECTPID = @"   SELECT * FROM LancamentoLoteFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM LancamentoLoteFuncionario";

            INSERT = @"  INSERT INTO LancamentoLoteFuncionario
							(codigo, incdata, inchora, incusuario, idLancamentoLote, idFuncionario, Efetivado, UltimaAcao, DescricaoErro)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idLancamentoLote, @idFuncionario, @Efetivado, @UltimaAcao, @DescricaoErro)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LancamentoLoteFuncionario SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idLancamentoLote = @idLancamentoLote,
                                idFuncionario = @idFuncionario,
                                Efetivado = @Efetivado, 
                                UltimaAcao = @UltimaAcao, 
                                DescricaoErro = @DescricaoErro
						WHERE id = @id";

            DELETE = @"  DELETE FROM LancamentoLoteFuncionario WHERE id = @id and efetivado = 1";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LancamentoLoteFuncionario";

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

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {                
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteFuncionario>();
                    obj = Mapper.Map<Modelo.LancamentoLoteFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteFuncionario();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.LancamentoLoteFuncionario obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteFuncionario>();
                    obj = Mapper.Map<Modelo.LancamentoLoteFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteFuncionario();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.LancamentoLoteFuncionario> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoLoteFuncionario>>();
                    obj = Mapper.Map<List<Modelo.LancamentoLoteFuncionario>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.LancamentoLoteFuncionario>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.LancamentoLoteFuncionario>();
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
                new SqlParameter ("@IdLancamentoLote", SqlDbType.Int),
				new SqlParameter ("@IdFuncionario", SqlDbType.Int),
                new SqlParameter ("@Efetivado", SqlDbType.Bit),
                new SqlParameter ("@UltimaAcao", SqlDbType.Int),
                new SqlParameter ("@DescricaoErro", SqlDbType.VarChar)
                
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
            parms[1].Value = ((Modelo.LancamentoLoteFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.LancamentoLoteFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.LancamentoLoteFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.LancamentoLoteFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.LancamentoLoteFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.LancamentoLoteFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.LancamentoLoteFuncionario)obj).Altusuario;
            parms[8].Value = ((Modelo.LancamentoLoteFuncionario)obj).IdLancamentoLote;
            parms[9].Value = ((Modelo.LancamentoLoteFuncionario)obj).IdFuncionario;
            parms[10].Value = ((Modelo.LancamentoLoteFuncionario)obj).Efetivado;
            parms[11].Value = ((Modelo.LancamentoLoteFuncionario)obj).UltimaAcao;
            parms[12].Value = ((Modelo.LancamentoLoteFuncionario)obj).DescricaoErro;
        }

        public Modelo.LancamentoLoteFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoLoteFuncionario obj = new Modelo.LancamentoLoteFuncionario();
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

        public List<Modelo.LancamentoLoteFuncionario> GetAllList()
        {
            return GetListWhere("");
        }

        public List<Modelo.LancamentoLoteFuncionario> GetListWhere(string condicao)
        {
            List<Modelo.LancamentoLoteFuncionario> lista = new List<Modelo.LancamentoLoteFuncionario>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM LancamentoLoteFuncionario where 1 = 1 "+condicao, parms);
            SetaListaObjeto(dr, ref lista);
            return lista;
        }

        public List<Modelo.LancamentoLoteFuncionario> GetListNaoEfetivados(SqlTransaction trans, int idLote)
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = null;
            string sql = "SELECT * FROM LancamentoLoteFuncionario where idLancamentoLote = " + idLote + " and Efetivado = 0";
            dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, sql, parms);

            List<Modelo.LancamentoLoteFuncionario> lista = new List<Modelo.LancamentoLoteFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LancamentoLoteFuncionario>>(dr);
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

        public void ExcluirLoteIds(SqlTransaction trans, List<int> idsLancamentoLoteFuncionario)
        {
            if (idsLancamentoLoteFuncionario.Count() > 0)
            {
                SqlParameter[] parms = null;
                string strIDS = String.Join(",", idsLancamentoLoteFuncionario);

                SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"  DELETE FROM LancamentoLoteFuncionario WHERE id in (" + strIDS + ")", true, parms);   
            }
        }

        /// <summary>
        /// Exclui as folgas lançadas por lote de acordo com os funcionários, período e tipo lançamento passados por parâmetro
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idsFuncionarios"> Lista de Ids de Funcionários</param>
        /// <param name="dataInicial">Data Início</param>
        /// <param name="dataFinal">Data Fim</param>
        /// <param name="tpLancamento">Tipo do Lançamento</param>
        public void ExcluirFuncionariosDataTipo(SqlTransaction trans, List<int> idsFuncionarios, DateTime dataInicial, DateTime dataFinal, TipoLancamento tpLancamento)
        {
            SqlParameter[] parms = new SqlParameter[4]
            { 
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
                    new SqlParameter("@tipoLancamento", SqlDbType.Int)
            };
            parms[0].Value = string.Join(",", idsFuncionarios);
            parms[1].Value = dataInicial;
            parms[2].Value = dataFinal;
            parms[3].Value = (int)tpLancamento;

            string aux = @" delete from lancamentolotefuncionario 
                                     where id in (
		                                    select llf.id
		                                      from lancamentolotefuncionario llf
		                                     inner join lancamentolote ll on ll.id = llf.idLancamentoLote
		                                     where dataLancamento between @datainicial and @datafinal
		                                       and llf.idFuncionario in (select * from f_clausulaIn(@idsFuncionarios))
                                               and ll.tipoLancamento = @tipoLancamento
                                                 ) ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Seta os campos Efetivado = 0 e descricaoErro com a mensagem da última mudança de horário caso haja
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idLote">Id Lote a ser processado</param>
        public void ValidaAfastamentoPeriodoLote(SqlTransaction trans, int idLote)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idLote", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idLote);

            string aux = @" update lancamentolotefuncionario
                               set Efetivado = 0,
	                               descricaoErro = isnull(descricaoErro+'; Já existe um afastamento no período informado, afastamento de código ' + Convert(varchar, t.codigo) + ' período ' + Convert(varchar, t.datai, 103) + ' a ' + Convert(varchar, t.dataf, 103), 'Já existe um afastamento no período informado, afastamento de código ' + Convert(varchar, t.codigo) + ' período ' + Convert(varchar, t.datai, 103) + ' a ' + Convert(varchar, t.dataf, 103))
                              from lancamentolotefuncionario
                             inner join (
			                            select llf.idFuncionario, 
				                               a.codigo, 
				                               a.datai, 
				                               a.dataf, 
				                               case when a.tipo = 0 then 'Individual' when a.tipo = 1 then 'Departamento' when a.tipo = 2 then 'Empresa' when a.tipo = 3 then 'Contrato' else '' end as TipoAfastamentoStr
				                               , a.idLancamentoLoteFuncionario 
				                               , llf.id
			                              from LancamentoLote ll
			                             inner join LancamentoLotefuncionario llf on ll.id = llf.idlancamentolote
			                             inner join funcionario f on f.id = llf.idfuncionario
			                             inner join lancamentoloteafastamento lla on ll.id = lla.idlancamentolote
			                              left join afastamento as a on ( (lla.dataI between a.datai and a.dataf or lla.dataF between a.datai and a.dataf ) and
											                              ( ( a.idfuncionario = f.id) or 
												                            ( a.iddepartamento = f.iddepartamento and a.idempresa = f.idempresa) or 
												                            (a.iddepartamento is null and a.idempresa = f.idempresa))
											                            )
			                             where ll.id = @idLote
											                            ) t on t.idfuncionario = LancamentoLoteFuncionario.idfuncionario  and t.codigo is not null
                             where lancamentolotefuncionario.idlancamentolote = @idLote
                               and lancamentolotefuncionario.id != isnull(t.idlancamentolotefuncionario, 0) ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Seta os campos Efetivado = 0 e descricaoErro com a mensagem da última mudança de horário caso haja
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idLote">Id Lote a ser processado</param>
        public void ValidaMudancasPosterioresLote(SqlTransaction trans, int idLote)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idLote", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idLote);

            string aux = @" update lancamentolotefuncionario
                               set Efetivado = 0,
                                   descricaoErro = isnull(descricaoErro+'; Já existe uma mudança de horário igual ou posterior, data da última mudança encontrada = ' + Convert(varchar, ultMud.UltimaMudanca, 103), 'Já existe uma mudança de horário igual ou posterior a data desse lote, data da última mudança encontrada = ' + Convert(varchar, ultMud.UltimaMudanca, 103))
                              from lancamentolotefuncionario
                             inner join (
                                        select max(mh.data) UltimaMudanca, llf.id idLancamentoLoteFuncionario
                                          from lancamentolote l
                                         inner join lancamentolotefuncionario llf on l.id = llf.idlancamentolote
                                          left join mudancahorario mh on mh.idfuncionario = llf.idFuncionario and mh.data >= l.dataLancamento and  llf.id != mh.idLancamentoLoteFuncionario
                                         where tipolancamento = 2
                                           and l.id = @idLote
                                           and mh.data is not null
                                         group by llf.id
                                        ) ultMud on ultMud.idLancamentoLoteFuncionario = id ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Seta os campos Efetivado = 0 e descricaoErro com a mensagem da última mudança de horário caso haja
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="Lote">Objeto do Lote</param>
        public void ValidaMudancasConflitantes(SqlTransaction trans, Modelo.LancamentoLote Lote)
        {
            if (Lote.TipoLancamento == (int)TipoLancamento.MudancaHorario)
            {
                LancamentoLoteFuncionario dalFuncionarioLote = new LancamentoLoteFuncionario(db);
                Horario dalHorario = new Horario(db);
                IList<Modelo.LancamentoLoteFuncionario> lancamentoLoteFuncionario = Lote.LancamentoLoteFuncionarios;

                Funcionario dalFuncionario = new Funcionario(db);
                IList<Modelo.Funcionario> funcs = dalFuncionario.GetFuncionariosPorIds(String.Join(",", lancamentoLoteFuncionario.Select(s => s.IdFuncionario)));

                #region valida se dentro da mudança existe funcionário com dois registros, se houver barra a alteração pois não pode existir dois registros de emprego do mesmo funcionário com conflito de horários.
                List<string> lPis = funcs.Where(w => !String.IsNullOrEmpty(w.Pis)).Select(s => s.Pis).ToList();
                List<string> pisDuplicados = lPis.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key).ToList();
                List<Modelo.Funcionario> funcsDoisRegistros = dalFuncionario.GetAllPisDuplicados(lPis);
                if (pisDuplicados.Count() > 0)
                {
                    List<Modelo.Funcionario> funcsDupl = funcsDoisRegistros.Where(w => pisDuplicados.Contains(w.Pis)).ToList();
                    lancamentoLoteFuncionario.Where(w => funcsDupl.Select(s => s.Id).Contains(w.IdFuncionario)).ToList().ForEach(s => { s.Efetivado = false; s.DescricaoErro = "Horário em conflito com outro registro desse funcionário"; s.Acao = Modelo.Acao.Alterar; });
                }
                #endregion

                #region Valida se com a mudança de horário haverá conflito entre horários de outro registro de emprego do funcionário
                if (Lote.LancamentoLoteMudancaHorario != null)
                {
                    Modelo.Horario horarioRegistro1 = dalHorario.LoadObject(Lote.LancamentoLoteMudancaHorario.Idhorario);

                    List<Modelo.Funcionario> FuncsOutroRegistro = funcsDoisRegistros.Where(w => !funcs.Select(s => s.Id).Contains(w.Id)).ToList();
                    if (FuncsOutroRegistro.Count() > 0)
                    {
                        List<Modelo.Horario> horariosValidar = dalHorario.GetAllList(true, false, 2, "(" + String.Join(",", FuncsOutroRegistro.Select(s => s.Id)) + ")");
                        List<int> IdsHorariosComConflito = new List<int>();
                        MudancaHorario.ValidarHorariosEmConflito(horarioRegistro1, horariosValidar, out IdsHorariosComConflito);
                        if (IdsHorariosComConflito.Count() > 0)
                        {
                            List<string> pisConflito = FuncsOutroRegistro.Where(w => IdsHorariosComConflito.Contains(w.Idhorario)).Select(s => s.Pis).ToList();
                            List<Int32> idsConflito = funcs.Where(w => pisConflito.Contains(w.Pis)).Select(s => s.Id).ToList();
                            lancamentoLoteFuncionario.Where(w => idsConflito.Contains(w.IdFuncionario)).ToList().ForEach(s => { s.Efetivado = false; s.DescricaoErro = "Horário em conflito com outro registro desse funcionário"; s.Acao = Modelo.Acao.Alterar; });
                        }
                        lancamentoLoteFuncionario = lancamentoLoteFuncionario.Where(w => w.Acao == Modelo.Acao.Alterar).ToList();
                        if (lancamentoLoteFuncionario.Count() > 0)
                        {
                            foreach (Modelo.LancamentoLoteFuncionario llf in lancamentoLoteFuncionario)
                            {
                                Alterar(trans, llf);
                            }
                        }
                    }
                } 

            }
            
            #endregion
        }

        /// <summary>
        /// Seta os campos Efetivado = 0 e descricaoErro com a mensagem do último fechamento do ponto caso haja
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idLote">Id Lote a ser processado</param>
        public void ValidaFechamentoPontoLote(SqlTransaction trans, int idLote)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idLote", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idLote);

            string aux = @" update lancamentolotefuncionario
                               set Efetivado = 0,
                                   descricaoErro = isnull(descricaoErro+'; Ponto fechado, último fechamento em ' + Convert(varchar, ultFechamento.dtUltimoFechamento, 103), ' Ponto fechado, último fechamento em ' + Convert(varchar, ultFechamento.dtUltimoFechamento, 103))
                              from lancamentolotefuncionario
                             inner join (
                            select max(fp.dataFechamento) dtUltimoFechamento, llf.id idLancamentoLoteFuncionario
                              from lancamentolote
                             inner join lancamentolotefuncionario llf on lancamentolote.id = llf.idlancamentolote
                             inner join FechamentoPontoFuncionario fpf on fpf.idfuncionario = llf.idfuncionario
                             inner join fechamentoponto fp on fp.id = fpf.idfechamentoponto and fp.dataFechamento >= lancamentolote.datalancamento
                             where lancamentolote.id = @idLote
                             group by llf.id ) ultFechamento on ultFechamento.idLancamentoLoteFuncionario = id ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }


        /// <summary>
        /// Seta os campos Efetivado = 0 e descricaoErro com a mensagem do último fechamento do Banco de Horas caso haja
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idLote">Id Lote a ser processado</param>
        public void ValidaFechamentoBHLote(SqlTransaction trans, int idLote)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idLote", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idLote);

            string aux = @" update lancamentolotefuncionario
                               set Efetivado = 0,
                                   descricaoErro = isnull(descricaoErro+' ; Fechamento de Banco de Horas, último fechamento em ' + Convert(varchar, ultFechamento.dtUltimoFechamento, 103), ' Fechamento de Banco de Horas, último fechamento em ' + Convert(varchar, ultFechamento.dtUltimoFechamento, 103))
                              from lancamentolotefuncionario
                             inner join (
                            select max(fbh.data) dtUltimoFechamento, llf.id idLancamentoLoteFuncionario
                              from lancamentolote
                             inner join lancamentolotefuncionario llf on lancamentolote.id = llf.idlancamentolote
                             inner join marcacao m on m.idfuncionario = llf.idfuncionario and m.idfechamentobh is not null
                             inner join fechamentobh fbh on fbh.id = m.idfechamentobh
                             where lancamentolote.id = @idLote and fbh.data >= lancamentolote.dataLancamento
                             group by llf.id ) ultFechamento on ultFechamento.idLancamentoLoteFuncionario = id ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Seta os campos Efetivado = 0 e descricaoErro com a mensagem do último fechamento do Banco de Horas caso haja
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="idLote">Id Lote a ser processado</param>
        public void RemoveTratamentoErroDosFuncionariosLote(SqlTransaction trans, int idLote)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idLote", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idLote);

            string aux = @" update lancamentolotefuncionario
                               set Efetivado = 1,
	                               descricaoErro = null
                             where idLancamentoLote = @idLote ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void Excluir(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            this.ExcluirAux(trans, obj);
        }

    }
}
