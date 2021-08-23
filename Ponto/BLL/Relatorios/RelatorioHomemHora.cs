using DAL.SQL;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioHomemHora
    {
        private DAL.RelatoriosSQL.RelatorioHomemHora dalRelatorioHomemHora;
        public RelatorioHomemHora() : this(null)
        {
        }

        public RelatorioHomemHora(string conn)
        {
            if (String.IsNullOrEmpty(conn))
            {
                conn = Modelo.cwkGlobal.CONN_STRING;
            }
            DataBase db = new DataBase(conn);
            dalRelatorioHomemHora = new DAL.RelatoriosSQL.RelatorioHomemHora(db);
        }

        public DataTable GetRelatorioHomemHora(String idsFuncionarios, DateTime dataIni, DateTime dataFin, string idsOcorrencias)
        {
            return dalRelatorioHomemHora.GetRelatorioHomemHora(idsFuncionarios, dataIni, dataFin, idsOcorrencias);
        }

        public List<Modelo.Proxy.Relatorios.RelHomemHora> GetRelatorioHomemHora(List<int> idsFuncionarios, DateTime dataIni, DateTime dataFin, string idsOcorrencias)
        {
            DataTable dt = dalRelatorioHomemHora.GetRelatorioHomemHora(String.Join(",", idsFuncionarios), dataIni, dataFin, idsOcorrencias);
            List<Modelo.Proxy.Relatorios.RelHomemHora> lista = dt.DataTableMapToList<Modelo.Proxy.Relatorios.RelHomemHora>();
            return lista;
        }

        public List<Modelo.Proxy.Relatorios.PxyRelHomemHoraMonsanto> GetRelatorioHomemHoraMonsanto(List<int> idsFuncionarios, DateTime dataIni, DateTime dataFim)
        {

            DataTable dt = dalRelatorioHomemHora.GetRelatorioHomemHoraMonsanto(String.Join(",", idsFuncionarios), dataIni, dataFim);

            List<Modelo.Proxy.Relatorios.PxyRelHomemHoraMonsanto> lista = dt.DataTableMapToList<Modelo.Proxy.Relatorios.PxyRelHomemHoraMonsanto>();



            return PreenchePercentualRelatorio(lista, dt);
        }

        private List<PxyRelHomemHoraMonsanto> PreenchePercentualRelatorio(List<PxyRelHomemHoraMonsanto> lista, DataTable dt)
        {

            List<PxyRelHomemHoraMonsanto> lstRetorno = new List<PxyRelHomemHoraMonsanto>();

            BLL.HoraExtra HE = new BLL.HoraExtra(dt);
            IList<HorasExtrasPorDia> horasExtrasDoPeriodo = HE.CalcularHoraExtraDiaria();

            var listaPessoas = lista.GroupBy(n => n.Matricula)
                                    .Select(m => m.Where(o => o.Matricula == m.Key.ToString()).FirstOrDefault())
                                    .ToList();

            for (int i = 0; i < listaPessoas.Count(); i++)
            {
                PxyRelHomemHoraMonsanto objRelHomemHoraMonsanto = listaPessoas[i];
                objRelHomemHoraMonsanto.Percentuais = new List<Percentual>();
                List<Percentual> percentuais = new List<Percentual>();

                //List<string> colunas = dt.Columns
                //                    .Cast<DataColumn>()
                //                    .Select(x => x.ColumnName)
                //                    .ToList();

                foreach (DataRow row in dt.Rows)
                {
                    DateTime dataMarc = Convert.ToDateTime(row["data"]);
                    int idFuncionario = Convert.ToInt32(row["idFuncionario"]);

                    //Pegas os percentuais existentes
                    IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.Where(c => c.IdFuncionario == idFuncionario).SelectMany(x => x.HorasExtras).ToList();
                    var TotalHoras = heMS.GroupBy(l => l.Percentual)
                                    .Select(lg => new
                                    {
                                        Percentual = lg.Key,
                                        HoraDiurna = lg.Sum(w => w.HoraDiurna),
                                        HoraNoturna = lg.Sum(w => w.HoraNoturna)
                                    }).OrderBy(x => x.Percentual);

                    if (row["Matricula"].ToString() == objRelHomemHoraMonsanto.Matricula)
                    {
                        foreach (var item in TotalHoras)// Adiciona os percentuais nas respectivas colunas
                        {
                            if (!percentuais.Any(c => c.VlrPercentual.Equals(item.Percentual)))
                            {
                                percentuais.Add(new Percentual()
                                {
                                    VlrPercentual = item.Percentual,
                                    Diurno = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraDiurna),
                                    Noturno = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraNoturna)
                                });
                            }

                        }
                    }
                }
                objRelHomemHoraMonsanto.Percentuais = percentuais;

                lstRetorno.Add(objRelHomemHoraMonsanto);
            }

            return lstRetorno;
        }
    }
}
