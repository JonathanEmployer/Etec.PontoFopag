using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RegistradorBiometrico.Model.Util
{
    public static class Extensoes
    {
        #region Criação DataTable

        public static DataTable ToDataTable<T>(this T item)
        {
            DataTable table = new DataTable();
            try
            {
                table = CreateTable<T>();
                Type entityType = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }
            catch
            {
                table = new DataTable();
            }

            return table;
        }

        private static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        } 

        #endregion


        #region Criação HTML

        public static string ToHtml<T>(this T objeto, String titulo)
        {
            StringBuilder sbRetorno = new StringBuilder();
            try
            {
                if (objeto != null)
                {
                    sbRetorno.Append("<table>");

                    sbRetorno.Append(CriaTagHeader(titulo));
                    sbRetorno.Append(CriaTagCorpo<T>(objeto));

                    sbRetorno.Append("</table>");
                }
            }
            catch
            {
                sbRetorno = new StringBuilder();
            }

            return sbRetorno.ToString();
        }

        private static String CriaTagHeader(String titulo)
        {
            StringBuilder sbRetorno = new StringBuilder();

            try
            {
                sbRetorno.Append("<thead>");
                sbRetorno.Append(String.Concat("<center><h2>", titulo, "</h2></center>"));
                sbRetorno.Append("</thead>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbRetorno.ToString();
        }

        private static String CriaTagCorpo<T>(T objeto)
        {
            StringBuilder sbRetorno = new StringBuilder();

            try
            {
                sbRetorno.Append("<tbody>");

                foreach (var item in objeto.GetType().GetProperties())
                    sbRetorno.Append(CriaLinha<T>(objeto, item));

                sbRetorno.Append("</tbody>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbRetorno.ToString();
        }

        private static string CriaLinha<T>(T objeto, PropertyInfo item)
        {
            StringBuilder sbRetorno = new StringBuilder();
            String nome, valor;
            try
            {
                nome = TentaBuscarDisplayName(item);
                valor = Convert.ToString(item.GetValue(objeto, null));

                sbRetorno.Append("<tr>");

                sbRetorno.Append(CriaCelula(nome, true));
                sbRetorno.Append(CriaCelula(valor, false));

                sbRetorno.Append("</tr>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbRetorno.ToString();
        }

        private static string CriaCelula(String valor, Boolean bNegrito)
        {
            StringBuilder sbRetorno = new StringBuilder();
            try
            {
                sbRetorno.Append("<td>");

                if (bNegrito)
                    sbRetorno.Append("<b>" + valor + "</b>");
                else
                    sbRetorno.Append(valor);

                sbRetorno.Append("</td>");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sbRetorno.ToString();
        }

        #endregion

        private static string TentaBuscarDisplayName(PropertyInfo item)
        {
            String nome;

            try
            {
                var attribute = item.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single();
                nome = attribute.DisplayName;
            }
            catch (Exception)
            {
                nome = item.Name;
            }

            nome += ":";

            return nome;
        } 
    }
}
