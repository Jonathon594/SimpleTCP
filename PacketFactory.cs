using Jonathon594.SimpleTCP.Data;
using System;

namespace Jonathon594.SimpleTCP
{
    internal class PacketFactory
    {
        private readonly Func<ByteBuffer, Packet> factory;
        private readonly byte index;

        public PacketFactory(Func<ByteBuffer, Packet> factory, byte index)
        {
            this.factory = factory;
            this.index = index;
        }

        public Func<ByteBuffer, Packet> Factory => factory;

        public byte Index => index;
    }
}


