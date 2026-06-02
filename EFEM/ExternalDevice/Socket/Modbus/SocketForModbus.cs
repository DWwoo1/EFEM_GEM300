using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

using TickCounter_;

namespace ModbusTCPOnly
{
    class SocketForModbus
    {
        #region <Constructors>
        private SocketForModbus()
        {
            client = new TcpClient();
            TicksForConnection = new TickCounter();
            MessagesToSend = new ConcurrentQueue<byte[]>();
        }

        #endregion </Constructors>

        #region <Fields>
        private static SocketForModbus instance;
        private static readonly object lockObject = new object();

        private TcpClient client;
        private NetworkStream stream;
        private bool isRunning;
        private string serverIp;
        private int port;
        private readonly TickCounter TicksForConnection = null;
        private const uint IntervalReConnection = 5000;

        //private byte[] _messageToSend = null;
        private byte[] _temporaryReceivedData = null;
        private byte[] _receivedMessage = null;
        private readonly ConcurrentQueue<byte[]> MessagesToSend = null;
        private readonly object _lock = new object();
        #endregion </Fields>

        #region <Properties>
        public bool Connected
        {
            get
            {
                if (client == null || stream == null)
                    return false;

                return client.Connected;
            }
        }
        public static SocketForModbus Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SocketForModbus();
                    }
                    return instance;
                }
            }
        }
        #endregion </Properties>

        #region <Methods>

        #region <Internal Methods>
        private void Executing()
        {
            isRunning = true;
            bool needRetryConnect = false;
            while (true)
            {
                Thread.Sleep(1);

                if (false == isRunning)
                    break;

                #region <Connection>
                if (client == null || stream == null || false == client.Connected)
                {
                    if (false == TicksForConnection.IsTickOver(true))
                        continue;

                    try
                    {
                        lock(_lock)
                        {
                            if (client == null)
                                client = new TcpClient();

                            client.Connect(serverIp, port); // 동기 연결 시도
                            stream = client.GetStream();

                            // Keep-Alive 설정
                            SetKeepAlive(client.Client, true, 1000, 1000);
                            needRetryConnect = false;
                        }
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"연결 실패: {ex.Message}. 5초 후에 재시도...");
                        needRetryConnect = true;
                        TicksForConnection.SetTickCount(IntervalReConnection);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"예상치 못한 오류: {ex.Message}. 연결을 종료합니다.");
                        needRetryConnect = true;
                    }

                    if (needRetryConnect || false == client.Connected)
                    {
                        Close();
                        TicksForConnection.SetTickCount(IntervalReConnection);
                        continue;
                    }
                }
                #endregion </Connection>

                continue;

                #region <Send Message>
                //if (MessagesToSend.Count <= 0)
                //    continue;

                //try
                //{
                //    MessagesToSend.TryDequeue(out _messageToSend);
                //    if (_messageToSend == null)
                //        continue;
                //    stream.Write(_messageToSend, 0, _messageToSend.Length);
                //}
                //catch (SocketException ex)
                //{
                //    Console.WriteLine($"연결 실패: {ex.Message}. 5초 후에 재시도...");
                //    needRetryConnect = true;
                //    Close();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"예상치 못한 오류: {ex.Message}. 연결을 종료합니다.");
                //    needRetryConnect = true;
                //    Close();
                //}

                //if (needRetryConnect || false == client.Connected)
                //{
                //    Close();
                //    TicksForConnection.SetTickCount(IntervalReConnection);
                //    continue;
                //}
                #endregion </Send Message>

                #region <Check Received Message>
                //try
                //{
                //    _temporaryReceivedData = new byte[1024];
                //    int bytesRead = stream.Read(_temporaryReceivedData, 0, _temporaryReceivedData.Length);
                //    if (bytesRead <= 0)
                //        continue;

                //    _receivedMessage = new byte[bytesRead];
                //    Array.Copy(_temporaryReceivedData, _receivedMessage, _receivedMessage.Length);
                //}
                //catch (SocketException ex)
                //{
                //    Console.WriteLine($"연결 실패: {ex.Message}. 5초 후에 재시도...");
                //    needRetryConnect = true;
                //    Close();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"예상치 못한 오류: {ex.Message}. 연결을 종료합니다.");
                //    needRetryConnect = true;
                //    Close();
                //}

                //if (needRetryConnect || false == client.Connected)
                //{
                //    Close();
                //    TicksForConnection.SetTickCount(IntervalReConnection);
                //    continue;
                //}
                #endregion </Check Received Message>
            }
        }

        public void Close()
        {
            lock (_lock)
            {
                try
                {
                    stream?.Close();
                }
                catch { }
                stream = null;

                try
                {
                    client?.Close();
                }
                catch { }
                client = null;
            }
        }

        private static void SetKeepAlive(Socket socket, bool enable, uint keepAliveTime, uint keepAliveInterval)
        {
            int optionValue = enable ? 1 : 0;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, optionValue);

            //if (enable)
            //{
            //    socket.SetSocketOption(SocketOptionLevel.Tcp, (SocketOptionName)0x00000004, keepAliveTime);  // TCP_KEEPIDLE
            //    socket.SetSocketOption(SocketOptionLevel.Tcp, (SocketOptionName)0x00000005, keepAliveInterval); // TCP_KEEPINTVL
            //}
        }
        #endregion </Internal Methods>

        #region <External Methods>
        public void Initialize(string serverIp, int port)
        {
            this.serverIp = serverIp;
            this.port = port;

            // 스레드를 생성하여 연결 및 메시지 수신 처리
            Thread connectionThread = new Thread(Executing);
            connectionThread.IsBackground = true;
            connectionThread.Start();
        }
        public void Exit()
        {
            isRunning = false;
            Close();
        }

        public bool Send(byte[] message)
        {
            try
            {
                if (client == null || stream == null)
                {
                    return false;
                }

                if (false == client.Connected)
                    return false;

                _receivedMessage = null;
                stream.Write(message, 0, message.Length);

                if (_temporaryReceivedData == null)
                {
                    _temporaryReceivedData = new byte[1024];
                }
                int bytesRead = stream.Read(_temporaryReceivedData, 0, _temporaryReceivedData.Length);
                if (bytesRead <= 0)
                    return false;

                _receivedMessage = new byte[bytesRead];
                Array.Copy(_temporaryReceivedData, _receivedMessage, _receivedMessage.Length);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            //if (client?.Connected == true)
            //{
            //    MessagesToSend.Enqueue(message);
            //    return true;
            //}

            //return false;
        }


        public bool Receive(ref byte[] receivedMessages)
        {
            if (_receivedMessage == null)
                return false;

            if (receivedMessages == null)
            {
                receivedMessages = new byte[_receivedMessage.Length];
            }

            Array.Copy(_receivedMessage, receivedMessages, _receivedMessage.Length);

            return true;
        }
        #endregion </External Methods>

        #endregion </Methods>
    }
}