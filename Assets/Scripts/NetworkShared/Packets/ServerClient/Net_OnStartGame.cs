using LiteNetLib.Utils;
using NetworkShared;
using System;

namespace Isometric_Game_Server.NetworkShared.Packets.ServerClient {
    public struct Net_OnStartGame : INetPacket {
        public readonly PacketType Type => PacketType.OnStartGame;

        public string[] Players { get; set; }

        public Guid GameId { get; set; }


        public void Deserialize(NetDataReader reader) {
            Players = new string[reader.GetInt()];
            for(int i = 0; i < Players.Length; i++) {
                Players[i] = reader.GetString();
            }
            GameId = Guid.Parse(reader.GetString());
        }

        public void Serialize(NetDataWriter writer) {
             writer.Put((byte)Type);
             writer.Put(Players.Length);
             foreach(var player in Players) {
                 writer.Put(player);
             }
             writer.Put(GameId.ToString());
        }
    }
}
