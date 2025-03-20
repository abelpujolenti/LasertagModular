using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//typedefs
using OnSocketConnected = System.Action<Network.Sockets.TcpSocket>;
using OnReceivePacket = System.Action<byte[]>;
using OnSocketDisconnect = System.Action<Network.Sockets.TcpSocket>;

namespace Network.Sockets
{
    public class SocketManager
    {
        private MyTcpListener _listener;
        
        private Thread _listenerThread;

        private Mutex _listenerMutex = new Mutex();
        private Mutex _socketsMutex = new Mutex();

        private OnSocketConnected _onSocketConnected;

        private List<TcpSocket> _sockets = new List<TcpSocket>();

        private bool _isRunning;

        public SocketManager(OnSocketConnected onSocketConnected)
        {
            _onSocketConnected = onSocketConnected;
        }

        ~SocketManager()
        {
        }

        public void StartLoop()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;
            
            Thread loopThread = new Thread(SelectorLoop);
            loopThread.Start();
        }

        public bool StartListener(IPEndPoint ipEndPoint)
        {
            _listenerMutex.WaitOne();

            if (_listener != null)
            {
                _listenerMutex.ReleaseMutex();
                return false;
            }

            _listener = new MyTcpListener(ipEndPoint);
            
            _listener.Start();

            if (!_listener.Active())
            {
                _listener = null;
                _listenerMutex.ReleaseMutex();
                return false;
            }

            _listenerMutex.ReleaseMutex();
            return true;
        }

        public void StopListener()
        {
            _listener.Stop();
            _listener = null;
        }

        public bool ConnectToNetEntity(IPEndPoint ipEndPoint)
        {
            TcpSocket newSocket = new TcpSocket();

            if (!newSocket.Connect(ipEndPoint))
            {
                return false;
            }
            
            AddSocket(newSocket);

            return true;
        }

        private void SelectorLoop()
        {
            while (_isRunning)
            {
                CheckIncomingConnections();
                CheckIncomingPackets();
            }
        }

        private void CheckIncomingConnections()
        {
            _listenerMutex.WaitOne();
            
            if (_listener != null && _listener.Pending())
            {
                AddSocket(new TcpSocket(_listener.AcceptSocket()));
            }
            
            _listenerMutex.ReleaseMutex();
        }

        private void CheckIncomingPackets()
        {
            _socketsMutex.WaitOne();

            foreach (TcpSocket socket in _sockets)
            {
                if (socket.HasDataToRead())
                {
                    socket.ReceivePacket();
                }
            }
            
            _socketsMutex.ReleaseMutex();
        }

        private void AddSocket(TcpSocket socket)
        {
            _socketsMutex.WaitOne();
            
            _sockets.Add(socket);
            _onSocketConnected(socket);
            
            socket.SubscribeOnDisconnect(RemoveSocketAsync);
            
            _socketsMutex.ReleaseMutex();
        }

        private void RemoveSocket(TcpSocket socket)
        {
            _socketsMutex.WaitOne();

            _sockets.Remove(socket);
            
            _socketsMutex.ReleaseMutex();
        }

        public void RemoveSocketAsync(TcpSocket socket)
        {
            Thread removeSocketThread = new Thread(() =>
            {
                RemoveSocket(socket);
            });
            
            removeSocketThread.Start();
        }
    }
}
