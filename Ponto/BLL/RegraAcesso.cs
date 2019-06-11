using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class RegraAcesso
    {
        private DAL.SQL.MarcacaoAcesso dalMarcacaoAcesso;
        private string ConnectionString;

        public RegraAcesso()
            : this(null)
        {

        }

        public RegraAcesso(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalMarcacaoAcesso = new DAL.SQL.MarcacaoAcesso(new DataBase(ConnectionString));
        }

        public bool VerificaAcesso(Modelo.Funcionario objFuncionario, DateTime pData, byte pHora, byte pMinuto, byte pTipo)
        {
            if (objFuncionario.TipoTickets == -1)   //Horario
            {
                return VerificaAcessoPorHorario(objFuncionario, pData, pHora, pMinuto);
            }
            else                                    //Ticket
            {
                int qtdAcesso = VerificaAcessoPorTickets(objFuncionario, pData);

                if (qtdAcesso < (int)objFuncionario.QuantidadeTickets || pTipo == 0)
                    return true;
                else
                    return false;
            }
        }

        private bool VerificaAcessoPorHorario(Modelo.Funcionario objFuncionario, DateTime pData, byte pHora, byte pMinuto)
        {
            BLL.Horario bllHorario = new BLL.Horario(ConnectionString);

            Modelo.Horario objHorario = bllHorario.LoadObject(objFuncionario.Idhorario);
            int dia = Modelo.cwkFuncoes.Dia(pData);

            string entrada = "--:--";
            int entradaMinuto = 0;
            string saida = "--:--";
            int saidaMinuto = 0;

            if (objHorario.TipoHorario == 1)
            {
                entrada = BLL.CalculoHoras.OperacaoHoras('-', objHorario.HorariosDetalhe[dia - 1].Saida_1, objFuncionario.ToleranciaEntrada);
                saida = BLL.CalculoHoras.OperacaoHoras('+', objHorario.HorariosDetalhe[dia - 1].Entrada_2, objFuncionario.ToleranciaSaida);
            }
            else
            {
                Modelo.HorarioDetalhe objHorarioDetalhe = objHorario.HorariosFlexiveis.Where(w => w.Data == pData).First();
                if (objHorarioDetalhe == null)
                    return false;

                entrada = BLL.CalculoHoras.OperacaoHoras('-', objHorarioDetalhe.Saida_1, objFuncionario.ToleranciaEntrada);
                saida = BLL.CalculoHoras.OperacaoHoras('+', objHorarioDetalhe.Entrada_2, objFuncionario.ToleranciaSaida);
            }

            entradaMinuto = Modelo.cwkFuncoes.ConvertHorasMinuto(entrada);
            saidaMinuto = Modelo.cwkFuncoes.ConvertHorasMinuto(saida);

            int horarioBatida = (pHora * 60) + pMinuto;

            if (horarioBatida >= entradaMinuto && horarioBatida <= saidaMinuto)
                return true;
            else
                return false;
        }

        private int VerificaAcessoPorTickets(Modelo.Funcionario objFuncionario, DateTime pData)
        {
            DateTime dataInicial;
            DateTime dataFinal;

            switch (objFuncionario.TipoTickets)
            {
                case 0: //Diário
                    dataInicial = pData;
                    dataFinal = pData;
                    break;

                case 1: //Semanal
                    int dia = Modelo.cwkFuncoes.Dia(pData);
                    dataInicial = pData.AddDays((dia - 1) * -1);
                    dataFinal = pData.AddDays(7 - dia);
                    break;

                case 2: //Mensal
                    dataInicial = Convert.ToDateTime("01/" + pData.Month + "/" + pData.Year);
                    dataFinal = Convert.ToDateTime("01/" + (pData.Month + 1) + "/" + pData.Year).AddDays(-1);
                    break;

                default:
                    return 0;
            }

            return dalMarcacaoAcesso.GetQuantidadeAcessoTipoTicket(objFuncionario, dataInicial, dataFinal);
        }
    }
}
