using LiteNetLib.Utils;

namespace NetworkShared.Packets.ClientServer
{
    public struct Net_ServerRequestStatus : INetPacket {
        public readonly PacketType Type => PacketType.ServerRequestStatus;

        public void Deserialize(NetDataReader reader) {
            
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
        }
    }
}
