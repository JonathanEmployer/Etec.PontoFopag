using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.IDData
{
    public class CSocketClient
    {
        public const uint BUFFER_SIZE = 16720;

        #region Delegates

        //public delegate void ConnectionDelegate(Socket soc);
        public delegate void ConnectionDelegate();
        public delegate void ErrorDelegate(string ErroMessage, int ErroCode);
        public delegate void BufferEventHandler(byte[] rgbyBuffer);

        #endregion

        #region Eventos

        public event ConnectionDelegate OnConnect;
        public event ConnectionDelegate OnDisconnect;
        public event ConnectionDelegate OnWrite;
        public event ErrorDelegate OnError;
        public event BufferEventHandler OnRead;

        #endregion

        #region Variaveis

        private AsyncCallback WorkerCallBack;
        private Socket sckMain = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private IPEndPoint ipLocal;
        private int iTCPPort = 0;
        private byte[] rgbyBuffer = new byte[BUFFER_SIZE];
        private byte[] rgbyReceivedBytes;
        private string strReceivedText = "";
        private string strSentText = "";
        // private string strRemoteAddress = "";
        private string strRemoteHost = "";

        #endregion

        #region Properties

        /// <summary>
        /// Porta para conexão com o Servidor
        /// </summary>
        public int Port
        {
            get
            {
                return (iTCPPort);
            }
        }

        /// <summary>
        /// Bytes que chegaram ao Socket
        /// </summary>
        public byte[] ReceivedBytes
        {
            get
            {
                byte[] temp = null;
                if (this.rgbyReceivedBytes != null)
                {
                    temp = this.rgbyReceivedBytes;
                    this.rgbyReceivedBytes = null;
                }
                return (temp);
            }
        }

        /// <summary>
        /// Messagem que chegou ao Socket
        /// </summary>
        public string ReceivedText
        {
            get
            {
                string temp = this.strReceivedText;
                this.strReceivedText = "";
                return (temp);
            }
        }

        /// <summary>
        /// Messagem enviada pelo Socket
        /// </summary>
        public string WriteText
        {
            get
            {
                string temp = this.strSentText;
                this.strSentText = "";
                return (temp);
            }
        }

        ///// <summary>
        ///// IP do Servidor
        ///// </summary>
        //public string RemoteAddress
        //{
        //    get
        //    {
        //        if (this.sckMain.Connected)
        //        {
        //            return (this.strRemoteAddress);
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}

        /// <summary>
        /// Host do Servidor
        /// </summary>
        public string RemoteHost
        {
            get
            {
                if (this.sckMain.Connected)
                    return (this.strRemoteHost);
                else
                    return "";
            }
        }

        /// <summary>
        /// Retorna true se o ClientSocket estiver conectado a um Servidor
        /// </summary>
        public bool Connected
        {
            get
            {
                return (this.sckMain.Connected);
            }
        }

        #endregion

        #region Construtor

        public CSocketClient(string _strIPAddress, int _iTCPPort)
        {
            try
            {
                iTCPPort = _iTCPPort;
                IPAddress ipAddress = IPAddress.Parse(_strIPAddress);
                this.ipLocal = new IPEndPoint(ipAddress, this.iTCPPort);
            }
            catch (Exception se)
            {
                if (OnError != null) OnError(se.Message, 0);
            }
        }

        #endregion

        #region Methods and Events

        /// <summary>
        /// Conecta-se ao IP e Porta configurados
        /// </summary>
        public bool Connect()
        {
            try
            {
                //Connect to the server
                this.sckMain.BeginConnect(ipLocal, new AsyncCallback(ConfirmConnect), null);
                return true;
            }
            catch (ArgumentException se)
            {
                if (OnError != null) OnError(se.Message, 0);
                return false;
            }
            catch (InvalidOperationException se)
            {
                if (OnError != null) OnError(se.Message, 0);
                return false;
            }
            catch (SocketException se)
            {
                if (OnError != null) OnError(se.Message, se.ErrorCode);
                return false;
            }
        }

        /// <summary>
        /// Desfaz a conexão com o Servidor
        /// </summary>
        public bool Disconnect()
        {
            this.sckMain.Shutdown(SocketShutdown.Both);
            this.sckMain.Close();

            if (!sckMain.Connected)
            {
                if (OnDisconnect != null) OnDisconnect();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Desabilita a recepção e envio que estão processando
        /// </summary>
        public void ShutDown()
        {
            this.sckMain.Shutdown(SocketShutdown.Both);
        }

        private void ConfirmConnect(IAsyncResult asyn)
        {
            try
            {
                this.sckMain.EndConnect(asyn);
                WaitForData(sckMain);

                if (OnConnect != null) OnConnect();
            }
            catch (ObjectDisposedException se)
            {
                if (OnError != null) OnError(se.Message, 0);
            }
            catch (SocketException se)
            {
                if (OnError != null) OnError(se.Message, 0);
            }
        }

        private void WaitForData(Socket soc)
        {
            try
            {
                if (this.WorkerCallBack == null)
                {
                    this.WorkerCallBack = new AsyncCallback(OnDataReceived);
                }

                Array.Clear(this.rgbyBuffer, 0, this.rgbyBuffer.Length);

                if (soc != null && soc.Connected)
                {
                    soc.BeginReceive(this.rgbyBuffer, 0, this.rgbyBuffer.Length, SocketFlags.None, this.WorkerCallBack, null);
                }
            }
            catch (SocketException se)
            {
                if (OnError != null) OnError(se.Message, se.ErrorCode);
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                int iRx = sckMain.EndReceive(asyn);

                if (iRx < 1)
                {
                    this.sckMain.Shutdown(SocketShutdown.Both);
                    this.sckMain.Close();
                    if (this.sckMain.Connected == false)
                    {
                        if (OnDisconnect != null) { OnDisconnect(); }
                    }
                }
                else
                {
                    this.rgbyReceivedBytes = new byte[iRx];
                    Array.Copy(this.rgbyBuffer, this.rgbyReceivedBytes, iRx);

                    char[] chars = new char[iRx + 1];
                    Decoder d = Encoding.UTF8.GetDecoder();
                    d.GetChars(this.rgbyBuffer, 0, iRx, chars, 0);
                    this.strReceivedText = new String(chars);

                    if (OnRead != null) OnRead(this.rgbyReceivedBytes);

                    this.WaitForData(this.sckMain);
                }
            }
            catch (ArgumentException se)
            {
                if (OnError != null) OnError(se.Message, 0);
            }
            catch (InvalidOperationException se)
            {
                this.sckMain.Close();

                if (this.sckMain.Connected == false)
                {
                    if (OnDisconnect != null) OnDisconnect();
                }
                if (OnError != null) OnError(se.Message, 0);
            }
            catch (SocketException se)
            {
                if (OnError != null) OnError(se.Message, se.ErrorCode);

                if (this.sckMain.Connected == false)
                {
                    if (OnDisconnect != null) OnDisconnect();
                }
            }
            catch (Exception ex)
            {
                if (OnError != null) OnError(ex.Message, 0);
            }
        }

        /// <summary>
        /// Envia um vetor de bytes pela conexão
        /// </summary>
        /// <param name="byData">Buffer de dados a serem transmitidos</param>
        /// <param name="iNumBytes">Quantidade de bytes a serem enviados</param>
        /// <returns></returns>
        public bool SendBuffer(byte[] _rgbyDataBuffer)
        {
            try
            {
                int NumBytes = this.sckMain.Send(_rgbyDataBuffer, _rgbyDataBuffer.Length, SocketFlags.None);

                if (NumBytes == _rgbyDataBuffer.Length)
                {
                    if (OnWrite != null)
                    {
                        strSentText = _rgbyDataBuffer.ToString();
                        OnWrite();
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (ArgumentException se)
            {
                if (OnError != null) OnError(se.Message, 0);
                return false;
            }
            catch (ObjectDisposedException se)
            {
                if (OnError != null) OnError(se.Message, 0);
                return false;
            }
            catch (SocketException se)
            {
                if (OnError != null) OnError(se.Message, se.ErrorCode);
                return false;
            }
        }

        #endregion
    }

    public class REPConnection
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;

        private string _ip;
        private int _port;

        /// <summary>
        /// construtor
        /// </summary>
        /// <param name="ip">ip do REP</param>
        /// <param name="port">porta de comunicação com o REP</param>
        public REPConnection(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        /// <summary>
        /// estabelece conexão com o REP
        /// </summary>
        public void Connect()
        {
            //retries
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _tcpClient = new TcpClient(_ip, _port);

                    //define timeout de envio e recebimento
                    _tcpClient.ReceiveTimeout = 2000;
                    _tcpClient.SendTimeout = 2000;

                    _stream = _tcpClient.GetStream();

                    break;
                }
                catch
                {
                    //lança exceção se atingido numero de retries
                    if (i == 4)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// finaliza conexão com o REP
        /// </summary>
        public void Disconnect()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
            }
        }

        /// <summary>
        /// finaliza conexão atual e estabelece nova conexão
        /// </summary>
        public void Reconnect()
        {
            Disconnect();
            Connect();
        }

        /// <summary>
        /// envia mensagem para o rep e aguarda resposta
        /// </summary>
        /// <param name="message">buffer do comando</param>
        /// <returns>resposta do rep</returns>
        public byte[] ProcessMessage(byte[] message)
        {
            lock (_tcpClient)
            {
                //retries
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        //envia mensagem para o REP
                        SendMessage(message);

                        //recebe resposta do REP
                        return ReceiveMessage();
                    }
                    catch
                    {
                        Reconnect();

                        //lança exceção se atingido numero de retries 
                        if (i == 4)
                        {
                            throw;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// envia mensagem
        /// </summary>
        /// <param name="buffer">buffer do comando</param>
        private void SendMessage(byte[] buffer)
        {
            try
            {
                _stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// recebe resposta do rep
        /// </summary>
        /// <returns>resposta do rep</returns>
        private byte[] ReceiveMessage()
        {
            List<byte> messageBytes = new List<byte>();

            //recebe cabeçalho da mensagem
            int dataSize = ReceiveMessageHeader(messageBytes);

            //recebe dados da mensagem
            ReceiveMessageData(dataSize, messageBytes);

            return messageBytes.ToArray();
        }

        /// <summary>
        /// recebe cabeçalho da mensagem
        /// </summary>
        /// <param name="messageBytes">buffer para aramazenar a responsta</param>
        /// <returns>quantidade de bytes de dados</returns>
        private int ReceiveMessageHeader(List<byte> messageBytes)
        {
            byte[] buffer = new byte[15];

            int readBytes = 0;

            if (_stream.CanRead == true)
            {
                readBytes = _stream.Read(buffer, 0, 15);
            }

            if (readBytes < 15)
            {
                throw new Exception("quantidade de dados lidos menor que quantidade esperada");
            }

            messageBytes.AddRange(buffer);

            //extrai tamanho do campo dados
            byte[] sizeBuffer = new byte[2];
            Array.Copy(buffer, 11, sizeBuffer, 0, 2);
            Array.Reverse(sizeBuffer);

            return BitConverter.ToInt16(sizeBuffer, 0);
        }

        /// <summary>
        /// recebe o campo dados da mensagem
        /// </summary>
        /// <param name="dataSize">quantidade de bytes do campo dados</param>
        /// <param name="messageBytes">buffer para armazenar resposta</param>
        private void ReceiveMessageData(int dataSize, List<byte> messageBytes)
        {
            if (dataSize > 0)
            {
                byte[] buffer = new byte[dataSize + 1];

                int readBytes = _stream.Read(buffer, 0, dataSize + 1);

                if (readBytes < dataSize)
                {
                    throw new Exception("quantidade de dados lidos menor que quantidade esperada");
                }

                messageBytes.AddRange(buffer);
            }
        }
    }
}
