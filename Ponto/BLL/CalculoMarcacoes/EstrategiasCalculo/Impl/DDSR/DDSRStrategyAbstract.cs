using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.CalculoMarcacoes.EstrategiasCalculo.Impl.DDSR
{
    public abstract class DDSRStrategyAbstract
    {
        private IEnumerable<DataRow> _Marcacoes;

        protected IEnumerable<DataRow> Marcacoes
        {
            get { return _Marcacoes; }
            private set { _Marcacoes = value; }
        }

        protected string _ConnectionString;

        public string ConnectionString
        {
            get { return _ConnectionString; }
            private set { _ConnectionString = value; }
        }

        private Dictionary<int, Modelo.Horario> _HorariosDasMarcacoes;

        protected Dictionary<int, Modelo.Horario> HorariosDasMarcacoes
        {
            get { return _HorariosDasMarcacoes; }
            private set { _HorariosDasMarcacoes = value; }
        }

        private DDSRStrategyAbstract()
        {

        }

        public DDSRStrategyAbstract(IEnumerable<DataRow> marcacoes, Dictionary<int, Modelo.Horario> horariosDasMarcacoes, string connectionString)
            : this()
        {
            Marcacoes = marcacoes;
            ConnectionString = connectionString;
            HorariosDasMarcacoes = horariosDasMarcacoes;
        }

        /// <summary>
        /// Método que edita a marcação que "hospedará" o DSR
        /// </summary>
        /// <param name="pMarcacao">Linha de marcação original da tabela de marcação</param>
        /// <param name="ValorDDSR">Valor do Desconto do DSR à ser gravado na tabela de marcação</param>
        /// <returns>Marcação de DSR editada para gravação na tabela de marcação</returns>
        protected Modelo.Marcacao MarcacaoDSRFactory(DataRow pMarcacao, string ValorDDSR)
        {
            Modelo.Marcacao obj = new Modelo.Marcacao();
            obj.Valordsr = ValorDDSR;
            obj.Dsr = 1;
            obj.Legenda = Convert.ToString(pMarcacao["legenda"]);
            obj.Id = Convert.ToInt32(pMarcacao["id"]);
            obj.Dscodigo = Convert.ToString(pMarcacao["dscodigo"]);
            obj.Idfuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
            obj.Dia = Convert.ToString(pMarcacao["dia"]);
            obj.Ocorrencia = Convert.ToString(pMarcacao["ocorrencia"]);
            obj.Horastrabalhadas = Convert.ToString(pMarcacao["horastrabalhadas"]);
            obj.Horastrabalhadasnoturnas = Convert.ToString(pMarcacao["horastrabalhadasnoturnas"]);
            obj.Horasextrasdiurna = Convert.ToString(pMarcacao["horasextrasdiurna"]);
            obj.Horasextranoturna = Convert.ToString(pMarcacao["horasextranoturna"]);
            obj.Horasfaltas = Convert.ToString(pMarcacao["horasfaltas"]);
            obj.Horasfaltanoturna = Convert.ToString(pMarcacao["horasfaltanoturna"]);
            obj.Bancohorascre = Convert.ToString(pMarcacao["bancohorascre"]);
            obj.Bancohorasdeb = Convert.ToString(pMarcacao["bancohorasdeb"]);
            obj.Folga = Convert.ToInt16(pMarcacao["folga"]);
            obj.Neutro = Convert.ToBoolean(pMarcacao["neutro"]);
            obj.TotalHorasTrabalhadas = Convert.ToString(pMarcacao["totalHorasTrabalhadas"]);
            obj.Naoconsiderarcafe = Convert.ToInt16(pMarcacao["naoconsiderarcafe"]);
            obj.Naoentrarbanco = Convert.ToInt16(pMarcacao["naoentrarbanco"]);
            obj.Semcalculo = Convert.ToInt16(pMarcacao["semcalculo"]);
            obj.Data = Convert.ToDateTime(pMarcacao["data"]);
            obj.Horascompensadas = Convert.ToString(pMarcacao["horascompensadas"]);
            obj.Idcompensado = pMarcacao["idcompensado"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idcompensado"]);
            obj.Idhorario = Convert.ToInt32(pMarcacao["idhorario"]);
            obj.Entrada_1 = Convert.ToString(pMarcacao["entrada_1"]);
            obj.Entrada_2 = Convert.ToString(pMarcacao["entrada_2"]);
            obj.Entrada_3 = Convert.ToString(pMarcacao["entrada_3"]);
            obj.Entrada_4 = Convert.ToString(pMarcacao["entrada_4"]);
            obj.Entrada_5 = Convert.ToString(pMarcacao["entrada_5"]);
            obj.Entrada_6 = Convert.ToString(pMarcacao["entrada_6"]);
            obj.Entrada_7 = Convert.ToString(pMarcacao["entrada_7"]);
            obj.Entrada_8 = Convert.ToString(pMarcacao["entrada_8"]);
            obj.Saida_1 = Convert.ToString(pMarcacao["saida_1"]);
            obj.Saida_2 = Convert.ToString(pMarcacao["saida_2"]);
            obj.Saida_3 = Convert.ToString(pMarcacao["saida_3"]);
            obj.Saida_4 = Convert.ToString(pMarcacao["saida_4"]);
            obj.Saida_5 = Convert.ToString(pMarcacao["saida_5"]);
            obj.Saida_6 = Convert.ToString(pMarcacao["saida_6"]);
            obj.Saida_7 = Convert.ToString(pMarcacao["saida_7"]);
            obj.Saida_8 = Convert.ToString(pMarcacao["saida_8"]);
            obj.Entradaextra = Convert.ToString(pMarcacao["entradaextra"]);
            obj.Saidaextra = Convert.ToString(pMarcacao["saidaextra"]);
            obj.Ent_num_relogio_1 = Convert.ToString(pMarcacao["ent_num_relogio_1"]);
            obj.Ent_num_relogio_2 = Convert.ToString(pMarcacao["ent_num_relogio_2"]);
            obj.Ent_num_relogio_3 = Convert.ToString(pMarcacao["ent_num_relogio_3"]);
            obj.Ent_num_relogio_4 = Convert.ToString(pMarcacao["ent_num_relogio_4"]);
            obj.Ent_num_relogio_5 = Convert.ToString(pMarcacao["ent_num_relogio_5"]);
            obj.Ent_num_relogio_6 = Convert.ToString(pMarcacao["ent_num_relogio_6"]);
            obj.Ent_num_relogio_7 = Convert.ToString(pMarcacao["ent_num_relogio_7"]);
            obj.Ent_num_relogio_8 = Convert.ToString(pMarcacao["ent_num_relogio_8"]);
            obj.Sai_num_relogio_1 = Convert.ToString(pMarcacao["sai_num_relogio_1"]);
            obj.Sai_num_relogio_2 = Convert.ToString(pMarcacao["sai_num_relogio_2"]);
            obj.Sai_num_relogio_3 = Convert.ToString(pMarcacao["sai_num_relogio_3"]);
            obj.Sai_num_relogio_4 = Convert.ToString(pMarcacao["sai_num_relogio_4"]);
            obj.Sai_num_relogio_5 = Convert.ToString(pMarcacao["sai_num_relogio_5"]);
            obj.Sai_num_relogio_6 = Convert.ToString(pMarcacao["sai_num_relogio_6"]);
            obj.Sai_num_relogio_7 = Convert.ToString(pMarcacao["sai_num_relogio_7"]);
            obj.Sai_num_relogio_8 = Convert.ToString(pMarcacao["sai_num_relogio_8"]);
            obj.Naoentrarnacompensacao = Convert.ToInt16(pMarcacao["naoentrarnacompensacao"]);
            obj.Idfechamentobh = pMarcacao["idfechamentobh"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idfechamentobh"]);
            obj.IdFechamentoPonto = pMarcacao["idfechamentoPonto"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idfechamentoPonto"]);
            obj.Abonardsr = Convert.ToInt16(pMarcacao["abonardsr"]);
            obj.Totalizadoresalterados = Convert.ToInt16(pMarcacao["totalizadoresalterados"]);
            obj.Calchorasextrasdiurna = Convert.ToInt32(pMarcacao["calchorasextrasdiurna"]);
            obj.Calchorasextranoturna = Convert.ToInt32(pMarcacao["calchorasextranoturna"]);
            obj.Calchorasfaltas = Convert.ToInt32(pMarcacao["calchorasfaltas"]);
            obj.Calchorasfaltanoturna = Convert.ToInt32(pMarcacao["calchorasfaltanoturna"]);
            obj.Incdata = Convert.ToDateTime(pMarcacao["incdata"]);
            obj.Inchora = Convert.ToDateTime(pMarcacao["inchora"]);
            obj.Incusuario = Convert.ToString(pMarcacao["incusuario"]);
            obj.Codigo = Convert.ToInt32(pMarcacao["codigo"]);
            obj.ExpHorasextranoturna = Convert.ToString(pMarcacao["exphorasextranoturna"]);
            obj.Chave = obj.ToMD5();
            return obj;
        }

        /// <summary>
        /// Método que retorna o valor do Desconto à ser efetuado no DSR do funcionário
        /// </summary>
        /// <param name="horario">Horário gravado na marcação do funcionário</param>
        /// <param name="minutosFalta">Somatório (em minutos) de faltas desde o último DSR do período</param>
        /// <returns></returns>
        protected string RetornaValorDescontoDSR(Modelo.Horario horario, int minutosFalta)
        {
            if (horario.DescontardsrBool)
            {
                if (horario.bUtilizaDDSRProporcional && horario.LimitesDDsrProporcionais.Count() > 0)
                {
                    if (minutosFalta <= Modelo.cwkFuncoes.ConvertHorasMinuto(horario.LimitesDDsrProporcionais
                                    .OrderBy(o => Modelo.cwkFuncoes.ConvertHorasMinuto(o.LimitePerdaDsr)).FirstOrDefault().LimitePerdaDsr))
                    {
                        return Modelo.cwkFuncoes.ConvertMinutosHora(Decimal.Zero);
                    }
                    if (minutosFalta > Modelo.cwkFuncoes.ConvertHorasMinuto(horario.LimitesDDsrProporcionais
                                    .OrderByDescending(o => Modelo.cwkFuncoes.ConvertHorasMinuto(o.LimitePerdaDsr)).FirstOrDefault().LimitePerdaDsr))
                    { 
                        return horario.LimitesDDsrProporcionais
                                    .OrderByDescending(o => Modelo.cwkFuncoes.ConvertHorasMinuto(o.LimitePerdaDsr)).FirstOrDefault().QtdHorasDsr;
                    }

                    Modelo.LimiteDDsr limite = null;
                    
                    foreach (var item in horario.LimitesDDsrProporcionais
                                    .OrderByDescending(o => Modelo.cwkFuncoes.ConvertHorasMinuto(o.LimitePerdaDsr)))
                    {
                        if (minutosFalta >= Modelo.cwkFuncoes.ConvertHorasMinuto(item.LimitePerdaDsr))
                        {
                            limite = item;
                            break;
                        }
                    }
                    if (limite == null)
                    {
                        limite = horario.LimitesDDsrProporcionais
                            .OrderByDescending(o => Modelo.cwkFuncoes.ConvertHorasMinuto(o.LimitePerdaDsr))
                            .FirstOrDefault();
                    }
                    if (limite == null)
                    {
                        return Modelo.cwkFuncoes.ConvertMinutosHora(Decimal.Zero);
                    }
                    else
                    {
                        return limite.QtdHorasDsr;
                    }
                }
                else if (horario.DSRPorPercentual)
                {
                    return Modelo.cwkFuncoes.ConvertMinutosHora((int)Math.Round(minutosFalta * horario.Descontohorasdsr, 0));
                }
                else
                {
                    return minutosFalta > Modelo.cwkFuncoes.ConvertHorasMinuto(horario.Limiteperdadsr) ?
                        horario.Qtdhorasdsr : Modelo.cwkFuncoes.ConvertMinutosHora(minutosFalta);
                }
            }
            else
            {
                return Modelo.cwkFuncoes.ConvertMinutosHora(Decimal.Zero);
            }
        }

        protected List<IEnumerable<DataRow>> AgrupaMarcacoesPorMudancaHorario(IEnumerable<DataRow> semanaComDSR)
        {
            try
            {
                List<IEnumerable<DataRow>> MarcacoesAgrupadasPorMudancasHorario = new List<IEnumerable<DataRow>>();

                IEnumerable<DataRow> diasAteMudancaHorario = semanaComDSR.TakeWhile(f => !(f.Field<string>("legenda").ToLower().Contains("m")));
                MarcacoesAgrupadasPorMudancasHorario.Add(diasAteMudancaHorario);
                int count = diasAteMudancaHorario.Count();
                while (true)
                {
                    var MarcacoesAteProximaMudanca = semanaComDSR.Skip(count).Take(1);
                    MarcacoesAteProximaMudanca = MarcacoesAteProximaMudanca.Concat(semanaComDSR.Skip(count + 1).TakeWhile(f => !(f.Field<string>("legenda").ToLower().Contains("m"))));
                    if (MarcacoesAteProximaMudanca != null)
                    {
                        if (MarcacoesAteProximaMudanca.Count() > 0)
                        {
                            MarcacoesAgrupadasPorMudancasHorario.Add(MarcacoesAteProximaMudanca);
                            count += MarcacoesAteProximaMudanca.Count();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int qtdMarcAgrp = MarcacoesAgrupadasPorMudancasHorario.Sum(s => s.Count());
                var MarcacoesRestantes = semanaComDSR.Skip(qtdMarcAgrp).Take(semanaComDSR.Count() - qtdMarcAgrp);
                if (MarcacoesRestantes.Count() > 0)
                {
                    MarcacoesAgrupadasPorMudancasHorario.Add(MarcacoesRestantes);
                }
                
                return MarcacoesAgrupadasPorMudancasHorario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
