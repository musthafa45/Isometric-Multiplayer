using NetworkShared.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetworkShared.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthSuccess)]
    public class OnAuthHandler : IPacketHandler {
        public void HandlePacket(INetPacket packet, int connectionId) {
            Debug.Log("OnAuthHandler Triggered");
            SceneManager.LoadScene("Lobby");
        }
    }
}
