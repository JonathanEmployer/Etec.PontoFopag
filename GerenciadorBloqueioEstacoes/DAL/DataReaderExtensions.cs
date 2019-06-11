using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DataReaderExtensions
    {
        public static IEnumerable<T> MapearTodos<T>(this IDataReader reader, Func<IDataReader, T> conversor)
        {
            while (reader.Read())
                yield return conversor(reader);
        }

        public static T Mapear<T>(this IDataReader reader, Func<IDataReader, T> conversor)
        {
            if (reader.Read())
                return conversor(reader);
            return default(T);
        }

        public static DataTable ToDataTable(this IDataReader reader)
        {
            DataTable saida = new DataTable();
            saida.Load(reader);
            return saida;
        }
    }
}
