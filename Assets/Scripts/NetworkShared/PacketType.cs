
using LiteNetLib.Utils;
using NetworkShared;

namespace NetworkShared {
    public enum PacketType : byte {
        #region ClientServer
        Invalid = 0,
        AuthRequest = 1,
        ServerRequestStatus = 2,
        FindOpponentRequest = 3,
        CancelFindOpponentRequest = 4,
        #endregion

        #region ServerClient
        OnAuthSuccess = 100,
        OnAuthFailure = 101,
        OnServerStatus = 102,
        OnFindOpponentRequest = 103,
        #endregion
    }
}

public interface INetPacket : INetSerializable {
    PacketType Type { get; }
}