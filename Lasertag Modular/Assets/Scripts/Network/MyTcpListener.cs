using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace Network
{
    public class MyTcpListener : TcpListener
    {
        public MyTcpListener(int port) : base(port)
        {
        }

        public MyTcpListener([NotNull] IPAddress localaddr, int port) : base(localaddr, port)
        {
        }

        public MyTcpListener([NotNull] IPEndPoint localEP) : base(localEP)
        {
        }

        public new bool Active()
        {
            return base.Active;
        }
    }
}