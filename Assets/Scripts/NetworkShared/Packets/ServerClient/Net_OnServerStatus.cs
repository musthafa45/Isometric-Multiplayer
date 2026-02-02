using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerClient {
    public struct Net_OnServerStatus : INetPacket {
        public readonly PacketType Type => PacketType.OnServerStatus;

        public void Deserialize(NetDataReader reader) {
            
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
        }
    }
}
