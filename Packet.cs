using Jonathon594.SimpleTCP.Data;
using System;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// A base level implementation of a network <see cref="Packet"/>. Extend this <see cref="Type"/> to create your own packets.
    /// All <see cref="Packet"/> must contain a method for <see cref="Encode(ByteBuffer)"/> and <see cref="Handle(NetworkContext, NetworkDirection)"/>.
    /// It is recommended to create your own constructor with necessary parameters, as well as a constructor taking a <see cref="ByteBuffer"/> for decoding.
    /// </summary>
    public abstract class Packet
    {
        /// <summary>
        /// Converts all packet related data into <see cref="byte"/> and adds them to the <see cref="ByteBuffer"/> for sending across the network.
        /// It is important to <see cref="Encode(ByteBuffer)"/> the data in the same order it is expected to be decoded.
        /// </summary>
        /// <param name="buffer">The <see cref="ByteBuffer"/> to encode on.</param>
        internal abstract void Encode(ByteBuffer buffer);

        /// <summary>
        /// This method will be called when the packet is received on either the <see cref="Net"/> or <see cref="Net"/>.
        /// Use the method to specify what needs to happen when the packet is recieved on either side.
        /// </summary>
        /// <param name="context">Contains contextual information about the packet, ie. the <see cref="ServerClient"/> sender when on the server.</param>
        /// <param name="direction">The <see cref="NetworkDirection"/> the packet was sent towards.</param>
        public abstract void Handle(NetworkContext context, NetworkDirection direction);
    }
}
