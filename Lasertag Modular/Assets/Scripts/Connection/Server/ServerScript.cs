using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System;
using UnityEngine;
using WebSocketSharp.Net;

class ServerConnection : MonoBehaviour
{
    TcpListener server = null;

    private Thread acceptClientsThread;
    bool isRunningClientThread = true;

    public void Start()
    {
        Debug.Log("START");

        StartServer();

    }

    private void StartServer()
    {
        IPAddress ipAddress = GetLocalIPAddress();

        //IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
        int port = 8080;
        server = new TcpListener(ipAddress, port);
        server.Start();

        Debug.Log($"Server started on ip {ipAddress.ToString()} on port {port}...");
        
        acceptClientsThread = new Thread(LookForClients);

        acceptClientsThread.Start();
    }

    private void LookForClients() 
    {

        while (isRunningClientThread)
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected!");

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
}

