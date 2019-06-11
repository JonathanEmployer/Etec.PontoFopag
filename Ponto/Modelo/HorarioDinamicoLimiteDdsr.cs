using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class HorarioDinamicoLimiteDdsr : Modelo.ModeloBase
    {
        [Display(Name = "IdHorarioDinamico")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [DataTableAttribute()]
        public Int32 IdHorarioDinamico { get; set; }

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
        public bool Delete { get; set; }

    }
}
