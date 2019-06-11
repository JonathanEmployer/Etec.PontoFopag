using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DAL
{
    public static class Extensions
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            const BindingFlags allProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            Dictionary<string, string> ColumnDePara = new Dictionary<string, string>();
            Type tipoObjeto = data.FirstOrDefault().GetType();

            foreach (var property in tipoObjeto.GetProperties(allProperties))
            {
                var gerarColumn = property.GetCustomAttributes(typeof(Modelo.DataTableAttribute), true).ToList();

                if (gerarColumn.Any())
                {
                    var atributo = property.Name;
                    var nomeCollumn = atributo;
                    var attribute = (Modelo.DataTableAttribute)gerarColumn.First();

                    if (!string.IsNullOrEmpty(attribute.Description))
                    {
                        nomeCollumn = attribute.Description;
                    }
                    ColumnDePara.Add(atributo, nomeCollumn);
                }
            }

            if (ColumnDePara.Count == 0)
            {
                throw new Exception("Para utilizar o ToDataTable é necessário anotar os atributos da classe com a annotations DataTableAttribute");
            }

            DataTable table = new DataTable();
            foreach (KeyValuePair<string, string> propriedade in ColumnDePara)
            {
                PropertyInfo myPropInfo = tipoObjeto.GetProperty(propriedade.Key);
                table.Columns.Add(propriedade.Value, Nullable.GetUnderlyingType(
                myPropInfo.PropertyType) ?? myPropInfo.PropertyType);
            }

            object[] values = new object[ColumnDePara.Count];
            int count = 0;
            foreach (T item in data)
            {
                count = 0;
                foreach (KeyValuePair<string, string> propriedade in ColumnDePara)
                {
                    PropertyInfo myPropInfo = tipoObjeto.GetProperty(propriedade.Key);
                    values[count] = item.GetType().GetProperty(propriedade.Key).GetValue(item, null) ?? DBNull.Value;
                    count++;
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
