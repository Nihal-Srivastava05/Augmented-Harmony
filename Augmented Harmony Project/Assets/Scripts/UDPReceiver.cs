using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using TMPro;

public class UDPReceiver : MonoBehaviour
{   
    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public bool startReceiving = true;
    public bool printToConsole = false;
    public string data;
    public string url="127.0.0.1";
    public GameObject inputField;

    public void setUrl() {
        url = inputField.GetComponent<TMP_InputField>().text;
    }

    // Start is called before the first frame update
    void Start()
    {
        // setUrl();
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while(startReceiving)
        {
            try 
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                // IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse(url), 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if(printToConsole) {print(data);}
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        client.Close();
    }
}
