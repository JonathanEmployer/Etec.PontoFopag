using DAL.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = args.FirstOrDefault();
            if (!string.IsNullOrEmpty(input))
            {
                string output = new AES().Encrypt(input.Trim());
                Console.WriteLine(output);
                Console.ReadLine();
            }
        }
    }
}
