using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class HorasInItinere
    {
        private string conn;
        private Modelo.Cw_Usuario usuarioLogado;
        public HorasInItinere(string conn, Modelo.Cw_Usuario usuarioLogado)
        {
            this.conn = conn;
            this.usuarioLogado = usuarioLogado;
        }

        public IList<PxyRelatorioHorasInItinere> BuscaDadosRelatorio(IList<int> idsFuncs, DateTime dtIni, DateTime dtFin, ProgressBar? objProgressBar)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usuarioLogado);
            BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usuarioLogado);
            BLL.Jornada bllJornada = new BLL.Jornada(conn, usuarioLogado);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, usuarioLogado);
            IList<PxyRelatorioHorasInItinere> rels = new List<PxyRelatorioHorasInItinere>();

            try
            {
                if (objProgressBar != null)
                {
                    objProgressBar.GetValueOrDefault().setaMensagem("Carregando dados do(s) " + idsFuncs.Count() + " funcionário(s)");
                }
                IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> funcs = bllFuncionario.GetFuncionariosCabecalhoRel(idsFuncs);
               
                IList<Horario> horario = new List<Horario>();

                int progress = 0;
                int totalCalc = funcs.Count();
                if (objProgressBar != null)
                {
                    objProgressBar.GetValueOrDefault().setaMinMaxPB(0, totalCalc);
                    objProgressBar.GetValueOrDefault().setaValorPB(progress);
                }
                foreach (Modelo.Proxy.PxyFuncionarioCabecalhoRel func in funcs.OrderBy(o => o.EmpresaNome).ThenBy(o => o.Nome))
                {
                    progress++;
                    if (objProgressBar != null)
                    {
                        objProgressBar.GetValueOrDefault().setaValorPB(progress);
                    }

                    PxyRelatorioHorasInItinere rel = new PxyRelatorioHorasInItinere();
                    rel.PxyFuncionarioCabecalhoRel = func; 
                    List<Modelo.Marcacao> marcs = bllMarcacao.GetCartaoPontoV2(new List<int> { func.IdFunc }, dtIni, dtFin);
                    marcs = marcs.Where(w => (w.InItinereHrsDentroJornada != "00:00" && w.InItinereHrsDentroJornada != "--:--" && w.InItinereHrsDentroJornada != null) || (w.InItinereHrsForaJornada != "00:00" && w.InItinereHrsForaJornada != "--:--" && w.InItinereHrsForaJornada != null)).ToList();

                    IList<Modelo.Jornada> jornadas = new List<Modelo.Jornada>();

                    DadosCartao(dtIni, dtFin, rel);

                    foreach (Modelo.Marcacao marc in marcs)
                    {
                        if (objProgressBar != null)
                        {
                            objProgressBar.GetValueOrDefault().setaMensagem("(" + progress.ToString() + "/" + totalCalc.ToString() + ") " + "Calculando dados do funcionário: " + func.Nome);
                        }
                        JornadasFunc(bllJornada, rel, jornadas, marc);
                    }

                    rel.Marcacoes = marcs;
                    SetaPadroesParaRel(rel);
                    if (marcs.Count > 0)
                    {
                        rels.Add(rel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rels;
        }

        private static void SetaPadroesParaRel(PxyRelatorioHorasInItinere cp)
        {
            if (cp.Jornadas.Count() == 0)
            {
                cp.Jornadas.Add(new Modelo.Jornada());
            }
        }

        private static void DadosCartao(DateTime dtIni, DateTime dtFin, PxyRelatorioHorasInItinere cp)
        {
            cp.Periodo = dtIni.ToShortDateString() + " a " + dtFin.ToShortDateString();
            cp.Jornadas = new List<Modelo.Jornada>();
        }

        private static void JornadasFunc(BLL.Jornada bllJornada, PxyRelatorioHorasInItinere rel, IList<Modelo.Jornada> jornadas, Modelo.Marcacao marc)
        {
            if (String.IsNullOrEmpty(marc.JornadaSTR))
            {
                Modelo.Jornada j = rel.Jornadas.Where(x => x.Id == marc.IdJornada).FirstOrDefault();
                if (j == null || j.Id < 0)
                {
                    j = jornadas.Where(x => x.Id == marc.IdJornada).FirstOrDefault();
                    if (j == null || j.Id < 0)
                    {
                        j = bllJornada.LoadObject(marc.IdJornada);
                        jornadas.Add(j);
                        rel.Jornadas.Add(j);
                    }
                    else
                    {
                        rel.Jornadas.Add(j);
                    }
                }
                if (j != null && j.Id > 0)
                {
                    if (j.Entrada_1 != "--:--")
                    {
                        marc.JornadaSTR = j.Entrada_1 + "-" + j.Saida_1;
                    }
                    if (j.Entrada_2 != "--:--")
                    {
                        marc.JornadaSTR += "-" + j.Entrada_2 + "-" + j.Saida_2;
                    }
                    if (j.Entrada_3 != "--:--")
                    {
                        marc.JornadaSTR += "-" + j.Entrada_3 + "-" + j.Saida_3;
                    }
                    if (j.Entrada_4 != "--:--")
                    {
                        marc.JornadaSTR += "-" + j.Entrada_4 + "-" + j.Saida_4;
                    }
                }
            }
        }
    }
}
