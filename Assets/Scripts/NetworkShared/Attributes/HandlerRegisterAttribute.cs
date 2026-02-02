using System;

namespace NetworkShared.Attributes {
   
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerRegisterAttribute : Attribute {
        public PacketType PacketType { get; set; }
        public HandlerRegisterAttribute(PacketType packetType) {
            PacketType = packetType;
        }
    }
}
