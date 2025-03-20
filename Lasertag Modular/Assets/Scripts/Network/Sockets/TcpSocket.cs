using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
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

        private Dictionary<uint, OnReceivePacket> _subscriptions = new Dictionary<uint, OnReceivePacket>();

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

        public void SendPacket(uint key)
        {
            _socket.Send(BitConverter.GetBytes(key));
        }

        public void SendPacket<T>(uint key, T obj)
            where T : ISerializable
        {
            byte[] keyData = BitConverter.GetBytes(key);

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
                Buffer.SetByte(data, i, arr2[i]);
            }

            return data;
        }

        public void ReceivePacket()
        {
            byte[] buffer = new byte[256];
            _socket.Receive(buffer);

            uint key = BitConverter.ToUInt32(buffer, 0);
            
            int sizeOfUint = sizeof(uint);
            
            byte[] data = new byte[buffer.Length - sizeOfUint];

            for (int i = sizeOfUint; i < buffer.Length; i++)
            {
                data[i - sizeOfUint] = buffer[i];
            }
            
            ProcessPacket(key, data);
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

        public bool HasDataToRead()
        {
            return _socket.Available != 0;
        }

        private void ProcessPacket(uint key, byte[] data)
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
    }
}