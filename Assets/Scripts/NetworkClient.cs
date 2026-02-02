using LiteNetLib;
using LiteNetLib.Utils;
using NetworkShared;
using NetworkShared.Registries;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkClient : MonoBehaviour, INetEventListener {

    public static NetworkClient Instance { get; private set; }

    private NetManager netManager;
    private NetPeer serverPeer;
    private NetDataWriter netDataWriter;
    private PacketRegistry packetRegistry;
    private HandlerRegistry handlerRegistry;

    public event Action OnConnected;
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start() {
        Init();
    }

    private void Init() {
        netDataWriter = new NetDataWriter();

        packetRegistry = new PacketRegistry();
        handlerRegistry = new HandlerRegistry();

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

        // Resolve Packet
        PacketType packetType = (PacketType)reader.GetByte();
        INetPacket packet = ResolvePacket(reader, packetType);

        // Resolve Handler
        IPacketHandler handler = ResolveHandler(packetType);
        handler.HandlePacket(packet, peer.Id);

        reader.Recycle();
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

    private INetPacket ResolvePacket(NetPacketReader reader, PacketType packetType) {
        Type type = packetRegistry.PacketTypes[packetType];
        INetPacket packet = (INetPacket)Activator.CreateInstance(type);
        packet.Deserialize(reader);
        return packet;
    }

    private IPacketHandler ResolveHandler(PacketType packetType) {
        Type type = handlerRegistry.PacketHandlers[packetType];
        IPacketHandler packetHandler = (IPacketHandler)Activator.CreateInstance(type);
        return packetHandler;
    }
}
