using LiteNetLib.Utils;


namespace NetworkShared.Packets.ServerClient {
    public struct Net_OnAuthFailure : INetPacket {
        public PacketType Type => PacketType.OnAuthFailure;

        public void Deserialize(NetDataReader reader) {
           
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
        }
    }
}
