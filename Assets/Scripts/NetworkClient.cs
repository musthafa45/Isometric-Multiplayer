using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkClient : MonoBehaviour, INetEventListener {

    public static NetworkClient Instance { get; private set; }

    private NetManager netManager;
    private NetPeer serverPeer;
    private NetDataWriter netDataWriter;

    public event Action OnConnected;
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Init();
    }

    private void Init() {
        netDataWriter = new NetDataWriter();
        netManager = new NetManager(this) {
            DisconnectTimeout = 100000
        };

        netManager.Start();
    }

    public void Connect() {
        netManager.Connect("localhost", 9050, "");
    }

    private void Update() {
        netManager.PollEvents();
    }

    public void SendDataToServer<T>(T packet,DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable {
        if(serverPeer != null) {
            netDataWriter.Reset();
            packet.Serialize(netDataWriter);
            serverPeer.Send(netDataWriter, deliveryMethod);
        } else {
            Debug.LogWarning("Not connected to server.");
        }
    }

    public void OnConnectionRequest(ConnectionRequest request) {
        //throw new System.NotImplementedException();
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
        //throw new System.NotImplementedException();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency) {
        //throw new System.NotImplementedException();
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {
        var message = System.Text.Encoding.UTF8.GetString(reader.RawData).Replace("\0","");
        Debug.Log("Received from server: " + message);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
        //throw new System.NotImplementedException();
    }

    public void OnPeerConnected(NetPeer peer) {
        Debug.Log("Connected to server: " + peer.Address);
        serverPeer = peer;
        OnConnected?.Invoke();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        //throw new System.NotImplementedException();
    }

}
