using LiteNetLib.Utils;
using NetworkShared;

namespace NetworkShared.Packets.ClientServer {
    public struct Net_CancelFindOpponentRequest : INetPacket {
        public readonly PacketType Type => PacketType.CancelFindOpponentRequest;

        public void Deserialize(NetDataReader reader) {

        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
        }
    }
}
