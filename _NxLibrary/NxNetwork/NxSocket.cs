using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NxBase;

namespace NxNetwork
{
    public class NxSocket
    {
        private Socket _socket;

        public NxSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Connect(string ip, int port)
        {
            return Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        public bool Connect(IPEndPoint ipPoint)
        {
            bool connected = false;
            try
            {
                NxLog.v(this, "连接服务器:" + ipPoint.Address.ToString());
                _socket.NoDelay = true;
                _socket.Connect(ipPoint);
                _socket.Blocking = false;
                connected = true;
            }
            catch(Exception ex)
            {
                NxLog.v(this, "连接出现异常:" + ex.Message);
            }
            return connected;
        }

        public bool Disconnect()
        {
            bool success = false;
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                success = true;
            }
            catch (Exception ex)
            {
                NxLog.v(this, "断开连接出现异常:" + ex.Message);
            }
            return success;
        }

        public bool Send(byte[] buf, int lenght)
        {
            bool success = false;
            try
            {
                _socket.Send(buf, lenght, SocketFlags.None);
                success = true;
            }
            catch(SocketException ex)
            {
                NxLog.v(this, "发送出现异常:" + ex.Message);
            }
            return success;
        }

        public int Receive(byte[] buf, int offset, int size)
        {
            return _socket.Receive(buf, offset, size, SocketFlags.None);
        }

        public int Receive(ref byte[] buf)
        {
            int lenght = 0;
            try
            {
                lenght = _socket.Receive(buf);
            }
            catch(Exception ex)
            {
                NxLog.v(this, "接受出现异常:" + ex.Message);
            }
            return lenght;
        }

        public int Available
        {
            get
            {
                try
                {
                    if (_socket != null)
                    {
                        lock (_socket)
                        {
                            return _socket.Available;
                        }
                    }
                }
                catch(Exception ex)
                {
                    NxLog.v(this, "Available出现异常:" + ex.Message);
                }
                return 0;
            }
        }

        public bool Connected
        {
            get
            {
                if(_socket != null && _socket.Connected)
                {
                    return true;
                }
                return false;
            }
        }

        public void Dispose()
        {
            if(_socket != null)
            {
                lock(_socket)
                {
                    if (_socket.Connected)
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                    }
                    _socket.Close();
                    _socket = null;
                }
            }
            NxLog.v(this, "析构Socket");
        }
    }
}
