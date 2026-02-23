using NetworkShared.Packets.ServerClient;
using NetworkShared;
using NetworkShared.Attributes;

[HandlerRegister(PacketType.OnGameQuestion)]
public class OnGameQuestionHandler : IPacketHandler {
    public void HandlePacket(INetPacket packet, int connectionId) {
        Net_OnGameQuestion msgQuestion = (Net_OnGameQuestion)packet;
        GameManager.Instance.SetProblem(new Problem {
            Id = msgQuestion.Id,
            Complexity = msgQuestion.Complexity,
            Question = msgQuestion.Question,
            AnswerA = msgQuestion.AnswerA,
            AnswerB = msgQuestion.AnswerB,
            AnswerC = msgQuestion.AnswerC,
            AnswerD = msgQuestion.AnswerD,
            CorrectIndex = msgQuestion.CorrectIndex,
        });
    }

}
