using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public class Ticket
    {
        private string _TicketCompleto;

        public string TicketCompleto
        {
            get { return _TicketCompleto; }
            set { _TicketCompleto = value; }
        }

        private int _NSR;

        public int NSR
        {
            get { return _NSR; }
            set { _NSR = value; }
        }

        private int _TipoTicket;

        public int TipoTicket
        {
            get { return _TipoTicket; }
            set { _TipoTicket = value; }
        }

        private DateTime _DataHora;

        public DateTime DataHora
        {
            get { return _DataHora; }
            set { _DataHora = value; }
        }

        private string _Dados;

        public string Dados
        {
            get { return _Dados; }
            set { _Dados = value; }
        }

        private string _CRC16;

        public string CRC16
        {
            get { return _CRC16; }
            set { _CRC16 = value; }
        }


        public Ticket(string ticketCompleto)
        {
            if (string.IsNullOrEmpty(ticketCompleto))
            {
                throw new ArgumentException("Ticket mal-formado");
            }
            TicketCompleto = GetStringSomenteAlfanumerico(ticketCompleto).Replace("+", "");
            try
            {
                NSR = Convert.ToInt32(TicketCompleto.Substring(0, 9));
                TipoTicket = Convert.ToInt32(TicketCompleto.Substring(9, 1));
                CultureInfo ptBR = new CultureInfo("pt-BR");
                DateTime dtOut;
                string strData = TicketCompleto.Substring(10, 12);
                if (DateTime.TryParseExact(strData, "ddMMyyyyHHmm", ptBR, DateTimeStyles.AllowWhiteSpaces, out dtOut))
                {
                    DataHora = dtOut;
                }
                Dados = TicketCompleto.Substring(22, TicketCompleto.Length - 22);
                CRC16 = TicketCompleto.Substring(TicketCompleto.Length - 4, 4);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Ticket mal-formado", e);
            }
        }

        private static string GetStringSomenteAlfanumerico(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            string r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                              char.IsWhiteSpace(c) ||
                                                              c == '+' ||
                                                              c == ',' ||
                                                              c == '/' ||
                                                              c == ':' ||
                                                              c == ']'))
                                                              .ToArray());
            return r;
        }

        public override string ToString()
        {
            return _TicketCompleto.Substring(0, _TicketCompleto.Length -4);
        }
    }
}
