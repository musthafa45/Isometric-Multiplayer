using LiteNetLib.Utils;

namespace NetworkShared.Packets.ServerClient {
    public class Net_OnGameQuestion : INetPacket {
        public PacketType Type => PacketType.OnGameQuestion;

        public ushort Id;
        public Complexity Complexity;

        public string Question;
        public string AnswerA;
        public string AnswerB;
        public string AnswerC;
        public string AnswerD;

        public int CorrectIndex;

        public void Deserialize(NetDataReader reader) {
            Id = reader.GetUShort();
            Complexity = (Complexity)reader.GetInt();

            Question = reader.GetString();
            AnswerA = reader.GetString();
            AnswerB = reader.GetString();
            AnswerC = reader.GetString();
            AnswerD = reader.GetString();

            CorrectIndex = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer) {
            writer.Put((byte)Type);
            writer.Put(Id);
            writer.Put((int)Complexity);

            writer.Put(Question);
            writer.Put(AnswerA);
            writer.Put(AnswerB);
            writer.Put(AnswerC);
            writer.Put(AnswerD);

            writer.Put(CorrectIndex);
        }
    }
}
