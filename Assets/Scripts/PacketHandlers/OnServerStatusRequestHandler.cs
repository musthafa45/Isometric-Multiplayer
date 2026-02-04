using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using System;
using UnityEngine.SceneManagement;

namespace PacketHandlers
{
    [HandlerRegister(PacketType.OnServerStatus)]
    public class OnServerStatusRequestHandler : IPacketHandler {
        public static event Action<Net_OnServerStatus> OnServerStatusResponseEvent;

        public void HandlePacket(INetPacket packet, int connectionId) {
            if(SceneManager.GetActiveScene().name == "Lobby") {
                Net_OnServerStatus packetToSend = (Net_OnServerStatus)packet;
                OnServerStatusResponseEvent?.Invoke(packetToSend);
            }
            
        }
    }
}
