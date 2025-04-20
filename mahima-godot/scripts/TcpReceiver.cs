using Godot;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public partial class TcpReceiver : Node
{
    private TcpListener server;
    private TcpClient client;
    private NetworkStream stream;
    private Thread listenerThread;
    private bool isRunning = false;

    [Signal] public delegate void CameraMotionReceivedEventHandler(float dx, float dy);
    [Signal] public delegate void FingerDataReceivedEventHandler(int count);

    public override void _Ready()
    {
        StartServer(65432);
    }

    public override void _ExitTree()
    {
        StopServer();
    }

    public void StartServer(int port)
    {
        isRunning = true;
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        GD.Print($"Server listening on port {port}");

        listenerThread = new Thread(() =>
        {
            // Block until a client connects
            client = server.AcceptTcpClient();
            stream = client.GetStream();
            GD.Print("Client connected");

            byte[] buf = new byte[1024];
            while (isRunning)
            {
                try
                {
                    if (stream.DataAvailable)
                    {
                        int len = stream.Read(buf, 0, buf.Length);
                        if (len <= 0) break;

                        string data = Encoding.UTF8.GetString(buf, 0, len).Trim();
                        var parts = data.Split(',');
                        if (parts.Length == 2
                            && float.TryParse(parts[0], out float dx)
                            && float.TryParse(parts[1], out float dy))
                        {
                            EmitSignal(nameof(CameraMotionReceived), dx, dy);
                            }
                        else if (int.TryParse(data, out int f))
                        {
                            EmitSignal(nameof(FingerDataReceived),    f);                        
                        }
                        else
                        {
                            GD.PrintErr("Bad data:", data);
                        }
                    }
                }
                catch (Exception e)
                {
                    GD.PrintErr("Stream error:", e.Message);
                    break;
                }
                Thread.Sleep(5);
            }

            stream?.Close();
            client?.Close();
        });
        listenerThread.Start();
    }

    public void StopServer()
    {
        isRunning = false;
        stream?.Close();
        client?.Close();
        server?.Stop();
        listenerThread?.Join();
    }
}
