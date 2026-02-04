using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerClient {
    public struct Net_OnServerStatus : INetPacket {
        public readonly PacketType Type => PacketType.OnServerStatus;
        public ushort OnlinePlayersCount { get; set; }
        public PlayerNetDTO[] TopPlayersNetDTOs { get; set; }


        public void Deserialize(NetDataReader reader) {
            OnlinePlayersCount = reader.GetUShort();

            ushort topPlayerLength = reader.GetUShort();

            TopPlayersNetDTOs = new PlayerNetDTO[topPlayerLength];

            for (int i = 0; i < topPlayerLength; i++) {
                TopPlayersNetDTOs[i] = reader.Get<PlayerNetDTO>();
            }
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);

            writer.Put(OnlinePlayersCount);

            writer.Put((ushort)TopPlayersNetDTOs.Length);

            foreach (PlayerNetDTO playerDto in TopPlayersNetDTOs) {
                writer.Put(playerDto);
            }
        }
    }

    public struct PlayerNetDTO : INetSerializable
    {
        public string Username { get; set; }

        public ushort Score { get; set; }

        public bool IsOnline { get; set; }

        public void Deserialize(NetDataReader reader) {
            Username = reader.GetString();
            Score = reader.GetUShort();
            IsOnline = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put(Username);
            writer.Put(Score);
            writer.Put(IsOnline);
        }
    }
}
