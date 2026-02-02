using NetworkShared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetworkShared.Registries {
    public class HandlerRegistry {

        private Dictionary<PacketType, Type> _packetHandlers = new Dictionary<PacketType, Type>();

        public Dictionary<PacketType, Type> PacketHandlers {
            get {
                if(_packetHandlers.Count == 0) {
                    Initialize();
                }
                return _packetHandlers;
            }
        }

        private void Initialize() {
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.DefinedTypes)
                .Where(type => !type.IsInterface && !type.IsAbstract && !type.IsGenericTypeDefinition)
                .Where(type => typeof(IPacketHandler).IsAssignableFrom(type))
                .Select(t => (type: t, attr: t.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(t => t.attr != null);

            //foreach (var handler in handlerTypes) {
            //    services.AddScoped(handler.AsType());
            //}

            foreach (var (type, attr) in handlers) {
                if (!_packetHandlers.ContainsKey(attr.PacketType)) {
                    _packetHandlers[attr.PacketType] = type;
                }
                else {
                    throw new Exception($"Multiple handers Assignged For this {attr.PacketType}");
                }
            }
        }
    }
}
