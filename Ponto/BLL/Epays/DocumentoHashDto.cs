using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Epays
{

    /// <summary>
    /// Importante! Classe criada p/ integração exclusivamente para atender a integração do Epays
    /// para fazer o encrypt/decrypt dos campos e injetados na Metatag do arquivo no storage Azure formando um Hash, caso essa classe seja
    /// altera deve-se replicar para o projeto no repositório Etec.Services -> Etec.Service.Pontofopag.EPays -> Etec.Service.Pontofopag.Business
    /// </summary>
    public class DocumentoHashDto
    {
        private string _cpf;
        private string _cnpj;

        public DocumentoHashDto()
        {

        }

        public DocumentoHashDto(string encryptContent)
        {
            this.FromEncrypt(encryptContent);
        }

        public string Tracking { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Cnpj { get => Regex.Replace(_cnpj, @"[^\d]", ""); set => _cnpj = Regex.Replace(value, @"[^\d]", ""); }
        public string Cpf { get => Regex.Replace(_cpf, @"[^\d]", ""); set => _cpf = Regex.Replace(value, @"[^\d]", ""); }
        public string Nome { get; set; }
        public string Matricula { get; set; }
        public int IdFechamento { get; set; }
        public string DataBaseName { get; set; }
        public (int current, int total) Info { get; set; }
        public int IdFuncionario { get; set; }
        public DateTime InicioPeriodo { get; set; }
        public DateTime FimPeriodo { get; set; }
        public string NomeArquivo { get; set; } = null;
        public int IdEmpresa { get; set; }
        public string UserEPays { get; set; }
        public string PasswordEPays { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { Formatting = Formatting.None, NullValueHandling = NullValueHandling.Ignore });
        }
        public string ToEncrypt()
        {
            this.NomeArquivo = null;
            return Encrypt(this.ToString());
        }
        public void FromEncrypt(string base64)
        {
            JsonConvert.PopulateObject(Decrypt(base64), this);
        }

        #region Crypto 

        string encryptKey = @"ssEu`Um)Qq;2m(p/p827-,gdKd>v[?D=}{fdamnmF_TL:3>p]N'eB%n2n!*FYVh)[u)f#w#{W5HVR#2C/9`%NSWLG}]QazQ$K]dYVP*4h<k,@HdC.V4enXW!CnrqS\ZH-[(u]jFL*X'CUd}c4d~T>8{9SZ3{fFSWjj_&xuF@(#dJb>_#mW^YQ/4z-ZSTNd6Gqf2T9SeCe%xTgZ/fUsP{,26Zd[3R5S,TD7HFMDW\7P?Nka:WQWWvNrTsKM]&`>?/";

        private string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion  
    }
}
