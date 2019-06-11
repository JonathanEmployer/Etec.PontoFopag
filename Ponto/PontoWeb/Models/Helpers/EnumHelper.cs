using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelo;

namespace cwkPontoWeb.Models.Helpers
{
    public static class EnumHelper
    {
        public static SelectList GetSelectedItemList<T>(int contador) where T : struct
        {
            T t = default(T);
            if (!t.GetType().IsEnum)
            {
                throw new ArgumentNullException("Tipo de dados incompatível com Enum");
            }

            var nomeLista = t.GetType().GetEnumNames();

            int cont = contador;

            Dictionary<int, String> dict = new Dictionary<int,string>();
            if (nomeLista != null && nomeLista.Length > 0)
            {
                foreach (var nome in nomeLista)
                {
                    T newEnum = (T) Enum.Parse(t.GetType(), nome);
                    string descricao = GetDescricaoDoEnum(newEnum as Enum);

                    if (!dict.ContainsKey(cont))
	                {
		                 dict.Add(cont, descricao);
	                }
                    cont++;
                }
                cont = 0;
                return new SelectList(dict,"Key","Value");
            }
            return null;
        }

        private static string GetDescricaoDoEnum(Enum value)
        {
            DescriptionAttribute descricaoAtributo = 
                value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return descricaoAtributo == null ? value.ToString() : descricaoAtributo.Description;
        }

        /// <summary>
        /// Recebe o indice de um Enum e retorna a Descrição do mesmo
        /// </summary>
        /// <param name="value">Índice do Enum </param>
        /// <returns>Descrição do Enum</returns>
        public static string GetDescricaoEnum(int value)
        {
            Listas.DiaDSR diaDSR = (Listas.DiaDSR)value;
            string stringValue = GetDescricaoDoEnum(diaDSR);
            return stringValue;
        }
    }
}