using LiteNetLib.Utils;
using NetworkShared;

namespace Packets.ClientServer {
    public struct Net_FindOpponentRequest : INetPacket {
        public readonly PacketType Type => PacketType.FindOpponentRequest;
        public ushort PlayersCount { get; set; }
        public void Deserialize(NetDataReader reader) {
            PlayersCount = reader.GetUShort();
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
            writer.Put(PlayersCount);
        }
    }
}
