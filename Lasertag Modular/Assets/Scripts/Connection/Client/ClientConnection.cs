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
        await ConnectToServer("127.0.0.1", 8080);
    }

    async Task ConnectToServer(string ip, int port)
    {
        try
        {


            client = new TcpClient();
            await client.ConnectAsync(ip, port);
            isConnected = true;

            Debug.Log("Connected to server!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection failed: {e.Message}");
        }
    }


}
