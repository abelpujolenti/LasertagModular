using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClientConnection : MonoBehaviour
{
    private TcpClient client;
    private bool isConnected = false;

    async void Start()
    {

        string serverIP = "10.40.2.189";
        int port = 8079;

        Debug.Log($"Attempting to connect to {serverIP}:{port}...");

        await ConnectToServer(serverIP, port);
    }

    async Task ConnectToServer(string ip, int port)
    {
        client = new TcpClient();
        try
        {
            await client.ConnectAsync(ip, port);
            isConnected = true;

            Debug.Log("Connected to server!");
        }
        catch (Exception ex)
        {
            Debug.Log($"Failed to connect on port {port} {ex}");

            await ConnectToServer(ip, port + 1);
        }
    }


}
