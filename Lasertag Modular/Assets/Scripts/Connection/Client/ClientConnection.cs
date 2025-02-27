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

        string serverIP = "10.40.1.53";
        int port = 8080;

        Debug.Log($"Attempting to connect to {serverIP}:{port}...");

        await ConnectToServer(serverIP, port);
    }

    async Task ConnectToServer(string ip, int port)
    {

        await Task.Delay(TimeSpan.FromSeconds(3));

        client = new TcpClient();
        await client.ConnectAsync(ip, port);
        isConnected = true;

        Debug.Log("Connected to server!");
    }


}
