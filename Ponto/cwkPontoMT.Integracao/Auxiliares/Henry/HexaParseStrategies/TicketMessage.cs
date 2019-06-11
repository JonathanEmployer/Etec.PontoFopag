using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public class TicketMessage: IMessage
    {
        public int Qtd
        {
            get
            {
                return Tickets.Count;
            }
        }
        private List<Ticket> _Tickets;

        public List<Ticket> Tickets
        {
            get { return _Tickets; }
            set { _Tickets = value; }
        }

        public int MinNSR
        {
            get
            {
                if (Tickets.Count > 0)
                {
                    return Tickets.Min(m => m.NSR);
                }
                return 0;
            }
        }
        
        public int MaxNSR
        {
            get
            {
                if (Tickets.Count > 0)
                {
                    return Tickets.Max(m => m.NSR);
                }
                return 0;
            }
        }
        

        public TicketMessage(string retornoRep)
        {
            try
            {
                Tickets = new List<Ticket>();
                List<String> retSplit = retornoRep.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string item in retSplit)
                {
                    string itemFiltrado = GetStringSomenteAlfanumerico(item);
                    if (!String.IsNullOrEmpty(itemFiltrado))
                    {
                        var t = new Ticket(item);
                        Tickets.Add(t); 
                    }
                }
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
        }

        public TicketMessage()
        {
            Tickets = new List<Ticket>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Tickets)
            {
                sb.AppendLine(item.TicketCompleto);
            }
            return sb.ToString();
        }

        private string GetStringSomenteAlfanumerico(string s)
        {
            string r = String.Empty;
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            try
            {
                r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                                      char.IsWhiteSpace(c) ||
                                                                      c == ','))
                                                                      .ToArray());
            }
            catch (Exception)
            {
                r = String.Empty;
            }
            return r;
        }
    }
}
