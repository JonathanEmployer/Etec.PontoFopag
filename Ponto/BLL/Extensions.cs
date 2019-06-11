using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Section<T>(this IEnumerable<T> source, int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length");

            var section = new List<T>(length);

            foreach (var item in source)
            {
                section.Add(item);

                if (section.Count == length)
                {
                    yield return section.AsReadOnly();
                    section = new List<T>(length);
                }
            }

            if (section.Count > 0)
                yield return section.AsReadOnly();
        }

        public static List<T> DataTableMapToList<T>(this DataTable dt)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            List<string> colunas = dt.Columns
                                     .Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToList();
            foreach (DataRow row in dt.Rows)
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (colunas.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        if (!object.Equals(row[prop.Name], DBNull.Value))
                        {
                            var valor = DAL.SQL.DALBase.ChangeType(row[prop.Name], prop.PropertyType);
                            prop.SetValue(obj, valor, null);
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Retorna a primeira data anterior após o ultimo DSR
        /// </summary>
        /// <param name="diaBase">data Base</param>
        /// <param name="DSR">Dia do DSR</param>
        /// <returns>Retorna a primeira data anterior após o ultimo DSR</returns>
        public static DateTime GetInicioSemanaBasedadoDSR(this DateTime diaBase, DayOfWeek DSR)
        {
            int currentDay = (int)diaBase.DayOfWeek, diaDsr = (int)DSR;
            DateTime inicioSemana;
            if (diaDsr - currentDay < 0)
            {
                inicioSemana = diaBase.AddDays(1).AddDays(diaDsr - currentDay);
            }
            else
            {
                inicioSemana = diaBase.AddDays(1).AddDays(-7);
            }
            return inicioSemana;
        }
    }
}
