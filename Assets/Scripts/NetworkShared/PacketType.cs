
using LiteNetLib.Utils;
using NetworkShared;

namespace NetworkShared {
    public enum PacketType : byte {
        #region ClientServer
        Invalid = 0,
        AuthRequest = 1,
        ServerRequestStatus = 2,
        #endregion

        #region ServerClient
        OnAuthSuccess = 100,
        OnAuthFailure = 101,
        OnServerStatus = 102,
        #endregion
    }
}

public interface INetPacket : INetSerializable {
    PacketType Type { get; }
}