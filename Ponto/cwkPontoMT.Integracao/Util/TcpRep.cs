using cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace cwkPontoMT.Integracao.Relogios
{
    public class TcpRep
    {
        private ManualResetEvent SendDone = new ManualResetEvent(false);
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private readonly byte[] KeyAes = new byte[16];
        private readonly byte[] BufferBytes = new byte[1024];
        private int QuantBytesRec = 0;
        public byte[] Biometric = new byte[384];
        private String response = String.Empty;
        private Random Rnd = new Random();
        private int ChkSum = 0;
        private readonly string StrRec = "";
        private readonly int IdxByte = 0;
        private byte[] Iv = new byte[16];
        private int I = 0;
        private Socket Client;
        int idxByte = 0;

        public TcpRep()
        {
        }

        public bool ConnectTcp(string Ip, int Port, string User, string Password)
        {
            try
            {
                IPHostEntry IpHostInfo = Dns.Resolve(Ip);
                IPAddress IpAddress = IpHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(IpAddress, Port);

                string command = "";
                string preCommand = "";
                int idxByte = 0;
                string strModulo = "";
                string strExpodente = "";
                string strRec = "";
                string strComandoComCriptografia = "";
                string strAux = "";

                int i = 0;

                if (Client == null)
                {
                    Client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                }
                // Create a TCP/IP socket.
                bool conectado = Client.Connected;

                if (conectado == false)
                {
                    // Connect to the remote endpoint.
                    Client.BeginConnect(remoteEP,
                        new AsyncCallback(ConnectCallback), Client);
                    connectDone.WaitOne();
                }


                LoadRandom(true);

                command = "";
                command = command + (char)(2);
                // start byte

                preCommand = preCommand + (char)(7);
                // tamanho do comando
                preCommand = preCommand + (char)(0);
                // tamanho do comando
                preCommand = preCommand + "1+RA+00";
                ChkSum = CalcCheckSumString(preCommand);

                command = command + preCommand;
                command = command + Convert.ToChar(ChkSum);
                // checksum
                // end byte
                command = command + (char)(3);


                // Send test data to the remote device.
                Send(Client, command);
                SendDone.WaitOne();

                QuantBytesRec = Client.Receive(BufferBytes);

                response = "";
                while (i < QuantBytesRec)
                {
                    response = response + (char)BufferBytes[i];

                    i = i + 1;
                }



                while (idxByte < QuantBytesRec)
                {
                    if (idxByte >= 3)
                    {
                        if (idxByte <= QuantBytesRec - 3)
                        {
                            strRec = strRec + response.ElementAt(idxByte);
                        }
                    }
                    idxByte = idxByte + 1;
                }
                strRec = Mid(strRec, strRec.IndexOf("+") + 2, strRec.Length - strRec.IndexOf("+") - 1);
                strRec = Mid(strRec, strRec.IndexOf("+") + 2, strRec.Length - strRec.IndexOf("+") - 1);
                strRec = Mid(strRec, strRec.IndexOf("+") + 2, strRec.Length - strRec.IndexOf("+") - 1);

                strModulo = Mid(strRec, 1, strRec.IndexOf("]"));
                strExpodente = Trim(Mid(strRec, strRec.IndexOf("]") + 2, strRec.Length - strRec.IndexOf("]") - 1));

                strAux = "1]" + User + "]" + Password + "]" + System.Convert.ToBase64String(KeyAes);

                RSAPersistKeyInCSP(strModulo);
                byte[] dataToEncrypt = Encoding.Default.GetBytes(strAux);
                byte[] encryptedData = null;

                RSAParameters RSAKeyInfo = new RSAParameters();

                RSAKeyInfo.Modulus = System.Convert.FromBase64String(strModulo);
                RSAKeyInfo.Exponent = System.Convert.FromBase64String(strExpodente);

                encryptedData = RSAEncrypt(dataToEncrypt, RSAKeyInfo, false);

                strAux = System.Convert.ToBase64String(encryptedData);


                strComandoComCriptografia = "2+EA+00+" + strAux;

                preCommand = "";
                command = "";
                command = command + Convert.ToChar(2);
                // start byte
                preCommand = preCommand + Convert.ToChar(strComandoComCriptografia.Length);
                // tamanho do comando
                preCommand = preCommand + Convert.ToChar(0);
                // tamanho do comando
                preCommand = preCommand + strComandoComCriptografia;
                ChkSum = CalcCheckSumString(preCommand);

                command = command + preCommand;
                command = command + Convert.ToChar(ChkSum);
                // checksum

                command = command + Convert.ToChar(3);
                // end byte
                Send(Client, command);
                SendDone.WaitOne();

                QuantBytesRec = Client.Receive(BufferBytes);

                response = "";
                i = 0;
                while (i < QuantBytesRec)
                {

                    response = response + Convert.ToChar(BufferBytes[i]);
                    i = i + 1;
                }

                strRec = "";
                idxByte = 0;
                while (idxByte < QuantBytesRec)
                {
                    if (idxByte >= 3)
                    {
                        if (idxByte <= QuantBytesRec - 3)
                        {
                            strRec = strRec + response.ElementAt(idxByte);
                        }
                    }
                    idxByte = idxByte + 1;
                }
                strRec = Mid(strRec, strRec.IndexOf("+") + 2, strRec.Length - strRec.IndexOf("+") - 1);
                strRec = Mid(strRec, strRec.IndexOf("+") + 2, strRec.Length - strRec.IndexOf("+") - 1);

                if (strRec == "000")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string ReceiveBiometricsAmount(string Command)
        {
            Random rnd = new Random();

            int i = 0;
            int chkSum = 0;
            string strRec = "";
            int idxByte = 0;
            Biometric = new byte[384];


            LoadRandom(false);


            int tamanhoPacote = 48;
            byte[] comandoByte = new byte[53];
            int IdxComandoByte = 3;
            comandoByte[0] = 2;
            // start byte
            comandoByte[1] = (byte)(tamanhoPacote & 0xff);
            // tamanho
            comandoByte[2] = (byte)((tamanhoPacote >> 8) & 0xff);
            // tamanho

            byte[] cmdCrypt = Encoding.Default.GetBytes(Encoding.Default.GetChars(EncryptStringToBytes_Aes(Command, KeyAes, Iv)));
            chkSum = 0;
            i = 0;
            while (i < Iv.Length)
            {
                comandoByte[IdxComandoByte] = Iv[i];

                IdxComandoByte = IdxComandoByte + 1;
                i = i + 1;
            }

            i = 0;
            while (i < cmdCrypt.Length)
            {
                comandoByte[IdxComandoByte] = cmdCrypt[i];

                IdxComandoByte = IdxComandoByte + 1;
                i = i + 1;
            }
            i = 1;
            while (i < IdxComandoByte)
            {
                chkSum = chkSum ^ comandoByte[i];
                i = i + 1;
            }
            comandoByte[IdxComandoByte] = (byte)chkSum;
            IdxComandoByte = IdxComandoByte + 1;
            comandoByte[IdxComandoByte] = 3;

            string strAux = "";
            i = 0;
            while (i < IdxComandoByte)
            {
                strAux = strAux + Convert.ToChar(comandoByte[i]);
                i = i + 1;
            }
            Send2(Client, comandoByte);

            SendDone.WaitOne();


            QuantBytesRec = Client.Receive(BufferBytes);

            response = "";
            i = 0;
            while (i < QuantBytesRec)
            {

                response = response + (char)BufferBytes[i];

                i = i + 1;
            }

            i = 0;
            strRec = "";
            idxByte = 0;
            byte[] byteData = new byte[QuantBytesRec - 5];
            while (idxByte < QuantBytesRec)
            {
                if (idxByte >= 3)
                {
                    if (idxByte <= QuantBytesRec - 3)
                    {
                        byteData[i] = Convert.ToByte(response.ElementAt(idxByte));
                        i = i + 1;
                        strRec = strRec + response.ElementAt(idxByte);
                    }
                }
                idxByte = idxByte + 1;
            }
            i = 0;
            while (i < 16)
            {
                Iv[i] = byteData[i];
                i = i + 1;
            }

            byte[] byteData2 = new byte[QuantBytesRec - 16 - 5];
            i = 0;

            while (i < byteData.Length - 16)
            {
                byteData2[i] = byteData[i + 16];
                i = i + 1;
            }

            strRec = DecryptStringFromBytes_Aes(byteData2, KeyAes, Iv);
            strRec = Mid(strRec, strRec.IndexOf("}") + 2, strRec.Length - strRec.IndexOf("}") - 1);
            return strRec;
        }

        public string ReceiveBiometrics(string Command, int SizePackage)
        {
            LoadRandom(false);

            byte[] ComandoByte = new byte[53];
            int IdxComandoByte = 3;
            ComandoByte[0] = 2;
            ComandoByte[1] = (byte)(SizePackage & 0xff);
            ComandoByte[2] = (byte)((SizePackage >> 8) & 0xff);

            byte[] cmdCrypt = Encoding.Default.GetBytes(Encoding.Default.GetChars(EncryptStringToBytes_Aes(Command, KeyAes, Iv)));
            ChkSum = 0;
            I = 0;
            while (I < Iv.Length)
            {
                ComandoByte[IdxComandoByte] = Iv[I];

                IdxComandoByte = IdxComandoByte + 1;
                I = I + 1;
            }

            I = 0;
            while (I < cmdCrypt.Length)
            {
                ComandoByte[IdxComandoByte] = cmdCrypt[I];

                IdxComandoByte = IdxComandoByte + 1;
                I = I + 1;
            }
            I = 1;
            while (I < IdxComandoByte)
            {
                ChkSum = ChkSum ^ ComandoByte[I];
                I = I + 1;
            }
            ComandoByte[IdxComandoByte] = (byte)ChkSum;
            IdxComandoByte = IdxComandoByte + 1;
            ComandoByte[IdxComandoByte] = 3;

            string strAux = "";
            I = 0;
            while (I < IdxComandoByte)
            {
                strAux = strAux + Convert.ToChar(ComandoByte[I]);
                I = I + 1;
            }
            Send2(Client, ComandoByte);

            SendDone.WaitOne();

            QuantBytesRec = Client.Receive(BufferBytes);

            response = "";
            I = 0;
            while (I < QuantBytesRec)
            {
                response = response + (char)BufferBytes[I];
                I = I + 1;
            }

            I = 0;
            string strRec = "";
            idxByte = 0;
            byte[] byteData = new byte[QuantBytesRec - 5];
            while (idxByte < QuantBytesRec)
            {
                if (idxByte >= 3)
                {
                    if (idxByte <= QuantBytesRec - 3)
                    {
                        byteData[I] = Convert.ToByte(response.ElementAt(idxByte));
                        I = I + 1;
                        strRec = strRec + response.ElementAt(idxByte);
                    }
                }
                idxByte = idxByte + 1;
            }
            I = 0;
            while (I < 16)
            {
                Iv[I] = byteData[I];
                I = I + 1;
            }

            byte[] byteData2 = new byte[QuantBytesRec - 16 - 5];
            I = 0;

            while (I < byteData.Length - 16)
            {
                byteData2[I] = byteData[I + 16];
                I = I + 1;
            }

            var ttttstrRec = DecryptStringFromBytes_Aes(byteData2, KeyAes, Iv);

            byte[] bufferRecDecrypt = DecryptStringFromBytes_Aes2(byteData2, KeyAes, Iv);
            I = 0;
            while (I < bufferRecDecrypt.Length)
            {
                if (Convert.ToChar(bufferRecDecrypt[I]) == '{')
                {
                    break;
                }
                I = I + 1;
            }
            I = I + 1;

            int x = 0;
            while (x < 384)
            {
                Biometric[x] = bufferRecDecrypt[I + x];
                x = x + 1;
            }

            return  Encoding.Default.GetString(Biometric);
        }

        private void LoadRandom(bool Aes)
        {
            if (Aes)
            {
                KeyAes[0] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[1] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[2] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[3] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[4] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[5] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[6] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[7] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[8] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[9] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[10] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[11] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[12] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[13] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[14] = Convert.ToByte(Rnd.Next(1, 256));
                KeyAes[15] = Convert.ToByte(Rnd.Next(1, 256));
            }
            else
            {
                Iv[0] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[1] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[2] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[3] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[4] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[5] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[6] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[7] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[8] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[9] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[10] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[11] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[12] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[13] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[14] = Convert.ToByte(Rnd.Next(1, 256));
                Iv[15] = Convert.ToByte(Rnd.Next(1, 256));
            }
        }

        private byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                aesAlg.Padding = PaddingMode.None;
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);

                            int quant = plainText.Length;

                            if (quant > 16)
                            {
                                quant = quant % 16;
                            }

                            quant = 16 - quant;
                            while (quant < 16 && quant != 0)
                            {
                                swEncrypt.Write(Convert.ToChar(Convert.ToByte("0")));
                                quant = quant - 1;
                            }
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        private void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.Default.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }
        private void Send2(Socket client, byte[] byteData)
        {
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                SendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public byte CalcCheckSumString(string data)
        {
            String strBuf = "";
            String strAux = "";
            byte cks = 0;
            int i = 0;

            while (i < data.Length)
            {
                strAux = ((byte)(data.ElementAt(i))).ToString("X2");
                strBuf = strBuf + strAux;
                cks = (byte)(cks ^ (byte)(data.ElementAt(i)));
                i = i + 1;
            }
            return cks;
        }
        private string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        private string Trim(string s)
        {
            return s.Trim();
        }

        private void RSAPersistKeyInCSP(string ContainerName)
        {
            try
            {
                CspParameters cspParams = new CspParameters();

                cspParams.KeyContainerName = ContainerName;

                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(cspParams);

                Console.WriteLine("The RSA key was persisted in the container, \"{0}\".", ContainerName);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.ImportParameters(RSAKeyInfo);

                return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        private byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.ImportParameters(RSAKeyInfo);

                return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                aesAlg.Padding = PaddingMode.None;
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        private byte[] DecryptStringFromBytes_Aes2(byte[] cipherText, byte[] Key, byte[] IV)
        {
            byte[] bufferDecrypt = new byte[cipherText.Length];

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                aesAlg.Padding = PaddingMode.None;
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Read(bufferDecrypt, 0, bufferDecrypt.Length);
                    }
                }
            }
            return bufferDecrypt;
        }
    }
}
