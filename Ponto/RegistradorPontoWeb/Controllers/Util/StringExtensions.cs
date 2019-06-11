using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Controllers.Util
{
    static class StringExtensions
    {
        public static IEnumerable<string> SplitOnLength(this string input, int length)
        {
            int index = 0;
            while (index < input.Length)
            {
                if (index + length < input.Length)
                    yield return input.Substring(index, length);
                else
                    yield return input.Substring(index);

                index += length;
            }
        }
    }
}