using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Network.Sockets;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
                
                Thread.Sleep(1000);
                
                socket.SendPacket(1, new Test
                {
                    sos ="sos", 
                    puto = 5
                });
                
            });

            _portToListen = FindAvailablePort(_portToListen, _triesToFindPort);

            IPAddress ipAddress = GetLocalIPAddress();

            //var host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (var ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        Debug.Log(ip.ToString());
            //    }
            //}

            if (_serverSocketManager.StartListener(ipAddress, _portToListen))
            {

                Debug.Log($"Server started on ip {ipAddress.ToString()}, on port {_portToListen})");

                _serverSocketManager.StartLoop();
            }
        }

        private IPAddress GetLocalIPAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork &&
                            !IPAddress.IsLoopback(ip.Address))
                        {
                            return ip.Address; // Return the first valid LAN IP
                        }
                    }
                }
            }
            throw new Exception("No network adapters with a valid IPv4 address found.");
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