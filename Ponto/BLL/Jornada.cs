using System;
using System.Collections.Generic;
using System.Data;
using DAL.SQL;

namespace BLL
{
    public class Jornada : IBLL<Modelo.Jornada>
    {
        DAL.IJornada dalJornada;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Jornada() : this(null)
        {
            
        }

        public Jornada(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Jornada(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalJornada = new DAL.SQL.Jornada(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dalJornada.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalJornada.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalJornada.GetAll();
        }

        public List<Modelo.Jornada> GetAllList()
        {
            return dalJornada.GetAllList();
        }
        public List<Modelo.Jornada> GetAllList(List<int> idsJornadas)
        {
            return dalJornada.GetAllList(idsJornadas);
        }

        public Modelo.Jornada LoadObject(int id)
        {
            return dalJornada.LoadObject(id);
        }

        public List<Modelo.Jornada> getTodosHorariosDaEmpresa(int pIdEmpresa)
        {
            return dalJornada.getTodosHorariosDaEmpresa(pIdEmpresa);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Jornada objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            //if (objeto.Descricao == null)
            //{
            //    ret.Add("txtDescricao", "Campo obrigatório.");
            //}
            //else if (objeto.Descricao.TrimEnd() == String.Empty)
            //{
            //    ret.Add("txtDescricao", "Campo obrigatório.");
            //}

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Jornada objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalJornada.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalJornada.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalJornada.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalJornada.getId(pValor, pCampo, pValor2);
        }

        public bool JornadaExiste(Modelo.Jornada objJornada)
        {
            return dalJornada.JornadaExiste(objJornada);
        }

        public void AtualizaHorarios(Modelo.Jornada objJornada)
        {
            HorarioDetalhe bllHorarioDetalhe = new HorarioDetalhe(ConnectionString, UsuarioLogado);
            List<Modelo.HorarioDetalhe> horariosDetalhe = bllHorarioDetalhe.GetPorJornada(objJornada.Id);
            string totalD, totalN;
            foreach (Modelo.HorarioDetalhe hd in horariosDetalhe)
            {
                hd.Entrada_1 = objJornada.Entrada_1;
                hd.Saida_1 = objJornada.Saida_1;
                hd.Entrada_2 = objJornada.Entrada_2;
                hd.Saida_2 = objJornada.Saida_2;
                hd.Entrada_3 = objJornada.Entrada_3;
                hd.Saida_3 = objJornada.Saida_3;
                hd.Entrada_4 = objJornada.Entrada_4;
                hd.Saida_4 = objJornada.Saida_4;

                totalD = "";
                totalN = "";
                BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(hd.getEntradas(), hd.getSaidas(), hd.InicioAdNoturno, hd.FimAdNoturno, ref totalD, ref totalN);

                if (hd.Marcacargahorariamista == 1)
                {
                    hd.Cargahorariamista = BLL.CalculoHoras.OperacaoHoras('+', totalD, totalN);
                    hd.Totaltrabalhadadiurna = "--:--";
                    hd.Totaltrabalhadanoturna = "--:--";
                }
                else
                {
                    hd.Cargahorariamista = "--:--";
                    hd.Totaltrabalhadadiurna = (totalD != "00:00" ? totalD : "--:--");
                    hd.Totaltrabalhadanoturna = (totalN != "00:00" ? totalN : "--:--");
                }

                bllHorarioDetalhe.Salvar(Modelo.Acao.Alterar, hd);
            }
        }

        public Modelo.Jornada LoadObjectCodigo(int codigo)
        {
            return dalJornada.LoadObjectCodigo(codigo);
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoJornada(int id)
        {
            return dalJornada.FechamentoPontoJornada(id);
        }

        public void CalculaTrabalhadas(ref List<Modelo.Jornada> jornadas, string InicioAdNoturno, string FimAdNoturno)
        {
            foreach (Modelo.Jornada jornada in jornadas)
            {
                int HoraDiurna = 0;
                int HoraNoturna = 0;
                BLL.CalculoHoras.QtdHorasDiurnaNoturna(jornada.EntradasMin, jornada.SaidasMin, Modelo.cwkFuncoes.ConvertHorasMinuto(InicioAdNoturno), Modelo.cwkFuncoes.ConvertHorasMinuto(FimAdNoturno), ref HoraDiurna, ref HoraNoturna);
                jornada.TotalTrabalhadaDiurna = Modelo.cwkFuncoes.ConvertMinutosHora2(2, HoraDiurna);
                jornada.TotalTrabalhadaNoturna = Modelo.cwkFuncoes.ConvertMinutosHora2(2, HoraNoturna);
                jornada.TotalTrabalhada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, (HoraDiurna + HoraNoturna));
            }
        }

        public List<Modelo.Proxy.PxyIdPeriodo> GetFuncionariosRecalculo(int id)
        {
            return dalJornada.GetFuncionariosRecalculo(id);
        }
    }
   
}
