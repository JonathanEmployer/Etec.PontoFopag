using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Crypto
{
    public class AES
    {
        private readonly byte[] _key;
        private readonly Random _random;
        private readonly RijndaelManaged _rm;
        private readonly UTF8Encoding _encoder;

        public AES()
        {
            _key = Convert.FromBase64String("02b98a2459de4aeca1b5e24c03e4a051");
            _random = new Random();
            _rm = new RijndaelManaged();
            _encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            var vector = new byte[16];
            _random.NextBytes(vector);
            var cryptogram = vector.Concat(this.Encrypt(_encoder.GetBytes(unencrypted), vector));
            return Convert.ToBase64String(cryptogram.ToArray());
        }

        public string Decrypt(string encrypted)
        {
            var cryptogram = Convert.FromBase64String(encrypted);
            if (cryptogram.Length < 17)
            {
                throw new ArgumentException("encrypted");
            }

            var vector = cryptogram.Take(16).ToArray();
            var buffer = cryptogram.Skip(16).ToArray();
            return _encoder.GetString(this.Decrypt(buffer, vector));
        }

        private byte[] Encrypt(byte[] buffer, byte[] vector)
        {
            var encryptor = _rm.CreateEncryptor(_key, vector);
            return this.Transform(buffer, encryptor);
        }

        private byte[] Decrypt(byte[] buffer, byte[] vector)
        {
            var decryptor = _rm.CreateDecryptor(_key, vector);
            return this.Transform(buffer, decryptor);
        }

        private byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }

            return stream.ToArray();
        }

    }
}
