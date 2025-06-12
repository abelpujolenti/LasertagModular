using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Network.Packets;
using Stream;
//typedefs
using OnReceivePacket = System.Action<byte[]>;
using OnSocketDisconnect = System.Action<Network.Sockets.TcpSocket>;
using Socket = System.Net.Sockets.Socket;

namespace Network.Sockets
{
    public class TcpSocket
    {
        private Socket _socket;

        private Mutex _subscriptionsMutex = new Mutex();
        private Mutex _socketDisconnectionMutex = new Mutex();

        private Dictionary<PacketKeys, OnReceivePacket> _subscriptions = new Dictionary<PacketKeys, OnReceivePacket>();

        private List<OnSocketDisconnect> _disconnectionSubscriptions = new List<OnSocketDisconnect>();

        public TcpSocket()
        {
            _socket = new(SocketType.Stream, ProtocolType.Tcp);
        }

        public TcpSocket(Socket socket)
        {
            _socket = socket;
        }

        public bool Connect(IPEndPoint ipEndPoint)
        {
            _socket.Connect(ipEndPoint);

            return _socket.Connected;
        }

        public void SendPacket(PacketKeys key)
        {
            _socket.Send(BitConverter.GetBytes((byte)key));
        }

        public void SendPacket<T>(PacketKeys key, T obj)
        {
            byte[] keyData = {(byte)key};

            byte[] objectData = ConvertTo.ObjectToByteArray(obj);
            
            _socket.Send(AppendBytes(keyData, objectData));
        }

        byte[] AppendBytes(byte[] arr1, byte[] arr2)
        {
            int keyDataLength = arr1.Length;
            int objectDataLength = arr2.Length;
            
            byte[] data = new byte[keyDataLength + objectDataLength];
            
            for (int i = 0; i < keyDataLength; i++)
            {
                Buffer.SetByte(data, i, arr1[i]);
            }

            for (int i = 0; i < objectDataLength; i++)
            {
                Buffer.SetByte(data, i + keyDataLength, arr2[i]);
            }

            return data;
        }

        public void ReceivePacket()
        {
            byte[] buffer = new byte[255];  
            _socket.Receive(buffer);

            PacketKeys key = (PacketKeys)buffer[0];
            
            byte sizeOfByte = sizeof(byte);
            
            byte[] data = new byte[buffer.Length - sizeOfByte];

            for (byte i = sizeOfByte; i < buffer.Length; i++)
            {
                data[i - sizeOfByte] = buffer[i];
            }

            ProcessPacket(key, data);
        }

        public void Subscribe(PacketKeys key, OnReceivePacket onReceivePacketAction)
        {
            _subscriptionsMutex.WaitOne();
            
            _subscriptions.Add(key, onReceivePacketAction);
            
            _subscriptionsMutex.ReleaseMutex();
        }

        public void SubscribeAsync(PacketKeys key, OnReceivePacket onReceivePacketAction)
        {
            Thread subscribeThread = new Thread(() =>
            {
                Subscribe(key, onReceivePacketAction);
            });
            
            subscribeThread.Start();
        }

        public void Unsubscribe(PacketKeys key)
        {
            _subscriptionsMutex.WaitOne();

            _subscriptions.Remove(key);
            
            _subscriptionsMutex.ReleaseMutex();
        }

        public void UnsubscribeASync(PacketKeys key)
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

        public bool HasDataToRead()
        {
            return _socket.Available != 0;
        }

        private void ProcessPacket(PacketKeys key, byte[] data)
        {
            _subscriptionsMutex.WaitOne();
            
            if (!_subscriptions.ContainsKey(key))
            {
                _subscriptionsMutex.ReleaseMutex();
                return;
            }

            _subscriptions[key](data);
            
            _subscriptionsMutex.ReleaseMutex();
        }

        public EndPoint GetLocalAddress()
        {
            return _socket.LocalEndPoint;
        }

        public EndPoint GetRemoteAddress()
        {
            return _socket.RemoteEndPoint;
        }
    }
}