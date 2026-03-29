using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class GestureReceiver : MonoBehaviour
{
    UdpClient client;
    Thread receiveThread;

    public string gesture = "NONE";

    void Start()
    {
        client = new UdpClient(5052);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData()
    {
        while (true)
        {
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.Receive(ref anyIP);
            gesture = Encoding.UTF8.GetString(data);
        }
    }

    void Update()
    {
        Debug.Log("Gesture: " + gesture);
    }
}