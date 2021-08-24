using Jonathon594.SimpleTCP.Data;
using System.Net;
using System.Net.Sockets;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// A class representing a lower level conntion to a <see cref="TcpClient"/>.
    /// Can be used as an identifier for connected clients. See <see cref="Server.SendPacket(Packet, ServerClient)"/>
    /// </summary>
    public class ServerClient
    {
        private TcpClient client;

        internal ServerClient(TcpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Checks to see whether the client is actually connected to a remote host.
        /// </summary>
        /// <returns>True when connected. False when not.</returns>
        public bool isConnected()
        {
            return client.Connected;
        }

        /// <summary>
        /// Gets the <see cref="EndPoint"/> of the connected <see cref="ServerClient"/>.
        /// </summary>
        /// <returns>The <see cref="EndPoint"/> of the connected <see cref="ServerClient"/>.</returns>
        public EndPoint GetIPEndPoint()
        {
            return client.Client.RemoteEndPoint;
        }

        internal ByteBuffer readBuffer(int size)
        {
            NetworkStream stream = client.GetStream();
            byte[] bytes = new byte[size];
            int length = stream.Read(bytes, 0, bytes.Length);
            return new ByteBuffer(bytes, length);
        }

        internal void SendPacket(Packet packet)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteByte(NetworkManager.GetPacketIndex(packet.GetType()));
            packet.Encode(buffer);
            client.GetStream().Write(buffer.GetBytes(), 0, buffer.GetByteCount());
        }
    }
}