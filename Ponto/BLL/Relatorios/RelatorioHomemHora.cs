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
                        if (row["percentualextra50"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturna50"].ToString().Replace(",", "").Replace(".", "") != "000");
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "50",
                                Diurno = row["percentualextra50"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturna50"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );

                        }

                        if (row["percentualextra60"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturna60"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "60",
                                Diurno = row["percentualextra60"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturna60"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );

                        }

                        if (row["percentualextra70"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturna70"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "70",
                                Diurno = row["percentualextra70"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturna70"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );

                        }

                        if (row["percentualextra80"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturna80"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "80",
                                Diurno = row["percentualextra80"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturna80"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );

                        }

                        if (row["percentualextra90"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturna90"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "90",
                                Diurno = row["percentualextra90"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturna90"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );

                        }

                        if (row["percentualextra100"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturna100"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "100",
                                Diurno = row["percentualextra100"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturna100"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );
                        }

                        if (row["percentualextrasab"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturnasab"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Sábado",
                                Diurno = row["percentualextrasab"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturnasab"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );
                        }

                        if (row["percentualextradom"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturnadom"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Domingo",
                                Diurno = row["percentualextradom"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturnadom"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );
                        }

                        if (row["percentualextrafer"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturnafer"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Feriado",
                                Diurno = row["percentualextrafer"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturnafer"].ToString().Replace(",", "").Replace(".", "")
                            }
                            );
                        }

                        if (row["percentualextrafol"].ToString().Replace(",", "").Replace(".", "") != "000" || row["percentualextraNoturnafol"].ToString().Replace(",", "").Replace(".", "") != "000")
                        {
                            percentuais.Add(new Percentual()
                            {
                                VlrPercentual = "Folga",
                                Diurno = row["percentualextrafol"].ToString().Replace(",", "").Replace(".", ""),
                                Noturno = row["percentualextraNoturnafol"].ToString().Replace(",", "").Replace(".", "")
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
