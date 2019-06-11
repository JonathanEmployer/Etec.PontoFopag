using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BLL.Util
{
    public class GetPropertiesCartaoPontoCustom
    {
        public static List<CartaoPontoCamposParaCustomizacao> GetProperties(Type type)
        {
            const BindingFlags allProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var items = new List<CartaoPontoCamposParaCustomizacao>();

            foreach (var property in type.GetProperties(allProperties))
            {
                var exportToXlsAttributes = property.GetCustomAttributes(typeof(CartaoPontoCustomInfo)).ToList();

                if (exportToXlsAttributes.Any())
                {
                    var displayName = property.Name;
                    var attribute = (CartaoPontoCustomInfo)exportToXlsAttributes.First();

                    if (!string.IsNullOrEmpty(attribute.Description))
                    {
                        displayName = attribute.Description;
                    }
                    items.Add(new CartaoPontoCamposParaCustomizacao { Header = attribute.Header, NomePropriedade = property.Name, Descricao = attribute.Descricao, TamanhoPX = attribute.TamanhoPX, Somar = attribute.Somar });
                }
            }

            return items.OrderBy(a => a.Descricao).ToList();
        }
    }
}