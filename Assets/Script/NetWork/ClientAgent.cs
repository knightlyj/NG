using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class ClientAgent : MonoBehaviour {
    int reiliableChannelId;
    int unreliableChannelId;
    int localHostId = -1;
    int connectionId;
    // Use this for initialization
    void Start () {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        reiliableChannelId = config.AddChannel(QosType.Reliable);
        unreliableChannelId = config.AddChannel(QosType.Unreliable);
        HostTopology topology = new HostTopology(config, 10);

        localHostId = NetworkTransport.AddHost(topology, 6666);
        byte error;
        connectionId = NetworkTransport.Connect(localHostId, GameSetting.remoteServerIp, GameSetting.remoteServerPort, 0, out error);
    }

    public bool connected = false;
    const int bufferSize = 4096;
    byte[] recBuffer = new byte[bufferSize];
    // Update is called once per frame
    void Update () {
        int dataSize;
        int channel;
        int connection;
        byte error;
        NetworkEventType recData = NetworkTransport.ReceiveFromHost(localHostId, out connection, out channel, recBuffer, bufferSize, out dataSize, out error);
        if (error != 0)
        {
            Debug.Log("recv error is" + error);
        }
        switch (recData)
        {
            case NetworkEventType.Nothing:
                //Debug.Log("nothing");
                break;
            case NetworkEventType.ConnectEvent:
                connected = true;
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("disconnected");
                break;
            case NetworkEventType.DataEvent:
                OnRecvGameMsg(recBuffer, dataSize);
                break;
        }
    }

    void OnDestroy()
    {
        if (localHostId >= 0)
            NetworkTransport.RemoveHost(localHostId);
    }
    
    public bool SendGameMsg(GameMsg msg)
    {
        if (!connected)
            return false;
        int length;
        byte[] data = MsgPacker.Pack(msg, out length);
        byte error;
        return NetworkTransport.Send(localHostId, connectionId, unreliableChannelId, data, length, out error);
    }

    public void OnRecvGameMsg(byte[] data, int length)
    {
        object o = MsgPacker.Unpack(data, length);
        GameMsg msg = o as GameMsg;
        if (msg == null)
            return;
    }
    
}

