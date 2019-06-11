using Modelo.Utils;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BLL.Util
{
    public class ListaObjetosToExcel
    {
        public byte[] ObjectToExcel<T>(string nomeAba, List<T> objs)
        {
            Byte[] arquivo = null;
            using (var p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = nomeAba;

                //Create a sheet
                p.Workbook.Worksheets.Add(nomeAba);

                var sheet = p.Workbook.Worksheets[1];


                var row = 1; // 1 is the header.
                foreach (var item in objs)
                {
                    if (item != null)
                    {
                        List<ItemsForExport> itemsForExport = GetPropertiesForExport(item.GetType());
                        var col = 1;
                        if (row == 1)
                        {

                            foreach (var itemForExport in itemsForExport)
                            {
                                sheet.Cells[1, col].Value = itemForExport.Title;
                                sheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                sheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                                col++;
                            }
                            row++;
                            col = 1;
                        }

                        foreach (var itemForExport in itemsForExport)
                        {
                            var pi = item.GetType().GetProperty(itemForExport.PropertyName);
                            sheet.Cells[row, col].Value = pi.GetValue(item);
                            if (pi.PropertyType.FullName.Contains("DateTime"))
                            {
                                sheet.Cells[row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                            }
                            col++;
                        } 
                    }
                    row++;
                }
                sheet.Cells.AutoFitColumns();
                arquivo = p.GetAsByteArray();
            }
            return arquivo;
        }

        private List<ItemsForExport> GetPropertiesForExport(Type type)
        {
            const BindingFlags allProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var itemsToExport = new List<ItemsForExport>();

            foreach (var property in type.GetProperties(allProperties))
            {
                var exportToXlsAttributes = property.GetCustomAttributes(typeof(ExportToXlsAttribute)).ToList();

                if (exportToXlsAttributes.Any())
                {
                    var displayName = property.Name;
                    var attribute = (ExportToXlsAttribute)exportToXlsAttributes.First();

                    if (!string.IsNullOrEmpty(attribute.Description))
                    {
                        displayName = attribute.Description;
                    }
                    itemsToExport.Add(new ItemsForExport { Index = attribute.Index, PropertyName = property.Name, Title = displayName });
                }
            }

            return itemsToExport.OrderBy(a => a.Index).ToList();
        }
    }
}