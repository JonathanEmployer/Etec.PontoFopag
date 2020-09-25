using System;
using System.Globalization;

namespace PontoWeb.Utils
{
    public static class Encryption
    {
        public static string EncryptionXOR(this string Text, bool Encryption)
        {
            var Chave = "SG12L2NX33HSAÇ3FS36JGKP9D8XVNJTW788UFSÇFSASD5NM56E" +
                        "RSD427DFWE8FSA,4SDI8PO9FJ64KF3SP2FJSSLFHDSLKHGDLKH" +
                        "LASD27SDFE8FDS,SDFI8FD9SDF4SDFSSDFJHSHFHJSLKHGDLKG" +
                        "MSD427DFWE8FSA,4SVBNVOVBN64KF3SVNVBSSNFHDSSDHGDLKJ";

            int li = 0;
            int i = 0;
            string lCodificado = "";
            string lAux = "";

            if (Encryption)
            {
                for (li = 0; li < Text.Length; li++)
                {
                    if (i == Chave.Length)
                        i = 0;

                    lCodificado += string.Format("{0:X2}", (ushort)Text[li] ^ (ushort)Chave[i]);
                    i++;
                }
            }
            else
            {
                while (li < Text.Length)
                {
                    lAux += (char)Int16.Parse(Text.Substring(li, 2), NumberStyles.AllowHexSpecifier);
                    li += 2;
                }
                for (li = 0; li < lAux.Length; li++)
                {
                    if (i == Chave.Length)
                        i = 0;

                    lCodificado += (char)((ushort)lAux[li] ^ (ushort)Chave[i]);
                    if (li == Chave.Length)
                    {
                        Chave += Chave;
                    }
                    i++;
                }
            }
            return lCodificado;
        }
    }
}