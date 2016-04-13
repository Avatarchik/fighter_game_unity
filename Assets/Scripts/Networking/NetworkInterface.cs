using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Timers;
using System;
using System.Net;

public class NetworkInterface : MonoBehaviour{

    public string ip;
    public int port;
    public int maxReconnectTry = 3;
    public const int UNABLE_TO_CONNECT = 1;
    public const int CONNECTION_SUCESSFULL = 2;
    public const int ALLREDY_CONNECTED = 3;
    
    private Reciver reciver;
    private TcpClient mySocket;
    public int connectionStatus;
    private NetworkStream theStream;
    private StreamWriter theWriter;
    private StreamReader theReader;
    private bool socketRedy = false;
    private int reconnectTry = 0;

    // Use this for initialization
    void Start () {
        reciver = GameObject.FindGameObjectWithTag("GameController").GetComponent<Reciver>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (socketRedy) {
            Recive();
        } else if(connectionStatus != UNABLE_TO_CONNECT){
            connectionStatus = UNABLE_TO_CONNECT;
        }
	}
    
    public int Init() {
        if (!socketRedy) {
            try {
                
                mySocket = new TcpClient(ip, port);
                theStream = mySocket.GetStream();
                theWriter = new StreamWriter(theStream);
                theReader = new StreamReader(theStream);
                mySocket.ReceiveTimeout = 1;
                socketRedy = true;
                connectionStatus = CONNECTION_SUCESSFULL;
                reciver.ConnectionResult(CONNECTION_SUCESSFULL);
                reconnectTry = 0;
                return CONNECTION_SUCESSFULL;
            }catch(Exception ex) {
                connectionStatus = UNABLE_TO_CONNECT;
                reciver.ConnectionResult(UNABLE_TO_CONNECT);
                reconnectTry++;
                return UNABLE_TO_CONNECT;
            }
        } else {
            connectionStatus = ALLREDY_CONNECTED;
            reciver.ConnectionResult(ALLREDY_CONNECTED);
            return ALLREDY_CONNECTED;
        }
    }  

    public void Disconnect() {
        if (!socketRedy) {
            connectionStatus = UNABLE_TO_CONNECT;
            return;
        }
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        mySocket = null;
    }

    public void Send(string message) {
        try {
            if (!socketRedy) {
                connectionStatus = UNABLE_TO_CONNECT;
                return;
            }
            Debug.Log("SENDED: " + message);
            String foo = message + "\r\n";
            theWriter.Write(foo);
            theWriter.Flush();
        }catch (Exception e) {
            //Server closed the socket!
            socketRedy = false;
            connectionStatus = UNABLE_TO_CONNECT;
            if(reconnectTry < maxReconnectTry) {
                Init();
            }
            Debug.Log(e.Message);
        }
    }
    //Timeot, flush, readToEnd
    public void Recive() {
        if (socketRedy) {
            try {
                if (theStream.CanRead) {
                    string message = "";
                    message += (char)theReader.Read();
                    message += theReader.ReadLine();
                    Debug.Log("RECV: " + message);
                    string[] data = message.Split(';');
                    if (int.Parse(data[0]).Equals(DefaultMessages.PING_REQUEST)) {
                        Send(DefaultMessages.PING_RESPONSE + "");
                    } else {
                        reciver.OnRecive(data);
                    }
                }
            } catch (IOException ex) {
                //Recv time out.. it's okey
            }catch (Exception e) {
                //Recv exception unable to send to server
                socketRedy = false;
                connectionStatus = UNABLE_TO_CONNECT;
                if (reconnectTry < maxReconnectTry) {
                    Init();
                }
                Debug.Log(e.Message);
            }
        } else {
            //Debug.Log("Socket not redy!");
            connectionStatus = UNABLE_TO_CONNECT;
            return;
        }
    }
}
