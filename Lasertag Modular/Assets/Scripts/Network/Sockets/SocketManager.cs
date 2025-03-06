using System;
using System.Net.Sockets;
using System.Threading;

namespace Network.Sockets
{
    public class SocketManager
    {
        private TcpListener _listener;
        
        private Thread _listenerThread;
        
        private Mutex _connectionMutex;

        private Mutex _socketsMutex;

        public SocketManager(Action<>)
        {
        }

        ~SocketManager()
        {
            
        }
    }
}
