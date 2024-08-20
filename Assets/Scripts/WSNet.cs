using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;

public class WSNet : MonoBehaviour
{
    public string ipAddress = "127.0.0.1";
    public int portNumber = 5000;
    public TMP_Text text_Log;

    Thread listnerThread;
    string logText;

    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        text_Log.text = logText;
    }

    public void Initialize()
    {
        // Start TcpServer background thread
        listnerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        listnerThread.IsBackground = true;
        listnerThread.Start();
    }

    void ListenForIncommingRequest()
    {
        Debug.Log("Start Server : " + ipAddress + ", " + portNumber);
        logText += $"Start Server : {ipAddress}: {portNumber}\r\n";

        UdpClient listener = new UdpClient(portNumber);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, portNumber);
        try
        {
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP);
                string response = Encoding.UTF8.GetString(bytes);
                print(response);

                logText += $"{response}+\r\n";
            }
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException " + e.ToString());
            logText += "SocketException " + e.ToString();
        }
        finally
        {
            listener.Close();
        }
    }

}
