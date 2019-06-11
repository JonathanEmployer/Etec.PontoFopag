using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Modelo
{
    public class LimiteDDsr : ModeloBase
    {
        private DateTime dtLimitePerdaDsr;
        public virtual DateTime DTLimitePerdaDsr 
        {
            get
            {
                DateTime dtAtual = DateTime.Now.Date;
                string[] dthora = new string[2] { "00", "00" };
                if (LimitePerdaDsr != null)
                {
                    dthora = new string[2];
                    dthora = LimitePerdaDsr.Split(':');
                }

                return new DateTime(dtAtual.Year, dtAtual.Month, dtAtual.Day, Convert.ToInt32(dthora[0]), Convert.ToInt32(dthora[1]), 0);
            }
            set
            {
                dtLimitePerdaDsr = value;
                LimitePerdaDsr = dtLimitePerdaDsr.ToString("HH:mm");
            }
        }

        private DateTime dtQtdHorasDsr; 
        public virtual DateTime DTQtdHorasDsr
        {
            get
            {
                DateTime dtAtual = DateTime.Now.Date;
                string[] dthora = new string[2] { "00", "00" };
                if (QtdHorasDsr != null)
                {
                    dthora = new string[2];
                    dthora = QtdHorasDsr.Split(':');
                }

                return new DateTime(dtAtual.Year, dtAtual.Month, dtAtual.Day, Convert.ToInt32(dthora[0]), Convert.ToInt32(dthora[1]), 0);
            }
            set
            {
                dtQtdHorasDsr = value;
                QtdHorasDsr = dtQtdHorasDsr.ToShortTimeString();
            }
        }

        [Display(Name = "Limite para Perda de DSR")]
        [DataTableAttribute()]
        public string LimitePerdaDsr { get; set; }

        [Display(Name = "Qtd. Horas DSR")]
        [DataTableAttribute()]
        public string QtdHorasDsr { get; set; }

        private double DevolveMinutos(string horarioString)
        {
            double retorno = 0;
            double horas, minutos;
            string horasStr, minutosStr;
            string[] listaValores;

            if (!String.IsNullOrEmpty(horarioString))
            {
                listaValores = horarioString.Split(':');

                if (listaValores.Length > 0)
                {
                    horasStr = listaValores.First().Trim();
                    minutosStr = listaValores.Last().Trim();

                    horas = Convert.ToDouble(horasStr);
                    minutos = Convert.ToDouble(minutosStr);

                    retorno = (horas * 60) + minutos;
                }
            }

            return retorno;
        }

        [DataTableAttribute()]
        public int IdHorario { get; set; }

        [Display(Name = "Horário")]
        public string Horario { get; set; }
        public bool Delete { get; set; }
    }
}
