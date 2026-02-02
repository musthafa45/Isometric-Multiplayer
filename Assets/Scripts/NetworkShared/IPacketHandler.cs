namespace NetworkShared {
    public interface IPacketHandler {
        void HandlePacket(INetPacket packet, int connectionId);
    }
}
