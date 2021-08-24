using Jonathon594.SimpleTCP.Data;
using System;
using System.Collections.Generic;
using System.Net;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// Main class for handling network setup with the Net.Work library.
    /// For hosting a server see <see cref="CreateServer(int)"/>.
    /// For connecting to a server see <see cref="CreateClient(IPAddress, int)"/>
    /// Make sure to register any packet types that will be used on the network with
    /// <see cref="RegisterPacketHandler{MSG}(Func{ByteBuffer, MSG})"/>.
    /// </summary>
    public class NetworkManager
    {
        private static Dictionary<Type, PacketFactory> registeredPackets = new Dictionary<Type, PacketFactory>();

        /// <summary>
        /// Creats a <see cref="Server"/> object configured with the <see cref="IPAddress"/> and port.
        /// Use <see cref="Server.StartServer"/> to start listening.
        /// </summary>
        /// <param name="iPAddress">The IP Address to listen for.</param>
        /// <param name="port">The port to listen on.</param>
        /// <returns>A <see cref="Server"/> object.</returns>
        public static Server CreateServer(IPAddress iPAddress, int port)
        {
            Server server = new Server(iPAddress, port);
            return server;
        }

        /// <summary>
        /// Creates a <see cref="Server"/> object configured for listening for any <see cref="IPAddress"/> on the specified port.
        /// Use <see cref="Server.StartServer"/> to start listening.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        /// <returns>A <see cref="Client"/> object.</returns>
        public static Server CreateServer(int port)
        {
            return CreateServer(IPAddress.Any, port);
        }

        /// <summary>
        /// Creates a <see cref="Client"/> object configured to connect to the specified <see cref="IPAddress"/> on the given port.
        /// </summary>
        /// <param name="iPAddress">The IP address to connect to.</param>
        /// <param name="port">The port to use.</param>
        /// <returns>A <see cref="Client"/> object.</returns>
        public static Client CreateClient(IPAddress iPAddress, int port)
        {
            Client client = new Client(iPAddress, port);
            return client;
        }

        /// <summary>
        /// Register a factory method to create instances of a packet from the network.
        /// </summary>
        /// <typeparam name="MSG">A <see cref="Type"/> extending <see cref="Packet"/>.</typeparam>
        /// <param name="factory">A Function consuming a <see cref="ByteBuffer"/> returning an instance of typeparam MSG.</param>
        public static void RegisterPacketHandler<MSG>(Func<ByteBuffer, MSG> factory) where MSG : Packet
        {
            registeredPackets.Add(typeof(MSG), new PacketFactory(factory, (byte) registeredPackets.Count));
        }

        /// <summary>
        /// Returns a <see cref="Packet"/> instance decoded by the packet decoder registered under the given index.
        /// see <see cref="RegisterPacketHandler{MSG}(Func{ByteBuffer, MSG})"/>
        /// </summary>
        /// <param name="index">The byte index the packet factory is registered under.</param>
        /// <param name="buffer">the buffer to be decoded.</param>
        /// <returns>A <see cref="Packet"/> object that has been decoded.</returns>
        public static Packet DecodePacket(byte index, ByteBuffer buffer)
        {
            if (index >= registeredPackets.Count) throw new IndexOutOfRangeException();
            foreach(PacketFactory packetFactory in registeredPackets.Values)
            {
                if (packetFactory.Index == index) return packetFactory.Factory.Invoke(buffer);
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Get a packet index by the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">a <see cref="Type"/> extending <see cref="Packet"/>.</param>
        /// <returns>A <see cref="byte"/> index for the given <see cref="Packet"/> type.</returns>
        public static byte GetPacketIndex(Type type)
        {
            return registeredPackets[type].Index;
        }
    }
}
