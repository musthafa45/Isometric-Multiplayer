using LiteNetLib.Utils;

namespace NetworkShared.Packets.ClientServer {
    public struct Net_FindOpponentRequest : INetPacket {
        public readonly PacketType Type => PacketType.FindOpponentRequest;

        public void Deserialize(NetDataReader reader) {
            
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
        }
    }
}
