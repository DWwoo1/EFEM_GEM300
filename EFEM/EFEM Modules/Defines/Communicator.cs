using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Define.DefineEnumBase.Common;
using EFEM.Defines.Communicator.Connection;

namespace EFEM.Defines.Communicator
{
    public class Communicator
    {
        #region <Constructors>
        public Communicator(EN_CONNECTION_TYPE interfaceType, int commIndex)
        {
            switch (interfaceType)
            {
                case EN_CONNECTION_TYPE.TCP:
                    _connection = new ConnectionTcpIp(commIndex);
                    break;
                case EN_CONNECTION_TYPE.SERIAL:
                    _connection = new ConnectionSerial(commIndex);
                    break;
                default:
                    return;
            }
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly BaseConnection _connection = null;
        #endregion </Fields>

        #region <Properties>
        public bool IsConnected
        {
            get
            {
                if (_connection == null)
                    return false;

                return _connection.IsConnected();
            }
        }
        #endregion </Properties>

        #region <Methods>
        public bool OpenPort()
        {
            return _connection.Connect();
        }

        public bool ClosePort()
        {
            return _connection.Disconnect();
        }

        public bool WriteStringData(string messageToSend)
        {
            return _connection.WriteData(messageToSend);
        }

        public bool WriteByteData(byte[] messageToSend)
        {
            return _connection.WriteData(messageToSend);
        }

        public bool ReadStringData(ref string receivedMessage)
        {
            return _connection.ReadData(ref receivedMessage);
        }

        public bool ReadByteData(ref byte[] receivedMessage)
        {
            return _connection.ReadData(ref receivedMessage);
        }


        #endregion </Methods>

    }

    namespace Connection
    {
        using Socket_;
        using Serial_;

        public abstract class BaseConnection
        {
            #region <Constructors>
            public BaseConnection(int index)
                : base()
            {
                _myIndex = index;
            }
            #endregion </Constructors>

            #region <Fields>
            protected int _myIndex;
            #endregion </Fields>

            #region <Methods>
            public abstract bool IsConnected();
            public abstract bool Connect();
            public abstract bool Disconnect();
            public abstract bool WriteData(byte[] messageToSend);
            public abstract bool WriteData(string messageToSend);
            public abstract bool ReadData(ref byte[] receivedMessage);
            public abstract bool ReadData(ref string receivedMessage);
            #endregion </Methods>

        }

        public class ConnectionTcpIp : BaseConnection
        {
            #region <Constructors>
            public ConnectionTcpIp(int index)
                : base(index)
            {
                _comm = Socket.GetInstance();
            }
            #endregion </Constructors>

            #region <Fields>
            private readonly Socket _comm = null;
            #endregion </Fields>

            #region <Methods>
            public override bool IsConnected()
            {
                if (_comm == null) return false;

                SOCKET_ITEM_STATE state = _comm.GetState(_myIndex);

                bool connected = true;
                connected &= state != SOCKET_ITEM_STATE.DISCONNECTED;
                connected &= state != SOCKET_ITEM_STATE.READY;
                connected &= state != SOCKET_ITEM_STATE.DISABLED;
                connected &= state != SOCKET_ITEM_STATE.WAITING_CONNECTION;

                return connected;
            }

            public override bool Connect()
            {
                return _comm.Connect(_myIndex);
            }

            public override bool Disconnect()
            {
                _comm.Disconnect(_myIndex);

                return true;
            }
            public override bool WriteData(byte[] messageToSend)
            {
                return _comm.Send(_myIndex, messageToSend);
            }

            public override bool WriteData(string messageToSend)
            {
                return _comm.Send(_myIndex, messageToSend);
            }

            public override bool ReadData(ref byte[] receivedMessage)
            {
                return _comm.Receive(_myIndex, ref receivedMessage);
            }

            public override bool ReadData(ref string receivedMessage)
            {
                return _comm.Receive(_myIndex, ref receivedMessage);
            }
            #endregion </Methods>
        }

        public class ConnectionSerial : BaseConnection
        {
            #region <Constructors>
            public ConnectionSerial(int index)
                : base(index)
            {
                _comm = Serial.GetInstance();
            }
            #endregion </Constructors>

            #region <Fields>
            private readonly Serial _comm = null;
            #endregion </Fields>

            #region <Methods>
            public override bool IsConnected()
            {
                if (_comm == null) return false;

                SERIAL_ITEM_STATE state = _comm.GetState(_myIndex);

                bool connected = true;
                connected &= state != SERIAL_ITEM_STATE.READY;
                connected &= state != SERIAL_ITEM_STATE.DISABLED;

                return connected;
            }

            public override bool Connect()
            {
                return _comm.Open(_myIndex);
            }

            public override bool Disconnect()
            {
                _comm.Close(_myIndex);

                return true;
            }

            public override bool WriteData(byte[] messageToSend)
            {
                return _comm.Write(_myIndex, messageToSend);
            }
            public override bool WriteData(string messageToSend)
            {
                return _comm.Write(_myIndex, messageToSend);
            }
            public override bool ReadData(ref byte[] receivedMessage)
            {
                return _comm.Read(_myIndex, ref receivedMessage);
            }
            public override bool ReadData(ref string receivedMessage)
            {
                return _comm.Read(_myIndex, ref receivedMessage);
            }
            #endregion </Methods>
        }
    }
}