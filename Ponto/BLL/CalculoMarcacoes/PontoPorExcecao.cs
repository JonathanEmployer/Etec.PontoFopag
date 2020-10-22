using DAL.SQL;
using Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.CalculoMarcacoes
{
    public class PontoPorExcecao
    {
        private string _connectionString;
        private DAL.SQL.BilhetesImp dalBilhetesImp;
        private Modelo.Cw_Usuario _usuarioLogado;
        private IFormatProvider _cultureInfoBr = new CultureInfo("pt-BR");
        public PontoPorExcecao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            _connectionString = connString;
            dalBilhetesImp = new DAL.SQL.BilhetesImp(new DataBase(connString));
            dalBilhetesImp.UsuarioLogado = usuarioLogado;
            _usuarioLogado = usuarioLogado;
        }

        //public List<Modelo.BilhetesImp> CriarBilhetesPontoPorExcecao(List<int> idsFuncs)
        //{
        //    List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao> registrosValidar = dalBilhetesImp.RegistrosValidarPontoExcecao(idsFuncs);

        //    int maxCodigo = dalBilhetesImp.MaxCodigo();
        //    List<Modelo.BilhetesImp> bilhetesInc = new List<Modelo.BilhetesImp>();
        //    //Adiciona os bilhetes do Ponto Por Exceção para os dias que tem jornada e estão sem bilhetes
        //    foreach (Modelo.Proxy.PxyRegistrosValidarPontoExcecao registro in registrosValidar.Where(w => w.Id == 0))
        //    {
        //        List<(string jornada, string entSai)> previstos = new List<(string jornada, string entSai)>();
        //        previstos.Add((registro.EntradaPrevista1,"E"));
        //        previstos.Add((registro.SaidaPrevista1, "S"));
        //        previstos.Add((registro.EntradaPrevista2, "E"));
        //        previstos.Add((registro.SaidaPrevista2, "S"));
        //        previstos.Add((registro.EntradaPrevista3, "E"));
        //        previstos.Add((registro.SaidaPrevista3, "S"));
        //        previstos.Add((registro.EntradaPrevista4, "E"));
        //        previstos.Add((registro.SaidaPrevista4, "S"));
        //        string jornadaAnt = "00:00";
        //        int posicao = 0;
        //        foreach ((string, string) previsto in previstos.Where(w => w.Item1 != "--:--"))
        //        {
        //            bilhetesInc.Add(new Modelo.BilhetesImp()
        //            {
        //                Acao = Acao.Incluir,
        //                Codigo = maxCodigo++,
        //                Data = Modelo.cwkFuncoes.ConvertBatidaMinuto(jornadaAnt) < Modelo.cwkFuncoes.ConvertBatidaMinuto(previsto.Item1) ? registro.Data : registro.Data.AddDays(1),
        //                Mar_data = registro.Data,
        //                Hora = previsto.Item1,
        //                Mar_hora = previsto.Item1,
        //                Relogio = "PE",
        //                Mar_relogio = "PE",
        //                IdFuncionario = registro.IdFuncionario,
        //                PIS = registro.PIS,
        //                DsCodigo = registro.DsCodigo,
        //                Func = registro.DsCodigo,
        //                Ent_sai = previsto.Item2,
        //                Importado = 0,
        //                Ordem = previsto.Item2 == "E" ? "010" : "011",
        //                Posicao = posicao++,
        //                Incdata = DateTime.Now.Date,
        //                Inchora = DateTime.Now,
        //                Altdata = null,
        //                Althora = null,
        //                Incusuario = _usuarioLogado.Login
        //            });
        //        }
        //    }

        //    dalBilhetesImp.InserirRegistros<Modelo.BilhetesImp>(bilhetesInc);
        //    return bilhetesInc;
        //}

        public List<Modelo.RegistroPonto> CriarRegistroPontoPorExcecao()
        {
            return CriarRegistroPontoPorExcecao(new List<int>(), new List<int>());
        }

        public List<Modelo.RegistroPonto> CriarRegistroPontoPorExcecao(List<int> idsFuncs, List<int> idsHorario)
        {
            RegistroPonto bllRegistroPonto = new RegistroPonto(_connectionString, _usuarioLogado);
            List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao> registrosValidar = dalBilhetesImp.RegistrosValidarPontoExcecao(idsFuncs, idsHorario);

            int maxCodigo = bllRegistroPonto.MaxCodigo();
            List<Modelo.RegistroPonto> registroPontos = new List<Modelo.RegistroPonto>();
            //Adiciona os bilhetes do Ponto Por Exceção para os dias que tem jornada e estão sem bilhetes
            foreach (Modelo.Proxy.PxyRegistrosValidarPontoExcecao registro in registrosValidar.Where(w => w.Id == 0).OrderBy(o => o.DataMarcacacao))
            {
                List<(string jornada, string entSai)> previstos = new List<(string jornada, string entSai)>();
                previstos.Add((registro.EntradaPrevista1, "E"));
                previstos.Add((registro.SaidaPrevista1, "S"));
                previstos.Add((registro.EntradaPrevista2, "E"));
                previstos.Add((registro.SaidaPrevista2, "S"));
                previstos.Add((registro.EntradaPrevista3, "E"));
                previstos.Add((registro.SaidaPrevista3, "S"));
                previstos.Add((registro.EntradaPrevista4, "E"));
                previstos.Add((registro.SaidaPrevista4, "S"));

                foreach ((string, string) previsto in previstos.Where(w => w.Item1 != "--:--"))
                {
                    registroPontos.Add(new Modelo.RegistroPonto()
                    {
                        Acao = Acao.Incluir,
                        Codigo = maxCodigo++,
                        Incdata = DateTime.Now.Date,
                        Inchora = DateTime.Now,
                        Incusuario = _usuarioLogado.Login,
                        Altdata = null,
                        Althora = null,
                        Batida = Convert.ToDateTime($"{registro.DataMarcacacao.ToString("dd/MM/yyyy")} {previsto.Item1}", _cultureInfoBr),
                        OrigemRegistro = "PE",
                        Situacao = "I",
                        IdFuncionario = registro.IdFuncionario
                    });
                }
            }

            bllRegistroPonto.InserirRegistros(registroPontos);
            return registroPontos;
        }
    }
}
