using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistradorBiometrico.Model
{
    public class Fusos
    {
        public String ID { get; set; }
        public TimeZoneInfo ObjTimeZoneInfo { get; set; }
        public Int32 Indice { get; set; }

        public Fusos(String pID, TimeZoneInfo pObjTimeZoneInfo, Int32 pIndice)
        {
            ID = pID;
            ObjTimeZoneInfo = pObjTimeZoneInfo;
            Indice = pIndice;
        }

        public static List<Fusos> FusosSistema
        {
            get
            {
                return ListaFusos();
            }
        }

        private static List<Fusos> ListaFusos()
        {
            List<Fusos> ListaFusos = new List<Fusos>();
            Int32 indice = 0;

            TimeZoneInfo.GetSystemTimeZones().ToList().
                ForEach(timeZone => 
                {
                    ListaFusos.Add(new Fusos(timeZone.Id, timeZone, indice));
                    indice++;
                });

            return ListaFusos;
        }

        public override string ToString()
        {
            return ObjTimeZoneInfo.ToString();
        }

    }
}
