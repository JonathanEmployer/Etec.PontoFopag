using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Utils
{
    public class CPF_CNPJ
    {

        public static string FormatCNPJ(string CNPJ)
        {
            if (CNPJ.Contains("."))
            {
                return CNPJ;
            }
            else
            {
                return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
            }
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        public static string FormatCPF(string CPF)
        {
            if (CPF.Contains("."))
            {
                return CPF;
            }
            else
            {
                return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
            }
        }
        /// <summary>
        /// Retira a Formatacao de uma string CNPJ/CPF
        /// </summary>
        /// <param name="Codigo">string Codigo Formatada</param>
        /// <returns>string sem formatacao</returns>
        /// <example>Recebe '99.999.999/9999-99' Devolve '99999999999999'</example>

        public static string RemoveFormatacaoCPF_CNPJFormatacao(string Codigo)
        {
            return Codigo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }

        /// <summary>
        /// Método para validar um CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado</param>
        /// <returns>caso o CNPJ seja válido, o retorno é verdadeiro, caso contrário o retorno é falso</returns>
        internal static bool ValidaCnpj(string cnpj)
        {
            if (cnpj == null)
            {
                return false;
            }

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;

            int resto;

            string digito;

            string tempCnpj;

            cnpj = cnpj.Trim();

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace("_", "");

            if (cnpj.Length != 14)

                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;

            for (int i = 0; i < 12; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;

            for (int i = 0; i < 13; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }

        /// <summary>
        /// Método que verifica se determinada string representa um CPF válido.
        /// </summary>
        /// <param name="cpf">String a ser verificada.</param>
        /// <returns>Caso seja um cpf válido, retorna verdadeiro, caso contrário retorna falso.</returns>
        internal static bool ValidaCpf(string cpf)
        {
            if (cpf == null)
            {
                return false;
            }

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;

            string digito;

            int soma;

            int resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "").Replace("_", "");

            if (cpf.Length != 11)

                return false;

            tempCpf = cpf.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);

        }

        /// <summary>
        /// Método para validar a CEI - Cadastro Específico do INSS
        /// </summary>
        /// <param name="cei">CEI a ser validado</param>
        /// <returns>caso o CEI seja válido, o retorno é verdadeiro, caso contrário o retorno é falso</returns>
        internal static bool ValidaCei(string cei)
        {
            if (cei == null)
                return false;

            int[] multiplicador = new int[11] { 7, 4, 1, 8, 5, 2, 1, 6, 3, 7, 4 };

            int soma = 0, total = 0;

            cei = cei.Trim();
            cei = cei.Replace(".", "").Replace("/", "");

            if (cei.Length != 12)
                return false;

            string tempCei = cei.Substring(0, 11);

            for (int i = 0; i < 11; i++)
            {
                if (tempCei[i].ToString() == "_")
                {
                    return false;
                }
                soma += (Convert.ToInt16(tempCei[i].ToString()) * multiplicador[i]);
            }

            string strSoma = soma.ToString();

            total = int.Parse(strSoma[strSoma.Length - 2].ToString()) + int.Parse(strSoma[strSoma.Length - 1].ToString());

            string strTotal = total.ToString();

            string strResultado = strTotal[strTotal.Length - 1].ToString();
            strResultado = (10 - Convert.ToInt16(strResultado)).ToString();

            if (cei[11].ToString() == strResultado)
                return true;
            else
                return false;
        }
    }
}
