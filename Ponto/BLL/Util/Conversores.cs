using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace BLL.Util
{
    public static class Conversores
    {
        public static SelectList ToSelectList(this DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public static SelectList ToSelectList2(this DataTable table, string textField, string valueField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Value = row[valueField].ToString(),
                    Text = row[textField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public static DataTable ToDataTable(List<Dictionary<string, int>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();
            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, int> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }

            return result;
        }

        public static List<T> ConvertTo<T>(this DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                    columnsNames.Add(DataColumn.ColumnName);
                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }

        public static DataTable ToDataTable<T>(IList<T> items)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyInfo[] Properties;
            Properties = typeof(T).GetProperties().Where(w => !w.PropertyType.Name.Contains("List")).ToArray();

            foreach (T item in items)
            {
                DataRow row = table.NewRow();

                foreach (var prop in Properties)
                {
                    var attrDisplayType = typeof(DisplayAttribute);
                    DisplayAttribute customAtribute = (DisplayAttribute)prop.GetCustomAttribute(attrDisplayType, true);
                    row[customAtribute == null ? prop.Name : customAtribute.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string customColumnName = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();

                foreach (PropertyInfo objProperty in Properties)
                {
                    var attrDisplayType = typeof(DisplayAttribute);

                    DisplayAttribute customAtribute = (DisplayAttribute)objProperty.GetCustomAttribute(attrDisplayType, true);
                    customColumnName = columnsName.Find(name => name.ToLower() == customAtribute.Name.ToLower());

                    if (!string.IsNullOrEmpty(customColumnName))
                    {
                        value = row[customColumnName].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[customColumnName].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[customColumnName].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }

        private static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);

            PropertyInfo[] Properties;
            Properties = typeof(T).GetProperties().Where(w => !w.PropertyType.Name.Contains("List")).ToArray();

            foreach (var prop in Properties)
            {
                var attrDisplayType = typeof(DisplayAttribute);
                DisplayAttribute customAtribute = (DisplayAttribute)prop.GetCustomAttribute(attrDisplayType, true);

                table.Columns.Add((customAtribute == null ? prop.Name : customAtribute.Name), Nullable.GetUnderlyingType(Type.GetType(prop.PropertyType.ToString())) ?? Type.GetType(prop.PropertyType.ToString()));
            }

            return table;
        }

        public static string RemoveAcentosECaracteresEspeciais(string texto)
        {
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string ficou = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            return Regex.Replace(ficou, "[^0-9a-zA-Z ]+", "", RegexOptions.IgnoreCase);
        }

        public static List<List<T>> ListaGroupBy<T>(this IList<T> lista, Func<T, int> keySelector) where T : new()
        {
            List<List<T>> lAux = new List<List<T>>();

            var p = lista.GroupBy(keySelector).ToList();

            p.ForEach((x) => { lAux.Add(x.ToList()); });

            return lAux;
        }

        public static string dictionayToJson<T>(this IEnumerable<IGrouping<int, T>> grupo, Func<IGrouping<int, T>, int> keySelector, Func<IGrouping<int, T>, IList<T>> ElementSelector) where T : new()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(grupo.ToDictionary(keySelector, ElementSelector));
        }
    }
}
