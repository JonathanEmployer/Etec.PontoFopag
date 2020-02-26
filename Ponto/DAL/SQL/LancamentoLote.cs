using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class LancamentoLote : DAL.SQL.DALBase, DAL.ILancamentoLote
    {
        public LancamentoLote(DataBase database)
        {
            db = database;
            TABELA = "LancamentoLote";

            SELECTPID = @"   SELECT * FROM LancamentoLote WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM LancamentoLote";

            INSERT = @"  INSERT INTO LancamentoLote
							(codigo, incdata, inchora, incusuario, dataLancamento, descricao, observacao, tipoLancamento)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @dataLancamento, @Descricao, @observacao, @tipoLancamento)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LancamentoLote SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                dataLancamento = @dataLancamento,
                                descricao = @descricao, 
                                observacao = @observacao,
                                tipoLancamento = @tipoLancamento
						WHERE id = @id";

            DELETE = @"  DELETE FROM LancamentoLote WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LancamentoLote";

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
            AuxManutencao(trans, obj);
            if (((Modelo.LancamentoLote)obj).Acao == Modelo.Acao.Excluir)
            {
                base.ExcluirAux(trans, obj);
            }
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            //Se existir Funcionarios vinculados ao fechamento, adiciona o relacionamento e adiciona o id na marcacao;
            if (((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios != null || ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Count() > 0)
            {
                try
                {
                    DAL.SQL.LancamentoLoteFuncionario dalLancamentoLoteFuncionario = new DAL.SQL.LancamentoLoteFuncionario(new DataBase(db.ConnectionString));
                    dalLancamentoLoteFuncionario.UsuarioLogado = this.UsuarioLogado;
                    //inclui funcionarios no fechamento
                    foreach (Modelo.LancamentoLoteFuncionario fpf in ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Incluir))
                    {
                        fpf.IdLancamentoLote = ((Modelo.LancamentoLote)obj).Id;
                        fpf.UltimaAcao = (int)Modelo.Acao.Incluir;
                        dalLancamentoLoteFuncionario.Incluir(trans, fpf);
                    }

                    IncluirLancamentoLoteAfastamento(trans, obj);
                    // Para os lançamentos em lote de mudanca de horário e exclusões não é necessária essa exclusão, a exclusão que vai acontecer adiante é suficiente.
                    if (((Modelo.LancamentoLote)obj).TipoLancamento != (int)Modelo.TipoLancamento.MudancaHorario)
                    {
                        //Exclui os lançamentos que não foram efetivados anteriormente.
                        IList<Modelo.LancamentoLoteFuncionario> excluiNaoIncluidos = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && (x.UltimaAcao == (int)Modelo.Acao.Incluir && x.Efetivado == false)).ToList();
                        dalLancamentoLoteFuncionario.ExcluirLoteIds(trans, excluiNaoIncluidos.Select(x => x.Id).ToList());
                        //Retiro os que já foram excluídos e não tinham sido efetivados
                        ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => !excluiNaoIncluidos.Contains(x)).ToList(); 
                    }

                    //Valido os lançamentos que sobraram
                    ValidaLancamentos(trans, obj, dalLancamentoLoteFuncionario);

                    //Altera funcionarios no fechamento quando necessário
                    foreach (Modelo.LancamentoLoteFuncionario fpf in ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Alterar))
                    {
                        bool Alterar = false;
                        if (((Modelo.LancamentoLote)obj).DataLancamento != ((Modelo.LancamentoLote)obj).DataLancamentoAnt)
                        {
                            Alterar = true;
                        }
                        else if (((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario != null && ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario.Idhorario != ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario.Idhorario_ant)
                        {
                            Alterar = true;
                        }
                        else if (((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp != null && ((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp.Hora != ((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp.Hora_Ant)
                        {
                            Alterar = true;
                        }
                        if (((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento != null &&
                            (((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.IdOcorrencia != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.IdOcorrencia ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BAbonado != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BAbonado_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoDiurno != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoDiurno_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoNoturno != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoNoturno_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BParcial != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BParcial_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSemCalculo != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSemCalculo_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSuspensao != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSuspensao_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataI != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataI_Ant ||
                             ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataF != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataF_Ant
                            ))
                        {
                            Alterar = true;
                        }
                        else if (fpf.Efetivado != fpf.EfetivadoAnt)
                        {
                            Alterar = true;
                        }

                        if (Alterar)
                        {
                            // Se a inclusão não tinha sido efetivado, os registros filhos serão incluídos, portanto mantenho a ultima acao como incluir.
                            if (fpf.EfetivadoAnt == false && fpf.Efetivado != fpf.EfetivadoAnt && fpf.UltimaAcao == (int)Modelo.Acao.Incluir)
                                fpf.UltimaAcao = (int)Modelo.Acao.Incluir;
                            else
                                fpf.UltimaAcao = (int)Modelo.Acao.Alterar;
                            dalLancamentoLoteFuncionario.Alterar(trans, fpf);
                        }
                    }

                    ExcluirLancamentoLoteFuncionarioERelacionados(trans, obj, dalLancamentoLoteFuncionario);
                }
                catch (Exception e)
                {
                    throw new Exception ("Erro ao vincular os funcionários ao lote. Erro: "+e.Message);
                }

                switch (((Modelo.LancamentoLote)obj).TipoLancamento)
                {
                    case (int)Modelo.TipoLancamento.MudancaHorario:
                        TratarMudancaHorario(trans, obj);
                        break;
                    case (int)Modelo.TipoLancamento.Afastamento:
                        TratarAfastamentos(trans, obj);
                        break;
                    case (int)Modelo.TipoLancamento.Folga:
                        TratarFolgaMarcacao(trans, obj);
                        break;
                    case (int)Modelo.TipoLancamento.InclusaoBanco:
                        TratarInclusaoBanco(trans, obj);
                        break;
                    case (int)Modelo.TipoLancamento.BilhetesImp:
                        TratarBilhetesImp(trans, obj);
                        break;
                    default:
                        break;
                }
                
            }
        }

        private void IncluirLancamentoLoteAfastamento(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento != null && ((Modelo.LancamentoLote)obj).Acao != Modelo.Acao.Excluir)
            {
                DAL.SQL.LancamentoLoteAfastamento dalLancamentoLoteAfastamento = new DAL.SQL.LancamentoLoteAfastamento(new DataBase(db.ConnectionString));
                dalLancamentoLoteAfastamento.UsuarioLogado = UsuarioLogado;
                try
                {
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.Id > 0)
                    {
                        dalLancamentoLoteAfastamento.Alterar(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento);
                    }
                    else
                    {
                        ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.IdLancamentoLote = ((Modelo.LancamentoLote)obj).Id;
                        ((Modelo.LancamentoLote)obj).Codigo = dalLancamentoLoteAfastamento.MaxCodigo();
                        dalLancamentoLoteAfastamento.Incluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao gerar a mudança de horário. Detalhes: " + e.Message);
                }
            }
        }

        private void TratarAfastamentos(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento != null && ((Modelo.LancamentoLote)obj).Acao != Modelo.Acao.Excluir)
            {
                DAL.SQL.LancamentoLoteAfastamento dalLancamentoLoteAfastamento = new DAL.SQL.LancamentoLoteAfastamento(new DataBase(db.ConnectionString));
                dalLancamentoLoteAfastamento.UsuarioLogado = UsuarioLogado;
                List<int> lFuncAdd = new List<int>();
                try
                {
                    lFuncAdd = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Incluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncAdd.Count() > 0)
                    {
                        dalLancamentoLoteAfastamento.IncluirAfastamentoLote(trans, lFuncAdd);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao adicionar a(s) mudança(s) de horário. Detalhes: " + e.Message);
                }

                try
                {
                    List<Modelo.LancamentoLoteFuncionario> lFuncAlt = new List<Modelo.LancamentoLoteFuncionario>();
                    lFuncAlt = ListaFuncionariosAfastamentoAlterado(obj);
                    if (lFuncAlt.Count() > 0)
                    {
                        dalLancamentoLoteAfastamento.UpdateAfastamentoLote(trans, lFuncAlt.Select(s => s.Id).ToList());
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao alterar a(s) mudança(s) de horário. Detalhes: " + e.Message);
                }
            }
        }

        public static List<Modelo.LancamentoLoteFuncionario> ListaFuncionariosAfastamentoAlterado(Modelo.ModeloBase obj)
        {
            List<Modelo.LancamentoLoteFuncionario> llfAlt = new List<Modelo.LancamentoLoteFuncionario>();
            if (((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Alterar && x.Efetivado == true).Count() > 0 &&
                (((Modelo.LancamentoLote)obj).DataLancamento != ((Modelo.LancamentoLote)obj).DataLancamentoAnt ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.IdOcorrencia != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.IdOcorrencia ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BAbonado != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BAbonado_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoDiurno != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoDiurno_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoNoturno != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.AbonoNoturno_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BParcial != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BParcial_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSemCalculo != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSemCalculo_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSuspensao != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.BSuspensao_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataI != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataI_Ant ||
                 ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataF != ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento.DataF_Ant
                ))
            {
                llfAlt = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Alterar).ToList();
            }
            return llfAlt;
        }

        private static void ValidaLancamentos(SqlTransaction trans, Modelo.ModeloBase obj, DAL.SQL.LancamentoLoteFuncionario dalLancamentoLoteFuncionario)
        {
            int idLote = ((Modelo.LancamentoLote)obj).Id;
            dalLancamentoLoteFuncionario.RemoveTratamentoErroDosFuncionariosLote(trans, idLote);
            ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.ToList().ForEach(x => { x.Efetivado = true; x.DescricaoErro = ""; });
            dalLancamentoLoteFuncionario.ValidaFechamentoPontoLote(trans, idLote);
            dalLancamentoLoteFuncionario.ValidaFechamentoBHLote(trans, idLote);
            if (((Modelo.LancamentoLote)obj).Acao != Modelo.Acao.Excluir)
            {
                dalLancamentoLoteFuncionario.ValidaMudancasPosterioresLote(trans, ((Modelo.LancamentoLote)obj).Id);
                dalLancamentoLoteFuncionario.ValidaMudancasConflitantes(trans, (Modelo.LancamentoLote)obj); 
            }
            dalLancamentoLoteFuncionario.ValidaAfastamentoPeriodoLote(trans, ((Modelo.LancamentoLote)obj).Id);

            List<Modelo.LancamentoLoteFuncionario> llFuncNaoEfetivados = dalLancamentoLoteFuncionario.GetListNaoEfetivados(trans, idLote);
            foreach (var fNaoEfetivados in llFuncNaoEfetivados)
            {
                foreach (Modelo.LancamentoLoteFuncionario item in ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Id == fNaoEfetivados.Id))
	            {
                    item.Efetivado = fNaoEfetivados.Efetivado;
                    item.DescricaoErro = fNaoEfetivados.DescricaoErro;
	            }
                
            }
            int qtdFuncionarios = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Count();
            int qtdFuncionariosExcluir = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(w => w.Efetivado == true).Count();
            if (((Modelo.LancamentoLote)obj).Acao == Modelo.Acao.Excluir && qtdFuncionarios != qtdFuncionariosExcluir)
            {
                ((Modelo.LancamentoLote)obj).Acao = Modelo.Acao.Alterar;
            }
        }

        private void ExcluirLancamentoLoteFuncionarioERelacionados(SqlTransaction trans, Modelo.ModeloBase obj, DAL.SQL.LancamentoLoteFuncionario dalLancamentoLoteFuncionario)
        {
            ExcluiRegistrosRelacionados(trans, obj);
            // o que não foi efetivado apenas altera dizendo qual foi a ultima ação.
            foreach (Modelo.LancamentoLoteFuncionario fpf in ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && x.Efetivado == false))
            {
                    fpf.UltimaAcao = (int)Modelo.Acao.Excluir;
                    dalLancamentoLoteFuncionario.Alterar(trans, fpf);                
            }
            dalLancamentoLoteFuncionario.ExcluirLoteIds(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && x.Efetivado == true).Select(x => x.Id).ToList());
        }

        /// <summary>
        /// Exlui registros de tabelas relacionas
        /// </summary>
        /// <param name="trans">Transação</param>
        /// <param name="obj"> Objeto Tipo Lançamento</param>
        private void ExcluiRegistrosRelacionados(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            switch (((Modelo.LancamentoLote)obj).TipoLancamento)
            {
                case (int)Modelo.TipoLancamento.MudancaHorario:
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario != null && ((Modelo.LancamentoLote)obj).Acao == Modelo.Acao.Excluir)
                    {
                        LancamentoLoteMudancaHorario dalLLMH = new LancamentoLoteMudancaHorario(new DataBase(db.ConnectionString));
                        dalLLMH.UsuarioLogado = UsuarioLogado;
                        dalLLMH.Excluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario);
                    }
                    ExcluirMudancaHorarioLote(trans, obj);
                    break;
                case (int)Modelo.TipoLancamento.InclusaoBanco:
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco != null && ((Modelo.LancamentoLote)obj).Acao == Modelo.Acao.Excluir)
                    {
                        LancamentoLoteInclusaoBanco dalLLIB = new LancamentoLoteInclusaoBanco(new DataBase(db.ConnectionString));
                        dalLLIB.UsuarioLogado = UsuarioLogado;
                        dalLLIB.Excluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco);
                    }
                    ExcluirInclusaoBancoLote(trans, obj);
                    break;
                case (int)Modelo.TipoLancamento.BilhetesImp:
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp != null && ((Modelo.LancamentoLote)obj).Acao == Modelo.Acao.Excluir)
                    {
                        LancamentoLoteBilhetesImp dalLLBI = new LancamentoLoteBilhetesImp(new DataBase(db.ConnectionString));
                        dalLLBI.UsuarioLogado = UsuarioLogado;
                        dalLLBI.Excluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp);
                    }
                    ExcluirBilhetesImpLote(trans, obj);
                    break;
                case (int)Modelo.TipoLancamento.Afastamento:
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento != null && ((Modelo.LancamentoLote)obj).Acao == Modelo.Acao.Excluir)
                    {
                        LancamentoLoteAfastamento dalLLA = new LancamentoLoteAfastamento(new DataBase(db.ConnectionString));
                        dalLLA.UsuarioLogado = UsuarioLogado;
                        dalLLA.Excluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteAfastamento);
                    }
                    ExcluirAfastamentosLote(trans, obj);
                    break;
                default:
                    break;
            }
        }

        private void ExcluirBilhetesImpLote(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).TipoLancamento == (int)Modelo.TipoLancamento.BilhetesImp)
            {
                try
                {
                    List<int> lFuncExc = new List<int>();
                    lFuncExc = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncExc.Count() > 0)
                    {
                        DAL.SQL.LancamentoLoteBilhetesImp dalLancamentoLoteBilhetesImp = new DAL.SQL.LancamentoLoteBilhetesImp(new DataBase(db.ConnectionString));
                        dalLancamentoLoteBilhetesImp.UsuarioLogado = UsuarioLogado;
                        dalLancamentoLoteBilhetesImp.ExcluirLancamentoLoteBilhetesImp(trans, lFuncExc);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao excluir as marcações manuais. Detalhes: " + e.Message);
                }
            }
        }

        private void ExcluirInclusaoBancoLote(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).TipoLancamento == (int)Modelo.TipoLancamento.InclusaoBanco)
            {
                try
                {
                    List<int> lFuncExc = new List<int>();
                    lFuncExc = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncExc.Count() > 0)
                    {
                        DAL.SQL.LancamentoLoteInclusaoBanco dalLancamentoLoteInclusaoBanco = new DAL.SQL.LancamentoLoteInclusaoBanco(new DataBase(db.ConnectionString));
                        dalLancamentoLoteInclusaoBanco.UsuarioLogado = UsuarioLogado;
                        dalLancamentoLoteInclusaoBanco.ExcluirLancamentoLoteInclusaoBanco(trans, lFuncExc);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao excluir as inclusões de banco de horas. Detalhes: " + e.Message);
                }
            }
        }

        private void ExcluirMudancaHorarioLote(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).TipoLancamento == (int)Modelo.TipoLancamento.MudancaHorario)
            {
                try
                {
                    List<int> lFuncExc = new List<int>();
                    lFuncExc = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncExc.Count() > 0)
                    {
                        DAL.SQL.LancamentoLoteMudancaHorario dalLancamentoLoteMudancaHorario = new DAL.SQL.LancamentoLoteMudancaHorario(new DataBase(db.ConnectionString));
                        dalLancamentoLoteMudancaHorario.UsuarioLogado = UsuarioLogado;
                        dalLancamentoLoteMudancaHorario.ExcluirMudancaHorarioLote(trans, lFuncExc);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao excluir a(s) mudança(s) de horário. Detalhes: " + e.Message);
                }
            }
        }

        private void ExcluirAfastamentosLote(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).TipoLancamento == (int)Modelo.TipoLancamento.Afastamento)
            {
                try
                {
                    List<int> lFuncExc = new List<int>();
                    lFuncExc = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Excluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncExc.Count() > 0)
                    {
                        DAL.SQL.LancamentoLoteAfastamento dalLancamentoLoteAfastamento = new DAL.SQL.LancamentoLoteAfastamento(new DataBase(db.ConnectionString));
                        dalLancamentoLoteAfastamento.UsuarioLogado = UsuarioLogado;
                        dalLancamentoLoteAfastamento.ExcluirAfastamentoLote(trans, lFuncExc);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao excluir a(s) mudança(s) de horário. Detalhes: " + e.Message);
                }
            }
        }
        private void TratarMudancaHorario(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario != null && ((Modelo.LancamentoLote)obj).Acao != Modelo.Acao.Excluir)
            {
                DAL.SQL.LancamentoLoteMudancaHorario dalLancamentoLoteMudancaHorario = new DAL.SQL.LancamentoLoteMudancaHorario(new DataBase(db.ConnectionString));
                dalLancamentoLoteMudancaHorario.UsuarioLogado = UsuarioLogado;
                try
                {
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario.Id > 0)
                    {
                        dalLancamentoLoteMudancaHorario.Alterar(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario);
                    }
                    else
                    {
                        ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario.IdLancamentoLote = ((Modelo.LancamentoLote)obj).Id;
                        ((Modelo.LancamentoLote)obj).Codigo = dalLancamentoLoteMudancaHorario.MaxCodigo();
                        dalLancamentoLoteMudancaHorario.Incluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao gerar a mudança de horário. Detalhes: " + e.Message);
                }

                List<int> lFuncAdd = new List<int>();
                List<int> lFuncAlt = new List<int>();
                try
                {
                    lFuncAdd = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Incluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncAdd.Count() > 0)
                    {
                        dalLancamentoLoteMudancaHorario.IncluirMudancaHorarioLote(trans, lFuncAdd);
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("AK_MudancaHorario_Funcionario_Data"))
                    {
                        throw new Exception("Erro ao adicionar a(s) mudança(s) de horário. Existem funcionários que já possuem mudança nesse mesmo dia");
                    }
                    else
                    {
                        throw new Exception("Erro ao adicionar a(s) mudança(s) de horário. Detalhes: " + e.Message);
                    }
                }

                try
                {
                    lFuncAlt = new List<int>();
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Alterar && x.Efetivado == true).Count() > 0 &&
                        (((Modelo.LancamentoLote)obj).DataLancamento != ((Modelo.LancamentoLote)obj).DataLancamentoAnt ||
                        ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario.Idhorario != ((Modelo.LancamentoLote)obj).LancamentoLoteMudancaHorario.Idhorario_ant))
                    {
                        lFuncAlt = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Alterar).Select(s => s.Id).ToList();
                    }
                    if (lFuncAlt.Count() > 0)
                    {
                        dalLancamentoLoteMudancaHorario.UpdateMudancaHorarioLote(trans, lFuncAlt);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao alterar a(s) mudança(s) de horário. Detalhes: " + e.Message);
                } 
            }
        }

        private void TratarInclusaoBanco(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco != null && ((Modelo.LancamentoLote)obj).Acao != Modelo.Acao.Excluir)
            {
                DAL.SQL.LancamentoLoteInclusaoBanco dalLancamentoLoteInclusaoBanco = new DAL.SQL.LancamentoLoteInclusaoBanco(new DataBase(db.ConnectionString));
                dalLancamentoLoteInclusaoBanco.UsuarioLogado = UsuarioLogado;
                try
                {
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco.Id > 0)
                    {
                        dalLancamentoLoteInclusaoBanco.Alterar(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco);
                    }
                    else
                    {
                        ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco.IdLancamentoLote = ((Modelo.LancamentoLote)obj).Id;
                        ((Modelo.LancamentoLote)obj).Codigo = dalLancamentoLoteInclusaoBanco.MaxCodigo();
                        dalLancamentoLoteInclusaoBanco.Incluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao gerar a mudança de horário. Detalhes: " + e.Message);
                }

                List<int> lFuncAdd = new List<int>();
                List<int> lFuncAlt = new List<int>();
                try
                {
                    lFuncAdd = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Incluir && x.Efetivado == true).Select(s => s.Id).ToList();
                    if (lFuncAdd.Count() > 0)
                    {
                        dalLancamentoLoteInclusaoBanco.IncluirLancamentoLoteInclusaoBanco(trans, lFuncAdd);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao adicionar a(s) mudança(s) de horário. Detalhes: " + e.Message);
                }

                try
                {
                    lFuncAlt = new List<int>();
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Alterar && x.Efetivado == true).Count() > 0 &&
                        (((Modelo.LancamentoLote)obj).DataLancamento != ((Modelo.LancamentoLote)obj).DataLancamentoAnt ||
                        ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco.Credito != ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco.Credito_ant ||
                        ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco.Debito != ((Modelo.LancamentoLote)obj).LancamentoLoteInclusaoBanco.Debito_ant))
                    {
                        lFuncAlt = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Alterar).Select(s => s.Id).ToList();
                    }
                    if (lFuncAlt.Count() > 0)
                    {
                        dalLancamentoLoteInclusaoBanco.UpdateLancamentoLoteInclusaoBanco(trans, lFuncAlt);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao alterar a(s) mudança(s) de horário. Detalhes: " + e.Message);
                }
            }
        }

        private void TratarFolgaMarcacao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).TipoLancamento == (int)Modelo.TipoLancamento.Folga)
            {
                Marcacao dalMarcacao = new Marcacao(db);
                dalMarcacao.UsuarioLogado = UsuarioLogado;
                try
                {
                    // Se alterou a data do lançamento e existe funcionários para alteração altera as folgas na marcação
                    List<int> funcsEdits = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Alterar && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
                    if (((Modelo.LancamentoLote)obj).DataLancamento != ((Modelo.LancamentoLote)obj).DataLancamentoAnt && funcsEdits.Count() > 0)
                    {
                        dalMarcacao.SetaFolgaEmLote(trans, funcsEdits, ((Modelo.LancamentoLote)obj).DataLancamentoAnt, false);
                        dalMarcacao.SetaFolgaEmLote(trans, funcsEdits, ((Modelo.LancamentoLote)obj).DataLancamento, true);
                    }

                    // Retira as folgas dos funcionários retirados do lote
                    List<int> funcsExc = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Excluir && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
                    if (funcsExc.Count() > 0)
                    {
                        dalMarcacao.SetaFolgaEmLote(trans, funcsExc, ((Modelo.LancamentoLote)obj).DataLancamentoAnt, false);
                    }

                    // Adiciona as folgas dos funcionários incluidos do lote
                    List<int> funcsInc = ((Modelo.LancamentoLote)obj).LancamentoLoteFuncionarios.Where(w => w.Acao == Modelo.Acao.Incluir && w.Efetivado == true).Select(s => s.IdFuncionario).ToList();
                    if (funcsInc.Count() > 0)
                    {
                        dalMarcacao.SetaFolgaEmLote(trans, funcsInc, ((Modelo.LancamentoLote)obj).DataLancamento, true);
                    }

                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao vincular os funcionários a folga. Erro: " + e.Message);
                }
            }
        }

        private void TratarBilhetesImp(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp != null && ((Modelo.LancamentoLote)obj).Acao != Modelo.Acao.Excluir)
            {
                DAL.SQL.LancamentoLoteBilhetesImp dalLancamentoLoteBilhetesImp = new DAL.SQL.LancamentoLoteBilhetesImp(new DataBase(db.ConnectionString));
                dalLancamentoLoteBilhetesImp.UsuarioLogado = UsuarioLogado;
                try
                {
                    if (((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp.Id > 0)
                    {
                        dalLancamentoLoteBilhetesImp.Alterar(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp);
                    }
                    else
                    {
                        ((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp.IdLancamentoLote = ((Modelo.LancamentoLote)obj).Id;
                        ((Modelo.LancamentoLote)obj).Codigo = dalLancamentoLoteBilhetesImp.MaxCodigo();
                        dalLancamentoLoteBilhetesImp.Incluir(trans, ((Modelo.LancamentoLote)obj).LancamentoLoteBilhetesImp);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao gerar a marcação manual. Detalhes: " + e.Message);
                }

                try
                {
                    dalLancamentoLoteBilhetesImp.IncluirLancamentoLoteBilhetesImp(trans, (Modelo.LancamentoLote)obj);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao adicionar as marcações manuais. Detalhes: " + e.Message);
                }

                try
                {
                    dalLancamentoLoteBilhetesImp.UpdateLancamentoLoteBilhetesImp(trans, (Modelo.LancamentoLote)obj);
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao alterar as marcações manuais. Detalhes: " + e.Message);
                }
            }
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLote>();
                    obj = Mapper.Map<Modelo.LancamentoLote>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLote();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLote();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.LancamentoLote obj)
        {
            try
            {

                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLote>();
                    obj = Mapper.Map<Modelo.LancamentoLote>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLote();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLote();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.LancamentoLote> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoLote>>();
                    obj = Mapper.Map<List<Modelo.LancamentoLote>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.LancamentoLote>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.LancamentoLote>();
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
                new SqlParameter ("@dataLancamento", SqlDbType.DateTime),
				new SqlParameter ("@Descricao", SqlDbType.VarChar),
                new SqlParameter ("@Observacao", SqlDbType.VarChar),
                new SqlParameter ("@tipoLancamento", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.LancamentoLote)obj).Codigo;
            parms[2].Value = ((Modelo.LancamentoLote)obj).Incdata;
            parms[3].Value = ((Modelo.LancamentoLote)obj).Inchora;
            parms[4].Value = ((Modelo.LancamentoLote)obj).Incusuario;
            parms[5].Value = ((Modelo.LancamentoLote)obj).Altdata;
            parms[6].Value = ((Modelo.LancamentoLote)obj).Althora;
            parms[7].Value = ((Modelo.LancamentoLote)obj).Altusuario;
            parms[8].Value = ((Modelo.LancamentoLote)obj).DataLancamento;
            parms[9].Value = ((Modelo.LancamentoLote)obj).Descricao;
            parms[10].Value = ((Modelo.LancamentoLote)obj).Observacao;
            parms[11].Value = ((Modelo.LancamentoLote)obj).TipoLancamento;
        }

        public Modelo.LancamentoLote LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoLote obj = new Modelo.LancamentoLote();
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

        public List<Modelo.LancamentoLote> GetAllList()
        {
            List<Modelo.LancamentoLote> lista = new List<Modelo.LancamentoLote>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            SetaListaObjeto(dr,ref lista);
            return lista;
        }

        public List<Modelo.LancamentoLote> GetAllListTipoLancamento(List<Modelo.TipoLancamento> TipoLancamento)
        {
            List<Modelo.LancamentoLote> lista = new List<Modelo.LancamentoLote>();
            SqlParameter[] parms = new SqlParameter[0];
            List<int> tp = TipoLancamento.Select(s => (int)s).ToList();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL + " where tipoLancamento in (select * from [dbo].[F_ClausulaIn](" + String.Join(",", tp.ToArray()) + ")) ", parms);
            SetaListaObjeto(dr, ref lista);
            return lista;
        }

        /// <summary>
        /// Exclui os lotes que não possuem registros filhos
        /// </summary>
        /// <param name="trans">Transação</param>
        public void ExcluiLoteSemFilho(SqlTransaction trans)
        {
            SqlParameter[] parms = new SqlParameter[0]
            { 
            };

            string aux = @" DELETE   from lancamentolote
                                where not exists (select * from LancamentoLoteFuncionario where idlancamentolote = lancamentolote.id)
                                      AND NOT EXISTS (SELECT * FROM LancamentoLoteBilhetesImp WHERE idLancamentoLote = LancamentoLote.id)
                                      AND NOT EXISTS (SELECT * FROM LancamentoLoteAfastamento WHERE idLancamentoLote = LancamentoLote.id)
                                      AND NOT EXISTS (SELECT * FROM LancamentoLoteInclusaoBanco WHERE idLancamentoLote = LancamentoLote.id)
                                      AND NOT EXISTS (SELECT * FROM LancamentoLoteMudancaHorario WHERE idLancamentoLote = LancamentoLote.id)
                           ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
        #endregion
    }
}
