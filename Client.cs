using Jonathon594.SimpleTCP.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// A class managing lower level connections to a server. Automatically starts and manages receiving threads. Used for sending packets to the connected server.
    /// See <see cref="SendPacket(Packet)"/>.
    /// To create a new instance see <see cref="NetworkManager.CreateClient(IPAddress, int)"/>.
    /// </summary>
    public class Client
    {
        private TcpClient client;
        private readonly IPAddress iPAddress;
        private readonly int port;

        /// <summary>
        /// Called when the client successfully connects to a server.
        /// </summary>
        public event EventHandler<EventArgs> ClientConnected;

        /// <summary>
        /// Called when the client disconnects from a server.
        /// </summary>
        public event EventHandler<EventArgs> ClientDisconnected;

        /// <summary>
        /// Called when the client looses connection to the server.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionLost;

        internal Client(IPAddress iPAddress, int port)
        {
            client = new TcpClient();
            this.iPAddress = iPAddress;
            this.port = port;
        }

        /// <summary>
        /// Attempts to connect to the specified <see cref="IPAddress"/> and port and starts listening for data.
        /// </summary>
        public void Connect()
        {
            client.Connect(iPAddress, port);
            Thread thread = new Thread(() => ReceiveData()); thread.Start();
            
            OnClientConnected();
        }

        /// <summary>
        /// Stops the connection to the remote host.
        /// </summary>
        public void Disconnect()
        {
            client.Dispose();

            OnClientDisconnected();
        }

        private void ReceiveData()
        {
            while (true)
            {
                try
                {
                    ByteBuffer buffer = ReadBuffer(8192);
                    if (buffer.GetByteCount() == 0) break;
                    byte packetIndex = buffer.ReadByte();
                    Packet packet = NetworkManager.DecodePacket(packetIndex, buffer);
                    packet.Handle(new NetworkContext(), NetworkDirection.Client);
                } catch (IOException)
                {
                    break;
                }
            }

            OnConnectionLost();
            client.Dispose();
        }

        internal ByteBuffer ReadBuffer(int size)
        {
            NetworkStream stream = client.GetStream();
            byte[] bytes = new byte[size];
            int length = stream.Read(bytes, 0, bytes.Length);
            return new ByteBuffer(bytes, length);
        }

        /// <summary>
        /// Sends a <see cref="Packet"/> to the connected <see cref="Net"/>.
        /// </summary>
        /// <param name="packet">The <see cref="Packet"/> to send.</param>
        public void SendPacket(Packet packet)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteByte(NetworkManager.GetPacketIndex(packet.GetType()));
            packet.Encode(buffer);
            client.GetStream().Write(buffer.GetBytes(), 0, buffer.GetByteCount());
        }

        /// <summary>
        /// Raises the <see cref="ClientConnected"/> event.
        /// </summary>
        protected virtual void OnClientConnected()
        {
            EventHandler<EventArgs> eventHandler = ClientConnected; eventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Raises the <see cref="ClientDisconnected"/> event.
        /// </summary>
        protected virtual void OnClientDisconnected()
        {
            EventHandler<EventArgs> eventHandler = ClientDisconnected; eventHandler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Raises the <see cref="ConnectionLost"/> event.
        /// </summary>
        protected virtual void OnConnectionLost()
        {
            EventHandler<EventArgs> eventHandler = ConnectionLost; eventHandler?.Invoke(this, new EventArgs());
        }
    }
}