using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TimeZoneConverter;

namespace RegistradorPontoWeb.Controllers.Util
{
    public static class DataHora
    {
        public static string TimeZoneIANA(string lat, string lon)
        {
            string timeZoneIANA = TimeZoneLookup.GetTimeZone(Double.Parse(lat, CultureInfo.InvariantCulture), Double.Parse(lon, CultureInfo.InvariantCulture)).Result;
            return timeZoneIANA;
        }

        public static DateTime DataHoraPorTimeZoneIANA(string timeZoneIANA)
        {
            string idTimeZone = TZConvert.IanaToWindows(timeZoneIANA);
            DateTime dataHoraTimeZone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, idTimeZone);
            return dataHoraTimeZone;
        }
    }
}