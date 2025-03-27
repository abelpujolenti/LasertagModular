using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System;
using UnityEngine;
using System.Threading.Tasks;

class ServerConnection : MonoBehaviour
{
    TcpListener server = null;

    private Thread acceptClientsThread;
    bool isRunningClientThread = true;

    private void Start()
    {
        Debug.Log("START");

        StartServer();

    }

    private void StartServer()
    {
        IPAddress ipAddress = GetLocalIPAddress();
        int port = FindAvailablePort(8050, 100);
        server = new TcpListener(ipAddress, port);

        server.Start();

        Debug.Log($"Server started on ip {ipAddress.ToString()}, on port {port})");

        acceptClientsThread = new Thread(LookForClients);

        acceptClientsThread.Start();
    }

    private void LookForClients() 
    {
        Debug.Log("Looking");

        while (isRunningClientThread)
        {
            TcpClient client = server.AcceptTcpClient();
            Debug.Log("Client connected!");

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

