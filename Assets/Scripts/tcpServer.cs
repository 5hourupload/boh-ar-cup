using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class tcpServer : MonoBehaviour
{
    public string m_ipAddress = "127.0.0.1";
    public int m_port = 2001;
    public GameObject cup;
    private TcpListener m_tcpListener;
    private TcpClient m_tcpClient;
    private NetworkStream m_networkStream;
    private string m_message = string.Empty; 

    private void Awake()
    {
        Task.Run(() => OnProcess());
    }

    private void OnProcess()
    {
        var ipAddress = IPAddress.Parse(m_ipAddress);
        m_tcpListener = new TcpListener(ipAddress, m_port);
        m_tcpListener.Start();
        Debug.Log("waiting");

        m_tcpClient = m_tcpListener.AcceptTcpClient();
        Debug.Log("connected");

        m_networkStream = m_tcpClient.GetStream();
        while (true)
        {
            var buffer = new byte[256];
            var count = m_networkStream.Read(buffer, 0, buffer.Length);

            if (count == 0)
            {
                Debug.Log("disconnected");

                OnDestroy();

                Task.Run(() => OnProcess());
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, count);

            string[] arr = message.Split(' ');
            float[] vals = { float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2]), float.Parse(arr[3]), float.Parse(arr[4]), float.Parse(arr[5]) };
            Debug.Log("here");

            cup.transform.position = new Vector3(vals[0], vals[1], vals[2]);
            //cup.transform.position = new Vector3(0,0,0);

            m_message += message + "\n";
            Debug.Log("send success " + arr);
        }
    }

    private void OnGUI()
    {
        GUILayout.TextArea(m_message);
    }

    private void OnDestroy()
    {
        m_networkStream?.Dispose();
        m_tcpClient?.Dispose();
        m_tcpListener?.Stop();
    }
}
