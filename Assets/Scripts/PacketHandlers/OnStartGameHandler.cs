using Isometric_Game_Server.NetworkShared.Packets.ServerClient;
using NetworkShared;
using NetworkShared.Attributes;
using UnityEngine.SceneManagement;

[HandlerRegister(PacketType.OnStartGame)]
public class OnStartGameHandler : IPacketHandler {

    public void HandlePacket(INetPacket packet, int connectionId) {
        Net_OnStartGame msg = (Net_OnStartGame)packet;
        GameManager.Instance.RegisterGame(msg.GameId, msg.Players);
        SceneManager.LoadScene("Game");
    }
}
