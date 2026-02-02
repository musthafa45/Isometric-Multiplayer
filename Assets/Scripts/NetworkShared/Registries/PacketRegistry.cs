using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkShared.Registries {
    public class PacketRegistry {
        private Dictionary<PacketType,Type> _packetTypes = new Dictionary<PacketType, Type>();

        public Dictionary<PacketType, Type> PacketTypes {
            get { 

                if(_packetTypes.Count == 0) {
                    Initialize();
                }
                return _packetTypes;
            } 
        }

        private void Initialize() {
            Type packetTypes = typeof(INetPacket);

            IEnumerable<Type> packets = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(p => packetTypes.IsAssignableFrom(p) && !p.IsInterface);

            foreach (Type packet in packets) {
                INetPacket packetInstance = (INetPacket)Activator.CreateInstance(packet);
                _packetTypes.Add(packetInstance.Type,packet);
            }
        }
    }
}
