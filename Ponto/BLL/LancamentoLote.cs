using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class LancamentoLote : IBLL<Modelo.LancamentoLote>
    {
        DAL.ILancamentoLote dalLancamentoLote;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoLote()
            : this(null)
        {

        }

        public LancamentoLote(string connString)
            : this(connString, null)
        {

        }

        public LancamentoLote(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLancamentoLote = new DAL.SQL.LancamentoLote(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoLote.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoLote.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoLote.GetAll();
        }

        public List<Modelo.LancamentoLote> GetAllList()
        {
            return dalLancamentoLote.GetAllList();
        }

        public Modelo.LancamentoLote LoadObject(int id)
        {
            return dalLancamentoLote.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoLote objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("Codigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("Descricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoLote objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                if (objeto.TipoLancamento == (int)Modelo.TipoLancamento.MudancaHorario && (objeto.LancamentoLoteMudancaHorario != null && objeto.LancamentoLoteMudancaHorario.Tipohorario == 3))
                {
                    BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(ConnectionString, UsuarioLogado);
                    int tipoHorario = objeto.LancamentoLoteMudancaHorario.Tipohorario;
                    int idHorario = objeto.LancamentoLoteMudancaHorario.IdHorarioDinamico.GetValueOrDefault();
                    int? idHorarioDinamico = objeto.LancamentoLoteMudancaHorario.IdHorarioDinamico;
                    bllMudancaHorario.ValidaHorarioDinamico(ref tipoHorario, ref idHorario, objeto.DataLancamento, objeto.LancamentoLoteMudancaHorario.CicloSequenciaIndice, ref idHorarioDinamico);
                    objeto.LancamentoLoteMudancaHorario.Tipohorario = tipoHorario;
                    objeto.LancamentoLoteMudancaHorario.Idhorario = idHorario;
                    objeto.LancamentoLoteMudancaHorario.IdHorarioDinamico = idHorarioDinamico;
                }

                objeto.Acao = pAcao;
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoLote.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoLote.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        if (objeto.LancamentoLoteFuncionarios == null)
                        {
                            BLL.LancamentoLoteFuncionario bllFPF = new LancamentoLoteFuncionario(ConnectionString, UsuarioLogado);
                            objeto.LancamentoLoteFuncionarios = bllFPF.GetListWhere("and idLancamentoLote = " + objeto.Id.ToString());
                        }
                        switch (objeto.TipoLancamento)
	                    {
                            case (int)TipoLancamento.Folga:
                                break;
                            case (int)TipoLancamento.Afastamento:
                                break;
                            case (int)TipoLancamento.MudancaHorario:
                                BLL.LancamentoLoteMudancaHorario bllLLMH = new LancamentoLoteMudancaHorario(ConnectionString, UsuarioLogado);
                                objeto.LancamentoLoteMudancaHorario = bllLLMH.LoadByIdLote(objeto.Id);
                                break;
                            case (int)TipoLancamento.InclusaoBanco:
                                BLL.LancamentoLoteInclusaoBanco bllLLIB = new LancamentoLoteInclusaoBanco(ConnectionString, UsuarioLogado);
                                objeto.LancamentoLoteInclusaoBanco = bllLLIB.LoadByIdLote(objeto.Id);
                                break;
                            case (int)TipoLancamento.BilhetesImp:
                                BLL.LancamentoLoteBilhetesImp bllLLBI = new LancamentoLoteBilhetesImp(ConnectionString, UsuarioLogado);
                                objeto.LancamentoLoteBilhetesImp = bllLLBI.LoadByIdLote(objeto.Id);
                                break;
                            default:
                                break;
	                    }
                        objeto.LancamentoLoteFuncionarios.ToList().ForEach(i => i.Acao = Acao.Excluir);
                        dalLancamentoLote.Excluir(objeto);
                        break;
                }

                // Após fazer Inclusões/Alterações/Exclusões em lote corrigi os registros relacionados.
                switch (objeto.TipoLancamento)
                {
                    case (int)Modelo.TipoLancamento.MudancaHorario: //Ao realizar mudança em lote atualiza os horários das marcações e funcionários.
                        try
                        {
                            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
                            BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, UsuarioLogado);
                            DateTime dataBase = objeto.DataLancamento > objeto.DataLancamentoAnt ? objeto.DataLancamento : objeto.DataLancamentoAnt;
                            List<int> FuncRecalc = IdsFuncionariosRecalcularLote(objeto);
                            if (FuncRecalc.Count() > 0)
                            {
                                bllMarcacao.AtualizaMudancaHorarioMarcacao(FuncRecalc, dataBase);
                                bllFuncionario.AtualizaHorariosFuncionariosMudanca(FuncRecalc.ToList());
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    break;
                    case (int)Modelo.TipoLancamento.InclusaoBanco: //Ao realizar mudança em lote atualiza as inclusões de Banco das marcações e funcionários.=
                    break;
                    default:
                    break;
                }
            }
            return erros;
        }

        public static List<int> IdsFuncionariosRecalcularLote(Modelo.LancamentoLote objeto)
        {
            List<int> FuncRecalc = objeto.LancamentoLoteFuncionarios.Where(w => w.Efetivado == true).Where(w => w.Acao == Modelo.Acao.Incluir || w.Acao == Modelo.Acao.Excluir).Select(s => s.IdFuncionario).ToList();
            switch (objeto.TipoLancamento)
            {
                case (int)Modelo.TipoLancamento.MudancaHorario:
                    if (objeto.DataLancamento != objeto.DataLancamentoAnt || objeto.LancamentoLoteMudancaHorario.Idhorario != objeto.LancamentoLoteMudancaHorario.Idhorario_ant)
                    {
                        FuncRecalc.AddRange(objeto.LancamentoLoteFuncionarios.Where(w => w.Efetivado == true).Where(w => w.Acao == Modelo.Acao.Alterar).Select(s => s.IdFuncionario).ToList());
                    }
                    break;
                case (int)Modelo.TipoLancamento.Afastamento:
                    List<int> lFuncAlt = new List<int>();
                    lFuncAlt = DAL.SQL.LancamentoLote.ListaFuncionariosAfastamentoAlterado(objeto).Select(s => s.IdFuncionario).ToList();
                    if (lFuncAlt.Count() > 0)
                    {
                        FuncRecalc.AddRange(lFuncAlt);
                    }
                    break;
                default:
                    if (objeto.DataLancamento != objeto.DataLancamentoAnt)
                    {
                        FuncRecalc.AddRange(objeto.LancamentoLoteFuncionarios.Where(w => w.Efetivado == true).Where(w => w.Acao == Modelo.Acao.Alterar).Select(s => s.IdFuncionario).ToList());
                    }
                    break;
            }
            return FuncRecalc;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalLancamentoLote.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Lista de Lançamento em lote de acordo com tipos de lançamento
        /// </summary>
        /// <param name="TipoLancamento">Lista com os tipos de lançamentos</param>
        /// <returns></returns>
        public List<Modelo.LancamentoLote> GetAllListTipoLancamento(List<Modelo.TipoLancamento> TipoLancamento)
        {
            return dalLancamentoLote.GetAllListTipoLancamento(TipoLancamento);
        }
    }
}
