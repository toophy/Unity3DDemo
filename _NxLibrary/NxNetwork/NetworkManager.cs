using NxBase;
using NxBase.Dispatcher;
using NxBase.Helper;
using NxNetwork.MSG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxNetwork
{
    public class NetworkManager : NxSingleton<NetworkManager>
    {
        private NxSocket _socket = new NxSocket();

        private byte[] _recvData = new byte[GlobalVar.BUFFER_LENGTH];       //消息接受缓存
        private byte[] _recvDataTmp = new byte[GlobalVar.BUFFER_LENGTH];
        private UInt32 _recvOffset = 0;

        NxDispatcher _commDispatcher = new NxDispatcher();

        public NetworkManager()
        {
            
        }

        public void Register<T>(ProtoCallBack<T> handle)
        {
            _commDispatcher.Register(handle);
        }

        public void Init()
        {
            NxTask.Instance.AddTask(Recv());
        }

        public void ConnectServer(string ip, int port)
        {
            _socket.Connect(ip, port);
        }

        public void SendMsg<T>(T data)
        {
            var bData = SerializationHelper.ProtoSerialization(data);
            MainMessage msg = new MainMessage(bData.Length, DateTime.Now.ToBinary(), data.GetType().Name, bData);
            var msgData = msg.ToBinary();
            SendMsg(msgData);
        }

        public void SendMsg(byte[] data)
        {
            _socket.Send(data, data.Length);

            if (_socket == null || !_socket.Connected)
            {
                //TODO 重连
            }
        }

        private IEnumerator Recv()
        {
            while (true)
            {
                if (_socket != null && _socket.Connected)
                {
                    if (_socket.Available > 0)
                    {
                        int receiveLength = _socket.Receive(ref _recvData);
                        if (receiveLength != 0)
                        {
                            Array.Copy(_recvData, 0, _recvDataTmp, _recvOffset, receiveLength);
                            _recvOffset += (UInt32)receiveLength;
                            if (_recvOffset > GlobalVar.HEAD_SIZE)
                            {
                                byte[] receiveData = new byte[_recvOffset];
                                Array.Copy(_recvDataTmp, receiveData, _recvOffset);
                                Array.Clear(_recvDataTmp, 0, _recvDataTmp.Length);
                                _recvOffset = 0;
                                RecvDataHandle(receiveData);
                            }
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void RecvDataHandle(byte[] receiveData)
        {
            MainMessage msg = SerializationHelper.BinaryDeSerialization<MainMessage>(receiveData);
            byte[] data = new byte[msg.Lenght];
            Array.Copy(msg.MsgBuffer, data, msg.Lenght);
            _commDispatcher.Dispatcher(msg.MsgName, data);
        }
    }
}
