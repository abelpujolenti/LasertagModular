using System;
using System.Collections.Generic;
using System.Net;
using Network.Sockets;
using Stream;
using UnityEngine;
//typdef
using OnReceivePacket = System.Action<byte[]>;

namespace Network.NetEntities
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _portToListen;

        private TcpSocket _socketWithServer;
        
        private SocketManager _serverSocketManager;

        private void Start()
        {
            _serverSocketManager = new SocketManager((socket) =>
            {
                Debug.Log("Socket Connected: " + socket.GetRemoteAddress());
                
                socket.SubscribeOnDisconnect((socketDisconnected) =>
                {
                    Debug.Log("Socket Disconnected: " + socketDisconnected.GetRemoteAddress());
                });
            });
        }

        public void Connect()
        {
            ConnectToServer(new IPEndPoint(IPAddress.Parse(_ipAddress), _portToListen));
        }

        private void ConnectToServer(IPEndPoint ipEndPoint)
        {
            if (!_serverSocketManager.ConnectToNetEntity(ipEndPoint))
            {
                return;
            }

            Action<List<TcpSocket>> action = (sockets) =>
            {
                if (!_serverSocketManager.StartListener(_portToListen))
                {
                    return;
                }

                foreach (TcpSocket socket in sockets)
                {
                    _socketWithServer = socket;
            
                    _socketWithServer.Subscribe(0, (bits) =>
                    {
                        Test a = bits.ByteArrayToObjectT<Test>();
                        Debug.Log(a.sos);
                    });
                }
                    
                _serverSocketManager.StartLoop();
            };

            _serverSocketManager.GetSocketsAsync(action);
        }
    }
}