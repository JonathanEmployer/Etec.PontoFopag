using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PontoWeb.Utils
{
    public class GetPropertiesTableHTML
    {
        public static TableHTML GetProperties(string tableName, bool multipleSelect, string controllerDados, string acaoDados, Type type)
        {
            TableHTML tableHtml = new TableHTML(tableName, multipleSelect, controllerDados, acaoDados);
            const BindingFlags allProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var itemsToTableHTML = new List<ItemsForTable>();

            foreach (var property in type.GetProperties(allProperties))
            {
                var exportTableHTMLAttribute = property.GetCustomAttributes(typeof(TableHTMLAttribute)).ToList();

                if (exportTableHTMLAttribute.Any())
                {
                    var displayName = property.Name;
                    var attribute = (TableHTMLAttribute)exportTableHTMLAttribute.First();

                    if (!string.IsNullOrEmpty(attribute.Description))
                    {
                        displayName = attribute.Description;
                    }

                    var tipoColuna = "";

                    if ((attribute.ColumnType == ColumnType.nenhum && attribute.Description.Contains("Data")) || (attribute.ColumnType == ColumnType.data))
                    {
                        tipoColuna = "";
                    }
                    else if(attribute.ColumnType == ColumnType.texto || (property.PropertyType == typeof(string) && attribute.ColumnType == ColumnType.nenhum))
                    {
                        tipoColuna = "portugues";
                    }
                    else if(attribute.ColumnType == ColumnType.automatico)
                    {
                        tipoColuna = "";
                    } 
                    else
                    {
                        tipoColuna = "";
                    }

                    itemsToTableHTML.Add(new ItemsForTable { Index = attribute.Index, Description = displayName, PropertyName = property.Name, Visible = attribute.Visible, Search = attribute.Search, Ordenacao = attribute.Ordenacao, ColumnType = tipoColuna });
                }
            }
            tableHtml.Columns = itemsToTableHTML.OrderBy(a => a.Index).ToList();
            return tableHtml;
        }
    }
}