using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class LancamentoCartaoPonto : IBLL<Modelo.LancamentoCartaoPonto>
    {
        DAL.ILancamentoCartaoPonto dalLancamentoCartaoPonto;
        private string ConnectionString;
        private readonly Modelo.Cw_Usuario UsuarioLogado;

        public LancamentoCartaoPonto() : this(null)
        {
            
        }

        public LancamentoCartaoPonto(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LancamentoCartaoPonto(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalLancamentoCartaoPonto = new DAL.SQL.LancamentoCartaoPonto(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalLancamentoCartaoPonto = new DAL.SQL.LancamentoCartaoPonto(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLancamentoCartaoPonto.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalLancamentoCartaoPonto.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalLancamentoCartaoPonto.GetAll();
        }

        public Modelo.LancamentoCartaoPonto LoadObject(int id)
        {
            return dalLancamentoCartaoPonto.LoadObject(id);
        }

        public List<Modelo.LancamentoCartaoPonto> GetAllList()
        {
            return dalLancamentoCartaoPonto.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LancamentoCartaoPonto objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Regs.Where(w => w.Editavel).Count() == 0)
            {
                ret.Add("Regs", "Nenhum registro novo lan?ado para o funcion?rio.");
            }
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LancamentoCartaoPonto objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLancamentoCartaoPonto.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalLancamentoCartaoPonto.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLancamentoCartaoPonto.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// M?todo respons?vel em retornar o id da tabela. O campo padr?o para busca ? o campo c?digo, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso n?o desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo C?digo</param>
        /// <param name="pCampo">Nome do segundo campo que ser? utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalLancamentoCartaoPonto.getId(pValor, pCampo, pValor2);
        }

        public Dictionary<string, string> SalvarCartaoPonto(Modelo.LancamentoCartaoPonto obj, out List<Modelo.BilhetesImp> bilhetes, out Modelo.Funcionario funcionario)
        {
            Dictionary<string, string> erros = ValidaObjeto(obj);
            bilhetes = new List<Modelo.BilhetesImp>();
            funcionario = new Modelo.Funcionario();
            if (erros.Count == 0)
            {
                Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                Funcionario bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
                funcionario = bllFuncionario.LoadObject(obj.IdFuncionario);
                GeraMarcacoesNaoGeradas(obj, funcionario, bllMarcacao);

                bool virouDia = false;
                LancamentoCartaoPontoRegistros[] registrosSalvar = obj.Regs.Where(w => w.Editavel).ToArray();
                Modelo.LancamentoCartaoPontoRegistros reg = new Modelo.LancamentoCartaoPontoRegistros();
                BilhetesImp bllBilhetesImp = new BilhetesImp(ConnectionString, UsuarioLogado);
                Horario bllHorario = new Horario(ConnectionString, UsuarioLogado);
                List<Modelo.Marcacao> marcsFuncs = bllMarcacao.GetPorFuncionario(obj.IdFuncionario, Convert.ToDateTime(obj.DataInicial), Convert.ToDateTime(obj.DataFinal), false);
                List<int> idsHorarios = marcsFuncs.Select(s => s.Idhorario).Distinct().ToList();
                List<Modelo.Horario> horarios = new List<Modelo.Horario>();
                foreach (int idHorario in idsHorarios)
                {
                    horarios.Add(bllHorario.LoadObject(idHorario));
                }

                int maxCodigoBilhetes = bllBilhetesImp.MaxCodigo();
                for (int i = 0; i < registrosSalvar.Count(); i++)
                {
                    string ent = "";
                    string sai = "";
                    string regAnt = "";
                    reg = registrosSalvar[i];
                    virouDia = false;
                    DateTime marData = reg.Data;
                    bool primeiroRegistroDiaAnterior = false;
                    for (int j = 1; j < 9; j++)
                    {
                        var ev = reg.GetType().GetProperty("E" + j).GetValue(reg, null);
                        ent = ev == null ? "" : ev.ToString();
                        var sv = reg.GetType().GetProperty("S" + j).GetValue(reg, null);
                        sai = sv == null ? "" : sv.ToString();
                        string entTeste = ent;
                        ValidaViradaDia(ref virouDia, ref entTeste, sai);
                        //Verifica se a primeira batida ? quem deve ser puxada para o dia corrente Ex: jornada 00:00 - 03:00 - 04:00 - 07:00 Registros 23:50 - 03:00 - 04:00 - 07:00
                        if (j == 1 && virouDia)
                        {
                            Modelo.Marcacao marcDia = marcsFuncs.Where(w => w.Data == marData).FirstOrDefault();
                            int idHorario = marcDia.Idhorario;
                            Modelo.Horario horario = horarios.Where(w => w.Id == idHorario).FirstOrDefault();
                            Modelo.HorarioDetalhe horarioDetalhe = new Modelo.HorarioDetalhe();
                            if (horario.HorariosDetalhe.ToList().Where(w => w != null).Any())
                            {
                                horarioDetalhe = horario.HorariosDetalhe.ToList().Where(w => w.DiaStr == marcDia.Dia).FirstOrDefault();
                            }
                            else
                            {
                                horarioDetalhe = horario.HorariosFlexiveis.ToList().Where(w => w.Data == marcDia.Data).FirstOrDefault();
                            }
                            
                            if (horarioDetalhe != null && ent.ConvertHorasMinuto() > sai.ConvertHorasMinuto())
                            {
                                int entPrevistaMin = horarioDetalhe.Entrada_1.ConvertHorasMinuto();
                                entPrevistaMin = entPrevistaMin == 0 ? 1440 : entPrevistaMin;
                                int entMin = ent.ConvertHorasMinuto();
                                int difAceitavel = horario.Limitemin.ConvertHorasMinuto();
                                int dif = Math.Abs(entPrevistaMin - entMin);
                                if (dif <= difAceitavel)
                                {
                                    primeiroRegistroDiaAnterior = true;
                                    AddBilhete(bilhetes, reg.Data.AddDays(-1), reg.Data, ent, "E", funcionario, obj.IdJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                                    AddBilhete(bilhetes, reg.Data, reg.Data, sai, "S", funcionario, obj.IdJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                                }
                                virouDia = false;
                            }
                        }

                        if (!primeiroRegistroDiaAnterior)
                        {
                            ValidaViradaDia(ref virouDia, ref regAnt, ent);
                            AddBilhete(bilhetes, !virouDia ? reg.Data : reg.Data.AddDays(+1), reg.Data, ent, "E", funcionario, obj.IdJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                            ValidaViradaDia(ref virouDia, ref regAnt, sai);
                            AddBilhete(bilhetes, !virouDia ? reg.Data : reg.Data.AddDays(+1), reg.Data, sai, "S", funcionario, obj.IdJustificativa, obj.Motivo, maxCodigoBilhetes++, j);
                        }
                        else
                            primeiroRegistroDiaAnterior = false;
                    }
                }

                if (bilhetes.Count > 0)
                {
                    registrosSalvar.ToList().ForEach(f => f.IdFuncionario = obj.IdFuncionario);
                    dalLancamentoCartaoPonto.IncluirRegistros(bilhetes, registrosSalvar.ToList());
                }
            }
            return erros;
        }

        private static void GeraMarcacoesNaoGeradas(Modelo.LancamentoCartaoPonto obj, Modelo.Funcionario funcionario, Marcacao bllMarcacao)
        {
            if (!DateTime.TryParse(obj.DataInicial, out DateTime pdataInicial))
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new Exception("Erro ao converter a data inicial do lan?amento do cart?o ponto");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            if (!DateTime.TryParse(obj.DataFinal, out DateTime pDataFinal))
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new Exception("Erro ao converter a data final do lan?amento do cart?o ponto");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }

            pdataInicial = pdataInicial.AddDays(-1);
            pDataFinal = pDataFinal.AddDays(+1);
            TimeSpan ts = pDataFinal.AddDays(1) - pdataInicial;

            int qtd = bllMarcacao.QuantidadeMarcacoes(funcionario.Id, pdataInicial, pDataFinal);

            if (ts.TotalDays > qtd)
            {
                Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                bllMarcacao.AtualizaData(pdataInicial, pDataFinal, funcionario);
            }
        }

        private static void ValidaViradaDia(ref bool virouDia, ref string regAnt, string hora)
        {
            if (!string.IsNullOrEmpty(regAnt) && !string.IsNullOrEmpty(hora) && regAnt.ConvertHorasMinuto() > hora.ConvertHorasMinuto())
            {
                virouDia = true;
            }
            if (!string.IsNullOrEmpty(hora))
            {
                regAnt = hora;
            }
        }

        private void AddBilhete(List<Modelo.BilhetesImp> bilhetes, DateTime dt, DateTime dt_mar, string hora, string entSai, Modelo.Funcionario funcionario, int idJustificativa, string motivo, int codigo, int posicao)
        {
            if (!string.IsNullOrEmpty(hora))
            {
                bilhetes.Add(new Modelo.BilhetesImp()
                {
                    Acao = Acao.Incluir,
                    Codigo = codigo,
                    Data = dt,
                    Mar_data = dt_mar,
                    Hora = hora,
                    Mar_hora = hora,
                    Relogio = "MA",
                    Mar_relogio = "MA",
                    Idjustificativa = idJustificativa,
                    IdFuncionario = funcionario.Id,
                    PIS = funcionario.Pis,
                    DsCodigo = funcionario.Dscodigo,
                    Func = funcionario.Dscodigo,
                    Ent_sai = entSai,
                    Importado = 1,
                    Ordem = entSai == "E" ? "010" : "011",
                    Posicao = posicao,
                    Incdata = DateTime.Now.Date,
                    Inchora = DateTime.Now,
                    Altdata = null,
                    Althora = null,
                    Incusuario = UsuarioLogado.Login,
                    Ocorrencia = 'I',
                    Motivo = motivo
                });
            }
        }
    }
}
