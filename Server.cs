using Jonathon594.SimpleTCP.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// A class managing lower level implementations of TCP protocol. Use this object to send <see cref="Packet"/> to connected clients.
    /// <see cref="SendPacket(Packet, ServerClient)"/>.
    /// <see cref="SendToAll(Packet)"/>.
    /// Automatically starts and manages listenening threads.
    /// See <see cref="NetworkManager.CreateServer(int)"/> for creating an instance of <see cref="Server"/>.
    /// </summary>
    public class Server
    {
        private TcpListener tcpListener;
        private Dictionary<EndPoint, ServerClient> clientList = new Dictionary<EndPoint, ServerClient>();
        private readonly object _lock = new object();
        private Thread acceptClientsThread;
        private bool connected = false;

        /// <summary>
        /// Called when the server begins successfully listening for incoming data.
        /// </summary>
        public event EventHandler<EventArgs> ServerConnected;

        /// <summary>
        /// Called when the server stops listening for incoming data.
        /// </summary>
        public event EventHandler<EventArgs> ServerDisconnected;

        /// <summary>
        /// Called when a client disconnects from the server.
        /// </summary>
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        /// <summary>
        /// Called when a client connects to the server.
        /// </summary>
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;

        internal Server(IPAddress iPAddress, int port)
        {
            tcpListener = new TcpListener(iPAddress, port);
        }

        /// <summary>
        /// Starts listening on the configured <see cref="IPAddress"/> and port then begins listening for client connections.
        /// </summary>
        public void StartServer()
        {
            tcpListener.Start();
            connected = true;
            acceptClientsThread = new Thread(() => AcceptClients()); acceptClientsThread.Start();

            OnServerConnected();
        }

        /// <summary>
        /// Stops the server and terminates the listening thread.
        /// </summary>
        public void StopServer()
        {
            connected = false;
            tcpListener.Stop();
            acceptClientsThread.Join();

            OnServerDisconnected();
        }

        internal void AcceptClients()
        {
            while (connected)
            {
                try
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(tcpClient);
                    lock (_lock) clientList.Add(tcpClient.Client.RemoteEndPoint, serverClient);
                    Thread thread = new Thread(() => ReceiveData(serverClient)); thread.Start();

                    OnClientConnected(serverClient);
                } catch(SocketException)
                {
                    break;
                }
            }
        }

        internal void ReceiveData(ServerClient serverClient)
        {
            while (true)
            {
                if(!serverClient.isConnected()) break;
                if (!connected) break;

                ByteBuffer buffer = serverClient.readBuffer(8192);
                if (buffer.GetByteCount() == 0) break;
                byte packetIndex = buffer.ReadByte();
                Packet packet = NetworkManager.DecodePacket(packetIndex, buffer);
                NetworkContext networkContext = new NetworkContext(serverClient, this);
                packet.Handle(networkContext, NetworkDirection.Server);
            }
            clientList.Remove(serverClient.GetIPEndPoint());

            OnClientDisconnected(serverClient);
        }

        /// <summary>
        /// Sends a <see cref="Packet"/> over the network to the connected <see cref="ServerClient"/>.
        /// </summary>
        /// <param name="packet">the <see cref="Packet"/> to send.</param>
        /// <param name="client">the <see cref="ServerClient"/> to send to.</param>
        public void SendPacket(Packet packet, ServerClient client)
        {
            client.SendPacket(packet);
        }

        /// <summary>
        /// Sends a <see cref="Packet"/> over the network to all connected <see cref="ServerClient"/>.
        /// </summary>
        /// <param name="packet">The <see cref="Packet"/> to send.</param>
        public void SendToAll(Packet packet)
        {
            foreach (ServerClient serverClient in clientList.Values)
            {
                serverClient.SendPacket(packet);
            }
        }

        /// <summary>
        /// Sends a <see cref="Packet"/> over the network to all connected <see cref="ServerClient"/>.
        /// Excludes sending the packet to a specific <see cref="ServerClient"/>.
        /// </summary>
        /// <param name="packet">The <see cref="Packet"/> to send.</param>
        /// <param name="excluded">The <see cref="ServerClient"/> to exclude.</param>
        public void SendToAllBut(Packet packet, ServerClient excluded)
        {
            foreach (ServerClient serverClient in clientList.Values)
            {
                if (serverClient == excluded) continue;
                serverClient.SendPacket(packet);
            }
        }

        /// <summary>
        /// Raises the <see cref="ServerConnected"/> event.
        /// </summary>
        protected virtual void OnServerConnected()
        {
            EventHandler<EventArgs> handler = ServerConnected; handler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Raises the <see cref="ServerDisconnected"/> event.
        /// </summary>
        protected virtual void OnServerDisconnected()
        {
            EventHandler<EventArgs> handler = ServerDisconnected; handler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Raises the <see cref="ClientDisconnected"/> event.
        /// </summary>
        protected virtual void OnClientDisconnected(ServerClient client)
        {
            EventHandler<ClientDisconnectedEventArgs> handler = ClientDisconnected; handler?.Invoke(this, new ClientDisconnectedEventArgs(client));
        }

        /// <summary>
        /// Raises the <see cref="ClientConnected"/> event.
        /// </summary>
        protected virtual void OnClientConnected(ServerClient client)
        {
            EventHandler<ClientConnectedEventArgs> handler = ClientConnected; handler?.Invoke(this, new ClientConnectedEventArgs(client));
        }
    }
}
