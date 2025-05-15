using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class VideoReceiver : MonoBehaviour
{
    public RawImage rawImage;

    private Texture2D tex;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] latestFrame;
    private object frameLock = new object();
    private bool isConnected = false;

    void Start()
    {
        tex = new Texture2D(320, 240, TextureFormat.RGB24, false); // tamaño fijo
        rawImage.texture = tex;

        Thread serverThread = new Thread(StartServer);
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void StartServer()
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8000);
            listener.Start();
            Debug.Log("Esperando conexión desde Raspberry...");
            client = listener.AcceptTcpClient();
            stream = client.GetStream();
            isConnected = true;
            Debug.Log("¡Raspberry conectada!");

            ReceiveLoop();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error en servidor: " + ex.Message);
        }
    }

    void ReceiveLoop()
    {
        try
        {
            while (true)
            {
                byte[] lengthBytes = new byte[4];
                int totalRead = 0;
                while (totalRead < 4)
                {
                    int read = stream.Read(lengthBytes, totalRead, 4 - totalRead);
                    if (read <= 0) return;
                    totalRead += read;
                }

                // Interpreta tamaño en big-endian
                int frameLength = (lengthBytes[0] << 24) | (lengthBytes[1] << 16) | (lengthBytes[2] << 8) | lengthBytes[3];

                byte[] frameBytes = new byte[frameLength];
                int bytesRead = 0;
                while (bytesRead < frameLength)
                {
                    int read = stream.Read(frameBytes, bytesRead, frameLength - bytesRead);
                    if (read <= 0) return;
                    bytesRead += read;
                }

                lock (frameLock)
                {
                    latestFrame = frameBytes;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error en recepción: " + ex.Message);
        }
    }

    void Update()
    {
        if (isConnected && latestFrame != null)
        {
            lock (frameLock)
            {
                try
                {
                    if (tex.LoadImage(latestFrame))
                    {
                        rawImage.texture = tex;
                    }
                    else
                    {
                        Debug.LogWarning("No se pudo cargar imagen.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error al cargar imagen: " + e.Message);
                }

                latestFrame = null;
            }
        }
    }
}
