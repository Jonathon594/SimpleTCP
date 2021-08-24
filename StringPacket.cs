using Jonathon594.SimpleTCP.Data;
using System;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// An abstract implementation of <see cref="Packet"/> allowing to send a single string over the network. 
    /// Override <see cref="Packet.Handle(NetworkContext, NetworkDirection)"/> in your own implementations.
    /// </summary>
    public abstract class StringPacket : Packet
    {
        /// <summary>
        /// The <see cref="string"/> to send.
        /// </summary>
        protected string s;

        /// <summary>
        /// Creates a new <see cref="StringPacket"/> with the specified <see cref="string"/> to be encoded.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to be encoded.</param>
        public StringPacket(string s)
        {
            this.s = s;
        }

        /// <summary>
        /// A constructor taking a <see cref="ByteBuffer"/> for decoding, use this in 
        /// <see cref="NetworkManager.RegisterPacketHandler{MSG}(Func{ByteBuffer, MSG})"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="ByteBuffer"/> to decode from.</param>
        public StringPacket(ByteBuffer buffer)
        {
            s = buffer.ReadString();
        }

        internal override void Encode(ByteBuffer buffer)
        {
            buffer.WriteString(s);
        }
    }
}
