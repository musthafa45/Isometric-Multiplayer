using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using System;

namespace NetworkShared.PacketHandlers
{
    [HandlerRegister(PacketType.OnAuthFailure)]
    public class OnAuthFailure : IPacketHandler {

        public static event Action<Net_OnAuthFailure> OnAuthFailureEvent;

        public void HandlePacket(INetPacket packet, int connectionId) {
            INetPacket msg = (Net_OnAuthFailure)packet;
            OnAuthFailureEvent?.Invoke((Net_OnAuthFailure)msg);

        }
    }
}
