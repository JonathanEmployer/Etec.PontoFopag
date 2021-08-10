using DAL.SQL;
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

        //public DataTable GetRelatorioHomemHoraMonsanto(String idsFuncionarios, DateTime dataIni, DateTime dataFin, string idsOcorrencias)
        //{
        //    return dalRelatorioHomemHora.GetRelatorioHomemHoraMonsanto(idsFuncionarios, dataIni, dataFin);
        //}

        public List<Modelo.Proxy.Relatorios.PxyRelHomemHoraMonsanto> GetRelatorioHomemHoraMonsanto(List<int> idsFuncionarios, DateTime dataIni, DateTime dataFim)
        {

            DataTable dt = dalRelatorioHomemHora.GetRelatorioHomemHoraMonsanto(String.Join(",", idsFuncionarios), dataIni, dataFim);

            List<Modelo.Proxy.Relatorios.PxyRelHomemHoraMonsanto> lista = dt.DataTableMapToList<Modelo.Proxy.Relatorios.PxyRelHomemHoraMonsanto>();



            return PreenchePercentualRelatorio(lista, dt);
        }

        private List<PxyRelHomemHoraMonsanto> PreenchePercentualRelatorio(List<PxyRelHomemHoraMonsanto> lista, DataTable dt)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                PxyRelHomemHoraMonsanto item = lista[i];

                item.Percentuais = new List<Percentual>();

                List<Percentual> percentuais = new List<Percentual>();

                List<string> colunas = dt.Columns
                                    .Cast<DataColumn>()
                                    .Select(x => x.ColumnName)
                                    .ToList();

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Matricula"].ToString() == item.Matricula)
                    {
                        if (row["percentualextra50"].ToString() != "0,00" || row["percentualextraNoturna50"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "50",
                                Diurno = row["percentualextra50"].ToString(),
                                Noturno = row["percentualextraNoturna50"].ToString()
                            }
                            );

                        }

                        if (row["percentualextra60"].ToString() != "0,00" || row["percentualextraNoturna60"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "60",
                                Diurno = row["percentualextra60"].ToString(),
                                Noturno = row["percentualextraNoturna60"].ToString()
                            }
                            );

                        }

                        if (row["percentualextra70"].ToString() != "0,00" || row["percentualextraNoturna70"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "70",
                                Diurno = row["percentualextra70"].ToString(),
                                Noturno = row["percentualextraNoturna70"].ToString()
                            }
                            );

                        }

                        if (row["percentualextra80"].ToString() != "0,00" || row["percentualextraNoturna80"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "80",
                                Diurno = row["percentualextra80"].ToString(),
                                Noturno = row["percentualextraNoturna80"].ToString()
                            }
                            );

                        }

                        if (row["percentualextra90"].ToString() != "0,00" || row["percentualextraNoturna90"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "90",
                                Diurno = row["percentualextra90"].ToString(),
                                Noturno = row["percentualextraNoturna90"].ToString()
                            }
                            );

                        }

                        if (row["percentualextra100"].ToString() != "0,00" || row["percentualextraNoturna100"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "100",
                                Diurno = row["percentualextra100"].ToString(),
                                Noturno = row["percentualextraNoturna100"].ToString()
                            }
                            );
                        }

                        if (row["percentualextrasab"].ToString() != "0,00" || row["percentualextraNoturnasab"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Sábado",
                                Diurno = row["percentualextrasab"].ToString(),
                                Noturno = row["percentualextraNoturnasab"].ToString()
                            }
                            );
                        }

                        if (row["percentualextradom"].ToString() != "0,00" || row["percentualextraNoturnadom"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Domingo",
                                Diurno = row["percentualextradom"].ToString(),
                                Noturno = row["percentualextraNoturnadom"].ToString()
                            }
                            );
                        }

                        if (row["percentualextrafer"].ToString() != "0,00" || row["percentualextraNoturnafer"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Feriado",
                                Diurno = row["percentualextrafer"].ToString(),
                                Noturno = row["percentualextraNoturnafer"].ToString()
                            }
                            );
                        }

                        if (row["percentualextrafol"].ToString() != "0,00" || row["percentualextraNoturnafol"].ToString() != "0,00")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Folga",
                                Diurno = row["percentualextrafol"].ToString(),
                                Noturno = row["percentualextraNoturnafol"].ToString()
                            }
                            );
                        }                    
                    }
                }
                item.Percentuais = percentuais;
            }

            return lista;
        }
    }
}
