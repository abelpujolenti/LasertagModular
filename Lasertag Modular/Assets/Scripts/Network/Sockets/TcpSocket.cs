using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//
using OnReceivePacket = System.Action<byte[]>;
using OnSocketDisconnect = System.Action<Network.Sockets.TcpSocket>;

namespace Network.Sockets
{
    public class TcpSocket
    {
        private Socket _socket = new(SocketType.Stream, ProtocolType.Tcp);

        private Mutex _subscriptionsMutex = new Mutex();
        private Mutex _socketDisconnectionMutex = new Mutex();

        private Dictionary<uint, OnReceivePacket> _subscriptions = new Dictionary<uint, OnReceivePacket>();

        private List<OnSocketDisconnect> _disconnectionSubscriptions = new List<OnSocketDisconnect>();

        public bool Connect(IPEndPoint ipEndPoint)
        {
            _socket.Connect(ipEndPoint);
            
            _socket.Shutdown(SocketShutdown.Both);

            return _socket.Connected;
        }

        public void SendPacket(uint key)
        {
            _socket.Send(new byte[256]);
        }

        public void SendPacket(uint key, byte[] data)
        {
            _socket.Send(data);
        }

        public void ReceivePacket()
        {
            byte[] buffer = new byte[256];
            int bytesRead = _socket.Receive(buffer);
        }

        public void Subscribe(uint key, OnReceivePacket onReceivePacketAction)
        {
            _subscriptionsMutex.WaitOne();
            
            _subscriptions.Add(key, onReceivePacketAction);
            
            _subscriptionsMutex.ReleaseMutex();
        }

        public void SubscribeAsync(uint key, OnReceivePacket onReceivePacketAction)
        {
            Thread subscribeThread = new Thread(() =>
            {
                Subscribe(key, onReceivePacketAction);
            });
            
            subscribeThread.Start();
        }

        public void Unsubscribe(uint key)
        {
            _subscriptionsMutex.WaitOne();

            _subscriptions.Remove(key);
            
            _subscriptionsMutex.ReleaseMutex();
        }

        public void UnsubscribeASync(uint key)
        {
            Thread unsubscribeThread = new Thread(() =>
            {
                Unsubscribe(key);
            });
            unsubscribeThread.Start();
        }

        public void SubscribeOnDisconnect(OnSocketDisconnect onSocketDisconnectAction)
        {
            _socketDisconnectionMutex.WaitOne();

            _disconnectionSubscriptions.Add(onSocketDisconnectAction);
            
            _socketDisconnectionMutex.ReleaseMutex();
        }

        private void ProcessPacket(byte[] data)
        {
            
        }
    }
}