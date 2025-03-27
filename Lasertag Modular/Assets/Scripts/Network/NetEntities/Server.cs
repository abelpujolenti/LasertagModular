using System.Net;
using System.Net.Sockets;
using System.Threading;
using Network.Sockets;
using UnityEngine;

namespace Network.NetEntities
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private int _portToListen;
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
            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.Log(ip.ToString());
                }
            }

            if (_serverSocketManager.StartListener(_portToListen))
            {
                _serverSocketManager.StartLoop();
            }
        }
    }
}