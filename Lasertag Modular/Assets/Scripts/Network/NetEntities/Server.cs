using System.Net;
using System.Net.Sockets;
using Network.Packets;
using Network.Sockets;
using UnityEngine;
using Stream;

namespace Network.NetEntities
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private int _portToListen;
        [SerializeField] private int _triesToFindPort = 1000;
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
                
                SubscribeToSetupMobile(socket);
                SubscribeToSetupVest(socket);
                SubscribeToSetupWeapon(socket);
            });

            _portToListen = FindAvailablePort(_portToListen, _triesToFindPort);

            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (_serverSocketManager.StartListener(_portToListen))
                    {
                        Debug.Log($"Server started on ip {ip.ToString()}, on port {_portToListen})");

                        _serverSocketManager.StartLoop();
                        return;
                    }
                }
            }
        }

        private void SubscribeToSetupMobile(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_MOBILE, (bytes) => 
            {
                    
            });
        }

        private void SubscribeToSetupVest(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_VEST, (bytes) => 
            {
                    
            });
        }

        private void SubscribeToSetupWeapon(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_WEAPON, (bytes) => 
            {
                    
            });
        }

        public int FindAvailablePort(int startPort, int maxAttempts)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                int testPort = startPort + i;
                if (IsPortAvailable(testPort))
                {
                    return testPort;
                }
            }
            return -1; // No available port found

        }

        private bool IsPortAvailable(int port)
        {
            try
            {
                TcpListener testListener = new TcpListener(IPAddress.Any, port);
                testListener.Start();
                testListener.Stop();
                return true; // Port is available
            }
            catch (SocketException)
            {
                return false; // Port is in use
            }
        }
    }
}