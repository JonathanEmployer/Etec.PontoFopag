using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public static class ClSeguranca
    {
        static string CONST_CHAVE = "19q#jcksw";

        public static string Criptografar(string dados)
        {
            string chave = CONST_CHAVE;
            byte[] b = Encoding.UTF8.GetBytes(dados);
            byte[] pw = Encoding.UTF8.GetBytes(chave);

            RijndaelManaged rm = new RijndaelManaged();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(chave, new MD5CryptoServiceProvider().ComputeHash(pw));
            rm.Key = pdb.GetBytes(32);
            rm.IV = pdb.GetBytes(16);
            rm.BlockSize = 128;
            rm.Padding = PaddingMode.PKCS7;

            MemoryStream ms = new MemoryStream();

            CryptoStream cryptStream = new CryptoStream(ms, rm.CreateEncryptor(rm.Key, rm.IV), CryptoStreamMode.Write);
            cryptStream.Write(b, 0, b.Length);
            cryptStream.FlushFinalBlock();


            return System.Convert.ToBase64String(ms.ToArray());

        }

        public static string Descriptografar(string sDados)
        {
            if (String.IsNullOrEmpty(sDados))
                return String.Empty;

            string chave = CONST_CHAVE;
            byte[] dados = System.Convert.FromBase64String(sDados);
            byte[] pw = Encoding.UTF8.GetBytes(chave);

            RijndaelManaged rm = new RijndaelManaged();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(chave, new MD5CryptoServiceProvider().ComputeHash(pw));
            rm.Key = pdb.GetBytes(32);
            rm.IV = pdb.GetBytes(16);
            rm.BlockSize = 128;
            rm.Padding = PaddingMode.PKCS7;

            MemoryStream ms = new MemoryStream(dados, 0, dados.Length);

            CryptoStream cryptStream = new CryptoStream(ms, rm.CreateDecryptor(rm.Key, rm.IV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cryptStream);

            return sr.ReadToEnd();
        }
    }
}